using PKHeX.Core;

namespace TeraFinder.Core;

public class EncounterRaid9(ITeraRaid9 encounter) : IEncounterable, IEncounterConvertible<PK9>, ITeraRaid9, IExtendedTeraRaid9, IMoveset, IFlawlessIVCount, IFixedGender
{
    protected ITeraRaid9 EncounterRaid { get; init; } = encounter as ITeraRaid9;
    protected IEncounterable Encounterable { get; init; } = (encounter as IEncounterable)!;
    protected IEncounterConvertible<PK9> EncounterConvertible { get; init; } = (encounter as IEncounterConvertible<PK9>)!;
    protected IMoveset EncounterMoveset { get; init; } = (encounter as IMoveset)!;
    protected IFlawlessIVCount EncounterIV { get; init; } = (encounter as IFlawlessIVCount)!;
    protected IFixedGender EncounterGender { get; init; } = (encounter as IFixedGender)!;

    protected IExtendedTeraRaid9? EncounterRaidExtended => encounter as IExtendedTeraRaid9;

    public bool IsDistribution => EncounterRaid.IsDistribution;
    public byte Index => EncounterRaid.Index;
    public byte Stars => EncounterRaid.Stars;
    public byte RandRate => EncounterRaid.RandRate;
    public GemType TeraType => EncounterRaid.TeraType;
    public ushort Species => Encounterable.Species;
    public byte Form => Encounterable.Form;
    public GameVersion Version => Encounterable.Version; 
    public byte FlawlessIVCount => EncounterIV.FlawlessIVCount; 
    public AbilityPermission Ability => Encounterable.Ability;
    public Shiny Shiny => Encounterable.Shiny;
    public byte Gender => EncounterGender.Gender;
    public byte Level => Encounterable.LevelMin;
    public Moveset Moves => EncounterMoveset.Moves;
    public string Name => Encounterable.Name;
    public string LongName => Encounterable.LongName;
    public EntityContext Context => Encounterable.Context;
    public bool EggEncounter => Encounterable.EggEncounter;
    public int Generation => Encounterable.Generation;
    public bool IsShiny => Encounterable.IsShiny;
    public byte LevelMin => Encounterable.LevelMin;
    public byte LevelMax => Encounterable.LevelMax;
    public int Location => Encounterable.Location;
    public int EggLocation => Encounterable.EggLocation;
    public Ball FixedBall => Encounterable.FixedBall;
    public TeraRaidMapParent Map => GetEncounterMap();
    public RaidContent Content => GetRaidContent();

    public uint Identifier => GetIdentifier(); 
    public int Item => GetItem(); 
    public Nature Nature => GetNature(); 
    public SizeType9 ScaleType => GetScaleType(); 
    public byte Scale => GetScale();
    public IndividualValueSet IVs => GetIVs(); 
    public ulong FixedRewardHash => GetFixedRewardHash(); 
    public ulong LotteryRewardHash => GetLotteryRewardHash(); 
    public ushort RandRateMinScarlet => GetRandRateMinScarlet(); 
    public ushort RandRateMinViolet => GetRandRateMinViolet();
    public ExtraMoves ExtraMoves => GetExtraMoves();
    public bool CanBeEncounteredScarlet => GetCanBeEncountered(GameVersion.SL);
    public bool CanBeEncounteredViolet => GetCanBeEncountered(GameVersion.VL);

    byte IGeneration.Generation => throw new NotImplementedException();

    ushort ILocation.Location => throw new NotImplementedException();

    ushort ILocation.EggLocation => throw new NotImplementedException();

    public Type GetEncounterType() => EncounterRaid.GetType();
    public bool CanBeEncountered(uint seed) => EncounterRaid.CanBeEncountered(seed);

    public static EncounterRaid9[] GetEncounters(ITeraRaid9[] encounters)
    {
        var res = new List<EncounterRaid9>();
        foreach (var encounter in encounters)
            res.Add(new EncounterRaid9(encounter));
        return [.. res];
    }

    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria) => ConvertToPKM(tr, criteria);
    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr);
    public PK9 ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr, EncounterCriteria.Unrestricted);
    public PK9 ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria) => EncounterConvertible.ConvertToPKM(tr, criteria);

    public ushort GetRandRateTotalScarlet(int stage) =>
        EncounterRaid switch
        {
            EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.GetRandRateTotalScarlet(stage),
            PKHeX.Core.EncounterDist9 => (ushort)((dynamic)EncounterRaid).GetRandRateTotalScarlet(stage),
            _ => 0,
        };

    public ushort GetRandRateTotalViolet(int stage) =>
        EncounterRaid switch
        {
            EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.GetRandRateTotalViolet(stage),
            PKHeX.Core.EncounterDist9 => (ushort)((dynamic)EncounterRaid).GetRandRateTotalViolet(stage),
            _ => 0,
        };

    public ushort GetRandRateMinScarlet(int stage = 0) =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.GetRandRateMinScarlet(stage),
            PKHeX.Core.EncounterTera9 => (ushort)((dynamic)EncounterRaid).RandRateMinScarlet,
            _ => (ushort)((dynamic)EncounterRaid).GetRandRateMinScarlet(stage),
        };

    public ushort GetRandRateMinViolet(int stage = 0) =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.GetRandRateMinViolet(stage),
            PKHeX.Core.EncounterTera9 => (ushort) ((dynamic) EncounterRaid).RandRateMinViolet,
            _ => (ushort) ((dynamic)EncounterRaid).GetRandRateMinViolet(stage),
        };

    private Nature GetNature() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.Nature,
            PKHeX.Core.EncounterTera9 => Nature.Random,
            _ => ((dynamic)EncounterRaid).Nature
        };

    private SizeType9 GetScaleType() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.ScaleType,
            PKHeX.Core.EncounterTera9 => SizeType9.RANDOM,
            _ => ((dynamic)EncounterRaid).ScaleType
        };    

    private byte GetScale() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.Scale,
            PKHeX.Core.EncounterTera9 => 0,
            _ => ((dynamic)EncounterRaid).Scale
        };

    private IndividualValueSet GetIVs() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.IVs,
            PKHeX.Core.EncounterTera9 => default,
            _ => ((dynamic)EncounterRaid).IVs
        };

    private uint GetIdentifier() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.Identifier,
            _ => 0,
        };

    private ulong GetFixedRewardHash() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.FixedRewardHash,
            _ => 0,
        };

    private ulong GetLotteryRewardHash() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.LotteryRewardHash,
            _ => 0,
        };

    private int GetItem() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.Item,
            _ => 0,
        };

    private ExtraMoves GetExtraMoves() =>
        EncounterRaid switch
        {
            EncounterTera9 or EncounterDist9 or EncounterMight9 => EncounterRaidExtended!.ExtraMoves,
            _ => new(),
        };

    private bool GetCanBeEncountered(GameVersion version) =>
        EncounterRaid switch
        {
            EncounterTera9 t => version switch { GameVersion.SL => t.IsAvailableHostScarlet, _ => t.IsAvailableHostViolet },
            PKHeX.Core.EncounterTera9 t => version switch { GameVersion.SL => t.IsAvailableHostScarlet, _ => t.IsAvailableHostViolet },
            _ => CanDistBeEncountered(version),
        };

    private bool CanDistBeEncountered(GameVersion version)
    {
        for (var progress = 0; progress <= 3; progress++)
        {
            var maxRate = version switch { GameVersion.SL => GetRandRateTotalScarlet(progress), _ => GetRandRateTotalViolet(progress) };
            var minRate = version switch { GameVersion.SL => GetRandRateMinScarlet(progress), _ => GetRandRateMinViolet(progress) };

            if (minRate >= 0 && maxRate > 0)
                return true;
        }

        return false;
    }

    private TeraRaidMapParent GetEncounterMap() =>
        EncounterRaid switch
        {
            EncounterTera9 tera9 => tera9.Map,
            PKHeX.Core.EncounterTera9 tera9 => tera9.Map,
            _ => TeraRaidMapParent.Paldea,
        };

    private RaidContent GetRaidContent()
    {
        if (Stars == 7)
            return RaidContent.Event_Mighty;
        if (IsDistribution)
            return RaidContent.Event;
        if (Stars == 6)
            return RaidContent.Black;

        return RaidContent.Standard;
    }
}
