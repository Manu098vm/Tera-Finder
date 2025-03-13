using PKHeX.Core;

namespace TeraFinder.Core;

public interface ITFPlugin : IPlugin, ITranslatable, IEncounterRaidCollection
{
    public string Version { get; init; }
    public SAV9SV SAV { get; set; }
}
