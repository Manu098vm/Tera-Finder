using PKHeX.Core;

namespace TeraFinder
{
    public class DataBlock
    {
        public string? Name { get; set; }
        public uint Key { get; set; }
        public SCTypeCode Type { get; set; }
        public SCTypeCode SubType { get; set; }
        public IReadOnlyList<long>? Pointer { get; set; }
        public bool IsEncrypted { get; set; }
        public int Size { get; set; }
    }

    public static class Blocks
    {
        public static DataBlock KTeraRaids = new()
        {
            Name = "KTeraRaids",
            Key = 0xCAAC8800,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0x160, 0x40 },
            IsEncrypted = false,
            Size = 0xC98,
        };

        public static DataBlock KMyStatus = new()
        {
            Name = "KMyStatus",
            Key = 0xE3E89BD1,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0xE0, 0x40 },
            IsEncrypted = false,
            Size = 0x68,
        };

        public static DataBlock KBCATEventRaidIdentifier = new()
        {
            Name = "KBCATEventRaidIdentifier",
            Key = 0x37B99B4D,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44A98C8, 0x160, 0xD8 },
            IsEncrypted = false,
            Size = 0x04,
        };

        public static DataBlock KBCATFixedRewardItemArray = new()
        {
            Name = "KBCATFixedRewardItemArray",
            Key = 0x7D6C2B82,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0x160, 0x6C68, 0x0 },
            IsEncrypted = false,
            Size = 0x6B40,
        };

        public static DataBlock KBCATLotteryRewardItemArray = new()
        {
            Name = "KBCATLotteryRewardItemArray",
            Key = 0xA52B4811,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0x160, 0x6CA0, 0x0 },
            IsEncrypted = false,
            Size = 0xD0D8,
        };

        public static DataBlock KBCATRaidEnemyArray = new()
        {
            Name = "KBCATRaidEnemyArray",
            Key = 0x0520A1B0,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0x160, 0x6C30, 0x0 },
            IsEncrypted = false,
            Size = 0x7530,
        };

        public static DataBlock KBCATRaidPriorityArray = new()
        {
            Name = "KBCATRaidPriorityArray",
            Key = 0x095451E4,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44CCA68, 0x160, 0x6CD8, 0x0 },
            IsEncrypted = false,
            Size = 0x58,
        };

        public static DataBlock KUnlockedTeraRaidBattles = new()
        {
            Name = "KUnlockedTeraRaidBattles",
            Key = 0x27025EBF,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x44AAC88, 0xE0, 0x80, 0x8, 0x0 }, //TODO
            IsEncrypted = true,
            Size = 1,
        };

        public static DataBlock KUnlockedRaidDifficulty3 = new()
        {
            Name = "KUnlockedRaidDifficulty3",
            Key = 0xEC95D8EF,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x44AAC88, 0xE0, 0x80, 0x8, 0x2C1A0 }, //Ty Santacrab & Lincoln-LM!
            IsEncrypted = true,
            Size = 1,
        };

        public static DataBlock KUnlockedRaidDifficulty4 = new()
        {
            Name = "KUnlockedRaidDifficulty4",
            Key = 0xA9428DFE,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x44AAC88, 0xE0, 0x80, 0x8, 0x1F5E0 }, //Ty Santacrab & Lincoln-LM!
            IsEncrypted = true,
            Size = 1,
        };

        public static DataBlock KUnlockedRaidDifficulty5 = new()
        {
            Name = "KUnlockedRaidDifficulty5",
            Key = 0x9535F471,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x44AAC88, 0xE0, 0x80, 0x8, 0x1B800 }, //Ty Santacrab & Lincoln-LM!
            IsEncrypted = true,
            Size = 1,
        };

        public static DataBlock KUnlockedRaidDifficulty6 = new()
        {
            Name = "KUnlockedRaidDifficulty6",
            Key = 0x6E7F8220,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x44AAC88, 0xE0, 0x80, 0x8, 0x14040 }, //Ty Santacrab & Lincoln-LM!
            IsEncrypted = true,
            Size = 1,
        };
    }
}