using PKHeX.Core;

namespace TeraFinder.Core;

public class ExtraMoves
{
    public HashSet<Move> ExtraMoveList { get; private init; }

    public Move ExtraMove1 { get => ExtraMoveList.Count > 0 ? ExtraMoveList.ElementAt(0) : Move.None; }
    public Move ExtraMove2 { get => ExtraMoveList.Count > 1 ? ExtraMoveList.ElementAt(1) : Move.None; }
    public Move ExtraMove3 { get => ExtraMoveList.Count > 2 ? ExtraMoveList.ElementAt(2) : Move.None; }
    public Move ExtraMove4 { get => ExtraMoveList.Count > 3 ? ExtraMoveList.ElementAt(3) : Move.None; }
    public Move ExtraMove5 { get => ExtraMoveList.Count > 4 ? ExtraMoveList.ElementAt(4) : Move.None; }
    public Move ExtraMove6 { get => ExtraMoveList.Count > 5 ? ExtraMoveList.ElementAt(5) : Move.None; }

    public ExtraMoves(ushort extra1 = 0, ushort extra2 = 0, ushort extra3 = 0, ushort extra4 = 0, ushort extra5 = 0, ushort extra6 = 0)
    {
        ExtraMoveList = [];
        if (extra1 != 0)
            ExtraMoveList.Add((Move)extra1);
        if (extra2 != 0)
            ExtraMoveList.Add((Move)extra2);
        if (extra3 != 0)
            ExtraMoveList.Add((Move)extra3);
        if (extra4 != 0)
            ExtraMoveList.Add((Move)extra4);
        if (extra5 != 0)
            ExtraMoveList.Add((Move)extra5);
        if (extra6 != 0)
            ExtraMoveList.Add((Move)extra6);
    }

    public Move GetExtraMove(int index) => ExtraMoveList.ElementAt(index);
}