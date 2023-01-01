using PKHeX.Core;

namespace TeraFinder
{
    public class TeraDetails
    {
        public uint Seed { get; set; }
        public TeraShiny Shiny { get; set; }
        public int Stars { get; set; }
        public Species Species { get; set; }
        public int Form { get; set; }
        public int Level { get; set; }
        public MoveType TeraType { get; set; }
        public uint EC { get; set; }
        public uint PID { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int SPA { get; set; }
        public int SPD { get; set; }
        public int SPE { get; set; }
        public Ability Ability { get; set; }
        public Nature Nature { get; set; }
        public Gender Gender { get; set; }
        public byte Height { get; set; }
        public byte Weight { get; set; }
        public byte Scale { get; set; }
        public Move Move1 { get; set; }
        public Move Move2 { get; set; }
        public Move Move3 { get; set; }
        public Move Move4 { get; set; }
        public uint Calcs { get; set; }


        public int GetAbilityNumber()
        {
            var entry = PersonalTable.SV.GetFormEntry((ushort)Species, (byte)Form);
            if (Ability == (Ability)entry.Ability1)
                return 1;
            else if (Ability == (Ability)entry.Ability2)
                return 2;
            //else if (Ability == (Ability)entry.AbilityH)
                return 3;
        }

        public string[] GetStrings()
        {
            var list = new string[25];
            list[0] = ($"{Seed:X8}");
            list[1] = ($"{Shiny}");
            list[2] = ($"{Stars}");
            list[3] = ($"{GetName()}");
            list[4] = ($"{Level}");
            list[5] = ($"{TeraType}");
            list[6] = ($"{EC:X8}");
            list[7] = ($"{PID:X8}");
            list[8] = ($"{HP}");
            list[9] = ($"{ATK}");
            list[10] = ($"{DEF}");
            list[11] = ($"{SPA}");
            list[12] = ($"{SPD}");
            list[13] = ($"{SPE}");
            list[14] = ($"{GetAbilityName()}");
            list[15] = ($"{Nature}");
            list[16] = ($"{GetGenderSymbol()}");
            list[17] = ($"{Height}");
            list[18] = ($"{Weight}");
            list[19] = ($"{Scale}");
            list[20] = ($"{Move1}");
            list[21] = ($"{Move2}");
            list[22] = ($"{Move3}");
            list[23] = ($"{Move4}");
            list[24] = ($"{Calcs}");
            return list;
        }

        private string GetName()
        {
            var forms = FormConverter.GetFormList((ushort)Species, GameInfo.Strings.Types, GameInfo.Strings.forms, GameInfo.GenderSymbolASCII, EntityContext.Gen9);
            return $"{Species}{(forms.Length > 1 ? $"-{forms[Form]}" : "")}";
        }

        private string GetAbilityName()
        {
            var abilites = GameInfo.Strings.abilitylist;
            var num = GetAbilityNumber();
            return $"{abilites[(int)Ability]} ({(num == 3 ? "H" : num)})";
        }

        private string GetGenderSymbol()
        {
            if (Gender is Gender.Male)
                return "♂️";
            else if (Gender is Gender.Female)
                return "♀️";
            else 
                return Gender.ToString();
        }
    }

    public class GridEntry
    {
        public string? Seed { get; private set; }
        public string? Shiny { get; private set; }
        public string? Stars { get; private set; }
        public string? Species { get; private set; }
        public string? Level { get; private set; }
        public string? TeraType { get; private set; }
        public string? EC { get; private set; }
        public string? PID { get; private set; }
        public string? HP { get; private set; }
        public string? ATK { get; private set; }
        public string? DEF { get; private set; }
        public string? SPA { get; private set; }
        public string? SPD { get; private set; }
        public string? SPE { get; private set; }
        public string? Ability { get; private set; }
        public string? Nature { get; private set; }
        public string? Gender { get; private set; }
        public string? Height { get; private set; }
        public string? Weight { get; private set; }
        public string? Scale { get; private set; }
        public string? Move1 { get; private set; }
        public string? Move2 { get; private set; }
        public string? Move3 { get; private set; }
        public string? Move4 { get; private set; }
        public string? Calcs { get; private set; }

        public GridEntry(TeraDetails res)
        {
            var str = res.GetStrings();
            Seed = str[0];
            Shiny = str[1];
            Stars = str[2];
            Species = str[3];
            Level = str[4];
            TeraType = str[5];
            EC = str[6];
            PID = str[7];
            HP = str[8];
            ATK = str[9];
            DEF = str[10];
            SPA = str[11];
            SPD = str[12];
            SPE = str[13];
            Ability = str[14];
            Nature = str[15];
            Gender = str[16];
            Height = str[17];
            Weight = str[18];
            Scale = str[19];
            Move1 = str[20];
            Move2 = str[21];
            Move3 = str[22];
            Move4 = str[23];
            Calcs = str[24];
        }

        public static List<GridEntry> GetGridEntriesFromList(List<TeraDetails> reslist)
        {
            var list = new List<GridEntry>();
            foreach (var res in reslist)
                list.Add(new GridEntry(res));
            return list;
        }
    }

    public class TeraFilter
    {
        public int MinHP { get; set; }
        public int MaxHP { get; set; }
        public int MinAtk { get; set; }
        public int MaxAtk { get; set; }
        public int MinDef { get; set; }
        public int MaxDef { get; set; }
        public int MinSpa { get; set; }
        public int MaxSpa { get; set; }
        public int MinSpd { get; set; }
        public int MaxSpd { get; set; }
        public int MinSpe { get; set; }
        public int MaxSpe { get; set; }
        public int MinScale { get; set; }
        public int MaxScale { get; set; }
        public int Stars { get; set; }
        public Species Species { get; set; }
        public int Form { get; set; }
        public MoveType TeraType { get; set; }
        public int AbilityNumber { get; set; }
        public Nature Nature { get; set; }
        public Gender Gender { get; set; }
        public TeraShiny Shiny { get; set; }
        public bool AltEC { get; set; }

        public bool IsFilterMatch(TeraDetails res)
        {
            if (!(MinHP <= res.HP && res.HP <= MaxHP))
                return false;
            if (!(MinAtk <= res.ATK && res.ATK <= MaxAtk))
                return false;
            if (!(MinDef <= res.DEF && res.DEF <= MaxDef))
                return false;
            if (!(MinSpa <= res.SPA && res.SPA <= MaxSpa))
                return false;
            if (!(MinSpd <= res.SPD && res.SPD <= MaxSpd))
                return false;
            if (!(MinSpe <= res.SPE && res.SPE <= MaxSpe))
                return false;
            if (!(MinScale <= res.Scale && res.Scale <= MaxScale))
                return false;

            if (Stars != 0 && Stars != res.Stars)
                return false;

            if (Species != 0 && Species != res.Species)
                return false;
            if (Form != 0 && Form != res.Form)
                return false;

            if (TeraType != MoveType.Any && TeraType != res.TeraType)
                return false;
            if (AbilityNumber != 0 && AbilityNumber != res.GetAbilityNumber())
                return false;
            if(Nature != Nature.Random && Nature != res.Nature)
                return false;
            if (Gender != Gender.Random && Gender != res.Gender)
                return false;

            if (Shiny is TeraShiny.No && res.Shiny >= TeraShiny.Yes)
                return false;
            if (Shiny is TeraShiny.Yes && res.Shiny < TeraShiny.Yes)
                return false;
            if (Shiny > TeraShiny.Yes && Shiny != res.Shiny)
                return false;

            if (AltEC && res.EC % 100 != 0)
                return false;

            return true;
        }

        public bool IsFilterNull()
        {
            if (!(MinHP == 0))
                return false;
            if (!(MinAtk == 0))
                return false;
            if (!(MinDef == 0))
                return false;
            if (!(MinSpa == 0))
                return false;
            if (!(MinSpd == 0))
                return false;
            if (!(MinSpe == 0))
                return false;
            if (!(MaxHP == 31))
                return false;
            if (!(MaxAtk == 31))
                return false;
            if (!(MaxDef == 31))
                return false;
            if (!(MaxSpa == 31))
                return false;
            if (!(MaxSpd == 31))
                return false;
            if (!(MaxSpe == 31))
                return false;
            if (!(Stars == 0))
                return false;
            if (!(Species == Species.None))
                return false;
            if (!(Form == 0))
                return false;
            if (!(TeraType == MoveType.Any))
                return false;
            if (!(AbilityNumber == 0))
                return false;
            if (!(Nature == (Nature)25))
                return false;
            if (!(Gender == Gender.Random))
                return false;
            if (!(Shiny == TeraShiny.Any))
                return false;
            if (!(AltEC == false))
                return false;
            if (!(MinScale == 0))
                return false;
            if (!(MaxScale == 255))
                return false;

            return true;
        }

        public bool CompareFilter(TeraFilter res)
        {
            if (!(MinHP == res.MinHP))
                return false;
            if (!(MinAtk == res.MinAtk))
                return false;
            if (!(MinDef == res.MinDef))
                return false;
            if (!(MinSpa == res.MinSpa))
                return false;
            if (!(MinSpd == res.MinSpd))
                return false;
            if (!(MinSpe == res.MinSpe))
                return false;
            if (!(MaxHP == res.MaxHP))
                return false;
            if (!(MaxAtk == res.MaxAtk))
                return false;
            if (!(MaxDef == res.MaxDef))
                return false;
            if (!(MaxSpa == res.MaxSpa))
                return false;
            if (!(MaxSpd == res.MaxSpd))
                return false;
            if (!(MaxSpe == res.MaxSpe))
                return false;
            if (!(MinScale == res.MinScale))
                return false;
            if (!(MaxScale == res.MaxScale))
                return false;
            if (!(Stars == res.Stars))
                return false;
            if (!(Species == res.Species))
                return false;
            if (!(Form == res.Form))
                return false;
            if (!(TeraType == res.TeraType))
                return false;
            if (!(AbilityNumber == res.AbilityNumber))
                return false;
            if (!(Nature == res.Nature))
                return false;
            if (!(Gender == res.Gender))
                return false;
            if (!(Shiny == res.Shiny))
                return false;
            if (!(AltEC == res.AltEC))
                return false;

            return true;
        }
    }
}
