using TeraFinder;
using TeraFinder.Forms;

namespace TeraFinder.Launcher
{
    public partial class TeraFinderForm : Form
    {
        private readonly TeraPlugin Plugin = new();
        private ConnectionForm? Connection = null;

        public TeraFinderForm()
        {
            InitializeComponent();
            Plugin.StandaloneInitialize();
            this.Text += TeraPlugin.Version;
            txtSAV.Text = GetGameString();
            btnEditGame.Enabled = false;
            //btnImportNews.Enabled = false;
            btnStartEditor.Enabled = false;
            btnExport.Enabled = false;
        }

        private void FormEnabledChanged(object sender, EventArgs e)
        {
            if(Connection is not null)
            {
                if (Connection.IsConnected())
                {
                    btnEditGame.Enabled = true;
                    btnStartEditor.Enabled = true;
                    txtSAV.Text = GetGameString();
                }
                else if(Plugin.GetSavName().Equals("TeraFinder"))
                {
                    btnEditGame.Enabled = false;
                    btnStartEditor.Enabled = false;
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(Connection is not null && Connection.IsConnected())
            {
                //Chiedere se va bene disconnettersi e disconnettersi prima di far selezionare un save file
            }
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
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                LoadLocalFiles(files[0]);
            }
        }

        public void LoadLocalFiles(string file)
        {
            if(File.Exists(file) && Path.GetExtension(file).Equals(".zip"))
                Plugin.TryLoadFile(file);
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
                    Plugin.StandaloneInitialize(sav);
                    txtSAV.Text = GetGameString();
                    btnEditGame.Enabled = true;
                    btnImportNews.Enabled = true;
                    btnStartEditor.Enabled = true;
                    btnExport.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Not a valid save file.");
                    Plugin.StandaloneInitialize();
                    txtSAV.Text = GetGameString();
                    btnEditGame.Enabled = false;
                    //btnImportNews.Enabled = false;
                    btnStartEditor.Enabled = false;
                    btnExport.Enabled = false;
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
        }

        private void btnStartEditor_Click(object sender, EventArgs e)
        {
            Plugin.LaunchEditor();
        }

        private void btnStartCalculator_Click(object sender, EventArgs e)
        {
            Plugin.LaunchCalculator();
        }

        private void btnStartFinder_Click(object sender, EventArgs e)
        {
            Plugin.LaunchFinder();
        }

        private void btnStartRewardCalc_Click(object sender, EventArgs e)
        {
            Plugin.LaunchRewardCalculator();
        }

        private string GetGameString()
        {
            var str = Plugin.GetSavName();
            str = str.Replace("Any", "ScVi");
            str = str.Replace("SL", "Scarlet");
            str = str.Replace("VL", "Violet");
            str = str.Replace("(0)", "(Blank)");
            return str;
        }

        private void btnRemoteConnect_Click(object sender, EventArgs e)
        {
            Connection = Plugin.LaunchConnector(this);
        }
    }
}