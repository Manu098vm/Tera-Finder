using PKHeX.Core;

namespace TeraFinder.Core;

public class EncounterRaid9 : IEncounterable, IEncounterConvertible<PK9>, ITeraRaid9, IMoveset, IFlawlessIVCount, IFixedGender
{
    protected ITeraRaid9 Encounter { get; set; } = null!;

    public EncounterRaid9(ITeraRaid9 encounter) => Encounter = encounter;

    public bool IsDistribution => Encounter.IsDistribution;
    public byte Index => Encounter.Index;
    public byte Stars => Encounter.Stars;
    public byte RandRate => Encounter.RandRate;
    public GemType TeraType => Encounter.TeraType;
    public ushort Species => ((dynamic)Encounter).Species;
    public byte Form => ((dynamic)Encounter).Form;
    public GameVersion Version => ((dynamic)Encounter).Version; 
    public byte FlawlessIVCount => ((dynamic)Encounter).FlawlessIVCount; 
    public AbilityPermission Ability => ((dynamic)Encounter).Ability;
    public Shiny Shiny => ((dynamic)Encounter).Shiny;
    public byte Gender => ((dynamic)Encounter).Gender;
    public byte Level => ((dynamic)Encounter).Level;
    public Moveset Moves => ((dynamic)Encounter).Moves;

    public string Name => ((dynamic)Encounter).Name;
    public string LongName => ((dynamic)Encounter).LongName;
    public EntityContext Context => ((dynamic)Encounter).Context;
    public bool EggEncounter => ((dynamic)Encounter).EggEncounter;
    public int Generation => ((dynamic)Encounter).Generation;
    public bool IsShiny => ((dynamic)Encounter).IsShiny;
    public byte LevelMin => ((dynamic)Encounter).LevelMin;
    public byte LevelMax => ((dynamic)Encounter).LevelMax;
    public int Location => ((dynamic)Encounter).Location;
    public int EggLocation => ((dynamic)Encounter).EggLocation;
    public Ball FixedBall => ((dynamic)Encounter).FixedBall;

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

    public bool CanBeEncountered(uint seed) => Encounter.CanBeEncountered(seed);

    public static EncounterRaid9[] GetEncounters(ITeraRaid9[] encounters)
    {
        var res = new List<EncounterRaid9>();
        foreach (var encounter in encounters)
            res.Add(new EncounterRaid9(encounter));
        return res.ToArray();
    }

    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria) => ConvertToPKM(tr, criteria);
    PKM IEncounterConvertible.ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr);
    public PK9 ConvertToPKM(ITrainerInfo tr) => ConvertToPKM(tr, EncounterCriteria.Unrestricted);
    public PK9 ConvertToPKM(ITrainerInfo tr, EncounterCriteria criteria) => ((dynamic)Encounter).ConvertToPKM(tr, criteria);

    public ushort GetRandRateTotalScarlet(int stage)
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return 0;

        return ((dynamic)Encounter).GetRandRateTotalScarlet(stage);
    }

    public ushort GetRandRateTotalViolet(int stage)
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return 0;

        return ((dynamic)Encounter).GetRandRateTotalViolet(stage);
    }

    public ushort GetRandRateMinScarlet(int stage = 0)
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return (ushort)((dynamic)Encounter).RandRateMinScarlet;

        return ((dynamic)Encounter).GetRandRateMinScarlet(stage);
    }

    public ushort GetRandRateMinViolet(int stage = 0)
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return (ushort)((dynamic)Encounter).RandRateMinViolet;

        return ((dynamic)Encounter).GetRandRateMinViolet(stage);
    }

    private Nature GetNature()
    {
        if (Encounter is EncounterMight9 or PKHeX.Core.EncounterMight9)
            return ((dynamic)Encounter).Nature;

        return Nature.Random;
    }

    private SizeType9 GetScaleType()
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return 0;

        return ((dynamic)Encounter).ScaleType;
    }

    private byte GetScale()
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return 0;

        return ((dynamic)Encounter).Scale;
    }

    private IndividualValueSet GetIVs()
    {
        if (Encounter is EncounterTera9 or PKHeX.Core.EncounterTera9)
            return default;

        return ((dynamic)Encounter).IVs;
    }

    private uint GetIdentifier()
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9)
            return 0;

        return ((dynamic)Encounter).Identifier;
    }

    private ulong GetFixedRewardHash()
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9)
            return 0;

        return ((dynamic)Encounter).FixedRewardHash;
    }

    private ulong GetLotteryRewardHash()
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9)
            return 0;

        return ((dynamic)Encounter).LotteryRewardHash;
    }

    private int GetItem()
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9)
            return 0;

        return ((dynamic)Encounter).Item;
    }

    private ExtraMoves GetExtraMoves()
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9)
            return new();

        return ((dynamic)Encounter).ExtraMoves;
    }

    private bool GetCanBeEncountered(GameVersion version)
    {
        if (Encounter is PKHeX.Core.EncounterTera9 or EncounterTera9)
            return version switch { GameVersion.SL => ((dynamic)Encounter).IsAvailableHostScarlet, _ => ((dynamic)Encounter).IsAvailableHostViolet, };

        if (Encounter is PKHeX.Core.EncounterDist9 or PKHeX.Core.EncounterMight9 or EncounterDist9 or EncounterMight9)
        {
            for (var progress = 0; progress <= 3; progress++) 
            {
                var maxRate = version switch { GameVersion.SL => GetRandRateTotalScarlet(progress), _ => GetRandRateTotalViolet(progress) };
                var minRate = version switch { GameVersion.SL => GetRandRateMinScarlet(progress), _ => GetRandRateMinViolet(progress) };

                if (minRate >= 0 && maxRate > 0)
                    return true;
            }
        }

        return false;
    }
}
