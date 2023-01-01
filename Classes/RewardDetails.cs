using PKHeX.Core;

namespace TeraFinder
{
    public class Reward
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
        public int Probability { get; set; }
        public int Aux { get; set; }

        public string GetItemName(string[]? itemnames = null, LanguageID lang = LanguageID.English, bool quantity = false)
        {
            if (ItemID == ushort.MaxValue)
                return RewardUtil.Material[(int)lang];
            else if(ItemID == ushort.MaxValue-1)
                return RewardUtil.TeraShard[(int)lang];

            itemnames ??= GameInfo.Strings.itemlist;

            return $"{itemnames[ItemID]}{(Aux == 0 ? "" : Aux == 1 ? " (H)" : Aux == 2 ? " (C)" : Aux == 3 ? " (Once)" : "")}{(quantity ? $" x{Amount}" : "")}";
        }

        public bool CompareItem(Reward item, bool greaterthan = false)
        {
            if (item.ItemID != ItemID)
                return false;
            if (!greaterthan && item.Amount != Amount) 
                return false;
            if(greaterthan && Amount < item.Amount)
                return false;

            return true;
        }

        public bool NeedAccurate()
        {
            return ItemID switch
            {
                >= 1862 and <= 1879 => true,
                >= 1956 and <= 2159 => true,
                _ => false,
            };
        }
    }

    public class RewardDetails
    {
        public uint Seed { get; set; }
        public List<Reward>? Rewards {get; set;}
        public uint Calcs { get; set; }

        public string[] GetStrings(string[] itemnames, LanguageID lang = LanguageID.English)
        {
            var list = new string[20];
            if(Rewards is not null)
                for(var i = 0; i < Rewards.Count; i++)
                    list[i] = Rewards[i].GetItemName(itemnames, lang, true);
            return list;
        }
    }

    public class RewardGridEntry
    {
        public string? Seed { get; private set; }
        public string? Item1 { get; private set; }
        public string? Item2 { get; private set; }
        public string? Item3 { get; private set; }
        public string? Item4 { get; private set; }
        public string? Item5 { get; private set; }
        public string? Item6 { get; private set; }
        public string? Item7 { get; private set; }
        public string? Item8 { get; private set; }
        public string? Item9 { get; private set; }
        public string? Item10 { get; private set; }
        public string? Item11 { get; private set; }
        public string? Item12 { get; private set; }
        public string? Item13 { get; private set; }
        public string? Item14 { get; private set; }
        public string? Item15 { get; private set; }
        public string? Item16 { get; private set; }
        public string? Item17 { get; private set; }
        public string? Item18 { get; private set; }
        public string? Item19 { get; private set; }
        public string? Item20 { get; private set; }
        public string? Calcs { get; private set; }

        public RewardGridEntry(RewardDetails res, string[] itemnames, LanguageID lang = LanguageID.English)
        {
            var str = res.GetStrings(itemnames, lang);
            Seed = $"{res.Seed:X8}";
            Item1 = str[0];
            Item2 = str[1];
            Item3 = str[2];
            Item4 = str[3];
            Item5 = str[4];
            Item6 = str[5];
            Item7 = str[6];
            Item8 = str[7];
            Item9 = str[8];
            Item10 = str[9];
            Item11 = str[10];
            Item12 = str[11];
            Item13 = str[12];
            Item14 = str[13];
            Item15 = str[14];
            Item16 = str[15];
            Item17 = str[16];
            Item18 = str[17];
            Item19 = str[18];
            Item20 = str[19];
            Calcs = $"{res.Calcs}";
        }

        public static List<GridEntry> GetGridEntriesFromList(List<TeraDetails> reslist)
        {
            var list = new List<GridEntry>();
            foreach (var res in reslist)
                list.Add(new GridEntry(res));
            return list;
        }
    }

    public class RewardFilter
    {
        public Reward[]? FilterRewards { get; set; }

        public bool IsFilterMatch(RewardDetails res)
        {
            var itemlist = new List<Reward>();
            foreach (var item in res.Rewards!)
            {
                var index = itemlist.FindIndex(i => i.ItemID == item.ItemID);
                if (index >= 0) 
                    itemlist[index].Amount += item.Amount;
                else 
                    itemlist.Add(new Reward { ItemID = item.ItemID, Amount = item.Amount });
            }

            var match = 0;
            foreach(var item in itemlist)
                foreach(var filter in FilterRewards!)
                    if (item.CompareItem(filter, true))
                        match++;

            return match >= FilterRewards!.Length;
        }

        public bool IsFilterNull()
        {
            if (FilterRewards is null)
                return true;
            if(FilterRewards.Length <= 0)
                return true;

            return false;
        }

        public bool CompareFilter(RewardFilter res)
        {
            if(res.FilterRewards is null || FilterRewards is null)
                return false;
            if (res.FilterRewards.Length != FilterRewards.Length)
                return false;

            for(var i = 0; i < FilterRewards.Length; i++)
                if (!res.FilterRewards[i].CompareItem(FilterRewards[i]))
                    return false;

            return true;
        }

        public bool NeedAccurate()
        {
            if(!IsFilterNull())
                foreach(var f in FilterRewards!)
                    if(f.NeedAccurate())
                        return true;
            return false;
        }
    }
}
