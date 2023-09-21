using PKHeX.Core;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class CheckerForm : Form
{
    private readonly PK9 PKM = null!;
    private readonly SAV9SV SAV;
    private Dictionary<string, string> Strings = null!;

    public CheckerForm(PKM pk, SAV9SV sav, string language)
    {
        InitializeComponent();
        GenerateDictionary();
        TranslateDictionary(language);
        this.TranslateInterface(language);

        var species = GameInfo.GetStrings(language).specieslist;
        var natures = GameInfo.GetStrings(language).natures;
        var types = GameInfo.GetStrings(language).types;
        cmbSpecies.Items.Clear();
        cmbSpecies.Items.AddRange(species);
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
        cmbSpecies.SelectedIndex = PKM.Species;
        cmbNature.SelectedIndex = PKM.Nature;
        cmbTera.SelectedIndex = (int)PKM.TeraTypeOriginal;
        numHeight.Value = PKM.HeightScalar;
        numWeight.Value = PKM.WeightScalar;
        numScale.Value = PKM.Scale;
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
        var species = (ushort)cmbSpecies.SelectedIndex;
        var tid = Convert.ToUInt32(txtTid.Text);
        var sid = Convert.ToUInt32(txtSid.Text);
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
            Species = species,
            EncryptionConstant = ec,
            PID = pid,
            TrainerTID7 = tid,
            TrainerSID7 = sid,
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
            if (legality.EncounterMatch is PKHeX.Core.EncounterDist9)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Event"]})";
            else if (legality.EncounterMatch is PKHeX.Core.EncounterMight9)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Event_Mighty"]})";
            else
                txtSeed.Text = (byte)((dynamic)legality.EncounterMatch).Stars == 6 ?
                    $"{seed:X8} ({Strings["RaidContent.Black"]})" : $"{seed:X8} ({Strings["RaidContent.Standard"]})";
        }
        else
            txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Invalid"]})";
    }
}