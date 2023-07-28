using PKHeX.Core;
using System.CodeDom.Compiler;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class ProgressForm : Form
{
    SAV9SV SAV = null!;
    List<SevenStarRaidDetail> Raids = null!;
    private Dictionary<string, string> Strings = null!;
    private ConnectionForm? Connection = null;
    public ProgressForm(SAV9SV sav, string language, ConnectionForm connection)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(language);
        TranslateCmbProgress();
        this.TranslateInterface(language);

        SAV = sav;
        Raids = new();

        cmbProgress.SelectedIndex = (int)TeraUtil.GetProgress(sav);
        var raid7 = sav.RaidSevenStar.GetAllRaids();
        foreach (var raid in raid7) {
            if (raid.Identifier > 0) {
                var name = $"{raid.Identifier}";
                cmbMightyIndex.Items.Add(name);
                Raids.Add(raid);
            }
        }

        if (Raids.Count == 0)
            grpRaidMighty.Enabled = false;
        else
            cmbMightyIndex.SelectedIndex = 0;
        Connection = connection;
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
            { "MsgSuccess", "Done." }
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
        if (SAV.Accessor is not null)
        {
            var progress = (GameProgress)cmbProgress.SelectedIndex;
            EditProgress(SAV, progress);
            if(Connection is not null && Connection.IsConnected())
               await WriteProgressLive(SAV,progress);
            MessageBox.Show(Strings["MsgSuccess"]);
        }
        else
        {
            MessageBox.Show(Strings["SAVInvalid"]);
            this.Close();
        }
    }
    private async Task WriteProgressLive(SAV9SV sav, GameProgress progress)
    {
        if (progress >= GameProgress.Unlocked3Stars)
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty3, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, Blocks.KUnlockedRaidDifficulty3, CancellationToken.None, toexpect);
        }
        else
        {

            bool toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty3, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, Blocks.KUnlockedRaidDifficulty3, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked4Stars)
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty4, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, Blocks.KUnlockedRaidDifficulty4, CancellationToken.None, toexpect);
        }
        else
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty4, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, Blocks.KUnlockedRaidDifficulty4, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked5Stars)
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty5, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, Blocks.KUnlockedRaidDifficulty5, CancellationToken.None, toexpect);
        }
        else
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty5, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, Blocks.KUnlockedRaidDifficulty5, CancellationToken.None, toexpect);
        }

        if (progress >= GameProgress.Unlocked6Stars)
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty6, CancellationToken.None);
            await Connection.Executor.WriteBlock(true, Blocks.KUnlockedRaidDifficulty6, CancellationToken.None, toexpect);
        }
        else
        {

            var toexpect = (bool)await Connection.Executor.ReadBlock(Blocks.KUnlockedRaidDifficulty6, CancellationToken.None);
            await Connection.Executor.WriteBlock(false, Blocks.KUnlockedRaidDifficulty6, CancellationToken.None, toexpect);
        }
    }
    public static void EditProgress(SAV9SV sav, GameProgress progress)
    {
        if (progress >= GameProgress.UnlockedTeraRaids)
        {
            var dummy = Blocks.KUnlockedTeraRaidBattles;
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
            var dummy = Blocks.KUnlockedTeraRaidBattles;
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
            var dummy = Blocks.KUnlockedRaidDifficulty3;
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
            var dummy = Blocks.KUnlockedRaidDifficulty3;
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
            var dummy = Blocks.KUnlockedRaidDifficulty4;
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
            var dummy = Blocks.KUnlockedRaidDifficulty4;
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
            var dummy = Blocks.KUnlockedRaidDifficulty5;
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
            var dummy = Blocks.KUnlockedRaidDifficulty5;
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
            var dummy = Blocks.KUnlockedRaidDifficulty6;
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
            var dummy = Blocks.KUnlockedRaidDifficulty6;
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
        var raid = Raids.ElementAt(cmbMightyIndex.SelectedIndex);
        if (chkCaptured.Checked)
            raid.Captured = true;
        else
            raid.Captured = false;
        if (chkDefeated.Checked)
            raid.Defeated = true;
        else
            raid.Defeated = false;
        var success = true;
        if (Connection is not null && Connection.IsConnected())
        {
            success = await Connection.Executor.WriteBlock(SAV.RaidSevenStar.Data, Blocks.RaidSevenStar, CancellationToken.None).ConfigureAwait(false);
        }
        if (success)
            MessageBox.Show(Strings["MsgSuccess"]);
        else
            MessageBox.Show("failed");
    }

    private void cmbMightyIndex_IndexChanged(object sender, EventArgs e)
    {
        var raid = Raids.ElementAt(cmbMightyIndex.SelectedIndex);
        if (raid.Captured)
            chkCaptured.Checked = true;
        else
            chkCaptured.Checked = false;
        if (raid.Defeated)
            chkDefeated.Checked = true;
        else
            chkDefeated.Checked = false;
    }
}
