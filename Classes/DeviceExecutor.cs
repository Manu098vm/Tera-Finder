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

        public async Task<bool> WriteBlock(object data, DataBlock block, CancellationToken token, object? toExpect = default)
        {
            if (block.IsEncrypted)
                return await WriteEncryptedBlockSafe(block, toExpect, data, token).ConfigureAwait(false);
            else
                return await WriteDecryptedBlock((byte[])data!, block, token).ConfigureAwait(false);
        }

        //Thanks santacrab2 & Zyro670 for the help with the following code
        public async Task<object?> ReadBlock(DataBlock block, CancellationToken token)
        {
            if (block.IsEncrypted)
                return await ReadEncryptedBlock(block, token).ConfigureAwait(false);
            else
                return await ReadDecryptedBlock(block, token).ConfigureAwait(false);
        }

        private async Task<bool> WriteDecryptedBlock(byte[] data, DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Writing decrypted block {block.Key:X8}...");
            await SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);
            Log("Done");

            return true;
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

        private async Task<bool> WriteEncryptedBlockSafe(DataBlock block, object? toExpect, object toWrite, CancellationToken token)
        {
            if (toExpect == default || toWrite == default)
                return false;

            return block.Type switch
            {
                SCTypeCode.Array => await WriteEncryptedBlockArray(block, (byte[])toExpect, (byte[])toWrite, token).ConfigureAwait(false),
                SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await WriteEncryptedBlockBool(block, (bool)toExpect, (bool)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Byte or SCTypeCode.SByte => await WriteEncryptedBlockByte(block, (byte)toExpect, (byte)toWrite, token).ConfigureAwait(false),
                SCTypeCode.UInt32 => await WriteEncryptedBlockUint(block, (uint)toExpect, (uint)toWrite, token).ConfigureAwait(false),
                SCTypeCode.Int32 => await WriteEncryptedBlockInt32(block, (int)toExpect, (int)toWrite, token).ConfigureAwait(false),
                _ => throw new NotSupportedException($"Block {block.Name} (Type {block.Type}) is currently not supported.")
            };
        }

        private async Task<object?> ReadEncryptedBlock(DataBlock block, CancellationToken token)
        {
            return block.Type switch
            {
                SCTypeCode.Object => await ReadEncryptedBlockObject(block, token).ConfigureAwait(false),
                SCTypeCode.Array => await ReadEncryptedBlockArray(block, token).ConfigureAwait(false),
                SCTypeCode.Bool1 or SCTypeCode.Bool2 or SCTypeCode.Bool3 => await ReadEncryptedBlockBool(block, token).ConfigureAwait(false),
                SCTypeCode.Byte or SCTypeCode.SByte => await ReadEncryptedBlockByte(block, token).ConfigureAwait(false),
                SCTypeCode.UInt32 => await ReadEncryptedBlockUint(block, token).ConfigureAwait(false),
                SCTypeCode.Int32 => await ReadEncryptedBlockInt32(block, token).ConfigureAwait(false),
                _ => throw new NotSupportedException($"Block {block.Name} (Type {block.Type}) is currently not supported.")
            };
        }

        private async Task<bool> WriteEncryptedBlockUint(DataBlock block, uint valueToExpect ,uint valueToInject, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            //Always read and decrypt first to validate address and data
            Log($"Writing encrypted block {block.Key:X8}...");
            ulong address;
            try { address = await GetBlockAddress(block, token).ConfigureAwait(false); }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadUInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteUInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = BlockUtil.EncryptBlock(block.Key, header);
            await SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);
            Log("Done");
            return true;
        }

        private async Task<uint> ReadEncryptedBlockUint(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return ReadUInt32LittleEndian(header.AsSpan()[1..]);
        }

        private async Task<bool> WriteEncryptedBlockInt32(DataBlock block, int valueToExpect, int valueToInject, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            //Always read and decrypt first to validate address and data
            Log($"Writing encrypted block {block.Key:X8}...");
            ulong address;
            try { address = await GetBlockAddress(block, token).ConfigureAwait(false); }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = ReadInt32LittleEndian(header.AsSpan()[1..]);
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            WriteInt32LittleEndian(header.AsSpan()[1..], valueToInject);
            header = BlockUtil.EncryptBlock(block.Key, header);
            await SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);
            Log("Done");
            return true;
        }

        private async Task<int> ReadEncryptedBlockInt32(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return ReadInt32LittleEndian(header.AsSpan()[1..]);
        }

        private async Task<bool> WriteEncryptedBlockByte(DataBlock block, byte valueToExpect, byte valueToInject, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            //Always read and decrypt first to validate address and data
            Log($"Writing encrypted block {block.Key:X8}...");
            ulong address;
            try { address = await GetBlockAddress(block, token).ConfigureAwait(false); }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var header = await SwitchConnection.ReadBytesAbsoluteAsync(address, 5, token).ConfigureAwait(false);
            header = BlockUtil.DecryptBlock(block.Key, header);
            //Validate ram data
            var ram = header[1];
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            header[1] = valueToInject;
            header = BlockUtil.EncryptBlock(block.Key, header);
            await SwitchConnection.WriteBytesAbsoluteAsync(header, address, token).ConfigureAwait(false);
            Log("Done");
            return true;
        }

        private async Task<byte> ReadEncryptedBlockByte(DataBlock block, CancellationToken token)
        {
            var header = await ReadEncryptedBlockHeader(block, token).ConfigureAwait(false);
            return header[1];
        }

        private async Task<bool> WriteEncryptedBlockArray(DataBlock block, byte[] arrayToExpect, byte[] arrayToInject, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            //Always read and decrypt first to validate address and data
            Log($"Writing encrypted block {block.Key:X8}...");
            ulong address;
            try { address = await GetBlockAddress(block, token).ConfigureAwait(false); }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, 6 + block.Size, token).ConfigureAwait(false);
            data = BlockUtil.DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[6..];
            if (!ram.SequenceEqual(arrayToExpect)) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            Array.ConstrainedCopy(arrayToInject, 0, data, 6, block.Size);
            data = BlockUtil.EncryptBlock(block.Key, data);
            await SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);
            Log("Done");
            return true;
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

        private async Task<bool> WriteEncryptedBlockBool(DataBlock block, bool valueToExpect, bool valueToInject, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            //Always read and decrypt first to validate address and data
            Log($"Writing encrypted block {block.Key:X8}...");
            ulong address;
            try { address = await GetBlockAddress(block, token).ConfigureAwait(false); }
            catch (Exception) { return false; }
            //If we get there without exceptions, the block address is valid
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            data = BlockUtil.DecryptBlock(block.Key, data);
            //Validate ram data
            var ram = data[0] == 2;
            if (ram != valueToExpect) return false;
            //If we get there then both block address and block data are valid, we can safely inject
            data[0] = valueToInject ? (byte)2 : (byte)1;
            data = BlockUtil.EncryptBlock(block.Key, data);
            await SwitchConnection.WriteBytesAbsoluteAsync(data, address, token).ConfigureAwait(false);
            Log("Done");
            return true;
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

        private async Task<bool> ReadEncryptedBlockBool(DataBlock block, CancellationToken token)
        {
            if (!Connection.Connected)
                throw new InvalidOperationException("No remote connection");

            Log($"Reading encrypted block {block.Key:X8}[L:{block.Size:X8}]...");
            var address = await GetBlockAddress(block, token).ConfigureAwait(false);
            var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
            var res = BlockUtil.DecryptBlock(block.Key, data);
            Log("Done");
            return res[0] == 2;
        }

        private async Task<ulong> GetBlockAddress(DataBlock block, CancellationToken token)
        {
            var read_key = ReadUInt32LittleEndian(await SwitchConnection.PointerPeek(4, block.Pointer!, token).ConfigureAwait(false));
            if (read_key == block.Key)
                return await SwitchConnection.PointerAll(PreparePointer(block.Pointer!), token).ConfigureAwait(false);
            var direction = block.Key > read_key ? 1 : -1;
            var base_offset = block.Pointer![block.Pointer.Count - 1];
            for (var offset = base_offset; offset < base_offset + 0x2000 && offset > base_offset - 0x2000; offset += direction * 0x20)
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