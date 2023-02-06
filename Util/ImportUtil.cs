﻿using PKHeX.Core;
using System.IO.Compression;

namespace TeraFinder
{
    internal static class ImportUtil
    {
        public static bool ImportNews(SaveFile sav, 
                                    ref EncounterRaid9[]? dist,
                                    ref EncounterRaid9[]? mighty,
                                    ref Dictionary<ulong, List<Reward>>? distFixedRewards,
                                    ref Dictionary<ulong, List<Reward>>? distLotteryRewards,
                                    string language,
                                    string path = "",
                                    bool plugin = false)
        {
            var valid = false;
            var zip = false;

            if (sav is not SAV9SV)
                return false;

            var strings = GenerateDictionary().TranslateInnerStrings(language);

            if (path.Equals(""))
            {
                var dialog = new OpenFileDialog
                {
                    Title = strings["ImportNews.Title"],
                    Filter = $"{strings["ImportNews.Filter"]} (*.*)|*.*",
                    FileName = strings["ImportNews.FolderSelection"],
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
                return FinalizeImport(path, sav, zip, ref dist, ref mighty, ref distFixedRewards, ref distLotteryRewards, strings);

            if (plugin || zip)
            {
                if (zip) DeleteFilesAndDirectory(path);
                MessageBox.Show(strings["ImportNews.InvalidSelection"]);
            }

            return false;
        }

        private static bool IsValidFolder(string path)
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

        private static bool FinalizeImport(string path, 
                                          SaveFile sv, 
                                          bool zip,
                                          ref EncounterRaid9[]? dist,
                                          ref EncounterRaid9[]? mighty,
                                          ref Dictionary<ulong, List<Reward>>? distFixedRewards,
                                          ref Dictionary<ulong, List<Reward>>? distLotteryRewards,
                                          Dictionary<string, string> strings)
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

                var KBCATEventRaidIdentifier = sav.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);
                var KBCATFixedRewardItemArray = sav.Accessor.FindOrDefault(Blocks.KBCATFixedRewardItemArray.Key);
                var KBCATLotteryRewardItemArray = sav.Accessor.FindOrDefault(Blocks.KBCATLotteryRewardItemArray.Key);
                var KBCATRaidEnemyArray = sav.Accessor.FindOrDefault(Blocks.KBCATRaidEnemyArray.Key);
                var KBCATRaidPriorityArray = sav.Accessor.FindOrDefault(Blocks.KBCATRaidPriorityArray.Key);

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
                    MessageBox.Show($"{strings["ImportNews.Success"]} [{index}]!");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{strings["ImportNews.Error"]}\n{ex}");
                return false;
            }
        }

        private static Dictionary<string, string> GenerateDictionary()
        {
            var strings = new Dictionary<string, string>
            {
                { "ImportNews.Title", "Open Poké Portal News Zip file or Folder" },
                { "ImportNews.Filter", "All files" },
                { "ImportNews.FolderSelection", "Folder Selection" },
                { "ImportNews.InvalidSelection", "Invalid file(s). Aborted." },
                { "ImportNews.Success", "Succesfully imported Raid Event" },
                { "ImportNews.Error", "Import error! Is the provided file valid?" },
            };
            return strings;
        }
    }
}
