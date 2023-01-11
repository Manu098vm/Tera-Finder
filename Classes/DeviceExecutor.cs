using PKHeX.Core;
using SysBot.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder
{
    public class DeviceState : BotState<RoutineType, SwitchConnectionConfig>
    {
        public override void IterateNextRoutine() => CurrentRoutineType = NextRoutineType;
        public override void Initialize() => Resume();
        public override void Pause() => NextRoutineType = RoutineType.None;
        public override void Resume() => NextRoutineType = InitialRoutine;
    }

    public class DeviceExecutor : SwitchRoutineExecutor<DeviceState>
    {
        private const string ScarletID = "0100A3D008C5C000";
        private const string VioletID = "01008F6008C5E000";

        public DeviceExecutor(DeviceState cfg) : base(cfg) { }

        public override string GetSummary()
        {
            var current = Config.CurrentRoutineType;
            var initial = Config.InitialRoutine;
            if (current == initial)
                return $"{Connection.Label} - {initial}";
            return $"{Connection.Label} - {initial} ({current})";
        }

        public override void SoftStop() => Config.Pause();
        public override Task HardStop() => Task.CompletedTask;

        public override Task MainLoop(CancellationToken token)
        {
            Config.IterateNextRoutine();
            return Task.CompletedTask;
        }

        public async Task Connect(CancellationToken token)
        {
            Connection.Connect();
            Log("Initializing connection with console...");
            await InitialStartup(token).ConfigureAwait(false);
        }

        public void Disconnect() => Connection.Disconnect();

        public async Task<GameVersion> ReadGameVersion(CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection.");

            var title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            if (title.Equals(VioletID))
                return GameVersion.VL;
            else if (title.Equals(ScarletID))
                return GameVersion.SL;
            else throw new ArgumentOutOfRangeException("Invalid Title ID.");
        }

        public async Task<GameProgress> ReadGameProgress(CancellationToken token)
        {
            var Unlocked6Stars = (await ReadEncryptedBlock(Blocks.KUnlockedRaidDifficulty6, 1, token).ConfigureAwait(false))[0] == 2;

            if (Unlocked6Stars)
                return GameProgress.Unlocked6Stars;

            var Unlocked5Stars = (await ReadEncryptedBlock(Blocks.KUnlockedRaidDifficulty5, 1, token).ConfigureAwait(false))[0] == 2;

            if (Unlocked5Stars)
                return GameProgress.Unlocked5Stars;

            var Unlocked4Stars = (await ReadEncryptedBlock(Blocks.KUnlockedRaidDifficulty4, 1, token).ConfigureAwait(false))[0] == 2;

            if (Unlocked4Stars)
                return GameProgress.Unlocked4Stars;

            var Unlocked3Stars = (await ReadEncryptedBlock(Blocks.KUnlockedRaidDifficulty3, 1, token).ConfigureAwait(false))[0] == 2;

            if (Unlocked3Stars)
                return GameProgress.Unlocked3Stars;

            return GameProgress.UnlockedTeraRaids;
        }

        public async Task<byte[]> ReadDecryptedBlock(DataBlock block, int size, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection.");

            var data = await SwitchConnection.PointerPeek(size, block.Pointer!, token).ConfigureAwait(false);
            return data;
        }

        public async Task WriteDecryptedBlock(byte[] data, DataBlock block, int size, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection.");

            await SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);
        }

        //Thanks to Lincoln-LM (original scblock code) and Architdate (ported C# reference code)!!
        //https://github.com/Lincoln-LM/sv-live-map/blob/e0f4a30c72ef81f1dc175dae74e2fd3d63b0e881/sv_live_map_core/nxreader/raid_reader.py#L168
        //https://github.com/LegoFigure11/RaidCrawler/blob/2e1832ae89e5ac39dcc25ccf2ae911ef0f634580/MainWindow.cs#L199

        public async Task<byte[]?> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection.");

            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            var size = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5 + (int)size, token);
            return BlockUtil.DecryptBlock(block.Key, data)[5..];
        }

        public async Task<byte[]> ReadEncryptedBlock(DataBlock block, int size, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection.");

            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, size, token).ConfigureAwait(false);
            return BlockUtil.DecryptBlock(block.Key, data);
        }

        private async Task<ulong> GetBlockAddress(DataBlock block, CancellationToken token)
        {
            var read_key = ReadUInt32LittleEndian(await SwitchConnection.PointerPeek(4, block.Pointer!, token).ConfigureAwait(false));
            if (read_key == block.Key)
                return await SwitchConnection.PointerAll(PreparePointer(block.Pointer!), token).ConfigureAwait(false);
            var direction = block.Key > read_key ? 1 : -1;
            var base_offset = block.Pointer![block.Pointer.Count - 1];
            for (var offset = base_offset; offset < base_offset + 0x1000 && offset > base_offset - 0x1000; offset += direction * 0x20)
            {
                var pointer = block.Pointer!.ToArray();
                pointer[^1] = offset;
                read_key = ReadUInt32LittleEndian(await SwitchConnection.PointerPeek(4, pointer, token).ConfigureAwait(false));
                if (read_key == block.Key)
                    return await SwitchConnection.PointerAll(PreparePointer(pointer), token).ConfigureAwait(false);
            }
            throw new ArgumentOutOfRangeException("Save block not found in range +- 0x1000");
        }

        private static IEnumerable<long> PreparePointer(IEnumerable<long> pointer)
        {
            var count = pointer.Count();
            var p = new long[count+1];
            for (var i = 0; i < pointer.Count(); i++)
                p[i] = pointer.ElementAt(i);
            p[count-1] += 8;
            p[count] = 0x0;
            return p;
        }
    }
}