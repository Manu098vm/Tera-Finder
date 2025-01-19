using PKHeX.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core;

public class GameCoordinates(SCBlock coordinates)
{
    protected readonly SCBlock Coordinates = coordinates;

    public float X { get => ReadSingleLittleEndian(Coordinates.Data); set => SetX(value); }
    public float Y { get => ReadSingleLittleEndian(Coordinates.Data[4..]); set => SetY(value); }
    public float Z { get => ReadSingleLittleEndian(Coordinates.Data[8..]); set => SetZ(value); }

    private void SetX(float x) => SetCoordinates(0, x);
    private void SetY(float y) => SetCoordinates(4, y);
    private void SetZ(float z) => SetCoordinates(8, z);

    private void SetCoordinates(int index, float value) => WriteSingleLittleEndian(Coordinates.Data[index..], value);

    public ReadOnlySpan<byte> GetCoordinates() => Coordinates.Data;
    public void SetCoordinates(ReadOnlySpan<byte> coordinates) => Coordinates.ChangeData(coordinates);
}