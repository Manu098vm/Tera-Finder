using PKHeX.Core;
using System.IO.Compression;

namespace TeraFinder
{
    internal static class ImportUtil
    {
        public static bool ImportNews(SaveFile sav, string path = "", bool plugin = false)
        {
            var valid = false;
            var zip = false;

            if (sav is FakeSaveFile)
                return false;

            if (sav is not SAV9SV)
                return false;

            if (path.Equals(""))
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Open Poké Portal News Zip file or Folder",
                    Filter = "All files (*.*)|*.*",
                    FileName = "Folder Selection",
                    ValidateNames = false,
                    CheckFileExists = false,
                    CheckPathExists = true,
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!Path.GetExtension(dialog.FileName).Equals(".zip"))
                        path = Path.GetDirectoryName(dialog.FileName)!;
                    else
                        path = dialog.FileName;
                }
                else return false;
            }

            if (File.Exists(path))
                if (Path.GetExtension(path).Equals(".zip"))
                {
                    var tmp = $"{Path.GetDirectoryName(path)}\\tmp";
                    ZipFile.ExtractToDirectory(path, tmp);
                    path = tmp;
                    zip = true;
                }

            if (Directory.Exists(path))
                if (IsValidFolder(path))
                    valid = true;

            if (valid)
                return FinalizeImport(path, sav, zip);

            if (plugin || zip)
            {
                if (zip) DeleteFilesAndDirectory(path);
                MessageBox.Show("Invalid file(s). Aborted.");
            }

            return false;
        }

        public static bool IsValidFolder(string path)
        {
            if (!File.Exists($"{path}\\Identifier.txt"))
                return false;
            if (!File.Exists($"{path}\\Files\\event_raid_identifier"))
                return false;
            if (!File.Exists($"{path}\\Files\\fixed_reward_item_array"))
                return false;
            if (!File.Exists($"{path}\\Files\\lottery_reward_item_array"))
                return false;
            if (!File.Exists($"{path}\\Files\\raid_enemy_array"))
                return false;
            if (!File.Exists($"{path}\\Files\\raid_priority_array"))
                return false;

            return true;
        }

        public static bool FinalizeImport(string path, SaveFile sv, bool zip)
        {
            try
            {
                var index = File.ReadAllText($"{path}\\Identifier.txt");
                var identifierBlock = File.ReadAllBytes($"{path}\\Files\\event_raid_identifier");
                var rewardItemBlock = File.ReadAllBytes($"{path}\\Files\\fixed_reward_item_array");
                var lotteryItemBlock = File.ReadAllBytes($"{path}\\Files\\lottery_reward_item_array");
                var raidEnemyBlock = File.ReadAllBytes($"{path}\\Files\\raid_enemy_array");
                var raidPriorityBlock = File.ReadAllBytes($"{path}\\Files\\raid_priority_array");

                if (zip) DeleteFilesAndDirectory(path);

                var sav = (SAV9SV)sv;

                var KBCATEventRaidIdentifier = sav.AllBlocks.Where(block => block.Key == 0x37B99B4D).FirstOrDefault();
                var KBCATFixedRewardItemArray = sav.AllBlocks.Where(block => block.Key == 0x7D6C2B82).FirstOrDefault();
                var KBCATLotteryRewardItemArray = sav.AllBlocks.Where(block => block.Key == 0xA52B4811).FirstOrDefault();
                var KBCATRaidEnemyArray = sav.AllBlocks.Where(block => block.Key == 0x0520A1B0).FirstOrDefault();
                var KBCATRaidPriorityArray = sav.AllBlocks.Where(block => block.Key == 0x095451E4).FirstOrDefault();

                if (KBCATEventRaidIdentifier is not null)
                    KBCATEventRaidIdentifier.ChangeData(identifierBlock);
                if (KBCATFixedRewardItemArray is not null)
                    KBCATFixedRewardItemArray.ChangeData(rewardItemBlock);
                if (KBCATLotteryRewardItemArray is not null)
                    KBCATLotteryRewardItemArray.ChangeData(lotteryItemBlock);
                if(KBCATRaidEnemyArray is not null)
                    KBCATRaidEnemyArray.ChangeData(raidEnemyBlock);
                if (KBCATRaidPriorityArray is not null)
                    KBCATRaidPriorityArray.ChangeData(raidPriorityBlock);

                if(KBCATRaidEnemyArray is not null)
                    MessageBox.Show($"Succesfully imported Raid Event [{index}]!");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import error! Is the provided file valid?\n{ex}");
                return false;
            }
        }

        private static void DeleteFilesAndDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
                DeleteFilesAndDirectory(dir);

            Directory.Delete(targetDir, false);
        }
    }
}
