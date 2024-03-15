using PKHeX.Core;
using System.Collections.Concurrent;
using System.ComponentModel;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class RewardCalcForm : Form
{
    public EditorForm Editor = null!;
    private RewardFilter? Filter = null;
    public string[] SpeciesNames = null!;
    public string[] FormNames = null!;
    public string[] TypeNames = null!;
    public string[] ShinyNames = null!;
    public string[] Items = null!;
    private CancellationTokenSource Token = new();

    private readonly ConcurrentBag<RewardDetails> CalculatedList = [];
    private readonly ConcurrentList<RewardGridEntry> GridEntries = [];

    private Dictionary<string, string> Strings = null!;

    public RewardCalcForm(EditorForm editor)
    {
        InitializeComponent();
        Editor = editor;
        GenerateDictionary();
        TranslateDictionary(Editor.Language);
        this.TranslateInterface(Editor.Language);
        TranslateContextMenu();

        if (Application.OpenForms.OfType<EditorForm>().FirstOrDefault() is null)
            contextMenuStrip.Items.Remove(btnSendSelectedRaid);

        ShinyNames = [Strings["TeraShiny.Any"], Strings["TeraShiny.No"], Strings["TeraShiny.Yes"], Strings["TeraShiny.Star"], Strings["TeraShiny.Square"]];
        SpeciesNames = GameInfo.GetStrings(editor.Language).specieslist;
        FormNames = GameInfo.GetStrings(Editor.Language).forms;
        TypeNames = GameInfo.GetStrings(Editor.Language).types;
        cmbSpecies.Items[0] = Strings["Any"];
        cmbSpecies.Items.AddRange(SpeciesNames[1..]);
        cmbSpecies.SelectedIndex = 0;
        cmbStars.Items[0] = Strings["Any"];
        cmbStars.SelectedIndex = 0;

        Items = GameInfo.GetStrings(editor.Language).itemlist;
        var cbitems = new List<string>
        {
            Strings["Any"],
            Strings["AnyHerba"],
        };
        cbitems.AddRange(Items[1..]);

        var items = cbitems.ToArray();
        foreach (var cb in grpItems.Controls.OfType<ComboBox>())
        {
            cb.Items.AddRange(items);
            cb.SelectedIndex = 0;
        }

        foreach (var num in grpItems.Controls.OfType<NumericUpDown>())
        {
            num.Value = 1;
            num.Minimum = 1;
        }

        var progress = (int)editor.Progress;
        cmbProgress.SelectedIndex = progress;
        cmbGame.SelectedIndex = Editor.SAV.Version == GameVersion.VL ? 1 : 0;
        txtTID.Text = $"{Editor.SAV.TrainerTID7}";
        txtSID.Text = $"{Editor.SAV.TrainerSID7}";
        if (!IsBlankSAV()) grpProfile.Enabled = false;
        txtSeed.Text = Editor.txtSeed.Text;
        cmbContent.SelectedIndex = Editor.cmbContent.SelectedIndex;
        cmbBoost.SelectedIndex = 0;

        cmbMap.Items.Clear();
        cmbMap.Items.Add(Strings["Plugin.MapPaldea"]);
        cmbMap.Items.Add(Strings["Plugin.MapKitakami"]);
        cmbMap.Items.Add(Strings["Plugin.MapBlueberry"]);
        cmbMap.SelectedIndex = (int)Editor.CurrMap;

        toolTip1.SetToolTip(chkAllResults, Strings["ToolTipAllResults"]);

        TranslateCmbProgress();
        TranslateCmbGame();
        TranslateCmbContent();
        TranslateCmbBoost();

        dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
        dataGrid.RowHeadersVisible = false;
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "Any", "Any" },
            { "AnyHerba", "Any Herba Mystica" },
            { "Found", "Found" },
            { "True", "True" },
            { "False", "False" },
            { "FiltersPopup", "Do you want to apply filters in the existing search?" },
            { "FiltersApply", "Apply Filters" },
            { "ActionSearch", "Search" },
            { "ActionStop", "Stop" },
            { "ToolTipAccurate", "Force the calculator to determine Pokémon Tera Type and Shinyness from a given seed, " +
            "in order to accurately determine Tera Shards types and Extra Infos.\nMakes the searches a little slower." },
            { "ToolTipAllResults", "Disabled - Stop each thread search at the first result that matches the filters.\n" +
            "Enabled - Compute all possible results until Max Calcs number is reached.\nIgnored if no filter is set." },
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
            { "SandwichBoost.No Boost", "No Boost"},
            { "SandwichBoost.Level 1", "Level 1"},
            { "SandwichBoost.Level 2", "Level 2"},
            { "SandwichBoost.Level 3", "Level 3"},
            { "GameVersionSL", "Scarlet" },
            { "GameVersionVL", "Violet" },
            { "TeraShiny.Any", "Any" },
            { "TeraShiny.No", "No" },
            { "TeraShiny.Yes", "Yes" },
            { "TeraShiny.Star", "Star" },
            { "TeraShiny.Square", "Square" },
            { "RewardCalcForm.btnSaveAllTxt", "Save All Results as TXT" },
            { "RewardCalcForm.btnSaveSelectedTxt", "Save Selected Results as TXT" },
            { "RewardCalcForm.btnSendSelectedRaid", "Send Selected Result to Raid Editor" },
            { "RewardCalcForm.btnCopySeed", "Copy Seed" },
            { "Plugin.MapPaldea", "Paldea" },
            { "Plugin.MapKitakami", "Kitakami" },
            { "Plugin.MapBlueberry", "Blueberry" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void TranslateCmbProgress()
    {
        cmbProgress.Items[0] = Strings["GameProgress.Beginning"];
        cmbProgress.Items[1] = Strings["GameProgress.UnlockedTeraRaids"];
        cmbProgress.Items[2] = Strings["GameProgress.Unlocked3Stars"];
        cmbProgress.Items[3] = Strings["GameProgress.Unlocked4Stars"];
        cmbProgress.Items[4] = Strings["GameProgress.Unlocked5Stars"];
        cmbProgress.Items[5] = Strings["GameProgress.Unlocked6Stars"];
    }

    private void TranslateCmbContent()
    {
        cmbContent.Items[0] = Strings["RaidContent.Standard"];
        cmbContent.Items[1] = Strings["RaidContent.Black"];
        cmbContent.Items[2] = Strings["RaidContent.Event"];
        cmbContent.Items[3] = Strings["RaidContent.Event_Mighty"];
    }

    private void TranslateCmbBoost()
    {
        cmbBoost.Items[0] = Strings["SandwichBoost.No Boost"];
        cmbBoost.Items[1] = Strings["SandwichBoost.Level 1"];
        cmbBoost.Items[2] = Strings["SandwichBoost.Level 2"];
        cmbBoost.Items[3] = Strings["SandwichBoost.Level 3"];
    }

    private void TranslateContextMenu()
    {
        btnSaveAllTxt.Text = Strings["RewardCalcForm.btnSaveAllTxt"];
        btnSaveSelectedTxt.Text = Strings["RewardCalcForm.btnSaveSelectedTxt"];
        btnSendSelectedRaid.Text = Strings["RewardCalcForm.btnSendSelectedRaid"];
        btnCopySeed.Text = Strings["RewardCalcForm.btnCopySeed"];
    }

    private void TranslateCmbGame()
    {
        cmbGame.Items[0] = Strings["GameVersionSL"];
        cmbGame.Items[1] = Strings["GameVersionVL"];
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
        if (Editor.Progress is GameProgress.Beginning)
            return true;
        return false;
    }

    private void cmbStars_IndexChanged(object sender, EventArgs e)
    {
        EncounterRaidTF9[] encs = GetCurrentEncounters();
        var species = EncounterRaidTF9.GetAvailableSpecies(encs, GetStars(), SpeciesNames, FormNames, TypeNames, Strings);

        cmbSpecies.Items.Clear();
        cmbSpecies.Items.Add(Strings["Any"]);
        cmbSpecies.Items.AddRange([.. species]);
        cmbSpecies.SelectedIndex = 0;
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
            var isForm = Array.IndexOf(SpeciesNames, str) == -1 && formLocation > 0;

            if (byte.TryParse(str[^2].ToString(), out var index) && str[^1] == ')')
            {
                result.index = index;
                str = str[..^4];
            }
            if (!isForm)
            {
                var species = Editor.Language.ToLower().Equals("en") ? str :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(SpeciesNames, str)];
                result.species = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty).Replace("-", string.Empty));
            }
            else
            {
                var species = Editor.Language.ToLower().Equals("en") ? str[..formLocation] :
                    GameInfo.GetStrings("en").specieslist[Array.IndexOf(SpeciesNames, str[..formLocation])];
                result.species = (ushort)Enum.Parse(typeof(Species), species.Replace(" ", string.Empty).Replace("-", string.Empty));
                result.form = ShowdownParsing.GetFormFromString(str.AsSpan()[(formLocation + 1)..], GameInfo.GetStrings(Editor.Language), result.species, EntityContext.Gen9);
            }
        }
        return result;
    }

    private EncounterRaidTF9[] GetCurrentEncounters() => (RaidContent)cmbContent.SelectedIndex switch
    {
        RaidContent.Standard => (TeraRaidMapParent)cmbMap.SelectedIndex switch
        {
            TeraRaidMapParent.Paldea => Editor.Paldea,
            TeraRaidMapParent.Kitakami => Editor.Kitakami,
            TeraRaidMapParent.Blueberry => Editor.Blueberry,
            _ => throw new NotImplementedException(nameof(cmbMap.SelectedIndex)),
        },
        RaidContent.Black => (TeraRaidMapParent)cmbMap.SelectedIndex switch
        {
            TeraRaidMapParent.Paldea => Editor.PaldeaBlack,
            TeraRaidMapParent.Kitakami => Editor.KitakamiBlack,
            TeraRaidMapParent.Blueberry => Editor.BlueberryBlack,
            _ => throw new NotImplementedException(nameof(cmbMap.SelectedIndex)),
        },
        RaidContent.Event => Editor.Dist,
        RaidContent.Event_Mighty => Editor.Mighty,
        _ => throw new NotImplementedException(nameof(cmbContent.SelectedIndex)),
    };

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
                            GridEntries.Add(new RewardGridEntry(el, Items, SpeciesNames, ShinyNames, Editor.Language));
                    });
                });
                GridEntries.FinalizeElements();
                dataGrid.DataSource = GridEntries;
            }
        }
        UpdateFoundLabel();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
        chkShiny.Checked = false;
        cmbSpecies.SelectedIndex = 0;
        //cmbStars.SelectedIndex = 0;
        foreach (var cb in grpItems.Controls.OfType<ComboBox>())
            cb.SelectedIndex = 0;
        foreach (var num in grpItems.Controls.OfType<NumericUpDown>())
            num.Value = 1;
    }

    private void CreateFilter()
    {
        var items = new List<Reward>();
        items.Clear();

        var anyherba = false;
        var nums = grpItems.Controls.OfType<NumericUpDown>();
        foreach (var cb in grpItems.Controls.OfType<ComboBox>())
        {
            if (cb.SelectedIndex > 0)
            {
                var numericName = $"numericUpDown{cb.Name[8..]}";
                var num = nums.Where(num => num.Name.Equals(numericName)).FirstOrDefault()!;
                if (!anyherba)
                    anyherba = cb.SelectedIndex == 1;
                items.Add(new Reward { ItemID = cb.SelectedIndex == 1 ? ushort.MaxValue - 2 : cb.SelectedIndex - 1, Amount = (int)num.Value });
            }
        }

        var encounterFilter = cmbSpecies.SelectedIndex > 0 || cmbStars.SelectedIndex > 0;

        var itemlist = new List<Reward>();
        foreach (var item in items)
        {
            var index = itemlist.FindIndex(i => i.ItemID == item.ItemID);
            if (index >= 0) itemlist[index].Amount += item.Amount;
            else itemlist.Add(new Reward { ItemID = item.ItemID, Amount = item.Amount });
        }

        var filter = new RewardFilter(encounterFilter, anyherba)
        {
            FilterRewards = [.. itemlist],
            Shiny = chkShiny.Checked ? TeraShiny.Yes : TeraShiny.Any,
            Encounter = new EncounterFilter((ushort)cmbSpecies.SelectedIndex, (byte)cmbStars.SelectedIndex),
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
        grpProfile.Enabled = false;
        cmbContent.Enabled = false;
        cmbBoost.Enabled = false;
        chkAllResults.Enabled = false;
        txtSeed.Enabled = false;
        numMaxCalc.Enabled = false;
        cmbMap.Enabled = false;
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
        cmbMap.Enabled = true;
    }

    private void UpdateFoundLabel()
    {
        if (Filter is not null && !Filter.IsFilterNull())
        {
            if (chkAllResults.Checked)
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

    private async void btnSearch_Click(object sender, EventArgs e)
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
            if (Filter is null || Filter.Encounter is null || !Filter.EncounterFilter || Filter.Encounter.Stars == 0)
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
            var boost = cmbBoost.SelectedIndex;

            try
            {
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                await StartSearch(sav, progress, content, boost, entity.index, (TeraRaidMapParent)cmbMap.SelectedIndex, Token);
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

    private async Task StartSearch(SAV9SV sav, GameProgress progress, RaidContent content, int boost, byte index, TeraRaidMapParent map, CancellationTokenSource token)
    {
        var seed = txtSeed.Text.Equals("") ? 0 : Convert.ToUInt32(txtSeed.Text, 16);
        var lang = (LanguageID)Editor.SAV.Language;

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

                var romMaxRate = sav.Version is GameVersion.VL ? EncounterTera9.GetRateTotalVL(Filter.Encounter.Stars, map) : EncounterTera9.GetRateTotalSL(Filter.Encounter.Stars, map);
                var eventProgress = EventUtil.GetEventStageFromProgress(progress);

                var initialValue = uint.Parse(txtSeed.Text);
                var maxValue = (long)initialValue + (uint)numMaxCalc.Value;

                Parallel.For(initialValue, maxValue, (seed, iterator) =>
                {
                    if (token.IsCancellationRequested)
                        iterator.Break();

                    if (EncounterRaidTF9.TryGenerateRewardDetails((uint)seed, effective_encounters, Filter, romMaxRate, sav.Version, progress, eventProgress, content, sav.ID32, group, boost, out _, out var result))
                    {
                        CalculatedList.Add(result.Value);
                        GridEntries.Add(new RewardGridEntry(result.Value, Items, SpeciesNames, ShinyNames, Editor.Language));

                        if (!chkAllResults.Checked)
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

    private void btnSaveAllTxt_Click(object sender, EventArgs e) => dataGrid.SaveAllTxt(Editor.Language);

    private void btnSaveSelectedTxt_Click(object sender, EventArgs e) => dataGrid.SaveSelectedTxt(Editor.Language);

    private void btnSendSelectedRaid_Click(object sender, EventArgs e) => dataGrid.SendSelectedRaidEditor(this, Editor.Language);

    private void btnCopySeed_Click(object sender, EventArgs e) => dataGrid.CopySeed(Editor.Language);
}
