using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class OutbreakForm : Form
{
    public SAV9SV SAV = null!;

    private TeraRaidMapParent CurrMap = TeraRaidMapParent.Paldea;
    public List<IOutbreak> MassOutbreaksMain = [];
    public List<IOutbreak> MassOutbreaksDLC1 = [];
    public List<IOutbreak> MassOutbreaksDLC2 = [];

    public string Language = null!;
    private Dictionary<string, string> Strings = null!;

    private readonly ConnectionForm? Connection;

    private readonly Image DefBackground = null!;
    private Size DefSize = new(0, 0);
    private bool Loaded = false;
    private bool Importing = false;
    private bool PrevWasEvent = false;

    private readonly string[] SpeciesList = null!;
    private readonly string[] FormsList = null!;
    private readonly string[] TypesList = null!;
    private readonly string[] GenderList = null!;
    private readonly string[] GameList = null!;
    private readonly string[] PaldeaSpeciesList = null!;
    private readonly string[] KitakamiSpeciesList = null!;
    private readonly string[] BlueberrySpeciesList = null!;

    public OutbreakForm(TeraPlugin container)
    {
        InitializeComponent();
        SAV = container.SAV;
        Language = container.Language;
        Connection = container.Connection;

        this.TranslateInterface(Language);
        GenerateDictionary();
        TranslateDictionary(Language);

        for (var i = 1; i <= 8; i++)
            MassOutbreaksMain.Add(new MassOutbreak(SAV, i, TeraRaidMapParent.Paldea));

        for (var i = 1; i <= 4; i++)
            MassOutbreaksDLC1.Add(new MassOutbreak(SAV, i, TeraRaidMapParent.Kitakami));

        for (var i = 1; i <= 5; i++)
            MassOutbreaksDLC2.Add(new MassOutbreak(SAV, i, TeraRaidMapParent.Blueberry));

        for (var i = 1; i <= 10; i++)
        {
            MassOutbreaksMain.Add(new EventMassOutbreak(SAV, i, TeraRaidMapParent.Paldea));
            MassOutbreaksDLC1.Add(new EventMassOutbreak(SAV, i, TeraRaidMapParent.Kitakami));
            MassOutbreaksDLC2.Add(new EventMassOutbreak(SAV, i, TeraRaidMapParent.Blueberry));
        }

        DefBackground = pictureBox.BackgroundImage!;
        DefSize = pictureBox.Size;
        SpeciesList = GameInfo.GetStrings(Language).specieslist;
        FormsList = GameInfo.GetStrings(Language).forms;
        TypesList = GameInfo.GetStrings(Language).types;
        GenderList = [.. GameInfo.GenderSymbolUnicode];
        GameList = GameInfo.GetStrings(Language).gamelist;

        for (var i = 0; i < 8; i++)
            cmbOutbreaks.Items[i] = $"{Strings["OutBreakForm.MassOutbreakName"]} {i + 1} - " +
                $"{SpeciesList[SpeciesConverter.GetNational9((ushort)MassOutbreaksMain[i].Species)]}";

        PaldeaSpeciesList = [..Enum.GetValues(typeof(PaldeaSpeciesOb)).Cast<ushort>().Select(p => SpeciesList[p])];
        KitakamiSpeciesList = [..Enum.GetValues(typeof(KitakamiSpeciesOb)).Cast<ushort>().Select(k => SpeciesList[k])];
        BlueberrySpeciesList = [..Enum.GetValues(typeof(BlueberrySpeciesOb)).Cast<ushort>().Select(b => SpeciesList[b])];

        cmbSpecies.Items.AddRange(PaldeaSpeciesList);
        cmbMap.Items.Add($"{Strings["Plugin.MapPaldea"]} ({Strings["Plugin.Main"]})");
        if (SAV.SaveRevision > 0)
          cmbMap.Items.Add($"{Strings["Plugin.MapKitakami"]} ({Strings["Plugin.DLC1"]})");
        if (SAV.SaveRevision > 1 && MassOutbreaksDLC2[0].Species > 0)
            cmbMap.Items.Add($"{Strings["Plugin.MapBlueberry"]} ({Strings["Plugin.DLC2"]})");

        cmbMap.SelectedIndex = 0;
        cmbMap.Enabled = cmbMap.Items.Count > 1;

        cmbSpecies.KeyDown += (s, e) =>
            { e.SuppressKeyPress = (e.KeyCode is Keys.Up or Keys.Down or Keys.Left or Keys.Right); };
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "OutBreakForm.MassOutbreakName", "Mass Outbreak" },
            { "OutBreakForm.EventOutbreakName", "Event Outbreak" },
            { "OutbreakForm.DeviceDisconnected", "Device disconnected." },
            { "OutbreakForm.ErrorParsing", "Error while parsing:" },
            { "OutbreakForm.LoadDefault", "Do you want to load default legal data for {species}?" },
            { "OutbreakForm.SpeciesExclusive", "{species} is a {game} exclusive!" },
            { "Plugin.MapPaldea", "Paldea" },
            { "Plugin.MapKitakami", "Kitakami" },
            { "Plugin.MapBlueberry", "Blueberry" },
            { "Plugin.Main", "Main" },
            { "Plugin.DLC1", "DLC1" },
            { "Plugin.DLC2", "DLC2" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void UpdateForm(FakeOutbreak outbreak)
    {
        SuspendLayout();
        Enabled = false;

        if (Importing)
            cmbSpecies.SelectedIndex = Array.IndexOf(CurrMap switch 
            { TeraRaidMapParent.Paldea => PaldeaSpeciesList, TeraRaidMapParent.Kitakami => KitakamiSpeciesList, _ => BlueberrySpeciesList }
            , SpeciesList[SpeciesConverter.GetNational9((ushort)outbreak.Species)]);

        numMaxSpawn.Value = outbreak.MaxSpawns;
        numKO.Value = 0;
        txtCenterX.Text = $"{outbreak.CenterX}";
        txtCenterY.Text = $"{outbreak.CenterY}";
        txtCenterZ.Text = $"{outbreak.CenterZ}";
        txtDummyX.Text = $"{outbreak.DummyX}";
        txtDummyY.Text = $"{outbreak.DummyY}";
        txtDummyZ.Text = $"{outbreak.DummyZ}";
        chkEnabled.Checked = true;
        chkFound.Checked = false;
        var wasImporting = Importing;
        Importing = true;
        cmbForm.SelectedIndex = outbreak.Form;
        Importing = wasImporting;
        Enabled = true;
        ResumeLayout();
    }

    private void cmbMap_IndexChanged(object sender, EventArgs e)
    {
        Loaded = false;

        CurrMap = (TeraRaidMapParent)cmbMap.SelectedIndex;
        imgMap.BackgroundImage = CurrMap switch { TeraRaidMapParent.Paldea => Properties.Resources.paldea, TeraRaidMapParent.Kitakami => Properties.Resources.kitakami, _ => Properties.Resources.blueberry };
        imgMap.ResetMap();

        var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
        cmbOutbreaks.Items.Clear();

        foreach(var outbreak in massOutbreaks)
            cmbOutbreaks.Items.Add($"{Strings[(outbreak.IsEvent ? "OutBreakForm.EventOutbreakName" : 
                "OutBreakForm.MassOutbreakName")]} {outbreak.ID} - {SpeciesList[SpeciesConverter.GetNational9((ushort)outbreak.Species)]}");

        var species = CurrMap switch { TeraRaidMapParent.Paldea => PaldeaSpeciesList, TeraRaidMapParent.Kitakami => KitakamiSpeciesList, _ => BlueberrySpeciesList };
        cmbSpecies.Items.Clear();
        cmbSpecies.Items.AddRange(species);

        cmbOutbreaks.SelectedIndex = 0;

        Loaded = true;
    }

    private void cmbOutbreaks_IndexChanged(object sender, EventArgs e)
    {
        Loaded = false;

        if (cmbOutbreaks.SelectedIndex == 0)
            btnPrev.Enabled = false;
        else
            btnPrev.Enabled = true;

        if (cmbOutbreaks.SelectedIndex == cmbOutbreaks.Items.Count - 1)
            btnNext.Enabled = false;
        else
            btnNext.Enabled = true;

        var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
        var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];

        if (outbreak.IsEvent && !PrevWasEvent)
        {
            cmbSpecies.Items.Clear();
            cmbSpecies.Items.AddRange(SpeciesList);
        }
        else if (!outbreak.IsEvent && PrevWasEvent)
        {
            var specieslist = CurrMap switch { TeraRaidMapParent.Paldea => PaldeaSpeciesList, TeraRaidMapParent.Kitakami => KitakamiSpeciesList, _ => BlueberrySpeciesList };
            cmbSpecies.Items.Clear();
            cmbSpecies.Items.AddRange(specieslist);
        }
        PrevWasEvent = outbreak.IsEvent;

        var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
        if (!SpeciesList[species].Equals(cmbSpecies.Text))
            cmbSpecies.SelectedIndex = Array.IndexOf(outbreak.IsEvent ? SpeciesList : CurrMap switch
            {
                TeraRaidMapParent.Paldea => PaldeaSpeciesList,
                TeraRaidMapParent.Kitakami => KitakamiSpeciesList,
                _ => BlueberrySpeciesList
            }, SpeciesList[SpeciesConverter.GetNational9((ushort)outbreak.Species)]);
        else
            cmbSpecies_IndexChanged(this, EventArgs.Empty);

        numMaxSpawn.Value = outbreak.MaxSpawns > 0 ? outbreak.MaxSpawns : 1;
        numKO.Value = outbreak.NumKO;

        chkEnabled.Checked = outbreak.Enabled;
        chkFound.Checked = outbreak.Found;

        txtCenterX.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.X : 0)}";
        txtCenterY.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.Y : 0)}";
        txtCenterZ.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.Z : 0)}";

        txtDummyX.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.X : 0)}";
        txtDummyY.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.Y : 0)}";
        txtDummyZ.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.Z : 0)}";

        if (outbreak.LocationCenter is not null)
            imgMap.SetMapPoint(CurrMap, outbreak.LocationCenter, (MoveType)PersonalTable.SV.GetFormEntry((ushort)outbreak.Species, outbreak.Form).Type1);
        else
            imgMap.ResetMap();

        Loaded = true;
    }

    private void cmbSpecies_IndexChanged(object sender, EventArgs e)
    {
        cmbForm.Items.Clear();
        var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
        var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
        var toExpect = outbreak.Species;
        var species = (ushort)Array.IndexOf(SpeciesList, cmbSpecies.Text);
        var formlist = FormConverter.GetFormList(species, TypesList, FormsList, GenderList, EntityContext.Gen9);
        if (formlist.Length == 0 || (formlist.Length == 1 && formlist[0].Equals("")))
            cmbForm.Items.Add("---");
        else
            cmbForm.Items.AddRange(formlist);

        if (Loaded)
        {
            var restore = false;
            var json = "";

            if (!Importing)
            {
                var versionType = SAV.Version is GameVersion.SL ? typeof(VioletExclusives) : typeof(ScarletExclusives);
                var isExclusive = Enum.IsDefined(versionType, species);

                if (!isExclusive)
                {
                    var resourceName = $"{CurrMap switch { TeraRaidMapParent.Kitakami => "dlc1", TeraRaidMapParent.Blueberry => "dlc2", _ => "" }}_{species}";
                    json = ResourcesUtil.GetTextResource(resourceName);
                    if (!outbreak.IsEvent && json is not null && json.Length > 0)
                    {
                        var message = Strings["OutbreakForm.LoadDefault"].Replace("{species}", SpeciesList[species]);
                        var dialog = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                        if (dialog is DialogResult.Yes)
                            restore = true;
                    }
                    else json = "";
                }
                else
                {
                    var message = Strings["OutbreakForm.SpeciesExclusive"]
                        .Replace("{species}", $"{SpeciesList[species]}")
                        .Replace("{game}", $"{(versionType == typeof(ScarletExclusives) ?
                        GameList[(byte)GameVersion.SL] : GameList[(byte)GameVersion.VL])}");

                    MessageBox.Show(message);
                }
            }

            if (restore)
            {
                var clone = outbreak.Clone();
                clone.RestoreFromJson(json!);
                UpdateForm(clone);
                outbreak.Species = SpeciesConverter.GetInternal9(species);
            }
            else
            {
                var wasImporting = Importing;
                Importing = true;
                outbreak.Species = SpeciesConverter.GetInternal9(species);
                cmbForm.SelectedIndex = 0;
                Importing = wasImporting;
            }

            var index = cmbOutbreaks.SelectedIndex;
            cmbOutbreaks.Items[index] = $"{Strings[(outbreak.IsEvent ? "OutBreakForm.EventOutbreakName" :
                "OutBreakForm.MassOutbreakName")]} {outbreak.ID} - {SpeciesList[species]}";

            if (Connection is not null && Connection.IsConnected())
            {
                var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                var success = false;
                var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                if(BC == "BC")
                {
                    selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                }
                var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}Species")!.GetValue(new BlockDefinition())!;
                Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Species, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
        else
        {
            cmbForm.SelectedIndex = outbreak.Form;
        }


        pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
        if (pictureBox.Image is not null)
        {
            pictureBox.BackgroundImage = null;
            pictureBox.Size = pictureBox.Image.Size;
        }
        else
        {
            pictureBox.BackgroundImage = DefBackground;
            pictureBox.Size = DefSize;
        }
    }

    private void cmbForm_IndexChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = outbreak.Form;
            var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);

            var resourceName = $"{CurrMap switch { TeraRaidMapParent.Kitakami => "dlc1", TeraRaidMapParent.Blueberry => "dlc2", _ => "" }}_{species}_{cmbForm.SelectedIndex}";
            var json = ResourcesUtil.GetTextResource(resourceName);
            if (!outbreak.IsEvent && !Importing && json is not null)
            {
                if (json is not null && json.Length > 0)
                {
                    var formlist = FormConverter.GetFormList(species, TypesList, FormsList, GenderList, EntityContext.Gen9);
                    var hasForm = false;
                    if (!(formlist.Length == 0 || (formlist.Length == 1 && formlist[0].Equals(""))))
                        hasForm = true;
                    var name = $"{SpeciesList[species]}{(hasForm ? $"-{formlist[cmbForm.SelectedIndex]}" : "")}";

                    var message = Strings["OutbreakForm.LoadDefault"].Replace("{species}", name);
                    var dialog = MessageBox.Show(message, "", MessageBoxButtons.YesNo);
                    if (dialog is DialogResult.Yes)
                    {
                        var clone = outbreak.Clone();
                        clone.RestoreFromJson(json!);
                        UpdateForm(clone);
                    }
                }
            }

            outbreak.Form = (byte)cmbForm.SelectedIndex;
            pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
            if (pictureBox.Image is not null)
            {
                pictureBox.BackgroundImage = null;
                pictureBox.Size = pictureBox.Image.Size;
            }
            else
            {
                pictureBox.BackgroundImage = DefBackground;
                pictureBox.Size = DefSize;
            }

            if (Connection is not null && Connection.IsConnected())
            {
                var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                var success = false;
                var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                if (BC == "BC")
                {
                    selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                }
                var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}Form")!.GetValue(new BlockDefinition())!;
                Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Form, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
    }

    private void numMaxSpawn_ValueChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = outbreak.MaxSpawns;
            outbreak.MaxSpawns = (int)numMaxSpawn.Value;

            if (Connection is not null && Connection.IsConnected())
            {
                var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                var success = false;
                var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                if (BC == "BC")
                {
                    selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                }
                var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}TotalSpawns")!.GetValue(new BlockDefinition())!;
                Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.MaxSpawns, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
    }

    private void numKO_ValueChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = outbreak.NumKO;
            outbreak.NumKO = (int)numKO.Value;

            if (Connection is not null && Connection.IsConnected())
            {
                var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                var success = false;
                var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                if (BC == "BC")
                {
                    selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                }
                var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}NumKOed")!.GetValue(new BlockDefinition())!;
                Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.NumKO, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
    }

    private void chkFound_CheckedChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = outbreak.Found;

            if (chkFound.Checked)
                outbreak.Found = true;
            else
                outbreak.Found = false;

            if (Connection is not null && Connection.IsConnected())
            {
                var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                var success = false;
                var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                if (BC == "BC")
                {
                    selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                }
                var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}Found")!.GetValue(new BlockDefinition())!;
                Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Found, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
    }

    private void chkEnabled_CheckChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = (byte)outbreak.AmountAvailable;

            if (chkEnabled.Checked)
                outbreak.Enabled = true;
            else
                outbreak.Enabled = false;

            var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
            pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
            if (pictureBox.Image is not null)
            {
                pictureBox.BackgroundImage = null;
                pictureBox.Size = pictureBox.Image.Size;
            }
            else
            {
                pictureBox.BackgroundImage = DefBackground;
                pictureBox.Size = DefSize;
            }

            if (Connection is not null && Connection.IsConnected())
            {
                var success = false;
                var value = (byte)outbreak.AmountAvailable;
                var blockInfo = (CurrMap,outbreak) switch
                {
                    (TeraRaidMapParent.Kitakami,MassOutbreak) => BlockDefinitions.KOutbreakDLC1NumActive,
                    (TeraRaidMapParent.Blueberry,MassOutbreak) => BlockDefinitions.KOutbreakDLC2NumActive,
                    (_,MassOutbreak) => BlockDefinitions.KOutbreakMainNumActive,
                    (TeraRaidMapParent.Kitakami,EventMassOutbreak) => BlockDefinitions.KOutbreakBCDLC1NumActive,
                    (TeraRaidMapParent.Blueberry,EventMassOutbreak) => BlockDefinitions.KOutbreakBCDLC2NumActive,
                    (_,EventMassOutbreak) => BlockDefinitions.KOutbreakBCMainNumActive,
                    (_,_)=>BlockDefinitions.KOutbreakMainNumActive
                };

                Task.Run(async () => { success = await Connection.Executor.WriteBlock(value, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                if (!success)
                {
                    Connection.Disconnect();
                    MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                }
            }
        }
    }

    private void txtCenterX_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationCenter is not null)
            {
                var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationCenter.X = Convert.ToSingle(txtCenterX.Text);
                    imgMap.SetMapPoint(CurrMap, outbreak.LocationCenter);

                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                        {
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        }
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}CenterPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception)
                {
                    imgMap.ResetMap();
                }
            }
        }
    }

    private void txtCenterY_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationCenter is not null)
            {
                var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationCenter.Y = Convert.ToSingle(txtCenterY.Text);
                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                        {
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        }
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}CenterPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    private void txtCenterZ_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationCenter is not null)
            {
                var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationCenter.Z = Convert.ToSingle(txtCenterZ.Text);
                    imgMap.SetMapPoint(CurrMap, outbreak.LocationCenter);

                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                        {
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        }
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}CenterPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception)
                {
                    imgMap.ResetMap();
                }
            }
        }
    }

    private void txtDummyX_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationDummy is not null)
            {
                var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationDummy.X = Convert.ToSingle(txtDummyX.Text);
                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}DummyPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    private void txtDummyY_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationDummy is not null)
            {
                var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationDummy.Y = Convert.ToSingle(txtDummyY.Text);
                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}DummyPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    private void txtDummyZ_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            if (outbreak.LocationDummy is not null)
            {
                var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                try
                {
                    outbreak.LocationDummy.Z = Convert.ToSingle(txtDummyZ.Text);
                    if (Connection is not null && Connection.IsConnected())
                    {
                        var selectedOutbreak = cmbOutbreaks.SelectedIndex + 1;
                        var success = false;
                        var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                        var locationMap = CurrMap switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };
                        var BC = CurrMap == TeraRaidMapParent.Paldea && cmbOutbreaks.SelectedIndex > 8 ? "BC" : CurrMap == TeraRaidMapParent.Kitakami && cmbOutbreaks.SelectedIndex > 4 ? "BC" : CurrMap == TeraRaidMapParent.Blueberry && cmbOutbreaks.SelectedIndex > 5 ? "BC" : "";
                        if (BC == "BC")
                            selectedOutbreak = CurrMap switch { TeraRaidMapParent.Kitakami => selectedOutbreak - 4, TeraRaidMapParent.Blueberry => selectedOutbreak - 5, _ => selectedOutbreak - 8 };
                        var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak{BC}{selectedOutbreak:00}{locationMap}DummyPos")!.GetValue(new BlockDefinition())!;
                        Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                        if (!success)
                        {
                            Connection.Disconnect();
                            MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        btnPrev.Enabled = true;
        cmbOutbreaks.SelectedIndex++;
        cmbOutbreaks.Focus();
    }

    private void btnPrev_Click(object sender, EventArgs e)
    {
        btnNext.Enabled = true;
        cmbOutbreaks.SelectedIndex--;
        cmbOutbreaks.Focus();
    }

    private void dumpToJsonToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex];
            var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
            var formlist = FormConverter.GetFormList(species, TypesList, FormsList, GenderList, EntityContext.Gen9);
            var hasForm = false;
            if (!(formlist.Length == 0 || (formlist.Length == 1 && formlist[0].Equals(""))))
                hasForm = true;
            saveFileDialog.FileName = $"{species}{(hasForm ? $"-{outbreak.Form}" : "")}";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                outbreak.DumpTojson(saveFileDialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings["OutbreakForm.ErrorParsing"]}\n{ex.Message}");
        }
    }

    private void injectFromJsonToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Importing = true;
        try
        {
            var massOutbreaks = CurrMap switch { TeraRaidMapParent.Paldea => MassOutbreaksMain, TeraRaidMapParent.Kitakami => MassOutbreaksDLC1, _ => MassOutbreaksDLC2 };
            var outbreak = massOutbreaks[cmbOutbreaks.SelectedIndex].Clone();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var json = File.ReadAllText(openFileDialog.FileName);
                outbreak.RestoreFromJson(json);
                UpdateForm(outbreak);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings["OutbreakForm.ErrorParsing"]}\n{ex.Message}");
        }
        Importing = false;
    }
}