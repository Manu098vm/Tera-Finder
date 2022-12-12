using PKHeX.Core;
using TeraFinder.Forms;

namespace TeraFinder
{
    public class TeraPlugin : IPlugin
    {
        private const string Version = "1.0.0";

        public string Name => nameof(TeraFinder);
        public int Priority => 1;

        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView PKMEditor { get; private set; } = null!;

        private ToolStripMenuItem Plugin = new("Tera Raid Plugin");
        private ToolStripMenuItem Editor = new("Tera Raid Editor");
        private ToolStripMenuItem Finder = new("Tera Raid Seed Finder");
        private ToolStripMenuItem Flags = new("Edit Game Flags");
        private ToolStripMenuItem Events = new("Import Poké Portal News");
        
        public void Initialize(params object[] args)
        {
            if (IsUpdateAvailable())
                MessageBox.Show("Update available!");

            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            LoadMenuStrip(menu);
            NotifySaveLoaded(); 
        }

        public bool TryLoadFile(string filePath) =>
            ImportUtil.ImportNews((SAV9SV)SaveFileEditor.SAV, filePath);

        public void NotifySaveLoaded()
        {
            if (SaveFileEditor.SAV is SAV9SV)
                EnablePlugins();
            else
                DisablePlugins();
        }

        public static string GetPluginVersion() => Version;

        private static string GetLatestVersion() => "1.0.0"; //Dummy

        private bool IsUpdateAvailable()
        {
            var currentVersion = ParseVersion(GetPluginVersion());
            var latestVersion = ParseVersion(GetLatestVersion());

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
            Editor.Click += (s, e) => new EditorForm((SAV9SV)SaveFileEditor.SAV, PKMEditor).Show();
            Events.Click += (s, e) => ImportUtil.ImportNews(SaveFileEditor.SAV, plugin: true);
            Flags.Click += (s, e) => new ProgressForm((SAV9SV)SaveFileEditor.SAV).Show();
            Finder.Click += (s, e) => new CheckerForm(PKMEditor, (SAV9SV)SaveFileEditor.SAV).Show();
            tools.DropDownItems.Add(Plugin);
        }

        private void EnablePlugins() => Plugin.Enabled = true;

        private void DisablePlugins() => Plugin.Enabled = false;
    }
}
