using Xunit;
using PKHeX.Core;
using TeraFinder.Core;

#pragma warning disable CS8604 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace TeraFinder.Tests
{
    public class GenerateRewardsWithFilters
    {
        private const uint MaxTests = 0xFFFF;

        [Fact]
        public void TestEncounterTeraTF9StandardFilter()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
                for (byte stars = 1; stars <= 5; stars++)
                {
                    var encounters = ResourcesUtil.GetAllTeraEncounters(map).standard;
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var romTotal = game is GameVersion.VL ? EncounterTera9.GetRateTotalVL(stars, map) : EncounterTera9.GetRateTotalSL(stars, map);
                        var filter = new RewardFilter(true, false) { Encounter = new EncounterFilter(0, stars), FilterRewards = [] };
                        var effective_encounters = encounters.Where(filter.IsEncounterMatch).ToArray();

                        for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.Unlocked6Stars; progress++)
                            ParallelizeGeneration(effective_encounters, filter, game, RaidContent.Standard, romTotal, progress);
                    }
                }
        }

        [Fact]
        public void TestEncounterTeraTF9BlackFilter()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
            {
                var encounters = ResourcesUtil.GetAllTeraEncounters(map).black;
                for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                {
                    var romTotal = game is GameVersion.VL ? EncounterTera9.GetRateTotalVL(6, map) : EncounterTera9.GetRateTotalSL(6, map);
                    var filter = new RewardFilter(true, false) { Encounter = new EncounterFilter(0, 6), FilterRewards = [] };
                    var effective_encounters = encounters.Where(filter.IsEncounterMatch).ToArray();

                    ParallelizeGeneration(effective_encounters, filter, game, RaidContent.Black, romTotal, GameProgress.Unlocked6Stars);
                }
            }
        }

        private static void ParallelizeGeneration(EncounterRaidTF9[] encounters, RewardFilter filter, GameVersion version, RaidContent content, short rateTotal = 0, GameProgress progress = GameProgress.UnlockedTeraRaids, EventProgress eventProgress = EventProgress.Stage0, byte groupid = 0)
        {
            Parallel.For(0L, MaxTests, (seed, state) =>
            {
                if (EncounterRaidTF9.TryGenerateRewardDetails((uint)seed, encounters, filter, rateTotal, version, progress, eventProgress, content, 0, groupid, 1, out var encounter, out var result))
                    Assert.NotEmpty(result.Value.Rewards);
            });
        }
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8604 // Dereference of a possibly null reference.