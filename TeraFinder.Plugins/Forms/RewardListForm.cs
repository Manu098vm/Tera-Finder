using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class RewardListForm : Form
{
    public RewardListForm(string language, List<Reward>? lvl0 = null, List<Reward>? lvl1 = null, List<Reward>? lvl2 = null, List<Reward>? lvl3 = null)
    {
        InitializeComponent();
        this.TranslateInterface(language);
        var items = GameInfo.GetStrings(language).itemlist;

        if(lvl0 is not null)
        {
            foreach(var item in lvl0)
            {
                if (RewardUtil.IsTM(item.ItemID))
                    gridNoBoost.Rows.Add($"{RewardUtil.GetNameTM(item.ItemID, items, GameInfo.GetStrings(language).movelist)}  {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID < items.Length)
                    gridNoBoost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    gridNoBoost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl1 is not null)
        {
            foreach (var item in lvl1)
            {
                if (RewardUtil.IsTM(item.ItemID))
                    grid1Boost.Rows.Add($"{RewardUtil.GetNameTM(item.ItemID, items, GameInfo.GetStrings(language).movelist)}  {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID < items.Length)
                    grid1Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid1Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl2 is not null)
        {
            foreach (var item in lvl2)
            {
                if (RewardUtil.IsTM(item.ItemID))
                    grid2Boost.Rows.Add($"{RewardUtil.GetNameTM(item.ItemID, items, GameInfo.GetStrings(language).movelist)}  {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID < items.Length)
                    grid2Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid2Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl3 is not null)
        {
            foreach (var item in lvl3)
            {
                if (RewardUtil.IsTM(item.ItemID))
                    grid3Boost.Rows.Add($"{RewardUtil.GetNameTM(item.ItemID, items, GameInfo.GetStrings(language).movelist)}  {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID < items.Length)
                    grid3Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid3Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }
    }
}
