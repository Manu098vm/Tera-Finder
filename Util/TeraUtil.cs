using PKHeX.Core;

namespace TeraFinder
{
    public static class TeraUtil
    {
        public static GameProgress GetProgress(SAV9SV sav)
        {
            try
            {
                var Unlocked6Stars = sav.AllBlocks.Where(block => block.Key == 0x6E7F8220).Select(block => block.Type).ToArray()[0] is SCTypeCode.Bool2;
                
                if (Unlocked6Stars)
                    return GameProgress.Unlocked6Stars;

                var Unlocked5Stars = sav.AllBlocks.Where(block => block.Key == 0x9535F471).Select(block => block.Type).ToArray()[0] is SCTypeCode.Bool2;

                if (Unlocked5Stars)
                    return GameProgress.Unlocked5Stars;

                var Unlocked4Stars = sav.AllBlocks.Where(block => block.Key == 0xA9428DFE).Select(block => block.Type).ToArray()[0] is SCTypeCode.Bool2;

                if (Unlocked4Stars)
                    return GameProgress.Unlocked4Stars;

                var Unlocked3Stars = sav.AllBlocks.Where(block => block.Key == 0xEC95D8EF).Select(block => block.Type).ToArray()[0] is SCTypeCode.Bool2;

                if (Unlocked3Stars)
                    return GameProgress.Unlocked3Stars;

                var UnlockedTeraRaids = sav.AllBlocks.Where(block => block.Key == 0x27025EBF).Select(block => block.Type).ToArray()[0] is SCTypeCode.Bool2;

                if (UnlockedTeraRaids)
                    return GameProgress.UnlockedTeraRaids;

                return GameProgress.Beginning;
            }
            catch
            {
                //Blank save file
                return GameProgress.Beginning;
            }
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

            if (progress is GameProgress.UnlockedTeraRaids && rand <= 80 ||
                progress is GameProgress.Unlocked3Stars && rand <= 30 ||
                progress is GameProgress.Unlocked4Stars && rand <= 20)
                return 1;

            else if (progress is GameProgress.UnlockedTeraRaids && rand > 80 ||
                     progress is GameProgress.Unlocked3Stars && rand <= 70 ||
                     progress is GameProgress.Unlocked4Stars && rand <= 40)
                return 2;

            else if (progress is GameProgress.Unlocked3Stars && rand > 70 ||
                     progress is GameProgress.Unlocked4Stars && rand <= 70 ||
                     progress is GameProgress.Unlocked5Stars && rand <= 40 ||
                     progress is GameProgress.Unlocked6Stars && rand <= 30)
                return 3;

            else if (progress is GameProgress.Unlocked4Stars && rand > 70 ||
                     progress is GameProgress.Unlocked5Stars && rand <= 75 ||
                     progress is GameProgress.Unlocked6Stars && rand <= 70)
                return 4;

            else if (progress is GameProgress.Unlocked5Stars && rand > 75 ||
                     progress is GameProgress.Unlocked6Stars && rand > 70)
                return 5;

            else
                throw new ArgumentOutOfRangeException();
        }

        public static List<string> GetAvailableSpecies(SAV9SV sav, int stars, RaidContent content)
        {
            List<string> list = new();
            var game = (GameVersion)sav.Game;

            var encounters = content is RaidContent.Event ? EncounterRaid9.GetEncounters(EncounterDist9.GetArray(EventUtil.GetEventEncounterDataFromSAV(sav)[0])) :
                content is RaidContent.Event_Mighty ? EncounterRaid9.GetEncounters(EncounterMight9.GetArray(EventUtil.GetEventEncounterDataFromSAV(sav)[1])) :
                EncounterRaid9.GetEncounters(EncounterTera9.GetArray(Properties.Resources.encounter_gem_paldea));

            foreach (var encounter in encounters)
            {
                if ((encounter.Version is GameVersion.SV || encounter.Version == game) && (stars == 0 || encounter.Stars == stars))
                {
                    var forms = FormConverter.GetFormList(encounter.Species, GameInfo.Strings.Types, GameInfo.Strings.forms, GameInfo.GenderSymbolASCII, EntityContext.Gen9);
                    var str = $"{(Species)encounter.Species}{(forms.Length > 1 ? $"-{forms[encounter.Form]}" : "")}";
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
            var events = EventUtil.GetEventEncounterDataFromSAV(sav);
            var dist = EncounterRaid9.GetEncounters(EncounterDist9.GetArray(events[0]));
            var mighty = EncounterRaid9.GetEncounters(EncounterMight9.GetArray(events[1]));
            return new EncounterRaid9[][] { dist, mighty };
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
                    return new EncounterRaid9(encounter);
            }
            return null;
        }

        public static EncounterRaid9? GetDistEncounter(uint seed, SAV9SV sav, GameProgress progress, EncounterRaid9[] encounters, bool isMighty)
        {
            var game = (GameVersion)sav.Game;
            var p = isMighty ? progress is GameProgress.Unlocked6Stars ? 3 : 0 : progress switch
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
                        return new EncounterRaid9(encounter);
                }
            }
            return null;
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

        public static TeraDetails CalcRNG(uint seed, int tid, int sid, RaidContent content, EncounterRaid9 encounter, uint calc = 0)
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
                TrainerID7 = tid,
                TrainerSID7 = sid,
                TeraTypeOriginal = (MoveType)Tera9RNG.GetTeraType(seed, encounter.TeraType, encounter.Species, encounter.Form),
            };

            Encounter9RNG.GenerateData(pkm, param, EncounterCriteria.Unrestricted, seed);
            var shiny = pkm.IsShiny ? (pkm.ShinyXor == 0 ? TeraShiny.Square : TeraShiny.Star) : TeraShiny.No;

            var result = new TeraDetails
            {
                Seed = seed,
                Stars = encounter.Stars,
                Species = (Species)encounter.Species,
                Form = encounter.Form,
                TeraType = pkm.TeraTypeOriginal,
                EC = pkm.EncryptionConstant,
                PID = pkm.PID,
                HP = pkm.IV_HP,
                ATK = pkm.IV_ATK,
                DEF = pkm.IV_DEF,
                SPA = pkm.IV_SPA,
                SPD = pkm.IV_SPD,
                SPE = pkm.IV_SPE,
                Ability = (Ability)pkm.Ability,
                Nature = (Nature)pkm.Nature,
                Gender = (Gender)pkm.Gender,
                Shiny = shiny,
                Height = pkm.HeightScalar,
                Weight = pkm.WeightScalar,
                Scale = pkm.Scale,
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
