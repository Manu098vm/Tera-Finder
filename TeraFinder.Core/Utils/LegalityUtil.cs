using PKHeX.Core;

namespace TeraFinder.Core;

public static class LegalityUtil
{
    public static LegalitySettings StoreLegalitySettings() => new()
    {
        CheckActiveHandler = ParseSettings.Settings.Handler.CheckActiveHandler,
        ZeroHeightWeight = ParseSettings.Settings.HOMETransfer.ZeroHeightWeight
    };

    public static void SetDefaultlegalitySettings() => SetLegalitySettings(new()
    {
        CheckActiveHandler = false,
        ZeroHeightWeight = Severity.Fishy
    });

    public static void SetLegalitySettings(LegalitySettings settings)
    {
        ParseSettings.Settings.Handler.CheckActiveHandler = settings.CheckActiveHandler;
        ParseSettings.Settings.HOMETransfer.ZeroHeightWeight = settings.ZeroHeightWeight;
    }

    public struct LegalitySettings
    {
        public required bool CheckActiveHandler;
        public required Severity ZeroHeightWeight;
    }
}
