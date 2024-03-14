using PKHeX.Core;

namespace TeraFinder.Core;

public abstract record EncounterRaidTF9 : IExtendedTeraRaid9
{
    public required PersonalInfo9SV Personal { get; init; }
    public required ushort Species { get; init; }
    public required byte Form { get; init; }
    public required byte Gender { get; init; }
    public required byte GenderRatio { get; init; }
    public required AbilityPermission Ability { get; init; }
    public required byte FlawlessIVCount { get; init; }
    public required Shiny Shiny { get; init; }
    public required Nature Nature { get; init; }
    public required byte Level { get; init; }
    public required Moveset Moves { get; init; }
    public required ExtraMoves ExtraMoves { get; init; }
    public required IndividualValueSet IVs { get; init; }
    public required GemType TeraType { get; init; }
    public required byte Index { get; init; }
    public required byte Stars { get; init; }
    public required byte RandRate { get; init; }
    public required SizeType9 ScaleType { get; init; }
    public required byte Scale { get; init; }
    public required TeraRaidMapParent Map { get; init; }
    public required RaidContent ContentType { get; init; }
    public required int HeldItem { get; init; }
    public required uint Identifier { get; init; }
    public required List<Reward> FixedRewards { get; init; }
    public required List<Reward> LotteryRewards { get; init; }

    public virtual bool CanBeEncounteredScarlet { get; protected set; }
    public virtual bool CanBeEncounteredViolet { get; protected set; }

    public bool IsDistribution => Index != 0;
    public bool IsMighty => IsDistribution && Stars == 7;
    public bool IsBlack => !IsDistribution && Stars >= 6;
    public bool IsRandomUnspecificForm => Form >= EncounterUtil.FormDynamic;

    public abstract bool CanBeEncountered(uint seed);

    //Generate Tera Details with no Filters;
    //Assume there is at least a Stars filter
    public static bool TryGenerateTeraDetails(in uint seed, in EncounterRaidTF9[] encounters, in GameVersion version, in GameProgress progress,
        in EventProgress eventProgress, in RaidContent content, in TeraRaidMapParent map, in uint id32, in byte groupid, out EncounterRaidTF9? encounter, out TeraDetails? result)
    {
        result = null;
        encounter = content switch
        {
            RaidContent.Standard or RaidContent.Black => EncounterTeraTF9.GetEncounterFromSeed(seed, encounters, version, progress, content, map),
            RaidContent.Event or RaidContent.Event_Mighty => EncounterEventTF9.GetEncounterFromSeed(seed, encounters as EncounterEventTF9[], version, eventProgress, groupid),
            _ => throw new ArgumentOutOfRangeException(nameof(content)),
        };

        if (encounter is null)
            return false;

        result = encounter.GenerateData(seed, id32, groupid);
        return true;
    }

    //Generate Tera Details with Filters; assumes the encounters pool to be already filtered based on species and stars
    public static bool TryGenerateTeraDetails(in uint seed, in EncounterRaidTF9[] encounters, in TeraFilter filter, in short romTotal, in GameVersion version, 
        in GameProgress progress, in EventProgress eventProgress, in RaidContent content, in uint id32, in byte groupid, out EncounterRaidTF9? encounter, out TeraDetails? result)
    {
        result = null;

        encounter = content switch
        {
            RaidContent.Standard or RaidContent.Black => EncounterTeraTF9.GetEncounterFromSeed(seed, encounters as EncounterTeraTF9[], romTotal, version, progress, content),
            RaidContent.Event or RaidContent.Event_Mighty => EncounterEventTF9.GetEncounterFromSeed(seed, encounters as EncounterEventTF9[], version, eventProgress, groupid),
            _ => throw new ArgumentOutOfRangeException(nameof(content)),
        };

        if (encounter is null)
            return false;

        if (!encounter.GenerateData(filter, seed, id32, groupid, out result))
            return false;

        return true;
    }

    //Generate RNG Details and Reward Dtails with no Filters
    public static bool TryGenerateRewardDetails(in uint seed, in EncounterRaidTF9[] encounters, in GameVersion version, in GameProgress progress, in EventProgress eventProgress,
        RaidContent content, in TeraRaidMapParent map, uint id32, byte groupid, in int boost, out TeraDetails? teraRes, out RewardDetails? rewardRes)
    {
        teraRes = null;
        rewardRes = null;

        var encounter = content switch
        {
            RaidContent.Standard or RaidContent.Black => EncounterTeraTF9.GetEncounterFromSeed(seed, encounters, version, progress, content, map),
            RaidContent.Event or RaidContent.Event_Mighty => EncounterEventTF9.GetEncounterFromSeed(seed, encounters as EncounterEventTF9[], version, eventProgress, groupid),
            _ => throw new ArgumentOutOfRangeException(nameof(content)),
        };

        if (encounter is null)
            return false;

        teraRes = encounter.GenerateData(seed, id32, groupid);
        var rewards = RewardUtil.GetCombinedRewardList(teraRes!.Value, encounter!.FixedRewards, encounter.LotteryRewards, boost);
        rewardRes = new RewardDetails { Seed = seed, Rewards = rewards, Species = encounter.Species, Stars = encounter.Stars, Shiny = teraRes.Value.Shiny, TeraType = teraRes.Value.TeraType, EventIndex = groupid };

        return true;
    }

    //Generate RNG Details and Reward Details with Filters; assumes the encounters pool to be already filtered based on species and stars
    //Assume there is at least a Stars filter
    public static bool TryGenerateRewardDetails(uint seed, EncounterRaidTF9[] encounters, RewardFilter filter, in short romTotal, GameVersion version, 
        GameProgress progress, EventProgress eventProgress, RaidContent content, uint id32, byte groupid, in int boost, out TeraDetails? teraRes, out RewardDetails? rewardRes)
    {
        teraRes = null;
        rewardRes = null;

        var encounter = content switch
        {
            RaidContent.Standard or RaidContent.Black => EncounterTeraTF9.GetEncounterFromSeed(seed, encounters as EncounterTeraTF9[], romTotal, version, progress, content),
            RaidContent.Event or RaidContent.Event_Mighty => EncounterEventTF9.GetEncounterFromSeed(seed, encounters as EncounterEventTF9[], version, eventProgress, groupid),
            _ => throw new ArgumentOutOfRangeException(nameof(content)),
        };

        if (encounter is null)
            return false;

        teraRes = encounter.GenerateData(seed, id32, groupid);
        var rewards = RewardUtil.GetCombinedRewardList(teraRes!.Value, encounter!.FixedRewards, encounter.LotteryRewards, boost);
        rewardRes = new RewardDetails { Seed = seed, Rewards = rewards, Species = encounter.Species, Stars = encounter.Stars, Shiny = teraRes.Value.Shiny, TeraType = teraRes.Value.TeraType, EventIndex = groupid };

        if (!filter.IsFilterMatch(rewardRes.Value))
            return false;

        return true;
    }

    //Generate a PK9 from a given seed
    public bool GeneratePK9(uint seed, uint id32, GameVersion version, string ot_name, int ot_language, byte ot_gender, out PK9? result, out LegalityAnalysis legality)
    {
        result = null;
        var rngres = this.GenerateData(seed, id32);

        result = new PK9(Properties.Resources.template)
        {
            Species = Species,
            HeldItem = HeldItem,
            MetLevel = Level,
            CurrentLevel = Level,
            Obedience_Level = Level,
            RibbonMarkMightiest = IsMighty,
            ID32 = id32,
            Version = version,
            Language = ot_language,
            HandlingTrainerLanguage = (byte)ot_language,
            OriginalTrainerName = ot_name,
            HandlingTrainerName = ot_name,
            OriginalTrainerGender = ot_gender,
            TeraTypeOriginal = (MoveType)rngres.TeraType,
            EncryptionConstant = rngres.EC,
            Form = rngres.Form,
            PID = rngres.PID,
            IV_HP = rngres.HP,
            IV_ATK = rngres.ATK,
            IV_DEF = rngres.DEF,
            IV_SPA = rngres.SPA,
            IV_SPD = rngres.SPD,
            IV_SPE = rngres.SPE,
            Gender = (byte)rngres.Gender,
            Nature = rngres.Nature,
            StatNature = rngres.Nature,
            HeightScalar = rngres.Height,
            WeightScalar = rngres.Weight,
            Scale = rngres.Scale,
            Move1 = rngres.Move1,
            Move2 = rngres.Move2,
            Move3 = rngres.Move3,
            Move4 = rngres.Move4,
            Ability = rngres.Ability,
            AbilityNumber = rngres.AbilityNumber,
            MetDate = DateOnly.FromDateTime(DateTime.Now),
        };

        result.HealPP();
        result.ClearNickname();
        result.ClearRecordFlags();
        result.RefreshChecksum();

        legality = new LegalityAnalysis(result);
        if (!legality.Valid)
        {
            var changed = false;
            var la_ot = legality.Results.Where(l => l.Identifier is CheckIdentifier.Trainer).FirstOrDefault();
            if ((LanguageID)result.Language is LanguageID.ChineseS or LanguageID.ChineseT or LanguageID.Korean or LanguageID.Japanese && !la_ot.Valid)
            {
                result.OriginalTrainerName = "TF";
                result.RefreshChecksum();
                changed = true;
            }

            if (changed)
                legality = new LegalityAnalysis(result);

            if (!legality.Valid)
                return false;
        }

        return true;
    }

    //Get the encounter names from a pool of encounters
    public static HashSet<string> GetAvailableSpecies(EncounterRaidTF9[] encounters, byte stars, string[] speciesNames, string[] formsNames, string[] typesNames, Dictionary<string, string> pluginStrings)
    {
        HashSet<string> list = [];
        foreach (var encounter in encounters)
        {
            if (stars != 0 && encounter.Stars != stars)
                continue;

            var formlist = FormConverter.GetFormList(encounter.Species, typesNames, formsNames, GameInfo.GenderSymbolASCII, EntityContext.Gen9);
            var str = $"{speciesNames[encounter.Species]}{(formlist.Length > 1 ? $"-{$"{formlist[encounter.Form]}"}" : "")}" +
                $"{(encounter.Index > 0 ? $" ({encounter.Index})" : "")}";

            if (!encounter.CanBeEncounteredScarlet)
                str += $" ({pluginStrings["GameVersionVL"]})";

            if (!encounter.CanBeEncounteredViolet)
                str += $" ({pluginStrings["GameVersionSL"]})";

            list.Add(str);
        }
        return list;
    }
}