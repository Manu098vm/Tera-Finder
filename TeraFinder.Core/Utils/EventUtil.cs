using System.Data;
using System.Reflection;
using System.Text;
using System.Text.Json;
using PKHeX.Core;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers.SV;

namespace TeraFinder.Core;

public static class EventUtil
{
    public static EventProgress GetEventStageFromProgress(GameProgress progress) => progress switch
    {
        GameProgress.Unlocked6Stars or GameProgress.Unlocked5Stars => EventProgress.Stage3,
        GameProgress.Unlocked4Stars => EventProgress.Stage2,
        GameProgress.Unlocked3Stars => EventProgress.Stage1,
        _ => EventProgress.Stage0,
    };

    public static (EncounterEventTF9[] dist, EncounterEventTF9[] mighty)
        GetCurrentEventEncounters(SAV9SV sav, (Dictionary<ulong, List<Reward>> fixedRewards, Dictionary<ulong, List<Reward>> lotteryRewards) rewards)
    {
        try
        {
            (var distData, var mightyData) = GetEventEncounterData(sav);
            return (EncounterEventTF9.GetArray(distData, rewards.fixedRewards, rewards.lotteryRewards), EncounterEventTF9.GetArray(mightyData, rewards.fixedRewards, rewards.lotteryRewards));
        }
        catch { }
        return ([], []);
    }

    public static (string distRewards, string mightyRewards) GetCurrentEventRewards(SAV9SV sav)
    {
        var KBCATEventRaidIdentifier = sav.Accessor.FindOrDefault(BlockDefinitions.KBCATEventRaidIdentifier.Key);
        if (KBCATEventRaidIdentifier.Type is not SCTypeCode.None && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0)
        {
            var KBCATFixedRewardItemArray = sav.Accessor.FindOrDefault(BlockDefinitions.KBCATFixedRewardItemArray.Key).Data;
            var KBCATLotteryRewardItemArray = sav.Accessor.FindOrDefault(BlockDefinitions.KBCATLotteryRewardItemArray.Key).Data;
            var tableDrops = pkNX.Structures.FlatBuffers.FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(KBCATFixedRewardItemArray.ToArray());
            var tableBonus = pkNX.Structures.FlatBuffers.FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(KBCATLotteryRewardItemArray.ToArray());
            var opt = new JsonSerializerOptions { WriteIndented = true };
            var drops = JsonSerializer.Serialize(tableDrops, opt);
            var lottery = JsonSerializer.Serialize(tableBonus, opt);
            return (drops, lottery);
        }

        var drops_def = Encoding.UTF8.GetString(Properties.Resources.raid_fixed_reward_item_array);
        var lottery_def = Encoding.UTF8.GetString(Properties.Resources.raid_lottery_reward_item_array);
        return (drops_def, lottery_def);
    }

    private static (byte[] distData, byte[] mightyData) GetEventEncounterData(SAV9SV sav)
    {
        var type2list = new List<byte[]>();
        var type3list = new List<byte[]>();

        var KBCATRaidEnemyArray = sav.Accessor.FindOrDefault(BlockDefinitions.KBCATRaidEnemyArray.Key).Data;
        var tableEncounters = pkNX.Structures.FlatBuffers.FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(KBCATRaidEnemyArray.ToArray());

        var byGroupID = tableEncounters.Table
            .Where(z => z.Info.Rate != 0)
            .GroupBy(z => z.Info.DeliveryGroupID);

        var seven = DistroGroupSet.None;
        var other = DistroGroupSet.None;
        foreach (var group in byGroupID)
        {
            var items = group.ToArray();
            var groupSet = Evaluate(items);

            if (items.Any(z => z.Info.Difficulty > 7))
                throw new Exception($"Undocumented difficulty {items.First(z => z.Info.Difficulty > 7).Info.Difficulty}");

            if (items.All(z => z.Info.Difficulty == 7))
            {
                if (items.Any(z => z.Info.CaptureRate != 2))
                    throw new Exception($"Undocumented 7 star capture rate {items.First(z => z.Info.CaptureRate != 2).Info.CaptureRate}");

                if (!TryAdd(ref seven, groupSet))
                    Console.WriteLine("Already saw a 7-star group. How do we differentiate this slot determination from prior?");

                AddToList(items, type3list, RaidSerializationFormat.Might);
                continue;
            }

            if (items.Any(z => z.Info.Difficulty == 7))
                throw new Exception($"Mixed difficulty {items.First(z => z.Info.Difficulty >= 7).Info.Difficulty}");

            if (!TryAdd(ref other, groupSet))
                Console.WriteLine("Already saw a not-7-star group. How do we differentiate this slot determination from prior?");

            AddToList(items, type2list, RaidSerializationFormat.Distribution);
        }

        return ([..type2list.SelectMany(z => z)], [..type3list.SelectMany(z => z)]);
    }

    public static DeliveryRaidPriority? GetEventDeliveryPriority(SAV9SV sav)
    {
        var KBCATRaidPriorityArray = sav.Accessor.FindOrDefault(BlockDefinitions.KBCATRaidPriorityArray.Key);
        if (KBCATRaidPriorityArray.Type is not SCTypeCode.None && KBCATRaidPriorityArray.Data.Length > 0)
        {
            try { return pkNX.Structures.FlatBuffers.FlatBufferConverter.DeserializeFrom<DeliveryRaidPriorityArray>(KBCATRaidPriorityArray.Data.ToArray()).Table.First(); }
            catch { }
        }
        return null;
    }

    private static bool TryAdd(ref DistroGroupSet exist, DistroGroupSet add)
    {
        if ((exist & add) != 0)
            return false;
        exist |= add;
        return true;
    }

    [Flags]
    private enum DistroGroupSet
    {
        None = 0,
        SL = 1,
        VL = 2,
        Both = SL | VL,
    }

    private static DistroGroupSet Evaluate(DeliveryRaidEnemyTable[] items)
    {
        var versions = items.Select(z => z.Info.RomVer).Distinct().ToArray();
        if (versions.Length == 2 && versions.Contains(RaidRomType.TYPE_A) && versions.Contains(RaidRomType.TYPE_B))
            return DistroGroupSet.Both;
        if (versions.Length == 1)
        {
            return versions[0] switch
            {
                RaidRomType.BOTH => DistroGroupSet.Both,
                RaidRomType.TYPE_A => DistroGroupSet.SL,
                RaidRomType.TYPE_B => DistroGroupSet.VL,
                _ => throw new Exception("Unknown type."),
            };
        }
        throw new Exception("Unknown version");
    }

    private static void AddToList(IReadOnlyCollection<DeliveryRaidEnemyTable> table, List<byte[]> list, RaidSerializationFormat format)
    {
        // Get the total weight for each stage of star count
        Span<ushort> weightTotalS = stackalloc ushort[StageStars.Length];
        Span<ushort> weightTotalV = stackalloc ushort[StageStars.Length];
        foreach (var enc in table)
        {
            var info = enc.Info;
            if (info.Rate == 0)
                continue;
            var difficulty = info.Difficulty;
            for (int stage = 0; stage < StageStars.Length; stage++)
            {
                if (!StageStars[stage].Contains(difficulty))
                    continue;
                if (info.RomVer != RaidRomType.TYPE_B)
                    weightTotalS[stage] += (ushort)info.Rate;
                if (info.RomVer != RaidRomType.TYPE_A)
                    weightTotalV[stage] += (ushort)info.Rate;
            }
        }

        Span<ushort> weightMinS = stackalloc ushort[StageStars.Length];
        Span<ushort> weightMinV = stackalloc ushort[StageStars.Length];
        foreach (var enc in table)
        {
            var info = enc.Info;
            if (info.Rate == 0)
                continue;
            var difficulty = info.Difficulty;
            TryAddToPickle(info, list, format, weightTotalS, weightTotalV, weightMinS, weightMinV);
            for (int stage = 0; stage < StageStars.Length; stage++)
            {
                if (!StageStars[stage].Contains(difficulty))
                    continue;
                if (info.RomVer != RaidRomType.TYPE_B)
                    weightMinS[stage] += (ushort)info.Rate;
                if (info.RomVer != RaidRomType.TYPE_A)
                    weightMinV[stage] += (ushort)info.Rate;
            }
        }
    }

    private static void TryAddToPickle(RaidEnemyInfo enc, List<byte[]> list, RaidSerializationFormat format,
        ReadOnlySpan<ushort> totalS, ReadOnlySpan<ushort> totalV, ReadOnlySpan<ushort> minS, ReadOnlySpan<ushort> minV)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        enc.SerializePKHeX(bw, (byte)enc.Difficulty, enc.Rate, format);
        for (int stage = 0; stage < StageStars.Length; stage++)
        {
            bool noTotal = !StageStars[stage].Contains(enc.Difficulty);
            ushort mS = minS[stage];
            ushort mV = minV[stage];
            bw.Write(noTotal ? (ushort)0 : mS);
            bw.Write(noTotal ? (ushort)0 : mV);
            bw.Write(noTotal || enc.RomVer is RaidRomType.TYPE_B ? (ushort)0 : totalS[stage]);
            bw.Write(noTotal || enc.RomVer is RaidRomType.TYPE_A ? (ushort)0 : totalV[stage]);
        }

        if (format == RaidSerializationFormat.Might)
            enc.SerializeMight(bw);

        if (format == RaidSerializationFormat.Distribution)
            enc.SerializeDistribution(bw);

        enc.SerializeTeraFinder(bw);

        var bin = ms.ToArray();
        if (!list.Any(z => z.SequenceEqual(bin)))
            list.Add(bin);
    }

    private static readonly int[][] StageStars =
    [
        [1, 2],
        [1, 2, 3],
        [1, 2, 3, 4],
        [3, 4, 5, 6, 7],
    ];

    public static byte GetDeliveryGroupID(IEventRaid9[] encounters, SAV9SV sav, EventProgress progress, RaidSpawnList9 raids, int currRaid)
    {
        var possibleGroups = new HashSet<int>();

        foreach (var enc in encounters)
            if ((sav.Version is PKHeX.Core.GameVersion.SL && enc.GetRandRateTotalScarlet(progress) > 0) ||
                (sav.Version is PKHeX.Core.GameVersion.VL && enc.GetRandRateTotalViolet(progress) > 0))
                    possibleGroups.Add(enc.Index);

        var eventCount = GetEventCount(raids, ++currRaid);

        var priority = GetEventDeliveryPriority(sav);
        var groupid = priority is not null ? GetDeliveryGroupID(eventCount, priority.GroupID.Groups, possibleGroups) : (byte)0;

        return groupid;
    }

    private static int GetEventCount(RaidSpawnList9 raids, int selected)
    {
        var count = 0;
        for (var i = 0; i < selected; i++)
            if (raids.GetRaid(i).Content >= TeraRaidContentType.Distribution)
                count++;
        return count;
    }

    //From https://github.com/LegoFigure11/RaidCrawler/blob/7e764a9a5c0aa74270b3679083c813471abc55d6/Structures/TeraDistribution.cs#L145
    //GPL v3 License
    //Thanks LegoFigure11 & architade!
    private static byte GetDeliveryGroupID(int eventct, GroupSet ids, HashSet<int> possible_groups)
    {
        if (eventct > -1 && possible_groups.Count > 0)
        {
            var cts = new int[10];
            for (var i = 0; i < ids.Groups_Length; i++)
                cts[i] = GroupSet.Groups_Item(ref ids, i);

            for (int i = 0; i < cts.Length; i++)
            {
                var ct = cts[i];
                if (!possible_groups.Contains(i + 1))
                    continue;
                if (eventct <= ct)
                    return (byte)(i + 1);
                eventct -= ct;
            }
        }
        return 0;
    }
}