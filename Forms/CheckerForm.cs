using PKHeX.Core;
using System.Data;

namespace TeraFinder.Forms
{
    public partial class CheckerForm : Form
    {
        private IPKMView Editor = null!;
        private SAV9SV SAV;
        private PK9 PKM = null!;

        public CheckerForm(IPKMView editor, SAV9SV sav)
        {
            InitializeComponent();
            Editor = editor;
            SAV = sav;
            var pkm = (PK9)Editor.PreparePKM();
            txtEC.Text = $"{pkm.EncryptionConstant:X8}";
            txtPID.Text = $"{pkm.PID:X8}";
            numHP.Value = pkm.IV_HP;
            numAtk.Value = pkm.IV_ATK;
            numDef.Value = pkm.IV_DEF;
            numSpA.Value = pkm.IV_SPA;
            numSpD.Value = pkm.IV_SPD;
            numSpe.Value = pkm.IV_SPE;
            cmbNature.SelectedIndex = pkm.Nature;
            cmbTera.SelectedIndex = (int)pkm.TeraTypeOriginal;
            numHeight.Value = pkm.HeightScalar;
            numWeight.Value = pkm.WeightScalar;
            numScale.Value = pkm.Scale;
            PKM = pkm;
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                e.Handled = true;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            var ec = Convert.ToUInt32(txtEC.Text, 16);
            var pid = Convert.ToUInt32(txtPID.Text, 16);
            var hp = (int)numHP.Value;
            var atk = (int)numAtk.Value;
            var def = (int)numDef.Value;
            var spa = (int)numSpA.Value;
            var spd = (int)numSpD.Value;
            var spe = (int)numSpe.Value;
            var nature = cmbNature.SelectedIndex;
            var tera = cmbTera.SelectedIndex;
            var height = (byte)numHeight.Value;
            var weight = (byte)numWeight.Value;
            var scale = (byte)numScale.Value;

            var pk = new PK9(PKM.Data);
            pk.EncryptionConstant = ec;
            pk.PID = pid;
            pk.IV_HP = hp;
            pk.IV_ATK = atk;
            pk.IV_DEF = def;
            pk.IV_SPA = spa;
            pk.IV_SPD = spd;
            pk.IV_SPE = spe;
            pk.Nature = nature;
            pk.TeraTypeOriginal = (MoveType)tera;
            pk.HeightScalar = height;
            pk.WeightScalar = weight;
            pk.Scale = scale;

            var seed = Tera9RNG.GetOriginalSeed(pk);

            for (var content = RaidContent.Standard; content <= RaidContent.Event_Mighty; content++)
            {
                for (var progress = GameProgress.UnlockedTeraRaids; progress < GameProgress.None; progress++)
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var sav = (SAV9SV)SAV.Clone();
                        sav.Game = (int)game;
                        if(!SetProgress(sav, progress))
                        {
                            MessageBox.Show("Invalid save file.");
                            return;
                        }

                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress)) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, content is RaidContent.Event_Mighty, true);

                        if (encounter is not null)
                        {
                            var param = new GenerateParam9
                            {
                                GenderRatio = TeraUtil.GetGender(encounter, content is RaidContent.Event_Mighty),
                                FlawlessIVs = encounter.FlawlessIVCount,
                                RollCount = 1,
                                Height = 0,
                                Weight = 0,
                                Scale = encounter.Scale,
                                Ability = encounter.Ability,
                                Shiny = encounter.Shiny,
                                Nature = encounter.Nature,
                                IVs = encounter.IVs,
                            };
                            var pkm = new PK9
                            {
                                Species = encounter.Species,
                                Form = encounter.Form,
                                TrainerID7 = PKM.TrainerID7,
                                TrainerSID7 = PKM.TrainerSID7,
                                TeraTypeOriginal = (MoveType)Tera9RNG.GetTeraType(seed, encounter.TeraType, encounter.Species, encounter.Form),
                            };

                            Encounter9RNG.GenerateData(pkm, param, EncounterCriteria.Unrestricted, seed);
                            var success = ComparePKM(pk, pkm);
                            if (success)
                            {
                                txtSeed.Text = $"{seed:X8} ({content})";
                                return;
                            }
                        }
                    }
                }
            }
            txtSeed.Text = $"{seed:X8} (INVALID)";
        }

        private static bool ComparePKM(PK9 pk, PK9 pkm)
        {
            if (pk.EncryptionConstant != pkm.EncryptionConstant)
                return false;
            if (pk.PID != pkm.PID)
                return false;
            if (pk.IV_HP != pkm.IV_HP)
                return false;
            if (pk.IV_ATK != pkm.IV_ATK)
                return false;
            if (pk.IV_DEF != pkm.IV_DEF)
                return false;
            if (pk.IV_SPA != pkm.IV_SPA)
                return false;
            if (pk.IV_SPD != pkm.IV_SPD)
                return false;
            if (pk.IV_SPE != pkm.IV_SPE)
                return false;
            if (pk.Nature != pkm.Nature)
                return false;
            if (pk.TeraTypeOriginal != pkm.TeraTypeOriginal)
                return false;
            if (pk.HeightScalar != pkm.HeightScalar)
                return false;
            if (pk.WeightScalar != pkm.WeightScalar)
                return false;
            if (pk.Scale != pkm.Scale)
                return false;

            return true;
        }

        private bool SetProgress(SAV9SV sav, GameProgress progress)
        {
            if (sav.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault() is not null)
            {
                if (progress >= GameProgress.UnlockedTeraRaids)
                    sav.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    sav.AllBlocks.Where(block => block.Key == 0x27025EBF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked3Stars)
                    sav.AllBlocks.Where(block => block.Key == 0xEC95D8EF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    sav.AllBlocks.Where(block => block.Key == 0xEC95D8EF).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked4Stars)
                    sav.AllBlocks.Where(block => block.Key == 0xA9428DFE).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    sav.AllBlocks.Where(block => block.Key == 0xA9428DFE).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked5Stars)
                    sav.AllBlocks.Where(block => block.Key == 0x9535F471).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    sav.AllBlocks.Where(block => block.Key == 0x9535F471).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                if (progress >= GameProgress.Unlocked6Stars)
                    sav.AllBlocks.Where(block => block.Key == 0x6E7F8220).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    sav.AllBlocks.Where(block => block.Key == 0x6E7F8220).FirstOrDefault()!.ChangeBooleanType(SCTypeCode.Bool1);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
