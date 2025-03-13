namespace TeraFinder.Core;

public interface IEncounterRaidCollection
{
    EncounterRaidTF9[] Paldea { get; set; }
    EncounterRaidTF9[] PaldeaBlack { get; set; }
    EncounterRaidTF9[] Kitakami { get; set; }
    EncounterRaidTF9[] KitakamiBlack { get; set; }
    EncounterRaidTF9[] Blueberry { get; set; }
    EncounterRaidTF9[] BlueberryBlack { get; set; }
    EncounterEventTF9[] Dist { get; set; }
    EncounterEventTF9[] Mighty { get; set; }

    Dictionary<uint, HashSet<EncounterEventTF9>> AllDist { get; set; }
    Dictionary<uint, HashSet<EncounterEventTF9>> AllMighty { get; set; }
}