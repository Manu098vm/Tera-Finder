using PKHeX.Core;
using System.Buffers.Binary;
using static PKHeX.Core.AbilityPermission;

//Extension of https://github.com/kwsch/PKHeX/blob/master/PKHeX.Core/Legality/Encounters/EncounterStatic/EncounterTera9.cs
namespace TeraFinder.Core;

/// <summary>
/// Generation 9 Tera Raid Encounter
/// </summary>
public sealed record EncounterTera9 : IEncounterable, IEncounterConvertible<PK9>, ITeraRaid9, IExtendedTeraRaid9, IMoveset, IFlawlessIVCount, IFixedGender
{
    //TeraFinder Serialization
    public uint Identifier { get; private init; }
    public ulong FixedRewardHash { get; private init; }
    public ulong LotteryRewardHash { get; private init; }
    public int Item { get; private init; }
    public required ExtraMoves ExtraMoves { get; init; }
    public required Nature Nature { get; init; }
    public required SizeType9 ScaleType { get; init; }
    public required byte Scale { get; init; }
    public required IndividualValueSet IVs { get; init; }

    //PKHeX Serialization
    public int Generation => 9;
    public EntityContext Context => EntityContext.Gen9;
    public GameVersion Version => GameVersion.SV;
    int ILocation.Location => Location;
    public const ushort Location = Locations.TeraCavern9;
    public bool IsDistribution => Index != 0;
    public Ball FixedBall => Ball.None;
    public bool EggEncounter => false;
    public bool IsShiny => Shiny == Shiny.Always;
    public int EggLocation => 0;

    public required ushort Species { get; init; }
    public required byte Form { get; init; }
    public required byte Gender { get; init; }
    public required AbilityPermission Ability { get; init; }
    public required byte FlawlessIVCount { get; init; }
    public required Shiny Shiny { get; init; }
    public required byte Level { get; init; }
    public required Moveset Moves { get; init; }
    public required GemType TeraType { get; init; }
    public required byte Index { get; init; }
    public required byte Stars { get; init; }
    public required byte RandRate { get; init; } // weight chance of this encounter
    public required short RandRateMinScarlet { get; init; } // weight chance total of all lower index encounters, for Scarlet
    public required short RandRateMinViolet { get; init; } // weight chance total of all lower index encounters, for Violet
    public bool IsAvailableHostScarlet => RandRateMinScarlet != -1;
    public bool IsAvailableHostViolet => RandRateMinViolet != -1;
    public required TeraRaidMapParent Map { get; init; }

    public string Name => "Tera Raid Encounter";
    public string LongName => Name;
    public byte LevelMin => Level;
    public byte LevelMax => Level;

    public bool CanBeEncountered(uint seed) => Tera9RNG.IsMatchStarChoice(seed, Stars, RandRate, RandRateMinScarlet, RandRateMinViolet, Map);

    /// <summary>
    /// Fetches the rate sum for the base ROM raid, depending on star count.
    /// </summary>
    /// <param name="star">Raid Difficulty</param>
    /// <param name="map">Map the encounter originates on.</param>
    /// <returns>Total rate value the game uses to call rand(x) with.</returns>
    public static short GetRateTotalSL(int star, TeraRaidMapParent map) => map switch
    {
        TeraRaidMapParent.Paldea => GetRateTotalBaseSL(star),
        TeraRaidMapParent.Kitakami => GetRateTotalKitakamiSL(star),
        TeraRaidMapParent.Blueberry => GetRateTotalBlueberry(star),
        _ => 0,
    };

    /// <inheritdoc cref="GetRateTotalSL(int, TeraRaidMapParent)"/>"/>
    public static short GetRateTotalVL(int star, TeraRaidMapParent map) => map switch
    {
        TeraRaidMapParent.Paldea => GetRateTotalBaseVL(star),
        TeraRaidMapParent.Kitakami => GetRateTotalKitakamiVL(star),
        TeraRaidMapParent.Blueberry => GetRateTotalBlueberry(star),
        _ => 0,
    };

    public static short GetRateTotalBaseSL(int star) => star switch
    {
        1 => 5800,
        2 => 5300,
        3 => 7400,
        4 => 8800, // Scarlet has one more encounter.
        5 => 9100,
        6 => 6500,
        _ => 0,
    };

    public static short GetRateTotalBaseVL(int star) => star switch
    {
        1 => 5800,
        2 => 5300,
        3 => 7400,
        4 => 8700, // Violet has one less encounter.
        5 => 9100,
        6 => 6500,
        _ => 0,
    };

    public static short GetRateTotalKitakamiSL(int star) => star switch
    {
        1 => 1500,
        2 => 1500,
        3 => 2500,
        4 => 2100,
        5 => 2250,
        6 => 2475, // -99
        _ => 0,
    };

    public static short GetRateTotalKitakamiVL(int star) => star switch
    {
        1 => 1500,
        2 => 1500,
        3 => 2500,
        4 => 2100,
        5 => 2250,
        6 => 2574, // +99
        _ => 0,
    };

    public static short GetRateTotalBlueberry(int star) => star switch
    {
        1 => 1100,
        2 => 1100,
        3 => 2000,
        4 => 1900,
        5 => 2100,
        6 => 2600,
        _ => 0,
    };

    public ushort GetRandRateTotalScarlet(int stage) => 0;
    public ushort GetRandRateTotalViolet(int stage) => 0;
    public ushort GetRandRateMinScarlet(int stage) => (ushort)RandRateMinScarlet;
    public ushort GetRandRateMinViolet(int stage) => (ushort)RandRateMinViolet;

    public static EncounterTera9[] GetArray(ReadOnlySpan<byte> data, TeraRaidMapParent map)
    {
        var count = data.Length / SerializedSize;
        var result = new EncounterTera9[count];
        for (int i = 0; i < result.Length; i++)
            result[i] = ReadEncounter(data.Slice(i * SerializedSize, SerializedSize), map);
        return result;
    }

    private const int SerializedSize = 0x3C;

    private static EncounterTera9 ReadEncounter(ReadOnlySpan<byte> data, TeraRaidMapParent map) => new()
    {
        Species = BinaryPrimitives.ReadUInt16LittleEndian(data),
        Form = data[0x02],
        Gender = (byte)(data[0x03] - 1),
        Ability = GetAbility(data[0x04]),
        FlawlessIVCount = data[5],
        Shiny = data[0x06] switch { 0 => Shiny.Random, 1 => Shiny.Never, 2 => Shiny.Always, _ => throw new ArgumentOutOfRangeException(nameof(data)) },
        Level = data[0x07],
        Moves = new Moveset(
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x08..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x0A..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x0C..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x0E..])),
        TeraType = (GemType)data[0x10],
        Index = data[0x11],
        Stars = data[0x12],
        RandRate = data[0x13],
        RandRateMinScarlet = BinaryPrimitives.ReadInt16LittleEndian(data[0x14..]),
        RandRateMinViolet = BinaryPrimitives.ReadInt16LittleEndian(data[0x16..]),
        Identifier = BinaryPrimitives.ReadUInt32LittleEndian(data[0x18..]),
        FixedRewardHash = BinaryPrimitives.ReadUInt64LittleEndian(data[0x1C..]),
        LotteryRewardHash = BinaryPrimitives.ReadUInt64LittleEndian(data[0x24..]),
        Item = (int)BinaryPrimitives.ReadUInt32LittleEndian(data[0x2C..]),
        ExtraMoves = new ExtraMoves(BinaryPrimitives.ReadUInt16LittleEndian(data[0x30..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x32..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x34..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x36..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x38..]),
            BinaryPrimitives.ReadUInt16LittleEndian(data[0x3A..])),
        IVs = default,
        Nature = Nature.Random,
        ScaleType = SizeType9.RANDOM,
        Scale = 0,
        Map = map,
    };

    private static AbilityPermission GetAbility(byte b) => b switch
    {
        0 => Any12,
        1 => Any12H,
        2 => OnlyFirst,
        3 => OnlySecond,
        4 => OnlyHidden,
        _ => throw new ArgumentOutOfRangeException(nameof(b), b, null),
    };

    #region Generating
    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria) => ConvertToPKM(tr, criteria);
    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr);
    public PK9 ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr, EncounterCriteria.Unrestricted);
    public PK9 ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria)
    {
        int lang = (int)Language.GetSafeLanguage(Generation, (LanguageID)tr.Language);
        var version = this.GetCompatibleVersion((GameVersion)tr.Game);
        var pi = PersonalTable.SV[Species, Form];
        var pk = new PK9
        {
            Language = lang,
            Species = Species,
            Form = Form,
            CurrentLevel = LevelMin,
            OT_Friendship = pi.BaseFriendship,
            Met_Location = Location,
            Met_Level = LevelMin,
            MetDate = EncounterDate.GetDateSwitch(),
            Version = (byte)version,
            Ball = (byte)Ball.Poke,

            Nickname = SpeciesName.GetSpeciesNameGeneration(Species, lang, Generation),
            Obedience_Level = LevelMin,
            OT_Name = tr.OT,
            OT_Gender = tr.Gender,
            ID32 = tr.ID32,
        };
        SetPINGA(pk, criteria, pi);
        pk.SetMoves(Moves);

        pk.ResetPartyStats();
        return pk;
    }

    private void SetPINGA(PK9 pk, EncounterCriteria criteria, PersonalInfo9SV pi)
    {
        const byte rollCount = 1;
        const byte undefinedSize = 0;
        var param = new GenerateParam9(Species, pi.Gender, FlawlessIVCount, rollCount,
            undefinedSize, undefinedSize, undefinedSize, undefinedSize,
            Ability, Shiny);

        var init = Util.Rand.Rand64();
        var success = this.TryApply32(pk, init, param, criteria);
        if (!success)
            this.TryApply32(pk, init, param, EncounterCriteria.Unrestricted);
    }
    #endregion
}