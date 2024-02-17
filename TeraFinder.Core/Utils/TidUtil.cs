namespace TeraFinder.Core;

public static class TidUtil
{
    public static uint GetID32(uint tid7, uint sid7) => (uint)(tid7 + ((ulong)sid7 * 1000000));

    public static ushort GetTID16(uint id32) => (ushort)(id32 & 0xFFFF);

    public static ushort GetSID16(uint id32) => (ushort)(id32 >> 16);
}
