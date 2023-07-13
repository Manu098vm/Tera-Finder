using PKHeX.Core;
using SysBot.Base;
using System.Globalization;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core;

public class DeviceState : BotState<RoutineType, SwitchConnectionConfig>
{
    public override void IterateNextRoutine() => CurrentRoutineType = NextRoutineType;
    public override void Initialize() => Resume();
    public override void Pause() => NextRoutineType = RoutineType.None;
    public override void Resume() => NextRoutineType = InitialRoutine;
}

public class DeviceExecutor : SwitchRoutineExecutor<DeviceState>
{
    public const decimal BotbaseVersion = 2.3m;

    //Game Infos
    private const string VersionNumber = "1.3.2";
    private const string ScarletID = "0100A3D008C5C000";
    private const string VioletID = "01008F6008C5E000";

    private ulong KeyBlockAddress = 0;

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
        var botbase = await VerifyBotbaseVersion(token).ConfigureAwait(false);
        Log($"Valid Botbase Version: {botbase}");
        var game = await ReadGame(token).ConfigureAwait(false);
        Log($"Valid Title ID ({(game is GameVersion.SL ? $"{ScarletID}" : $"{VioletID}")})");
        var version = await SwitchConnection.GetGameInfo("version", token).ConfigureAwait(false);
        Log($"Valid Game Version: {version}");
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
        KeyBlockAddress = 0;
        HardStop();
        Connection.Disconnect();
    }

    public async Task<GameVersion> ReadGame(CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var title = await SwitchConnection.GetTitleID(token).ConfigureAwait(false);
        if (title.Equals(VioletID))
            return GameVersion.VL;
        else if (title.Equals(ScarletID))
            return GameVersion.SL;
        else throw new ArgumentOutOfRangeException($"Invalid Title ID ({title})");
    }

    //Thanks Anubis
    //https://github.com/kwsch/SysBot.NET/blob/b26c8c957364efe316573bec4b82e8c5c5a1a60f/SysBot.Pokemon/SV/PokeRoutineExecutor9SV.cs#L83C19-L83C19
    //AGPL v3 License
    public async Task<string> ReadGameVersion(CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var version = await SwitchConnection.GetGameInfo("version", token).ConfigureAwait(false);

        if (!version.SequenceEqual(VersionNumber))
            throw new Exception($"Game version is not supported. Expected version {VersionNumber}, and current game version is {version}.");

        return version;
    }

    //Thanks Anubis
    //https://github.com/kwsch/SysBot.NET/blob/b26c8c957364efe316573bec4b82e8c5c5a1a60f/SysBot.Pokemon/Actions/PokeRoutineExecutor.cs#L88
    //AGPL v3 License
    public async Task<string> VerifyBotbaseVersion(CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        var data = await SwitchConnection.GetBotbaseVersion(token).ConfigureAwait(false);
        var version = decimal.TryParse(data, CultureInfo.InvariantCulture, out var v) ? v : 0;
        if (version < BotbaseVersion)
        {
            var protocol = Config.Connection.Protocol;
            var msg = protocol is SwitchProtocol.WiFi ? "sys-botbase" : "usb-botbase";
            msg += $" version is not supported. Expected version {BotbaseVersion} or greater, and current version is {version}. Please download the latest version from: ";
            if (protocol is SwitchProtocol.WiFi)
                msg += "https://github.com/olliz0r/sys-botbase/releases/latest";
            else
                msg += "https://github.com/Koi-3088/usb-botbase/releases/latest";
            throw new Exception(msg);
        }
        return data;
    }

    public async Task<GameProgress> ReadGameProgress(CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Writing decrypted block {block.Key:X8}...");
        await SwitchConnection.PointerPoke(data, block.Pointer!, token).ConfigureAwait(false);
        Log("Done");

        return true;
    }

    private async Task<byte[]> ReadDecryptedBlock(DataBlock block, CancellationToken token)
    {
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
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
        if (Config.Connection.Protocol is SwitchProtocol.WiFi && !Connection.Connected)
            throw new InvalidOperationException("No remote connection");

        Log($"Reading encrypted block {block.Key:X8}[L:{block.Size:X8}]...");
        var address = await GetBlockAddress(block, token).ConfigureAwait(false);
        var data = await SwitchConnection.ReadBytesAbsoluteAsync(address, block.Size, token).ConfigureAwait(false);
        var res = BlockUtil.DecryptBlock(block.Key, data);
        Log("Done");
        return res[0] == 2;
    }

    //Thanks Architdate & Santacrab2!
    //https://github.com/LegoFigure11/RaidCrawler/blob/f8e996aac4b134e6eb6231d539c345748fead490/RaidCrawler.Core/Connection/ConnectionWrapper.cs#L126
    private async Task<ulong> GetBlockAddress(DataBlock block, CancellationToken token, bool prepareAddress = true)
    {
        if (KeyBlockAddress == 0)
            KeyBlockAddress = await SwitchConnection.PointerAll(block.Pointer!, token).ConfigureAwait(false);

        var keyblock = await SwitchConnection.ReadBytesAbsoluteAsync(KeyBlockAddress, 16, token).ConfigureAwait(false);
        var start = BitConverter.ToUInt64(keyblock.AsSpan()[..8]);
        var end = BitConverter.ToUInt64(keyblock.AsSpan()[8..]);
        var ct = (ulong)48;

        while (start < end)
        {
            var block_ct = (end - start) / ct;
            var mid = start + (block_ct >> 1) * ct;

            var data = await SwitchConnection.ReadBytesAbsoluteAsync(mid, 4, token).ConfigureAwait(false);
            var found = BitConverter.ToUInt32(data);
            if (found == block.Key)
            {
                if(prepareAddress)
                    mid = await PrepareAddress(mid, token).ConfigureAwait(false);
                return mid;
            }

            if (found >= block.Key)
                end = mid;
            else start = mid + ct;
        }
        throw new ArgumentOutOfRangeException("Save block not found.");
    }

    private async Task<ulong> PrepareAddress(ulong address, CancellationToken token) =>
        BitConverter.ToUInt64(await SwitchConnection.ReadBytesAbsoluteAsync(address + 8, 8, token).ConfigureAwait(false));
}