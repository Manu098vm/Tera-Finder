using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class RewardListForm : Form
{
    public RewardListForm(string language, MoveType tera, List<Reward>? lvl0 = null, List<Reward>? lvl1 = null, List<Reward>? lvl2 = null, List<Reward>? lvl3 = null)
    {
        InitializeComponent();
        this.TranslateInterface(language);

        if (lvl0 is not null)
            foreach (var item in lvl0)
                gridNoBoost.Rows.Add([item.GetItemName(tera, language: language, quantity: false), $"{item.Amount}"]);

        if (lvl1 is not null)
            foreach (var item in lvl1)
                grid1Boost.Rows.Add([item.GetItemName(tera, language: language, quantity: false), $"{item.Amount}"]);

        if (lvl2 is not null)
            foreach (var item in lvl2)
                grid2Boost.Rows.Add([item.GetItemName(tera, language: language, quantity: false), $"{item.Amount}"]);

        if (lvl3 is not null)
            foreach (var item in lvl3)
                grid3Boost.Rows.Add([item.GetItemName(tera, language: language, quantity: false), $"{item.Amount}"]);
    }
}
