using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class ProgressForm : Form
{
    readonly SAV9SV SAV = null!;
    readonly List<SevenStarRaidDetail> Raids = null!;
    private Dictionary<string, string> Strings = null!;
    private ConnectionForm? Connection = null;

    private string[] NameList = null!;
    private string[] FormList = null!;
    private string[] TypeList = null!;
    private string[] GenderListAscii = null!;

    public ProgressForm(TeraPlugin container)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(container.Language);
        TranslateCmbProgress();
        this.TranslateInterface(container.Language);

        Connection = container.Connection;
        SAV = container.SAV;
        Raids = [];

        NameList = GameInfo.GetStrings(container.Language).specieslist;
        FormList = GameInfo.GetStrings(container.Language).forms;
        TypeList = GameInfo.GetStrings(container.Language).types;
        GenderListAscii = [.. GameInfo.GenderSymbolASCII];

        cmbProgress.SelectedIndex = (int)SavUtil.GetProgress(SAV);
        var raid7 = SAV.RaidSevenStar.GetAllRaids();
        foreach (var raid in raid7)
        {
            if (raid.Identifier > 0)
            {
                var name = $"{raid.Identifier}";
                var key = Convert.ToUInt32(name[..^2], 10);

                if (container.AllMighty.TryGetValue(key, out var mighties))
                {
                    var enc = mighties.FirstOrDefault(e => e.Identifier == raid.Identifier);
                    if (enc is not null)
                        name = GetEncounterName(enc);
                }
                else if (container.AllDist.TryGetValue(key, out var dists))
                {
                    var enc = dists.FirstOrDefault(e => e.Identifier == raid.Identifier);
                    if (enc is not null)
                        name = GetEncounterName(enc);
                }

                cmbMightyIndex.Items.Add(name);
                Raids.Add(raid);
            }
        }

        if (Raids.Count == 0)
            grpRaidMighty.Enabled = false;
        else
            cmbMightyIndex.SelectedIndex = 0;
    }

    private string GetEncounterName(EncounterEventTF9 enc)
    {
        var forms = FormConverter.GetFormList(enc.Species, TypeList, FormList, GenderListAscii, EntityContext.Gen9);
        var speciesName = $"{NameList[enc.Species]}{(forms.Length > 1 && enc.Form != 0 ? $"-{forms[enc.Form]}" : "")}";

        var encName = enc switch
        {
            { IsMighty: true, Shiny: Shiny.Always or Shiny.AlwaysSquare or Shiny.AlwaysStar } => Strings["ProgressForm.SpeciesMighty"].Replace("{species}", Strings["ProgressForm.SpeciesShiny"]),
            { IsMighty: true } => Strings["ProgressForm.SpeciesMighty"],
            { Shiny: Shiny.Always or Shiny.AlwaysSquare or Shiny.AlwaysStar } => Strings["ProgressForm.SpeciesShiny"],
            _ => "{species}"
        };

        return encName.Replace("{species}", speciesName);
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "GameProgress.Beginning", "Beginning" },
            { "GameProgress.UnlockedTeraRaids", "Unlocked Tera Raids" },
            { "GameProgress.Unlocked3Stars", "Unlocked 3 Stars" },
            { "GameProgress.Unlocked4Stars", "Unlocked 4 Stars" },
            { "GameProgress.Unlocked5Stars", "Unlocked 5 Stars" },
            { "GameProgress.Unlocked6Stars", "Unlocked 6 Stars" },
            { "SAVInvalid", "Not a valid save file." },
            { "MsgSuccess", "Done." },
            { "DisconnectionSuccess", "Device disconnected." },
            { "ProgressForm.SpeciesMighty", "Mighty {species}" },
            { "ProgressForm.SpeciesShiny", "Shiny {species}" }
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

    private void TranslateCmbProgress()
    {
        cmbProgress.Items[0] = Strings["GameProgress.Beginning"];
        cmbProgress.Items[1] = Strings["GameProgress.UnlockedTeraRaids"];
        cmbProgress.Items[2] = Strings["GameProgress.Unlocked3Stars"];
        cmbProgress.Items[3] = Strings["GameProgress.Unlocked4Stars"];
        cmbProgress.Items[4] = Strings["GameProgress.Unlocked5Stars"];
        cmbProgress.Items[5] = Strings["GameProgress.Unlocked6Stars"];
    }

    private async void btnApplyProgress_Click(object sender, EventArgs e)
    {
        var failed = false;
        if (SAV.Accessor is not null)
        {
            var progress = (GameProgress)cmbProgress.SelectedIndex;
            EditProgress(SAV, progress);
            if (Connection is not null && Connection.IsConnected())
            {
                try
                {
                    await WriteProgressLive(progress);
                }
                catch
                {
                    if (Connection is not null)
                    {
                        failed = true;
                        Connection.Disconnect();
                        MessageBox.Show(Strings["DisconnectionSuccess"]);
                        Connection = null;
                    }
                }
            }
        }
        else
        {

            failed = true;
            MessageBox.Show(Strings["SAVInvalid"]);
            Close();
        }

        if (!failed)
            MessageBox.Show(Strings["MsgSuccess"]);
    }

    private async Task WriteProgressLive(GameProgress progress)
    {
        if (Connection is null)
            return;

        if (progress >= GameProgress.Unlocked3Stars)
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty3, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, BlockDefinitions.KUnlockedRaidDifficulty3, CancellationToken.None, toexpect);
        }
        else
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty3, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, BlockDefinitions.KUnlockedRaidDifficulty3, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked4Stars)
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty4, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, BlockDefinitions.KUnlockedRaidDifficulty4, CancellationToken.None, toexpect);
        }
        else
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty4, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, BlockDefinitions.KUnlockedRaidDifficulty4, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked5Stars)
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty5, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, BlockDefinitions.KUnlockedRaidDifficulty5, CancellationToken.None, toexpect);
        }
        else
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty5, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, BlockDefinitions.KUnlockedRaidDifficulty5, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked6Stars)
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty6, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, BlockDefinitions.KUnlockedRaidDifficulty6, CancellationToken.None, toexpect);
        }
        else
        {
            var toexpect = (bool?)await Connection.Executor.ReadBlock(BlockDefinitions.KUnlockedRaidDifficulty6, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, BlockDefinitions.KUnlockedRaidDifficulty6, CancellationToken.None, toexpect);
        }
    }

    public static void EditProgress(SAV9SV sav, GameProgress progress)
    {
        if (progress >= GameProgress.UnlockedTeraRaids)
        {
            var dummy = BlockDefinitions.KUnlockedTeraRaidBattles;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool2);
        }
        else
        {
            var dummy = BlockDefinitions.KUnlockedTeraRaidBattles;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool1);
        }

        if (progress >= GameProgress.Unlocked3Stars)
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty3;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool2);
        }
        else
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty3;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool1);
        }

        if (progress >= GameProgress.Unlocked4Stars)
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty4;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool2);
        }
        else
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty4;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool1);
        }

        if (progress >= GameProgress.Unlocked5Stars)
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty5;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool2);
        }
        else
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty5;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool1);
        }

        if (progress >= GameProgress.Unlocked6Stars)
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty6;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool2);
        }
        else
        {
            var dummy = BlockDefinitions.KUnlockedRaidDifficulty6;
            var block = sav.Accessor.FindOrDefault(dummy.Key);
            if (block.Type is SCTypeCode.None)
            {
                block = BlockUtil.CreateDummyBlock(dummy.Key, dummy.Type);
                BlockUtil.AddBlockToFakeSAV(sav, block);
            }
            block.ChangeBooleanType(SCTypeCode.Bool1);
        }
    }

    private async void btnApplyRaid7_Click(object sender, EventArgs e)
    {
        var failed = false;
        var raid = Raids.ElementAt(cmbMightyIndex.SelectedIndex);
        if (chkCaptured.Checked)
            raid.Captured = true;
        else
            raid.Captured = false;

        if (Connection is not null && Connection.IsConnected())
        {
            try
            {
                await Connection.Executor.WriteBlock(SAV.RaidSevenStar.Captured.Data.ToArray(), BlockDefinitions.KSevenStarRaidsCapture, CancellationToken.None).ConfigureAwait(false);
            }
            catch
            {
                if (Connection is not null)
                {
                    failed = true;
                    Connection.Disconnect();
                    MessageBox.Show(Strings["DisconnectionSuccess"]);
                    Connection = null;
                }
            }
        }

        if (!failed)
            MessageBox.Show(Strings["MsgSuccess"]);
    }

    private void cmbMightyIndex_IndexChanged(object sender, EventArgs e)
    {
        var raid = Raids.ElementAt(cmbMightyIndex.SelectedIndex);
        if (raid.Captured)
            chkCaptured.Checked = true;
        else
            chkCaptured.Checked = false;
    }
}