using PKHeX.Core;
using System.Text;

namespace TeraFinder.Core;

public static class TeraUtil
{
    public static byte[] GetDenLocations(TeraRaidMapParent map) => map switch
    {
        TeraRaidMapParent.Paldea => Properties.Resources.paldea_locations,
        TeraRaidMapParent.Kitakami => Properties.Resources.kitakami_locations,
        _ => Properties.Resources.blueberry_locations
    };

    public static string? GetTextResource(string res)
    {
        var obj = Properties.Resources.ResourceManager.GetObject(res);
        if (res.Contains("lang"))
            return (string?)obj;
        else if (obj is null) return null;
        return Encoding.UTF8.GetString((byte[]) obj);
    }

    public static PK9 GenerateTeraEntity(SAV9SV sav, EncounterRaid9 encounter, RaidContent content, uint seed, uint tid, uint sid, int groupid)
    {
        var template = new PK9(Properties.Resources.template);
        var rngres = CalcRNG(seed, tid, sid, content, encounter, groupid);
        template.Species = rngres.Species;
        template.Form = rngres.Form;
        if (rngres.Stars == 7) template.RibbonMarkMightiest = true;
        template.MetDate = DateOnly.FromDateTime(DateTime.Now);
        template.Met_Level = rngres.Level;
        template.CurrentLevel = rngres.Level;
        template.Obedience_Level = (byte)rngres.Level;
        template.TeraTypeOriginal = (MoveType)rngres.TeraType;
        template.EncryptionConstant = rngres.EC;
        template.TrainerTID7 = tid;
        template.TrainerSID7 = sid;
        template.Version = sav.Game;
        template.Language = (byte)sav.Language;
        template.HT_Name = sav.OT;
        template.HT_Language = (byte)sav.Language;
        template.OT_Name = sav.OT;
        template.OT_Gender = sav.Gender;
        template.PID = rngres.PID;
        template.IV_HP = rngres.HP;
        template.IV_ATK = rngres.ATK;
        template.IV_DEF = rngres.DEF;
        template.IV_SPA = rngres.SPA;
        template.IV_SPD = rngres.SPD;
        template.IV_SPE = rngres.SPE;
        template.Ability = rngres.Ability;
        template.AbilityNumber = rngres.GetAbilityNumber() == 3 ? 4 : rngres.GetAbilityNumber();
        template.Nature = rngres.Nature;
        template.StatNature = rngres.Nature;
        template.Gender = (int)rngres.Gender;
        template.HeightScalar = rngres.Height;
        template.WeightScalar = rngres.Weight;
        template.Scale = rngres.Scale;
        template.Move1 = rngres.Move1;
        template.Move2 = rngres.Move2;
        template.Move3 = rngres.Move3;
        template.Move4 = rngres.Move4;
        if (encounter.Item > 0) template.HeldItem = encounter.Item;

        template.HealPP();
        template.ClearNickname();

        try 
        {
            var la = new LegalityAnalysis(template);

            if (!la.Valid)
            {
                var ability = la.Results.Where(l => l.Identifier is CheckIdentifier.Ability).FirstOrDefault();
                var la_ot = la.Results.Where(l => l.Identifier is CheckIdentifier.Trainer).FirstOrDefault();
                if (!ability.Valid)
                {
                    for (var i = 0; i <= 4 && !la.Valid; i++)
                    {
                        template.AbilityNumber = i;
                        i++;
                        la = new LegalityAnalysis(template);
                    }
                }
                if ((LanguageID)template.Language is (LanguageID.ChineseS or LanguageID.ChineseT or LanguageID.Korean or LanguageID.Japanese) && !la_ot.Valid)
                    template.OT_Name = "TF";
            }
        }
        catch (Exception ex) 
        {
            //PKHeX currently has a bug when analyzing the Mighty Mark ribbon if the encounter is not in the db, this is an hacky way to handle the exception
            if (ex.ToString().Contains("RibbonMarkMightiest"))
                template.RibbonMarkMightiest = false;

            return template;
        }

        return template;
    }

    public static GameProgress GetProgress(SAV9SV sav)
    {
        var Unlocked6Stars = sav.Accessor.FindOrDefault(Blocks.KUnlockedRaidDifficulty6.Key).Type is SCTypeCode.Bool2;
            
        if (Unlocked6Stars)
            return GameProgress.Unlocked6Stars;

        var Unlocked5Stars = sav.Accessor.FindOrDefault(Blocks.KUnlockedRaidDifficulty5.Key).Type is SCTypeCode.Bool2;

        if (Unlocked5Stars)
            return GameProgress.Unlocked5Stars;

        var Unlocked4Stars = sav.Accessor.FindOrDefault(Blocks.KUnlockedRaidDifficulty4.Key).Type is SCTypeCode.Bool2;

        if (Unlocked4Stars)
            return GameProgress.Unlocked4Stars;

        var Unlocked3Stars = sav.Accessor.FindOrDefault(Blocks.KUnlockedRaidDifficulty3.Key).Type is SCTypeCode.Bool2;

        if (Unlocked3Stars)
            return GameProgress.Unlocked3Stars;

        var UnlockedTeraRaids = sav.Accessor.FindOrDefault(Blocks.KUnlockedTeraRaidBattles.Key).Type is SCTypeCode.Bool2;

        if (UnlockedTeraRaids)
            return GameProgress.UnlockedTeraRaids;

        return GameProgress.Beginning;
    }

    public static MoveType GetType(uint seed)
    {
        var xoro = new Xoroshiro128Plus(seed);
        return (MoveType)xoro.NextInt(18);
    }

    public static int GetStars(uint seed, GameProgress progress)
    {
        if (progress is GameProgress.Beginning)
            return 0;

        var xoro = new Xoroshiro128Plus(seed);
        var stars = progress == (GameProgress)6 ? 6 : CalcStars(xoro, progress);
        return stars;
    }

    private static int CalcStars(Xoroshiro128Plus xoro, GameProgress progress)
    {
        var rand = xoro.NextInt(100);

        return progress switch
        {
            GameProgress.Unlocked6Stars => rand switch
            {
                > 70 => 5,
                > 30 => 4,
                _ => 3,
            },
            GameProgress.Unlocked5Stars => rand switch
            {
                > 75 => 5,
                > 40 => 4,
                _ => 3,
            },
            GameProgress.Unlocked4Stars => rand switch
            {
                > 70 => 4,
                > 40 => 3,
                > 20 => 2,
                _ => 1,
            },
            GameProgress.Unlocked3Stars => rand switch
            {
                > 70 => 3,
                > 30 => 2,
                _ => 1,
            },
            _ => rand switch
            {
                > 80 => 2,
                _ => 1,
            },
        };
    }

    public static List<string> GetAvailableSpecies(SAV9SV sav, string[] species, string[] forms, string[] types, Dictionary<string, string> plugins, int stars, RaidContent content, TeraRaidMapParent map)
    {
        List<string> list = [];
        var game = (GameVersion)sav.Game;

        var encounters = content is RaidContent.Event ? GetSAVDistEncounters(sav)[0] : content is RaidContent.Event_Mighty ? GetSAVDistEncounters(sav)[1] :
            EncounterRaid9.GetEncounters(EncounterTera9.GetArray(map switch
            {
                TeraRaidMapParent.Paldea => Properties.Resources.encounter_gem_paldea,
                TeraRaidMapParent.Kitakami => Properties.Resources.encounter_gem_kitakami,
                _ => Properties.Resources.encounter_gem_blueberry
            }, map));

        foreach (var encounter in encounters)
        {
            if (encounter.Species > 0 && (encounter.Version is GameVersion.SV || encounter.Version == game) && ((stars == 0 && encounter.Stars != 6) || encounter.Stars == stars))
            {
                var formlist = FormConverter.GetFormList(encounter.Species, types, forms, GameInfo.GenderSymbolASCII, EntityContext.Gen9);
                var str = $"{species[encounter.Species]}{(formlist.Length > 1 ? $"-{$"{formlist[encounter.Form]}"}" : "")}";

                if (!encounter.CanBeEncounteredScarlet)
                    str += $" ({plugins["GameVersionVL"]})";

                if (!encounter.CanBeEncounteredViolet)
                    str += $" ({plugins["GameVersionSL"]})";

                if (!list.Contains(str))
                    list.Add(str);
            }
        }
        return list;
    }

    public static EncounterRaid9[] GetAllTeraEncounters(TeraRaidMapParent map) => 
        EncounterRaid9.GetEncounters(EncounterTera9.GetArray(map switch
        {
            TeraRaidMapParent.Paldea => Properties.Resources.encounter_gem_paldea,
            TeraRaidMapParent.Kitakami => Properties.Resources.encounter_gem_kitakami,
            _ => Properties.Resources.encounter_gem_blueberry
        }, map));

    public static EncounterRaid9[] GetAllDistEncounters(RaidContent type) =>
        type switch
        {
            RaidContent.Event => EncounterRaid9.GetEncounters(PKHeX.Core.EncounterDist9.GetArray(Util.GetBinaryResource("encounter_dist_paldea.pkl"))),
            RaidContent.Event_Mighty => EncounterRaid9.GetEncounters(PKHeX.Core.EncounterMight9.GetArray(Util.GetBinaryResource("encounter_might_paldea.pkl"))),
            _ => throw new ArgumentException("Invalid RaidContent type"),
        };

    public static EncounterRaid9[][] GetSAVDistEncounters(SAV9SV sav)
    {
        try
        {
            var events = EventUtil.GetEventEncounterDataFromSAV(sav);
            var dist = EncounterRaid9.GetEncounters(EncounterDist9.GetArray(events[0]));
            var mighty = EncounterRaid9.GetEncounters(EncounterMight9.GetArray(events[1]));
            return [dist, mighty];
        }
        catch
        {
            const int encSize = EncounterDist9.SerializedSize;
            var dist = EncounterRaid9.GetEncounters(EncounterDist9.GetArray(new byte[encSize]));
            var might = EncounterRaid9.GetEncounters(EncounterMight9.GetArray(new byte[encSize]));
            return [dist, might];
        }
    }

    public static EncounterRaid9? GetTeraEncounter(uint seed, GameVersion game, int stars, EncounterRaid9[] encounters, TeraRaidMapParent map)
    {
        var xoro = new Xoroshiro128Plus(seed);
        if (stars < 6) xoro.NextInt(100);
        var max = game is GameVersion.SL ? EncounterTera9.GetRateTotalSL(stars, map) : EncounterTera9.GetRateTotalVL(stars, map);
        var rateRand = (int)xoro.NextInt((uint)max);
        foreach (var encounter in encounters)
        {
            var min = game is GameVersion.SL ? encounter.RandRateMinScarlet : encounter.RandRateMinViolet;
            if (encounter.Stars == stars && min >= 0 && (uint)(rateRand - min) < encounter.RandRate)
                return encounter;
        }
        return null;
    }

    public static EncounterRaid9? GetDistEncounter(uint seed, GameVersion game, GameProgress progress, EncounterRaid9[] encounters, int groupid = -2)
    {
        var p = progress switch
        {
            GameProgress.Unlocked6Stars or GameProgress.Unlocked5Stars => 3,
            GameProgress.Unlocked4Stars => 2,
            GameProgress.Unlocked3Stars => 1,
            _ => 0,
        };

        foreach (var encounter in encounters)
        {
            if (groupid != -2 && encounter.Index != groupid)
                continue;
            var max = game is GameVersion.SL ? encounter.GetRandRateTotalScarlet(p) : encounter.GetRandRateTotalViolet(p);
            var min = game is GameVersion.SL ? encounter.GetRandRateMinScarlet(p) : encounter.GetRandRateMinViolet(p);
            if (min >= 0 && max > 0)
            {
                var xoro = new Xoroshiro128Plus(seed);
                xoro.NextInt(100);
                var rateRand = xoro.NextInt(max);
                if ((uint)(rateRand - min) < encounter.RandRate)
                    return encounter;
            }
        }
        return null;
    }

    public static EncounterRaid9[] FilterDistEncounters(uint seed, GameVersion version, GameProgress progress, EncounterRaid9[] encounters, int groupid, ushort species)
    {
        var res = new List<EncounterRaid9>();
        var p = progress switch
        {
            GameProgress.Unlocked6Stars or GameProgress.Unlocked5Stars => 3,
            GameProgress.Unlocked4Stars => 2,
            GameProgress.Unlocked3Stars => 1,
            _ => 0,
        };

        foreach (var encounter in encounters)
        {
            if (encounter.Species != species || encounter.Index != groupid)
                continue;
            var max = version is GameVersion.SL ? encounter.GetRandRateTotalScarlet(p) : encounter.GetRandRateTotalViolet(p);
            var min = version is GameVersion.SL ? encounter.GetRandRateMinScarlet(p) : encounter.GetRandRateMinViolet(p);
            if (min >= 0 && max > 0)
            {
                var xoro = new Xoroshiro128Plus(seed);
                xoro.NextInt(100);
                var rateRand = xoro.NextInt(max);
                if ((uint)(rateRand - min) < encounter.RandRate)
                    res.Add(encounter);
            }
        }

        return [..res];
    }

    public static byte GetGender(EncounterRaid9 enc, bool isMighty)
    {
        if (isMighty)
        {
            return enc.Gender switch
            {
                0 => PersonalInfo.RatioMagicMale,
                1 => PersonalInfo.RatioMagicFemale,
                2 => PersonalInfo.RatioMagicGenderless,
                _ => PersonalTable.SV.GetFormEntry(enc.Species, enc.Form).Gender,
            };
        }
        return PersonalTable.SV.GetFormEntry(enc.Species, enc.Form).Gender;
    }

    public static TeraDetails CalcRNG(uint seed, uint tid, uint sid, RaidContent content, EncounterRaid9 encounter, int groupid, ulong calc = 0)
    {
        var param = new GenerateParam9
        {
            Species = encounter.Species,
            GenderRatio = GetGender(encounter, content is RaidContent.Event_Mighty),
            FlawlessIVs = encounter.FlawlessIVCount,
            RollCount = 1,
            Height = 0,
            Weight = 0,
            ScaleType = encounter.ScaleType,
            Scale = encounter.Scale,
            Ability = encounter.Ability,
            Shiny = encounter.Shiny,
            Nature = encounter.Nature,
            IVs = encounter.IVs,
        };

        var pkm = new PK9
        {
            Species = encounter.Species,
            Form = encounter.Form,
            TrainerTID7 = tid,
            TrainerSID7 = sid,
            TeraTypeOriginal = (MoveType)Tera9RNG.GetTeraType(seed, encounter.TeraType, encounter.Species, encounter.Form),
        };

        Encounter9RNG.GenerateData(pkm, param, EncounterCriteria.Unrestricted, seed);
        var shiny = pkm.IsShiny ? (pkm.ShinyXor == 0 ? TeraShiny.Square : TeraShiny.Star) : TeraShiny.No;

        var result = new TeraDetails
        {
            Seed = seed,
            Stars = encounter.Stars,
            Species = encounter.Species,
            Level = encounter.Level,
            Form = encounter.Form,
            TeraType = (sbyte)pkm.TeraTypeOriginal,
            EC = pkm.EncryptionConstant,
            PID = pkm.PID,
            HP = pkm.IV_HP,
            ATK = pkm.IV_ATK,
            DEF = pkm.IV_DEF,
            SPA = pkm.IV_SPA,
            SPD = pkm.IV_SPD,
            SPE = pkm.IV_SPE,
            Ability = pkm.Ability,
            Nature = (byte)pkm.Nature,
            Gender = (Gender)pkm.Gender,
            Shiny = shiny,
            Height = pkm.HeightScalar,
            Weight = pkm.WeightScalar,
            Scale = pkm.Scale,
            Move1 = encounter.Moves.Move1,
            Move2 = encounter.Moves.Move2,
            Move3 = encounter.Moves.Move3,
            Move4 = encounter.Moves.Move4,
            EventIndex = (byte)groupid,
            Calcs = calc,
        };
        return result;
    }

    public static int GetDeliveryGroupID(SAV9SV sav, GameProgress progress, RaidContent content, EncounterRaid9[]? Dist, RaidSpawnList9 raids, int currRaid = -1)
    {
        var p = progress switch
        {
            GameProgress.Unlocked6Stars or GameProgress.Unlocked5Stars => 3,
            GameProgress.Unlocked4Stars => 2,
            GameProgress.Unlocked3Stars => 1,
            _ => 0,
        };

        var possibleGroups = new HashSet<int>();
        if (content is RaidContent.Event or RaidContent.Event_Mighty && Dist is not null)
            foreach (var enc in Dist)
                if (((GameVersion)sav.Game is GameVersion.SL && enc.GetRandRateTotalScarlet(p) > 0) ||
                    ((GameVersion)sav.Game is GameVersion.VL && enc.GetRandRateTotalViolet(p) > 0))
                    possibleGroups.Add(enc.Index);

        var eventCount = content >= RaidContent.Event ? GetEventCount(raids, currRaid+1) : 0;

        var priority = EventUtil.GetDeliveryPriority(sav);
        var groupid = priority is not null ? GetDeliveryGroupID(eventCount, priority.GroupID.Groups, possibleGroups) : 0;

        return groupid;
    }

    private static int GetEventCount(RaidSpawnList9 raids, int selected)
    {
        var count = 0;
        for (var i = 0; i < selected; i++)
            if ((RaidContent)raids.GetRaid(i).Content >= RaidContent.Event)
                count++;
        return count;
    }

    //From https://github.com/LegoFigure11/RaidCrawler/blob/7e764a9a5c0aa74270b3679083c813471abc55d6/Structures/TeraDistribution.cs#L145
    //GPL v3 License
    //Thanks LegoFigure11 & architade!
    private static int GetDeliveryGroupID(int eventct, pkNX.Structures.FlatBuffers.SV.GroupSet ids, HashSet<int> possible_groups)
    {
        if (eventct > -1 && possible_groups.Count > 0)
        {
            var cts = new int[10];
            for (var i = 0; i < ids.Groups_Length; i++)
                cts[i] = pkNX.Structures.FlatBuffers.SV.GroupSet.Groups_Item(ref ids, i);

            for (int i = 0; i < cts.Length; i++)
            {
                var ct = cts[i];
                if (!possible_groups.Contains(i + 1))
                    continue;
                if (eventct <= ct)
                    return i + 1;
                eventct -= ct;
            }
        }
        return 0;
    }

    //From https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/Areas.cs
    //GPL v3 License
    //Thanks LegoFigure11!
    public static string[] AreaPaldea = [
        "",
        "South Province (Area 1)",
        "", // Unused
        "", // Unused
        "South Province (Area 2)",
        "South Province (Area 4)",
        "South Province (Area 6)",
        "South Province (Area 5)",
        "South Province (Area 3)",
        "West Province (Area 1)",
        "Asado Desert",
        "West Province (Area 2)",
        "West Province (Area 3)",
        "Tagtree Thicket",
        "East Province (Area 3)",
        "East Province (Area 1)",
        "East Province (Area 2)",
        "Dalizapa Passage",
        "Casseroya Lake",
        "Glaseado Mountain",
        "North Province (Area 3)",
        "North Province (Area 1)",
        "North Province (Area 2)",
    ];

    //Thanks santacrab2!
    public static string[] AreaKitakami = [
        "",
        "Kitakami Road",
        "Apple Hills",
        "Revelers Road",
        "Oni Mountain",
        "Infernal Pass",
        "Crystal Pool",
        "Wistful Fields",
        "Mossfell Confluence",
        "Fellhorn Gorge",
        "Paradise Barrens",
        "Kitakami Wilds",
    ];

    //From https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/Areas.cs
    //GPL v3 License
    //Thanks LegoFigure11!
    public static string[] AreaBlueberry =
    [
        "",
        "Savannna Biome",
        "Coastal Biome",
        "Canyon Biome",
        "Polar Biome",
        "Savanna Biome",
        "Coastal Biome",
        "Canyon Biome",
        "Polar Biome",
    ];
}
