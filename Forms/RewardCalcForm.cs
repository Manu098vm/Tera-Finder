using PKHeX.Core;
using System.Collections.Generic;

namespace TeraFinder.Forms
{
    public partial class RewardCalcForm : Form
    {
        private EditorForm Editor = null!;
        private List<RewardDetails> CalculatedList = new();
        private RewardFilter? Filter = null;
        private string[] Items = null!;
        private CancellationTokenSource Token = new();

        public RewardCalcForm(EditorForm editor)
        {
            InitializeComponent();
            Editor = editor;
            Items = GameInfo.Strings.itemlist;
            Items[0] = "(Any)";

            foreach(var cb in grpItems.Controls.OfType<ComboBox>())
            {
                cb.Items.AddRange(Items);
                cb.SelectedIndex = 0;
            }

            foreach (var num in grpItems.Controls.OfType<NumericUpDown>())
            {
                num.Value = 1;
                num.Minimum = 1;
            }

            var progress = (int)(Editor.Progress == GameProgress.None ? 0 : Editor.Progress);
            cmbProgress.SelectedIndex = progress;
            cmbGame.SelectedIndex = Editor.SAV.Game == (int)GameVersion.VL ? 1 : 0;
            txtTID.Text = $"{Editor.SAV.TrainerID7}";
            txtSID.Text = $"{Editor.SAV.TrainerSID7}";
            if (!IsBlankSAV()) grpProfile.Enabled = false;
            txtSeed.Text = Editor.txtSeed.Text;
            cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;
            cmbBoost.SelectedIndex = 0;

            toolTip.SetToolTip(chkAccurateSearch, $"Force the calculator to determine Pokémon Species and Tera Type from a given seed, " +
                $"in order to accurately determine Materials and Tera Shards types.\nMakes the searches a little slower.");

            dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGrid.RowHeadersVisible = false;
        }

        private void TxtSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                e.Handled = true;
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !(c >= '0' && c <= '9'))
                e.Handled = true;
        }

        private bool IsBlankSAV()
        {
            if (Editor.Progress is GameProgress.Beginning or GameProgress.None)
                return true;
            return false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CreateFilter();
            if (dataGrid.Rows.Count > 0)
            {
                DialogResult d = MessageBox.Show("Do you want to apply filters in the existing search?", "Apply Filters", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    var list = new List<RewardGridEntry>();
                    foreach (var c in CalculatedList)
                        if (Filter is null || Filter.IsFilterMatch(c))
                            list.Add(new RewardGridEntry(c, Items, (LanguageID)Editor.SAV.Language));
                    dataGrid.DataSource = list;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            chkAccurateSearch.Checked = false;
            foreach (var cb in grpItems.Controls.OfType<ComboBox>())
                cb.SelectedIndex = 0;
            foreach (var num in grpItems.Controls.OfType<NumericUpDown>())
                num.Value = 1;
        }

        private void CreateFilter()
        {
            var items = new List<Reward>();
            items.Clear();

            var nums = grpItems.Controls.OfType<NumericUpDown>();
            foreach (var cb in grpItems.Controls.OfType<ComboBox>())
            {
                if (cb.SelectedIndex > 0)
                {
                    var numericName = $"numericUpDown{cb.Name[8..]}";
                    foreach(var num in nums)
                        if(num.Name.Equals(numericName))
                            items.Add(new Reward { ItemID = cb.SelectedIndex, Amount = (int)num.Value });
                }
            }

            var filter = new RewardFilter { FilterRewards = items.ToArray() };

            if (Filter is null && filter.IsFilterNull())
                return;

            if (Filter is not null && Filter.CompareFilter(filter))
                return;

            Filter = filter;
        }

        private void Form_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!Token.IsCancellationRequested)
                Token.Cancel();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text.Equals("Search"))
            {
                Token = new();
                if (cmbProgress.SelectedIndex is (int)GameProgress.Beginning or (int)GameProgress.None)
                {
                    cmbProgress.Focus();
                    return;
                }
                if (txtTID.Text.Equals(""))
                {
                    txtTID.Focus();
                    return;
                }
                if (txtSID.Text.Equals(""))
                {
                    txtSID.Focus();
                    return;
                }

                if (CalculatedList.Count > 0)
                {
                    CalculatedList.Clear();
                    dataGrid.DataSource = new List<RewardGridEntry>();
                }
                btnSearch.Text = "Stop";
                DisableControls();
                ActiveForm.Update();

                CreateFilter();
                var sav = (SAV9SV)Editor.SAV.Clone();
                sav.TrainerID7 = Int32.Parse(txtTID.Text);
                sav.TrainerSID7 = Int32.Parse(txtSID.Text);
                sav.Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV;
                var progress = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black ? GameProgress.None : (GameProgress)cmbProgress.SelectedIndex;
                var content = (RaidContent)cmbContent.SelectedIndex;
                var boost = cmbBoost.SelectedIndex;

                try
                {

                    var griddata = await bgWorkerSearch_DoWork(sav, progress, content, boost, Token);
                    dataGrid.DataSource = griddata;
                    btnSearch.Text = "Search";
                    EnableControls(IsBlankSAV());
                }
                catch (OperationCanceledException)
                {
                    btnSearch.Text = "Search";
                    EnableControls(IsBlankSAV());
                }
            }
            else
            {
                Token.Cancel();
                btnSearch.Text = "Search";
                EnableControls(IsBlankSAV());
                return;
            }
        }

        private void DisableControls()
        {
            grpFilters.Enabled = false;
            grpProfile.Enabled = false;
            cmbContent.Enabled = false;
            chkAllResults.Enabled = false;
            txtSeed.Enabled = false;
            numMaxCalc.Enabled = false;
        }

        private void EnableControls(bool enableProfile = false)
        {
            grpProfile.Enabled = enableProfile;
            grpFilters.Enabled = true;
            cmbContent.Enabled = true;
            chkAllResults.Enabled = true;
            txtSeed.Enabled = true;
            numMaxCalc.Enabled = true;
        }

        private static ulong GetNext(ulong seed) => new Xoroshiro128Plus(seed).Next();

        private async Task<List<RewardGridEntry>> bgWorkerSearch_DoWork(SAV9SV sav, GameProgress progress, RaidContent content, int boost, CancellationTokenSource token)
        {
            var gridList = new List<RewardGridEntry>();
            ulong seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);
            var lang = (LanguageID)Editor.SAV.Language;
            if (seed == 0) seed = Xoroshiro128Plus.XOROSHIRO_CONST;
            var first = CalcResult(seed, progress, sav, content, 0, chkAccurateSearch.Checked, boost);
            if (Filter is not null && first is not null && Filter.IsFilterMatch(first))
                gridList.Add(new RewardGridEntry(first, Items, lang));
            else if (Filter is null && first is not null)
                gridList.Add(new RewardGridEntry(first, Items, lang));

            return await Task.Run(() =>
            {
                for (uint i = 1; i < (uint)numMaxCalc.Value; i++)
                {
                    seed = GetNext(seed);
                    var res = CalcResult(seed, progress, sav, content, i, chkAccurateSearch.Checked, boost);
                    if (Filter is not null && res is not null && Filter.IsFilterMatch(res))
                    {
                        gridList.Add(new RewardGridEntry(res, Items, lang));
                        CalculatedList.Add(res);
                        if (!chkAllResults.Checked)
                            return gridList;
                    }
                    else if (Filter is null && res is not null)
                    {
                        gridList.Add(new RewardGridEntry(res, Items, lang));
                        CalculatedList.Add(res);
                    }
                    if (token.IsCancellationRequested)
                        return gridList;
                }
                return gridList;
            }, token.Token);
        }

        private RewardDetails? CalcResult(ulong Seed, GameProgress progress, SAV9SV sav, RaidContent content, uint calc, bool accuratesearch, int boost)
        {
            var seed = (uint)(Seed & 0xFFFFFFFF);
            var encounter = content is RaidContent.Standard or RaidContent.Black ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), Editor.Tera!) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Mighty!, true) : TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Dist!, false);

            if (encounter is null)
                return null;

            var fixedlist = encounter.IsDistribution ? Editor.DistFixedRewards : Editor.TeraFixedRewards;
            var lotterylist = encounter.IsDistribution ? Editor.DistLotteryRewards : Editor.TeraLotteryRewards;

            var list = accuratesearch ? RewardUtil.GetRewardList(TeraUtil.CalcRNG(seed, sav.TrainerID7, sav.TrainerSID7, content, encounter, calc), 
                encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist, boost) :
                RewardUtil.GetRewardList(seed, encounter.Stars, encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist, boost);

            return new RewardDetails{ Seed = seed, Rewards = list, Calcs = calc };
        }
    }
}
