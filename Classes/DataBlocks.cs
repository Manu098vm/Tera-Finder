using PKHeX.Core;

namespace TeraFinder
{
    public class DataBlock
    {
        public string? Name { get; set; }
        public uint Key { get; set; }
        public SCTypeCode Type { get; set; }
        public SCTypeCode SubType { get; set; }
    }

    public static class EventRaidBlocks
    {
        public static DataBlock KBCATEventRaidIdentifier = new()
        {
            Name = "KBCATEventRaidIdentifier",
            Key = 0x37B99B4D,
            Type = SCTypeCode.Object,
        };

        public static DataBlock KBCATFixedRewardItemArray = new()
        {
            Name = "KBCATFixedRewardItemArray",
            Key = 0x7D6C2B82,
            Type = SCTypeCode.Object,
        };

        public static DataBlock KBCATLotteryRewardItemArray = new()
        {
            Name = "KBCATLotteryRewardItemArray",
            Key = 0xA52B4811,
            Type = SCTypeCode.Object,
        };

        public static DataBlock KBCATRaidEnemyArray = new()
        {
            Name = "KBCATRaidEnemyArray",
            Key = 0x0520A1B0,
            Type = SCTypeCode.Object,
        };

        public static DataBlock KBCATRaidPriorityArray = new()
        {
            Name = "KBCATRaidPriorityArray",
            Key = 0x095451E4,
            Type = SCTypeCode.Object,
        };
    }

    public static class GameProgressBlocks
    {
        public static DataBlock KUnlockedTeraRaidBattles = new()
        {
            Name = "KUnlockedTeraRaidBattles",
            Key = 0x27025EBF,
            Type = SCTypeCode.Bool1,
        };

        public static DataBlock KUnlockedRaidDifficulty3 = new()
        {
            Name = "KUnlockedRaidDifficulty3",
            Key = 0xEC95D8EF,
            Type = SCTypeCode.Bool1,
        };

        public static DataBlock KUnlockedRaidDifficulty4 = new()
        {
            Name = "KUnlockedRaidDifficulty4",
            Key = 0xA9428DFE,
            Type = SCTypeCode.Bool1,
        };

        public static DataBlock KUnlockedRaidDifficulty5 = new()
        {
            Name = "KUnlockedRaidDifficulty5",
            Key = 0x9535F471,
            Type = SCTypeCode.Bool1,
        };

        public static DataBlock KUnlockedRaidDifficulty6 = new()
        {
            Name = "KUnlockedRaidDifficulty6",
            Key = 0x6E7F8220,
            Type = SCTypeCode.Bool1,
        };
    }
}