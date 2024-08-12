namespace TeraFinder.Core;

public interface IOutbreak
{
    int ID { get; }
    GameCoordinates? LocationCenter { get; set; }
    GameCoordinates? LocationDummy { get; set; }
    sbyte AmountAvailable { get; set; }
    bool Found { get; set; }
    bool Enabled { get; set; }
    uint Species { get; set; }
    byte Form { get; set; }
    int NumKO { get; set; }
    int MaxSpawns { get; set; }
    bool IsEvent { get; }

    FakeOutbreak Clone();
    void DumpTojson(string path);
    void RestoreFromJson(string json);
}