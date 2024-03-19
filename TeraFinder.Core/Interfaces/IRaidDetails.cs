namespace TeraFinder.Core;

public interface IRaidDetails
{
    uint Seed { get; }
    ushort Species { get; }
    int Stars { get; }
    TeraShiny Shiny { get; }
}
