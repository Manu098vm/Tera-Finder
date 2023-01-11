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
    }

    public static class Blocks
    {
        public static DataBlock KTeraRaids = new()
        {
            Name = "KTeraRaids",
            Key = 0xCAAC8800,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x43A77C8, 0x160, 0x40 },
        };

        public static DataBlock KMyStatus = new()
        {
            Name = "KMyStatus",
            Key = 0xE3E89BD1,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x29F40 }, //Ty Lincoln-LM!
        };

        public static DataBlock KBCATEventRaidIdentifier = new()
        {
            Name = "KBCATEventRaidIdentifier",
            Key = 0x37B99B4D,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x0 }, //TODO
        };

        public static DataBlock KBCATFixedRewardItemArray = new()
        {
            Name = "KBCATFixedRewardItemArray",
            Key = 0x7D6C2B82,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x16D40 } //Ty Lincoln-LM!
        };

        public static DataBlock KBCATLotteryRewardItemArray = new()
        {
            Name = "KBCATLotteryRewardItemArray",
            Key = 0xA52B4811,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x1E6A0 } //Ty Lincoln-LM!
        };

        public static DataBlock KBCATRaidEnemyArray = new()
        {
            Name = "KBCATRaidEnemyArray",
            Key = 0x0520A1B0,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x1040 } //Ty Lincoln-LM!
        };

        public static DataBlock KBCATRaidPriorityArray = new()
        {
            Name = "KBCATRaidPriorityArray",
            Key = 0x095451E4,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x1860 } //Ty Lincoln-LM!
        };

        public static DataBlock KUnlockedTeraRaidBattles = new()
        {
            Name = "KUnlockedTeraRaidBattles",
            Key = 0x27025EBF,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x0  } //TODO
        };

        public static DataBlock KUnlockedRaidDifficulty3 = new()
        {
            Name = "KUnlockedRaidDifficulty3",
            Key = 0xEC95D8EF,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x2BF20 } //Ty Lincoln-LM!
        };

        public static DataBlock KUnlockedRaidDifficulty4 = new()
        {
            Name = "KUnlockedRaidDifficulty4",
            Key = 0xA9428DFE,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x1F400 } //Ty Lincoln-LM!
        };

        public static DataBlock KUnlockedRaidDifficulty5 = new()
        {
            Name = "KUnlockedRaidDifficulty5",
            Key = 0x9535F471,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x1B640 } //Ty Lincoln-LM!
        };

        public static DataBlock KUnlockedRaidDifficulty6 = new()
        {
            Name = "KUnlockedRaidDifficulty6",
            Key = 0x6E7F8220,
            Type = SCTypeCode.Bool1,
            Pointer = new long[] { 0x4385F30, 0x80, 0x8, 0x13EC0 } //Ty Lincoln-LM!
        };
    }
}