using PKHeX.Core;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.SV;
using TeraFinder.Core;
using System.Buffers.Binary;
using System.Reflection;

namespace TeraFinder.Plugins;

public class TeraPlugin : IPlugin
{
    public const string Version = "4.0.3";
    private bool UpdatePrompted = false;

    public string Name => nameof(TeraFinder);
    public int Priority => 1;

    public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
    public IPKMView? PKMEditor { get; private set; } = null;

    public SAV9SV SAV = null!;
    public ConnectionForm? Connection = null;
    public string Language = Properties.Settings.Default.def_language;

    public EncounterTeraTF9[]? Paldea = null;
    public EncounterTeraTF9[]? PaldeaBlack = null;
    public EncounterTeraTF9[]? Kitakami = null;
    public EncounterTeraTF9[]? KitakamiBlack = null;
    public EncounterTeraTF9[]? Blueberry = null;
    public EncounterTeraTF9[]? BlueberryBlack = null;
    public EncounterEventTF9[]? Dist = null;
    public EncounterEventTF9[]? Mighty = null;

    public Dictionary<uint, HashSet<EncounterEventTF9>>? AllDist = null;
    public Dictionary<uint, HashSet<EncounterEventTF9>>? AllMighty = null;

    private readonly ToolStripMenuItem Plugin = new("Tera Finder Plugins");
    private readonly ToolStripMenuItem Connect = new("Connect to Remote Device");
    private readonly ToolStripMenuItem Editor = new("Tera Raid Viewer/Editor");
    private readonly ToolStripMenuItem RaidCalculator = new("Raid Calculator");
    private readonly ToolStripMenuItem RewardCalculator = new("Reward Calculator");
    private readonly ToolStripMenuItem Finder = new("Tera Raid Seed Checker");
    private readonly ToolStripMenuItem Flags = new("Edit Game Flags");
    private readonly ToolStripMenuItem Outbreaks = new("Mass Outbreak Viewer/Editor");
    private readonly ToolStripMenuItem Events = new("Import Poké Portal News");
    private readonly ToolStripMenuItem ImportNews = new("Import from files...");
    private readonly ToolStripMenuItem NullRaid = new("Import Empty (Null) Raid Event");
    private readonly ToolStripMenuItem NullOutbreak = new("Import Empty (Null) Outbreak Event");

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
        const ushort TeraLocation = 30024;

        if (Paldea is null || PaldeaBlack is null)
            (Paldea, PaldeaBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea);

        if (Kitakami is null || KitakamiBlack is null)
            (Kitakami, KitakamiBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami);

        if (Blueberry is null || BlueberryBlack is null)
            (Blueberry, BlueberryBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry);

        if (AllDist is null || AllMighty is null)
        {
            var (dist, mighty) = ResourcesUtil.GetAllEventEncounters();
            AllDist = SeedCheckerUtil.GroupEventEncounters(dist);
            AllMighty = SeedCheckerUtil.GroupEventEncounters(mighty);
        }

        var menuVSD = (ContextMenuStrip)((dynamic)SaveFileEditor).menu.mnuVSD;            
        menuVSD.Opening += (s, e) => {
            if (SaveFileEditor.SAV is SAV9SV sav) {
                var info = GetSenderInfo(ref s!);
                var pk = info.Slot.Read(sav);
                if (pk is PK9 pk9 && pk9.MetLocation == TeraLocation)
                {
                    var dic = new Dictionary<string, string> { { "CheckerForm", "" } }.TranslateInnerStrings(Language);
                    var calcSeed = new ToolStripMenuItem(dic["CheckerForm"]) { Image = Properties.Resources.icon.ToBitmap() };
                    menuVSD.Items.Insert(menuVSD.Items.Count, calcSeed);
                    calcSeed.Click += (s, e) => new CheckerForm(pk, Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, AllDist, AllMighty).ShowDialog();
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
                Version = GameVersion.SL,
                OT = defaultOT,
                Language = (int)GetLanguageID(language is not null ? language : Language),
            };

        (Paldea, PaldeaBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea);
        (Kitakami, KitakamiBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami);
        (Blueberry, BlueberryBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry);
        (Dist, Mighty) = EventUtil.GetCurrentEventEncounters(SAV, RewardUtil.GetDistRewardsTables(SAV));

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
            throw new ArgumentException(null, nameof(menuStrip));
        AddPluginControl(tools);
    }

    private void AddPluginControl(ToolStripDropDownItem tools)
    {
        Plugin.DropDownItems.Add(Connect);
        Plugin.DropDownItems.Add(Editor);
        Plugin.DropDownItems.Add(RaidCalculator);
        Plugin.DropDownItems.Add(RewardCalculator);
        Plugin.DropDownItems.Add(Outbreaks);
        Plugin.DropDownItems.Add(Finder);
        Plugin.DropDownItems.Add(Flags);
        Events.DropDownItems.Add(ImportNews);
        Events.DropDownItems.Add(NullRaid);
        Events.DropDownItems.Add(NullOutbreak);
        Plugin.DropDownItems.Add(Events);
        Connect.Click += (s, e) => LaunchConnector();
        Editor.Click += (s, e) => new EditorForm(SAV, PKMEditor, Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, Dist, Mighty, Connection).Show();
        RaidCalculator.Click += (s, e) => LaunchCalculator(true);
        RewardCalculator.Click += (s, e) => LaunchRewardCalculator(true);
        ImportNews.Click += (s, e) => ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, language: Language, plugin: true);
        NullRaid.Click += (s, e) => LaunchRaidNullImporter();
        NullOutbreak.Click += (s, e) => LaunchOutbreakNullImporter();
        Flags.Click += (s, e) => new ProgressForm(SAV, Language,Connection).ShowDialog();
        Finder.Click += (s, e) => AddSeedCheckerPluginControl();
        Outbreaks.Click += (s, e) => new OutbreakForm(SAV, Language, Connection).ShowDialog();
        tools.DropDownItems.Add(Plugin);
    }

    private void AddSeedCheckerPluginControl()
    {
        if (Paldea is null || PaldeaBlack is null)
            (Paldea, PaldeaBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea);

        if (Kitakami is null || KitakamiBlack is null)
            (Kitakami, KitakamiBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami);

        if (Blueberry is null || BlueberryBlack is null)
            (Blueberry, BlueberryBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry);

        if (AllDist is null || AllMighty is null)
        {
            var (dist, mighty) = ResourcesUtil.GetAllEventEncounters();
            AllDist = SeedCheckerUtil.GroupEventEncounters(dist);
            AllMighty = SeedCheckerUtil.GroupEventEncounters(mighty);
        }

        new CheckerForm(PKMEditor!.PreparePKM(), Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, AllDist, AllMighty).ShowDialog();
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
            { "Plugin.OutbreakViewer", "Mass Outbreak Viewer/Editor" },
            { "Plugin.ImportNews", "Import from files..." },
            { "Plugin.OutbreakNull", "Import Empty (Null) Outbreak Event" },
            { "Plugin.RaidNull", "Import Empty (Null) Raid Event" },
            { "Plugin.RaidCalculator", "Raid Calculator" },
            { "Plugin.RewardCalculator", "Reward Calculator" },
        }.TranslateInnerStrings(Language);

        Plugin.Text = dic["Plugin.TeraFinderPlugin"];
        Connect.Text = dic["Plugin.ConnectRemote"];
        Editor.Text = dic["Plugin.TeraViewer"];
        RaidCalculator.Text = dic["Plugin.RaidCalculator"];
        RewardCalculator.Text = dic["Plugin.RewardCalculator"];
        Finder.Text = dic["Plugin.SeedChecker"];
        Flags.Text = dic["Plugin.FlagEditor"];
        Events.Text = dic["Plugin.NewsImporter"];
        Outbreaks.Text = dic["Plugin.OutbreakViewer"];
        ImportNews.Text = dic["Plugin.ImportNews"];
        NullOutbreak.Text = dic["Plugin.OutbreakNull"];
        NullRaid.Text = dic["Plugin.RaidNull"];
    }

    public void LaunchEditor()
    {
        new EditorForm(SAV, PKMEditor, Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, Dist, Mighty, Connection).ShowDialog();
    }

    public void LaunchCalculator(bool plugin = false)
    {
        var editor = new EditorForm(SAV, PKMEditor, Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, Dist, Mighty, Connection);
        if (plugin)
            new CalculatorForm(editor).Show();
        else
            new CalculatorForm(editor).ShowDialog();
    }

    public void LaunchRewardCalculator(bool plugin = false)
    {
        var editor = new EditorForm(SAV, PKMEditor, Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, Dist, Mighty, Connection);
        if (plugin)
            new RewardCalcForm(editor).Show();
        else
            new RewardCalcForm(editor).ShowDialog();
    }

    public void LaunchImporter()
    {
        ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, language: Language, plugin: true);
    }

    public void LaunchRaidNullImporter()
    {
        ImportUtil.FinalizeImportRaid(SAV, Properties.Resources.event_raid_identifier, Properties.Resources.fixed_reward_item_array,
            Properties.Resources.lottery_reward_item_array, Properties.Resources.raid_enemy_array, Properties.Resources.raid_priority_array, "0",
            ref Dist, ref Mighty, ImportUtil.GenerateDictionary().TranslateInnerStrings(Language));
    }

    public void LaunchOutbreakNullImporter()
    {
        ImportUtil.FinalizeImportOutbreak(SAV, SAV.SaveRevision >= 2, Properties.Resources.pokedata_array_3_0_0, Properties.Resources.zone_main_array_3_0_0,
            Properties.Resources.zone_su1_array_3_0_0, Properties.Resources.zone_su2_array_3_0_0, "0", ImportUtil.GenerateDictionary().TranslateInnerStrings(Language));
    }

    public void LaunchGameEditor()
    {
        new ProgressForm(SAV, Language,Connection).ShowDialog();
    }

    public void LaunchSeedChecker()
    {
        if (Paldea is null || PaldeaBlack is null)
            (Paldea, PaldeaBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Paldea);

        if (Kitakami is null || KitakamiBlack is null)
            (Kitakami, KitakamiBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Kitakami);

        if (Blueberry is null || BlueberryBlack is null)
            (Blueberry, BlueberryBlack) = ResourcesUtil.GetAllTeraEncounters(TeraRaidMapParent.Blueberry);

        if (AllDist is null || AllMighty is null)
        {
            var (dist, mighty) = ResourcesUtil.GetAllEventEncounters();
            AllDist = SeedCheckerUtil.GroupEventEncounters(dist);
            AllMighty = SeedCheckerUtil.GroupEventEncounters(mighty);
        }

        new CheckerForm(new PK9 { TrainerTID7 = SAV.TrainerTID7, TrainerSID7 = SAV.TrainerSID7 }, 
            Language, Paldea, PaldeaBlack, Kitakami, KitakamiBlack, Blueberry, BlueberryBlack, AllDist, AllMighty).ShowDialog();
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
            (Dist, Mighty) = EventUtil.GetCurrentEventEncounters(SAV, RewardUtil.GetDistRewardsTables(SAV));
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

    public void NotifyDisplayLanguageChanged(string language)
    {
        Language = language;
        TranslatePlugins();
    }

    public string GetSavName()
    {
        var ot = SAV.OT;
        var game = SAV.Version;
        var tid = (int)SAV.TrainerTID7;
        return $"{game} - {ot} ({tid}) - {Language.ToUpper()}";
    }

    public uint GetRaidEventIdentifier()
    {
        var block = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATEventRaidIdentifier.Key);
        var data = block.Data;
        if (data.Length > 0)
            return BinaryPrimitives.ReadUInt32LittleEndian(data);

        return 0;
    }

    public uint GetOutbreakEventIdentifier()
    {
        var enableBlock = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATOutbreakEnabled.Key);
        if (enableBlock.Type == SCTypeCode.None || enableBlock.Type == SCTypeCode.Bool1)
            return 0;
        var pokeDataBlock = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATOutbreakPokeData.Key);
        var tablePokeData = FlatBufferConverter.DeserializeFrom<DeliveryOutbreakPokeDataArray>(pokeDataBlock.Data);
        return tablePokeData.Table[0].ID > 0 ? uint.Parse($"{tablePokeData.Table[0].ID}"[..8]) : 0;
    }

    public bool TryLoadFile(string filePath) => ImportUtil.ImportNews(SAV, ref Dist, ref Mighty, language: Language, path:filePath);

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
