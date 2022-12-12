using PKHeX.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeraRaidEditor.Forms
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
            if (SAV.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault() is not null)
            {
                var progress = (GameProgress)cmbProgress.SelectedIndex;

                if (progress >= GameProgress.UnlockedTeraRaids)
                    SAV.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    SAV.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked3Stars)
                    SAV.AllBlocks.Where(block => block.Key == 0xEC95D8EF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    SAV.AllBlocks.Where(block => block.Key == 0xEC95D8EF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked4Stars)
                    SAV.AllBlocks.Where(block => block.Key == 0xA9428DFE).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    SAV.AllBlocks.Where(block => block.Key == 0xA9428DFE).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked5Stars)
                    SAV.AllBlocks.Where(block => block.Key == 0x9535F471).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    SAV.AllBlocks.Where(block => block.Key == 0x9535F471).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked6Stars)
                    SAV.AllBlocks.Where(block => block.Key == 0x6E7F8220).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    SAV.AllBlocks.Where(block => block.Key == 0x6E7F8220).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

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
