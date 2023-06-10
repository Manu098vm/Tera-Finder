using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class CheckerForm : Form
{
    private PK9 PKM = null!;
    private SAV9SV SAV;

    private EncounterRaid9[] Tera = null!;
    private EncounterRaid9[] Dist = null!;
    private EncounterRaid9[] Mighty = null!;

    private Dictionary<string, string> Strings = null!;
    private string[] Contents = null!;

    public CheckerForm(PKM pk, SAV9SV sav, string language)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(language);
        this.TranslateInterface(language);
        Contents = new string[] { Strings["RaidContent.Standard"], Strings["RaidContent.Black"], Strings["RaidContent.Event"], Strings["RaidContent.Event_Mighty"] };

        var natures = GameInfo.GetStrings(language).natures;
        var types = GameInfo.GetStrings(language).types;
        cmbNature.Items.Clear();
        cmbNature.Items.AddRange(natures);
        cmbTera.Items.Clear();
        cmbTera.Items.AddRange(types);

        SAV = sav;
        PKM = (PK9)pk;
        txtTid.Text = $"{PKM.TrainerTID7}";
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
        var events = TeraUtil.GetAllDistEncounters();
        Tera = TeraUtil.GetAllTeraEncounters();
        Dist = events[0];
        Mighty = events[1];
    }

    private void GenerateDictionary()
    {
        Strings = new Dictionary<string, string>
        {
            { "RaidContent.Invalid", "INVALID" },
            { "RaidContent.Standard", "Standard" },
            { "RaidContent.Black", "Black" },
            { "RaidContent.Event", "Event" },
            { "RaidContent.Event_Mighty", "Event-Mighty" },
        };
    }

    private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

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
        var tid = (uint)Convert.ToInt32(txtTid.Text);
        var sid = (uint)Convert.ToInt32(txtSid.Text);
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

        var pk = new PK9(PKM.Data)
        {
            EncryptionConstant = ec,
            PID = pid,
            IV_HP = hp,
            IV_ATK = atk,
            IV_DEF = def,
            IV_SPA = spa,
            IV_SPD = spd,
            IV_SPE = spe,
            Nature = nature,
            TeraTypeOriginal = (MoveType)tera,
            HeightScalar = height,
            WeightScalar = weight,
            Scale = scale
        };

        var seed = Tera9RNG.GetOriginalSeed(pk);
        var legality = new LegalityAnalysis(pk);

        if (legality.Valid)
        {
            var match = legality.EncounterMatch is PKHeX.Core.EncounterDist9 ? CalculateDist(seed, tid, sid, pk) :
                legality.EncounterMatch is PKHeX.Core.EncounterMight9 ? CalculateMighty(seed, tid, sid, pk) : CalculateStandard(seed, tid, sid, pk);
            if (!match)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Invalid"]})";
        }
        else
        {
            if (!CalculateStandard(seed, tid, sid, pk) && !CalculateDist(seed, tid, sid, pk) && !CalculateMighty(seed, tid, sid, pk))
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Invalid"]})";
        }
    }

    private bool CalculateStandard(uint seed, uint tid, uint sid, PK9 pk)
    {
        for (var content = RaidContent.Standard; content <= RaidContent.Black; content++)
        {
            for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.Unlocked6Stars; progress++)
            {
                for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                {
                    var sav = (SAV9SV)SAV.Clone();
                    sav.Game = (int)game;

                    var curr = progress;
                    if (content is RaidContent.Black)
                        curr = GameProgress.None;

                    var encounter = TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, curr), Tera);
                    if (encounter is not null)
                    {
                        var rngres = TeraUtil.CalcRNG(seed, tid, sid, content, encounter);
                        var success = ComparePKM(pk, rngres);
                        if (success)
                        {
                            var type = $"{Contents[(byte)content]}";
                            txtSeed.Text = $"{seed:X8} ({type})";
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool CalculateDist(uint seed, uint tid, uint sid, PK9 pk) => CalculateEvent(seed, tid, sid, pk, RaidContent.Event, Dist);
    private bool CalculateMighty(uint seed, uint tid, uint sid, PK9 pk) => CalculateEvent(seed, tid, sid, pk, RaidContent.Event_Mighty, Mighty);

    private bool CalculateEvent(uint seed, uint tid, uint sid, PK9 pk, RaidContent content, EncounterRaid9[] encounters)
    {
        for (var progress = GameProgress.UnlockedTeraRaids; progress < GameProgress.Unlocked6Stars; progress++)
        {
            for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
            {
                for (var index = 0; index < encounters.Length; index++)
                {
                    var sav = (SAV9SV)SAV.Clone();
                    sav.Game = (int)game;

                    var encounter = TeraUtil.GetDistEncounterWithIndex(seed, sav, progress, encounters, index);

                    if (encounter is not null)
                    {
                        var rngres = TeraUtil.CalcRNG(seed, tid, sid, content, encounter);
                        var success = ComparePKM(pk, rngres);
                        if (success)
                        {
                            var type = $"{Contents[(byte)content]}";
                            txtSeed.Text = $"{seed:X8} ({type})";
                            return true;
                        }
                    }
                }
            }
        }

        return false;
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
        if (pk.Nature != pkm.Nature)
            return false;
        if ((sbyte)pk.TeraTypeOriginal != pkm.TeraType)
            return false;
        if (pk.HeightScalar != pkm.Height && pk.HeightScalar != pkm.Scale)
            return false;
        if (pk.WeightScalar != pkm.Weight)
            return false;
        if (pk.Scale != pkm.Scale)
            return false;

        return true;
    }
}
