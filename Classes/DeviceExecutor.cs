using PKHeX.Core;
using SysBot.Base;
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

        public override async Task MainLoop(CancellationToken token)
        {
            var version = await ReadGameVersion(token).ConfigureAwait(false);
            Log($"Valid Title ID ({(version is GameVersion.SL ? $"{ScarletID}" : $"{VioletID}")})");
            Log("Connection Test OK");
            Config.IterateNextRoutine();
        }

        public async Task Connect(CancellationToken token)
        {
            Connection.Connect();
            Log("Initializing connection with console...");
            await InitialStartup(token).ConfigureAwait(false);
        }

        public void Disconnect()
        {
            HardStop();
            Connection.Disconnect();
        }

        public async Task<GameVersion> ReadGameVersion(CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            var title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
            if (title.Equals(VioletID))
                return GameVersion.VL;
            else if (title.Equals(ScarletID))
                return GameVersion.SL;
            else throw new ArgumentOutOfRangeException($"Invalid Title ID ({title})");
        }

        public async Task<GameProgress> ReadGameProgress(CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            var Unlocked6Stars = await ReadEncryptedBlockBool(Blocks.KUnlockedRaidDifficulty6, token).ConfigureAwait(false);

            if (Unlocked6Stars)
                return GameProgress.Unlocked6Stars;

            var Unlocked5Stars = await ReadEncryptedBlockBool(Blocks.KUnlockedRaidDifficulty5, token).ConfigureAwait(false);

            if (Unlocked5Stars)
                return GameProgress.Unlocked5Stars;

            var Unlocked4Stars = await ReadEncryptedBlockBool(Blocks.KUnlockedRaidDifficulty4, token).ConfigureAwait(false);

            if (Unlocked4Stars)
                return GameProgress.Unlocked4Stars;

            var Unlocked3Stars = await ReadEncryptedBlockBool(Blocks.KUnlockedRaidDifficulty3, token).ConfigureAwait(false);

            if (Unlocked3Stars)
                return GameProgress.Unlocked3Stars;

            return GameProgress.UnlockedTeraRaids;
        }


        //Thanks santacrab2 & Zyro670 for the help with the following code
        public async Task<object?> ReadBlock(DataBlock block, CancellationToken token)
        {
            if (block.IsEncrypted)
            {
                return block.Type switch
                {
                    SCTypeCode.Object => await ReadEncryptedBlockObject(block, token).ConfigureAwait(false),
                    SCTypeCode.Array => await ReadEncryptedBlockArray(block, token).ConfigureAwait(false),
                    SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await ReadEncryptedBlockBool(block, token).ConfigureAwait(false),
                    SCTypeCode.Byte or SCTypeCode.SByte => await ReadEncryptedBlockByte(block, token).ConfigureAwait(false),
                    SCTypeCode.UInt32 => await ReadEncryptedBlockUint(block, token).ConfigureAwait(false),
                    SCTypeCode.Int32 => await ReadEncryptedBlockInt32(block, token).ConfigureAwait(false),
                    _ => ReadEncryptedBlock(block, token).ConfigureAwait(false),
                };
            }
            else
            {
                return await ReadDecryptedBlock(block, token).ConfigureAwait(false);
            }
        }

        public async Task WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Writing decrypted block {block.Key:X8}...");
            await SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);
            Log("Done");
        }

        private async Task<byte[]> ReadDecryptedBlock(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading decrypted block {block.Key:X8}...");
            var data = await SwitchConnection.PointerPeek(block.Size, block.Pointer!, token).ConfigureAwait(false);
            Log("Done");
            return data;
        }

        private async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            var data = await ReadEncryptedBlock(block, token).ConfigureAwait(false);
            return data[0] == 2;
        }

        private async Task<uint> ReadEncryptedBlockUint(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return ReadUInt32LittleEndian(header.AsSpan()[1..]);
        }

        private async Task<int> ReadEncryptedBlockInt32(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return ReadInt32LittleEndian(header.AsSpan()[1..]);
        }

        private async Task<byte> ReadEncryptedBlockByte(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return header[1];
        }

        private async Task<byte[]?> ReadEncryptedBlockArray(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading encrypted block {block.Key:X8}...");
            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = BlockUtil.DecryptBlock(block.Key, data);
            Log("Done");
            return data[6..];
        }

        private async Task<byte[]> ReadEncryptedBlockHeader(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading encrypted block header {block.Key:X8}...");
            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            Log("Done");
            return header;
        }

        //Thanks to Lincoln-LM (original scblock code) and Architdate (ported C# reference code)!!
        //https://github.com/Lincoln-LM/sv-live-map/blob/e0f4a30c72ef81f1dc175dae74e2fd3d63b0e881/sv_live_map_core/nxreader/raid_reader.py#L168
        //https://github.com/LegoFigure11/RaidCrawler/blob/2e1832ae89e5ac39dcc25ccf2ae911ef0f634580/MainWindow.cs#L199

        private async Task<byte[]?> ReadEncryptedBlockObject(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading encrypted block {block.Key:X8}...");
            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            var size = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5 + (int)size, token);
            var res = BlockUtil.DecryptBlock(block.Key, data)[5..];
            Log("Done");
            return res;
        }

        private async Task<byte[]> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading encrypted block {block.Key:X8}[L:{block.Size:X8}]...");
            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = BlockUtil.DecryptBlock(block.Key, data);
            Log("Done");
            return res;
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