using PKHeX.Core;
using static TeraFinder.GridUtil;

namespace TeraFinder.Forms
{
    public partial class RewardCalcForm : Form
    {
        public EditorForm Editor = null!;
        private List<RewardDetails> CalculatedList = new();
        private RewardFilter? Filter = null;
        public string[] Items = null!;
        private CancellationTokenSource Token = new();

        public RewardCalcForm(EditorForm editor)
        {
            InitializeComponent();
            Editor = editor;

            if (Application.OpenForms.OfType<EditorForm>().FirstOrDefault() is null)
                contextMenuStrip.Items.Remove(btnSendSelectedRaid);

            Items = GameInfo.GetStrings(editor.Language).itemlist;
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
                $"in order to accurately determine Materials and Tera Shards types.\nAlso determines if a given reward is Host or Client exclusive.\n" +
                $"Makes the searches a little slower.");
            toolTip1.SetToolTip(chkAllResults, $"Disabled - Stop each thread search at the first result that matches the filters.\n" +
                $"Enabled - Compute all possible results until Max Calcs number is reached.\nIgnored if no filter is set.");

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
            if (Filter is not null && Filter.NeedAccurate())
                chkAccurateSearch.Checked = true;
            if (dataGrid.Rows.Count > 0)
            {
                DialogResult d = MessageBox.Show("Do you want to apply filters in the existing search?", "Apply Filters", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    var list = new List<RewardGridEntry>();
                    foreach (var c in CalculatedList)
                        if (Filter is null || Filter.IsFilterMatch(c))
                            list.Add(new RewardGridEntry(c, Items, Editor.Language));
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
                    var num = nums.Where(num => num.Name.Equals(numericName)).FirstOrDefault()!;
                    items.Add(new Reward { ItemID = cb.SelectedIndex, Amount = (int)num.Value });
                }
            }

            var itemlist = new List<Reward>();
            foreach (var item in items)
            {
                var index = itemlist.FindIndex(i => i.ItemID == item.ItemID);
                if (index >= 0) itemlist[index].Amount += item.Amount;
                else itemlist.Add(new Reward { ItemID = item.ItemID, Amount = item.Amount });
            }

            var filter = new RewardFilter { FilterRewards = itemlist.ToArray() };

            if (Filter is null && filter.IsFilterNull())
                return;

            if (Filter is not null && Filter.CompareFilter(filter))
                return;

            if (filter.IsFilterNull())
                Filter = null;
            else
                Filter = filter;
        }

        private void Form_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!Token.IsCancellationRequested)
                Token.Cancel();
        }

        private void DisableControls()
        {
            grpFilters.Enabled = false;
            grpProfile.Enabled = false;
            cmbContent.Enabled = false;
            cmbBoost.Enabled = false;
            chkAllResults.Enabled = false;
            txtSeed.Enabled = false;
            numMaxCalc.Enabled = false;
        }

        private void EnableControls(bool enableProfile = false)
        {
            grpProfile.Enabled = enableProfile;
            grpFilters.Enabled = true;
            cmbContent.Enabled = true;
            cmbBoost.Enabled = true;
            chkAllResults.Enabled = true;
            txtSeed.Enabled = true;
            numMaxCalc.Enabled = true;
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
                if (Filter is not null && Filter.NeedAccurate())
                    chkAccurateSearch.Checked = true;

                var sav = (SAV9SV)Editor.SAV.Clone();
                sav.TrainerID7 = Int32.Parse(txtTID.Text);
                sav.TrainerSID7 = Int32.Parse(txtSID.Text);
                sav.Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV;
                var progress = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black ? GameProgress.None : (GameProgress)cmbProgress.SelectedIndex;
                var content = (RaidContent)cmbContent.SelectedIndex;
                var boost = cmbBoost.SelectedIndex;

                try
                {
                    //var stopwatch = new Stopwatch();
                    //stopwatch.Start();
                    var griddata = await StartSearch(sav, progress, content, boost, Token);
                    //MessageBox.Show($"Search completed in {stopwatch.ElapsedMilliseconds} ms");
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

        private async Task<List<RewardGridEntry>> StartSearch(SAV9SV sav, GameProgress progress, RaidContent content, int boost, CancellationTokenSource token)
        {
            var gridList = new List<RewardGridEntry>();
            var seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);
            var lang = (LanguageID)Editor.SAV.Language;
            if (seed == 0) seed = 1;

            await Task.Run(() =>
            {
                var nthreads = Environment.ProcessorCount;
                var gridresults = new List<RewardGridEntry>[nthreads];
                var calcresults = new List<RewardDetails>[nthreads];
                var resetEvent = new ManualResetEvent(false);
                var toProcess = nthreads;
                var maxcalcs = (uint)numMaxCalc.Value;
                var calcsperthread = maxcalcs / (uint)nthreads;

                for (uint j = 0; j < nthreads; j++)
                {
                    var n = j;
                    var tseed = seed;

                    new Thread(delegate ()
                    {
                        var tmpgridlist = new List<RewardGridEntry>();
                        var tmpcalclist = new List<RewardDetails>();

                        var initialFrame = calcsperthread * n;
                        var maxframe = n < nthreads - 1 ? calcsperthread * (n + 1) : maxcalcs;
                        tseed += initialFrame;

                        for (uint i = initialFrame; i <= maxframe && !token.IsCancellationRequested; i++)
                        {
                            var res = CalcResult(tseed, progress, sav, content, i, chkAccurateSearch.Checked, boost);
                            if (Filter is not null && res is not null && Filter.IsFilterMatch(res))
                            {
                                tmpgridlist.Add(new RewardGridEntry(res, Items, Editor.Language));
                                tmpcalclist.Add(res);
                                if (!chkAllResults.Checked)
                                {
                                    token.Cancel();
                                    break;
                                }
                            }
                            else if (Filter is null && res is not null)
                            {
                                tmpgridlist.Add(new RewardGridEntry(res, Items, Editor.Language));
                                tmpcalclist.Add(res);
                            }

                            if (token.IsCancellationRequested)
                                break;

                            tseed++;
                        }

                        gridresults[n] = tmpgridlist;
                        calcresults[n] = tmpcalclist;

                        if (Interlocked.Decrement(ref toProcess) == 0 || token.IsCancellationRequested)
                            resetEvent.Set();
                    }).Start();
                }

                resetEvent.WaitOne();

                for (var i = 0; i < nthreads; i++)
                {
                    if (gridresults[i] is not null && gridresults[i].Count > 0)
                        gridList.AddRange(gridresults[i]);
                    if (calcresults[i] is not null && calcresults[i].Count > 0)
                        CalculatedList.AddRange(calcresults[i]);
                }
            }, token.Token);

            return gridList;
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

        private void dataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = dataGrid.HitTest(e.X, e.Y).RowIndex;
                if (index >= 0)
                {
                    contextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void btnSaveAllTxt_Click(object sender, EventArgs e) => dataGrid.SaveAllTxt();

        private void btnSaveSelectedTxt_Click(object sender, EventArgs e) => dataGrid.SaveSelectedTxt();

        private void btnSendSelectedRaid_Click(object sender, EventArgs e) => dataGrid.SendSelectedRaidEditor(this);
    }
}
