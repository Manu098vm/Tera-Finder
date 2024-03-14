using PKHeX.Core;
using System.Collections.Concurrent;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class CalculatorForm : Form
{
    public EditorForm Editor = null!;
    private TeraFilter? Filter = null!;
    private CancellationTokenSource Token = new();

    private readonly ConcurrentBag<TeraDetails> CalculatedList = [];
    private readonly ConcurrentList<GridEntry> GridEntries = [];

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
        GenderListAscii = [.. GameInfo.GenderSymbolASCII];
        GenderListUnicode = [.. GameInfo.GenderSymbolUnicode];
        ShinyList = [Strings["TeraShiny.Any"], Strings["TeraShiny.No"], Strings["TeraShiny.Yes"], Strings["TeraShiny.Star"], Strings["TeraShiny.Square"]];

        var progress = (int)Editor.Progress;
        cmbProgress.SelectedIndex = progress;
        cmbGame.SelectedIndex = Editor.SAV.Version == GameVersion.VL ? 1 : 0;
        txtTID.Text = $"{Editor.SAV.TrainerTID7}";
        txtSID.Text = $"{Editor.SAV.TrainerSID7}";
        if (!IsBlankSAV()) grpGameInfo.Enabled = false;

        txtSeed.Text = Editor.txtSeed.Text;
        cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;

        cmbTeraType.Items.Clear();
        cmbTeraType.Items.Add(Strings["Any"]);
        cmbTeraType.Items.AddRange(TypeList);
        cmbNature.Items.Clear();
        cmbNature.Items.AddRange(NatureList);
        cmbNature.Items.Add(Strings["Any"]);

        cmbMap.Items.Clear();
        cmbMap.Items.Add(Strings["Plugin.MapPaldea"]);
        cmbMap.Items.Add(Strings["Plugin.MapKitakami"]);
        cmbMap.Items.Add(Strings["Plugin.MapBlueberry"]);

        cmbMap.SelectedIndex = (int)Editor.CurrMap;
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

        TranslateCmbProgress();
        TranslateCmbShiny();
        TranslateCmbGame();
        TranslateCmbContent();

        toolTip.SetToolTip(showresults, Strings["ToolTipAllResults"]);

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
            { "CalculatorForm.btnCopySeed", "Copy Seed" },
            { "Plugin.MapPaldea", "Paldea" },
            { "Plugin.MapKitakami", "Kitakami" },
            { "Plugin.MapBlueberry", "Blueberry" },
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
        btnCopySeed.Text = Strings["CalculatorForm.btnCopySeed"];
    }

    private void TranslateCmbGame()
    {
        cmbGame.Items[0] = Strings["GameVersionSL"];
        cmbGame.Items[1] = Strings["GameVersionVL"];
    }

    private bool IsBlankSAV()
    {
        if (Editor.Progress is GameProgress.Beginning)
            return true;
        return false;
    }

    private EncounterRaidTF9[] GetCurrentEncounters() => (RaidContent)cmbContent.SelectedIndex switch
    {
        RaidContent.Standard => (TeraRaidMapParent)cmbMap.SelectedIndex switch
        {
            TeraRaidMapParent.Kitakami => Editor.Kitakami,
            TeraRaidMapParent.Blueberry => Editor.Blueberry,
            _ => Editor.Paldea,
        },
        RaidContent.Black => (TeraRaidMapParent)cmbMap.SelectedIndex switch
        {
            TeraRaidMapParent.Kitakami => Editor.KitakamiBlack,
            TeraRaidMapParent.Blueberry => Editor.BlueberryBlack,
            _ => Editor.PaldeaBlack,
        },
        RaidContent.Event => Editor.Dist,
        RaidContent.Event_Mighty => Editor.Mighty,
        _ => throw new NotImplementedException(nameof(cmbContent.SelectedIndex)),
    };

    private void cmbMap_IndexChanged(object sender, EventArgs e)
    {
        EncounterRaidTF9[] encs = GetCurrentEncounters();
        var species = EncounterRaidTF9.GetAvailableSpecies(encs, GetStars(), NameList, FormList, TypeList, Strings);
        cmbSpecies.Items.Clear();
        cmbSpecies.Items.Add(Strings["Any"]);
        cmbSpecies.Items.AddRange([..species]);
        cmbSpecies.SelectedIndex = 0;
    }

    private void cmbContent_IndexChanged(object sender, EventArgs e) => UpdateCmbStars(sender, e);
    private void cmbProgress_IndexChanged(object sender, EventArgs e) => UpdateCmbStars(sender, e);

    private void UpdateCmbStars(object sender, EventArgs e)
    {
        if (cmbContent.SelectedIndex == -1 || cmbProgress.SelectedIndex == -1 || cmbGame.SelectedIndex == -1)
            return;

        var stars = TranslatedStars();
        if (cmbContent.SelectedIndex == 0 && cmbProgress.SelectedIndex == 1)
            stars = [stars[1], stars[2]];
        else if (cmbContent.SelectedIndex == 0 && cmbProgress.SelectedIndex == 2)
            stars = [stars[1], stars[2], stars[3]];
        else if (cmbContent.SelectedIndex == 0 && cmbProgress.SelectedIndex == 3)
            stars = [stars[1], stars[2], stars[3], stars[4]];
        else if (cmbContent.SelectedIndex == 0 && cmbProgress.SelectedIndex is 4 or 5)
            stars = [stars[3], stars[4], stars[5]];

        else if (cmbContent.SelectedIndex == 1)
            stars = [stars[6]];

        else if (cmbContent.SelectedIndex == 2)
        {
            var encounters = (EncounterEventTF9[])GetCurrentEncounters();
            var eventProgress = EventUtil.GetEventStageFromProgress((GameProgress)cmbProgress.SelectedIndex);
            var version = cmbGame.SelectedIndex == 1 ? GameVersion.SL : GameVersion.VL;

            var possibleStars = EncounterEventTF9.GetPossibleEventStars(encounters, eventProgress, version);
            var starsList = new List<string>();

            foreach (var s in possibleStars.OrderBy(star => star))
                starsList.Add(stars[s]);

            if (starsList.Count > 0)
                stars = [.. starsList];
            else
                stars = [stars[1], stars[2], stars[3], stars[4], stars[5]];
        }

        else if (cmbContent.SelectedIndex == 3)
            stars = [stars[7]];

        cmbStars.Items.Clear();
        cmbStars.Items.AddRange(stars);
        if (cmbStars.SelectedIndex == 0)
            cmbStars_IndexChanged(sender, e);
        cmbStars.SelectedIndex = 0;
    }

    private void cmbStars_IndexChanged(object sender, EventArgs e)
    {
        EncounterRaidTF9[] encs = GetCurrentEncounters();
        var species = EncounterRaidTF9.GetAvailableSpecies(encs, GetStars(), NameList, FormList, TypeList, Strings);

        cmbSpecies.Items.Clear();
        cmbSpecies.Items.Add(Strings["Any"]);
        cmbSpecies.Items.AddRange([..species]);
        cmbSpecies.SelectedIndex = 0;
    }

    private void cmbSpecies_IndexChanged(object sender, EventArgs e)
    {
        if (cmbSpecies.SelectedIndex > 0)
        {
            var entity = GetSpeciesFormIndex();
            var entry = PersonalTable.SV.GetFormEntry(entity.species, entity.form);
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
        //cmbStars.SelectedIndex = 0;
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

    private byte GetStars()
    {
        if (cmbStars.Text.Equals(Strings["Any"]))
            return 0;
        else
            return (byte)char.GetNumericValue(cmbStars.Text[0]);
    }

    private (ushort species, byte form, byte index) GetSpeciesFormIndex() => GetSpeciesFormIndex(cmbSpecies.Text);

    private (ushort species, byte form, byte index) GetSpeciesFormIndex(string str)
    {
        (ushort species, byte form, byte index) result = (0, 0, 0);
        str = str.Replace($" ({Strings["GameVersionSL"]})", string.Empty).Replace($" ({Strings["GameVersionVL"]})", string.Empty);
        if (!str.Equals(Strings["Any"]))
        {
            var formLocation = str.IndexOf('-');
            var isForm = Array.IndexOf(NameList, str) == -1 && formLocation > 0;

            if (byte.TryParse(str[^2].ToString(), out var index) && str[^1] == ')')
            {
                result.index = index;
                str = str[..^4];
            }
            if (!isForm)
            {
                var species = Editor.Language.ToLower().Equals("en") ? str :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(NameList, str)];
                result.species = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty).Replace("-", string.Empty));
            }
            else
            {
                var species = Editor.Language.ToLower().Equals("en") ? str[..formLocation] :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(NameList, str[..formLocation])];
                result.species = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty).Replace("-", string.Empty));
                result.form = ShowdownParsing.GetFormFromString(str.AsSpan()[(formLocation + 1)..], GameInfo.GetStrings(Editor.Language), result.species, EntityContext.Gen9);
            }
        }
        return result;
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

    private async void btnApply_Click(object sender, EventArgs e)
    {
        lblFound.Visible = false;
        CreateFilter();
        if (!CalculatedList.IsEmpty)
        {
            DialogResult dialogue = MessageBox.Show(Strings["FiltersPopup"], Strings["FiltersApply"], MessageBoxButtons.YesNo);
            if (dialogue is DialogResult.Yes)
            {
                dataGrid.DataSource = null;
                GridEntries.Clear();
                await Task.Run(() =>
                {
                    Parallel.ForEach(CalculatedList, el =>
                    {
                        if (Filter is null || Filter.IsFilterMatch(el))
                            GridEntries.Add(new GridEntry(el, NameList, AbilityList, NatureList, MoveList, TypeList, FormList, GenderListAscii, GenderListUnicode, ShinyList));
                    });
                });
                GridEntries.FinalizeElements();
                dataGrid.DataSource = GridEntries;
            }
        }
        UpdateFoundLabel();
    }

    private void CreateFilter()
    {
        var stars = GetStars();
        var entity = GetSpeciesFormIndex();
        var teraType = (sbyte)(cmbTeraType.SelectedIndex - 1);
        var gender = GetGender();

        var encounterFilter = entity.species > 0 || stars > 0;
        var ivFilter = nHpMin.Value > 0 || nAtkMin.Value > 0 || nDefMin.Value > 0 || nSpaMin.Value > 0 || nSpdMin.Value > 0 || nSpeMin.Value > 0 || numScaleMin.Value > 0 ||
            nHpMax.Value < 31 || nAtkMax.Value < 31 || nDefMax.Value < 31 || nSpaMax.Value < 31 || nSpdMax.Value < 31 || nSpeMax.Value < 31;
        var statFilter = teraType != -1 || cmbAbility.SelectedIndex != 0 || cmbNature.SelectedIndex != 25 || gender is not Gender.Random;
        var auxFilter = numScaleMin.Value > 0 || numScaleMax.Value < 255 || cmbEC.SelectedIndex == 1;

        var filter = new TeraFilter(encounterFilter, ivFilter, statFilter, auxFilter)
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
            Stars = stars,
            Species = entity.species,
            Form = entity.form,
            TeraType = teraType,
            AbilityNumber = cmbAbility.SelectedIndex == 3 ? 4 : cmbAbility.SelectedIndex,
            Nature = (Nature)cmbNature.SelectedIndex,
            Gender = gender,
            Shiny = (TeraShiny)cmbShiny.SelectedIndex,

            AltEC = cmbEC.SelectedIndex == 1,
            IsFormFilter = entity.form > 0,
        };

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
        lblFound.Visible = false;
        grpFilters.Enabled = false;
        grpGameInfo.Enabled = false;
        cmbContent.Enabled = false;
        showresults.Enabled = false;
        txtSeed.Enabled = false;
        numFrames.Enabled = false;
        cmbMap.Enabled = false;
    }

    private void EnableControls(bool enableProfile = false)
    {
        grpGameInfo.Enabled = enableProfile;
        grpFilters.Enabled = true;
        cmbContent.Enabled = true;
        showresults.Enabled = true;
        txtSeed.Enabled = true;
        numFrames.Enabled = true;
        cmbMap.Enabled = true;
    }

    private void UpdateFoundLabel()
    {
        if (Filter is not null && !Filter.IsFilterNull())
        {
            if (showresults.Checked)
                try
                {
                    lblFound.Text = $"{Strings["Found"]}: {GridEntries.Count}";
                }
                catch (Exception)
                {
                    lblFound.Text = $"{Strings["Found"]}: {GridEntries.LongCount()}";
                }
            else
                lblFound.Text = $"{Strings["Found"]}: {(!GridEntries.IsEmpty ? Strings["True"] : Strings["False"])}";
            lblFound.Visible = true;
        }
        else
            lblFound.Visible = false;
    }

    public async void btnSearch_Click(object sender, EventArgs e)
    {
        (ushort species, byte form, byte index) entity;

        if (btnSearch.Text.Equals(Strings["ActionSearch"]))
        {
            Token = new();
            if (cmbProgress.SelectedIndex is (int)GameProgress.Beginning)
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
            if ((cmbSpecies.Text.Contains(Strings["GameVersionSL"]) && cmbGame.SelectedIndex == 1) || 
                (cmbSpecies.Text.Contains(Strings["GameVersionVL"]) && cmbGame.SelectedIndex == 0))
            {
                cmbSpecies.Focus();
                return;
            }
            try
            {
                entity = GetSpeciesFormIndex();
            }
            catch (Exception)
            {
                cmbSpecies.Focus();
                return;
            }

            CreateFilter();
            if (Filter is null || !Filter.EncounterFilter || Filter.Stars == 0)
            {
                MessageBox.Show("No stars filter is set. Please select a stars filter.");
                cmbStars.Focus();
                return;
            }

            if (!CalculatedList.IsEmpty)
            {
                dataGrid.DataSource = null;
                CalculatedList.Clear();
                GridEntries.Clear();
            }
            btnSearch.Text = Strings["ActionStop"];
            DisableControls();
            //ActiveForm.Update();

            var sav = (SAV9SV)Editor.SAV.Clone();
            sav.TrainerTID7 = Convert.ToUInt32(txtTID.Text, 10);
            sav.TrainerSID7 = Convert.ToUInt32(txtSID.Text, 10);
            sav.Version = cmbGame.SelectedIndex == 0 ? GameVersion.SL : GameVersion.SV;
            var progress = (GameProgress)cmbProgress.SelectedIndex;
            var content = (RaidContent)cmbContent.SelectedIndex;

            try
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                await StartSearch(sav, progress, content, entity.index, (TeraRaidMapParent)cmbMap.SelectedIndex, Token);
#if DEBUG
                if (!GridEntries.IsFinalized)
                    MessageBox.Show("Something went wrong: Result list isn't finalized.");
#endif
                while (dataGrid.RowCount < GridEntries.Count)
                {
                    dataGrid.DataSource = null;
                    dataGrid.DataSource = GridEntries;
                }

                stopwatch.Stop();
                MessageBox.Show($"{stopwatch.Elapsed}");

                btnSearch.Text = Strings["ActionSearch"];
                EnableControls(IsBlankSAV());
                UpdateFoundLabel();
            }
            catch (OperationCanceledException)
            {
                btnSearch.Text = Strings["ActionSearch"];
                EnableControls(IsBlankSAV());
                UpdateFoundLabel();
            }
        }
        else
        {
            Token.Cancel();
            btnSearch.Text = Strings["ActionSearch"];
            EnableControls(IsBlankSAV());
            UpdateFoundLabel();
            return;
        }
    }

    private async Task StartSearch(SAV9SV sav, GameProgress progress, RaidContent content, byte index, TeraRaidMapParent map, CancellationTokenSource token)
    {
        var seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);

        var encounters = GetCurrentEncounters();

        var possibleGroups = new HashSet<byte>();
        if (index == 0 && content is RaidContent.Event or RaidContent.Event_Mighty)
            foreach (var enc in encounters.Where(Filter.IsEncounterMatch))
                possibleGroups.Add(enc.Index);
        else
            possibleGroups.Add(index);

        await Task.Run(() =>
        {
            foreach (var group in possibleGroups)
            {
                EncounterRaidTF9[] effective_encounters = content switch
                {
                    RaidContent.Standard or RaidContent.Black => ((EncounterTeraTF9[])encounters).Where(e => e.Index == group && Filter.IsEncounterMatch(e)).ToArray(),
                    RaidContent.Event or RaidContent.Event_Mighty => ((EncounterEventTF9[])encounters).Where(e => e.Index == group && Filter.IsEncounterMatch(e)).ToArray(),
                    _ => throw new NotImplementedException(nameof(content)),
                };

                var romMaxRate = sav.Version is GameVersion.VL ? EncounterTera9.GetRateTotalVL(Filter.Stars, map) : EncounterTera9.GetRateTotalSL(Filter.Stars, map);
                var eventProgress = EventUtil.GetEventStageFromProgress(progress);

                Parallel.For(0L, (long)numFrames.Value, (i, iterator) =>
                {
                    if (token.IsCancellationRequested)
                        iterator.Break();

                    if (EncounterRaidTF9.TryGenerateTeraDetails((uint)i, effective_encounters, Filter, romMaxRate, sav.Version, progress, eventProgress, content, sav.ID32, group, out _, out var result))
                    {
                        CalculatedList.Add(result.Value);
                        GridEntries.Add(new GridEntry(result.Value, NameList, AbilityList, NatureList, MoveList, TypeList, FormList, GenderListAscii, GenderListUnicode, ShinyList));
                        
                        if (!showresults.Checked)
                            token.Cancel();
                    }
                });
            }
            GridEntries.FinalizeElements();
        });
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

    private readonly static string[] TeraStars = [
        "Any",
        "1S ☆",
        "2S ☆☆",
        "3S ☆☆☆",
        "4S ☆☆☆☆",
        "5S ☆☆☆☆☆",
        "6S ☆☆☆☆☆☆",
        "7S ☆☆☆☆☆☆☆",
    ];

    private void btnSaveAll_Click(object sender, EventArgs e) => dataGrid.SaveAllTxt(Editor.Language);

    private void btnSave_Click(object sender, EventArgs e) => dataGrid.SaveSelectedTxt(Editor.Language);

    private void btnToPkmEditor_Click(object sender, EventArgs e) => dataGrid.SendSelectedPk9Editor(this, Editor.Language, (TeraRaidMapParent)cmbMap.SelectedIndex);

    private void btnSendToEditor_Click(object sender, EventArgs e) => dataGrid.SendSelectedRaidEditor(this, Editor.Language);

    private void btnSavePk9_Click(object sender, EventArgs e) => dataGrid.SaveSelectedPk9(this, Editor.Language, (TeraRaidMapParent)cmbMap.SelectedIndex);

    private void btnViewRewards_Click(object sender, EventArgs e) => dataGrid.ViewRewards(this, Editor.Language, (TeraRaidMapParent)cmbMap.SelectedIndex);

    private void btnCopySeed_Click(object sender, EventArgs e) => dataGrid.CopySeed(Editor.Language);
}
