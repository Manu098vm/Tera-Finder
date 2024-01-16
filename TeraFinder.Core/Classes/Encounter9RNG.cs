using PKHeX.Core;

namespace TeraFinder.Core;

//Tera Finder Adaptations of the Encounter9RNG class from PKHeX
//https://github.com/kwsch/PKHeX/blob/master/PKHeX.Core/Legality/RNG/Methods/Gen9/Encounter9RNG.cs
public static class Encounter9RNG
{
    private static readonly PersonalTable9SV Table = PersonalTable.SV;

    public static bool GenerateData(TeraDetails pk, in GenerateParam9 enc, uint id32, in ulong seed)
    {
        var criteria = EncounterCriteria.Unrestricted;
        var rand = new Xoroshiro128Plus(seed);
        pk.EC = (uint)rand.NextInt(uint.MaxValue);
        pk.PID = GetAdaptedPID(ref rand, pk, id32, enc);

        const int UNSET = -1;
        const int MAX = 31;
        Span<int> ivs = [UNSET, UNSET, UNSET, UNSET, UNSET, UNSET];
        if (enc.IVs.IsSpecified)
        {
            enc.IVs.CopyToSpeedLast(ivs);
        }
        else
        {
            for (int i = 0; i < enc.FlawlessIVs; i++)
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

        if (!criteria.IsIVsCompatibleSpeedLast(ivs, 9))
            return false;

        pk.HP = ivs[0];
        pk.ATK = ivs[1];
        pk.DEF = ivs[2];
        pk.SPA = ivs[3];
        pk.SPD = ivs[4];
        pk.SPE = ivs[5];

        int abil = enc.Ability switch
        {
            AbilityPermission.Any12H => (int)rand.NextInt(3) << 1,
            AbilityPermission.Any12 => (int)rand.NextInt(2) << 1,
            _ => (int)enc.Ability,
        };

        pk.RefreshAbility(abil >> 1);

        var gender_ratio = enc.GenderRatio;
        pk.Gender = gender_ratio switch
        {
            PersonalInfo.RatioMagicGenderless => Gender.Genderless,
            PersonalInfo.RatioMagicFemale => Gender.Female,
            PersonalInfo.RatioMagicMale => Gender.Male,
            _ => GetGender(gender_ratio, rand.NextInt(100)),
        }; ;

        byte nature = enc.Nature != Nature.Random ? (byte)enc.Nature : enc.Species == (int)Species.Toxtricity
                ? ToxtricityUtil.GetRandomNature(ref rand, pk.Form)
                : (byte)rand.NextInt(25);
        if (criteria.Nature != Nature.Random && nature != (int)criteria.Nature)
            return false;
        pk.Nature = nature;

        pk.Height = enc.Height != 0 ? enc.Height : (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        pk.Weight = enc.Weight != 0 ? enc.Weight : (byte)(rand.NextInt(0x81) + rand.NextInt(0x80));
        pk.Scale = enc.ScaleType.GetSizeValue(enc.Scale, ref rand);
        return true;
    }

    private static uint GetAdaptedPID(ref Xoroshiro128Plus rand, TeraDetails pk, uint id32, in GenerateParam9 enc)
    {
        var fakeTID = (uint)rand.NextInt();
        uint pid = (uint)rand.NextInt();
        if (enc.Shiny == Shiny.Random) // let's decide if it's shiny or not!
        {
            int i = 1;
            bool isShiny;
            uint xor;
            while (true)
            {
                xor = ShinyUtil.GetShinyXor(pid, fakeTID);
                isShiny = xor < 16;
                if (isShiny)
                {
                    if (xor != 0)
                        xor = 1;
                    break;
                }
                if (i >= enc.RollCount)
                    break;
                pid = (uint)rand.NextInt();
                i++;
            }
            ShinyUtil.ForceShinyState(isShiny, ref pid, id32, xor);
            pk.Shiny = xor > 15 ? TeraShiny.No : xor == 0 ? TeraShiny.Square : TeraShiny.Star;
        }
        else if (enc.Shiny == Shiny.Always)
        {
            var tid = (ushort)fakeTID;
            var sid = (ushort)(fakeTID >> 16);
            if (!ShinyUtil.GetIsShiny(fakeTID, pid)) // battled
                pid = ShinyUtil.GetShinyPID(tid, sid, pid, 0);
            if (!ShinyUtil.GetIsShiny(id32, pid)) // captured
                pid = ShinyUtil.GetShinyPID(TeraUtil.GetTID16(id32), TeraUtil.GetSID16(id32), pid, ShinyUtil.GetShinyXor(pid, fakeTID) == 0 ? 0u : 1u);
        }
        else // Never
        {
            if (ShinyUtil.GetIsShiny(fakeTID, pid)) // battled
                pid ^= 0x1000_0000;
            if (ShinyUtil.GetIsShiny(id32, pid)) // captured
                pid ^= 0x1000_0000;
        }
        return pid;
    }

    private static void RefreshAbility(this TeraDetails pkm, int n)
    {
        var pi = Table[pkm.Species, pkm.Form];
        if ((uint)n < pi.AbilityCount)
            pkm.Ability = pi.GetAbilityAtIndex(n);
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
