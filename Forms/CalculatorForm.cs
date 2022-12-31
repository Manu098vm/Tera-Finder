using PKHeX.Core;
using System.Diagnostics;

namespace TeraFinder
{
    public partial class CalculatorForm : Form
    {
        private EditorForm Editor = null!;
        private List<TeraDetails> CalculatedList = new();
        private TeraFilter? Filter = null;
        private CancellationTokenSource Token = new ();

        public CalculatorForm(EditorForm editor)
        {
            InitializeComponent();
            Editor = editor;

            var progress = (int)(Editor.Progress == GameProgress.None ? 0 : Editor.Progress);
            cmbProgress.SelectedIndex = progress;
            cmbGame.SelectedIndex = Editor.SAV.Game == (int)GameVersion.VL ? 1 : 0;
            txtTID.Text = $"{Editor.SAV.TrainerID7}";
            txtSID.Text = $"{Editor.SAV.TrainerSID7}";
            if (!IsBlankSAV()) grpGameInfo.Enabled = false;

            txtSeed.Text = Editor.txtSeed.Text;
            cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;

            cmbSpecies.SelectedIndex = 0;
            cmbTeraType.SelectedIndex = 0;
            cmbEC.SelectedIndex = 0;
            cmbAbility.SelectedIndex = 0;
            cmbNature.SelectedIndex = 25;
            cmbGender.SelectedIndex = 0;
            cmbShiny.SelectedIndex = 0;
            nHpMax.Value = 31;
            nAtkMax.Value = 31;
            nDefMax.Value = 31;
            nSpaMax.Value = 31;
            nSpdMax.Value = 31;
            nSpeMax.Value = 31;
            numScaleMax.Value = 255;

            toolTip.SetToolTip(showresults, $"If disabled: stop each thread search at the first result that matches the filters. Ignored if no filter is set.");

            dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGrid.RowHeadersVisible = false;
        }

        private bool IsBlankSAV()
        {
            if(Editor.Progress is GameProgress.Beginning or GameProgress.None)
                return true;
            return false;
        }

        private void cmbContent_IndexChanged(object sender, EventArgs e)
        {
            var stars = TeraStars;
            if (cmbContent.SelectedIndex == 0)
                stars = new string[] { stars[0], stars[1], stars[2], stars[3], stars[4], stars[5] };
            if (cmbContent.SelectedIndex == 1)
                stars = new string[] { stars[6] };
            if (cmbContent.SelectedIndex == 2)
                stars = new string[] { stars[0], stars[1], stars[2], stars[3], stars[4], stars[5] };
            if (cmbContent.SelectedIndex == 3)
                stars = new string[] { stars[7] };

            cmbStars.Items.Clear();
            cmbStars.Items.AddRange(stars);
            if (cmbStars.SelectedIndex == 0)
                cmbStars_IndexChanged(sender, e);
            cmbStars.SelectedIndex = 0;
        }

        private void cmbStars_IndexChanged(object sender, EventArgs e)
        {
            var sav = Editor.SAV.Clone();
            sav.TrainerID7 = Int32.Parse(txtTID.Text);
            sav.TrainerSID7 = Int32.Parse(txtSID.Text);
            sav.Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV;
            var species = TeraUtil.GetAvailableSpecies((SAV9SV)sav, GetStars(), (RaidContent)cmbContent.SelectedIndex);
            cmbSpecies.Items.Clear();
            cmbSpecies.Items.Add("Any");
            cmbSpecies.Items.AddRange(species.ToArray());
            cmbSpecies.SelectedIndex = 0;
        }

        private void cmbSpecies_IndexChanged(object sender, EventArgs e)
        {
            if (cmbSpecies.SelectedIndex > 0)
            {
                var entity = GetSpeciesAndForm();
                var entry = PersonalTable.SV.GetFormEntry(entity[0], (byte)entity[1]);
                var abilites = GameInfo.Strings.abilitylist;
                cmbAbility.Items.Clear();
                cmbAbility.Items.Add("Any");
                cmbAbility.Items.Add($"{abilites[entry.Ability1]} (1)");
                cmbAbility.Items.Add($"{abilites[entry.Ability2]} (2)");
                cmbAbility.Items.Add($"{abilites[entry.AbilityH]} (H)");
            }
            else
            {
                cmbAbility.Items.Clear();
                cmbAbility.Items.Add("Any");
                cmbAbility.Items.Add("(1)");
                cmbAbility.Items.Add("(2)");
                cmbAbility.Items.Add("(H)");
            }
            cmbAbility.SelectedIndex = 0;
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbStars.SelectedIndex = 0;
            cmbSpecies.SelectedIndex = 0;
            cmbTeraType.SelectedIndex = 0;
            cmbAbility.SelectedIndex = 0;
            cmbNature.SelectedIndex = 25;
            cmbGender.SelectedIndex = 0;
            cmbShiny.SelectedIndex = 0;
            cmbEC.SelectedIndex = 0;
            nHpMin.Value = 0;
            nAtkMin.Value = 0;
            nDefMin.Value = 0;
            nSpaMin.Value = 0;
            nSpdMin.Value = 0;
            nSpeMin.Value = 0;
            numScaleMin.Value = 0;
            nHpMax.Value = 31;
            nAtkMax.Value = 31;
            nDefMax.Value = 31;
            nSpaMax.Value = 31;
            nSpdMax.Value = 31;
            nSpeMax.Value = 31;
            numScaleMax.Value = 255;
        }

        private int GetStars()
        {
            if (cmbStars.Text.Equals("Any"))
                return 0;
            else
                return (int)char.GetNumericValue(cmbStars.Text[0]);
        }

        private ushort[] GetSpeciesAndForm()
        {
            var res = new ushort[2];
            if (!cmbSpecies.Text.Equals("Any"))
            {
                int charLocation = cmbSpecies.Text.IndexOf("-", StringComparison.Ordinal);

                if (charLocation == -1)
                    res[0] = (ushort)Enum.Parse(typeof(Species), cmbSpecies.Text);
                else
                {
                    res[0] = (ushort)Enum.Parse(typeof(Species), cmbSpecies.Text[..charLocation]);
                    res[1] = ShowdownParsing.GetFormFromString(cmbSpecies.Text[(charLocation + 1)..], GameInfo.Strings, res[0], EntityContext.Gen9);
                }
            }
            return res;
        }

        private Gender GetGender()
        {
            return cmbGender.SelectedIndex switch
            {
                1 => Gender.Male,
                2 => Gender.Female,
                _ => Gender.Random,
            };
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CreateFilter();
            if (dataGrid.Rows.Count > 0)
            {
                DialogResult d = MessageBox.Show("Do you want to apply filters in the existing search?", "Apply Filters" , MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    var list = new List<GridEntry>();
                    foreach (var c in CalculatedList)
                        if (Filter is null || Filter.IsFilterMatch(c))
                            list.Add(new GridEntry(c));
                    dataGrid.DataSource = list;
                }
            }
        }

        private void CreateFilter()
        {
            var filter = new TeraFilter
            {
                MinHP = (int)nHpMin.Value,
                MaxHP = (int)nHpMax.Value,
                MinAtk = (int)nAtkMin.Value,
                MaxAtk = (int)nAtkMax.Value,
                MinDef = (int)nDefMin.Value,
                MaxDef = (int)nDefMax.Value,
                MinSpa = (int)nSpaMin.Value,
                MaxSpa = (int)nSpaMax.Value,
                MinSpd = (int)nSpdMin.Value,
                MaxSpd = (int)nSpdMax.Value,
                MinSpe = (int)nSpeMin.Value,
                MaxSpe = (int)nSpeMax.Value,
                MinScale = (int)numScaleMin.Value,
                MaxScale = (int)numScaleMax.Value,
                Stars = GetStars(),
                Species = (Species)GetSpeciesAndForm()[0],
                Form = GetSpeciesAndForm()[1],
                TeraType = (MoveType)(cmbTeraType.SelectedIndex - 1),
                AbilityNumber = cmbAbility.SelectedIndex,
                Nature = (Nature)cmbNature.SelectedIndex,
                Gender = GetGender(),
                Shiny = (TeraShiny)cmbShiny.SelectedIndex,
                AltEC = cmbEC.SelectedIndex == 1,
            };

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

        private void DisableControls()
        {
            grpFilters.Enabled = false;
            grpGameInfo.Enabled = false;
            cmbContent.Enabled = false;
            showresults.Enabled = false;
            txtSeed.Enabled = false;
            numFrames.Enabled = false;
        }

        private void EnableControls(bool enableProfile = false)
        {
            grpGameInfo.Enabled = enableProfile;
            grpFilters.Enabled = true;
            cmbContent.Enabled = true;
            showresults.Enabled = true;
            txtSeed.Enabled = true;
            numFrames.Enabled = true;
        }

        public async void btnSearch_Click(object sender, EventArgs e)
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
                try
                {
                    GetSpeciesAndForm();
                }
                catch (Exception)
                {
                    cmbSpecies.Focus();
                    return;
                }

                if (CalculatedList.Count > 0)
                {
                    CalculatedList.Clear();
                    dataGrid.DataSource = new List<GridEntry>();
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

                try
                {
                    //var stopwatch = new Stopwatch();
                    //stopwatch.Start();
                    var griddata= await StartSearch(sav, progress, content, Token);
                    //MessageBox.Show($"Search completed in {stopwatch.ElapsedMilliseconds} ms");
                    dataGrid.DataSource = griddata;
                    btnSearch.Text = "Search";
                    EnableControls(IsBlankSAV());
                }
                catch(OperationCanceledException)
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

        private static ulong GetNext(ulong seed) => new Xoroshiro128Plus(seed).Next();

        private static ulong GetNext(ulong seed, uint advances)
        {
            var xoro = new Xoroshiro128Plus(seed);
            for (uint i = 0; i < (advances > 0 ? advances - 1 : 0); i++)
                xoro.Next();
            return xoro.Next();
        }

        private async Task<List<GridEntry>> StartSearch(SAV9SV sav, GameProgress progress, RaidContent content, CancellationTokenSource token)
        {
            var gridList = new List<GridEntry>();
            ulong seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);
            if (seed == 0) seed = Xoroshiro128Plus.XOROSHIRO_CONST;
            var first = CalcResult(seed, progress, sav, content, 0);
            if (Filter is not null && first is not null && Filter.IsFilterMatch(first))
            {
                gridList.Add(new GridEntry(first));
                CalculatedList.Add(first);
            }
            else if (Filter is null && first is not null)
            {
                gridList.Add(new GridEntry(first));
                CalculatedList.Add(first);
            }

            await Task.Run(() =>
            {
                var nthreads = Environment.ProcessorCount;
                var gridresults = new List<GridEntry>[nthreads];
                var calcresults = new List<TeraDetails>[nthreads];
                var resetEvent = new ManualResetEvent(false);
                var toProcess = nthreads;
                var maxcalcs = (uint)numFrames.Value;
                var calcsperthread = maxcalcs / (uint)nthreads;

                for (uint j = 0; j < nthreads; j++)
                {
                    var n = j;

                    new Thread(delegate ()
                    {
                        var tmpgridlist = new List<GridEntry>();
                        var tmpcalclist = new List<TeraDetails>();

                        var initialFrame = calcsperthread * n;
                        var maxframe = n < nthreads - 1 ? calcsperthread * (n + 1) : maxcalcs;
                        seed = token.IsCancellationRequested ? 0 : GetNext(seed, initialFrame);

                        /*if(n == 0 || n == nthreads - 1)
                            MessageBox.Show($"Thread: {n}\n" +
                            $"Initial Frame: {initialFrame}\n" +
                            $"Ending Frame: {maxframe}\n" +
                            $"Max Calcs: {maxcalcs}\n" +
                            $"Calcs per Thread: {calcsperthread}");*/

                        for (uint i = initialFrame; i < maxframe && !token.IsCancellationRequested; i++)
                        {
                            var res = CalcResult(seed, progress, sav, content, i + 1);
                            if (Filter is not null && res is not null && Filter.IsFilterMatch(res))
                            {
                                tmpgridlist.Add(new GridEntry(res));
                                tmpcalclist.Add(res);
                                if (!showresults.Checked)
                                {
                                    token.Cancel();
                                    break;
                                }
                            }
                            else if (Filter is null && res is not null)
                            {
                                tmpgridlist.Add(new GridEntry(res));
                                tmpcalclist.Add(res);
                            }

                            if (token.IsCancellationRequested)
                                break;

                            seed = GetNext(seed);
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

        private TeraDetails? CalcResult(ulong Seed, GameProgress progress, SAV9SV sav, RaidContent content, uint calc)
        {
            var seed = (uint)(Seed & 0xFFFFFFFF);
            var encounter = content is RaidContent.Standard or RaidContent.Black ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), Editor.Tera!) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Mighty!, true) : TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Dist!, false);

            if (encounter is null)
                return null;

            return TeraUtil.CalcRNG(seed, sav.TrainerID7, sav.TrainerSID7, content, encounter, calc);
        }

        private readonly static string[] TeraStars = new string[] {
            "Any",
            "1S ☆",
            "2S ☆☆",
            "3S ☆☆☆",
            "4S ☆☆☆☆",
            "5S ☆☆☆☆☆",
            "6S ☆☆☆☆☆☆",
            "7S ☆☆☆☆☆☆☆",
        };
    }
}
