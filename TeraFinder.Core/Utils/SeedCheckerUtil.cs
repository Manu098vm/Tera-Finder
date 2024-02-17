using PKHeX.Core;

namespace TeraFinder.Core;

public static class SeedCheckerUtil
{
    public static EncounterEventTF9[] FilterDistEncounters(uint seed, EncounterEventTF9[] encounters, GameVersion version, EventProgress progress, ushort species, byte groupid)
    {
        var res = new List<EncounterEventTF9>();
        foreach (var encounter in encounters)
        {
            if (encounter.Species != species || encounter.Index != groupid)
                continue;

            var max = version is GameVersion.SL ? encounter.GetRandRateTotalScarlet(progress) : encounter.GetRandRateTotalViolet(progress);
            var min = version is GameVersion.SL ? encounter.GetRandRateMinScarlet(progress) : encounter.GetRandRateMinViolet(progress);
            if (min >= 0 && max > 0)
            {
                var xoro = new Xoroshiro128Plus(seed);
                xoro.NextInt(100);
                var rateRand = xoro.NextInt(max);
                if ((uint)(rateRand - min) < encounter.RandRate)
                    res.Add(encounter);
            }
        }
        return [.. res];
    }
}
