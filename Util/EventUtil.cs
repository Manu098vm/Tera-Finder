using System.Data;
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
            var KBCATEventRaidIdentifier = sav.AllBlocks.Where(block => block.Key == 0x37B99B4D).FirstOrDefault();
            if (KBCATEventRaidIdentifier is not null && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0)
            {
                var KBCATFixedRewardItemArray = sav.AllBlocks.Where(block => block.Key == 0x7D6C2B82).FirstOrDefault()!.Data;
                var KBCATLotteryRewardItemArray = sav.AllBlocks.Where(block => block.Key == 0xA52B4811).FirstOrDefault()!.Data;
                //var KBCATRaidPriorityArray = sav.AllBlocks.Where(block => block.Key == 0x095451E4).FirstOrDefault()!.Data;
                var tableDrops = FlatBufferConverter.DeserializeFrom<DeliveryRaidFixedRewardItemArray>(KBCATFixedRewardItemArray);
                var tableBonus = FlatBufferConverter.DeserializeFrom<DeliveryRaidLotteryRewardItemArray>(KBCATLotteryRewardItemArray);
                //var tablePriority = FlatBufferConverter.DeserializeFrom<DeliveryRaidPriorityArray>(priority);
                var drops = TableUtil.GetTable(tableDrops.Table);
                var lottery = TableUtil.GetTable(tableBonus.Table);
                //var priority = TableUtil.GetTable(tablePriority.Table);
                var opt = new JsonSerializerOptions { WriteIndented = true };
                drops = JsonSerializer.Serialize(drops, opt);
                lottery = JsonSerializer.Serialize(lottery, opt);
                //priority = System.Text.Json.JsonSerializer.Serialize(priority, opt);
                return new string[2] { drops, lottery };
            }

            var drops_def = Encoding.UTF8.GetString(Properties.Resources.bcat_default_fixed_reward_item_array);
            var lottery_def = Encoding.UTF8.GetString(Properties.Resources.bcat_default_lottery_reward_item_array);
            return new string[2] { drops_def, lottery_def };
        }

        public static byte[][] GetEventEncounterDataFromSAV(PKHeX.Core.SAV9SV sav)
        {
            byte[][] res = null!;
            var type2list = new List<byte[]>();
            var type3list = new List<byte[]>();

            var KBCATEventRaidIdentifier = sav.AllBlocks.Where(block => block.Key == 0x37B99B4D).FirstOrDefault();
            var KBCATRaidEnemyArray = KBCATEventRaidIdentifier is not null && BitConverter.ToUInt32(KBCATEventRaidIdentifier.Data) > 0 ?
                sav.AllBlocks.Where(block => block.Key == 0x0520A1B0).FirstOrDefault()!.Data : Properties.Resources.bcat_default_raid_enemy_array;

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

            enc.SerializeTeraFinder(bw);

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