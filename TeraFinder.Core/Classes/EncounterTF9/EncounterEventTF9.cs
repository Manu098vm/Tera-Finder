using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core;

public sealed record EncounterEventTF9 : EncounterRaidTF9, IEventRaid9
{
    public const int SerializedSize = 0x62;

    public ushort RandRate0MinScarlet { get; private init; }
    public ushort RandRate0MinViolet { get; private init; }
    public ushort RandRate0TotalScarlet { get; private init; }
    public ushort RandRate0TotalViolet { get; private init; }

    public ushort RandRate1MinScarlet { get; private init; }
    public ushort RandRate1MinViolet { get; private init; }
    public ushort RandRate1TotalScarlet { get; private init; }
    public ushort RandRate1TotalViolet { get; private init; }

    public ushort RandRate2MinScarlet { get; private init; }
    public ushort RandRate2MinViolet { get; private init; }
    public ushort RandRate2TotalScarlet { get; private init; }
    public ushort RandRate2TotalViolet { get; private init; }

    public ushort RandRate3MinScarlet { get; private init; }
    public ushort RandRate3MinViolet { get; private init; }
    public ushort RandRate3TotalScarlet { get; private init; }
    public ushort RandRate3TotalViolet { get; private init; }

    private const int StageCount = 4;
    private const int StageNone = -1;

    public override bool CanBeEncountered(uint seed) => (int)GetProgressMaximum(seed) != StageNone;

    public static EncounterEventTF9[] GetArray(ReadOnlySpan<byte> data, Dictionary<ulong, List<Reward>> fixedRewards, Dictionary<ulong, List<Reward>> lotteryRewards)
    {
        var count = data.Length / SerializedSize;
        var encounters = new EncounterEventTF9[count];
        for (var i = 0; i < count; i++)
            encounters[i] = ReadEncounter(data.Slice(i * SerializedSize, SerializedSize), fixedRewards, lotteryRewards);
        return encounters;
    }

    private static EncounterEventTF9 ReadEncounter(ReadOnlySpan<byte> data, Dictionary<ulong, List<Reward>> fixedRewards, Dictionary<ulong, List<Reward>> lotteryRewards)
    {
        var species = ReadUInt16LittleEndian(data);
        var form = data[0x02]; 
        var gender = (byte)(data[0x03] - 1);
        var stars = data[0x12];
        var personal = PersonalTable.SV.GetFormEntry(species, form);

        var fxRewards = DeepCopyList(GetRewardList(ReadUInt64LittleEndian(data[0x42..]), fixedRewards));
        var ltRewards = DeepCopyList(GetRewardList(ReadUInt64LittleEndian(data[0x4A..]), lotteryRewards));
        fxRewards.ReplaceMaterialReward((Species)species);
        ltRewards.ReplaceMaterialReward((Species)species);

        const int WeightStart = 0x14;
        var enc = new EncounterEventTF9()
        {
            Personal = personal,
            Species = species,
            Form = form,
            Gender = gender,
            GenderRatio = GetGenderRatio(gender, stars, personal),
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

            RandRate0MinScarlet = ReadUInt16LittleEndian(data[WeightStart..]),
            RandRate0MinViolet = ReadUInt16LittleEndian(data[(WeightStart + sizeof(ushort))..]),
            RandRate0TotalScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 2))..]),
            RandRate0TotalViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 3))..]),

            RandRate1MinScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 4))..]),
            RandRate1MinViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 5))..]),
            RandRate1TotalScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 6))..]),
            RandRate1TotalViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 7))..]),

            RandRate2MinScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 8))..]),
            RandRate2MinViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 9))..]),
            RandRate2TotalScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 10))..]),
            RandRate2TotalViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 11))..]),

            RandRate3MinScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 12))..]),
            RandRate3MinViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 13))..]),
            RandRate3TotalScarlet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 14))..]),
            RandRate3TotalViolet = ReadUInt16LittleEndian(data[(WeightStart + (sizeof(ushort) * 15))..]),

            Nature = (Nature)data[0x34],
            IVs = new IndividualValueSet((sbyte)data[0x35], (sbyte)data[0x36], (sbyte)data[0x37], (sbyte)data[0x38], (sbyte)data[0x39], (sbyte)data[0x3A], (IndividualValueSetType)data[0x3B]),
            ScaleType = (SizeType9)data[0x3C],
            Scale = data[0x3D],

            Map = TeraRaidMapParent.Paldea,
            ContentType = stars == 7 ? RaidContent.Event_Mighty : RaidContent.Event,

            Identifier = ReadUInt32LittleEndian(data[0x3E..]),
            FixedRewards = fxRewards,
            LotteryRewards = ltRewards,
            HeldItem = (int)ReadUInt32LittleEndian(data[0x52..]),
            ExtraMoves = new ExtraMoves(
                ReadUInt16LittleEndian(data[0x56..]),
                ReadUInt16LittleEndian(data[0x58..]),
                ReadUInt16LittleEndian(data[0x5A..]),
                ReadUInt16LittleEndian(data[0x5C..]),
                ReadUInt16LittleEndian(data[0x5E..]),
                ReadUInt16LittleEndian(data[0x60..])),
        };

        enc.CanBeEncounteredScarlet = enc.CanBeEncountered(GameVersion.SL);
        enc.CanBeEncounteredViolet = enc.CanBeEncountered(GameVersion.VL);
        return enc;
    }

    private static List<Reward> DeepCopyList(List<Reward> list) =>
        list.Select(reward => new Reward { ItemID = reward.ItemID, Amount = reward.Amount, Probability = reward.Probability, Aux = reward.Aux }).ToList();

    private static List<Reward> GetRewardList(ulong hash, Dictionary<ulong, List<Reward>> dic) => dic.GetValueOrDefault(hash) ?? [];

    private static byte GetGenderRatio(byte gender, byte stars, PersonalInfo9SV personal) => stars switch
    {
        7 => gender switch
        {
            0 => PersonalInfo.RatioMagicMale,
            1 => PersonalInfo.RatioMagicFemale,
            2 => PersonalInfo.RatioMagicGenderless,
            _ => personal.Gender,
        },
        _ => personal.Gender,
    };

    private static AbilityPermission GetAbility(byte b) => b switch
    {
        0 => AbilityPermission.Any12,
        1 => AbilityPermission.Any12H,
        2 => AbilityPermission.OnlyFirst,
        3 => AbilityPermission.OnlySecond,
        4 => AbilityPermission.OnlyHidden,
        _ => throw new ArgumentOutOfRangeException(nameof(b), b, null),
    };

    public static EncounterRaidTF9? GetEncounterFromSeed(uint seed, EncounterEventTF9[] encounters, GameVersion game, EventProgress progress, int groupid)
    {
        foreach (var enc in encounters)
        {
            if (enc.Index != groupid)
                continue;

            var max = game switch { GameVersion.VL => enc.GetRandRateTotalViolet(progress), _ => enc.GetRandRateTotalScarlet(progress) };
            if (max > 0)
            {
                var xoro = new Xoroshiro128Plus(seed);
                xoro.NextInt(100);
                var rateRand = xoro.NextInt(max);
                var min = game switch { GameVersion.VL => enc.GetRandRateMinViolet(progress), _ => enc.GetRandRateMinScarlet(progress) };
                if ((uint)(rateRand - min) < enc.RandRate)
                    return enc;
            }
        }

        return null;
    }

    public bool CanBeEncountered(GameVersion version)
    {
        for (var progress = EventProgress.Stage0; progress <= EventProgress.Stage3; progress++)
            if (CanBeEncounteredFromStage(progress, version))
                return true;

        return false;
    }

    public bool CanBeEncounteredFromStage(EventProgress progress, GameVersion version)
    {
        var maxRate = version switch { GameVersion.SL => GetRandRateTotalScarlet(progress), _ => GetRandRateTotalViolet(progress) };

        if (maxRate > 0)
            return true;

        return false;
    }

    public ushort GetRandRateTotalScarlet(EventProgress stage) => stage switch
    {
        EventProgress.Stage0 => RandRate0TotalScarlet,
        EventProgress.Stage1 => RandRate1TotalScarlet,
        EventProgress.Stage2 => RandRate2TotalScarlet,
        EventProgress.Stage3 => RandRate3TotalScarlet,
        _ => throw new ArgumentOutOfRangeException(nameof(stage)),
    };

    public ushort GetRandRateTotalViolet(EventProgress stage) => stage switch
    {
        EventProgress.Stage0 => RandRate0TotalViolet,
        EventProgress.Stage1 => RandRate1TotalViolet,
        EventProgress.Stage2 => RandRate2TotalViolet,
        EventProgress.Stage3 => RandRate3TotalViolet,
        _ => throw new ArgumentOutOfRangeException(nameof(stage)),
    };

    public ushort GetRandRateMinScarlet(EventProgress stage) => stage switch
    {
        EventProgress.Stage0 => RandRate0MinScarlet,
        EventProgress.Stage1 => RandRate1MinScarlet,
        EventProgress.Stage2 => RandRate2MinScarlet,
        EventProgress.Stage3 => RandRate3MinScarlet,
        _ => throw new ArgumentOutOfRangeException(nameof(stage)),
    };

    public ushort GetRandRateMinViolet(EventProgress stage) => stage switch
    {
        EventProgress.Stage0 => RandRate0MinViolet,
        EventProgress.Stage1 => RandRate1MinViolet,
        EventProgress.Stage2 => RandRate2MinViolet,
        EventProgress.Stage3 => RandRate3MinViolet,
        _ => throw new ArgumentOutOfRangeException(nameof(stage)),
    };

    public EventProgress GetProgressMaximum(uint seed)
    {
        for (var i = (EventProgress)(StageCount - 1); i >= 0; i--)
            if (GetIsPossibleSlot(seed, i))
                return i;

        return (EventProgress)StageNone;
    }

    private bool GetIsPossibleSlot(uint seed, EventProgress stage) => GetIsPossibleSlotScarlet(seed, stage) || GetIsPossibleSlotViolet(seed, stage);

    private bool GetIsPossibleSlotScarlet(uint seed, EventProgress stage)
    {
        var totalScarlet = GetRandRateTotalScarlet(stage);
        if (totalScarlet != 0)
        {
            var rand = new Xoroshiro128Plus(seed);
            _ = rand.NextInt(100);
            var val = rand.NextInt(totalScarlet);
            var min = GetRandRateMinScarlet(stage);
            if ((uint)((int)val - min) < RandRate)
                return true;
        }

        return false;
    }

    private bool GetIsPossibleSlotViolet(uint seed, EventProgress stage)
    {
        var totalViolet = GetRandRateTotalViolet(stage);
        if (totalViolet != 0)
        {
            var rand = new Xoroshiro128Plus(seed);
            _ = rand.NextInt(100);
            var val = rand.NextInt(totalViolet);
            var min = GetRandRateMinViolet(stage);
            if ((uint)((int)val - min) < RandRate)
                return true;
        }

        return false;
    }

    public static HashSet<int> GetPossibleEventStars(EncounterEventTF9[] encounters, EventProgress progress, GameVersion version)
    {
        var set = new HashSet<int>();
        foreach (var enc in encounters)
            if (version switch { GameVersion.VL => enc.GetRandRateTotalViolet(progress), _ => enc.GetRandRateTotalScarlet(progress) } > 0)
                set.Add(enc.Stars);

        return set;
    }
}
