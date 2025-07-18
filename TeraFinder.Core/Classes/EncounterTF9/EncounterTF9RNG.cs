using PKHeX.Core;

namespace TeraFinder.Core;

//Tera Finder Adaptations of the Encounter9RNG class from PKHeX
//https://github.com/kwsch/PKHeX/blob/master/PKHeX.Core/Legality/RNG/Methods/Gen9/Encounter9RNG.cs
public static class EncounterTF9RNG
{
    public static bool GenerateData(this EncounterRaidTF9 encounter, in TeraFilter filter, in uint seed, in uint id32, in byte groupid, out TeraDetails? result)
    {
        result = null;

        var gemtype = Tera9RNG.GetTeraType(seed, encounter.TeraType, encounter.Species, encounter.Form);
        if (!filter.IsTeraMatch(gemtype))
            return false;

        var rand = new Xoroshiro128Plus(seed);

        var ec = (uint)rand.NextInt(uint.MaxValue);
        if (!filter.IsECMatch(ec))
            return false;

        var pid = GetAdaptedPID(ref rand, encounter, id32, out var shiny);
        if (!filter.IsShinyMatch(shiny))
            return false;

        const int UNSET = -1;
        const int MAX = 31;
        Span<int> ivs = [UNSET, UNSET, UNSET, UNSET, UNSET, UNSET];
        if (encounter.IVs.IsSpecified)
        {
            encounter.IVs.CopyToSpeedLast(ivs);
        }
        else
        {
            for (int i = 0; i < encounter.FlawlessIVCount; i++)
            {
                int index;
                do { index = (int)rand.NextInt(6); }
                while (ivs[index] != UNSET);
                ivs[index] = MAX;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (ivs[i] == UNSET)
                ivs[i] = (int)rand.NextInt(MAX + 1);
        }

        if (!filter.IsIVMatch(ivs))
            return false;

        int abilNum = encounter.Ability switch
        {
            AbilityPermission.Any12H => (int)rand.NextInt(3) << 1,
            AbilityPermission.Any12 => (int)rand.NextInt(2) << 1,
            _ => (int)encounter.Ability,
        };

        if (!filter.IsAbilityMatch(abilNum))
            return false;
        
        var abil = GetRefreshedAbility(encounter.Personal, abilNum >> 1);

        var gender = encounter.GenderRatio switch
        {
            PersonalInfo.RatioMagicGenderless => Gender.Genderless,
            PersonalInfo.RatioMagicFemale => Gender.Female,
            PersonalInfo.RatioMagicMale => Gender.Male,
            _ => GetGender(encounter.GenderRatio, rand.NextInt(100)),
        };

        if (!filter.IsGenderMatch(gender))
            return false;

        var nature = encounter.Nature != Nature.Random ? encounter.Nature : encounter.Species == (ushort)Species.Toxtricity
                ? ToxtricityUtil.GetRandomNature(ref rand, encounter.Form)
                : (Nature)rand.NextInt(25);
        
        if (!filter.IsNatureMatch(nature))
            return false;

        var height = (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        var weight = (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        var scale = encounter.ScaleType.GetSizeValue(encounter.Scale, ref rand);

        if (!filter.IsScaleMatch(scale))
            return false;

        result = new TeraDetails()
        {
            Seed = seed,
            Shiny = shiny,
            Stars = encounter.Stars,
            Species = encounter.Species,
            Form = encounter.Form,
            Level = encounter.Level,
            TeraType = gemtype,
            EC = ec,
            PID = pid,
            HP = ivs[0],
            ATK = ivs[1],
            DEF = ivs[2],
            SPA = ivs[3],
            SPD = ivs[4],
            SPE = ivs[5],
            Ability = abil,
            AbilityNumber = abilNum == 0 ? 1 : abilNum,
            Nature = nature,
            Gender = gender,
            Height = height,
            Weight = weight,
            Scale = scale,
            Move1 = encounter.Moves.Move1,
            Move2 = encounter.Moves.Move2,
            Move3 = encounter.Moves.Move3,
            Move4 = encounter.Moves.Move4,
            EventIndex = groupid,
        };

        return true;
    }

    public static TeraDetails GenerateData(this EncounterRaidTF9 encounter, in uint seed, in uint id32, in byte groupid = 0)
    {
        var tera = Tera9RNG.GetTeraType(seed, encounter.TeraType, encounter.Species, encounter.Form);
        var rand = new Xoroshiro128Plus(seed);
        var ec = (uint)rand.NextInt(uint.MaxValue);
        var pid = GetAdaptedPID(ref rand, encounter, id32, out var shiny);

        const int UNSET = -1;
        const int MAX = 31;
        Span<int> ivs = [UNSET, UNSET, UNSET, UNSET, UNSET, UNSET];
        if (encounter.IVs.IsSpecified)
        {
            encounter.IVs.CopyToSpeedLast(ivs);
        }
        else
        {
            for (int i = 0; i < encounter.FlawlessIVCount; i++)
            {
                int index;
                do { index = (int)rand.NextInt(6); }
                while (ivs[index] != UNSET);
                ivs[index] = MAX;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (ivs[i] == UNSET)
                ivs[i] = (int)rand.NextInt(MAX + 1);
        }

        int abilNum = encounter.Ability switch
        {
            AbilityPermission.Any12H => (int)rand.NextInt(3) << 1,
            AbilityPermission.Any12 => (int)rand.NextInt(2) << 1,
            _ => (int)encounter.Ability,
        };
        
        var abil = GetRefreshedAbility(encounter.Personal, abilNum >> 1);

        var gender = encounter.GenderRatio switch
        {
            PersonalInfo.RatioMagicGenderless => Gender.Genderless,
            PersonalInfo.RatioMagicFemale => Gender.Female,
            PersonalInfo.RatioMagicMale => Gender.Male,
            _ => GetGender(encounter.GenderRatio, rand.NextInt(100)),
        };

        var nature = encounter.Nature != Nature.Random ? encounter.Nature : encounter.Species == (ushort)Species.Toxtricity
                ? ToxtricityUtil.GetRandomNature(ref rand, encounter.Form)
                : (Nature)rand.NextInt(25);

        var height = (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        var weight = (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        var scale = encounter.ScaleType.GetSizeValue(encounter.Scale, ref rand);

        return new TeraDetails()
        {
            Seed = seed,
            Shiny = shiny,
            Stars = encounter.Stars,
            Species = encounter.Species,
            Form = encounter.Form,
            Level = encounter.Level,
            TeraType = tera,
            EC = ec,
            PID = pid,
            HP = ivs[0],
            ATK = ivs[1],
            DEF = ivs[2],
            SPA = ivs[3],
            SPD = ivs[4],
            SPE = ivs[5],
            Ability = abil,
            AbilityNumber = abilNum == 0 ? 1 : abilNum,
            Nature = nature,
            Gender = gender,
            Height = height,
            Weight = weight,
            Scale = scale,
            Move1 = encounter.Moves.Move1,
            Move2 = encounter.Moves.Move2,
            Move3 = encounter.Moves.Move3,
            Move4 = encounter.Moves.Move4,
            EventIndex = groupid,
        };
    }

    private static uint GetAdaptedPID(ref Xoroshiro128Plus rand, EncounterRaidTF9 encounter, uint id32, out TeraShiny shiny)
    {
        var fakeTID = (uint)rand.NextInt();
        var pid = (uint)rand.NextInt();

        if (encounter.Shiny is Shiny.Random)
        {
            var xor = ShinyUtil.GetShinyXor(pid, fakeTID);
            if (xor < 16)
            {
                if (xor != 0) xor = 1;
                ShinyUtil.ForceShinyState(true, ref pid, id32, xor);
                shiny = xor == 0 ? TeraShiny.Square : TeraShiny.Star;
            }
            else
            {
                ShinyUtil.ForceShinyState(false, ref pid, id32, xor);
                shiny = TeraShiny.No;
            }
        }
        else if (encounter.Shiny is Shiny.Always)
        {
            var tid = (ushort)fakeTID;
            var sid = (ushort)(fakeTID >> 16);
            var xor = ShinyUtil.GetShinyXor(pid, fakeTID);
            if (xor > 16)
                pid = ShinyUtil.GetShinyPID(tid, sid, pid, 0);
            if (!ShinyUtil.GetIsShiny6(id32, pid))
            {
                xor = ShinyUtil.GetShinyXor(pid, fakeTID);
                pid = ShinyUtil.GetShinyPID(TidUtil.GetTID16(id32), TidUtil.GetSID16(id32), pid, xor == 0 ? 0u : 1u);
            }
            shiny = xor == 0 ? TeraShiny.Square : TeraShiny.Star;
        }
        else
        {
            if (ShinyUtil.GetIsShiny6(fakeTID, pid))
                pid ^= 0x1000_0000;
            if (ShinyUtil.GetIsShiny6(id32, pid))
                pid ^= 0x1000_0000;
            shiny = TeraShiny.No;
        }
        return pid;
    }

    private static int GetRefreshedAbility(PersonalInfo9SV info, int n)
    {
        if ((uint)n < info.AbilityCount)
            n = info.GetAbilityAtIndex(n);

        return n;
    }

    private static Gender GetGender(in int ratio, in ulong rand100) => ratio switch
    {
        0x1F => rand100 < 12 ? Gender.Female : Gender.Male, // 12.5%
        0x3F => rand100 < 25 ? Gender.Female : Gender.Male, // 25%
        0x7F => rand100 < 50 ? Gender.Female : Gender.Male, // 50%
        0xBF => rand100 < 75 ? Gender.Female : Gender.Male, // 75%
        0xE1 => rand100 < 89 ? Gender.Female : Gender.Male, // 87.5%

        _ => throw new ArgumentOutOfRangeException(nameof(ratio)),
    };
}
