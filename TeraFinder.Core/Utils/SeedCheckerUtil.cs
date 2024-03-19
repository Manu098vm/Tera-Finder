namespace TeraFinder.Core;

public static class SeedCheckerUtil
{
    public static Dictionary<uint, HashSet<EncounterEventTF9>> GroupEventEncounters(EncounterEventTF9[] encounters) =>
        encounters.GroupBy(e => Convert.ToUInt32($"{e.Identifier}"[..^2], 10)).ToDictionary(g => g.Key, g => new HashSet<EncounterEventTF9>(g));
}
