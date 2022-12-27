using PKHeX.Core;

namespace TeraFinder.Forms
{
    public partial class ProgressForm : Form
    {
        SAV9SV SAV = null!;
        List<SevenStarRaidDetail> Raids = null!;

        public ProgressForm(SAV9SV sav)
        {
            InitializeComponent();
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
        }

        private void btnApplyProgress_Click(object sender, EventArgs e)
        {
            if (SAV.Accessor is not null)
            {
                var progress = (GameProgress)cmbProgress.SelectedIndex;

                if (progress >= GameProgress.UnlockedTeraRaids)
                {
                    var dummy = GameProgressBlocks.KUnlockedTeraRaidBattles;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                }
                else
                {
                    var dummy = GameProgressBlocks.KUnlockedTeraRaidBattles;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool1);
                }

                if (progress >= GameProgress.Unlocked3Stars)
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty3;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                }
                else
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty3;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool1);
                }

                if (progress >= GameProgress.Unlocked4Stars)
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty4;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                }
                else
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty4;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool1);
                }

                if (progress >= GameProgress.Unlocked5Stars)
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty5;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                }
                else
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty5;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool1);
                }

                if (progress >= GameProgress.Unlocked6Stars)
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty6;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                }
                else
                {
                    var dummy = GameProgressBlocks.KUnlockedRaidDifficulty6;
                    var block = SAV.Accessor.FindOrDefault(dummy.Key);
                    if (block.Type is SCTypeCode.None)
                    {
                        block = BlockUtil.CreateBoolBlock(dummy.Key, dummy.Type);
                        BlockUtil.AddBlockToFakeSAV(SAV, block);
                    }
                    block.ChangeBooleanType(SCTypeCode.Bool1);
                }

                MessageBox.Show("Done.");
            }
            else
            {
                MessageBox.Show("Not a valid save file");
                this.Close();
            }
        }

        private void btnApplyRaid7_Click(object sender, EventArgs e)
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

            MessageBox.Show("Done.");
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
}
