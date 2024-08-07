using PKHeX.Core;

namespace TeraFinder.Core;

public struct BlockDefinition
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
public static class BlockDefinitions
{
    public static readonly long[] SaveBlockKeyPointer = [0x47350D8, 0xD8, 0x0, 0x0, 0x30, 0x08];

    #region DataSAV
    public static readonly BlockDefinition KTeraRaidPaldea = new()
    {
        Name = "KTeraRaidPaldea",
        Key = 0xCAAC8800,
        Type = SCTypeCode.Object,
        Pointer = [0x47350D8, 0x1C0, 0x88, 0x40],
        IsEncrypted = false,
        Size = 0xC98,
    };

    public static readonly BlockDefinition KTeraRaidDLC = new()
    {
        Name = "KTeraRaidDLC",
        Key = 0x100B93DA,
        Type = SCTypeCode.Object,
        Pointer = [0x47350D8, 0x1C0, 0x88, 0xCD8],
        IsEncrypted = false,
        Size = 0x1910,
    };

    public static readonly BlockDefinition KTeraRaidBlueberry = new()
    {
        Name = "KTeraRaidBlueberry",
        Key = 0x100B93DA,
        Type = SCTypeCode.Object,
        Pointer = [0x47350D8, 0x1C0, 0x88, 0x1958],
        IsEncrypted = false,
        Size = 0xC90,
    };

    public static readonly BlockDefinition KSevenStarRaidsCapture = new()
    {
        Name = "KSevenStarRaidsCapture",
        Key = 0x8B14392F,
        Type = SCTypeCode.Object,
        Pointer = [0x47350D8, 0x1C0, 0x88, 0x25E8],
        IsEncrypted = false,
        Size = 0x5DC0,
    };

    public static readonly BlockDefinition KMyStatus = new()
    {
        Name = "KMyStatus",
        Key = 0xE3E89BD1,
        Type = SCTypeCode.Object,
        Pointer = [0x47350D8, 0xD8, 0x08, 0xB8, 0x0, 0x40],
        IsEncrypted = false,
        Size = 0x68,
    };

    public static readonly BlockDefinition KOutbreakMainNumActive = new()
    {
        Name = "KOutbreakMainNumActive",
        Key = 0x6C375C8A,
        Type = SCTypeCode.SByte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };

    public static readonly BlockDefinition KOutbreakDLC1NumActive = new()
    {
        Name = "KOutbreakDLC1NumActive",
        Key = 0xBD7C2A04,
        Type = SCTypeCode.SByte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };

    public static readonly BlockDefinition KOutbreakDLC2NumActive = new()
    {
        Name = "KOutbreakDLC2NumActive",
        Key = 0x19A98811,
        Type = SCTypeCode.SByte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };
    public static readonly BlockDefinition KOutbreakBCMainNumActive = new()
    {
        Name = "KOutbreakBCMainNumActive",
        Key = 0x7478FD9A,
        Type = SCTypeCode.SByte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };
    public static readonly BlockDefinition KOutbreakBCDLC1NumActive = new()
    {
        Name = "KOutbreakBCDLC1NumActive",
        Key = 0x0D326604,
        Type = SCTypeCode.SByte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };
    public static readonly BlockDefinition KOutbreakBCDLC2NumActive = new()
    {
        Name = "KOutbreakBCDLC2NumActive",
        Key = 0x1B4ECAC3,
        Type = SCTypeCode.SByte,
        Pointer= SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };
    #endregion

    #region KBCATRaid
    public static readonly BlockDefinition KBCATEventRaidIdentifier = new()
    {
        Name = "KBCATEventRaidIdentifier",
        Key = 0x37B99B4D,
        Type = SCTypeCode.Object,
        Pointer = [0x4763C80, 0x08, 0x288, 0xE300],
        IsEncrypted = false,
        Size = 0x04,
    };

    public static readonly BlockDefinition KBCATFixedRewardItemArray = new()
    {
        Name = "KBCATFixedRewardItemArray",
        Key = 0x7D6C2B82,
        Type = SCTypeCode.Object,
        Pointer = [0x4763C80, 0x08, 0x288, 0xE340, 0x0],
        IsEncrypted = false,
        Size = 0x6B40,
    };

    public static readonly BlockDefinition KBCATLotteryRewardItemArray = new()
    {
        Name = "KBCATLotteryRewardItemArray",
        Key = 0xA52B4811,
        Type = SCTypeCode.Object,
        Pointer = [0x4763C80, 0x08, 0x288, 0xE378, 0x0],
        IsEncrypted = false,
        Size = 0xD0D8,
    };

    public static readonly BlockDefinition KBCATRaidEnemyArray = new()
    {
        Name = "KBCATRaidEnemyArray",
        Key = 0x0520A1B0,
        Type = SCTypeCode.Object,
        Pointer = [0x4763C80, 0x08, 0x288, 0xE308, 0x0],
        IsEncrypted = false,
        Size = 0x7530,
    };

    public static readonly BlockDefinition KBCATRaidPriorityArray = new()
    {
        Name = "KBCATRaidPriorityArray",
        Key = 0x095451E4,
        Type = SCTypeCode.Object,
        Pointer = [0x4763C80, 0x08, 0x288, 0xE3B0, 0x0],
        IsEncrypted = false,
        Size = 0x58,
    };
    #endregion

    #region UnlockedFlags
    public static readonly BlockDefinition KUnlockedTeraRaidBattles = new()
    {
        Name = "KUnlockedTeraRaidBattles",
        Key = 0x27025EBF,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KUnlockedRaidDifficulty3 = new()
    {
        Name = "KUnlockedRaidDifficulty3",
        Key = 0xEC95D8EF,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KUnlockedRaidDifficulty4 = new()
    {
        Name = "KUnlockedRaidDifficulty4",
        Key = 0xA9428DFE,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KUnlockedRaidDifficulty5 = new()
    {
        Name = "KUnlockedRaidDifficulty5",
        Key = 0x9535F471,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KUnlockedRaidDifficulty6 = new()
    {
        Name = "KUnlockedRaidDifficulty6",
        Key = 0x6E7F8220,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    #endregion

    #region Outbreak01
    public static readonly BlockDefinition KOutbreak01MainCenterPos = new()
    {
        Name = "KOutbreak01MainCenterPos",
        Key = 0x2ED42F4D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01MainDummyPos = new()
    {
        Name = "KOutbreak01MainDummyPos",
        Key = 0x4A13BE7C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01MainSpecies = new()
    {
        Name = "KOutbreak01MainSpecies",
        Key = 0x76A2F996,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01MainForm = new()
    {
        Name = "KOutbreak01MainForm",
        Key = 0x29B4615D,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01MainFound = new()
    {
        Name = "KOutbreak01MainFound",
        Key = 0x7E203623,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01MainNumKOed = new()
    {
        Name = "KOutbreak01MainNumKOed",
        Key = 0x4B16FBC2,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01MainTotalSpawns = new()
    {
        Name = "KOutbreak01MainTotalSpawns",
        Key = 0xB7DC495A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak02
    public static readonly BlockDefinition KOutbreak02MainCenterPos = new()
    {
        Name = "KOutbreak02MainCenterPos",
        Key = 0x2ED5F198,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02MainDummyPos = new()
    {
        Name = "KOutbreak02MainDummyPos",
        Key = 0x4A118F71,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer, 
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02MainSpecies = new()
    {
        Name = "KOutbreak02MainSpecies",
        Key = 0x76A0BCF3,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02MainForm = new()
    {
        Name = "KOutbreak02MainForm",
        Key = 0x29B84368,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02MainFound = new()
    {
        Name = "KOutbreak02MainFound",
        Key = 0x7E22DF86,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02MainNumKOed = new()
    {
        Name = "KOutbreak02MainNumKOed",
        Key = 0x4B14BF1F,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02MainTotalSpawns = new()
    {
        Name = "KOutbreak02MainTotalSpawns",
        Key = 0xB7DA0CB7,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak03
    public static readonly BlockDefinition KOutbreak03MainCenterPos = new()
    {
        Name = "KOutbreak03MainCenterPos",
        Key = 0x2ECE09D3,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03MainDummyPos = new()
    {
        Name = "KOutbreak03MainDummyPos",
        Key = 0x4A0E135A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03MainSpecies = new()
    {
        Name = "KOutbreak03MainSpecies",
        Key = 0x76A97E38,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03MainForm = new()
    {
        Name = "KOutbreak03MainForm",
        Key = 0x29AF8223,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03MainFound = new()
    {
        Name = "KOutbreak03MainFound",
        Key = 0x7E25155D,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03MainNumKOed = new()
    {
        Name = "KOutbreak03MainNumKOed",
        Key = 0x4B1CA6E4,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03MainTotalSpawns = new()
    {
        Name = "KOutbreak03MainTotalSpawns",
        Key = 0xB7E1F47C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer, 
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak04
    public static readonly BlockDefinition KOutbreak04MainCenterPos = new()
    {
        Name = "KOutbreak04MainCenterPos",
        Key = 0x2ED04676,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04MainDummyPos = new()
    {
        Name = "KOutbreak04MainDummyPos",
        Key = 0x4A0BD6B7,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04MainSpecies = new()
    {
        Name = "KOutbreak04MainSpecies",
        Key = 0x76A6E26D,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04MainForm = new()
    {
        Name = "KOutbreak04MainForm",
        Key = 0x29B22B86,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04MainFound = new()
    {
        Name = "KOutbreak04MainFound",
        Key = 0x7E28F768,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04MainNumKOed = new()
    {
        Name = "KOutbreak04MainNumKOed",
        Key = 0x4B1A77D9,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer, 
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04MainTotalSpawns = new()
    {
        Name = "KOutbreak04MainTotalSpawns",
        Key = 0xB7DFC571,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak05
    public static readonly BlockDefinition KOutbreak05MainCenterPos = new()
    {
        Name = "KOutbreak05MainCenterPos",
        Key = 0x2EC78531,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak05MainDummyPos = new()
    {
        Name = "KOutbreak05MainDummyPos",
        Key = 0x4A1FFBD8,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak05MainSpecies = new()
    {
        Name = "KOutbreak05MainSpecies",
        Key = 0x76986F3A,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak05MainForm = new()
    {
        Name = "KOutbreak05MainForm",
        Key = 0x29A9D701,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak05MainFound = new()
    {
        Name = "KOutbreak05MainFound",
        Key = 0x7E13F8C7,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak05MainNumKOed = new()
    {
        Name = "KOutbreak05MainNumKOed",
        Key = 0x4B23391E,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak05MainTotalSpawns = new()
    {
        Name = "KOutbreak05MainTotalSpawns",
        Key = 0xB7E886B6,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak06
    public static readonly BlockDefinition KOutbreak06MainCenterPos = new()
    {
        Name = "KOutbreak06MainCenterPos",
        Key = 0x2ECB673C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak06MainDummyPos = new()
    {
        Name = "KOutbreak06MainDummyPos",
        Key = 0x4A1C868D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak06MainSpecies = new()
    {
        Name = "KOutbreak06MainSpecies",
        Key = 0x76947F97,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak06MainForm = new()
    {
        Name = "KOutbreak06MainForm",
        Key = 0x29AB994C,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak06MainFound = new()
    {
        Name = "KOutbreak06MainFound",
        Key = 0x7E16A22A,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak06MainNumKOed = new()
    {
        Name = "KOutbreak06MainNumKOed",
        Key = 0x4B208FBB,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak06MainTotalSpawns = new()
    {
        Name = "KOutbreak06MainTotalSpawns",
        Key = 0xB7E49713,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak07
    public static readonly BlockDefinition KOutbreak07MainCenterPos = new()
    {
        Name = "KOutbreak07MainCenterPos",
        Key = 0x2EC1CC77,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak07MainDummyPos = new()
    {
        Name = "KOutbreak07MainDummyPos",
        Key = 0x4A1A50B6,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak07MainSpecies = new()
    {
        Name = "KOutbreak07MainSpecies",
        Key = 0x769D40DC,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak07MainForm = new()
    {
        Name = "KOutbreak07MainForm",
        Key = 0x29A344C7,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak07MainFound = new()
    {
        Name = "KOutbreak07MainFound",
        Key = 0x7E1A8B01,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak07MainNumKOed = new()
    {
        Name = "KOutbreak07MainNumKOed",
        Key = 0x4B28E440,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer, 
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak07MainTotalSpawns = new()
    {
        Name = "KOutbreak07MainTotalSpawns",
        Key = 0xB7EE31D8,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak08
    public static readonly BlockDefinition KOutbreak08MainCenterPos = new()
    {
        Name = "KOutbreak08MainCenterPos",
        Key = 0x2EC5BC1A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak08MainDummyPos = new()
    {
        Name = "KOutbreak08MainDummyPos",
        Key = 0x4A166113,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak08MainSpecies = new()
    {
        Name = "KOutbreak08MainSpecies",
        Key = 0x769B11D1,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak08MainForm = new()
    {
        Name = "KOutbreak08MainForm",
        Key = 0x29A5EE2A,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak08MainFound = new()
    {
        Name = "KOutbreak08MainFound",
        Key = 0x7E1C4D4C,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak08MainNumKOed = new()
    {
        Name = "KOutbreak08MainNumKOed",
        Key = 0x4B256EF5,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak08MainTotalSpawns = new()
    {
        Name = "KOutbreak08MainTotalSpawns",
        Key = 0xB7EABC8D,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region BCATOutbreak01
    public static BlockDefinition KOutbreakBC01MainCenterPos = new()
    {
        Name = "KOutbreakBC01MainCenterPos",
        Key = 0x71DB2C9D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,

    };
    public static readonly BlockDefinition KOutbreakBC01MainDummyPos = new()
    {
        Name = "KOutbreakBC01MainDummyPos",
        Key = 0xB5D2D0EC,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC01MainSpecies = new()
    {
        Name = "KOutbreakBC01MainSpecies",
        Key = 0x84AB44A6,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC01MainForm = new()
    {
        Name = "KOutbreakBC01MainForm",
        Key = 0xD82BDDAD,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01MainFound = new()
    {
        Name = "KOutbreakBC01MainFound",
        Key = 0x6F473373,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC01MainNumKOed = new()
    {
        Name = "KOutbreakBC01MainNumKOed",
        Key = 0x65AC15F2,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01MainTotalSpawns = new()
    {
        Name = "KOutbreakBC01MainTotalSpawns",
        Key = 0x71862A2A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak02
    public static BlockDefinition KOutbreakBC02MainCenterPos = new()
    {
        Name = "KOutbreakBC02MainCenterPos",
        Key = 0x71DD5BA8,
        Type = SCTypeCode.Array,
        Pointer= SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC02MainDummyPos = new()
    {
        Name = "KOutbreakBC02MainDummyPos",
        Key = 0xB5D03521,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC02MainSpecies = new()
    {
        Name = "KOutbreakBC02MainSpecies",
        Key = 0x84A7C1C3,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC02MainForm = new()
    {
        Name = "KOutbreakBC02MainForm",
        Key = 0xD82E7978,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02MainFound = new()
    {
        Name = "KOutbreakBC02MainFound",
        Key = 0x6F497016,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC02MainNumKOed = new()
    {
        Name = "KOutbreakBC02MainNumKOed",
        Key = 0x65A8930F,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02MainTotalSpawns = new()
    {
        Name = "KOutbreakBC02MainTotalSpawns",
        Key = 0x718380C7,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak03
    public static BlockDefinition KOutbreakBC03MainCenterPos = new()
    {
        Name = "KOutbreakBC03MainCenterPos",
        Key = 0x71D49A63,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC03MainDummyPos = new()
    {
        Name = "KOutbreakBC03MainDummyPos",
        Key = 0xB5CC4C4A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC03MainSpecies = new()
    {
        Name = "KOutbreakBC03MainSpecies",
        Key = 0x84B15C88,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC03MainForm = new()
    {
        Name = "KOutbreakBC03MainForm",
        Key = 0xD825B833,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03MainFound = new()
    {
        Name = "KOutbreakBC03MainFound",
        Key = 0x6F4D58ED,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC03MainNumKOed = new()
    {
        Name = "KOutbreakBC03MainNumKOed",
        Key = 0x65B22DD4,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03MainTotalSpawns = new()
    {
        Name = "KOutbreakBC03MainTotalSpawns",
        Key = 0x718BD54C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak04
    public static BlockDefinition KOutbreakBC04MainCenterPos = new()
    {
        Name = "KOutbreakBC04MainCenterPos",
        Key = 0x71D743C6,
        Type = SCTypeCode.Array,
        Pointer= SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC04MainDummyPos = new()
    {
        Name = "KOutbreakBC04MainDummyPos",
        Key = 0xB5CA7C67,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC04MainSpecies = new()
    {
        Name = "KOutbreakBC04MainSpecies",
        Key = 0x84AD7A7D,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC04MainForm = new()
    {
        Name = "KOutbreakBC04MainForm",
        Key = 0xD829A7D6,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC04MainFound = new()
    {
        Name = "KOutbreakBC04MainFound",
        Key = 0x6F4FF4B8,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC04MainNumKOed = new()
    {
        Name = "KOutbreakBC04MainNumKOed",
        Key = 0x65AE4BC9,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static readonly BlockDefinition KOutbreakBC04MainTotalSpawns = new()
    {
        Name = "KOutbreakBC04MainTotalSpawns",
        Key = 0x718A1301,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak05
    public static BlockDefinition KOutbreakBC05MainCenterPos = new()
    {
        Name = "KOutbreakBC05MainCenterPos",
        Key = 0x71CEEF41,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC05MainDummyPos = new()
    {
        Name = "KOutbreakBC05MainDummyPos",
        Key = 0xB5DEA188,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC05MainSpecies = new()
    {
        Name = "KOutbreakBC05MainSpecies",
        Key = 0x849F074A,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC05MainForm = new()
    {
        Name = "KOutbreakBC05MainForm",
        Key = 0xD8200D11,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05MainFound = new()
    {
        Name = "KOutbreakBC05MainFound",
        Key = 0x6F3AF617,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC05MainNumKOed = new()
    {
        Name = "KOutbreakBC05MainNumKOed",
        Key = 0x65B70D0E,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05MainTotalSpawns = new()
    {
        Name = "KOutbreakBC05MainTotalSpawns",
        Key = 0x71926786,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak06
    public static BlockDefinition KOutbreakBC06MainCenterPos = new()
    {
        Name = "KOutbreakBC06MainCenterPos",
        Key = 0x71D2648C,
        Type = SCTypeCode.Array,
        Pointer= SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC06MainDummyPos = new()
    {
        Name = "KOutbreakBC06MainDummyPos",
        Key = 0xB5DABF7D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC06MainSpecies = new()
    {
        Name = "KOutbreakBC06MainSpecies",
        Key = 0x849D3767,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static BlockDefinition KOutbreakBC06MainForm = new()
    {
        Name = "KOutbreakBC06MainForm",
        Key = 0xD823EF1C,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06MainFound = new()
    {
        Name = "KOutbreakBC06MainFound",
        Key = 0x6F3EE5BA,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC06MainNumKOed = new()
    {
        Name = "KOutbreakBC06MainNumKOed",
        Key = 0x65B4D06B,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06MainTotalSpawns = new()
    {
        Name = "KOutbreakBC06MainTotalSpawns",
        Key = 0x718FBE23,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak07
    public static BlockDefinition KOutbreakBC07MainCenterPos = new()
    {
        Name = "KOutbreakBC07MainCenterPos",
        Key = 0x71CA1007,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC07MainDummyPos = new()
    {
        Name = "KOutbreakBC07MainDummyPos",
        Key = 0xB5D889A6,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC07MainSpecies = new()
    {
        Name = "KOutbreakBC07MainSpecies",
        Key = 0x84A58BEC,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static BlockDefinition KOutbreakBC07MainForm = new()
    {
        Name = "KOutbreakBC07MainForm",
        Key = 0xD81B2DD7,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07MainFound = new()
    {
        Name = "KOutbreakBC07MainFound",
        Key = 0x6F418851,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC07MainNumKOed = new()
    {
        Name = "KOutbreakBC07MainNumKOed",
        Key = 0x65BCB830,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07MainTotalSpawns = new()
    {
        Name = "KOutbreakBC07MainTotalSpawns",
        Key = 0x71987F68,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak08
    public static BlockDefinition KOutbreakBC08MainCenterPos = new()
    {
        Name = "KOutbreakBC08MainCenterPos",
        Key = 0x71CCB96A,
        Type = SCTypeCode.Array,
        Pointer= SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC08MainDummyPos = new()
    {
        Name = "KOutbreakBC08MainDummyPos",
        Key = 0xB5D506C3,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC08MainSpecies = new()
    {
        Name = "KOutbreakBC08MainSpecies",
        Key = 0x84A2F021,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static BlockDefinition KOutbreakBC08MainForm = new()
    {
        Name = "KOutbreakBC08MainForm",
        Key = 0xD81D6A7A,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08MainFound = new()
    {
        Name = "KOutbreakBC08MainFound",
        Key = 0x6F43B75C,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC08MainNumKOed = new()
    {
        Name = "KOutbreakBC08MainNumKOed",
        Key = 0x65BA8925,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08MainTotalSpawns = new()
    {
        Name = "KOutbreakBC08MainTotalSpawns",
        Key = 0x71949D5D,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak09
    public static BlockDefinition KOutbreakBC09MainCenterPos = new()
    {
        Name = "KOutbreakBC09MainCenterPos",
        Key = 0x71F18795,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC09MainDummyPos = new()
    {
        Name = "KOutbreakBC09MainDummyPos",
        Key = 0xB5E92BE4,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC09MainSpecies = new()
    {
        Name = "KOutbreakBC09MainSpecies",
        Key = 0x84C2791E,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static BlockDefinition KOutbreakBC09MainForm = new()
    {
        Name = "KOutbreakBC09MainForm",
        Key = 0xD842A565,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09MainFound = new()
    {
        Name = "KOutbreakBC09MainFound",
        Key = 0x6F5E67EB,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC09MainNumKOed = new()
    {
        Name = "KOutbreakBC09MainNumKOed",
        Key = 0x65954E3A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09MainTotalSpawns = new()
    {
        Name = "KOutbreakBC09MainTotalSpawns",
        Key = 0x719E3822,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak10
    public static BlockDefinition KOutbreakBC10MainCenterPos = new()
    {
        Name = "KOutbreakBC10MainCenterPos",
        Key = 0x71F42360,
        Type = SCTypeCode.Array,

        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC10MainDummyPos = new()
    {
        Name = "KOutbreakBC10MainDummyPos",
        Key = 0xB5E6FCD9,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC10MainSpecies = new()
    {
        Name = "KOutbreakBC10MainSpecies",
        Key = 0x84BFCFBB,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,

    };
    public static BlockDefinition KOutbreakBC10MainForm = new()
    {
        Name = "KOutbreakBC10MainForm",
        Key = 0xD8468770,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10MainFound = new()
    {
        Name = "KOutbreakBC10MainFound",
        Key = 0x6F60A48E,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC10MainNumKOed = new()
    {
        Name = "KOutbreakBC10MainNumKOed",
        Key = 0x65915E97,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10MainTotalSpawns = new()
    {
        Name = "KOutbreakBC10MainTotalSpawns",
        Key = 0x719A487F,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region Outbreak01DLC1
    public static readonly BlockDefinition KOutbreak01DLC1CenterPos = new()
    {
        Name = "KOutbreak01DLC1CenterPos",
        Key = 0x411A0C07,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01DLC1DummyPos = new()
    {
        Name = "KOutbreak01DLC1DummyPos",
        Key = 0x632EFBFE,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01DLC1Species = new()
    {
        Name = "KOutbreak01DLC1Species",
        Key = 0x37E55F64,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01DLC1Form = new()
    {
        Name = "KOutbreak01DLC1Form",
        Key = 0x69A930AB,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01DLC1Found = new()
    {
        Name = "KOutbreak01DLC1Found",
        Key = 0x7B688081,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01DLC1NumKOed = new()
    {
        Name = "KOutbreak01DLC1NumKOed",
        Key = 0xB29D7978,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01DLC1TotalSpawns = new()
    {
        Name = "KOutbreak01DLC1TotalSpawns",
        Key = 0x9E0CEC77,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak02DLC1
    public static readonly BlockDefinition KOutbreak02DLC1CenterPos = new()
    {
        Name = "KOutbreak02DLC1CenterPos",
        Key = 0x411CB56A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02DLC1DummyPos = new()
    {
        Name = "KOutbreak02DLC1DummyPos",
        Key = 0x632D2C1B,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02DLC1Species = new()
    {
        Name = "KOutbreak02DLC1Species",
        Key = 0x37E33059,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02DLC1Form = new()
    {
        Name = "KOutbreak02DLC1Form",
        Key = 0x69AD204E,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02DLC1Found = new()
    {
        Name = "KOutbreak02DLC1Found",
        Key = 0x7B6A42CC,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02DLC1NumKOed = new()
    {
        Name = "KOutbreak02DLC1NumKOed",
        Key = 0xB29ADDAD,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02DLC1TotalSpawns = new()
    {
        Name = "KOutbreak02DLC1TotalSpawns",
        Key = 0x9E10DC1A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak03DLC1
    public static readonly BlockDefinition KOutbreak03DLC1CenterPos = new()
    {
        Name = "KOutbreak03DLC1CenterPos",
        Key = 0x411EEB41,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03DLC1DummyPos = new()
    {
        Name = "KOutbreak03DLC1DummyPos",
        Key = 0x633580A0,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03DLC1Species = new()
    {
        Name = "KOutbreak03DLC1Species",
        Key = 0x37DFB442,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03DLC1Form = new()
    {
        Name = "KOutbreak03DLC1Form",
        Key = 0x69AEE965,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03DLC1Found = new()
    {
        Name = "KOutbreak03DLC1Found",
        Key = 0x7B61EE47,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03DLC1NumKOed = new()
    {
        Name = "KOutbreak03DLC1NumKOed",
        Key = 0xB298A7D6,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03DLC1TotalSpawns = new()
    {
        Name = "KOutbreak03DLC1TotalSpawns",
        Key = 0x9E12A531,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak04DLC1
    public static readonly BlockDefinition KOutbreak04DLC1CenterPos = new()
    {
        Name = "KOutbreak04DLC1CenterPos",
        Key = 0x4122608C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04DLC1DummyPos = new()
    {
        Name = "KOutbreak04DLC1DummyPos",
        Key = 0x6332E4D5,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04DLC1Species = new()
    {
        Name = "KOutbreak04DLC1Species",
        Key = 0x37DD779F,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04DLC1Form = new()
    {
        Name = "KOutbreak04DLC1Form",
        Key = 0x69B2CB70,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04DLC1Found = new()
    {
        Name = "KOutbreak04DLC1Found",
        Key = 0x7B6497AA,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04DLC1NumKOed = new()
    {
        Name = "KOutbreak04DLC1NumKOed",
        Key = 0xB294B833,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04DLC1TotalSpawns = new()
    {
        Name = "KOutbreak04DLC1TotalSpawns",
        Key = 0x9E16873C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region BCATOutbreak01DLC1
    public static BlockDefinition KOutbreakBC01DLC1CenterPos = new()
    {
        Name = "KOutbreakBC01DLC1CenterPos",
        Key = 0xB3C20007,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC1DummyPos = new()
    {
        Name = "KOutbreakBC01DLC1DummyPos",
        Key = 0xB2E537FE,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC01DLC1Species = new()
    {
        Name = "KOutbreakBC01DLC1Species",
        Key = 0x0F4D3B64,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC01DLC1Form = new()
    {
        Name = "KOutbreakBC01DLC1Form",
        Key = 0x41110CAB,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC1Found = new()
    {
        Name = "KOutbreakBC01DLC1Found",
        Key = 0x52D05C81,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC01DLC1NumKOed = new()
    {
        Name = "KOutbreakBC01DLC1NumKOed",
        Key = 0xAA733578,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC01DLC1TotalSpawns",
        Key = 0x95EC433C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak02DLC1
    public static BlockDefinition KOutbreakBC02DLC1CenterPos = new()
    {
        Name = "KOutbreakBC02DLC1CenterPos",
        Key = 0xB3C4A96A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC1DummyPos = new()
    {
        Name = "KOutbreakBC02DLC1DummyPos",
        Key = 0xB2E3681B,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC02DLC1Species = new()
    {
        Name = "KOutbreakBC02DLC1Species",
        Key = 0x0F4B0C59,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC02DLC1Form = new()
    {
        Name = "KOutbreakBC02DLC1Form",
        Key = 0x4114FC4E,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC1Found = new()
    {
        Name = "KOutbreakBC02DLC1Found",
        Key = 0x52D21ECC,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC02DLC1NumKOed = new()
    {
        Name = "KOutbreakBC02DLC1NumKOed",
        Key = 0xAA7099AD,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC02DLC1TotalSpawns",
        Key = 0x95E86131,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak03DLC1
    public static BlockDefinition KOutbreakBC03DLC1CenterPos = new()
    {
        Name = "KOutbreakBC03DLC1CenterPos",
        Key = 0xB3C6DF41,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC1DummyPos = new()
    {
        Name = "KOutbreakBC03DLC1DummyPos",
        Key = 0xB2EBBCA0,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC03DLC1Species = new()
    {
        Name = "KOutbreakBC03DLC1Species",
        Key = 0x0F479042,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC03DLC1Form = new()
    {
        Name = "KOutbreakBC03DLC1Form",
        Key = 0x4116C565,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC1Found = new()
    {
        Name = "KOutbreakBC03DLC1Found",
        Key = 0x52C9CA47,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC03DLC1NumKOed = new()
    {
        Name = "KOutbreakBC03DLC1NumKOed",
        Key = 0xAA6E63D6,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC03DLC1TotalSpawns",
        Key = 0x95E6981A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak04DLC1
    public static BlockDefinition KOutbreakBC04DLC1CenterPos = new()
    {
        Name = "KOutbreakBC04DLC1CenterPos",
        Key = 0xB3CA548C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC1DummyPos = new()
    {
        Name = "KOutbreakBC04DLC1DummyPos",
        Key = 0xB2E920D5,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC04DLC1Species = new()
    {
        Name = "KOutbreakBC04DLC1Species",
        Key = 0x0F45539F,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC04DLC1Form = new()
    {
        Name = "KOutbreakBC04DLC1Form",
        Key = 0x411AA770,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC1Found = new()
    {
        Name = "KOutbreakBC04DLC1Found",
        Key = 0x52CC73AA,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC04DLC1NumKOed = new()
    {
        Name = "KOutbreakBC04DLC1NumKOed",
        Key = 0xAA6A7433,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC04DLC1TotalSpawns",
        Key = 0x95E2A877,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak05DLC1
    public static BlockDefinition KOutbreakBC05DLC1CenterPos = new()
    {
        Name = "KOutbreakBC05DLC1CenterPos",
        Key = 0xB3CC8A63,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC1DummyPos = new()
    {
        Name = "KOutbreakBC05DLC1DummyPos",
        Key = 0xB2DAADA2,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC05DLC1Species = new()
    {
        Name = "KOutbreakBC05DLC1Species",
        Key = 0x0F5978C0,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC05DLC1Form = new()
    {
        Name = "KOutbreakBC05DLC1Form",
        Key = 0x4106824F,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC1Found = new()
    {
        Name = "KOutbreakBC05DLC1Found",
        Key = 0x52DAE6DD,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC05DLC1NumKOed = new()
    {
        Name = "KOutbreakBC05DLC1NumKOed",
        Key = 0xAA68AB1C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC05DLC1TotalSpawns",
        Key = 0x95F6CD98,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak06DLC1
    public static BlockDefinition KOutbreakBC06DLC1CenterPos = new()
    {
        Name = "KOutbreakBC06DLC1CenterPos",
        Key = 0xB3CF33C6,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC1DummyPos = new()
    {
        Name = "KOutbreakBC06DLC1DummyPos",
        Key = 0xB2D6BDFF,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC06DLC1Species = new()
    {
        Name = "KOutbreakBC06DLC1Species",
        Key = 0x0F560375,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC06DLC1Form = new()
    {
        Name = "KOutbreakBC06DLC1Form",
        Key = 0x41085232,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC1Found = new()
    {
        Name = "KOutbreakBC06DLC1Found",
        Key = 0x52DEC8E8,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC06DLC1NumKOed = new()
    {
        Name = "KOutbreakBC06DLC1NumKOed",
        Key = 0xAA64C911,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC06DLC1TotalSpawns",
        Key = 0x95F50B4D,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak07DLC1
    public static BlockDefinition KOutbreakBC07DLC1CenterPos = new()
    {
        Name = "KOutbreakBC07DLC1CenterPos",
        Key = 0xB3D31C9D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC1DummyPos = new()
    {
        Name = "KOutbreakBC07DLC1DummyPos",
        Key = 0xB2DF7F44,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC07DLC1Species = new()
    {
        Name = "KOutbreakBC07DLC1Species",
        Key = 0x0F53CD9E,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC07DLC1Form = new()
    {
        Name = "KOutbreakBC07DLC1Form",
        Key = 0x410C3B09,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC1Found = new()
    {
        Name = "KOutbreakBC07DLC1Found",
        Key = 0x52D607A3,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC07DLC1NumKOed = new()
    {
        Name = "KOutbreakBC07DLC1NumKOed",
        Key = 0xAA62267A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC07DLC1TotalSpawns",
        Key = 0x95F12276,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak08DLC1
    public static BlockDefinition KOutbreakBC08DLC1CenterPos = new()
    {
        Name = "KOutbreakBC08DLC1CenterPos",
        Key = 0xB3D54BA8,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC1DummyPos = new()
    {
        Name = "KOutbreakBC08DLC1DummyPos",
        Key = 0xB2DD5039,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC08DLC1Species = new()
    {
        Name = "KOutbreakBC08DLC1Species",
        Key = 0x0F51243B,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC08DLC1Form = new()
    {
        Name = "KOutbreakBC08DLC1Form",
        Key = 0x410E6A14,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC1Found = new()
    {
        Name = "KOutbreakBC08DLC1Found",
        Key = 0x52D8B106,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC08DLC1NumKOed = new()
    {
        Name = "KOutbreakBC08DLC1NumKOed",
        Key = 0xAA5FE9D7,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC08DLC1TotalSpawns",
        Key = 0x95EEE5D3,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak09DLC1
    public static BlockDefinition KOutbreakBC09DLC1CenterPos = new()
    {
        Name = "KOutbreakBC09DLC1CenterPos",
        Key = 0xB3D8C7BF,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC1DummyPos = new()
    {
        Name = "KOutbreakBC09DLC1DummyPos",
        Key = 0xB2CEDD06,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC09DLC1Species = new()
    {
        Name = "KOutbreakBC09DLC1Species",
        Key = 0x0F36E06C,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC09DLC1Form = new()
    {
        Name = "KOutbreakBC09DLC1Form",
        Key = 0x40F9D833,
        Type = SCTypeCode.Byte,
        Pointer =SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC1Found = new()
    {
        Name = "KOutbreakBC09DLC1Found",
        Key = 0x52E72439,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC09DLC1NumKOed = new()
    {
        Name = "KOutbreakBC09DLC1NumKOed",
        Key = 0xAA8B4370,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC09DLC1TotalSpawns",
        Key = 0x960377B4,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak10DLC1
    public static BlockDefinition KOutbreakBC10DLC1CenterPos = new()
    {
        Name = "KOutbreakBC10DLC1CenterPos",
        Key = 0xB3DB0462,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,

    };
    public static readonly BlockDefinition KOutbreakBC10DLC1DummyPos = new()
    {
        Name = "KOutbreakBC10DLC1DummyPos",
        Key = 0xB2CC33A3,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC10DLC1Species = new()
    {
        Name = "KOutbreakBC10DLC1Species",
        Key = 0x0F3444A1,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC10DLC1Form = new()
    {
        Name = "KOutbreakBC10DLC1Form",
        Key = 0x40FDC7D6,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10DLC1Found = new()
    {
        Name = "KOutbreakBC10DLC1Found",
        Key = 0x52E95344,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC10DLC1NumKOed = new()
    {
        Name = "KOutbreakBC10DLC1NumKOed",
        Key = 0xAA876165,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10DLC1TotalSpawns = new()
    {
        Name = "KOutbreakBC10DLC1TotalSpawns",
        Key = 0x95FF95A9,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region Outbreak01DLC2
    public static readonly BlockDefinition KOutbreak01DLC2CenterPos = new()
    {
        Name = "KOutbreak01DLC2CenterPos",
        Key = 0xCE463C0C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01DLC2DummyPos = new()
    {
        Name = "KOutbreak01DLC2DummyPos",
        Key = 0x0B0C71CB,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak01DLC2Species = new()
    {
        Name = "KOutbreak01DLC2Species",
        Key = 0xB8E99C8D,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01DLC2Form = new()
    {
        Name = "KOutbreak01DLC2Form",
        Key = 0xEFA6983A,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01DLC2Found = new()
    {
        Name = "KOutbreak01DLC2Found",
        Key = 0x32074910,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak01DLC2NumKOed = new()
    {
        Name = "KOutbreak01DLC2NumKOed",
        Key = 0x4EF9BC25,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak01DLC2TotalSpawns = new()
    {
        Name = "KOutbreak01DLC2TotalSpawns",
        Key = 0x4385E0AD,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak02DLC2
    public static readonly BlockDefinition KOutbreak02DLC2CenterPos = new()
    {
        Name = "KOutbreak02DLC2CenterPos",
        Key = 0xCE42C6C1,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02DLC2DummyPos = new()
    {
        Name = "KOutbreak02DLC2DummyPos",
        Key = 0x0B10616E,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak02DLC2Species = new()
    {
        Name = "KOutbreak02DLC2Species",
        Key = 0xB8ED11D8,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02DLC2Form = new()
    {
        Name = "KOutbreak02DLC2Form",
        Key = 0xEFA2A897,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02DLC2Found = new()
    {
        Name = "KOutbreak02DLC2Found",
        Key = 0x32051A05,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak02DLC2NumKOed = new()
    {
        Name = "KOutbreak02DLC2NumKOed",
        Key = 0x4EFBEB30,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak02DLC2TotalSpawns = new()
    {
        Name = "KOutbreak02DLC2TotalSpawns",
        Key = 0x43887C78,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak03DLC2
    public static readonly BlockDefinition KOutbreak03DLC2CenterPos = new()
    {
        Name = "KOutbreak03DLC2CenterPos",
        Key = 0xCE4090EA,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03DLC2DummyPos = new()
    {
        Name = "KOutbreak03DLC2DummyPos",
        Key = 0x0B130405,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak03DLC2Species = new()
    {
        Name = "KOutbreak03DLC2Species",
        Key = 0xB8E37713,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03DLC2Form = new()
    {
        Name = "KOutbreak03DLC2Form",
        Key = 0xEFAB69DC,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03DLC2Found = new()
    {
        Name = "KOutbreak03DLC2Found",
        Key = 0x3202776E,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak03DLC2NumKOed = new()
    {
        Name = "KOutbreak03DLC2NumKOed",
        Key = 0x4EF4036B,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak03DLC2TotalSpawns = new()
    {
        Name = "KOutbreak03DLC2TotalSpawns",
        Key = 0x437FBB33,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak04DLC2
    public static readonly BlockDefinition KOutbreak04DLC2CenterPos = new()
    {
        Name = "KOutbreak04DLC2CenterPos",
        Key = 0xCE3DE787,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04DLC2DummyPos = new()
    {
        Name = "KOutbreak04DLC2DummyPos",
        Key = 0x0B153310,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak04DLC2Species = new()
    {
        Name = "KOutbreak04DLC2Species",
        Key = 0xB8E766B6,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04DLC2Form = new()
    {
        Name = "KOutbreak04DLC2Form",
        Key = 0xEFA93AD1,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04DLC2Found = new()
    {
        Name = "KOutbreak04DLC2Found",
        Key = 0x31FE87CB,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak04DLC2NumKOed = new()
    {
        Name = "KOutbreak04DLC2NumKOed",
        Key = 0x4EF6400E,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak04DLC2TotalSpawns = new()
    {
        Name = "KOutbreak04DLC2TotalSpawns",
        Key = 0x4383AAD6,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region Outbreak05DLC2
    public static readonly BlockDefinition KOutbreak05DLC2CenterPos = new()
    {
        Name = "KOutbreak05DLC2CenterPos",
        Key = 0xCE513328,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak05DLC2DummyPos = new()
    {
        Name = "KOutbreak05DLC2DummyPos",
        Key = 0x0B01E76F,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };

    public static readonly BlockDefinition KOutbreak05DLC2Species = new()
    {
        Name = "KOutbreak05DLC2Species",
        Key = 0xB8DEA571,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak05DLC2Form = new()
    {
        Name = "KOutbreak05DLC2Form",
        Key = 0xEFB12296,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak05DLC2Found = new()
    {
        Name = "KOutbreak05DLC2Found",
        Key = 0x31FCBEB4,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };

    public static readonly BlockDefinition KOutbreak05DLC2NumKOed = new()
    {
        Name = "KOutbreak05DLC2NumKOed",
        Key = 0x4EED7EC9,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };

    public static readonly BlockDefinition KOutbreak05DLC2TotalSpawns = new()
    {
        Name = "KOutbreak05DLC2TotalSpawns",
        Key = 0x437A1011,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region BCATOutbreak01DLC2
    public static BlockDefinition KOutbreakBC01DLC2CenterPos = new()
    {
        Name = "KOutbreakBC01DLC2CenterPos",
        Key = 0xE623D9F6,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC2DummyPos = new()
    {
        Name = "KOutbreakBC01DLC2DummyPos",
        Key = 0xB1E70E4D,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC01DLC2Species = new()
    {
        Name = "KOutbreakBC01DLC2Species",
        Key = 0x03B50A2B,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC01DLC2Form = new()
    {
        Name = "KOutbreakBC01DLC2Form",
        Key = 0x9F47C0A8,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC2Found = new()
    {
        Name = "KOutbreakBC01DLC2Found",
        Key = 0x57C23026,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC01DLC2NumKOed = new()
    {
        Name = "KOutbreakBC01DLC2NumKOed",
        Key = 0x6CB77613,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC01DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC01DLC2TotalSpawns",
        Key = 0xCDB0C887,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak02DLC2
    public static BlockDefinition KOutbreakBC02DLC2CenterPos = new()
    {
        Name = "KOutbreakBC02DLC2CenterPos",
        Key = 0xE6219D53,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC2DummyPos = new()
    {
        Name = "KOutbreakBC02DLC2DummyPos",
        Key = 0xB1E8D098,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC02DLC2Species = new()
    {
        Name = "KOutbreakBC02DLC2Species",
        Key = 0x03B8F9CE,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC02DLC2Form = new()
    {
        Name = "KOutbreakBC02DLC2Form",
        Key = 0x9F45919D,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC2Found = new()
    {
        Name = "KOutbreakBC02DLC2Found",
        Key = 0x57BEAD43,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC02DLC2NumKOed = new()
    {
        Name = "KOutbreakBC02DLC2NumKOed",
        Key = 0x6CBB65B6,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC02DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC02DLC2TotalSpawns",
        Key = 0xCDB371EA,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak03DLC2
    public static BlockDefinition KOutbreakBC03DLC2CenterPos = new()
    {
        Name = "KOutbreakBC03DLC2CenterPos",
        Key = 0xE6298518,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC2DummyPos = new()
    {
        Name = "KOutbreakBC03DLC2DummyPos",
        Key = 0xB1E0E8D3,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC03DLC2Species = new()
    {
        Name = "KOutbreakBC03DLC2Species",
        Key = 0x03BAC2E5,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC03DLC2Form = new()
    {
        Name = "KOutbreakBC03DLC2Form",
        Key = 0x9F41A8C6,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC2Found = new()
    {
        Name = "KOutbreakBC03DLC2Found",
        Key = 0x57C84808,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC03DLC2NumKOed = new()
    {
        Name = "KOutbreakBC03DLC2NumKOed",
        Key = 0x6CBD9B8D,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC03DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC03DLC2TotalSpawns",
        Key = 0xCDB5A7C1,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak04DLC2
    public static BlockDefinition KOutbreakBC04DLC2CenterPos = new()
    {
        Name = "KOutbreakBC04DLC2CenterPos",
        Key = 0xE627C2CD,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC2DummyPos = new()
    {
        Name = "KOutbreakBC04DLC2DummyPos",
        Key = 0xB1E32576,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC04DLC2Species = new()
    {
        Name = "KOutbreakBC04DLC2Species",
        Key = 0x03BEA4F0,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC04DLC2Form = new()
    {
        Name = "KOutbreakBC04DLC2Form",
        Key = 0x9F3EFF63,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC2Found = new()
    {
        Name = "KOutbreakBC04DLC2Found",
        Key = 0x57C465FD,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC04DLC2NumKOed = new()
    {
        Name = "KOutbreakBC04DLC2NumKOed",
        Key = 0x6CC110D8,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC04DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC04DLC2TotalSpawns",
        Key = 0xCDB91D0C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak05DLC2
    public static BlockDefinition KOutbreakBC05DLC2CenterPos = new()
    {
        Name = "KOutbreakBC05DLC2CenterPos",
        Key = 0xE6194F9A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC2DummyPos = new()
    {
        Name = "KOutbreakBC05DLC2DummyPos",
        Key = 0xB1DA6431,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC05DLC2Species = new()
    {
        Name = "KOutbreakBC05DLC2Species",
        Key = 0x03AA7FCF,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC05DLC2Form = new()
    {
        Name = "KOutbreakBC05DLC2Form",
        Key = 0x9F3CC98C,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC2Found = new()
    {
        Name = "KOutbreakBC05DLC2Found",
        Key = 0x57B5F2CA,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC05DLC2NumKOed = new()
    {
        Name = "KOutbreakBC05DLC2NumKOed",
        Key = 0x6CACEBB7,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC05DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC05DLC2TotalSpawns",
        Key = 0xCDBB52E3,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak06DLC2
    public static BlockDefinition KOutbreakBC06DLC2CenterPos = new()
    {
        Name = "KOutbreakBC06DLC2CenterPos",
        Key = 0xE6155FF7,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC2DummyPos = new()
    {
        Name = "KOutbreakBC06DLC2DummyPos",
        Key = 0xB1DE463C,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC06DLC2Species = new()
    {
        Name = "KOutbreakBC06DLC2Species",
        Key = 0x03AC4FB2,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC06DLC2Form = new()
    {
        Name = "KOutbreakBC06DLC2Form",
        Key = 0x9F395441,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC2Found = new()
    {
        Name = "KOutbreakBC06DLC2Found",
        Key = 0x57B422E7,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC06DLC2NumKOed = new()
    {
        Name = "KOutbreakBC06DLC2NumKOed",
        Key = 0x6CAF285A,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC06DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC06DLC2TotalSpawns",
        Key = 0xCDBDFC46,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak07DLC2
    public static BlockDefinition KOutbreakBC07DLC2CenterPos = new()
    {
        Name = "KOutbreakBC07DLC2CenterPos",
        Key = 0xE61EFABC,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC2DummyPos = new()
    {
        Name = "KOutbreakBC07DLC2DummyPos",
        Key = 0xB1D4AB77,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC07DLC2Species = new()
    {
        Name = "KOutbreakBC07DLC2Species",
        Key = 0x03B03889,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC07DLC2Form = new()
    {
        Name = "KOutbreakBC07DLC2Form",
        Key = 0x9F371E6A,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC2Found = new()
    {
        Name = "KOutbreakBC07DLC2Found",
        Key = 0x57BC776C,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC07DLC2NumKOed = new()
    {
        Name = "KOutbreakBC07DLC2NumKOed",
        Key = 0x6CB2A471,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC07DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC07DLC2TotalSpawns",
        Key = 0xCDC1E51D,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak08DLC2
    public static BlockDefinition KOutbreakBC08DLC2CenterPos = new()
    {
        Name = "KOutbreakBC08DLC2CenterPos",
        Key = 0xE61B18B1,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC2DummyPos = new()
    {
        Name = "KOutbreakBC08DLC2DummyPos",
        Key = 0xB1D89B1A,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC08DLC2Species = new()
    {
        Name = "KOutbreakBC08DLC2Species",
        Key = 0x03B26794,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC08DLC2Form = new()
    {
        Name = "KOutbreakBC08DLC2Form",
        Key = 0x9F347507,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC2Found = new()
    {
        Name = "KOutbreakBC08DLC2Found",
        Key = 0x57B9DBA1,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC08DLC2NumKOed = new()
    {
        Name = "KOutbreakBC08DLC2NumKOed",
        Key = 0x6CB4D37C,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC08DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC08DLC2TotalSpawns",
        Key = 0xCDC41428,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak09DLC2
    public static BlockDefinition KOutbreakBC09DLC2CenterPos = new()
    {
        Name = "KOutbreakBC09DLC2CenterPos",
        Key = 0xE63BE7EE,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC2DummyPos = new()
    {
        Name = "KOutbreakBC09DLC2DummyPos",
        Key = 0xB1FDD605,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC09DLC2Species = new()
    {
        Name = "KOutbreakBC09DLC2Species",
        Key = 0x039DD5B3,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC09DLC2Form = new()
    {
        Name = "KOutbreakBC09DLC2Form",
        Key = 0x9F5E8860,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC2Found = new()
    {
        Name = "KOutbreakBC09DLC2Found",
        Key = 0x57D9649E,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC09DLC2NumKOed = new()
    {
        Name = "KOutbreakBC09DLC2NumKOed",
        Key = 0x6CCF840B,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC09DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC09DLC2TotalSpawns",
        Key = 0xCDC7903F,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion
    #region BCATOutbreak10DLC2
    public static BlockDefinition KOutbreakBC10DLC2CenterPos = new()
    {
        Name = "KOutbreakBC10DLC2CenterPos",
        Key = 0xE637F84B,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static readonly BlockDefinition KOutbreakBC10DLC2DummyPos = new()
    {
        Name = "KOutbreakBC10DLC2DummyPos",
        Key = 0xB2000510,
        Type = SCTypeCode.Array,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 12,
    };
    public static BlockDefinition KOutbreakBC10DLC2Species = new()
    {
        Name = "KOutbreakBC10DLC2Species",
        Key = 0x03A1C556,
        Type = SCTypeCode.UInt32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static BlockDefinition KOutbreakBC10DLC2Form = new()
    {
        Name = "KOutbreakBC10DLC2Form",
        Key = 0x9F5BEC95,
        Type = SCTypeCode.Byte,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10DLC2Found = new()
    {
        Name = "KOutbreakBC10DLC2Found",
        Key = 0x57D6BB3B,
        Type = SCTypeCode.Bool1,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 1,
    };
    public static BlockDefinition KOutbreakBC10DLC2NumKOed = new()
    {
        Name = "KOutbreakBC10DLC2NumKOed",
        Key = 0x6CD1C0AE,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
    };
    public static readonly BlockDefinition KOutbreakBC10DLC2TotalSpawns = new()
    {
        Name = "KOutbreakBC10DLC2TotalSpawns",
        Key = 0xCDC9CCE2,
        Type = SCTypeCode.Int32,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 4,
    };
    #endregion

    #region KBCATOutbreaks
    public static readonly BlockDefinition KBCATOutbreakZonesPaldea = new()
    {
        Name = "KBCATOutbreakZonesPaldea",
        Key = 0x3FDC5DFF,
        Type = SCTypeCode.Object,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x300,
    };

    public static readonly BlockDefinition KBCATOutbreakZonesKitakami = new()
    {
        Name = "KBCATOutbreakZonesKitakami",
        Key = 0xF9F156A3,
        Type = SCTypeCode.Object,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x300,
    };

    public static readonly BlockDefinition KBCATOutbreakZonesBlueberry = new()
    {
        Name = "KBCATOutbreakZonesBlueberry",
        Key = 0x1B45E41C,
        Type = SCTypeCode.Object,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x300,
    };

    public static readonly BlockDefinition KBCATOutbreakPokeData = new()
    {
        Name = "KBCATOutbreakPokeData",
        Key = 0x6C1A131B,
        Type = SCTypeCode.Object,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0xE18,
    };

    public static readonly BlockDefinition KBCATOutbreakEnabled = new()
    {
        Name = "KBCATOutbreakEnabled",
        Key = 0x61552076,
        Type = SCTypeCode.Bool2,
        Pointer = SaveBlockKeyPointer,
        IsEncrypted = true,
        Size = 0x01,
    };
    #endregion
}