using Xunit;
using PKHeX.Core;
using TeraFinder.Core;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
namespace TeraFinder.Tests
{
    public class GenerateEntities
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

        [Fact]
        public void TestEncounterEventTF9()
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
                                ParallelizeGeneration(group.Value.ToArray(), game, content, eventProgress: stage, groupid: index);
                        }
        }

        private static void ParallelizeGeneration(EncounterRaidTF9[] encounters, GameVersion version, RaidContent content, TeraRaidMapParent map = TeraRaidMapParent.Paldea, GameProgress progress = GameProgress.UnlockedTeraRaids, EventProgress eventProgress = EventProgress.Stage0, byte groupid = 0) =>
            Parallel.For(0L, MaxTests, (seed, state) =>
            {
                Assert.True(EncounterRaidTF9.TryGenerateTeraDetails((uint)seed, encounters, version, progress, eventProgress, content, map, 0, groupid, out var encounter, out var result));
                Assert.True(encounter.GeneratePK9(result.Value, 0, version, "test", 2, 0, out _, out _));
            });
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.