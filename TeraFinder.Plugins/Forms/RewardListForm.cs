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
        var lang = GameLanguage.GetLanguageIndex(language);

        if(lvl0 is not null)
        {
            foreach(var item in lvl0)
            {
                if (item.ItemID < items.Length)
                    gridNoBoost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue - 1)
                    gridNoBoost.Rows.Add($"{RewardUtil.TeraShard[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue)
                    gridNoBoost.Rows.Add($"{RewardUtil.Material[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    gridNoBoost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl1 is not null)
        {
            foreach (var item in lvl1)
            {
                if (item.ItemID < items.Length)
                    grid1Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue - 1)
                    grid1Boost.Rows.Add($"{RewardUtil.TeraShard[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue)
                    grid1Boost.Rows.Add($"{RewardUtil.Material[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid1Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl2 is not null)
        {
            foreach (var item in lvl2)
            {
                if (item.ItemID < items.Length)
                    grid2Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue - 1)
                    grid2Boost.Rows.Add($"{RewardUtil.TeraShard[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue)
                    grid2Boost.Rows.Add($"{RewardUtil.Material[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid2Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }

        if (lvl3 is not null)
        {
            foreach (var item in lvl3)
            {
                if (item.ItemID < items.Length)
                    grid3Boost.Rows.Add($"{items[item.ItemID]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue - 1)
                    grid3Boost.Rows.Add($"{RewardUtil.TeraShard[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else if (item.ItemID == ushort.MaxValue)
                    grid3Boost.Rows.Add($"{RewardUtil.Material[lang]} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
                else
                    grid3Boost.Rows.Add($"{item.ItemID} {(item.Aux == 1 ? "(H)" : item.Aux == 2 ? "(C)" : item.Aux == 3 ? "(Once)" : "")}", $"{item.Amount}");
            }
        }
    }
}
