﻿using PKHeX.Core;
using System.Data;

namespace TeraFinder.Forms
{
    public partial class CheckerForm : Form
    {
        private PK9 PKM = null!;
        private SAV9SV SAV;

        public CheckerForm(PKM pk, SAV9SV sav)
        {
            InitializeComponent();
            SAV = sav;
            PKM = (PK9)pk;
            txtTid.Text = $"{PKM.TrainerID7}";
            txtSid.Text = $"{PKM.TrainerSID7}";
            txtEC.Text = $"{PKM.EncryptionConstant:X8}";
            txtPID.Text = $"{PKM.PID:X8}";
            numHP.Value = PKM.IV_HP;
            numAtk.Value = PKM.IV_ATK;
            numDef.Value = PKM.IV_DEF;
            numSpA.Value = PKM.IV_SPA;
            numSpD.Value = PKM.IV_SPD;
            numSpe.Value = PKM.IV_SPE;
            cmbNature.SelectedIndex = PKM.Nature;
            cmbTera.SelectedIndex = (int)PKM.TeraTypeOriginal;
            numHeight.Value = PKM.HeightScalar;
            numWeight.Value = PKM.WeightScalar;
            numScale.Value = PKM.Scale;
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                e.Handled = true;
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            var c = e.KeyChar;
            if (!char.IsControl(e.KeyChar) && !(c >= '0' && c <= '9'))
                e.Handled = true;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            var tid = Convert.ToInt32(txtTid.Text);
            var sid = Convert.ToInt32(txtSid.Text);
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
                        SetProgress(sav, progress);

                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress)) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, content is RaidContent.Event_Mighty, true);

                        if (encounter is not null)
                        {
                            var rngres = TeraUtil.CalcRNG(seed, tid, sid, content, encounter);
                            var success = ComparePKM(pk, rngres);
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

        private static bool ComparePKM(PK9 pk, TeraDetails pkm)
        {
            if (pk.EncryptionConstant != pkm.EC)
                return false;
            if (pk.PID != pkm.PID)
                return false;
            if (pk.IV_HP != pkm.HP)
                return false;
            if (pk.IV_ATK != pkm.ATK)
                return false;
            if (pk.IV_DEF != pkm.DEF)
                return false;
            if (pk.IV_SPA != pkm.SPA)
                return false;
            if (pk.IV_SPD != pkm.SPD)
                return false;
            if (pk.IV_SPE != pkm.SPE)
                return false;
            if (pk.Nature != (int)pkm.Nature)
                return false;
            if (pk.TeraTypeOriginal != pkm.TeraType)
                return false;
            if (pk.HeightScalar != pkm.Height)
                return false;
            if (pk.WeightScalar != pkm.Weight)
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