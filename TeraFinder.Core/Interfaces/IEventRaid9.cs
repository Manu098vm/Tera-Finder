namespace TeraFinder.Core;

public interface IEventRaid9
{
    ushort Species { get; }
    byte Index { get; }
    byte RandRate { get; }

    ushort RandRate0MinScarlet { get; }
    ushort RandRate0MinViolet { get; }
    ushort RandRate0TotalScarlet { get; }
    ushort RandRate0TotalViolet { get; }

    ushort RandRate1MinScarlet { get; }
    ushort RandRate1MinViolet { get; }
    ushort RandRate1TotalScarlet { get; }
    ushort RandRate1TotalViolet { get; }

    ushort RandRate2MinScarlet { get; }
    ushort RandRate2MinViolet { get; }
    ushort RandRate2TotalScarlet { get; }
    ushort RandRate2TotalViolet { get; }

    ushort RandRate3MinScarlet { get; }
    ushort RandRate3MinViolet { get; }
    ushort RandRate3TotalScarlet { get; }
    ushort RandRate3TotalViolet { get; }

    ushort GetRandRateTotalScarlet(EventProgress stage);
    ushort GetRandRateTotalViolet(EventProgress stage);
    ushort GetRandRateMinScarlet(EventProgress stage);
    ushort GetRandRateMinViolet(EventProgress stage);

    EventProgress GetProgressMaximum(uint seed);
}
