using PKHeX.Core;
using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core;

public sealed record EncounterTeraTF9 : EncounterRaidTF9
{
    public const int SerializedSize = 0x3C;

    public required short RandRateMinScarlet { get; init; }
    public required short RandRateMinViolet { get; init; }

    public override bool CanBeEncountered(uint seed) => Tera9RNG.IsMatchStarChoice(seed, Stars, RandRate, RandRateMinScarlet, RandRateMinViolet, Map);

    public static EncounterTeraTF9[] GetArray(ReadOnlySpan<byte> data, Dictionary<ulong, List<Reward>> fixedRewards, Dictionary<ulong, List<Reward>> lotteryRewards, TeraRaidMapParent map)
    {
        var count = data.Length / SerializedSize;
        var encounters = new EncounterTeraTF9[count];
        for (var i = 0; i < count; i++)
            encounters[i] = ReadEncounter(data.Slice(i * SerializedSize, SerializedSize), fixedRewards, lotteryRewards, map);
        return encounters;
    }

    private static EncounterTeraTF9 ReadEncounter(ReadOnlySpan<byte> data, Dictionary<ulong, List<Reward>> fixedRewards, Dictionary<ulong, List<Reward>> lotteryRewards, TeraRaidMapParent map)
    {
        var species = ReadUInt16LittleEndian(data);
        var form = data[0x02];
        var stars = data[0x12];
        var randRateMinScarlet = ReadInt16LittleEndian(data[0x14..]);
        var randRateMinViolet = ReadInt16LittleEndian(data[0x16..]);
        var personal = PersonalTable.SV.GetFormEntry(species, form);

        var fxRewards = DeepCopyList(GetRewardList(ReadUInt64LittleEndian(data[0x1C..]), fixedRewards));
        var ltRewards = DeepCopyList(GetRewardList(ReadUInt64LittleEndian(data[0x24..]), lotteryRewards));
        fxRewards.ReplaceMaterialReward((Species)species);
        ltRewards.ReplaceMaterialReward((Species)species);

        return new()
        {
            Personal = personal,
            Species = species,
            Form = form,
            Gender = (byte)(data[0x03] - 1),
            GenderRatio = GetGenderRatio(personal),
            Ability = GetAbility(data[0x04]),
            FlawlessIVCount = data[5],
            Shiny = data[0x06] switch { 0 => Shiny.Random, 1 => Shiny.Never, 2 => Shiny.Always, _ => throw new ArgumentOutOfRangeException(nameof(data)) },
            Level = data[0x07],
            Moves = new Moveset(
                ReadUInt16LittleEndian(data[0x08..]),
                ReadUInt16LittleEndian(data[0x0A..]),
                ReadUInt16LittleEndian(data[0x0C..]),
                ReadUInt16LittleEndian(data[0x0E..])),
            TeraType = (GemType)data[0x10],
            Index = data[0x11],
            Stars = stars,
            RandRate = data[0x13],
            RandRateMinScarlet = randRateMinScarlet,
            RandRateMinViolet = randRateMinViolet,
            Identifier = ReadUInt32LittleEndian(data[0x18..]),
            FixedRewards = fxRewards,
            LotteryRewards = ltRewards,
            HeldItem = (int)ReadUInt32LittleEndian(data[0x2C..]),
            ExtraMoves = new ExtraMoves(ReadUInt16LittleEndian(data[0x30..]),
                ReadUInt16LittleEndian(data[0x32..]),
                ReadUInt16LittleEndian(data[0x34..]),
                ReadUInt16LittleEndian(data[0x36..]),
                ReadUInt16LittleEndian(data[0x38..]),
                ReadUInt16LittleEndian(data[0x3A..])),
            ContentType = GetContentType(stars),
            IVs = default,
            Nature = Nature.Random,
            ScaleType = SizeType9.RANDOM,
            Scale = 0,
            CanBeEncounteredScarlet = randRateMinScarlet != -1,
            CanBeEncounteredViolet = randRateMinViolet != -1,
            Map = map,
        };
    }

    private static List<Reward> DeepCopyList(List<Reward> list) =>
        list.Select(reward => new Reward { ItemID = reward.ItemID, Amount = reward.Amount, Probability = reward.Probability, Aux = reward.Aux }).ToList();

    private static List<Reward> GetRewardList(ulong hash, Dictionary<ulong, List<Reward>> dic) => dic.GetValueOrDefault(hash) ?? [];

    private  static byte GetGenderRatio(PersonalInfo9SV personal) => personal.Gender;

    private static AbilityPermission GetAbility(byte b) => b switch
    {
        0 => AbilityPermission.Any12,
        1 => AbilityPermission.Any12H,
        2 => AbilityPermission.OnlyFirst,
        3 => AbilityPermission.OnlySecond,
        4 => AbilityPermission.OnlyHidden,
        _ => throw new ArgumentOutOfRangeException(nameof(b), b, null),
    };

    private static RaidContent GetContentType(byte stars) => stars < 6 ? RaidContent.Standard : RaidContent.Black;

    public static EncounterRaidTF9? GetEncounterFromSeed(in uint seed, in EncounterTeraTF9[] encounters, in short rateTotal, in GameVersion game, GameProgress progress, RaidContent content)
    {
        var xoro = new Xoroshiro128Plus(seed);
        var randStars = content is RaidContent.Standard ? GetSeedStars(ref xoro, progress) : (byte)6;
        var rateRand = (int)xoro.NextInt((uint)rateTotal);

        foreach (var encounter in encounters)
        {
            if (randStars != encounter.Stars)
                continue;

            if ((uint)(rateRand - (game is GameVersion.VL ? encounter.RandRateMinViolet : encounter.RandRateMinScarlet)) < encounter.RandRate)
                return encounter;
        }
        return null;
    }

    public static EncounterRaidTF9? GetEncounterFromSeed(uint seed, EncounterRaidTF9[] encounters, GameVersion game, GameProgress progress, RaidContent content, TeraRaidMapParent map)
    {
        var xoro = new Xoroshiro128Plus(seed);
        var randStars = content is RaidContent.Standard ? GetSeedStars(ref xoro, progress) : (byte)6;
        var max = game is GameVersion.SL ? EncounterTera9.GetRateTotalSL(randStars, map) : EncounterTera9.GetRateTotalVL(randStars, map);
        var rateRand = (int)xoro.NextInt((uint)max);

        foreach (var encounter in (EncounterTeraTF9[])encounters)
        {
            if (randStars != encounter.Stars)
                continue;

            var min = game is GameVersion.SL ? encounter.RandRateMinScarlet : encounter.RandRateMinViolet;
            if ((uint)(rateRand - min) < encounter.RandRate)
                return encounter;
        }
        return null;
    }

    public static byte GetSeedStars(ref Xoroshiro128Plus xoro, GameProgress progress)
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
}
