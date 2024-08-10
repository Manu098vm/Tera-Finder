using PKHeX.Core;
using System.Text.Json.Nodes;
using System.Text.Json;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core;

public class FakeOutbreak(IOutbreak outbreak)
{
    private byte[] LocationCenter { get; set; } = [.. outbreak.LocationCenter!.GetCoordinates()];
    private byte[] LocationDummy { get; set; } = [.. outbreak.LocationDummy!.GetCoordinates()];
    public uint Species { get; private set; } = outbreak.Species;
    public byte Form { get; private set; } = outbreak.Form;
    public int MaxSpawns { get; private set; } = outbreak.MaxSpawns;

    public float CenterX { get => ReadSingleLittleEndian(LocationCenter.AsSpan()); }
    public float CenterY { get => ReadSingleLittleEndian(LocationCenter.AsSpan()[4..]); }
    public float CenterZ { get => ReadSingleLittleEndian(LocationCenter.AsSpan()[8..]); }

    public float DummyX { get => ReadSingleLittleEndian(LocationDummy.AsSpan()); }
    public float DummyY { get => ReadSingleLittleEndian(LocationDummy.AsSpan()[4..]); }
    public float DummyZ { get => ReadSingleLittleEndian(LocationDummy.AsSpan()[8..]); }

    public void RestoreFromJson(string json)
    {
        var simpleOutbreak = JsonSerializer.Deserialize<JsonNode>(json)!;
        var locationCenter = Convert.FromHexString(simpleOutbreak[nameof(LocationCenter)]!.GetValue<string>());
        var locationDummy = Convert.FromHexString(simpleOutbreak[nameof(LocationDummy)]!.GetValue<string>());
        var species = SpeciesConverter.GetInternal9(simpleOutbreak[nameof(Species)]!.GetValue<ushort>());
        var form = simpleOutbreak[nameof(Form)]!.GetValue<byte>();
        var maxSpawns = simpleOutbreak[nameof(MaxSpawns)]!.GetValue<int>();

        LocationCenter = locationCenter;
        LocationDummy = locationDummy;
        Species = species;
        Form = form;
        MaxSpawns = maxSpawns;
    }
}