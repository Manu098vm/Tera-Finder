using PKHeX.Core;
using System.Text;

namespace TeraFinder.Core;

public static class ResourcesUtil
{
    public static byte[] GetDenLocations(TeraRaidMapParent map) => map switch
    {
        TeraRaidMapParent.Paldea => Properties.Resources.paldea_locations,
        TeraRaidMapParent.Kitakami => Properties.Resources.kitakami_locations,
        _ => Properties.Resources.blueberry_locations
    };

    public static byte[] GetFixedRewardsData() => Properties.Resources.raid_fixed_reward_item_array;

    public static byte[] GetLotteryRewardsData() => Properties.Resources.raid_lottery_reward_item_array;

    public static (EncounterTeraTF9[] standard, EncounterTeraTF9[] black) GetAllTeraEncounters(TeraRaidMapParent map)
    {
        var (fixedRewards, lotteryRewards) = RewardUtil.GetTeraRewardsTables();
        var standard = EncounterTeraTF9.GetArray(map switch
        {
            TeraRaidMapParent.Paldea => Properties.Resources.encounter_gem_paldea_standard,
            TeraRaidMapParent.Kitakami => Properties.Resources.encounter_gem_kitakami_standard,
            TeraRaidMapParent.Blueberry => Properties.Resources.encounter_gem_blueberry_standard,
            _ => throw new ArgumentOutOfRangeException(nameof(map))
        }, fixedRewards, lotteryRewards, map);

        var black = EncounterTeraTF9.GetArray(map switch
        {
            TeraRaidMapParent.Paldea => Properties.Resources.encounter_gem_paldea_black,
            TeraRaidMapParent.Kitakami => Properties.Resources.encounter_gem_kitakami_black,
            TeraRaidMapParent.Blueberry => Properties.Resources.encounter_gem_blueberry_black,
            _ => throw new ArgumentOutOfRangeException(nameof(map))
        }, fixedRewards, lotteryRewards, map);

        return (standard, black);
    }

    public static (EncounterEventTF9[] dist, EncounterEventTF9[] might) GetAllEventEncounters()
    {
        var dist = EncounterEventTF9.GetArray(Properties.Resources.encounter_dist_paldea, [], []);
        var might = EncounterEventTF9.GetArray(Properties.Resources.encounter_might_paldea, [], []);
        return (dist, might);
    }

    public static string? GetTextResource(string res)
    {
        var obj = Properties.Resources.ResourceManager.GetObject(res);
        if (res.Contains("lang"))
            return (string?)obj;
        else if (obj is null) return null;
        return Encoding.UTF8.GetString((byte[])obj);
    }
}