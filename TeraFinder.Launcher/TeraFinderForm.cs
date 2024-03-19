using TeraFinder.Plugins;

namespace TeraFinder.Launcher
{
    public partial class TeraFinderForm : Form
    {
        private readonly TeraPlugin Plugin = new();
        private ConnectionForm? Connection = null;

        private string GameVersionSV = "ScVi";
        private string GameVersionSL = "Scarlet";
        private string GameVersionVL = "Violet";
        private string TrainerBlank = "TeraFinder";
        private string NewsEvent = "Poké Portal News Event: [Raid: {0}, Outbreak: {1}]";
        private string None = "None";
        private string SAVInvalid = "Not a valid save file.";

        public TeraFinderForm()
        {
            InitializeComponent();
            this.TranslateInterface(TeraPlugin.GetDefaultLanguageString());
            TranslateInnerStrings(TeraPlugin.GetDefaultLanguageString());
            Plugin.StandaloneInitialize(TrainerBlank);
            txtSAV.Text = GetGameString();
            btnEditGame.Enabled = false;
            btnStartEditor.Enabled = false;
            btnExport.Enabled = false;
            btnEditOutbreaks.Enabled = false;
            UpdateEventLabel();
            cmbLanguage.SelectedIndex = TeraPlugin.GetDefaultLanguage();
        }

        private void TranslateInnerStrings(string language)
        {
            var inner = new Dictionary<string, string>
            {
                { nameof(GameVersionSV), GameVersionSV },
                { nameof(GameVersionSL), GameVersionSL },
                { nameof(GameVersionVL), GameVersionVL },
                { nameof(TrainerBlank), TrainerBlank },
                { nameof(NewsEvent), NewsEvent },
                { nameof(None), None },
                { nameof(SAVInvalid), SAVInvalid },
            };

            var translated = inner.TranslateInnerStrings(language);
            if (translated.TryGetValue(nameof(GameVersionSV), out var sv))
                GameVersionSV = sv;
            if (translated.TryGetValue(nameof(GameVersionSL), out var sl))
                GameVersionSL = sl;
            if (translated.TryGetValue(nameof(GameVersionVL), out var vl))
                GameVersionVL = vl;
            if (translated.TryGetValue(nameof(TrainerBlank), out var trainer))
                TrainerBlank = trainer;
            if (translated.TryGetValue(nameof(NewsEvent), out var news))
                NewsEvent = news;
            if (translated.TryGetValue(nameof(None), out var none))
                None = none;
            if (translated.TryGetValue(nameof(SAVInvalid), out var sav))
                SAVInvalid = sav;
        }

        private void FormEnabledChanged(object sender, EventArgs e)
        {
            if (Connection is not null)
            {
                if (Connection.IsConnected())
                {
                    btnEditGame.Enabled = true;
                    btnStartEditor.Enabled = true;
                    btnEditOutbreaks.Enabled = true;
                    btnExport.Enabled = false;
                    btnLoad.Enabled = false;
                    this.TranslateInterface(Plugin.Language);
                    TranslateInnerStrings(Plugin.Language);
                    UpdateEventLabel();
                    this.Text += TeraPlugin.Version;
                    txtSAV.Text = GetGameString();
                }
                else if (Plugin.GetSavName().Equals(TrainerBlank))
                {
                    btnEditGame.Enabled = false;
                    btnEditOutbreaks.Enabled = false;
                    btnStartEditor.Enabled = false;
                    btnLoad.Enabled = true;
                    this.TranslateInterface(Plugin.Language);
                    TranslateInnerStrings(Plugin.Language);
                    UpdateEventLabel();
                    this.Text += TeraPlugin.Version;
                    txtSAV.Text = GetGameString();
                }
                else if (!Connection.IsConnected())
                {
                    btnLoad.Enabled = true;
                }
            }
            else
            {
                btnLoad.Enabled = true;
            }
        }

        private void UpdateEventLabel()
        {
            var raidId = Plugin.GetRaidEventIdentifier();
            var outbreakId = Plugin.GetOutbreakEventIdentifier();
            lblEvent.Text = string.Format(NewsEvent, raidId > 0 ? raidId : None, outbreakId > 0 ? outbreakId : None); ;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                LoadSAV(openFileDialog.FileName);
        }

        void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data is not null)
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
        }

        void FileDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is not null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
                LoadLocalFiles(files[0]);
            }
        }

        public void LoadLocalFiles(string file)
        {
            if (File.Exists(file) && Path.GetExtension(file).Equals(".zip"))
            {
                Plugin.TryLoadFile(file);
                UpdateEventLabel();
            }
            else
                LoadSAV(file);
        }

        public void LoadSAV(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    var sav = File.ReadAllBytes(file);
                    Plugin.StandaloneInitialize(TrainerBlank, sav);
                    btnEditGame.Enabled = true;
                    btnEditOutbreaks.Enabled = true;
                    btnImportNews.Enabled = true;
                    btnStartEditor.Enabled = true;
                    btnExport.Enabled = true;
                    this.TranslateInterface(Plugin.Language);
                    TranslateInnerStrings(Plugin.Language);
                    UpdateEventLabel();
                    this.Text += TeraPlugin.Version;
                    txtSAV.Text = GetGameString();
                }
                catch (Exception)
                {
                    MessageBox.Show(SAVInvalid);
                    Plugin.StandaloneInitialize(TrainerBlank);
                    btnEditGame.Enabled = false;
                    btnEditOutbreaks.Enabled = false;
                    btnStartEditor.Enabled = false;
                    btnExport.Enabled = false;
                    this.TranslateInterface(Plugin.Language);
                    TranslateInnerStrings(Plugin.Language);
                    UpdateEventLabel();
                    this.Text += TeraPlugin.Version;
                    txtSAV.Text = GetGameString();
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Plugin.ExportSAVDialog();
        }

        private void btnEditGame_Click(object sender, EventArgs e)
        {
            Plugin.LaunchGameEditor();
        }

        private void btnImportNews_Click(object sender, EventArgs e)
        {
            Plugin.LaunchImporter();
            UpdateEventLabel();
        }

        private void btnStartEditor_Click(object sender, EventArgs e)
        {
            Plugin.LaunchEditor();
        }

        private void btnStartCalculator_Click(object sender, EventArgs e)
        {
            Plugin.LaunchCalculator();
        }

        private void btnStartSeedChecker_Click(object sender, EventArgs e)
        {
            Plugin.LaunchSeedChecker();
        }

        private void btnStartRewardCalc_Click(object sender, EventArgs e)
        {
            Plugin.LaunchRewardCalculator();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Plugin.LaunchMassOutbreakEditor();
        }

        private string GetGameString()
        {
            var str = Plugin.GetSavName();
            str = str.Replace("Any", GameVersionSV);
            str = str.Replace("SL", GameVersionSL);
            str = str.Replace("VL", GameVersionVL);
            str = str.Replace("(0)", "");
            return str;
        }

        private void btnRemoteConnect_Click(object sender, EventArgs e)
        {
            Connection = Plugin.LaunchConnector(this);
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            var lang = TeraPlugin.GetStringLanguage(cmbLanguage.SelectedIndex);
            TeraPlugin.SetDefaultLanguage(cmbLanguage.SelectedIndex);

            if (GetGameString().Contains(TrainerBlank))
            {
                this.SuspendLayout();
                TranslateInnerStrings(lang);
                this.TranslateInterface(lang);
                Plugin.StandaloneInitialize(TrainerBlank, language: lang);
                UpdateEventLabel();
                txtSAV.Text = GetGameString();
                btnEditGame.Enabled = false;
                btnEditOutbreaks.Enabled = false;
                btnStartEditor.Enabled = false;
                btnExport.Enabled = false;
                cmbLanguage.PerformClick();
                this.Text += TeraPlugin.Version;
                this.ResumeLayout();
            }
        }

        private void ImportNullRaid_Click(object sender, EventArgs e)
        {
            Plugin.LaunchRaidNullImporter();
            UpdateEventLabel();
        }

        private void ImportNullOutbreak_Click(object sender, EventArgs e)
        {
            Plugin.LaunchOutbreakNullImporter();
            UpdateEventLabel();
        }
    }
}