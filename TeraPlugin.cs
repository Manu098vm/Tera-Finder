using PKHeX.Core;
using TeraFinder.Forms;

namespace TeraFinder
{
    public class TeraPlugin : IPlugin
    {
        public const string Version = "0.0.0";

        public string Name => nameof(TeraFinder);
        public int Priority => 1;

        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView? PKMEditor { get; private set; } = null;

        public SAV9SV SAV = null!;
        public EncounterRaid9[]? Tera = null;
        public EncounterRaid9[]? Dist = null;
        public EncounterRaid9[]? Mighty = null;

        private readonly ToolStripMenuItem Plugin = new("Tera Finder Plugin");
        private readonly ToolStripMenuItem Editor = new("Tera Raid Viewer/Editor");
        private readonly ToolStripMenuItem Finder = new("Tera Raid Seed Finder");
        private readonly ToolStripMenuItem Flags = new("Edit Game Flags");
        private readonly ToolStripMenuItem Events = new("Import Poké Portal News");
        
        public void Initialize(params object[] args)
        {
            var task = Task.Run(async () => { await GitHubUtil.TryUpdate(); });
            task.Wait();
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            LoadMenuStrip(menu);
            NotifySaveLoaded(); 
        }

        public void StandaloneInitialize(ReadOnlySpan<byte> data = default)
        {
            var task = Task.Run(async () => { await GitHubUtil.TryUpdate(); });
            task.Wait();
            if (data != default)
                SAV = new SAV9SV(data.ToArray());
            else
                SAV = new();
            Tera = TeraUtil.GetAllTeraEncounters();
            var events = TeraUtil.GetSAVDistEncounters(SAV);
            Dist = events[0];
            Mighty = events[1];
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
            Editor.Click += (s, e) => new EditorForm(SAV, PKMEditor, Tera, Dist, Mighty).Show();
            Events.Click += (s, e) => ImportUtil.ImportNews(SAV, plugin: true);
            Flags.Click += (s, e) => new ProgressForm(SAV).Show();
            Finder.Click += (s, e) => new CheckerForm(PKMEditor!.PreparePKM(), SAV).Show();
            tools.DropDownItems.Add(Plugin);
        }

        public void LaunchEditor()
        {
            new EditorForm(SAV, PKMEditor, Tera, Dist, Mighty).Show();
        }

        public void LaunchCalculator()
        {
            var editor = new EditorForm(SAV, PKMEditor, Tera, Dist, Mighty);
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
