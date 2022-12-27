using PKHeX.Core;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace TeraFinder
{
    internal static class ImportUtil
    {
        public static bool ImportNews(SaveFile sav, 
                                    ref EncounterRaid9[]? dist,
                                    ref EncounterRaid9[]? mighty,
                                    ref Dictionary<ulong, List<Reward>>? distFixedRewards,
                                    ref Dictionary<ulong, List<Reward>>? distLotteryRewards,
                                    string path = "",
                                    bool plugin = false)
        {
            var valid = false;
            var zip = false;

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
                return FinalizeImport(path, sav, zip, ref dist, ref mighty, ref distFixedRewards, ref distLotteryRewards);

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

        public static bool FinalizeImport(string path, 
                                          SaveFile sv, 
                                          bool zip,
                                          ref EncounterRaid9[]? dist,
                                          ref EncounterRaid9[]? mighty,
                                          ref Dictionary<ulong, List<Reward>>? distFixedRewards,
                                          ref Dictionary<ulong, List<Reward>>? distLotteryRewards)
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

                var KBCATEventRaidIdentifier = sav.Accessor.FindOrDefault(EventRaidBlocks.KBCATEventRaidIdentifier.Key);
                var KBCATFixedRewardItemArray = sav.Accessor.FindOrDefault(EventRaidBlocks.KBCATFixedRewardItemArray.Key);
                var KBCATLotteryRewardItemArray = sav.Accessor.FindOrDefault(EventRaidBlocks.KBCATLotteryRewardItemArray.Key);
                var KBCATRaidEnemyArray = sav.Accessor.FindOrDefault(EventRaidBlocks.KBCATRaidEnemyArray.Key);
                var KBCATRaidPriorityArray = sav.Accessor.FindOrDefault(EventRaidBlocks.KBCATRaidPriorityArray.Key);

                if (KBCATEventRaidIdentifier.Type is not SCTypeCode.None)
                    KBCATEventRaidIdentifier.ChangeData(identifierBlock);
                else
                    BlockUtil.EditBlock(KBCATEventRaidIdentifier, SCTypeCode.Object, identifierBlock);

                if (KBCATFixedRewardItemArray.Type is not SCTypeCode.None)
                    KBCATFixedRewardItemArray.ChangeData(rewardItemBlock);
                else
                    BlockUtil.EditBlock(KBCATFixedRewardItemArray, SCTypeCode.Object, rewardItemBlock);

                if (KBCATLotteryRewardItemArray.Type is not SCTypeCode.None)
                    KBCATLotteryRewardItemArray.ChangeData(lotteryItemBlock);
                else
                    BlockUtil.EditBlock(KBCATLotteryRewardItemArray, SCTypeCode.Object, lotteryItemBlock);

                if (KBCATRaidEnemyArray.Type is not SCTypeCode.None)
                    KBCATRaidEnemyArray.ChangeData(raidEnemyBlock);
                else
                    BlockUtil.EditBlock(KBCATRaidEnemyArray, SCTypeCode.Object, raidEnemyBlock);

                if (KBCATRaidPriorityArray.Type is not SCTypeCode.None)
                    KBCATRaidPriorityArray.ChangeData(raidPriorityBlock);
                else
                    BlockUtil.EditBlock(KBCATRaidPriorityArray, SCTypeCode.Object, raidPriorityBlock);

                var events = TeraUtil.GetSAVDistEncounters(sav);
                var eventsrewards = RewardUtil.GetDistRewardsTables(sav);
                dist = events[0];
                mighty = events[1];
                distFixedRewards = eventsrewards[0];
                distLotteryRewards = eventsrewards[1];

                if (KBCATRaidEnemyArray is not null)
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
