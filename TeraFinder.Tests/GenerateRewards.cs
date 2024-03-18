using Xunit;
using PKHeX.Core;
using TeraFinder.Core;

#pragma warning disable CS8604 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace TeraFinder.Tests
{
    public class GenerateRewards
    {
        private const uint MaxTests = 0xFFFF;

        [Fact]
        public void TestEncounterTeraTF9Standard()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
            {
                var encounters = ResourcesUtil.GetAllTeraEncounters(map).standard;
                for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.Unlocked6Stars; progress++)
                        ParallelizeGeneration(encounters, game, RaidContent.Standard, map, progress);
            }
        }

        [Fact]
        public void TestEncounterTeraTF9Black()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
            {
                var encounters = ResourcesUtil.GetAllTeraEncounters(map).black;
                for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    ParallelizeGeneration(encounters, game, RaidContent.Black, map, GameProgress.Unlocked6Stars);
            }
        }

        private static void ParallelizeGeneration(EncounterRaidTF9[] encounters, GameVersion version, RaidContent content, TeraRaidMapParent map = TeraRaidMapParent.Paldea, GameProgress progress = GameProgress.UnlockedTeraRaids, EventProgress eventProgress = EventProgress.Stage0, byte groupid = 0)
        {
            Parallel.For(0L, MaxTests, (seed, state) =>
            {
                Assert.True(EncounterRaidTF9.TryGenerateRewardDetails((uint)seed, encounters, version, progress, eventProgress, content, map, 0, groupid, 1, out _, out var result));
                Assert.NotEmpty(result.Value.Rewards);
            });
        }
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8604 // Dereference of a possibly null reference.