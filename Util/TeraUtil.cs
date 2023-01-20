using PKHeX.Core;

namespace TeraFinder
{
    public static class TeraUtil
    {
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

        public static List<string> GetAvailableSpecies(SAV9SV sav, string language, int stars, RaidContent content)
        {
            List<string> list = new();
            var game = (GameVersion)sav.Game;

            var encounters = content is RaidContent.Event ? GetSAVDistEncounters(sav)[0] : content is RaidContent.Event_Mighty ? GetSAVDistEncounters(sav)[1] :
                EncounterRaid9.GetEncounters(EncounterTera9.GetArray(Properties.Resources.encounter_gem_paldea));

            foreach (var encounter in encounters)
            {
                if ((encounter.Version is GameVersion.SV || encounter.Version == game) && (stars == 0 || encounter.Stars == stars))
                {
                    var forms = FormConverter.GetFormList(encounter.Species, GameInfo.GetStrings(language).Types, GameInfo.GetStrings(language).forms, GameInfo.GenderSymbolASCII, EntityContext.Gen9);
                    var names = GameInfo.GetStrings(language).Species;
                    var str = $"{names[encounter.Species]}{(forms.Length > 1 ? $"-{forms[encounter.Form]}" : "")}";
                    if (!list.Contains(str))
                        list.Add(str);
                }
            }
            return list;
        }

        public static EncounterRaid9[] GetAllTeraEncounters() => EncounterRaid9.GetEncounters(EncounterTera9.GetArray(Properties.Resources.encounter_gem_paldea));

        public static EncounterRaid9[][] GetAllDistEncounters()
        {
            var dist = EncounterRaid9.GetEncounters(PKHeX.Core.EncounterDist9.GetArray(Util.GetBinaryResource("encounter_dist_paldea.pkl")));
            var mighty = EncounterRaid9.GetEncounters(PKHeX.Core.EncounterMight9.GetArray(Util.GetBinaryResource("encounter_might_paldea.pkl")));
            return new EncounterRaid9[][] { dist, mighty };
        }

        public static EncounterRaid9[][] GetSAVDistEncounters(SAV9SV sav)
        {
            var KBCATEventRaidIdentifier = sav.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);
            if (KBCATEventRaidIdentifier.Type is not SCTypeCode.None && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0)
            {
                try
                {
                    var events = EventUtil.GetEventEncounterDataFromSAV(sav);
                    var dist = EncounterRaid9.GetEncounters(EncounterDist9.GetArray(events[0]));
                    var mighty = EncounterRaid9.GetEncounters(EncounterMight9.GetArray(events[1]));
                    return new EncounterRaid9[][] { dist, mighty };
                }
                catch
                {
                    var encounters = GetAllTeraEncounters();
                    return new EncounterRaid9[][] { encounters, encounters };
                }
            }
            else
            {
                var encounters = GetAllTeraEncounters();
                return new EncounterRaid9[][] { encounters, encounters };
            }
        }

        public static EncounterRaid9? GetTeraEncounter(uint seed, SAV9SV sav, int stars, EncounterRaid9[] encounters)
        {
            var game = (GameVersion)sav.Game;
            var xoro = new Xoroshiro128Plus(seed);
            if (stars < 6) xoro.NextInt(100);
            var max = game is GameVersion.SL ? EncounterTera9.GetRateTotalBaseSL(stars) : EncounterTera9.GetRateTotalBaseVL(stars);
            var rateRand = (int)xoro.NextInt((uint)max);
            foreach (var encounter in encounters)
            {
                var min = game is GameVersion.SL ? encounter.RandRateMinScarlet : encounter.RandRateMinViolet;
                if (encounter.Stars == stars && min >= 0 && (uint)(rateRand - min) < encounter.RandRate)
                    return encounter;
            }
            return null;
        }

        public static EncounterRaid9? GetDistEncounter(uint seed, SAV9SV sav, GameProgress progress, EncounterRaid9[] encounters)
        {
            var game = (GameVersion)sav.Game;
            var p = progress switch
            {
                GameProgress.Unlocked6Stars or GameProgress.Unlocked5Stars => 3,
                GameProgress.Unlocked4Stars => 2,
                GameProgress.Unlocked3Stars => 1,
                _ => 0,
            };

            foreach (var encounter in encounters)
            {
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

        public static EncounterRaid9? GetDistEncounterWithIndex(uint seed, SAV9SV sav, GameProgress progress, EncounterRaid9[] encounters, int index)
        {
            if (index < 0)
                return null;

            var encounter = new EncounterRaid9[] { encounters[index] };
            return GetDistEncounter(seed, sav, progress, encounter);
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
                    _ => (byte)PersonalTable.SV.GetFormEntry(enc.Species, enc.Form).Gender,
                };
            }
            return (byte)PersonalTable.SV.GetFormEntry(enc.Species, enc.Form).Gender;
        }

        public static TeraDetails CalcRNG(uint seed, uint tid, uint sid, RaidContent content, EncounterRaid9 encounter, ulong calc = 0)
        {
            var param = new GenerateParam9
            {
                GenderRatio = TeraUtil.GetGender(encounter, content is RaidContent.Event_Mighty),
                FlawlessIVs = encounter.FlawlessIVCount,
                RollCount = 1,
                Height = 0,
                Weight = 0,
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
                Calcs = calc,
            };
            return result;
        }

        //From https://github.com/LegoFigure11/RaidCrawler/blob/main/Structures/Areas.cs
        //GPL v3 License
        //Thanks LegoFigure11!
        public static string[] Area = new string[] {
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
        };
    }
}
