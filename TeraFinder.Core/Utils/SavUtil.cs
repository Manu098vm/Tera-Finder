using PKHeX.Core;

namespace TeraFinder.Core;

public static class SavUtil
{
    public static GameProgress GetProgress(SAV9SV sav)
    {
        var Unlocked6Stars = sav.Accessor.FindOrDefault(BlockDefinitions.KUnlockedRaidDifficulty6.Key).Type is SCTypeCode.Bool2;

        if (Unlocked6Stars)
            return GameProgress.Unlocked6Stars;

        var Unlocked5Stars = sav.Accessor.FindOrDefault(BlockDefinitions.KUnlockedRaidDifficulty5.Key).Type is SCTypeCode.Bool2;

        if (Unlocked5Stars)
            return GameProgress.Unlocked5Stars;

        var Unlocked4Stars = sav.Accessor.FindOrDefault(BlockDefinitions.KUnlockedRaidDifficulty4.Key).Type is SCTypeCode.Bool2;

        if (Unlocked4Stars)
            return GameProgress.Unlocked4Stars;

        var Unlocked3Stars = sav.Accessor.FindOrDefault(BlockDefinitions.KUnlockedRaidDifficulty3.Key).Type is SCTypeCode.Bool2;

        if (Unlocked3Stars)
            return GameProgress.Unlocked3Stars;

        var UnlockedTeraRaids = sav.Accessor.FindOrDefault(BlockDefinitions.KUnlockedTeraRaidBattles.Key).Type is SCTypeCode.Bool2;

        if (UnlockedTeraRaids)
            return GameProgress.UnlockedTeraRaids;

        return GameProgress.Beginning;
    }
}
