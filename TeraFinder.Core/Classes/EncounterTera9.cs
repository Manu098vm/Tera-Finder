using PKHeX.Core;
using System.Buffers.Binary;
using static PKHeX.Core.AbilityPermission;

//Extension of https://github.com/kwsch/PKHeX/blob/master/PKHeX.Core/Legality/Encounters/EncounterStatic/EncounterTera9.cs
namespace TeraFinder.Core;

/// <summary>
/// Generation 9 Tera Raid Encounter
/// </summary>
public sealed record EncounterTera9 : IEncounterable, IEncounterConvertible<PK9>, ITeraRaid9, IMoveset, IFlawlessIVCount, IFixedGender
{
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
    public required sbyte Gender { get; init; }
    public required AbilityPermission Ability { get; init; }
    public required byte FlawlessIVCount { get; init; }
    public required Shiny Shiny { get; init; }
    public required byte Level { get; init; }
    public required Moveset Moves { get; init; }
    public required GemType TeraType { get; init; }
    public required byte Index { get; init; }
    public required byte Stars { get; init; }

    //TeraFinder Serialization
    public uint Identifier { get; private init; }
    public ulong FixedRewardHash { get; private init; }
    public ulong LotteryRewardHash { get; private init; }
    public int Item { get; private init; }

    public required byte RandRate { get; init; } // weight chance of this encounter
    public required short RandRateMinScarlet { get; init; } // weight chance total of all lower index encounters, for Scarlet
    public required short RandRateMinViolet { get; init; } // weight chance total of all lower index encounters, for Violet
    public bool IsAvailableHostScarlet => RandRateMinScarlet != -1;
    public bool IsAvailableHostViolet => RandRateMinViolet != -1;

    public string Name => "Tera Raid Encounter";
    public string LongName => Name;
    public byte LevelMin => Level;
    public byte LevelMax => Level;

    public bool CanBeEncountered(uint seed) => Tera9RNG.IsMatchStarChoice(seed, Stars, RandRate, RandRateMinScarlet, RandRateMinViolet);

    /// <summary>
    /// Fetches the rate sum for the base ROM raid, depending on star count.
    /// </summary>
    /// <param name="star">Raid Difficulty</param>
    /// <returns>Total rate value the game uses to call rand(x) with.</returns>
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

    public static EncounterTera9[] GetArray(ReadOnlySpan<byte> data)
    {
        const int size = 0x30;
        var count = data.Length / size;
        var result = new EncounterTera9[count];
        for (int i = 0; i < result.Length; i++)
            result[i] = ReadEncounter(data.Slice(i * size, size));
        return result;
    }

    private static EncounterTera9 ReadEncounter(ReadOnlySpan<byte> data) => new()
    {
        Species = BinaryPrimitives.ReadUInt16LittleEndian(data),
        Form = data[0x02],
        Gender = (sbyte)(data[0x03] - 1),
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
        var pk = new PK9
        {
            Language = lang,
            Species = Species,
            Form = Form,
            CurrentLevel = LevelMin,
            OT_Friendship = PersonalTable.SV[Species, Form].BaseFriendship,
            Met_Location = Location,
            Met_Level = LevelMin,
            Version = (int)version,
            Ball = (byte)Ball.Poke,

            Nickname = SpeciesName.GetSpeciesNameGeneration(Species, lang, Generation),
            Obedience_Level = LevelMin,
        };
        SetPINGA(pk, criteria);
        pk.SetMoves(Moves);

        pk.ResetPartyStats();
        return pk;
    }

    private void SetPINGA(PK9 pk, EncounterCriteria criteria)
    {
        const byte rollCount = 1;
        const byte undefinedSize = 0;
        var pi = PersonalTable.SV.GetFormEntry(Species, Form);
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