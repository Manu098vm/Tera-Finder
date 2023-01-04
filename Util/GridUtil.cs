using PKHeX.Core;
using System.Text;
using TeraFinder.Forms;

namespace TeraFinder
{
    public static class GridUtil
    {
        public static void SaveAllTxt(this DataGridView dataGrid)
        {
            if (dataGrid.Rows.Count > 0)
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "TXT (*.txt)|*.txt",
                    FileName = "terafinder.txt"
                };
                var error = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            error = true;
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (!error)
                    {
                        try
                        {
                            var columnCount = dataGrid.Columns.Count;
                            var columnNames = "";
                            var outputTxt = new string[dataGrid.Rows.Count + 1];
                            for (var i = 0; i < columnCount; i++)
                                columnNames += dataGrid.Columns[i].HeaderText.ToString() + "\t";
                            outputTxt[0] += columnNames;

                            for (var i = 1; (i - 1) < dataGrid.Rows.Count; i++)
                                for (var j = 0; j < columnCount; j++)
                                    outputTxt[i] += Convert.ToString(dataGrid.Rows[i - 1].Cells[j].Value) + "\t";

                            File.WriteAllLines(sfd.FileName, outputTxt, Encoding.UTF8);
                            MessageBox.Show($"Exported to {sfd.FileName}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
                MessageBox.Show("No data available.");
        }

        public static void SaveSelectedTxt(this DataGridView dataGrid)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count > 0)
                {
                    var sfd = new SaveFileDialog
                    {
                        Filter = "TXT (*.txt)|*.txt",
                        FileName = "terafinder.txt"
                    };
                    var error = false;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        if (File.Exists(sfd.FileName))
                        {
                            try
                            {
                                File.Delete(sfd.FileName);
                            }
                            catch (IOException ex)
                            {
                                error = true;
                                MessageBox.Show(ex.Message);
                            }
                        }
                        if (!error)
                        {
                            try
                            {
                                var columnCount = dataGrid.Columns.Count;
                                var columnNames = "";
                                var outputTxt = new string[count + 1];
                                for (var i = 0; i < columnCount; i++)
                                    columnNames += dataGrid.Columns[i].HeaderText.ToString() + "\t";
                                outputTxt[0] += columnNames;

                                for (var i = 0; i < count; i++)
                                    for (var j = 0; j < columnCount; j++)
                                        if (count > 1 && int.Parse(selectedRows.ElementAt(0).Cells[columnCount-1].Value.ToString()!) > int.Parse(selectedRows.ElementAt(1).Cells[columnCount-1].Value.ToString()!))
                                            outputTxt[i + 1] += Convert.ToString(selectedRows.ElementAt(count - (i + 1)).Cells[j].Value) + "\t";
                                        else
                                            outputTxt[i + 1] += Convert.ToString(selectedRows.ElementAt(i).Cells[j].Value) + "\t";

                                File.WriteAllLines(sfd.FileName, outputTxt, Encoding.UTF8);
                                MessageBox.Show($"Exported to {sfd.FileName}");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No data available.");
                }
            }
            else
                MessageBox.Show("No data available.");
        }

        public static void SaveSelectedPk9(this DataGridView dataGrid, CalculatorForm f)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count == 1)
                {
                    try
                    {
                        var template = new PK9(Properties.Resources.template);
                        var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                        var content = GetContent(seed, selectedRows.ElementAt(0), f);
                        var progress = GetProgress(seed, selectedRows.ElementAt(0), f);

                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)GetGameVersion(seed, selectedRows.ElementAt(0), f);

                        var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter!);

                        template.Species = rngres.Species;
                        template.Form = rngres.Form;
                        if (rngres.Stars == 7) template.RibbonMarkMightiest = true;
                        template.Met_Level = rngres.Level;
                        template.CurrentLevel = rngres.Level;
                        template.Obedience_Level = (byte)rngres.Level;
                        template.TeraTypeOriginal = (MoveType)rngres.TeraType;
                        template.EncryptionConstant = rngres.EC;
                        template.TrainerID7 = Int32.Parse(f.txtTID.Text);
                        template.TrainerSID7 = Int32.Parse(f.txtSID.Text);
                        template.Version = f.cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.VL;
                        template.Language = (byte)sav.Language;
                        template.HT_Name = sav.OT;
                        template.HT_Language = (byte)sav.Language;
                        template.OT_Name = sav.OT;
                        template.PID = rngres.PID;
                        template.IV_HP = rngres.HP;
                        template.IV_ATK = rngres.ATK;
                        template.IV_DEF = rngres.DEF;
                        template.IV_SPA = rngres.SPA;
                        template.IV_SPD = rngres.SPD;
                        template.IV_SPE = rngres.SPE;
                        template.Ability = rngres.Ability;
                        template.AbilityNumber = rngres.GetAbilityNumber() == 3 ? 4 : rngres.GetAbilityNumber();
                        template.Nature = rngres.Nature;
                        template.Gender = (int)rngres.Gender;
                        template.HeightScalar = rngres.Height;
                        template.WeightScalar = rngres.Weight;
                        template.Scale = rngres.Scale;
                        template.Move1 = rngres.Move1;
                        template.Move2 = rngres.Move2;
                        template.Move3 = rngres.Move3;
                        template.Move4 = rngres.Move4;
                        template.HealPP();
                        template.ClearNickname();

                        var la = new LegalityAnalysis(template);
                        if (!la.Valid)
                        {
                            var ability = la.Results.Where(l => l.Identifier is CheckIdentifier.Ability).FirstOrDefault();
                            if (ability is not null && !ability.Valid)
                                for (var i = 0; i <= 4 && !la.Valid; i++)
                                {
                                    template.AbilityNumber = i;
                                    i++;
                                    la = new LegalityAnalysis(template);
                                }
                        }

                        if (!la.Valid)
                        {
                            MessageBox.Show($"Error while parsing the template. Please report this error to the plugin author.\n{la.Report()}");
                            return;
                        }

                        var sfd = new SaveFileDialog
                        {
                            Filter = "PK9 (*.pk9)|*.pk9",
                            FileName = $"{template.FileName}",
                        };

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            if (File.Exists(sfd.FileName))
                            {
                                try
                                {
                                    File.Delete(sfd.FileName);
                                }
                                catch (IOException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            File.WriteAllBytes(sfd.FileName, template.Data);
                            MessageBox.Show($"Exported to {sfd.FileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Rows selection exceeded the maximum allowed [1].");
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }
        }

        public static void ViewRewards(this DataGridView dataGrid, CalculatorForm f)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count == 1)
                {
                    try
                    {
                        var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                        var content = GetContent(seed, selectedRows.ElementAt(0), f);
                        var progress = GetProgress(seed, selectedRows.ElementAt(0), f);

                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)GetGameVersion(seed, selectedRows.ElementAt(0), f);

                        var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter!);

                        var lvl0 = RewardUtil.GetRewardList(rngres, encounter!.FixedRewardHash, encounter!.LotteryRewardHash,
                            encounter!.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards, encounter!.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards, 0);
                        var lvl1 = RewardUtil.GetRewardList(rngres, encounter!.FixedRewardHash, encounter!.LotteryRewardHash,
                            encounter!.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards, encounter!.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards, 1);
                        var lvl2 = RewardUtil.GetRewardList(rngres, encounter!.FixedRewardHash, encounter!.LotteryRewardHash,
                            encounter!.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards, encounter!.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards, 2);
                        var lvl3 = RewardUtil.GetRewardList(rngres, encounter!.FixedRewardHash, encounter!.LotteryRewardHash,
                            encounter!.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards, encounter!.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards, 3);

                        var form = new RewardListForm(f.Editor.Language, lvl0, lvl1, lvl2, lvl3);
                        form.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Rows selection exceeded the maximum allowed [1].");
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }
        }

        public static void SendSelectedRaidEditor(this DataGridView dataGrid, CalculatorForm f)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count == 1)
                {
                    try
                    {
                        var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                        var content = GetContent(seed, selectedRows.ElementAt(0), f);
                        f.Editor.txtSeed.Text = $"{seed:X8}";
                        f.Editor.cmbContent.SelectedIndex = (int)content;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Rows selection exceeded the maximum allowed [1].");
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }
        }

        public static void SendSelectedRaidEditor(this DataGridView dataGrid, RewardCalcForm f)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count == 1)
                {
                    try
                    {
                        var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                        var content = GetContent(seed, selectedRows.ElementAt(0), f);
                        f.Editor.txtSeed.Text = $"{seed:X8}";
                        f.Editor.cmbContent.SelectedIndex = (int)content;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Rows selection exceeded the maximum allowed [1].");
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }
        }

        public static void SendSelectedPk9Editor(this DataGridView dataGrid, CalculatorForm f)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
                var count = selectedRows.Count();
                if (count == 1)
                {
                    try
                    {
                        var template = new PK9(Properties.Resources.template);
                        var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                        var content = GetContent(seed, selectedRows.ElementAt(0), f);
                        var progress = GetProgress(seed, selectedRows.ElementAt(0), f);

                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)GetGameVersion(seed, selectedRows.ElementAt(0), f);

                        var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                            content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter!);

                        template.Species = rngres.Species;
                        template.Form = rngres.Form;
                        if (rngres.Stars == 7) template.RibbonMarkMightiest = true;
                        template.Met_Level = rngres.Level;
                        template.CurrentLevel = rngres.Level;
                        template.Obedience_Level = (byte)rngres.Level;
                        template.TeraTypeOriginal = (MoveType)rngres.TeraType;
                        template.EncryptionConstant = rngres.EC;
                        template.TrainerID7 = Int32.Parse(f.txtTID.Text);
                        template.TrainerSID7 = Int32.Parse(f.txtSID.Text);
                        template.Version = f.cmbGame.SelectedIndex == 0 ? (int)GameVersion.SL : (int)GameVersion.VL;
                        template.Language = (byte)sav.Language;
                        template.HT_Name = sav.OT;
                        template.HT_Language = (byte)sav.Language;
                        template.OT_Name = sav.OT;
                        template.PID = rngres.PID;
                        template.IV_HP = rngres.HP;
                        template.IV_ATK = rngres.ATK;
                        template.IV_DEF = rngres.DEF;
                        template.IV_SPA = rngres.SPA;
                        template.IV_SPD = rngres.SPD;
                        template.IV_SPE = rngres.SPE;
                        template.Ability = rngres.Ability;
                        template.AbilityNumber = rngres.GetAbilityNumber() == 3 ? 4 : rngres.GetAbilityNumber();
                        template.Nature = rngres.Nature;
                        template.Gender = (int)rngres.Gender;
                        template.HeightScalar = rngres.Height;
                        template.WeightScalar = rngres.Weight;
                        template.Scale = rngres.Scale;
                        template.Move1 = rngres.Move1;
                        template.Move2 = rngres.Move2;
                        template.Move3 = rngres.Move3;
                        template.Move4 = rngres.Move4;
                        template.HealPP();
                        template.ClearNickname();

                        var la = new LegalityAnalysis(template);
                        if (!la.Valid)
                        {
                            var ability = la.Results.Where(l => l.Identifier is CheckIdentifier.Ability).FirstOrDefault();
                            if (ability is not null && !ability.Valid) 
                                for(var i = 0; i <= 4 && !la.Valid; i++)
                                {
                                    template.AbilityNumber = i;
                                    i++;
                                    la = new LegalityAnalysis(template);
                                }
                        }

                        if (!la.Valid)
                        {
                            MessageBox.Show($"Error while parsing the template. Please report this error to the plugin author.\n{la.Report()}");
                            return;
                        }

                        f.Editor.PKMEditor!.PopulateFields(template, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Rows selection exceeded the maximum allowed [1].");
                }
            }
            else
            {
                MessageBox.Show("No data available.");
            }
        }

        private static GameProgress GetProgress(uint seed, DataGridViewRow row, CalculatorForm f)
        {
            for (var content = RaidContent.Standard; content <= RaidContent.Event_Mighty; content++)
            {
                for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.None; progress++)
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)game;
                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        if (encounter is not null)
                        {
                            var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter);
                            var success = true;

                            if (rngres != null)
                            {
                                var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode);
                                var grid = new GridEntry(ConvertToString(row)).GetStrings();
                                for (var i = 0; i < entry.Length - 1; i++)
                                    if (!entry[i].Equals(grid[i]))
                                        success = false;
                            }

                            if (success)
                                return progress;
                        }
                    }
                }
            }
            return (GameProgress)f.cmbProgress.SelectedIndex;
        }

        private static RaidContent GetContent(uint seed, DataGridViewRow row, CalculatorForm f)
        {
            for (var content = RaidContent.Standard; content <= RaidContent.Event_Mighty; content++)
            {
                for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.None; progress++)
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)game;

                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        if (encounter is not null)
                        {
                            var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter);
                            var success = true;

                            if (rngres != null)
                            {
                                var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode);
                                var grid = new GridEntry(ConvertToString(row)).GetStrings();
                                for (var i = 0; i < entry.Length - 1; i++)
                                    if (!entry[i].Equals(grid[i]))
                                        success = false;
                            }

                            if (success)
                            {
                                var type = content;
                                if (progress is GameProgress.None)
                                    type = RaidContent.Black;
                                return type;
                            }
                        }
                    }
                }
            }
            return (RaidContent)f.cmbContent.SelectedIndex;
        }

        private static RaidContent GetContent(uint seed, DataGridViewRow row, RewardCalcForm f)
        {
            for (var content = RaidContent.Standard; content <= RaidContent.Event_Mighty; content++)
            {
                for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.None; progress++)
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)game;

                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        if (encounter is not null)
                        {
                            var fixedlist = encounter.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards;
                            var lotterylist = encounter.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards;
                            var accuratesearch = true;

                            foreach(var str in ConvertToString(row))
                            {
                                if(RewardUtil.TeraShard.Contains(str) || RewardUtil.Material.Contains(str))
                                {
                                    accuratesearch = false;
                                    break;
                                }
                            }

                            var rngres = accuratesearch ? RewardUtil.GetRewardList(TeraUtil.CalcRNG(seed, sav.TrainerID7, sav.TrainerSID7, content, encounter),
                                encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist) :
                                RewardUtil.GetRewardList(seed, encounter.Stars, encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist);
                            var success = true;

                            if (rngres != null)
                            {
                                var strlist = new List<string>();

                                strlist.Add($"{seed:X8}");
                                foreach(var item in rngres)
                                    strlist.Add(item.GetItemName(f.Items, f.Editor.Language, true));


                                var entry = strlist.ToArray();
                                var grid = new RewardGridEntry(ConvertToRewardString(row)).GetStrings();
                                for (var i = 0; i < entry.Length - 1; i++)
                                    if (!entry[i].Equals(grid[i]))
                                        success = false;
                            }

                            if (success)
                            {
                                var type = content;
                                if (progress is GameProgress.None)
                                    type = RaidContent.Black;
                                return type;
                            }
                        }
                    }
                }
            }
            return (RaidContent)f.cmbContent.SelectedIndex;
        }

        private static GameVersion GetGameVersion(uint seed, DataGridViewRow row, CalculatorForm f)
        {
            for (var content = RaidContent.Standard; content <= RaidContent.Event_Mighty; content++)
            {
                for (var progress = GameProgress.UnlockedTeraRaids; progress <= GameProgress.None; progress++)
                {
                    for (var game = GameVersion.SL; game <= GameVersion.VL; game++)
                    {
                        var sav = (SAV9SV)f.Editor.SAV.Clone();
                        sav.Game = (int)game;
                        var encounter = content < RaidContent.Event ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, true) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, false);

                        if (encounter is not null)
                        {
                            var rngres = TeraUtil.CalcRNG(seed, Int32.Parse(f.txtTID.Text), Int32.Parse(f.txtSID.Text), content, encounter);
                            var success = true;

                            if (rngres != null)
                            {
                                var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode);
                                var grid = new GridEntry(ConvertToString(row)).GetStrings();
                                for (var i = 0; i < entry.Length - 1; i++)
                                    if (!entry[i].Equals(grid[i]))
                                        success = false;
                            }

                            if (success)
                                return game;
                        }
                    }
                }
            }
            return f.cmbGame.SelectedIndex == 0 ? GameVersion.SL : GameVersion.VL;
        }

        private static string[] ConvertToString(this DataGridViewRow row)
        {
            var columnCount = row.Cells.Count;
            var output = new string[columnCount];
            for (var i = 0; i < columnCount; i++)
                output[i] = Convert.ToString(row.Cells[i].Value)!;
            return output;
        }

        private static string[] ConvertToRewardString(this DataGridViewRow row)
        {
            var cellcount = row.Cells.Count;
            var output = new string[22];
            for (var i = 0; i < 22; i++)
                output[i] = cellcount > i ? Convert.ToString(row.Cells[i].Value)! : "";
            return output;
        }

    }
}
