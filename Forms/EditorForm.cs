using PKHeX.Core;
using TeraFinder.Forms;

namespace TeraFinder
{
    public partial class EditorForm : Form
    {
        public SAV9SV SAV { get; set; } = null!;
        public IPKMView? PKMEditor { get; private set; } = null!;
        public GameProgress Progress { get; set; } = GameProgress.None;
        private Image DefBackground = null!;
        private Size DefSize = new(0, 0);
        private bool Loaded = false;

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
            EncounterRaid9[]? tera,
            EncounterRaid9[]? dist,
            EncounterRaid9[]? mighty,
            Dictionary<ulong, List<Reward>>? terafixed,
            Dictionary<ulong, List<Reward>>? teralottery,
            Dictionary<ulong, List<Reward>>? distfixed,
            Dictionary<ulong, List<Reward>>? distlottery)
        {
            InitializeComponent();
            SAV = sav;
            PKMEditor = editor;
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
            DefBackground = pictureBox.BackgroundImage;
            DefSize = pictureBox.Size;
            Progress = TeraUtil.GetProgress(SAV);
            foreach (var name in GetRaidNameList())
                cmbDens.Items.Add(name);
            cmbDens.SelectedIndex = 0;
            btnSx.Enabled = false;
        }

        private void cmbDens_IndexChanged(object sender, EventArgs e)
        {
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
            if (!Loaded)
                UpdatePKMInfo(raid);
            Loaded = true;
        }

        private void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
                if (chkActive.Checked)
                    raid.IsEnabled = true;
                else
                    raid.IsEnabled = false;
                UpdatePKMInfo(raid);
            }
        }

        private void chkLP_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
                if (chkLP.Checked)
                    raid.IsClaimedLeaguePoints = false;
                else
                    raid.IsEnabled = true;
            }
        }

        private void cmbContent_IndexChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var raid = SAV.Raid.GetRaid(cmbDens.SelectedIndex);
                raid.Content = (TeraRaidContentType)cmbContent.SelectedIndex;
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
                    UpdatePKMInfo(raid);
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

                    lblSpecies.Text = $"Species: {rngres.Species}";
                    lblTera.Text = $"TeraType: {rngres.TeraType}";
                    lblNature.Text = $"Nature: {rngres.Nature}";
                    lblAbility.Text = $"Ability: {rngres.Ability}";
                    lblShiny.Text = $"Shiny: {rngres.Shiny}";
                    lblGender.Text = $"Gender: {rngres.Gender}";
                    txtHP.Text = $"{rngres.HP}";
                    txtAtk.Text = $"{rngres.ATK}";
                    txtDef.Text = $"{rngres.DEF}";
                    txtSpA.Text = $"{rngres.SPA}";
                    txtSpD.Text = $"{rngres.SPD}";
                    txtSpe.Text = $"{rngres.SPE}";

                    pictureBox.BackgroundImage = null;
                    pictureBox.Image = SpritesUtil.GetRaidResultSprite(rngres, raid.IsEnabled);
                    pictureBox.Size = pictureBox.Image.Size;
                    btnRewards.Width = pictureBox.Image.Width;
                    btnRewards.Visible = true;

                    CurrEncount = encounter;
                    CurrTera = rngres;

                    SetStarSymbols(rngres.Stars);
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

            pictureBox.BackgroundImage = DefBackground;
            pictureBox.Size = DefSize;
            pictureBox.Image = null;
            btnRewards.Visible = false;
            CurrEncount = null;
            CurrTera = null;
            SetStarSymbols(0);
        }

        private void SetStarSymbols(int stars)
        {
            var str = "";
            for (var i = 0; i < stars; i++)
                str += "☆";

            var img = pictureBox.Image != null ? pictureBox.Image : pictureBox.BackgroundImage;
            lblStarSymbols.Text = str;
            lblStarSymbols.Location = new(pictureBox.Location.X + (pictureBox.Width - lblStarSymbols.Size.Width) / 2, pictureBox.Location.Y + img.Height);
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

                /*var lvl0 = RewardUtil.GetRewardList(Convert.ToUInt32(txtSeed.Text, 16), CurrEncount.Stars, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                    CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 0);
                var lvl1 = RewardUtil.GetRewardList(Convert.ToUInt32(txtSeed.Text, 16), CurrEncount.Stars, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                    CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 1);
                var lvl2 = RewardUtil.GetRewardList(Convert.ToUInt32(txtSeed.Text, 16), CurrEncount.Stars, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                    CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 2);
                var lvl3 = RewardUtil.GetRewardList(Convert.ToUInt32(txtSeed.Text, 16), CurrEncount.Stars, CurrEncount.FixedRewardHash, CurrEncount.LotteryRewardHash,
                    CurrEncount.IsDistribution ? DistFixedRewards : TeraFixedRewards, CurrEncount.IsDistribution ? DistLotteryRewards : TeraLotteryRewards, 3);*/

                var form = new RewardListForm(SAV.Language, lvl0, lvl1, lvl2, lvl3);
                form.Show();
            }
        }
    }
}
