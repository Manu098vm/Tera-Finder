using PKHeX.Core;
using TeraFinder.Forms;
using Octokit;

namespace TeraFinder
{
    public class TeraPlugin : IPlugin
    {
        private const string Version = "1.0.0";

        public string Name => nameof(TeraFinder);
        public int Priority => 1;

        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView? PKMEditor { get; private set; } = null;

        public SAV9SV SAV = null!;

        private readonly ToolStripMenuItem Plugin = new("Tera Finder Plugin");
        private readonly ToolStripMenuItem Editor = new("Tera Raid Editor");
        private readonly ToolStripMenuItem Finder = new("Tera Raid Seed Finder");
        private readonly ToolStripMenuItem Flags = new("Edit Game Flags");
        private readonly ToolStripMenuItem Events = new("Import Poké Portal News");
        
        public void Initialize(params object[] args)
        {
            TryUpdate();
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            LoadMenuStrip(menu);
            NotifySaveLoaded(); 
        }

        public void StandaloneInitialize(ReadOnlySpan<byte> data = default)
        {
            TryUpdate();
            if (data != default)
                SAV = new SAV9SV(data.ToArray());
            else
                SAV = new();
        }

        public void LaunchEditor()
        {
            new EditorForm(SAV, PKMEditor).Show();
        }

        public void LaunchCalculator()
        {
            var editor = new EditorForm(SAV, PKMEditor);
            new CalculatorForm(editor).Show();
        }

        public void LaunchImporter()
        {
            ImportUtil.ImportNews(SAV, plugin: true);
        }

        public void LaunchGameEditor()
        {
            new ProgressForm(SAV).Show();
        }

        public void LaunchFinder()
        {
            new CheckerForm(new PK9 { TrainerID7 = SAV.TrainerID7, TrainerSID7 = SAV.TrainerSID7 }, SAV).Show();
        }

        public void NotifySaveLoaded()
        {
            if (SaveFileEditor.SAV is SAV9SV)
            {
                SAV = (SAV9SV)SaveFileEditor.SAV;
                EnablePlugins();
            }
            else
                DisablePlugins();
        }

        public string GetSavName()
        {
            var ot = SAV.OT;
            var game = (GameVersion)SAV.Game;
            var tid = SAV.TrainerID7;
            return $"{game} - {ot} ({tid})";
        }

        public bool TryLoadFile(string filePath) => ImportUtil.ImportNews(SAV, filePath);

        public static string GetPluginVersion() => Version;

        public static async Task<string> GetLatestVersion()
        {
            try
            {
                //GitHubClient client = new(new ProductHeaderValue("TeraFinder"));
                //IReadOnlyList<Release>? releases = await client.Repository.Release.GetAll("Manu098vm", "Tera-Finder");
                //if (releases is not null && releases.Count > 0)
                    //return releases[0].TagName;
                //else 
                    return "0.0.0";
            }
            catch (Exception)
            {
                return "0.0.0";
            }
        }

        public static void TryUpdate()
        {
            if (IsUpdateAvailable())
            {
                var result = MessageBox.Show("A Tera Finder update is available. Do you want to update the application?", "Update available", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start("https://github.com/Manu098vm/Tera-Finder/Releases");
            }
        }

        public static bool IsUpdateAvailable()
        {
            var currentVersion = ParseVersion(GetPluginVersion());
            var latestVersion = ParseVersion(GetLatestVersion().Result);

            if (latestVersion[0] > currentVersion[0])
                return true;
            else if(latestVersion[0] == currentVersion[0])
            {
                if (latestVersion[1] > currentVersion[1])
                    return true;
                else if(latestVersion[1] == currentVersion[1])
                {
                    if (latestVersion[2] > currentVersion[2])
                        return true;
                }
            }
            return false;
        }

        private static int[] ParseVersion(string version)
        {
            var v = new int[3];
            v[0] = int.Parse($"{version[0]}");
            v[1] = int.Parse($"{version[2]}");
            v[2] = int.Parse($"{version[4]}");
            return v;
        }

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            if (!(items.Find("Menu_Tools", false)[0] is ToolStripDropDownItem tools))
                throw new ArgumentException(nameof(menuStrip));
            AddPluginControl(tools);
        }

        private void AddPluginControl(ToolStripDropDownItem tools)
        {
            Plugin.DropDownItems.Add(Editor);
            Plugin.DropDownItems.Add(Finder);
            Plugin.DropDownItems.Add(Flags);
            Plugin.DropDownItems.Add(Events);
            Editor.Click += (s, e) => new EditorForm(SAV, PKMEditor).Show();
            Events.Click += (s, e) => ImportUtil.ImportNews(SAV, plugin: true);
            Flags.Click += (s, e) => new ProgressForm(SAV).Show();
            Finder.Click += (s, e) => new CheckerForm(PKMEditor!.PreparePKM(), SAV).Show();
            tools.DropDownItems.Add(Plugin);
        }

        private void EnablePlugins() => Plugin.Enabled = true;

        private void DisablePlugins() => Plugin.Enabled = false;


        //From PKHeX
        //https://github.com/kwsch/PKHeX/blob/master/PKHeX.WinForms/Util/WinFormsUtil.cs
        //GPL V3 license
        public bool ExportSAVDialog(int currentBox = 0)
        {

            using var sfd = new SaveFileDialog
            {
                Filter = SAV.Metadata.Filter,
                FileName = SAV.Metadata.FileName,
                FilterIndex = 1000, // default to last, All Files
                RestoreDirectory = true,
            };

            if (Directory.Exists(SAV.Metadata.FileFolder))
                sfd.InitialDirectory = SAV.Metadata.FileFolder;

            if (sfd.FileName is null || sfd.FileName.Equals(""))
                sfd.FileName = "main";

            if (sfd.ShowDialog() != DialogResult.OK)
                return false;

            // Set box now that we're saving
            if (SAV.HasBox)
                SAV.CurrentBox = currentBox;

            var path = sfd.FileName;
            if (path == null)
                throw new NullReferenceException(nameof(sfd.FileName));

            ExportSAV(SAV, path);
            return true;
        }

        private static void ExportSAV(SaveFile sav, string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var flags = sav.Metadata.GetSuggestedFlags(ext);

            try
            {
                File.WriteAllBytes(path, sav.Write(flags));
                sav.State.Edited = false;
                sav.Metadata.SetExtraInfo(path);
                Alert("SAV exported to:", path);
            }
            catch (Exception x)
            {
                switch (x)
                {
                    case UnauthorizedAccessException:
                    case FileNotFoundException:
                    case IOException:
                        Error("Unable to save file." + Environment.NewLine + x.Message, "If the file is on a removable disk (SD card), please ensure the write protection switch is not set.");
                        break;
                    default:
                        throw;
                }
            }
        }

        internal static DialogResult Alert(params string[] lines) => Alert(true, lines);

        internal static DialogResult Alert(bool sound, params string[] lines)
        {
            if (sound)
                System.Media.SystemSounds.Asterisk.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Alert", MessageBoxButtons.OK, sound ? MessageBoxIcon.Information : MessageBoxIcon.None);
        }

        internal static DialogResult Error(params string[] lines)
        {
            System.Media.SystemSounds.Hand.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
