using System.Text.Json;
using PKHeX.Core;

namespace TeraFinder
{
    public class Reward
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
        public double Probability { get; set; }
    }

    public enum RewardCategory : int
    {
        ItemNone = 0,
        Poke = 1,
        Gem = 2,
    }

    public static class RewardUtil
    {
        public static void GetTeraRewards(ulong hash)
        {
            var drops = new RaidFixedRewardItemArray{ Array = JsonSerializer.Deserialize<RaidFixedRewardItemArray.Root>(Properties.Resources.raid_fixed_reward_item_array)! };
            var lottery = new RaidLotteryRewardItemArray { Array = JsonSerializer.Deserialize<RaidLotteryRewardItemArray.Root>(Properties.Resources.raid_lottery_reward_item_array)! };
            var fixedTable = GetFixedTable(drops.Array.Table);  
            var lotteryTable = GetLotteryTable(lottery.Array.Table);
            fixedTable.TryGetValue(hash, out var list);            
        }

        public static void GetDistReward(SAV9SV sav)
        { 
            var rewards = EventUtil.GetEventItemDataFromSAV(sav);
            if (rewards is not null)
            {
                var drops = new RaidFixedRewardItemArray { Array = JsonSerializer.Deserialize<RaidFixedRewardItemArray.Root>(rewards[0])! };
                var lottery = new RaidLotteryRewardItemArray { Array = JsonSerializer.Deserialize<RaidLotteryRewardItemArray.Root>(rewards[1])! };
                var fixedTable = GetFixedTable(drops.Array.Table);
                var lotteryTable = GetLotteryTable(lottery.Array.Table);
            }
        }

        private static Dictionary<ulong, List<Reward>> GetFixedTable(List<RaidFixedRewardItemArray.Table> drops)
        {
            var table = new Dictionary<ulong, List<Reward>>();
            foreach (var d in drops)
            {
                var hash = d.TableName;
                var items = new List<Reward>();

                //RewardItem00
                if(d.RewardItem00.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem00.ItemID,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                    });
                else if(d.RewardItem00.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                    });
                }
                else if(d.RewardItem00.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem00.Num,
                        Probability = 100,
                    });
                }

                //RewardItem01
                if (d.RewardItem01.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem01.ItemID,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem01.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem01.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem01.Num,
                        Probability = 100,
                    });
                }

                //RewardItem02
                if (d.RewardItem02.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem02.ItemID,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem02.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem02.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem02.Num,
                        Probability = 100,
                    });
                }

                //RewardItem03
                if (d.RewardItem03.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem03.ItemID,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem03.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem03.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem03.Num,
                        Probability = 100,
                    });
                }

                //RewardItem04
                if (d.RewardItem04.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem04.ItemID,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem04.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem04.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem04.Num,
                        Probability = 100,
                    });
                }

                //RewardItem05
                if (d.RewardItem05.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem05.ItemID,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem05.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem05.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem05.Num,
                        Probability = 100,
                    });
                }

                //RewardItem06
                if (d.RewardItem06.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem06.ItemID,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem06.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem06.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem06.Num,
                        Probability = 100,
                    });
                }

                //RewardItem07
                if (d.RewardItem07.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem07.ItemID,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem07.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem07.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem07.Num,
                        Probability = 100,
                    });
                }

                //RewardItem08
                if (d.RewardItem08.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem08.ItemID,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem08.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem08.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem08.Num,
                        Probability = 100,
                    });
                }

                //RewardItem09
                if (d.RewardItem09.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem09.ItemID,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem09.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem09.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem09.Num,
                        Probability = 100,
                    });
                }

                //RewardItem10
                if (d.RewardItem10.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem10.ItemID,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem10.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem10.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem10.Num,
                        Probability = 100,
                    });
                }

                //RewardItem11
                if (d.RewardItem11.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem11.ItemID,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem11.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem11.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem11.Num,
                        Probability = 100,
                    });
                }

                //RewardItem12
                if (d.RewardItem12.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem12.ItemID,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem12.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem12.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem12.Num,
                        Probability = 100,
                    });
                }

                //RewardItem13
                if (d.RewardItem13.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem13.ItemID,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem13.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem13.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem13.Num,
                        Probability = 100,
                    });
                }

                //RewardItem14
                if (d.RewardItem14.ItemID != (int)RewardCategory.ItemNone)
                    items.Add(new Reward
                    {
                        ItemID = d.RewardItem14.ItemID,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                    });
                else if (d.RewardItem14.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                    });
                }
                else if (d.RewardItem14.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = d.RewardItem14.Num,
                        Probability = 100,
                    });
                }

                table.Add(hash, items);
            }
            return table;
        }

        private static Dictionary<ulong, List<Reward>> GetLotteryTable(List<RaidLotteryRewardItemArray.Table> lottery)
        {
            var table = new Dictionary<ulong, List<Reward>>();

            foreach(var l in lottery)
            {
                var hash = l.TableName;
                var items = new List<Reward>();

                var rate = 0;
                rate += l.RewardItem00.Rate;
                rate += l.RewardItem01.Rate;
                rate += l.RewardItem02.Rate;
                rate += l.RewardItem03.Rate;
                rate += l.RewardItem04.Rate;
                rate += l.RewardItem05.Rate;
                rate += l.RewardItem06.Rate;
                rate += l.RewardItem07.Rate;
                rate += l.RewardItem08.Rate;
                rate += l.RewardItem09.Rate;
                rate += l.RewardItem10.Rate;
                rate += l.RewardItem11.Rate;
                rate += l.RewardItem12.Rate;
                rate += l.RewardItem13.Rate;
                rate += l.RewardItem14.Rate;
                rate += l.RewardItem15.Rate;
                rate += l.RewardItem16.Rate;
                rate += l.RewardItem17.Rate;
                rate += l.RewardItem18.Rate;
                rate += l.RewardItem19.Rate;
                rate += l.RewardItem20.Rate;
                rate += l.RewardItem21.Rate;
                rate += l.RewardItem22.Rate;
                rate += l.RewardItem23.Rate;
                rate += l.RewardItem24.Rate;
                rate += l.RewardItem25.Rate;
                rate += l.RewardItem26.Rate;
                rate += l.RewardItem27.Rate;
                rate += l.RewardItem28.Rate;
                rate += l.RewardItem29.Rate;

                var totalrate = (double)rate;

                //RewardItem00
                if(l.RewardItem00.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem00.ItemID,
                        Amount = l.RewardItem00.Num,
                        Probability = ((double)(l.RewardItem00.Rate) / totalrate * 100)
                    });
                }
                else if(l.RewardItem00.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue -1,
                        Amount = l.RewardItem00.Num,
                        Probability = ((double)(l.RewardItem00.Rate) / totalrate * 100)
                    });
                }
                else if(l.RewardItem00.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem00.Num,
                        Probability = ((double)(l.RewardItem00.Rate) / totalrate * 100),
                    });
                }

                //RewardItem01
                if (l.RewardItem01.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem01.ItemID,
                        Amount = l.RewardItem01.Num,
                        Probability = ((double)(l.RewardItem01.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem01.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem01.Num,
                        Probability = ((double)(l.RewardItem01.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem01.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem01.Num,
                        Probability = ((double)(l.RewardItem01.Rate) / totalrate * 100),
                    });
                }

                //RewardItem02
                if (l.RewardItem02.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem02.ItemID,
                        Amount = l.RewardItem02.Num,
                        Probability = ((double)(l.RewardItem02.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem02.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem02.Num,
                        Probability = ((double)(l.RewardItem02.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem02.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem02.Num,
                        Probability = ((double)(l.RewardItem02.Rate) / totalrate * 100),
                    });
                }

                //RewardItem03
                if (l.RewardItem03.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem03.ItemID,
                        Amount = l.RewardItem03.Num,
                        Probability = ((double)(l.RewardItem03.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem03.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem03.Num,
                        Probability = ((double)(l.RewardItem03.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem03.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem03.Num,
                        Probability = ((double)(l.RewardItem03.Rate) / totalrate * 100),
                    });
                }

                //RewardItem04
                if (l.RewardItem04.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem04.ItemID,
                        Amount = l.RewardItem04.Num,
                        Probability = ((double)(l.RewardItem04.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem04.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem04.Num,
                        Probability = ((double)(l.RewardItem04.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem04.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem04.Num,
                        Probability = ((double)(l.RewardItem04.Rate) / totalrate * 100),
                    });
                }

                //RewardItem05
                if (l.RewardItem05.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem05.ItemID,
                        Amount = l.RewardItem05.Num,
                        Probability = ((double)(l.RewardItem05.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem05.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem05.Num,
                        Probability = ((double)(l.RewardItem05.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem05.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem05.Num,
                        Probability = ((double)(l.RewardItem05.Rate) / totalrate * 100),
                    });
                }

                //RewardItem06
                if (l.RewardItem06.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem06.ItemID,
                        Amount = l.RewardItem06.Num,
                        Probability = ((double)(l.RewardItem06.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem06.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem06.Num,
                        Probability = ((double)(l.RewardItem06.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem06.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem06.Num,
                        Probability = ((double)(l.RewardItem06.Rate) / totalrate * 100),
                    });
                }

                //RewardItem07
                if (l.RewardItem07.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem07.ItemID,
                        Amount = l.RewardItem07.Num,
                        Probability = ((double)(l.RewardItem07.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem07.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem07.Num,
                        Probability = ((double)(l.RewardItem07.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem07.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem07.Num,
                        Probability = ((double)(l.RewardItem07.Rate) / totalrate * 100),
                    });
                }

                //RewardItem08
                if (l.RewardItem08.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem08.ItemID,
                        Amount = l.RewardItem08.Num,
                        Probability = ((double)(l.RewardItem08.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem08.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem08.Num,
                        Probability = ((double)(l.RewardItem08.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem08.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem08.Num,
                        Probability = ((double)(l.RewardItem08.Rate) / totalrate * 100),
                    });
                }

                //RewardItem09
                if (l.RewardItem09.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem09.ItemID,
                        Amount = l.RewardItem09.Num,
                        Probability = ((double)(l.RewardItem09.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem09.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem09.Num,
                        Probability = ((double)(l.RewardItem09.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem09.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem09.Num,
                        Probability = ((double)(l.RewardItem09.Rate) / totalrate * 100),
                    });
                }

                //RewardItem10
                if (l.RewardItem10.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem10.ItemID,
                        Amount = l.RewardItem10.Num,
                        Probability = ((double)(l.RewardItem10.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem10.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem10.Num,
                        Probability = ((double)(l.RewardItem10.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem10.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem10.Num,
                        Probability = ((double)(l.RewardItem10.Rate) / totalrate * 100),
                    });
                }

                //RewardItem11
                if (l.RewardItem11.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem11.ItemID,
                        Amount = l.RewardItem11.Num,
                        Probability = ((double)(l.RewardItem11.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem11.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem11.Num,
                        Probability = ((double)(l.RewardItem11.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem11.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem11.Num,
                        Probability = ((double)(l.RewardItem11.Rate) / totalrate * 100),
                    });
                }

                //RewardItem12
                if (l.RewardItem12.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem12.ItemID,
                        Amount = l.RewardItem12.Num,
                        Probability = ((double)(l.RewardItem12.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem12.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem12.Num,
                        Probability = ((double)(l.RewardItem12.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem12.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem12.Num,
                        Probability = ((double)(l.RewardItem12.Rate) / totalrate * 100),
                    });
                }

                //RewardItem13
                if (l.RewardItem13.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem13.ItemID,
                        Amount = l.RewardItem13.Num,
                        Probability = ((double)(l.RewardItem13.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem13.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem13.Num,
                        Probability = ((double)(l.RewardItem13.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem13.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem13.Num,
                        Probability = ((double)(l.RewardItem13.Rate) / totalrate * 100),
                    });
                }

                //RewardItem14
                if (l.RewardItem14.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem14.ItemID,
                        Amount = l.RewardItem14.Num,
                        Probability = ((double)(l.RewardItem14.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem14.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem14.Num,
                        Probability = ((double)(l.RewardItem14.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem14.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem14.Num,
                        Probability = ((double)(l.RewardItem14.Rate) / totalrate * 100),
                    });
                }

                //RewardItem15
                if (l.RewardItem15.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem15.ItemID,
                        Amount = l.RewardItem15.Num,
                        Probability = ((double)(l.RewardItem15.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem15.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem15.Num,
                        Probability = ((double)(l.RewardItem15.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem15.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem15.Num,
                        Probability = ((double)(l.RewardItem15.Rate) / totalrate * 100),
                    });
                }

                //RewardItem16
                if (l.RewardItem16.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem16.ItemID,
                        Amount = l.RewardItem16.Num,
                        Probability = ((double)(l.RewardItem16.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem16.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem16.Num,
                        Probability = ((double)(l.RewardItem16.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem16.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem16.Num,
                        Probability = ((double)(l.RewardItem16.Rate) / totalrate * 100),
                    });
                }

                //RewardItem17
                if (l.RewardItem17.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem17.ItemID,
                        Amount = l.RewardItem17.Num,
                        Probability = ((double)(l.RewardItem17.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem17.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem17.Num,
                        Probability = ((double)(l.RewardItem17.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem17.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem17.Num,
                        Probability = ((double)(l.RewardItem17.Rate) / totalrate * 100),
                    });
                }

                //RewardItem18
                if (l.RewardItem18.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem18.ItemID,
                        Amount = l.RewardItem18.Num,
                        Probability = ((double)(l.RewardItem18.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem18.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem18.Num,
                        Probability = ((double)(l.RewardItem18.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem18.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem18.Num,
                        Probability = ((double)(l.RewardItem18.Rate) / totalrate * 100),
                    });
                }

                //RewardItem19
                if (l.RewardItem19.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem19.ItemID,
                        Amount = l.RewardItem19.Num,
                        Probability = ((double)(l.RewardItem19.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem19.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem19.Num,
                        Probability = ((double)(l.RewardItem19.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem19.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem19.Num,
                        Probability = ((double)(l.RewardItem19.Rate) / totalrate * 100),
                    });
                }

                //RewardItem20
                if (l.RewardItem20.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem20.ItemID,
                        Amount = l.RewardItem20.Num,
                        Probability = ((double)(l.RewardItem20.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem20.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem20.Num,
                        Probability = ((double)(l.RewardItem20.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem20.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem20.Num,
                        Probability = ((double)(l.RewardItem20.Rate) / totalrate * 100),
                    });
                }

                //RewardItem21
                if (l.RewardItem21.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem21.ItemID,
                        Amount = l.RewardItem21.Num,
                        Probability = ((double)(l.RewardItem21.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem21.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem21.Num,
                        Probability = ((double)(l.RewardItem21.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem21.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem21.Num,
                        Probability = ((double)(l.RewardItem21.Rate) / totalrate * 100),
                    });
                }

                //RewardItem22
                if (l.RewardItem22.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem22.ItemID,
                        Amount = l.RewardItem22.Num,
                        Probability = ((double)(l.RewardItem22.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem22.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem22.Num,
                        Probability = ((double)(l.RewardItem22.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem22.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem22.Num,
                        Probability = ((double)(l.RewardItem22.Rate) / totalrate * 100),
                    });
                }

                //RewardItem23
                if (l.RewardItem23.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem23.ItemID,
                        Amount = l.RewardItem23.Num,
                        Probability = ((double)(l.RewardItem23.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem23.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem23.Num,
                        Probability = ((double)(l.RewardItem23.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem23.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem23.Num,
                        Probability = ((double)(l.RewardItem23.Rate) / totalrate * 100),
                    });
                }

                //RewardItem24
                if (l.RewardItem24.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem24.ItemID,
                        Amount = l.RewardItem24.Num,
                        Probability = ((double)(l.RewardItem24.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem24.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem24.Num,
                        Probability = ((double)(l.RewardItem24.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem24.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem24.Num,
                        Probability = ((double)(l.RewardItem24.Rate) / totalrate * 100),
                    });
                }

                //RewardItem25
                if (l.RewardItem25.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem25.ItemID,
                        Amount = l.RewardItem25.Num,
                        Probability = ((double)(l.RewardItem25.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem25.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem25.Num,
                        Probability = ((double)(l.RewardItem25.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem25.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem25.Num,
                        Probability = ((double)(l.RewardItem25.Rate) / totalrate * 100),
                    });
                }

                //RewardItem26
                if (l.RewardItem26.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem26.ItemID,
                        Amount = l.RewardItem26.Num,
                        Probability = ((double)(l.RewardItem26.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem26.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem26.Num,
                        Probability = ((double)(l.RewardItem26.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem26.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem26.Num,
                        Probability = ((double)(l.RewardItem26.Rate) / totalrate * 100),
                    });
                }

                //RewardItem27
                if (l.RewardItem27.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem27.ItemID,
                        Amount = l.RewardItem27.Num,
                        Probability = ((double)(l.RewardItem27.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem27.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem27.Num,
                        Probability = ((double)(l.RewardItem27.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem27.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem27.Num,
                        Probability = ((double)(l.RewardItem27.Rate) / totalrate * 100),
                    });
                }

                //RewardItem28
                if (l.RewardItem28.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem28.ItemID,
                        Amount = l.RewardItem28.Num,
                        Probability = ((double)(l.RewardItem28.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem28.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem28.Num,
                        Probability = ((double)(l.RewardItem28.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem28.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem28.Num,
                        Probability = ((double)(l.RewardItem28.Rate) / totalrate * 100),
                    });
                }

                //RewardItem29
                if (l.RewardItem29.ItemID != (int)RewardCategory.ItemNone)
                {
                    items.Add(new Reward
                    {
                        ItemID = l.RewardItem29.ItemID,
                        Amount = l.RewardItem29.Num,
                        Probability = ((double)(l.RewardItem29.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem29.Category == (int)RewardCategory.Gem)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue - 1,
                        Amount = l.RewardItem29.Num,
                        Probability = ((double)(l.RewardItem29.Rate) / totalrate * 100)
                    });
                }
                else if (l.RewardItem29.Category == (int)RewardCategory.Poke)
                {
                    items.Add(new Reward
                    {
                        ItemID = ushort.MaxValue,
                        Amount = l.RewardItem29.Num,
                        Probability = ((double)(l.RewardItem29.Rate) / totalrate * 100),
                    });
                }

            }

            return table;
        }

    }
}
