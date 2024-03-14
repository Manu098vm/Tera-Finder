using Xunit;
using PKHeX.Core;
using TeraFinder.Core;
using Xunit.Sdk;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.

namespace TeraFinder.Tests
{
    public class GenerateEntitiesTests
    {
        [Fact]
        public void TestEncounterTeraTF9()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
            {
                var (standard, black) = ResourcesUtil.GetAllTeraEncounters(map);
                for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                {
                    for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.Unlocked6Stars; progress++)
                    {
                        for (var content = RaidContent.Standard; content <= RaidContent.Black; content++)
                        {
                            Parallel.For(0L, uint.MaxValue, (seed, state) =>
                            {
                                var encounters = content switch { RaidContent.Standard => standard, _ => black };
                                try
                                {
                                    Assert.True(Core.EncounterRaidTF9.TryGenerateTeraDetails((uint)seed, encounters, game, progress, EventProgress.Stage0, content, map, 0, 0, out var encounter, out var result));
                                    Assert.True(encounter.GeneratePK9(result.Value, 0, game, "test", 2, 0, out var pk9, out var legality));
                                }
                                catch (XunitException) { state.Stop(); }
                            });
                        }
                    }
                }
            }
        }
    }
}

#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.