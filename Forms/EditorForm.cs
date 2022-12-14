using Octokit;
using PKHeX.Core;

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

        public EditorForm(SAV9SV sav, IPKMView? editor)
        {
            InitializeComponent();
            SAV = sav;
            PKMEditor = editor;
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
            if(!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
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
                var encounter = cmbContent.SelectedIndex < 2 ? TeraUtil.GetTeraEncounter(raid.Seed, SAV, TeraUtil.GetStars(raid.Seed, progress)) :
                    TeraUtil.GetDistEncounter(raid.Seed, SAV, progress, raid.Content is TeraRaidContentType.Might7);

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
    }
}
