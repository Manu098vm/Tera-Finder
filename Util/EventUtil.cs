﻿using System.Data;
using System.Text;
using System.Text.Json;
using pkNX.Structures.FlatBuffers;

//Most of functions here are taken from pkNX
//https://github.com/kwsch/pkNX/blob/master/pkNX.WinForms/Dumping/TeraRaidRipper.cs
//GPL v3 Licence
namespace TeraFinder
{
    public static class EventUtil
    {
        public static string[] GetEventItemDataFromSAV(PKHeX.Core.SAV9SV sav)
        {
            //Read from save file block flatbuffer
            var KBCATEventRaidIdentifier = sav.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);
            if (KBCATEventRaidIdentifier.Type is not PKHeX.Core.SCTypeCode.None && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0)
            {
                var KBCATFixedRewardItemArray = sav.Accessor.FindOrDefault(Blocks.KBCATFixedRewardItemArray.Key).Data;
                var KBCATLotteryRewardItemArray = sav.Accessor.FindOrDefault(Blocks.KBCATLotteryRewardItemArray.Key).Data;
                var tableDrops = FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(KBCATFixedRewardItemArray);
                var tableBonus = FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(KBCATLotteryRewardItemArray);
                var opt = new JsonSerializerOptions { WriteIndented = true };
                var drops = JsonSerializer.Serialize(tableDrops, opt);
                var lottery = JsonSerializer.Serialize(tableBonus, opt);
                return new string[2] { drops, lottery };
            }

            var drops_def = Encoding.UTF8.GetString(Properties.Resources.raid_fixed_reward_item_array);
            var lottery_def = Encoding.UTF8.GetString(Properties.Resources.raid_lottery_reward_item_array);
            return new string[2] { drops_def, lottery_def };
        }

        public static byte[][] GetEventEncounterDataFromSAV(PKHeX.Core.SAV9SV sav)
        {
            byte[][] res = null!;
            var type2list = new List<byte[]>();
            var type3list = new List<byte[]>();

            var KBCATRaidEnemyArray = sav.Accessor.FindOrDefault(Blocks.KBCATRaidEnemyArray.Key).Data;

            var tableEncounters = FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(KBCATRaidEnemyArray);

            var byGroupID = tableEncounters.Table
                .Where(z => z.RaidEnemyInfo.Rate != 0)
                .GroupBy(z => z.RaidEnemyInfo.DeliveryGroupID);

            var seven = DistroGroupSet.None;
            var other = DistroGroupSet.None;
            foreach (var group in byGroupID)
            {
                var items = group.ToArray();
                var groupSet = Evaluate(items);

                if (items.Any(z => z.RaidEnemyInfo.Difficulty > 7))
                    throw new Exception($"Undocumented difficulty {items.First(z => z.RaidEnemyInfo.Difficulty > 7).RaidEnemyInfo.Difficulty}");

                if (items.All(z => z.RaidEnemyInfo.Difficulty == 7))
                {
                    if (items.Any(z => z.RaidEnemyInfo.CaptureRate != 2))
                        throw new Exception($"Undocumented 7 star capture rate {items.First(z => z.RaidEnemyInfo.CaptureRate != 2).RaidEnemyInfo.CaptureRate}");

                    if (!TryAdd(ref seven, groupSet))
                        Console.WriteLine("Already saw a 7-star group. How do we differentiate this slot determination from prior?");

                    AddToList(items, type3list, RaidSerializationFormat.Type3);
                    continue;
                }

                if (items.Any(z => z.RaidEnemyInfo.Difficulty == 7))
                    throw new Exception($"Mixed difficulty {items.First(z => z.RaidEnemyInfo.Difficulty >= 7).RaidEnemyInfo.Difficulty}");

                if (!TryAdd(ref other, groupSet))
                    Console.WriteLine("Already saw a not-7-star group. How do we differentiate this slot determination from prior?");

                AddToList(items, type2list, RaidSerializationFormat.Type2);
            }

            res = new byte[][] { type2list.SelectMany(z => z).ToArray(), type3list.SelectMany(z => z).ToArray() };
            return res;
        }

        public static DeliveryRaidPriority? GetDeliveryPriority(PKHeX.Core.SAV9SV sav)
        {
            var KBCATRaidPriorityArray = sav.Accessor.FindOrDefault(Blocks.KBCATRaidPriorityArray.Key);
            if (KBCATRaidPriorityArray.Type is not PKHeX.Core.SCTypeCode.None && KBCATRaidPriorityArray.Data.Length > 0)
            {
                try
                {
                    return FlatBufferConverter.DeserializeFrom<DeliveryRaidPriorityArray>(KBCATRaidPriorityArray.Data).Table.First();
                }
                catch (Exception)
                {
                    return null;
                }
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
            var versions = items.Select(z => z.RaidEnemyInfo.RomVer).Distinct().ToArray();
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
                var info = enc.RaidEnemyInfo;
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
                var info = enc.RaidEnemyInfo;
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

        private static void TryAddToPickle(RaidEnemyInfo enc, ICollection<byte[]> list, RaidSerializationFormat format,
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

            if (format == RaidSerializationFormat.Type3)
                enc.SerializeType3(bw);

            enc.SerializeTeraFinder(bw);

            if (format == RaidSerializationFormat.Type2)
                enc.SerializeType2(bw);

            var bin = ms.ToArray();
            if (!list.Any(z => z.SequenceEqual(bin)))
                list.Add(bin);
        }

        private static readonly int[][] StageStars =
        {
            new [] { 1, 2 },
            new [] { 1, 2, 3 },
            new [] { 1, 2, 3, 4 },
            new [] { 3, 4, 5, 6, 7 },
        };
    }
}