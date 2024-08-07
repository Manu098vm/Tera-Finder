namespace TeraFinder.Core;

public interface IOutbreak
{
    GameCoordinates? LocationCenter { get; set; }
    GameCoordinates? LocationDummy { get; set; }
    sbyte AmountAvailable { get; set; }
    bool Found { get; set; }
    bool Enabled { get; set; }
    uint Species { get; set; }
    byte Form { get; set; }
    int NumKO { get; set; }
    int MaxSpawns { get; set; }

    FakeOutbreak Clone();
    void DumpTojson(string path);
    void RestoreFromJson(string json);
}