using System.Diagnostics;
using Octokit;

namespace TeraFinder
{
    public static class GitHubUtil
    {
        public static async Task TryUpdate(string language)
        {
            var strings = new Dictionary<string, string>
            {
                { "Update.Popup", "" },
                { "Update.Message", "" }
            }.TranslateInnerStrings(language);

            if (await IsUpdateAvailable())
            {
                var result = MessageBox.Show(strings["Update.Message"], strings["Update.Popup"], MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo { FileName = @"https://github.com/Manu098vm/Tera-Finder/releases/latest", UseShellExecute = true } );
            }
        }

        private static async Task<bool> IsUpdateAvailable()
        {
            var currentVersion = ParseVersion(GetPluginVersion());
            var latestVersion = ParseVersion(await GetLatestVersion());

            if (latestVersion[0] > currentVersion[0])
                return true;
            else if (latestVersion[0] == currentVersion[0])
            {
                if (latestVersion[1] > currentVersion[1])
                    return true;
                else if (latestVersion[1] == currentVersion[1])
                {
                    if (latestVersion[2] > currentVersion[2])
                        return true;
                }
            }
            return false;
        }

        private static string GetPluginVersion() => TeraPlugin.Version;

        private static async Task<string> GetLatestVersion()
        {
            try
            {
                return await GetLatest();
            }
            catch (Exception)
            {
                return "0.0.0";
            }
        }

        private static async Task<string> GetLatest()
        {
            var client = new GitHubClient(new ProductHeaderValue("Tera-Finder"));
            var release = await client.Repository.Release.GetLatest("Manu098vm", "Tera-Finder");
            return release.Name;
        }

        private static int[] ParseVersion(string version)
        {
            var v = new int[3];
            v[0] = int.Parse($"{version[0]}");
            v[1] = int.Parse($"{version[2]}");
            v[2] = int.Parse($"{version[4]}");
            return v;
        }
    }
}
