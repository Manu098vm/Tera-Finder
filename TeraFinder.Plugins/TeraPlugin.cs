using PKHeX.Core;
using TeraFinder.Core;
using System.Buffers.Binary;
using System.Reflection;

namespace TeraFinder.Plugins;

public class TeraPlugin : IPlugin
{
    public const string Version = "2.7.0";
    private bool UpdatePrompted = false;

    public string Name => nameof(TeraFinder);
    public int Priority => 1;

    public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
    public IPKMView? PKMEditor { get; private set; } = null;

    public SAV9SV SAV = null!;
    public ConnectionForm? Connection = null;
    public string Language = Properties.Settings.Default.def_language;

    public EncounterRaid9[]? Paldea = null;
    public EncounterRaid9[]? Kitakami = null;
    public EncounterRaid9[]? Dist = null;
    public EncounterRaid9[]? Mighty = null;
    public Dictionary<ulong, List<Reward>>? TeraFixedRewards = null;
    public Dictionary<ulong, List<Reward>>? TeraLotteryRewards = null;
    public Dictionary<ulong, List<Reward>>? DistFixedRewards = null;
    public Dictionary<ulong, List<Reward>>? DistLotteryRewards = null;

    private readonly ToolStripMenuItem Plugin = new("Tera Finder Plugins");
    private readonly ToolStripMenuItem Connect = new("Connect to Remote Device");
    private readonly ToolStripMenuItem Editor = new("Tera Raid Viewer/Editor");
    private readonly ToolStripMenuItem Finder = new("Tera Raid Seed Checker");
    private readonly ToolStripMenuItem Flags = new("Edit Game Flags");
    private readonly ToolStripMenuItem Events = new("Import Poké Portal News");
    private readonly ToolStripMenuItem Outbreaks = new("Mass Outbreak Viewer/Editor");

    public void Initialize(params object[] args)
    {
        Plugin.Image = Properties.Resources.icon.ToBitmap();
        SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
        PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
        var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
        NotifySaveLoaded();
        LoadMenuStrip(menu);
        AddCheckerToList();

        if (!UpdatePrompted)
        {
            Task.Run(async () => { await GitHubUtil.TryUpdate(Language); }).Wait();
            UpdatePrompted = true;
        }
    }

    private void AddCheckerToList()
    {
        var menuVSD = (ContextMenuStrip)((dynamic)SaveFileEditor).menu.mnuVSD;            
        menuVSD.Opening += (s, e) => {
            if (SaveFileEditor.SAV is SAV9SV sav) {
                var info = GetSenderInfo(ref s!);
                var pk = info.Slot.Read(sav);
                if (pk is PK9 pk9 && pk9.Met_Location == 30024)
                {
                    var dic = new Dictionary<string, string> { { "CheckerForm", "" } }.TranslateInnerStrings(Language);
                    var calcSeed = new ToolStripMenuItem(dic["CheckerForm"]) { Image = Properties.Resources.icon.ToBitmap() };
                    menuVSD.Items.Insert(menuVSD.Items.Count, calcSeed);
                    calcSeed.Click += (s, e) => new CheckerForm(pk, sav, Language).ShowDialog();
                    menuVSD.Closing += (s, e) => menuVSD.Items.Remove(calcSeed);
                }
            }
        };
    }

    public void StandaloneInitialize(string defaultOT, ReadOnlySpan<byte> data = default, string? language = null)
    {
        if (data != default)
            SAV = new SAV9SV(data.ToArray());
        else
            SAV = new SAV9SV
            {
                Game = (int)GameVersion.SL,
                OT = defaultOT,
                Language = (int)GetLanguageID(language is not null ? language : Language),
            };

        var events = TeraUtil.GetSAVDistEncounters(SAV);
        var terarewards = RewardUtil.GetTeraRewardsTables();
        var eventsrewards = RewardUtil.GetDistRewardsTables(SAV);
        Paldea = TeraUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea);
        Kitakami = TeraUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami);
        Dist = events[0];
        Mighty = events[1];
        TeraFixedRewards = terarewards[0];
        TeraLotteryRewards = terarewards[1];
        DistFixedRewards = eventsrewards[0];
        DistLotteryRewards = eventsrewards[1];
        Language = GetStringLanguage((LanguageID)SAV.Language);

        if (!UpdatePrompted)
        {
            Task.Run(async () => { await GitHubUtil.TryUpdate(Language); }).Wait();
            UpdatePrompted = true;
        }
    }

    public static string GetStringLanguage(LanguageID lang)
    {
        return lang switch
        {
            LanguageID.Japanese => GameLanguage.Language2Char(0),
            LanguageID.English => GameLanguage.Language2Char(1),
            LanguageID.French => GameLanguage.Language2Char(2),
            LanguageID.Italian => GameLanguage.Language2Char(3),
            LanguageID.German => GameLanguage.Language2Char(4),
            LanguageID.Spanish => GameLanguage.Language2Char(5),
            LanguageID.Korean => GameLanguage.Language2Char(6),
            LanguageID.ChineseS => GameLanguage.Language2Char(7),
            LanguageID.ChineseT => GameLanguage.Language2Char(8),
            _ => GameLanguage.Language2Char(1),
        };
    }

    public static LanguageID GetLanguageID(string language) => GetLanguageID(GameLanguage.GetLanguageIndex(language));

    public static LanguageID GetLanguageID(int programLanguage)
    {
        return programLanguage switch
        {
            0 => LanguageID.Japanese,
            1 => LanguageID.English,
            2 => LanguageID.French,
            3 => LanguageID.Italian,
            4 => LanguageID.German,
            5 => LanguageID.Spanish,
            6 => LanguageID.Korean,
            7 => LanguageID.ChineseS,
            8 => LanguageID.ChineseT,
            _ => LanguageID.English,
        };
    }

    public static string GetStringLanguage(int programLanguage) => GameLanguage.Language2Char(programLanguage);

    public static int GetDefaultLanguage() => GameLanguage.GetLanguageIndex(GetDefaultLanguageString());

    public static string GetDefaultLanguageString() => Properties.Settings.Default.def_language;

    public static void SetDefaultLanguage(int programLanguage)
    {
        Properties.Settings.Default.def_language = GetStringLanguage(programLanguage);
        Properties.Settings.Default.Save();
    }

    private void LoadMenuStrip(ToolStrip menuStrip)
    {
        var items = menuStrip.Items;
        if (items.Find("Menu_Tools", false)[0] is not ToolStripDropDownItem tools)
            throw new ArgumentException(nameof(menuStrip));
        AddPluginControl(tools);
    }

    private void AddPluginControl(ToolStripDropDownItem tools)
    {
        Plugin.DropDownItems.Add(Connect);
        Plugin.DropDownItems.Add(Editor);
        Plugin.DropDownItems.Add(Outbreaks);
        Plugin.DropDownItems.Add(Finder);
        Plugin.DropDownItems.Add(Flags);
        Plugin.DropDownItems.Add(Events);
        Connect.Click += (s, e) => LaunchConnector();
        Editor.Click += (s, e) => new EditorForm(SAV, PKMEditor, Language, Paldea, Kitakami, Dist, Mighty, TeraFixedRewards, TeraLotteryRewards, DistFixedRewards, DistLotteryRewards, Connection).ShowDialog();
        Events.Click += (s, e) => ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, ref DistFixedRewards, ref DistLotteryRewards, language: Language, plugin: true);
        Flags.Click += (s, e) => new ProgressForm(SAV, Language,Connection).ShowDialog();
        Finder.Click += (s, e) => new CheckerForm(PKMEditor!.PreparePKM(), SAV, Language).ShowDialog();
        Outbreaks.Click += (s, e) => new OutbreakForm(SAV, Language, Connection).ShowDialog();
        tools.DropDownItems.Add(Plugin);
    }

    private void TranslatePlugins()
    {
        var dic = new Dictionary<string, string>
        {
            { "Plugin.TeraFinderPlugin", "Tera Finder Plugin" },
            { "Plugin.ConnectRemote", "Connect to Remote Device" },
            { "Plugin.TeraViewer", "Tera Raid Viewer/Editor" },
            { "Plugin.SeedChecker", "Tera Raid Seed Checker" },
            { "Plugin.FlagEditor", "Edit Game Flags" },
            { "Plugin.NewsImporter", "Import Poké Portal News" },
            { "Plugin.OutbreakViewer", "Mass Outbreak Viewer/Editor"}
        }.TranslateInnerStrings(Language);

        Plugin.Text = dic["Plugin.TeraFinderPlugin"];
        Connect.Text = dic["Plugin.ConnectRemote"];
        Editor.Text = dic["Plugin.TeraViewer"];
        Finder.Text = dic["Plugin.SeedChecker"];
        Flags.Text = dic["Plugin.FlagEditor"];
        Events.Text = dic["Plugin.NewsImporter"];
        Outbreaks.Text = dic["Plugin.OutbreakViewer"];
    }

    public void LaunchEditor()
    {
        new EditorForm(SAV, PKMEditor, Language, Paldea, Kitakami, Dist, Mighty, TeraFixedRewards, TeraLotteryRewards, DistFixedRewards, DistLotteryRewards, Connection).ShowDialog();
    }

    public void LaunchCalculator()
    {
        var editor = new EditorForm(SAV, PKMEditor, Language, Paldea, Kitakami, Dist, Mighty, TeraFixedRewards, TeraLotteryRewards, DistFixedRewards, DistLotteryRewards, Connection);
        new CalculatorForm(editor).ShowDialog();
    }

    public void LaunchRewardCalculator()
    {
        var editor = new EditorForm(SAV, PKMEditor, Language, Paldea, Kitakami, Dist, Mighty, TeraFixedRewards, TeraLotteryRewards, DistFixedRewards, DistLotteryRewards, Connection);
        new RewardCalcForm(editor).ShowDialog();
    }

    public void LaunchImporter()
    {
        ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, ref DistFixedRewards, ref DistLotteryRewards, language: Language, plugin: true);
    }

    public void LaunchGameEditor()
    {
        new ProgressForm(SAV, Language,Connection).ShowDialog();
    }

    public void LaunchFinder()
    {
        new CheckerForm(new PK9 { TrainerTID7 = SAV.TrainerTID7, TrainerSID7 = SAV.TrainerSID7 }, SAV, Language).ShowDialog();
    }

    public void LaunchMassOutbreakEditor()
    {
        new OutbreakForm(SAV, Language, Connection).ShowDialog();
    }

    public ConnectionForm LaunchConnector(Form? parent = null)
    {
        if (parent is not null)
            parent.Enabled = false;

        var formlist = new List<Form>();
        formlist.AddRange(Application.OpenForms.OfType<EditorForm>());
        formlist.AddRange(Application.OpenForms.OfType<CalculatorForm>());
        formlist.AddRange(Application.OpenForms.OfType<RewardCalcForm>());
        formlist.AddRange(Application.OpenForms.OfType<ProgressForm>());
        formlist.AddRange(Application.OpenForms.OfType<CheckerForm>());
        foreach (var form in formlist)
            form?.Close();

        var con = Connection is null ? new ConnectionForm(SAV, Language) : Connection;
        con.TranslateInterface(Language);
        con.FormClosing += (s, e) =>
        {
            var events = TeraUtil.GetSAVDistEncounters(SAV);
            var eventsrewards = RewardUtil.GetDistRewardsTables(SAV);
            Dist = events[0];
            Mighty = events[1];
            DistFixedRewards = eventsrewards[0];
            DistLotteryRewards = eventsrewards[1];

            if (parent is not null)
            {
                Language = GetStringLanguage((LanguageID)SAV.Language);
                parent.Enabled = true;
            }

            con.Hide();
            e.Cancel = true;
        };

        con.Show();
        Connection = con;
        return con;
    }

    public void NotifySaveLoaded()
    {
        Language = GameInfo.CurrentLanguage;
        TranslatePlugins();
        if (SaveFileEditor.SAV is SAV9SV sav)
        {
            SAV = sav;
            EnablePlugins();
        }
        else
            DisablePlugins();
    }

    public string GetSavName()
    {
        var ot = SAV.OT;
        var game = (GameVersion)SAV.Game;
        var tid = (int)SAV.TrainerTID7;
        return $"{game} - {ot} ({tid}) - {Language.ToUpper()}";
    }

    public uint GetEventIdentifier()
    {
        var block = SAV.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);
        var data = block.Data;
        if (data.Length > 0)
            return BinaryPrimitives.ReadUInt32LittleEndian(data);

        return 0;
    }

    public bool TryLoadFile(string filePath) => ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, ref DistFixedRewards, ref DistLotteryRewards, language: Language, path:filePath);

    private void EnablePlugins() => Plugin.Enabled = true;

    private void DisablePlugins() => Plugin.Enabled = false;


    // Use Reflection To get ContextMenuSAV's private static method GetSenderInfo
    private SlotViewInfo<PictureBox> GetSenderInfo(ref object sender)
    {
        Type contextMenuSAVType = ((dynamic)SaveFileEditor).menu.GetType();
        MethodInfo? getSenderInfoMethod = contextMenuSAVType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .SingleOrDefault(m => m.Name.Contains("GetSenderInfo"));
        return (SlotViewInfo<PictureBox>)getSenderInfoMethod?.Invoke(null, new object[] { sender })!;
    }

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

        var path = sfd.FileName ?? throw new NullReferenceException(nameof(sfd.FileName));
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
