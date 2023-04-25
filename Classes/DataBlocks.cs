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

    //Thanks Anubis, Lincoln-LM, santacrab2 and Zyro670 for a lot of Pointers and offsets!!
    public static class Blocks
    {
        public static readonly long[] SaveBlockKeyPointer = { 0x44b5158, 0xD8, 0x0, 0x0, 0x30, 0x8 };

        #region DataSAV
        public static readonly DataBlock KTeraRaids = new()
        {
            Name = "KTeraRaids",
            Key = 0xCAAC8800,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0x160, 0x40 },
            IsEncrypted = false,
            Size = 0xC98,
        };

        public static readonly DataBlock KMyStatus = new()
        {
            Name = "KMyStatus",
            Key = 0xE3E89BD1,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0xE0, 0x40 },
            IsEncrypted = false,
            Size = 0x68,
        };

        public static readonly DataBlock KMassOutbreakNumActive = new()
        {
            Name = "KMassOutbreakNumActive",
            Key = 0x6C375C8A,
            Type = SCTypeCode.SByte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 0x01,
        };
        #endregion

        #region DataBCAT
        public static readonly DataBlock KBCATEventRaidIdentifier = new()
        {
            Name = "KBCATEventRaidIdentifier",
            Key = 0x37B99B4D,
            Type = SCTypeCode.Object,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 0x04,
        };

        public static readonly DataBlock KBCATFixedRewardItemArray = new()
        {
            Name = "KBCATFixedRewardItemArray",
            Key = 0x7D6C2B82,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0x160, 0x6C68, 0x0 },
            IsEncrypted = false,
            Size = 0x6B40,
        };

        public static readonly DataBlock KBCATLotteryRewardItemArray = new()
        {
            Name = "KBCATLotteryRewardItemArray",
            Key = 0xA52B4811,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0x160, 0x6CA0, 0x0 },
            IsEncrypted = false,
            Size = 0xD0D8,
        };

        public static readonly DataBlock KBCATRaidEnemyArray = new()
        {
            Name = "KBCATRaidEnemyArray",
            Key = 0x0520A1B0,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0x160, 0x6C30, 0x0 },
            IsEncrypted = false,
            Size = 0x7530,
        };

        public static readonly DataBlock KBCATRaidPriorityArray = new()
        {
            Name = "KBCATRaidPriorityArray",
            Key = 0x095451E4,
            Type = SCTypeCode.Object,
            Pointer = new long[] { 0x44E2F48, 0x160, 0x6CD8, 0x0 },
            IsEncrypted = false,
            Size = 0x58,
        };
        #endregion

        #region UnlockedFlags
        public static readonly DataBlock KUnlockedTeraRaidBattles = new()
        {
            Name = "KUnlockedTeraRaidBattles",
            Key = 0x27025EBF,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KUnlockedRaidDifficulty3 = new()
        {
            Name = "KUnlockedRaidDifficulty3",
            Key = 0xEC95D8EF,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KUnlockedRaidDifficulty4 = new()
        {
            Name = "KUnlockedRaidDifficulty4",
            Key = 0xA9428DFE,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KUnlockedRaidDifficulty5 = new()
        {
            Name = "KUnlockedRaidDifficulty5",
            Key = 0x9535F471,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KUnlockedRaidDifficulty6 = new()
        {
            Name = "KUnlockedRaidDifficulty6",
            Key = 0x6E7F8220,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };
        #endregion

        #region Outbreak1
        public static readonly DataBlock KMassOutbreak01CenterPos = new()
        {
            Name = "KMassOutbreak01CenterPos",
            Key = 0x2ED42F4D,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak01DummyPos = new()
        {
            Name = "KMassOutbreak01DummyPos",
            Key = 0x4A13BE7C,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak01Species = new()
        {
            Name = "KMassOutbreak01Species",
            Key = 0x76A2F996,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak01Form = new()
        {
            Name = "KMassOutbreak01Form",
            Key = 0x29B4615D,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak01Found = new()
        {
            Name = "KMassOutbreak01Found",
            Key = 0x7E203623,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak01NumKOed = new()
        {
            Name = "KMassOutbreak01NumKOed",
            Key = 0x4B16FBC2,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak01TotalSpawns = new()
        {
            Name = "KMassOutbreak01TotalSpawns",
            Key = 0xB7DC495A,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak2
        public static readonly DataBlock KMassOutbreak02CenterPos = new()
        {
            Name = "KMassOutbreak02CenterPos",
            Key = 0x2ED5F198,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak02DummyPos = new()
        {
            Name = "KMassOutbreak02DummyPos",
            Key = 0x4A118F71,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer, 
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak02Species = new()
        {
            Name = "KMassOutbreak02Species",
            Key = 0x76A0BCF3,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak02Form = new()
        {
            Name = "KMassOutbreak02Form",
            Key = 0x29B84368,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak02Found = new()
        {
            Name = "KMassOutbreak02Found",
            Key = 0x7E22DF86,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak02NumKOed = new()
        {
            Name = "KMassOutbreak02NumKOed",
            Key = 0x4B14BF1F,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak02TotalSpawns = new()
        {
            Name = "KMassOutbreak02TotalSpawns",
            Key = 0xB7DA0CB7,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak3
        public static readonly DataBlock KMassOutbreak03CenterPos = new()
        {
            Name = "KMassOutbreak03CenterPos",
            Key = 0x2ECE09D3,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak03DummyPos = new()
        {
            Name = "KMassOutbreak03DummyPos",
            Key = 0x4A0E135A,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak03Species = new()
        {
            Name = "KMassOutbreak03Species",
            Key = 0x76A97E38,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak03Form = new()
        {
            Name = "KMassOutbreak03Form",
            Key = 0x29AF8223,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak03Found = new()
        {
            Name = "KMassOutbreak03Found",
            Key = 0x7E25155D,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak03NumKOed = new()
        {
            Name = "KMassOutbreak03NumKOed",
            Key = 0x4B1CA6E4,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak03TotalSpawns = new()
        {
            Name = "KMassOutbreak03TotalSpawns",
            Key = 0xB7E1F47C,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer, 
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak4
        public static readonly DataBlock KMassOutbreak04CenterPos = new()
        {
            Name = "KMassOutbreak04CenterPos",
            Key = 0x2ED04676,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak04DummyPos = new()
        {
            Name = "KMassOutbreak04DummyPos",
            Key = 0x4A0BD6B7,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak04Species = new()
        {
            Name = "KMassOutbreak04Species",
            Key = 0x76A6E26D,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak04Form = new()
        {
            Name = "KMassOutbreak04Form",
            Key = 0x29B22B86,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak04Found = new()
        {
            Name = "KMassOutbreak04Found",
            Key = 0x7E28F768,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak04NumKOed = new()
        {
            Name = "KMassOutbreak04NumKOed",
            Key = 0x4B1A77D9,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer, 
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak04TotalSpawns = new()
        {
            Name = "KMassOutbreak04TotalSpawns",
            Key = 0xB7DFC571,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak5
        public static readonly DataBlock KMassOutbreak05CenterPos = new()
        {
            Name = "KMassOutbreak05CenterPos",
            Key = 0x2EC78531,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak05DummyPos = new()
        {
            Name = "KMassOutbreak05DummyPos",
            Key = 0x4A1FFBD8,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak05Species = new()
        {
            Name = "KMassOutbreak05Species",
            Key = 0x76986F3A,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak05Form = new()
        {
            Name = "KMassOutbreak05Form",
            Key = 0x29A9D701,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak05Found = new()
        {
            Name = "KMassOutbreak05Found",
            Key = 0x7E13F8C7,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak05NumKOed = new()
        {
            Name = "KMassOutbreak05NumKOed",
            Key = 0x4B23391E,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak05TotalSpawns = new()
        {
            Name = "KMassOutbreak05TotalSpawns",
            Key = 0xB7E886B6,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak6
        public static readonly DataBlock KMassOutbreak06CenterPos = new()
        {
            Name = "KMassOutbreak06CenterPos",
            Key = 0x2ECB673C,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak06DummyPos = new()
        {
            Name = "KMassOutbreak06DummyPos",
            Key = 0x4A1C868D,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak06Species = new()
        {
            Name = "KMassOutbreak06Species",
            Key = 0x76947F97,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak06Form = new()
        {
            Name = "KMassOutbreak06Form",
            Key = 0x29AB994C,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak06Found = new()
        {
            Name = "KMassOutbreak06Found",
            Key = 0x7E16A22A,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak06NumKOed = new()
        {
            Name = "KMassOutbreak06NumKOed",
            Key = 0x4B208FBB,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak06TotalSpawns = new()
        {
            Name = "KMassOutbreak06TotalSpawns",
            Key = 0xB7E49713,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak7
        public static readonly DataBlock KMassOutbreak07CenterPos = new()
        {
            Name = "KMassOutbreak07CenterPos",
            Key = 0x2EC1CC77,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak07DummyPos = new()
        {
            Name = "KMassOutbreak07DummyPos",
            Key = 0x4A1A50B6,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak07Species = new()
        {
            Name = "KMassOutbreak07Species",
            Key = 0x769D40DC,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak07Form = new()
        {
            Name = "KMassOutbreak07Form",
            Key = 0x29A344C7,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak07Found = new()
        {
            Name = "KMassOutbreak07Found",
            Key = 0x7E1A8B01,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak07NumKOed = new()
        {
            Name = "KMassOutbreak07NumKOed",
            Key = 0x4B28E440,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer, 
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak07TotalSpawns = new()
        {
            Name = "KMassOutbreak07TotalSpawns",
            Key = 0xB7EE31D8,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion

        #region Outbreak8
        public static readonly DataBlock KMassOutbreak08CenterPos = new()
        {
            Name = "KMassOutbreak08CenterPos",
            Key = 0x2EC5BC1A,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak08DummyPos = new()
        {
            Name = "KMassOutbreak08DummyPos",
            Key = 0x4A166113,
            Type = SCTypeCode.Array,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 12,
        };

        public static readonly DataBlock KMassOutbreak08Species = new()
        {
            Name = "KMassOutbreak08Species",
            Key = 0x769B11D1,
            Type = SCTypeCode.UInt32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak08Form = new()
        {
            Name = "KMassOutbreak08Form",
            Key = 0x29A5EE2A,
            Type = SCTypeCode.Byte,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak08Found = new()
        {
            Name = "KMassOutbreak08Found",
            Key = 0x7E1C4D4C,
            Type = SCTypeCode.Bool1,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 1,
        };

        public static readonly DataBlock KMassOutbreak08NumKOed = new()
        {
            Name = "KMassOutbreak08NumKOed",
            Key = 0x4B256EF5,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };

        public static readonly DataBlock KMassOutbreak08TotalSpawns = new()
        {
            Name = "KMassOutbreak08TotalSpawns",
            Key = 0xB7EABC8D,
            Type = SCTypeCode.Int32,
            Pointer = SaveBlockKeyPointer,
            IsEncrypted = true,
            Size = 4,
        };
        #endregion
    }
}