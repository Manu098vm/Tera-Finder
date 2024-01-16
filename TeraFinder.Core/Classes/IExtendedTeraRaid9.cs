using PKHeX.Core;
namespace TeraFinder.Core;

public interface IExtendedTeraRaid9 : ITeraRaid9
{
    uint Identifier { get; }
    ulong FixedRewardHash { get; }
    ulong LotteryRewardHash { get; }
    int Item { get; }
    ExtraMoves ExtraMoves { get; }
    byte Level { get; }
    Nature Nature { get; }
    SizeType9 ScaleType { get; }
    byte Scale { get; }
    IndividualValueSet IVs { get; }

    ushort GetRandRateTotalScarlet(int stage);
    ushort GetRandRateTotalViolet(int stage);
    ushort GetRandRateMinScarlet(int stage);
    ushort GetRandRateMinViolet(int stage);
}
