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
    public GameProgress Progress { get; set; } = GameProgress.None;
    private readonly Image DefBackground = null!;
    private Size DefSize = new(0, 0);
    private bool Loaded = false;
    private Dictionary<string, string> Strings = null!;
    private string[] PaldeaLocations = null!;
    private string[] KitakamiLocations = null!;
    private string[] BlueberryLocations = null!;

    private ConnectionForm? Connection = null;

    public TeraRaidMapParent CurrMap = TeraRaidMapParent.Paldea;

    public string Language = null!;
    public EncounterRaid9[]? Paldea = null;
    public EncounterRaid9[]? Kitakami = null;
    public EncounterRaid9[]? Blueberry = null;
    public EncounterRaid9[]? Dist = null;
    public EncounterRaid9[]? Mighty = null;
    public Dictionary<ulong, List<Reward>>? TeraFixedRewards = null;
    public Dictionary<ulong, List<Reward>>? TeraLotteryRewards = null;
    public Dictionary<ulong, List<Reward>>? DistFixedRewards = null;
    public Dictionary<ulong, List<Reward>>? DistLotteryRewards = null;

    public EncounterRaid9? CurrEncount = null;
    public TeraDetails? CurrTera = null;

    public EditorForm(SAV9SV sav,
        IPKMView? editor,
        string language,
        EncounterRaid9[]? paldea,
        EncounterRaid9[]? kitakami,
        EncounterRaid9[]? blueberry,
        EncounterRaid9[]? dist,
        EncounterRaid9[]? mighty,
        Dictionary<ulong, List<Reward>>? terafixed,
        Dictionary<ulong, List<Reward>>? teralottery,
        Dictionary<ulong, List<Reward>>? distfixed,
        Dictionary<ulong, List<Reward>>? distlottery,
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

        if (dist is null)
        {
            var events = TeraUtil.GetSAVDistEncounters(SAV);
            var eventsrewards = RewardUtil.GetDistRewardsTables(SAV);
            Dist = events[0];
            Mighty = events[1];
            DistFixedRewards = eventsrewards[0];
            DistLotteryRewards = eventsrewards[1];
        }
        else
        {
            Dist = dist;
            Mighty = mighty;
            DistFixedRewards = distfixed;
            DistLotteryRewards = distlottery;
        }
        if (terafixed is null)
        {
            var terarewards = RewardUtil.GetTeraRewardsTables();
            TeraFixedRewards = terarewards[0];
            TeraLotteryRewards = terarewards[1];
        }
        else
        {
            TeraFixedRewards = terafixed;
            TeraLotteryRewards = teralottery;
        }
        Paldea = paldea is null ? TeraUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea) : paldea;
        Kitakami = kitakami is null ? TeraUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami) : kitakami;
        Blueberry = blueberry is null ? TeraUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry) : blueberry;
        DefBackground = pictureBox.BackgroundImage!;
        DefSize = pictureBox.Size;
        Progress = TeraUtil.GetProgress(SAV);
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
        DenLocations = JsonSerializer.Deserialize<Dictionary<string, float[]>>(TeraUtil.GetDenLocations(CurrMap))!;

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
        PaldeaLocations = TeraUtil.AreaPaldea;
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

        KitakamiLocations = TeraUtil.AreaKitakami;
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

        BlueberryLocations = TeraUtil.AreaBlueberry;
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
                var block = CurrMap is TeraRaidMapParent.Paldea ? Blocks.KTeraRaidPaldea : Blocks.KTeraRaidDLC;
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
            var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist,
                CurrMap switch { TeraRaidMapParent.Paldea => SAV.RaidPaldea, TeraRaidMapParent.Kitakami => SAV.RaidKitakami, _ => SAV.RaidBlueberry }, cmbDens.SelectedIndex);
            var progress = raid.Content is TeraRaidContentType.Black6 ? GameProgress.None : Progress;
            var encounter = cmbContent.SelectedIndex < 2 ? TeraUtil.GetTeraEncounter(raid.Seed, SAV.Version,
                TeraUtil.GetStars(raid.Seed, progress), CurrMap switch { TeraRaidMapParent.Paldea => Paldea!, TeraRaidMapParent.Kitakami => Kitakami!, _ => Blueberry! }, CurrMap) :
                raid.Content is TeraRaidContentType.Might7 ? TeraUtil.GetDistEncounter(raid.Seed, SAV.Version, progress, Mighty!, groupid) :
                TeraUtil.GetDistEncounter(raid.Seed, SAV.Version, progress, Dist!, groupid);

            if (encounter is not null)
            {
                var rngres = TeraUtil.CalcRNG(raid.Seed, SAV.TrainerTID7, SAV.TrainerSID7, (RaidContent)raid.Content, encounter, groupid);

                var species = GameInfo.GetStrings(Language).specieslist;
                var types = GameInfo.GetStrings(Language).types;
                var forms = GameInfo.GetStrings(Language).forms;
                var natures = GameInfo.GetStrings(Language).natures;
                var abilities = GameInfo.GetStrings(Language).abilitylist;
                var genders = GameInfo.GenderSymbolUnicode.ToArray();
                var moves = GameInfo.GetStrings(Language).movelist;

                lblSpecies.Text = $"{Strings["EditorForm.lblSpecies"]} {rngres.GetName(species, types, forms, genders)}";
                lblTera.Text = $"{Strings["EditorForm.lblTera"]} {types[rngres.TeraType]}";
                lblNature.Text = $"{Strings["EditorForm.lblNature"]} {natures[rngres.Nature]}";
                lblAbility.Text = $"{Strings["EditorForm.lblAbility"]} {abilities[rngres.Ability]}";
                lblShiny.Text = $"{Strings["EditorForm.lblShiny"]} {rngres.Shiny}";
                lblGender.Text = $"{Strings["EditorForm.lblGender"]} {genders[(int)rngres.Gender]}";
                lblIndex.Text = $"{Strings["EditorForm.lblIndex"]} {groupid}";
                lblIndex.Visible = groupid != 0;
                txtHP.Text = $"{rngres.HP}";
                txtAtk.Text = $"{rngres.ATK}";
                txtDef.Text = $"{rngres.DEF}";
                txtSpA.Text = $"{rngres.SPA}";
                txtSpD.Text = $"{rngres.SPD}";
                txtSpe.Text = $"{rngres.SPE}";
                txtScale.Text = $"{rngres.Scale}";
                txtMove1.Text = $"{moves[rngres.Move1]}";
                txtMove2.Text = $"{moves[rngres.Move2]}";
                txtMove3.Text = $"{moves[rngres.Move3]}";
                txtMove4.Text = $"{moves[rngres.Move4]}";

                pictureBox.BackgroundImage = null;
                pictureBox.Image = GetRaidResultSprite(rngres, raid.IsEnabled, encounter.Item);
                if (pictureBox.Image is not null)
                    pictureBox.Size = pictureBox.Image.Size;
                else
                {
                    pictureBox.BackgroundImage = DefBackground;
                    pictureBox.Size = DefSize;
                }

                imgMap.SetMapPoint((MoveType)rngres.TeraType, (int)raid.AreaID, (int)raid.LotteryGroup, (int)raid.SpawnPointID, CurrMap, DenLocations);

                btnRewards.Width = pictureBox.Image is not null ? pictureBox.Image.Width : pictureBox.BackgroundImage!.Width;
                btnRewards.Visible = true;

                CurrEncount = encounter;
                CurrTera = rngres;

                SetStarSymbols(rngres.Stars);
                SetLevelLabel(rngres.Level);

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

    private void btnOpenCalculator_Click(object sender, EventArgs e)
    {
        var calcform = new CalculatorForm(this);

        if (CurrEncount is not null && CurrEncount.IsDistribution)
        {
            calcform.CurrentViewedIndex = CurrEncount.Index;
            calcform.NotLinkedSearch = false;
        }

        calcform.Show();
    }

    private void btnOpenRewardCalculator_Click(object sender, EventArgs e)
    {
        var calcform = new RewardCalcForm(this);

        if (CurrEncount is not null && CurrEncount.IsDistribution)
        {
            calcform.CurrentViewedIndex = CurrEncount.Index;
            calcform.NotLinkedSearch = false;
        }

        calcform.Show();
    }

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
            var lvl0 = RewardUtil.GetRewardList(CurrTera, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 0);
            var lvl1 = RewardUtil.GetRewardList(CurrTera, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 1);
            var lvl2 = RewardUtil.GetRewardList(CurrTera, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 2);
            var lvl3 = RewardUtil.GetRewardList(CurrTera, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 3);

            var form = new RewardListForm(Language, lvl0, lvl1, lvl2, lvl3);
            form.ShowDialog();
        }
    }

    private void btnShinifyCurrent_Click(object sender, EventArgs e)
    {
        var spawnList = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };
        var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);

        var seed = raid.Seed;
        var content = (RaidContent)raid.Content;
        var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, spawnList, cmbDens.SelectedIndex);
        var progress = content is RaidContent.Black ? (GameProgress)6 : Progress;
        var originalEncounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV.Version,
            TeraUtil.GetStars(seed, progress), CurrMap switch { TeraRaidMapParent.Paldea => Paldea!, TeraRaidMapParent.Kitakami => Kitakami!, _ => Blueberry! }, CurrMap) :
            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Dist!, groupid);

        if (originalEncounter is null)
            return;

        if (originalEncounter.IsDistribution)
        {
            var canBeShiny = false;
            foreach (var enc in content is RaidContent.Event ? Dist! : Mighty!)
            {
                if (enc.Index != groupid)
                    continue;

                if (enc.Species != originalEncounter.Species || enc.Form != originalEncounter.Form)
                    continue;

                if (enc.Shiny is not Shiny.Never)
                {
                    canBeShiny = true;
                    break;
                }
            }

            if (!canBeShiny)
                return;
        }

        for (uint i = 0; i <= 0xFFFFFFFF; i++)
        {
            var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV.Version,
                TeraUtil.GetStars(seed, progress), CurrMap switch
                {
                    TeraRaidMapParent.Paldea => Paldea!,
                    TeraRaidMapParent.Kitakami => Kitakami!,
                    _ => Blueberry!
                }, CurrMap) :
            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Dist!, groupid);
            var rngres = encounter is not null && (encounter.Species == originalEncounter.Species && encounter.Form == originalEncounter.Form) ?
                TeraUtil.CalcRNG(seed, SAV.TrainerTID7, SAV.TrainerSID7, content, encounter, groupid) : null;

            var isShiny = rngres is not null && rngres.Shiny >= TeraShiny.Yes;

            if (!isShiny)
                seed++;
            else
            {
                seed = rngres!.Seed;
                break;
            }
        }

        raid.Seed = seed;
        raid.IsEnabled = true;
        raid.IsClaimedLeaguePoints = false;
        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
    }

    private void btnRandomizeCurrent_Click(object sender, EventArgs e)
    {
        var spawnList = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };
        var raid = spawnList.GetAllRaids().Length > 1 ? spawnList.GetRaid(cmbDens.SelectedIndex) : new TeraRaidDetail(new byte[TeraRaidDetail.SIZE]);

        var seed = raid.Seed;
        var content = (RaidContent)raid.Content;
        var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, spawnList, cmbDens.SelectedIndex);
        var progress = content is RaidContent.Black ? (GameProgress)6 : Progress;
        var originalEncounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV.Version,
            TeraUtil.GetStars(seed, progress), CurrMap switch
            {
                TeraRaidMapParent.Paldea => Paldea!,
                TeraRaidMapParent.Kitakami => Kitakami!,
                _ => Blueberry!
            }, CurrMap) :
            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Dist!, groupid);

        if (originalEncounter is null)
            return;

        var xoro = new Xoroshiro128Plus(seed);
        seed = (uint)(xoro.Next() & 0xFFFFFFFF);

        raid.Seed = seed;
        raid.IsEnabled = true;
        raid.IsClaimedLeaguePoints = false;
        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
    }

    private void btnRandomizeAll_Click(object sender, EventArgs e)
    {
        var spawnList = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };

        foreach (var raid in spawnList.GetAllRaids())
        {
            var seed = raid.Seed;
            var content = (RaidContent)raid.Content;
            var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, spawnList, cmbDens.SelectedIndex);
            var progress = content is RaidContent.Black ? (GameProgress)6 : Progress;
            var originalEncounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV.Version,
                TeraUtil.GetStars(seed, progress), CurrMap switch
                {
                    TeraRaidMapParent.Paldea => Paldea!,
                    TeraRaidMapParent.Kitakami => Kitakami!,
                    _ => Blueberry!
                }, CurrMap) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Dist!, groupid);

            if (originalEncounter is null)
                continue;

            var xoro = new Xoroshiro128Plus(seed);
            seed = (uint)(xoro.Next() & 0xFFFFFFFF);

            raid.Seed = seed;
            raid.IsEnabled = true;
            raid.IsClaimedLeaguePoints = false;
        }

        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
    }

    private void BtnShinifyAllRaids_Click(object sender, EventArgs e) => ShinifyRaids(false);
    private void BtnShinyAllEncounters_Click(object sender, EventArgs e) => ShinifyRaids(true);

    private void ShinifyRaids(bool keepEncounter)
    {
        var progressWindow = new ShinifyForm(0, Strings["ShinifyForm.lblValue"]);

        var spawnList = CurrMap switch
        {
            TeraRaidMapParent.Paldea => SAV.RaidPaldea,
            TeraRaidMapParent.Kitakami => SAV.RaidKitakami,
            _ => SAV.RaidBlueberry
        };
        var raidList = spawnList.GetAllRaids();
        var raidCount = raidList.Count(raid => raid.IsEnabled == true);
        foreach (var iterator in raidList.Select((value, i) => new { i, value }))
        {
            var raid = iterator.value;
            var index = iterator.i;

            if (index > raidCount)
                break;

            if (raid.AreaID == 0)
                continue;

            var currProgress = (index * 100) / raidCount;
            progressWindow.UpdateComputedValue(currProgress);

            var seed = raid.Seed;
            var content = (RaidContent)raid.Content;
            var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, spawnList, index);
            var progress = content is RaidContent.Black ? (GameProgress)6 : Progress;
            var originalEncounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV.Version,
                TeraUtil.GetStars(seed, progress), CurrMap switch
                {
                    TeraRaidMapParent.Paldea => Paldea!,
                    TeraRaidMapParent.Kitakami => Kitakami!,
                    _ => Blueberry!
                }, CurrMap) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV.Version, progress, Dist!, groupid);

            if (originalEncounter is null)
                continue;

            if (originalEncounter.IsDistribution)
            {
                var canBeShiny = false;
                foreach (var enc in content is RaidContent.Event ? Dist! : Mighty!)
                {
                    if (enc.Index != groupid)
                        continue;

                    if (keepEncounter && (enc.Species != originalEncounter.Species || enc.Form != originalEncounter.Form))
                        continue;

                    if (enc.Shiny is not Shiny.Never)
                    {
                        canBeShiny = true;
                        break;
                    }
                }

                if (!canBeShiny)
                    continue;
            }

            var token = new CancellationTokenSource();
            var nthreads = Environment.ProcessorCount;
            var resetEvent = new ManualResetEvent(false);
            var toProcess = nthreads;
            var calcsperthread = 0xFFFFFFFF / (uint)nthreads;

            for (uint i = 0; i < nthreads; i++)
            {
                var n = i;
                var tseed = seed;

                new Thread(delegate ()
                {
                    var initialFrame = calcsperthread * n;
                    var maxFrame = n < nthreads - 1 ? calcsperthread * (n + 1) : 0xFFFFFFFF;
                    tseed += initialFrame;

                    for (ulong j = initialFrame; j <= maxFrame && !token.IsCancellationRequested; j++)
                    {
                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(tseed, SAV.Version,
                            TeraUtil.GetStars(tseed, progress), CurrMap switch
                            {
                                TeraRaidMapParent.Paldea => Paldea!,
                                TeraRaidMapParent.Kitakami => Kitakami!,
                                _ => Blueberry!
                            }, CurrMap) :
                            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(tseed, SAV.Version, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(tseed, SAV.Version, progress, Dist!, groupid);

                        var rngres = encounter is not null && (!keepEncounter || (encounter.Species == originalEncounter.Species && encounter.Form == originalEncounter.Form)) ?
                            TeraUtil.CalcRNG(tseed, SAV.TrainerTID7, SAV.TrainerSID7, content, encounter, groupid) : null;

                        var isShiny = rngres is not null && rngres.Shiny >= TeraShiny.Yes;

                        if (!isShiny)
                            tseed++;
                        else
                        {
                            seed = rngres!.Seed;
                            token.Cancel();
                            break;
                        }
                    }

                    if (Interlocked.Decrement(ref toProcess) == 0 || token.IsCancellationRequested)
                        resetEvent.Set();

                }).Start();

                resetEvent.WaitOne();

                raid.Seed = seed;
                raid.IsEnabled = true;
                raid.IsClaimedLeaguePoints = false;
            }
        }

        progressWindow.Close();
        cmbDens_IndexChanged(this, new EventArgs());
        Task.Run(UpdateRemote).Wait();
        SystemSounds.Asterisk.Play();
        MessageBox.Show(Strings["ShinifiedAll"]);
    }

    public class ShinifyForm : Form
    {
        private readonly Label lblValue;
        private readonly string Progress;

        public ShinifyForm(int computedValue, string progress)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Size = new Size(200, 75);
            StartPosition = FormStartPosition.CenterParent;

            Progress = progress;
            lblValue = new Label { Text = $"{Progress} {computedValue}%", TextAlign = ContentAlignment.TopCenter };
            lblValue.Location = new Point((Width - lblValue.Width) / 2, lblValue.Location.Y);

            Controls.Add(lblValue);
            Show();
        }

        public void UpdateComputedValue(int computed)
        {
            lblValue.Text = $"{Progress} {computed}%";
            lblValue.Location = new Point((Width - lblValue.Width) / 2, lblValue.Location.Y);
        }
    }
}
