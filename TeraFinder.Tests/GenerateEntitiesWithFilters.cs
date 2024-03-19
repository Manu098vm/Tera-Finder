using Xunit;
using PKHeX.Core;
using TeraFinder.Core;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace TeraFinder.Tests
{
    public class GenerateEntitiesWithFilters
    {
        private const uint MaxTests = 0xFFFF;

        [Fact]
        public void TestEncounterTeraTF9StandardFilter()
        {
            for (var map = TeraRaidMapParent.Paldea; map <= TeraRaidMapParent.Blueberry; map++)
                for (var stars = 1; stars <= 5; stars++)
                {
                    var encounters = ResourcesUtil.GetAllTeraEncounters(map).standard;
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var romTotal = game is GameVersion.VL ? EncounterTera9.GetRateTotalVL(stars, map) : EncounterTera9.GetRateTotalSL(stars, map);
                        var filter = new TeraFilter(true, false, false, false) { Stars = stars };
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
                    var filter = new TeraFilter(true, false, false, false) { Stars = 6 };
                    var effective_encounters = encounters.Where(filter.IsEncounterMatch).ToArray();

                    ParallelizeGeneration(effective_encounters, filter, game, RaidContent.Black, romTotal, GameProgress.Unlocked6Stars);
                }
            }
        }

        [Fact]
        public void TestEncounterEventTF9Filter()
        {
            var (eventsData, mightyData) = ResourcesUtil.GetAllEventEncounters();
            var events = SeedCheckerUtil.GroupEventEncounters(eventsData);
            var mighty = SeedCheckerUtil.GroupEventEncounters(mightyData);

            for (var content = RaidContent.Event; content <= RaidContent.Event_Mighty; content++)
                foreach (var group in content is RaidContent.Event ? events : mighty)
                    foreach (var index in new HashSet<byte>(group.Value.Select(enc => enc.Index)))
                        for (var game = GameVersion.SL; game <= GameVersion.VL; ++game)
                        {
                            var possibleStages = new HashSet<EventProgress>(Enum.GetValues(typeof(EventProgress)).Cast<EventProgress>()
                                .Where(progress => group.Value.Any(enc => enc.Index == index && enc.CanBeEncounteredFromStage(progress, game))));

                            foreach (var stage in possibleStages)
                            {
                                foreach (var stars in new HashSet<byte>(group.Value.Select(encounter => encounter.Stars)))
                                {
                                    var filter = new TeraFilter(true, false, false, false) { Stars = stars };
                                    var encounters = group.Value.Where(filter.IsEncounterMatch).ToArray();

                                    ParallelizeGeneration(encounters, filter, game, content, eventProgress: stage, groupid: index);
                                }
                            }
                        }
        }

        private static void ParallelizeGeneration(EncounterRaidTF9[] encounters, TeraFilter filter, GameVersion version, RaidContent content, short rateTotal = 0, GameProgress progress = GameProgress.UnlockedTeraRaids, EventProgress eventProgress = EventProgress.Stage0, byte groupid = 0) =>
            Parallel.For(0L, MaxTests, (seed, state) =>
            {
                if (EncounterRaidTF9.TryGenerateTeraDetails((uint)seed, encounters, filter, rateTotal, version, progress, eventProgress, content, 0, groupid, out var encounter, out var result))
                    Assert.True(encounter.GeneratePK9(result.Value, 0, version, "test", 2, 0, out _, out _));
            });
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.