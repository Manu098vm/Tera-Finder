﻿using PKHeX.Core;

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
    #endregion

    #region DataBCAT
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

    #region Outbreak1
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

    #region Outbreak2
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

    #region Outbreak3
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

    #region Outbreak4
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

    #region Outbreak5
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

    #region Outbreak6
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

    #region Outbreak7
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

    #region Outbreak8
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