using System.Text.Json;
using PKHeX.Core;
using TeraFinder.Forms;
using static TeraFinder.ImagesUtil;

namespace TeraFinder
{
    public partial class EditorForm : Form
    {
        public SAV9SV SAV { get; set; } = null!;
        public IPKMView? PKMEditor { get; private set; } = null!;
        private Dictionary<string, float[]> DenLocations = null!;
        public GameProgress Progress { get; set; } = GameProgress.None;
        private Image DefBackground = null!;
        private Size DefSize = new(0, 0);
        private bool Loaded = false;
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
            DenLocations = JsonSerializer.Deserialize<Dictionary<string, float[]>>(Properties.Resources.den_locations)!;
            DefBackground = pictureBox.BackgroundImage;
            DefSize = pictureBox.Size;
            Progress = TeraUtil.GetProgress(SAV);
            foreach (var name in GetRaidNameList())
                cmbDens.Items.Add(name);
            cmbDens.SelectedIndex = 0;
            btnSx.Enabled = false;
            Connection = connection;
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
                    Task.Run(async () => { await UpdateRemote(); }).Wait();
                }
                else
                {
                    raid.IsEnabled = false;
                    Task.Run(async () => { await UpdateRemote(); }).Wait();
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
                    raid.IsClaimedLeaguePoints = false;
                    Task.Run(async () => { await UpdateRemote(); }).Wait();
                }
                else
                {
                    raid.IsEnabled = true;
                    Task.Run(async () => { await UpdateRemote(); }).Wait();
                }
            }
        }

        private void cmbContent_IndexChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
                raid.Content = (TeraRaidContentType)cmbContent.SelectedIndex;
                Task.Run(async () => { await UpdateRemote(); }).Wait();
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
                    Task.Run(async () => { await UpdateRemote(); }).Wait();
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
                    await Connection.Executor.WriteDecryptedBlock(block.Data, Blocks.KTeraRaids, new CancellationToken()).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                if (Connection is not null)
                {
                    Connection.Disconnect();
                    MessageBox.Show("The remote device has been disconnected.");
                    Connection = null;
                }
            }
        }

        private void UpdatePKMInfo(TeraRaidDetail raid)
        {
            if (Progress is not GameProgress.Beginning && raid.Seed != 0)
            {
                var progress = raid.Content is TeraRaidContentType.Black6 ? GameProgress.None : Progress;
                var encounter = cmbContent.SelectedIndex < 2 ? TeraUtil.GetTeraEncounter(raid.Seed, SAV, TeraUtil.GetStars(raid.Seed, progress), Tera!) :
                    raid.Content is TeraRaidContentType.Might7 ? TeraUtil.GetDistEncounter(raid.Seed, SAV, progress, Mighty!, true) :
                    TeraUtil.GetDistEncounter(raid.Seed, SAV, progress, Dist!, false);

                if (encounter is not null)
                {
                    var rngres = TeraUtil.CalcRNG(raid.Seed, SAV.TrainerID7, SAV.TrainerSID7, (RaidContent)raid.Content, encounter);

                    lblSpecies.Text = $"Species: {GameInfo.GetStrings(Language).specieslist[rngres.Species]}";
                    lblTera.Text = $"TeraType: {GameInfo.GetStrings(Language).types[rngres.TeraType]}";
                    lblNature.Text = $"Nature: {GameInfo.GetStrings(Language).natures[rngres.Nature]}";
                    lblAbility.Text = $"Ability: {GameInfo.GetStrings(Language).abilitylist[rngres.Ability]}";
                    lblShiny.Text = $"Shiny: {rngres.Shiny}";
                    lblGender.Text = $"Gender: {GameInfo.GenderSymbolUnicode[(int)rngres.Gender]}";
                    txtHP.Text = $"{rngres.HP}";
                    txtAtk.Text = $"{rngres.ATK}";
                    txtDef.Text = $"{rngres.DEF}";
                    txtSpA.Text = $"{rngres.SPA}";
                    txtSpD.Text = $"{rngres.SPD}";
                    txtSpe.Text = $"{rngres.SPE}";
                    txtMove1.Text = $"{GameInfo.GetStrings(Language).movelist[rngres.Move1]}";
                    txtMove2.Text = $"{GameInfo.GetStrings(Language).movelist[rngres.Move2]}";
                    txtMove3.Text = $"{GameInfo.GetStrings(Language).movelist[rngres.Move3]}";
                    txtMove4.Text = $"{GameInfo.GetStrings(Language).movelist[rngres.Move4]}";

                    pictureBox.BackgroundImage = null;
                    pictureBox.Image = GetRaidResultSprite(rngres, raid.IsEnabled);
                    pictureBox.Size = pictureBox.Image.Size;


                    imgMap.SetMapPoint(rngres.TeraType, (int)raid.AreaID, (int)raid.SpawnPointID, DenLocations);

                    btnRewards.Width = pictureBox.Image.Width;
                    btnRewards.Visible = true;

                    CurrEncount = encounter;
                    CurrTera = rngres;

                    SetStarSymbols(rngres.Stars);
                    SetLevelLabel(rngres.Level);
                    return;
                }
            }
            lblSpecies.Text = $"Species:";
            lblTera.Text = $"TeraType:";
            lblNature.Text = $"Nature:";
            lblAbility.Text = $"Ability:";
            lblShiny.Text = $"Shiny:";
            lblGender.Text = $"Gender:";
            txtHP.Text = $"";
            txtAtk.Text = $"";
            txtDef.Text = $"";
            txtSpA.Text = $"";
            txtSpD.Text = $"";
            txtSpe.Text = $"";
            txtMove1.Text = $"None";
            txtMove2.Text = $"None";
            txtMove3.Text = $"None";
            txtMove4.Text = $"None";

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

            var img = pictureBox.Image != null ? pictureBox.Image : pictureBox.BackgroundImage;
            lblStarSymbols.Text = str;
            lblStarSymbols.Location = new(pictureBox.Location.X + (pictureBox.Width - lblStarSymbols.Size.Width) / 2, pictureBox.Location.Y + img.Height);
        }

        private void SetLevelLabel(int level = 0)
        {
            var str = level == 0 ? "" : $"Lvl. {level}";
            lblLevel.Text = str;
            var img = pictureBox.Image != null ? pictureBox.Image : pictureBox.BackgroundImage;
            lblLevel.Location = new(pictureBox.Location.X + (pictureBox.Width - lblLevel.Size.Width) / 2, pictureBox.Location.Y - lblLevel.Height);
        }

        private string[] GetRaidNameList()
        {
            var names = new string[69];
            var raids = SAV.Raid.GetAllRaids();
            for (var i = 0; i < 69; i++)
                names[i] = $"Raid {i + 1} - {TeraUtil.Area[raids[i].AreaID]} [{raids[i].SpawnPointID}]";
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
    }
}
