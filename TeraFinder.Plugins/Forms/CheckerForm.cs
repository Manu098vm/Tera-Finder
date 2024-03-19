using PKHeX.Core;
using System;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public partial class CheckerForm : Form
{
    private readonly PK9 PKM = null!;
    private Dictionary<string, string> Strings = null!;

    private readonly EncounterTeraTF9[] Paldea = null!;
    private readonly EncounterTeraTF9[] PaldeaBlack = null!;
    private readonly EncounterTeraTF9[] Kitakami = null!;
    private readonly EncounterTeraTF9[] KitakamiBlack = null!;
    private readonly EncounterTeraTF9[] Blueberry = null!;
    private readonly EncounterTeraTF9[] BlueberryBlack = null!;

    private readonly Dictionary<uint, HashSet<EncounterEventTF9>> Dist = null!;
    private readonly Dictionary<uint, HashSet<EncounterEventTF9>> Mighty = null!;

    public CheckerForm(PKM pk, string language,
                       EncounterTeraTF9[] paldea,
                       EncounterTeraTF9[] paldeablack,
                       EncounterTeraTF9[] kitakami,
                       EncounterTeraTF9[] kitakamiblack,
                       EncounterTeraTF9[] blueberry,
                       EncounterTeraTF9[] blueberryblack,
                       Dictionary<uint, HashSet<EncounterEventTF9>> dist,
                       Dictionary<uint, HashSet<EncounterEventTF9>> mighty)
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
        cmbNature.SelectedIndex = (int)PKM.Nature;
        cmbTera.SelectedIndex = (int)PKM.TeraTypeOriginal;
        numHeight.Value = PKM.HeightScalar;
        numWeight.Value = PKM.WeightScalar;
        numScale.Value = PKM.Scale;

        Paldea = paldea;
        PaldeaBlack = paldeablack;
        Kitakami = kitakami;
        KitakamiBlack = kitakamiblack;
        Blueberry = blueberry;
        BlueberryBlack = blueberryblack;
        Dist = dist;
        Mighty = mighty;
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
        var nature = (Nature)cmbNature.SelectedIndex;
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

        //Standard Raids Check
        for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
        {
            for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.Unlocked6Stars; progress++)
            {
                for (var version = GameVersion.SL; version <= GameVersion.VL; version++)
                {
                    var encounters = map switch
                    {
                        TeraRaidMapParent.Paldea => Paldea,
                        TeraRaidMapParent.Kitakami => Kitakami,
                        TeraRaidMapParent.Blueberry => Blueberry,
                        _ => throw new NotImplementedException(nameof(map)),
                    };
                    if (EncounterRaidTF9.TryGenerateTeraDetails(seed, encounters, version, progress, EventProgress.Stage0, RaidContent.Standard, map, pk.ID32, 0, out var encounter, out var result))
                    {
                        if (CompareResult(pk, result!.Value))
                        {
                            SetReultText(seed, encounter);
                            return;
                        }
                    }
                }
            }
        }

        //Black Raids Check
        for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
        {
            for (var version = GameVersion.SL; version <= GameVersion.VL; version++)
            {
                var encounters = map switch
                {
                    TeraRaidMapParent.Paldea => PaldeaBlack,
                    TeraRaidMapParent.Kitakami => KitakamiBlack,
                    TeraRaidMapParent.Blueberry => BlueberryBlack,
                    _ => throw new NotImplementedException(nameof(map))
                };
                if (EncounterRaidTF9.TryGenerateTeraDetails(seed, encounters, version, GameProgress.Unlocked6Stars, EventProgress.Stage0, RaidContent.Black, map, pk.ID32, 0, out var encounter, out var result))
                {
                    if (CompareResult(pk, result!.Value))
                    {
                        SetReultText(seed, encounter);
                        return;
                    }
                }
            }
        }

        //Events Raids Check
        for (var content = RaidContent.Event; content <= RaidContent.Event_Mighty; content++)
        {
            foreach (var group in content is RaidContent.Event ? Dist : Mighty)
            {
                foreach (var index in new HashSet<byte>(group.Value.Select(enc => enc.Index)))
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; ++game)
                    {
                        var possibleStages = new HashSet<EventProgress>(Enum.GetValues(typeof(EventProgress)).Cast<EventProgress>()
                            .Where(progress => group.Value.Any(enc => enc.Index == index && enc.CanBeEncounteredFromStage(progress, game))));

                        foreach (var stage in possibleStages)
                        {
                            if (EncounterRaidTF9.TryGenerateTeraDetails(seed, group.Value.ToArray(), game, GameProgress.UnlockedTeraRaids, stage, RaidContent.Event, TeraRaidMapParent.Paldea, pk.ID32, index, out var encounter, out var result))
                            {
                                if (CompareResult(pk, result!.Value))
                                {
                                    SetReultText(seed, encounter);
                                    return;
                                }
                            }
                        }
                    }
                } 
            }
        }
    }

    private static bool CompareResult (PK9 pkm, TeraDetails rng)
    {
        if (pkm.Species != rng.Species)
            return false;
        if (pkm.EncryptionConstant != rng.EC)
            return false;
        if (pkm.PID != rng.PID)
            return false;
        if (pkm.IV_HP != rng.HP)
            return false;
        if (pkm.IV_ATK != rng.ATK)
            return false;
        if (pkm.IV_DEF != rng.DEF)
            return false;
        if (pkm.IV_SPA != rng.SPA)
            return false;
        if (pkm.IV_SPD != rng.SPD)
            return false;
        if (pkm.IV_SPE != rng.SPE)
            return false;
        if (pkm.Nature != rng.Nature)
            return false;
        if ((sbyte)pkm.TeraTypeOriginal != rng.TeraType)
            return false;
        if (pkm.HeightScalar != rng.Height &&
            pkm.HeightScalar != rng.Scale)
            return false;
        if (pkm.WeightScalar != rng.Weight)
            return false;
        if (pkm.Scale != rng.Scale)
            return false;

        return true;
    }

    private void SetReultText (uint seed, EncounterRaidTF9? encounter = null)
    {
        if (encounter is not null)
        {
            if (encounter.ContentType is RaidContent.Standard)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Standard"]})";
            else if (encounter.ContentType is RaidContent.Black)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Black"]})";
            else if (encounter.ContentType is RaidContent.Event)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Event"]})";
            else if (encounter.ContentType is RaidContent.Event_Mighty)
                txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Event_Mighty"]})";

            return;
        }

        txtSeed.Text = $"{seed:X8} ({Strings["RaidContent.Invalid"]})";
        return;

    }
}