using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class CalculatorForm : Form
{
    public EditorForm Editor = null!;
    private List<TeraDetails> CalculatedList = new();
    private TeraFilter? Filter = null;
    private CancellationTokenSource Token = new ();

    private Dictionary<string, string> Strings = null!;

    public string[] NameList = null!;
    public string[] FormList = null!;
    public string[] AbilityList = null!;
    public string[] NatureList = null!;
    public string[] MoveList = null!;
    public string[] TypeList = null!;
    public string[] GenderListAscii = null!;
    public string[] GenderListUnicode = null!;
    public string[] ShinyList = null!;

    public CalculatorForm(EditorForm editor)
    {
        InitializeComponent();
        Editor = editor;
        GenerateDictionary();
        TranslateDictionary(Editor.Language);
        this.TranslateInterface(Editor.Language);
        TranslateContextMenu();

        if (Editor.PKMEditor is null)
            contextMenuStrip.Items.Remove(btnToPkmEditor);
        if (Application.OpenForms.OfType<EditorForm>().FirstOrDefault() is null)
            contextMenuStrip.Items.Remove(btnSendToEditor);

        NameList = GameInfo.GetStrings(Editor.Language).specieslist;
        FormList = GameInfo.GetStrings(Editor.Language).forms;
        AbilityList = GameInfo.GetStrings(Editor.Language).abilitylist;
        NatureList = GameInfo.GetStrings(Editor.Language).natures;
        MoveList = GameInfo.GetStrings(Editor.Language).movelist;
        TypeList = GameInfo.GetStrings(Editor.Language).types;
        GenderListAscii = GameInfo.GenderSymbolASCII.ToArray();
        GenderListUnicode = GameInfo.GenderSymbolUnicode.ToArray();
        ShinyList = new string[] { Strings["TeraShiny.Any"], Strings["TeraShiny.No"], Strings["TeraShiny.Yes"], Strings["TeraShiny.Star"], Strings["TeraShiny.Square"] };

        var progress = (int)(Editor.Progress == GameProgress.None ? 0 : Editor.Progress);
        cmbProgress.SelectedIndex = progress;
        cmbGame.SelectedIndex = Editor.SAV.Game == (int)GameVersion.VL ? 1 : 0;
        txtTID.Text = $"{Editor.SAV.TrainerTID7}";
        txtSID.Text = $"{Editor.SAV.TrainerSID7}";
        if (!IsBlankSAV()) grpGameInfo.Enabled = false;

        txtSeed.Text = Editor.txtSeed.Text;
        cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;
        var content = (RaidContent)cmbContent.SelectedIndex;
        numEventCt.Value = content >= RaidContent.Event ? TeraUtil.GetDeliveryGroupID(Editor.SAV, Editor.Progress, content,
            content is RaidContent.Event_Mighty ? Editor.Mighty : Editor.Dist, Editor.cmbDens.SelectedIndex) : -1;

        cmbTeraType.Items.Clear();
        cmbTeraType.Items.Add(Strings["Any"]);
        cmbTeraType.Items.AddRange(TypeList);
        cmbNature.Items.Clear();
        cmbNature.Items.AddRange(NatureList);
        cmbNature.Items.Add(Strings["Any"]);

        cmbSpecies.SelectedIndex = 0;
        cmbTeraType.SelectedIndex = 0;
        cmbEC.Items[0] = Strings["Any"];
        cmbEC.Items[1] = Strings["AltEC"];
        cmbEC.SelectedIndex = 0;
        cmbAbility.SelectedIndex = 0;
        cmbNature.SelectedIndex = 25;
        cmbGender.Items[0] = Strings["Any"];
        cmbGender.SelectedIndex = 0;
        cmbShiny.SelectedIndex = 0;
        nHpMax.Value = 31;
        nAtkMax.Value = 31;
        nDefMax.Value = 31;
        nSpaMax.Value = 31;
        nSpdMax.Value = 31;
        nSpeMax.Value = 31;
        numScaleMax.Value = 255;

        toolTip.SetToolTip(showresults, Strings["ToolTipAllResults"]);

        TranslateCmbProgress();
        TranslateCmbShiny();
        TranslateCmbGame();
        TranslateCmbContent();

        dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        dataGrid.RowHeadersVisible = false;
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "Any", "Any" },
            { "StarsAbbreviation", "S" },
            { "Found", "Found" },
            { "True", "True" },
            { "False", "False" },
            { "FiltersPopup", "Do you want to apply filters in the existing search?" },
            { "FiltersApply", "Apply Filters" },
            { "ActionSearch", "Search" },
            { "ActionStop", "Stop" },
            { "ToolTipAllResults", "Disabled - Stop each thread search at the first result that matches the filters.\n" +
            "Enabled - Compute all possible results until Max Calcs number is reached.\nIgnored if no filter is set." },
            { "TeraShiny.Any", "Any" },
            { "TeraShiny.No", "No" },
            { "TeraShiny.Yes", "Yes" },
            { "TeraShiny.Star", "Star" },
            { "TeraShiny.Square", "Square" },
            { "TeraShiny.CmbStar", "Only Star" },
            { "TeraShiny.CmbSquare", "Only Square" },
            { "GameProgress.Beginning", "Beginning" },
            { "GameProgress.UnlockedTeraRaids", "Unlocked Tera Raids" },
            { "GameProgress.Unlocked3Stars", "Unlocked 3 Stars" },
            { "GameProgress.Unlocked4Stars", "Unlocked 4 Stars" },
            { "GameProgress.Unlocked5Stars", "Unlocked 5 Stars" },
            { "GameProgress.Unlocked6Stars", "Unlocked 6 Stars" },
            { "RaidContent.Standard", "Standard" },
            { "RaidContent.Black", "Black" },
            { "RaidContent.Event", "Event" },
            { "RaidContent.Event_Mighty", "Event-Mighty" },
            { "GameVersionSL", "Scarlet" },
            { "GameVersionVL", "Violet" },
            { "AltEC", "EC % 100 = 0" },
            { "CalculatorForm.btnViewRewards", "View Rewards" },
            { "CalculatorForm.btnSaveAll", "Save All Results as TXT" },
            { "CalculatorForm.btnSave", "Save Selected Results as TXT" },
            { "CalculatorForm.btnSavePk9", "Save Selected Result as PK9" },
            { "CalculatorForm.btnToPkmEditor", "Send Selected Result to PKM Editor" },
            { "CalculatorForm.btnSendToEditor", "Send Selected Result to Raid Editor" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void TranslateCmbShiny()
    {
        cmbShiny.Items[0] = Strings["TeraShiny.Any"];
        cmbShiny.Items[1] = Strings["TeraShiny.No"];
        cmbShiny.Items[2] = Strings["TeraShiny.Yes"];
        cmbShiny.Items[3] = Strings["TeraShiny.Star"];
        cmbShiny.Items[4] = Strings["TeraShiny.Square"];
    }

    private void TranslateCmbContent()
    {
        cmbContent.Items[0] = Strings["RaidContent.Standard"];
        cmbContent.Items[1] = Strings["RaidContent.Black"];
        cmbContent.Items[2] = Strings["RaidContent.Event"];
        cmbContent.Items[3] = Strings["RaidContent.Event_Mighty"];
    }

    private void TranslateCmbProgress()
    {
        cmbProgress.Items[0] = Strings["GameProgress.Beginning"];
        cmbProgress.Items[1] = Strings["GameProgress.UnlockedTeraRaids"];
        cmbProgress.Items[2] = Strings["GameProgress.Unlocked3Stars"];
        cmbProgress.Items[3] = Strings["GameProgress.Unlocked4Stars"];
        cmbProgress.Items[4] = Strings["GameProgress.Unlocked5Stars"];
        cmbProgress.Items[5] = Strings["GameProgress.Unlocked6Stars"];
    }

    private void TranslateContextMenu()
    {
        btnViewRewards.Text = Strings["CalculatorForm.btnViewRewards"];
        btnSaveAll.Text = Strings["CalculatorForm.btnSaveAll"];
        btnSave.Text = Strings["CalculatorForm.btnSave"];
        btnSavePk9.Text = Strings["CalculatorForm.btnSavePk9"];
        btnToPkmEditor.Text = Strings["CalculatorForm.btnToPkmEditor"];
        btnSendToEditor.Text = Strings["CalculatorForm.btnSendToEditor"];
    }

    private void TranslateCmbGame()
    {
        cmbGame.Items[0] = Strings["GameVersionSL"];
        cmbGame.Items[1] = Strings["GameVersionVL"];
    }

    private bool IsBlankSAV()
    {
        if(Editor.Progress is GameProgress.Beginning or GameProgress.None)
            return true;
        return false;
    }

    private void cmbContent_IndexChanged(object sender, EventArgs e)
    {
        var stars = TranslatedStars();
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
        sav.TrainerTID7 = Convert.ToUInt32(txtTID.Text, 10);
        sav.TrainerSID7 = Convert.ToUInt32(txtSID.Text, 10);
        sav.Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV;
        var species = TeraUtil.GetAvailableSpecies((SAV9SV)sav, Editor.Language, GetStars(), (RaidContent)cmbContent.SelectedIndex);
        cmbSpecies.Items.Clear();
        cmbSpecies.Items.Add(Strings["Any"]);
        cmbSpecies.Items.AddRange(species.ToArray());
        cmbSpecies.SelectedIndex = 0;
    }

    private void cmbSpecies_IndexChanged(object sender, EventArgs e)
    {
        if (cmbSpecies.SelectedIndex > 0)
        {
            var entity = GetSpeciesAndForm();
            var entry = PersonalTable.SV.GetFormEntry(entity[0], (byte)entity[1]);
            cmbAbility.Items.Clear();
            cmbAbility.Items.Add(Strings["Any"]);
            cmbAbility.Items.Add($"{AbilityList[entry.Ability1]} (1)");
            cmbAbility.Items.Add($"{AbilityList[entry.Ability2]} (2)");
            cmbAbility.Items.Add($"{AbilityList[entry.AbilityH]} (H)");
        }
        else
        {
            cmbAbility.Items.Clear();
            cmbAbility.Items.Add(Strings["Any"]);
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
        if (cmbStars.Text.Equals(Strings["Any"]))
            return 0;
        else
            return (int)char.GetNumericValue(cmbStars.Text[0]);
    }

    private ushort[] GetSpeciesAndForm()
    {
        var res = new ushort[2];
        if (!cmbSpecies.Text.Equals(Strings["Any"]))
        {
            int charLocation = cmbSpecies.Text.IndexOf("-", StringComparison.Ordinal);

            if (charLocation == -1)
            {
                var species = Editor.Language.ToLower().Equals("en") ? cmbSpecies.Text :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(NameList, cmbSpecies.Text)];
                res[0] = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty));
            }
            else
            {
                var species = Editor.Language.ToLower().Equals("en") ? cmbSpecies.Text[..charLocation] :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(NameList, cmbSpecies.Text[..charLocation])];
                res[0] = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty));
                res[1] = ShowdownParsing.GetFormFromString(cmbSpecies.Text.AsSpan()[(charLocation + 1)..], GameInfo.GetStrings(Editor.Language), res[0], EntityContext.Gen9);
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
        lblFound.Visible = false;
        CreateFilter();
        if (dataGrid.Rows.Count > 0)
        {
            DialogResult d = MessageBox.Show(Strings["FiltersPopup"], Strings["FiltersApply"] , MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                var list = new List<GridEntry>();
                foreach (var c in CalculatedList)
                    if (Filter is null || Filter.IsFilterMatch(c))
                        list.Add(new GridEntry(c, NameList, AbilityList, NatureList, MoveList, TypeList, FormList, GenderListAscii, GenderListUnicode, ShinyList));
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
            Species = GetSpeciesAndForm()[0],
            Form = GetSpeciesAndForm()[1],
            TeraType = (sbyte)(cmbTeraType.SelectedIndex - 1),
            AbilityNumber = cmbAbility.SelectedIndex,
            Nature = (byte)cmbNature.SelectedIndex,
            Gender = GetGender(),
            Shiny = (TeraShiny)cmbShiny.SelectedIndex,
            AltEC = cmbEC.SelectedIndex == 1,
            CheckForm = cmbSpecies.SelectedIndex > 0,
        };

        var isblack = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black or RaidContent.Event_Mighty;

        if (Filter is null && filter.IsFilterNull(isblack))
            return;

        if (Filter is not null && Filter.CompareFilter(filter))
            return;

        if (filter.IsFilterNull(isblack))
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
        lblFound.Visible= false;
        grpFilters.Enabled = false;
        grpGameInfo.Enabled = false;
        cmbContent.Enabled = false;
        showresults.Enabled = false;
        txtSeed.Enabled = false;
        numFrames.Enabled = false;
        numEventCt.Enabled = false;
    }

    private void EnableControls(bool enableProfile = false)
    {
        grpGameInfo.Enabled = enableProfile;
        grpFilters.Enabled = true;
        cmbContent.Enabled = true;
        showresults.Enabled = true;
        txtSeed.Enabled = true;
        numFrames.Enabled = true;
        numEventCt.Enabled = true;  
    }

    private void UpdateLabel()
    {
        var isblack = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black or RaidContent.Event_Mighty;
        if (Filter is not null && !Filter.IsFilterNull(isblack))
        {
            if (showresults.Checked)
                try
                {
                    lblFound.Text = $"{Strings["Found"]} {CalculatedList.Count}";
                }
                catch (Exception)
                {
                    lblFound.Text = $"{Strings["Found"]} {CalculatedList.LongCount()}";
                }
            else
                lblFound.Text = $"{Strings["Found"]}  {(CalculatedList.Count > 0 ? Strings["True"] : Strings["False"])}";
            lblFound.Visible = true;
        }
        else
            lblFound.Visible = false;
    }

    public async void btnSearch_Click(object sender, EventArgs e)
    {
        if (btnSearch.Text.Equals(Strings["ActionSearch"]))
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
            btnSearch.Text = Strings["ActionStop"];
            DisableControls();
            ActiveForm!.Update();

            CreateFilter();
            var sav = (SAV9SV)Editor.SAV.Clone();
            sav.TrainerTID7 = Convert.ToUInt32(txtTID.Text, 10);
            sav.TrainerSID7 = Convert.ToUInt32(txtSID.Text, 10);
            sav.Game = cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.SV;
            var progress = (RaidContent)cmbContent.SelectedIndex is RaidContent.Black ? GameProgress.None : (GameProgress)cmbProgress.SelectedIndex;
            var content = (RaidContent)cmbContent.SelectedIndex;

            try
            {
                var griddata= await StartSearch(sav, progress, content, Token);
                dataGrid.DataSource = griddata;
                btnSearch.Text = Strings["ActionSearch"];
                EnableControls(IsBlankSAV());
                UpdateLabel();
            }
            catch(OperationCanceledException)
            {
                btnSearch.Text = Strings["ActionSearch"];
                EnableControls(IsBlankSAV());
                UpdateLabel();
            }
        }
        else
        {
            Token.Cancel();
            btnSearch.Text = Strings["ActionSearch"];
            EnableControls(IsBlankSAV());
            UpdateLabel();
            return;
        }
    }

    private async Task<List<GridEntry>> StartSearch(SAV9SV sav, GameProgress progress, RaidContent content, CancellationTokenSource token)
    {
        var gridList = new List<GridEntry>();
        var seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);

        await Task.Run(() =>
        {
            var nthreads = (uint)numFrames.Value < 1000 ? 1 : Environment.ProcessorCount;
            var gridresults = new List<GridEntry>[nthreads];
            var calcresults = new List<TeraDetails>[nthreads];
            var resetEvent = new ManualResetEvent(false);
            var toProcess = nthreads;
            var maxcalcs = (uint)numFrames.Value;
            var calcsperthread = maxcalcs / (uint)nthreads;

            for (uint j = 0; j < nthreads; j++)
            {
                var n = j;
                var tseed = seed;

                new Thread(delegate ()
                {
                    var tmpgridlist = new List<GridEntry>();
                    var tmpcalclist = new List<TeraDetails>();

                    var initialFrame = calcsperthread * n;
                    var maxframe = n < nthreads - 1 ? calcsperthread * (n + 1) : maxcalcs;
                    tseed += initialFrame;

                    for (ulong i = initialFrame; i <= maxframe && !token.IsCancellationRequested; i++)
                    {
                        var res = CalcResult(tseed, progress, sav, content, i, (int)numEventCt.Value);
                        if (Filter is not null && res is not null && Filter.IsFilterMatch(res))
                        {
                            tmpgridlist.Add(new GridEntry(res, NameList, AbilityList, NatureList, MoveList, TypeList, FormList, GenderListAscii, GenderListUnicode, ShinyList));
                            tmpcalclist.Add(res);
                            if (!showresults.Checked)
                            {
                                token.Cancel();
                                break;
                            }
                        }
                        else if (Filter is null && res is not null)
                        {
                            tmpgridlist.Add(new GridEntry(res, NameList, AbilityList, NatureList, MoveList, TypeList, FormList, GenderListAscii, GenderListUnicode, ShinyList));
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

    private TeraDetails? CalcResult(ulong Seed, GameProgress progress, SAV9SV sav, RaidContent content, ulong calc, int groupid)
    {
        var seed = (uint)(Seed & 0xFFFFFFFF);
        var encounter = content is RaidContent.Standard or RaidContent.Black ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), Editor.Tera!) :
            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, sav, progress, Editor.Dist!, groupid);

        if (encounter is null)
            return null;

        return TeraUtil.CalcRNG(seed, sav.TrainerTID7, sav.TrainerSID7, content, encounter, calc);
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

    private string[] TranslatedStars()
    {
        var res = TeraStars;
        res[0] = Strings["Any"];
        for (var i = 0; i < res.Length; i++)
            res[i] = res[i].Replace("S", Strings["StarsAbbreviation"]);
        return res;
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

    private void btnSaveAll_Click(object sender, EventArgs e) => dataGrid.SaveAllTxt(Editor.Language);

    private void btnSave_Click(object sender, EventArgs e) => dataGrid.SaveSelectedTxt(Editor.Language);

    private void btnToPkmEditor_Click(object sender, EventArgs e) => dataGrid.SendSelectedPk9Editor(this, Editor.Language);

    private void btnSendToEditor_Click(object sender, EventArgs e) => dataGrid.SendSelectedRaidEditor(this, Editor.Language);

    private void btnSavePk9_Click(object sender, EventArgs e) => dataGrid.SaveSelectedPk9(this, Editor.Language);

    private void btnViewRewards_Click(object sender, EventArgs e) => dataGrid.ViewRewards(this, Editor.Language);
}
