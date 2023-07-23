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
    private readonly Dictionary<string, float[]> DenLocations = null!;
    public GameProgress Progress { get; set; } = GameProgress.None;
    private Image DefBackground = null!;
    private Size DefSize = new(0, 0);
    private bool Loaded = false;
    private Dictionary<string, string> Strings = null!;
    private string[] Locations = null!;
    private ConnectionForm? Connection = null;

    public string Language = null!;
    public EncounterRaid9[]? Tera = null;
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
        EncounterRaid9[]? tera,
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
        InitLocations();

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
        Tera = tera is null ? TeraUtil.GetAllTeraEncounters() : tera;
        DenLocations = JsonSerializer.Deserialize<Dictionary<string, float[]>>(TeraUtil.GetDenLocations())!;
        DefBackground = pictureBox.BackgroundImage!;
        DefSize = pictureBox.Size;
        Progress = TeraUtil.GetProgress(SAV);
        foreach (var name in GetRaidNameList())
            cmbDens.Items.Add(name);
        cmbDens.SelectedIndex = 0;
        btnSx.Enabled = false;
        Connection = connection;
    }

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
            { "ShinifyForm.lblValue", "Progress:" },
            { "ShinifiedAll", "All raids have been Shinified." },
            { "MultipleLocations", "This Raid Den may have multiple locations." }
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

    private void InitLocations()
    {
        Locations = TeraUtil.Area;
        Locations[1] = Strings["AREASPA1"];
        Locations[4] = Strings["AREASPA2"];
        Locations[5] = Strings["AREASPA4"];
        Locations[6] = Strings["AREASPA6"];
        Locations[7] = Strings["AREASPA5"];
        Locations[8] = Strings["AREASPA3"];
        Locations[9] = Strings["AREAWPA1"];
        Locations[10] = Strings["AREAASAD"];
        Locations[11] = Strings["AREAWPA2"];
        Locations[12] = Strings["AREAWPA3"];
        Locations[13] = Strings["AREATAGT"];
        Locations[14] = Strings["AREAEPA3"];
        Locations[15] = Strings["AREAEPA1"];
        Locations[16] = Strings["AREAEPA2"];
        Locations[17] = Strings["AREADALI"];
        Locations[18] = Strings["AREACASS"];
        Locations[19] = Strings["AREAGLAS"];
        Locations[20] = Strings["AREANPA3"];
        Locations[21] = Strings["AREANPA1"];
        Locations[22] = Strings["AREANPA2"];
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

        var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
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
            var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
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
            var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
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
            var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
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
                var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
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
                var block = SAV.Accessor.FindOrDefault(Blocks.KTeraRaids.Key)!;
                await Connection.Executor.WriteBlock(block.Data, Blocks.KTeraRaids, new CancellationToken()).ConfigureAwait(false);
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
            var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, cmbDens.SelectedIndex);
            var progress = raid.Content is TeraRaidContentType.Black6 ? GameProgress.None : Progress;

            var encounter = cmbContent.SelectedIndex < 2 ? TeraUtil.GetTeraEncounter(raid.Seed, SAV, TeraUtil.GetStars(raid.Seed, progress), Tera!) :
                raid.Content is TeraRaidContentType.Might7 ? TeraUtil.GetDistEncounter(raid.Seed, SAV, progress, Mighty!, groupid) :
                TeraUtil.GetDistEncounter(raid.Seed, SAV, progress, Dist!, groupid);

            if (encounter is not null)
            {
                var rngres = TeraUtil.CalcRNG(raid.Seed, SAV.TrainerTID7, SAV.TrainerSID7, (RaidContent)raid.Content, encounter);

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
                {
                    pictureBox.Size = pictureBox.Image.Size;
                }
                else
                {
                    pictureBox.BackgroundImage = DefBackground;
                    pictureBox.Size = DefSize;
                }

                imgMap.SetMapPoint(Strings["MultipleLocations"], rngres.TeraType, (int)raid.AreaID, (int)raid.SpawnPointID, DenLocations);

                btnRewards.Width = pictureBox.Image is not null ? pictureBox.Image.Width : pictureBox.BackgroundImage!.Width;
                btnRewards.Visible = true;

                CurrEncount = encounter;
                CurrTera = rngres;

                SetStarSymbols(rngres.Stars);
                SetLevelLabel(rngres.Level);
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

    private string[] GetRaidNameList()
    {
        var names = new string[69];
        var raids = SAV.Raid.GetAllRaids();
        for (var i = 0; i < 69; i++)
            names[i] = $"{Strings["EditorForm.CmbRaid"]} {i + 1} - {TeraUtil.Area[raids[i].AreaID]} [{raids[i].SpawnPointID}]";
        return names;
    }

    private void btnOpenCalculator_Click(object sender, EventArgs e)
    {
        var calcform = new CalculatorForm(this);
        calcform.Show();
    }

    private void btnOpenRewardCalculator_Click(object sender, EventArgs e)
    {
        var calcform = new RewardCalcForm(this);
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
            form.Show();
        }
    }

    private void BtnShinifyAllRaids_Click(object sender, EventArgs e) => ShinifyRaids(false);
    private void BtnShinyAllEncounters_Click(object sender, EventArgs e) => ShinifyRaids(true);

    private void ShinifyRaids(bool keepEncounter)
    {
        var progressWindow = new ShinifyForm(0, Strings["ShinifyForm.lblValue"]);

        var raidList = SAV.Raid.GetAllRaids();
        foreach (var iterator in raidList.Select((value, i) => new { i, value }))
        {
            var raid = iterator.value;
            var index = iterator.i;

            if (index > 69)
                break;

            if (raid.AreaID == 0)
                continue;

            var currProgress = (index * 100) / 69;
            progressWindow.UpdateComputedValue(currProgress);

            var seed = raid.Seed;
            var content = (RaidContent)raid.Content;
            var groupid = TeraUtil.GetDeliveryGroupID(SAV, Progress, content, content is RaidContent.Event_Mighty ? Mighty : Dist, index);
            var progress = content is RaidContent.Black ? (GameProgress)6 : Progress;
            var originalEncounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, SAV, TeraUtil.GetStars(seed, progress), Tera!) :
                content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, SAV, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(seed, SAV, progress, Dist!, groupid);

            if (originalEncounter is null)
                continue;

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
                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(tseed, SAV, TeraUtil.GetStars(tseed, progress), Tera!) :
                            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(tseed, SAV, progress, Mighty!, groupid) : TeraUtil.GetDistEncounter(tseed, SAV, progress, Dist!, groupid);

                        var rngres = encounter is not null && (!keepEncounter || (encounter.Species == originalEncounter.Species && encounter.Form == originalEncounter.Form)) ?
                            TeraUtil.CalcRNG(tseed, SAV.TrainerTID7, SAV.TrainerSID7, content, encounter) : null;

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
        MessageBox.Show(Strings["ShinifiedAll"]);
        SystemSounds.Asterisk.Play();
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
