using PKHeX.Core;

namespace TeraFinder.Core;

public class ExtraMoves
{
    public Move ExtraMove1;
    public Move ExtraMove2;
    public Move ExtraMove3;
    public Move ExtraMove4;
    public Move ExtraMove5;
    public Move ExtraMove6;

    public ExtraMoves(ushort extra1 = 0, ushort extra2 = 0, ushort extra3 = 0, ushort extra4 = 0, ushort extra5 = 0, ushort extra6 = 0)
    {
        ExtraMove1 = (Move)extra1;
        ExtraMove2 = (Move)extra2;
        ExtraMove3 = (Move)extra3;
        ExtraMove4 = (Move)extra4;
        ExtraMove5 = (Move)extra5;
        ExtraMove6 = (Move)extra6;
    }
}