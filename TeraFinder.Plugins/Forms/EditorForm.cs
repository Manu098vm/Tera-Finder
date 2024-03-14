using System.Media;
using System.Text.Json;
using PKHeX.Core;
using TeraFinder.Core;
using static TeraFinder.Plugins.ImagesUtil;

namespace TeraFinder.Plugins;

public partial class EditorForm : Form
{
    public SAV9SV SAV { get; set; } = null!;
    public IPKMView? PKMEditor { get; private set; } = null!;
    private Dictionary<string, float[]> DenLocations = null!;
    private readonly Image DefBackground = null!;
    private Size DefSize = new(0, 0);
    private bool Loaded = false;
    private Dictionary<string, string> Strings = null!;
    private string[] PaldeaLocations = null!;
    private string[] KitakamiLocations = null!;
    private string[] BlueberryLocations = null!;

    public string Language = null!;
    private ConnectionForm? Connection = null;
    public GameProgress Progress { get; set; } = GameProgress.Beginning;
    public TeraRaidMapParent CurrMap = TeraRaidMapParent.Paldea;

    public EncounterTeraTF9[] Paldea = null!;
    public EncounterTeraTF9[] PaldeaBlack = null!;
    public EncounterTeraTF9[] Kitakami = null!;
    public EncounterTeraTF9[] KitakamiBlack = null!;
    public EncounterTeraTF9[] Blueberry = null!;
    public EncounterTeraTF9[] BlueberryBlack = null!;

    public EncounterEventTF9[] Dist = null!;
    public EncounterEventTF9[] Mighty = null!;

    public EncounterRaidTF9? CurrEncount = null;
    public TeraDetails? CurrTera = null;

    public EditorForm(SAV9SV sav,
        IPKMView? editor,
        string language,
        EncounterTeraTF9[]? paldea,
        EncounterTeraTF9[]? paldeablack,
        EncounterTeraTF9[]? kitakami,
        EncounterTeraTF9[]? kitakamiblack,
        EncounterTeraTF9[]? blueberry,
        EncounterTeraTF9[]? blueberryblack,
        EncounterEventTF9[]? dist,
        EncounterEventTF9[]? mighty,
        ConnectionForm? connection)
    {
        InitializeComponent();
        SAV = sav;
        PKMEditor = editor;
        Language = language;

        GenerateDictionary();
        TranslateDictionary(Language);
        TranslateCmbContent();
        this.TranslateInterface(Language);
        InitLocationNames();
        GenerateDenLocations();

        (Paldea, PaldeaBlack) = paldea is null || paldeablack is null ? ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea) : (paldea, paldeablack);
        (Kitakami, KitakamiBlack) = kitakami is null || kitakamiblack is null ? ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami) : (kitakami, kitakamiblack);
        (Blueberry, BlueberryBlack) = blueberry is null || blueberryblack is null ? ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry) : (blueberry, blueberryblack);
        (Dist, Mighty) = dist is null || mighty is null ? EventUtil.GetCurrentEventEncounters(SAV, RewardUtil.GetDistRewardsTables(SAV)) : (dist, mighty);

        DefBackground = pictureBox.BackgroundImage!;
        DefSize = pictureBox.Size;
        Progress = SavUtil.GetProgress(SAV);
        foreach (var name in UpdateRaidNameList())
            cmbDens.Items.Add(name);
        cmbMap.Items.Add($"{Strings["Plugin.MapPaldea"]} ({Strings["Plugin.Main"]})");
        if (SAV.SaveRevision > 0)
          cmbMap.Items.Add($"{Strings["Plugin.MapKitakami"]} ({Strings["Plugin.DLC1"]})");
        if (SAV.SaveRevision > 1)
            cmbMap.Items.Add($"{Strings["Plugin.MapBlueberry"]} ({Strings["Plugin.DLC2"]})");
        cmbMap.SelectedIndex = 0;
        cmbMap.Enabled = cmbMap.Items.Count > 1;
        cmbDens.SelectedIndex = 0;
        btnSx.Enabled = false;
        Connection = connection;
    }

    private void GenerateDenLocations() =>
        DenLocations = JsonSerializer.Deserialize<Dictionary<string, float[]>>(ResourcesUtil.GetDenLocations(CurrMap))!;

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "DisconnectionSuccess", "Device disconnected." },
            { "EditorForm.lblSpecies", "Species:" },
            { "EditorForm.lblTera", "TeraType:" },
            { "EditorForm.lblAbility", "Ability:" },
            { "EditorForm.lblNature", "Nature:" },
            { "EditorForm.lblShiny", "Shiny:" },
            { "EditorForm.lblGender", "Gender:" },
            { "EditorForm.lblIndex", "Event Index:" },
            { "EditorForm.txtMove1", "None" },
            { "EditorForm.txtMove2", "None" },
            { "EditorForm.txtMove3", "None" },
            { "EditorForm.txtMove4", "None" },
            { "TeraShiny.No", "No" },
            { "TeraShiny.Yes", "Yes" },
            { "TeraShiny.Star", "Star" },
            { "TeraShiny.Square", "Square" },
            { "EditorForm.LblLvl", "Lvl." },
            { "EditorForm.CmbRaid", "Raid" },
            { "RaidContent.Standard", "Standard" },
            { "RaidContent.Black", "Black" },
            { "RaidContent.Event", "Event" },
            { "RaidContent.Event_Mighty", "Event-Mighty" },
            { "AREASPA1", "South Province (Area 1)" },
            { "AREASPA2", "South Province (Area 2)" },
            { "AREASPA4", "South Province (Area 4)" },
            { "AREASPA6", "South Province (Area 6)" },
            { "AREASPA5", "South Province (Area 5)" },
            { "AREASPA3", "South Province (Area 3)" },
            { "AREAWPA1", "West Province (Area 1)" },
            { "AREAASAD", "Asado Desert" },
            { "AREAWPA2", "West Province (Area 2)" },
            { "AREAWPA3", "West Province (Area 3)" },
            { "AREATAGT", "Tagtree Thicket" },
            { "AREAEPA3", "East Province (Area 3)" },
            { "AREAEPA1", "East Province (Area 1)" },
            { "AREAEPA2", "East Province (Area 2)" },
            { "AREADALI", "Dalizapa Passage" },
            { "AREACASS", "Casseroya Lake" },
            { "AREAGLAS", "Glaseado Mountain" },
            { "AREANPA3", "North Province (Area 3)" },
            { "AREANPA1", "North Province (Area 1)" },
            { "AREANPA2", "North Province (Area 2)" },
            { "AREA1KR", "Kitakami Road" },
            { "AREA1AH", "Apple Hills" },
            { "AREA1RR", "Revelers Road" },
            { "AREA1OM", "Oni Mountain" },
            { "AREA1IP", "Infernal Pass" },
            { "AREA1CP", "Crystal Pool" },
            { "AREA1WF", "Wistful Fields" },
            { "AREA1MC", "Mossfell Confluence" },
            { "AREA1FG", "Fellhorn Gorge" },
            { "AREA1PB", "Paradise Barrens" },
            { "AREA1KW", "Kitakami Wilds" },
            { "AREA2SV", "Savannna Biome" },
            { "AREA2CO", "Coastal Biome" },
            { "AREA2CA", "Canyon Biome" },
            { "AREA2PO", "Polar Biome" },
            { "ShinifyForm.lblValue", "Progress:" },
            { "ShinifiedAll", "All raids have been Shinified." },
            { "Plugin.MapPaldea", "Paldea" },
            { "Plugin.MapKitakami", "Kitakami" },
            { "Plugin.MapBlueberry", "Blueberry" },
            { "Plugin.Main", "Main" },
            { "Plugin.DLC1", "DLC1" },
            { "Plugin.DLC2", "DLC2" },
            { "EditorForm.toolTipMoves", "Extra Raid Moves:" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void TranslateCmbContent()
    {
        cmbContent.Items[0] = Strings["RaidContent.Standard"];
        cmbContent.Items[1] = Strings["RaidContent.Black"];
        cmbContent.Items[2] = Strings["RaidContent.Event"];
        cmbContent.Items[3] = Strings["RaidContent.Event_Mighty"];
    }

    private void InitLocationNames()
    {
        PaldeaLocations = AreaNames.AreaPaldea;
        PaldeaLocations[1] = Strings["AREASPA1"];
        PaldeaLocations[4] = Strings["AREASPA2"];
        PaldeaLocations[5] = Strings["AREASPA4"];
        PaldeaLocations[6] = Strings["AREASPA6"];
        PaldeaLocations[7] = Strings["AREASPA5"];
        PaldeaLocations[8] = Strings["AREASPA3"];
        PaldeaLocations[9] = Strings["AREAWPA1"];
        PaldeaLocations[10] = Strings["AREAASAD"];
        PaldeaLocations[11] = Strings["AREAWPA2"];
        PaldeaLocations[12] = Strings["AREAWPA3"];
        PaldeaLocations[13] = Strings["AREATAGT"];
        PaldeaLocations[14] = Strings["AREAEPA3"];
        PaldeaLocations[15] = Strings["AREAEPA1"];
        PaldeaLocations[16] = Strings["AREAEPA2"];
        PaldeaLocations[17] = Strings["AREADALI"];
        PaldeaLocations[18] = Strings["AREACASS"];
        PaldeaLocations[19] = Strings["AREAGLAS"];
        PaldeaLocations[20] = Strings["AREANPA3"];
        PaldeaLocations[21] = Strings["AREANPA1"];
        PaldeaLocations[22] = Strings["AREANPA2"];

        KitakamiLocations = AreaNames.AreaKitakami;
        KitakamiLocations[1] = Strings["AREA1KR"];
        KitakamiLocations[2] = Strings["AREA1AH"];
        KitakamiLocations[3] = Strings["AREA1RR"];
        KitakamiLocations[4] = Strings["AREA1OM"];
        KitakamiLocations[5] = Strings["AREA1IP"];
        KitakamiLocations[6] = Strings["AREA1CP"];
        KitakamiLocations[7] = Strings["AREA1WF"];
        KitakamiLocations[8] = Strings["AREA1MC"];
        KitakamiLocations[9] = Strings["AREA1FG"];
        KitakamiLocations[10] = Strings["AREA1PB"];
        KitakamiLocations[11] = Strings["AREA1KW"];

        BlueberryLocations = AreaNames.AreaBlueberry;
        BlueberryLocations[1] = Strings["AREA2SV"];
        BlueberryLocations[2] = Strings["AREA2CO"];
        BlueberryLocations[3] = Strings["AREA2CA"];
        BlueberryLocations[4] = Strings["AREA2PO"];
        BlueberryLocations[5] = Strings["AREA2SV"];
        BlueberryLocations[6] = Strings["AREA2CO"];
        BlueberryLocations[7] = Strings["AREA2CA"];
        BlueberryLocations[8] = Strings["AREA2PO"];
    }

    private void cmbMap_IndexChanged(object sender, EventArgs e)
    {
        Loaded = false;

        CurrMap = (TeraRaidMapParent)cmbMap.SelectedIndex;
        GenerateDenLocations();
        imgMap.BackgroundImage = CurrMap switch 
            { 
                TeraRaidMapParent.Paldea => Properties.Resources.paldea, 
                TeraRaidMapParent.Kitakami => Properties.Resources.kitakami, 
                _ => Properties.Resources.blueberry 
            };
        imgMap.ResetMap();

        cmbDens.Items.Clear();
        foreach (var name in UpdateRaidNameList())
            cmbDens.Items.Add(name);

        if (cmbDens.Items.Count == 0)
            cmbDens.Items.Add($"");

        cmbDens.SelectedIndex = 0;

        Loaded = true;
    }

    private void cmbDens_IndexChanged(object sender, EventArgs e)
    {
        Loaded = false;
        if (cmbDens.SelectedIndex == 0)
            btnSx.Enabled = false;
        else
            btnSx.Enabled = true;

        if (cmbDens.SelectedIndex == cmbDens.Items.Count - 1)
            btnDx.Enabled = false;
        else
            btnDx.Enabled = true;

        var spawnList = CurrMap switch { TeraRaidMapParent.Paldea => SAV.RaidPaldea, TeraRaidMapParent.Kitakami => SAV.RaidKitakami, _ => SAV.RaidBlueberry };
        var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);
        chkLP.Checked = raid.IsClaimedLeaguePoints;
        chkActive.Checked = raid.IsEnabled;

        if (raid.IsEnabled)
        {
            if (raid.Content is TeraRaidContentType.Base05)
                cmbContent.SelectedIndex = 0;
            else if (raid.Content is TeraRaidContentType.Black6)
                cmbContent.SelectedIndex = 1;
            else if (raid.Content is TeraRaidContentType.Distribution)
                cmbContent.SelectedIndex = 2;
            else if (raid.Content is TeraRaidContentType.Might7)
                cmbContent.SelectedIndex = 3;
        }
        else cmbContent.SelectedIndex = 0;

        txtSeed.Text = $"{raid.Seed:X8}";
        UpdatePKMInfo(raid);
        Loaded = true;
    }

    private void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var spawnList = CurrMap switch { TeraRaidMapParent.Paldea => SAV.RaidPaldea, TeraRaidMapParent.Kitakami => SAV.RaidKitakami, _ => SAV.RaidBlueberry };
            var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);
            if (chkActive.Checked)
            {
                raid.IsEnabled = true;
                Task.Run(UpdateRemote).Wait();
            }
            else
            {
                raid.IsEnabled = false;
                Task.Run(UpdateRemote).Wait();
            }
            UpdatePKMInfo(raid);
        }
    }

    private void chkLP_CheckedChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var spawnList = CurrMap switch
            {
                TeraRaidMapParent.Paldea => SAV.RaidPaldea,
                TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
                _ => SAV.RaidBlueberry
            };
            var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);
            if (chkLP.Checked)
            {
                raid.IsClaimedLeaguePoints = true;
                Task.Run(UpdateRemote).Wait();
            }
            else
            {
                raid.IsClaimedLeaguePoints = false;
                Task.Run(UpdateRemote).Wait();
            }
        }
    }

    private void cmbContent_IndexChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            var spawnList = CurrMap switch
            {
                TeraRaidMapParent.Paldea => SAV.RaidPaldea,
                TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
                _ => SAV.RaidBlueberry
            };
            var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);
            raid.Content = (TeraRaidContentType)cmbContent.SelectedIndex;
            Task.Run(UpdateRemote).Wait();
            UpdatePKMInfo(raid);
        }
    }

    private void txtSeed_KeyPress(object sender, KeyPressEventArgs e)
    {
        var c = e.KeyChar;
        if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
            e.Handled = true;
    }

    private void txtSeed_TextChanged(object sender, EventArgs e)
    {
        if (Loaded)
        {
            if (!txtSeed.Text.Equals(""))
            {
                var spawnList = CurrMap switch
                {
                    TeraRaidMapParent.Paldea => SAV.RaidPaldea,
                    TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
                    _ => SAV.RaidBlueberry
                };
                var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);
                try
                {
                    var seed = Convert.ToUInt32(txtSeed.Text, 16);
                    raid.Seed = seed;
                }
                catch { }
                Task.Run(UpdateRemote).Wait();
                UpdatePKMInfo(raid);
            }
        }
    }

    private async Task UpdateRemote()
    {
        try
        {
            if (Connection is not null && Connection.IsConnected())
            {
                var block = CurrMap is TeraRaidMapParent.Paldea ? BlockDefinitions.KTeraRaidPaldea : BlockDefinitions.KTeraRaidDLC;
                var savBlock = SAV.Accessor.FindOrDefault(block.Key)!;
                await Connection.Executor.WriteBlock(savBlock.Data, block, new CancellationToken()).ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            if (Connection is not null)
            {
                Connection.Disconnect();
                MessageBox.Show(Strings["DisconnectionSuccess"]);
                Connection = null;
            }
        }
    }

    private void UpdatePKMInfo(TeraRaidDetail raid)
    {
        if (Progress is not GameProgress.Beginning && ((raid.Seed == 0 && raid.IsEnabled) || raid.Seed > 0))
        {
            var content = (RaidContent)cmbContent.SelectedIndex;
            var groupid = content switch
            {
                RaidContent.Event or RaidContent.Event_Mighty => EventUtil.GetDeliveryGroupID(content switch
                {
                    RaidContent.Event => Dist,
                    RaidContent.Event_Mighty => Mighty,
                    _ => throw new NotImplementedException(nameof(content)),
                }, SAV, EventUtil.GetEventStageFromProgress(Progress), CurrMap switch
                {
                    TeraRaidMapParent.Paldea => SAV.RaidPaldea,
                    TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
                    TeraRaidMapParent.Blueberry => SAV.RaidBlueberry,
                    _ => throw new NotImplementedException(nameof(CurrMap))
                }, cmbDens.SelectedIndex),
                _ => (byte)0,
            };

            var success = EncounterRaidTF9.TryGenerateTeraDetails(raid.Seed, content switch
            {
                RaidContent.Standard => CurrMap switch { TeraRaidMapParent.Paldea => Paldea, TeraRaidMapParent.Kitakami => Kitakami, _ => Blueberry },
                RaidContent.Black => CurrMap switch { TeraRaidMapParent.Paldea => PaldeaBlack, TeraRaidMapParent.Kitakami => KitakamiBlack, _ => BlueberryBlack },
                RaidContent.Event => Dist,
                RaidContent.Event_Mighty => Mighty,
                _ => throw new NotImplementedException(nameof(content)),
            }, SAV.Version, Progress, EventUtil.GetEventStageFromProgress(Progress), content, CurrMap, SAV.ID32, groupid, out var encounter, out var result);
                
            if (success && encounter is not null && result is not null)
            {
                var species = GameInfo.GetStrings(Language).specieslist;
                var types = GameInfo.GetStrings(Language).types;
                var forms = GameInfo.GetStrings(Language).forms;
                var natures = GameInfo.GetStrings(Language).natures;
                var abilities = GameInfo.GetStrings(Language).abilitylist;
                var genders = GameInfo.GenderSymbolUnicode.ToArray();
                var moves = GameInfo.GetStrings(Language).movelist;

                lblSpecies.Text = $"{Strings["EditorForm.lblSpecies"]} {result.Value.GetName(species, types, forms, genders)}";
                lblTera.Text = $"{Strings["EditorForm.lblTera"]} {types[result.Value.TeraType]}";
                lblNature.Text = $"{Strings["EditorForm.lblNature"]} {natures[(byte)result.Value.Nature]}";
                lblAbility.Text = $"{Strings["EditorForm.lblAbility"]} {abilities[result.Value.Ability]}";
                lblShiny.Text = $"{Strings["EditorForm.lblShiny"]} {result.Value.Shiny}";
                lblGender.Text = $"{Strings["EditorForm.lblGender"]} {genders[(int)result.Value.Gender]}";
                lblIndex.Text = $"{Strings["EditorForm.lblIndex"]} {groupid}";
                lblIndex.Visible = groupid != 0;
                txtHP.Text = $"{result.Value.HP}";
                txtAtk.Text = $"{result.Value.ATK}";
                txtDef.Text = $"{result.Value.DEF}";
                txtSpA.Text = $"{result.Value.SPA}";
                txtSpD.Text = $"{result.Value.SPD}";
                txtSpe.Text = $"{result.Value.SPE}";
                txtScale.Text = $"{result.Value.Scale}";
                txtMove1.Text = $"{moves[result.Value.Move1]}";
                txtMove2.Text = $"{moves[result.Value.Move2]}";
                txtMove3.Text = $"{moves[result.Value.Move3]}";
                txtMove4.Text = $"{moves[result.Value.Move4]}";

                pictureBox.BackgroundImage = null;
                pictureBox.Image = GetRaidResultSprite(result.Value, raid.IsEnabled, encounter.HeldItem);
                if (pictureBox.Image is not null)
                    pictureBox.Size = pictureBox.Image.Size;
                else
                {
                    pictureBox.BackgroundImage = DefBackground;
                    pictureBox.Size = DefSize;
                }

                imgMap.SetMapPoint((MoveType)result.Value.TeraType, (int)raid.AreaID, (int)raid.LotteryGroup, (int)raid.SpawnPointID, CurrMap, DenLocations);

                btnRewards.Width = pictureBox.Image is not null ? pictureBox.Image.Width : pictureBox.BackgroundImage!.Width;
                btnRewards.Visible = true;

                CurrEncount = encounter;
                CurrTera = result;

                SetStarSymbols(result.Value.Stars);
                SetLevelLabel(result.Value.Level);

                var movestring = $"{Strings["EditorForm.toolTipMoves"]}";
                if (encounter.ExtraMoves.ExtraMoveList.Count > 0)
                    foreach (var extramove in encounter.ExtraMoves.ExtraMoveList)
                        movestring += $"{Environment.NewLine}- {moves[(ushort)extramove]}";
                else
                    movestring += $"{Environment.NewLine}- {moves[0]}";
                toolTipMoves.SetToolTip(grpMoves, movestring);

                return;
            }
        }
        lblSpecies.Text = $"{Strings["EditorForm.lblSpecies"]}";
        lblTera.Text = $"{Strings["EditorForm.lblTera"]}";
        lblNature.Text = $"{Strings["EditorForm.lblNature"]}";
        lblAbility.Text = $"{Strings["EditorForm.lblAbility"]}";
        lblShiny.Text = $"{Strings["EditorForm.lblShiny"]}";
        lblGender.Text = $"{Strings["EditorForm.lblGender"]}";
        lblIndex.Text = $"{Strings["EditorForm.lblIndex"]} {Strings["EditorForm.txtMove1"]}";
        lblIndex.Visible = true;
        txtHP.Text = $"";
        txtAtk.Text = $"";
        txtDef.Text = $"";
        txtSpA.Text = $"";
        txtSpD.Text = $"";
        txtSpe.Text = $"";
        txtScale.Text = $"";
        txtMove1.Text = $"{Strings["EditorForm.txtMove1"]}";
        txtMove2.Text = $"{Strings["EditorForm.txtMove2"]}";
        txtMove3.Text = $"{Strings["EditorForm.txtMove3"]}";
        txtMove4.Text = $"{Strings["EditorForm.txtMove4"]}";

        pictureBox.BackgroundImage = DefBackground;
        pictureBox.Size = DefSize;
        pictureBox.Image = null;
        imgMap.ResetMap();

        btnRewards.Visible = false;
        CurrEncount = null;
        CurrTera = null;
        SetStarSymbols();
        SetLevelLabel();

        var extramoves = $"{Strings["EditorForm.toolTipMoves"]}{Environment.NewLine}- {GameInfo.GetStrings(Language).movelist[0]}";
        toolTipMoves.SetToolTip(grpMoves, extramoves);
    }

    private void SetStarSymbols(int stars = 0)
    {
        var str = "";
        for (var i = 0; i < stars; i++)
            str += "☆";

        var img = pictureBox.Image ?? pictureBox.BackgroundImage!;
        lblStarSymbols.Text = str;
        lblStarSymbols.Location = new(pictureBox.Location.X + (pictureBox.Width - lblStarSymbols.Size.Width) / 2, pictureBox.Location.Y + img.Height);
    }

    private void SetLevelLabel(int level = 0)
    {
        var str = level == 0 ? "" : $"{Strings["EditorForm.LblLvl"]} {level}";
        lblLevel.Text = str;
        lblLevel.Location = new(pictureBox.Location.X + (pictureBox.Width - lblLevel.Size.Width) / 2, pictureBox.Location.Y - lblLevel.Height);
    }

    private string[] UpdateRaidNameList()
    {
        var names = new string[CurrMap switch { TeraRaidMapParent.Paldea => 69, TeraRaidMapParent.Kitakami => 26, _ => 24 }];
        var spawnList = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };
        var raids = spawnList.GetAllRaids();
        if (raids is not null && raids.Length >= names.Length)
            for (var i = 0; i < names.Length; i++)
                names[i] = $"{Strings["EditorForm.CmbRaid"]} {i + 1} - {CurrMap switch { TeraRaidMapParent.Paldea => PaldeaLocations[raids[i].AreaID],
                    TeraRaidMapParent.Kitakami => KitakamiLocations[raids[i].AreaID], _ => BlueberryLocations[raids[i].AreaID] }} [{raids[i].SpawnPointID}]";
        else
            for (var i = 0; i < names.Length; i++)
                names[i] = $"{Strings["EditorForm.CmbRaid"]} {i + 1} - [---]";

        return names;
    }

    private void btnOpenCalculator_Click(object sender, EventArgs e) => new CalculatorForm(this).Show();
    private void btnOpenRewardCalculator_Click(object sender, EventArgs e) => new RewardCalcForm(this).Show();

    private void btnDx_Click(object sender, EventArgs e)
    {
        cmbDens.SelectedIndex++;
        cmbDens.Focus();
    }

    private void btnSx_Click(object sender, EventArgs e)
    {
        cmbDens.SelectedIndex--;
        cmbDens.Focus();
    }

    private void cmbDens_KeyPress(object sender, KeyPressEventArgs e)
    {
        e.Handled = true;
    }

    private void btnRewards_Click(object sender, EventArgs e)
    {
        if (CurrEncount is not null && CurrTera is not null)
        {
            var lvl0 = RewardUtil.GetCombinedRewardList(CurrTera.Value, CurrEncount.FixedRewards, CurrEncount.LotteryRewards, 0);
            var lvl1 = RewardUtil.GetCombinedRewardList(CurrTera.Value, CurrEncount.FixedRewards, CurrEncount.LotteryRewards, 1);
            var lvl2 = RewardUtil.GetCombinedRewardList(CurrTera.Value, CurrEncount.FixedRewards, CurrEncount.LotteryRewards, 2);
            var lvl3 = RewardUtil.GetCombinedRewardList(CurrTera.Value, CurrEncount.FixedRewards, CurrEncount.LotteryRewards, 3);

            var form = new RewardListForm(Language, lvl0, lvl1, lvl2, lvl3);
            form.ShowDialog();
        }
    }

    public EncounterRaidTF9[] GetCurrentEncounters(RaidContent content, TeraRaidMapParent map) => content switch
    {
        RaidContent.Standard => map switch
        {
            TeraRaidMapParent.Paldea => Paldea,
            TeraRaidMapParent.Kitakami => Kitakami,
            TeraRaidMapParent.Blueberry => Blueberry,
            _ => throw new NotImplementedException(nameof(cmbMap.SelectedIndex)),
        },
        RaidContent.Black => map switch
        {
            TeraRaidMapParent.Paldea => PaldeaBlack,
            TeraRaidMapParent.Kitakami => KitakamiBlack,
            TeraRaidMapParent.Blueberry => BlueberryBlack,
            _ => throw new NotImplementedException(nameof(cmbMap.SelectedIndex)),
        },
        RaidContent.Event => Dist,
        RaidContent.Event_Mighty => Mighty,
        _ => throw new NotImplementedException(nameof(cmbContent.SelectedIndex)),
    };

    private void btnShinifyCurrent_Click(object sender, EventArgs e) => EditCurrentRaid(true, true);
    private void btnRandomizeCurrent_Click(object sender, EventArgs e) => EditCurrentRaid(false, false);

    private void btnRandomizeAll_Click(object sender, EventArgs e) => EditAllRaids(false, false);
    private void BtnShinifyAllRaids_Click(object sender, EventArgs e) => EditAllRaids(false, true);
    private void BtnShinyAllEncounters_Click(object sender, EventArgs e) => EditAllRaids(true, true);

    private void EditCurrentRaid(bool forceEncounter, bool forceShiny)
    {
        if (CurrEncount is null || CurrTera is null)
            return;

        var spawns = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };

        if (!spawns.GetAllRaids().Any(raid => raid.IsEnabled))
            return;

        ShinifyRaid(CurrEncount, CurrTera.Value, spawns.GetRaid(cmbDens.SelectedIndex), forceEncounter, forceShiny);
        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
    }

    private void EditAllRaids(bool forceEncouner, bool forceShiny)
    {
        var spawns = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };

        if (!spawns.GetAllRaids().Any(raid => raid.IsEnabled))
            return;

        Parallel.For(0, spawns.GetAllRaids().Length, i =>
        {
            var raid = spawns.GetRaid(i);
            var content = (RaidContent)raid.Content;
            var encounters = GetCurrentEncounters(content, CurrMap);
            var eventProgress = EventUtil.GetEventStageFromProgress(Progress);
            var groupid = content >= RaidContent.Event ? EventUtil.GetDeliveryGroupID(content is RaidContent.Event ?
                (EncounterEventTF9[])encounters : (EncounterEventTF9[])encounters, SAV, eventProgress, spawns, i) : (byte)0;

            if (!EncounterRaidTF9.TryGenerateTeraDetails(raid.Seed, encounters, SAV.Version, Progress, eventProgress, content, CurrMap, SAV.ID32, groupid, out var encounter, out var detail))
                return;

            ShinifyRaid(encounter, detail!.Value, raid, forceEncouner, forceShiny);
        });

        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
    }

    private void ShinifyRaid(EncounterRaidTF9? encounter, TeraDetails details, TeraRaidDetail raid, bool forceEncounter, bool forceShiny)
    {
        if (encounter is null)
            return;

        if (encounter.Shiny is Shiny.Never)
            return;

        if (details.Shiny is TeraShiny.Yes)
            return;

        var filter = new TeraFilter(forceEncounter, false, false, false)
        {
            IsFormFilter = forceEncounter,
            MinHP = 0,
            MaxHP = 31,
            MinAtk = 0,
            MaxAtk = 31,
            MinDef = 0,
            MaxDef = 31,
            MinSpa = 0,
            MaxSpa = 31,
            MinSpd = 0,
            MaxSpd = 31,
            MinSpe = 0,
            MaxSpe = 31,
            MinScale = 0,
            MaxScale = 255,
            Stars = encounter.Stars,
            Species = encounter.Species,
            Form = encounter.Form,
            TeraType = -1,
            AbilityNumber = 0,
            Nature = Nature.Random,
            Gender = Gender.Random,
            Shiny = forceShiny ? TeraShiny.Yes : TeraShiny.Any,
            AltEC = details.EC % 100 == 0,
        };

        TeraDetails? res = null;
        var xoro = new Xoroshiro128Plus(details.Seed);

        EncounterRaidTF9[] encounters = filter.EncounterFilter is true ? encounter.ContentType switch
        {
            RaidContent.Standard or RaidContent.Black => ((EncounterTeraTF9[])GetCurrentEncounters(encounter.ContentType, encounter.Map)).Where(filter.IsEncounterMatch).ToArray(),
            RaidContent.Event or RaidContent.Event_Mighty => ((EncounterEventTF9[])GetCurrentEncounters(encounter.ContentType, encounter.Map)).Where(filter.IsEncounterMatch).ToArray(),
            _ => throw new NotImplementedException(nameof(encounter.ContentType)),
        } : GetCurrentEncounters(encounter.ContentType, encounter.Map);

        for (uint i = 0; i <= 0xFFFFFFFF; i++)
            if (EncounterRaidTF9.TryGenerateTeraDetails((uint)(xoro.Next() & 0xFFFFFFFF), encounters, SAV.Version, Progress,
                    EventUtil.GetEventStageFromProgress(Progress), encounter.ContentType, encounter.Map, SAV.ID32, encounter.Index, out _, out res))
                    if (filter.IsFilterMatch(res!.Value))
                        break;

        raid.Seed = res is not null ? res.Value.Seed : raid.Seed;
        raid.IsEnabled = true;
        raid.IsClaimedLeaguePoints = false;
    }
}
