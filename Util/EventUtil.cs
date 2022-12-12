using System.Data;
using pkNX.Structures.FlatBuffers;

namespace TeraRaidEditor
{
    public static class EventUtil
    {
        public static byte[][] GetEventData(PKHeX.Core.SAV9SV sav, bool allEncounters = false)
        {
            byte[][] res = null!;
            //Read from save file block flatbuffer
            var KBCATEventRaidIdentifier = sav.AllBlocks.Where(block => block.Key == 0x37B99B4D).FirstOrDefault();
            if (KBCATEventRaidIdentifier is not null && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0 && !allEncounters)
            {
                var type2list = new List<byte[]>();
                var type3list = new List<byte[]>();
                var KBCATRaidEnemyArray = sav.AllBlocks.Where(block => block.Key == 0x0520A1B0).FirstOrDefault()!.Data;
                var tableEncounters = FlatBufferConverter.DeserializeFrom<DeliveryRaidEnemyTableArray>(KBCATRaidEnemyArray);

                var byGroupID = tableEncounters.Table
                    .Where(z => z.RaidEnemyInfo.Rate != 0)
                    .GroupBy(z => z.RaidEnemyInfo.DeliveryGroupID);

                bool isNot7Star = false;
                foreach (var group in byGroupID)
                {
                    var items = group.ToArray();
                    if (items.Any(z => z.RaidEnemyInfo.Difficulty > 7))
                        throw new Exception($"Undocumented difficulty {items.First(z => z.RaidEnemyInfo.Difficulty > 7).RaidEnemyInfo.Difficulty}");

                    if (items.All(z => z.RaidEnemyInfo.Difficulty == 7))
                    {
                        if (items.Any(z => z.RaidEnemyInfo.CaptureRate != 2))
                            throw new Exception($"Undocumented 7 star capture rate {items.First(z => z.RaidEnemyInfo.CaptureRate != 2).RaidEnemyInfo.CaptureRate}");
                        AddToList(items, type3list, RaidSerializationFormat.Type3);
                        continue;
                    }

                    if (items.Any(z => z.RaidEnemyInfo.Difficulty == 7))
                        throw new Exception($"Mixed difficulty {items.First(z => z.RaidEnemyInfo.Difficulty > 7).RaidEnemyInfo.Difficulty}");
                    if (isNot7Star)
                        throw new Exception("Already saw a not-7-star group. How do we differentiate this slot determination from prior?");
                    isNot7Star = true;
                    AddToList(items, type2list, RaidSerializationFormat.Type2);
                }
                res = new byte[][] { type2list.SelectMany(z => z).ToArray(), type3list.SelectMany(z => z).ToArray() };
                return res;
            }

            //Read from pkhex resources
            var type2 = PKHeX.Core.Util.GetBinaryResource("encounter_dist_paldea.pkl");
            var type3 = PKHeX.Core.Util.GetBinaryResource("encounter_might_paldea.pkl");
            res = new byte[][] { type2, type3};
            return res;
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
                bw.Write(noTotal ? (ushort)0 : totalS[stage]);
                bw.Write(noTotal ? (ushort)0 : totalV[stage]);
            }
            if (format == RaidSerializationFormat.Type3)
                enc.SerializeType3(bw);

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