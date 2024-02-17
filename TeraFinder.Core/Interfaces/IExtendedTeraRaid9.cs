using PKHeX.Core;
namespace TeraFinder.Core;

public interface IExtendedTeraRaid9 : ITeraRaid9
{
    uint Identifier { get; }
    List<Reward> FixedRewards { get; }
    List<Reward> LotteryRewards { get; }
    int HeldItem { get; }
    ExtraMoves ExtraMoves { get; }
    byte Level { get; }
    Nature Nature { get; }
    SizeType9 ScaleType { get; }
    byte Scale { get; }
    IndividualValueSet IVs { get; }
}
