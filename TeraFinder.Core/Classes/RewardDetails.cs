using PKHeX.Core;

namespace TeraFinder.Core;

public record Reward
{
    public int ItemID { get; set; }
    public int Amount { get; set; }
    public int Probability { get; set; }
    public int Aux { get; set; }

    public string GetItemName(string[]? itemnames = null, string language = "en", bool quantity = false)
    {
        if (RewardUtil.IsTM(ItemID))
            return $"{RewardUtil.GetNameTM(ItemID, itemnames, GameInfo.GetStrings(language).movelist)} {(quantity ? $"x{Amount}" : "")} {GetClientOrHostString()}";

        itemnames ??= GameInfo.GetStrings(language).itemlist;

        return $"{itemnames[ItemID]} {(quantity ? $"x{Amount}" : "")} {GetClientOrHostString()}";
    }

    private string GetClientOrHostString() =>
        $"{(Aux == 0 ? "" : Aux == 1 ? "(H)" : Aux == 2 ? "(C)" : Aux == 3 ? "(Once)" : "")}";

    public bool CompareItem(Reward item, bool greaterthan = false)
    {
        if (greaterthan && item.ItemID == ushort.MaxValue - 2)
        {
            if (ItemID == ushort.MaxValue-2 || (ItemID >= 1904 && ItemID <= 1908))
                if (Amount >= item.Amount)
                    return true;

            return false;
        }

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

    public bool IsHerba() => ItemID >= 1904 && ItemID <= 1908;
}

public struct RewardDetails : IRaidDetails
{
    public uint Seed { get; set; }
    public List<Reward>? Rewards {get; set;}
    public ushort Species { get; set; }
    public int Stars { get; set; }
    public TeraShiny Shiny { get; set; }
    public byte TeraType { get; set; }
    public byte EventIndex { get; set; }

    public readonly string[] GetStrings(string[] itemnames, string language)
    {
        var list = new string[25];
        if(Rewards is not null)
            for(var i = 0; i < Rewards.Count; i++)
                list[i] = Rewards[i].GetItemName(itemnames, language, true);
        return list;
    }
}

public struct RewardGridEntry
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
    public string? Item21 { get; private set; }
    public string? Item22 { get; private set; }
    public string? Item23 { get; private set; }
    public string? Item24 { get; private set; }
    public string? Item25 { get; private set; }
    public string? ExtraInfo { get; private set; }
    public string? EventIndex { get; private set; }

    public RewardGridEntry(RewardDetails res, string[] itemnames, string[]speciesnames, string[] shinynames, string language)
    {
        var str = res.GetStrings(itemnames, language);
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
        Item21 = str[20];
        Item22 = str[21];
        Item23 = str[22];
        Item24 = str[23];
        Item25 = str[24];
        ExtraInfo = $"{speciesnames[res.Species]} ({res.Stars}☆) {(res.Shiny > TeraShiny.No ? $"({shinynames[(int)res.Shiny]})" : "")}";
        EventIndex = $"{res.EventIndex}";
    }

    public RewardGridEntry(string[] str)
    {
        Seed = str[0];
        Item1 = str[1];
        Item2 = str[2];
        Item3 = str[3];
        Item4 = str[4];
        Item5 = str[5];
        Item6 = str[6];
        Item7 = str[7];
        Item8 = str[8];
        Item9 = str[9];
        Item10 = str[10];
        Item11 = str[11];
        Item12 = str[12];
        Item13 = str[13];
        Item14 = str[14];
        Item15 = str[15];
        Item16 = str[16];
        Item17 = str[17];
        Item18 = str[18];
        Item19 = str[19];
        Item20 = str[20];
        Item21 = str[21];
        Item22 = str[22];
        Item23 = str[23];
        Item24 = str[24];
        Item25 = str[25];
        ExtraInfo = str[22];
        EventIndex = str[23];
    }

    public readonly string[] GetStrings()
    {
        var list = new List<string>
        {
#pragma warning disable CS8604
            Seed,
            Item1,
            Item2,
            Item3,
            Item4,
            Item5,
            Item6,
            Item7,
            Item8,
            Item9,
            Item10,
            Item11,
            Item12,
            Item13,
            Item14,
            Item15,
            Item16,
            Item17,
            Item18,
            Item19,
            Item20,
            Item21,
            Item22,
            Item23,
            Item24,
            Item25,
            ExtraInfo,
            EventIndex,
#pragma warning restore CS8604
        };
        return [.. list];
    }
}

/// <summary>
/// Check if a RewardDetails matches the filter.
/// </summary>
/// <param name="isEncounterFilter">check for Stars and Species.</param>
/// <param name="isAnyHerbaFilter">check for Any Herba Mystica.</param>
public class RewardFilter(bool isEncounterFilter, bool isAnyHerbaFilter)
{
    public bool EncounterFilter { get; init; } = isEncounterFilter;
    protected bool HerbaFilter { get; init; } = isAnyHerbaFilter;

    public Reward[]? FilterRewards { get; set; }
    public EncounterFilter? Encounter { get; set; }
    public TeraShiny Shiny { get; set; }

    public bool IsFilterMatch(RewardDetails res)
    {
        if (EncounterFilter)
        {
            if (Encounter!.Species > 0)
                if (Encounter.Species != res.Species)
                    return false;

            if (Encounter.Stars > 0)
                if (Encounter.Stars != res.Stars)
                    return false;
        }

        if (Shiny > TeraShiny.No)
            if (res.Shiny < TeraShiny.Yes)
                return false;

        if (FilterRewards is not null && FilterRewards.Length > 0)
        {
            var itemlist = new Dictionary<int, int>();
            foreach (var item in res.Rewards!)
            {
                var itemId = HerbaFilter && item.IsHerba() ? ushort.MaxValue - 2 : item.ItemID;
                if (itemlist.ContainsKey(itemId))
                {
                    itemlist[itemId] += item.Amount;
                }
                else
                {
                    itemlist[itemId] = item.Amount;
                }
            }

            var match = 0;
            foreach (var filter in FilterRewards!)
            {
                if (itemlist.TryGetValue(filter.ItemID, out var amount) && amount >= filter.Amount)
                {
                    match++;
                }
            }

            return match >= FilterRewards!.Length;
        }

        return true;
    }

    public bool IsEncounterMatch(EncounterRaidTF9 encounter)
    {
        if (EncounterFilter)
        {
            if (Encounter!.Species > 0)
                if (Encounter.Species != encounter.Species)
                    return false;

            if (Encounter.Stars > 0)
                if (Encounter.Stars != encounter.Stars)
                    return false;
        }

        return true;
    }

    public bool IsFilterNull()
    {
        if (FilterRewards is null && !EncounterFilter && Shiny is TeraShiny.Any)
            return true;
        if(FilterRewards is not null && FilterRewards.Length <= 0 && !EncounterFilter && Shiny is TeraShiny.Any)
            return true;

        return false;
    }

    public bool CompareFilter(RewardFilter res)
    {
        if (res.FilterRewards is not null && FilterRewards is not null)
        {
            if (res.FilterRewards.Length != FilterRewards.Length)
                return false;

            for (var i = 0; i < FilterRewards.Length; i++)
                if (!res.FilterRewards[i].CompareItem(FilterRewards[i]))
                    return false;
        }

        if (EncounterFilter)
        {
            if (res.EncounterFilter)
            {
                if (res.Encounter!.Species != Encounter!.Species)
                    return false;

                if (res.Encounter.Stars != Encounter.Stars)
                    return false;
            }
            else
            {
                return false;
            }
        }

        if (res.Shiny != Shiny)
            return false;

        return true;
    }

    public bool NeedAccurate()
    {
        if (!IsFilterNull())
        {
            if (Shiny > TeraShiny.No)
                return true;

            foreach (var f in FilterRewards!)
                if (f.NeedAccurate())
                    return true;
        }
        return false;
    }
}

public class EncounterFilter(ushort species, byte stars)
{
    public ushort Species { get; init; } = species;
    public byte Stars { get; init; } = stars;
}
