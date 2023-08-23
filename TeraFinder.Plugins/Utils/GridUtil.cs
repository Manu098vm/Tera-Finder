using NLog;
using PKHeX.Core;
using System.Text;
using TeraFinder.Core;

namespace TeraFinder.Plugins;

public static class GridUtil
{
    private static Dictionary<string, string> GenerateDictionary(string language)
    {
        return new Dictionary<string, string>
        {
            { "GridUtil.Exported", "" },
            { "GridUtil.NoData", "" },
            { "GridUtil.ErrorParsing", "" },
            { "GridUtil.MissingData", "" },
            { "GridUtil.CheckWiki", "" },
            { "GridUtil.Report", "" },
            { "GridUtil.RowsExceeded", "" },
            { "GridUtil.MismatchGroupID", "" },
        }.TranslateInnerStrings(language);
    }

    public static void SaveAllTxt(this DataGridView dataGrid, string language)
    {
        var strings = GenerateDictionary(language);
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
                        MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        else
            MessageBox.Show(strings["GridUtil.NoData"]);
    }

    public static void SaveSelectedTxt(this DataGridView dataGrid, string language)
    {
        var strings = GenerateDictionary(language);
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
                                    if (count > 1 && Convert.ToUInt32((string)selectedRows.ElementAt(0).Cells[columnCount - 1].Value, 10) > Convert.ToUInt32((string)selectedRows.ElementAt(1).Cells[columnCount - 1].Value, 10))
                                        outputTxt[i + 1] += Convert.ToString(selectedRows.ElementAt(count - (i + 1)).Cells[j].Value) + "\t";
                                    else
                                        outputTxt[i + 1] += Convert.ToString(selectedRows.ElementAt(i).Cells[j].Value) + "\t";

                            File.WriteAllLines(sfd.FileName, outputTxt, Encoding.UTF8);
                            MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
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
                MessageBox.Show(strings["GridUtil.NoData"]);
            }
        }
        else
            MessageBox.Show(strings["GridUtil.NoData"]);
    }

    public static void SaveSelectedPk9(this DataGridView dataGrid, CalculatorForm f, string language)
    {
        var strings = GenerateDictionary(language);
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            var count = selectedRows.Count();
            if (count == 1)
            {
                try
                {
                    var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 2].Value.ToString()!, 10);
                    var content = GetContent(seed, groupid, selectedRows.ElementAt(0), f);
                    var progress = GetProgress(seed, groupid, selectedRows.ElementAt(0), f);
                    var tid = Convert.ToUInt32(f.txtTID.Text, 10);
                    var sid = Convert.ToUInt32(f.txtSID.Text, 10);

                    var sav = (SAV9SV)f.Editor.SAV.Clone();
                    sav.Game = (int)GetGameVersion(seed, groupid, selectedRows.ElementAt(0), f);

                    var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, groupid) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, groupid);

                    var res = TeraUtil.GenerateTeraEntity(sav, encounter!, content, seed, tid, sid, groupid);
                    var la = new LegalityAnalysis(res);

                    if (!la.Valid)
                    {
                        var la_encounter = la.Results.Where(l => l.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
                        if (content is RaidContent.Event or RaidContent.Event_Mighty)
                            MessageBox.Show($"{strings["GridUtil.ErrorParsing"]}\n{strings["GridUtil.MissingData"]} [{encounter!.Identifier}].\n{strings["GridUtil.CheckWiki"]}");
                        else
                            MessageBox.Show($"{strings["GridUtil.ErrorParsing"]} {strings["GridUtil.Report"]}\n{la.Report()}");
                        return;
                    }

                    var sfd = new SaveFileDialog
                    {
                        Filter = "PK9 (*.pk9)|*.pk9",
                        FileName = $"{res.FileName}",
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
                        File.WriteAllBytes(sfd.FileName, res.Data);
                        MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show(strings["GridUtil.RowsExceeded"]);
            }
        }
        else
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
        }
    }

    public static void ViewRewards(this DataGridView dataGrid, CalculatorForm f, string language)
    {
        var strings = GenerateDictionary(language);
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            var count = selectedRows.Count();
            if (count == 1)
            {
                try
                {
                    var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 2].Value.ToString()!, 10);
                    var content = GetContent(seed, groupid, selectedRows.ElementAt(0), f);
                    var progress = GetProgress(seed, groupid, selectedRows.ElementAt(0), f);

                    var sav = (SAV9SV)f.Editor.SAV.Clone();
                    sav.Game = (int)GetGameVersion(seed, groupid, selectedRows.ElementAt(0), f);

                    var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!);

                    var rngres = TeraUtil.CalcRNG(seed, Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10), content, encounter!, groupid);

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
                MessageBox.Show(strings["GridUtil.RowsExceeded"]);
            }
        }
        else
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
        }
    }

    public static void SendSelectedRaidEditor(this DataGridView dataGrid, CalculatorForm f, string language)
    {
        var strings = GenerateDictionary(language);
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            var count = selectedRows.Count();
            if (count == 1)
            {
                try
                {
                    var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 2].Value.ToString()!, 10);
                    var content = GetContent(seed, groupid, selectedRows.ElementAt(0), f);

                    if (content is RaidContent.Event or RaidContent.Event_Mighty)
                    {
                        if (f.Editor.CurrTera!.EventIndex != groupid)
                        {
                            MessageBox.Show($"{strings["GridUtil.MismatchGroupID"].Replace("{editorIndex}", $"{f.Editor.CurrTera!.EventIndex}").Replace("{resultIndex}", $"{groupid}")}");
                            return;
                        }
                    }

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
                MessageBox.Show(strings["GridUtil.RowsExceeded"]);
            }
        }
        else
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
        }
    }

    public static void SendSelectedRaidEditor(this DataGridView dataGrid, RewardCalcForm f, string language)
    {
        var strings = GenerateDictionary(language);
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            var count = selectedRows.Count();
            if (count == 1)
            {
                try
                {
                    var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 2].Value.ToString()!, 10);
                    var content = GetContent(seed, groupid, selectedRows.ElementAt(0), f);

                    if (content is RaidContent.Event or RaidContent.Event_Mighty)
                    {
                        if (f.Editor.CurrTera!.EventIndex != groupid)
                        {
                            MessageBox.Show($"{strings["GridUtil.MismatchGroupID"].Replace("{editorIndex}", $"{f.Editor.CurrTera!.EventIndex}").Replace("{resultIndex}", $"{groupid}")}");
                            return;
                        }
                    }

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
                MessageBox.Show(strings["GridUtil.RowsExceeded"]);
            }
        }
        else
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
        }
    }

    public static void SendSelectedPk9Editor(this DataGridView dataGrid, CalculatorForm f, string language)
    {
        var strings = GenerateDictionary(language);
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
            var count = selectedRows.Count();
            if (count == 1)
            {
                try
                {
                    var seed = Convert.ToUInt32(selectedRows.ElementAt(0).Cells[0].Value.ToString()!, 16);
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 2].Value.ToString()!, 10);
                    var content = GetContent(seed, groupid, selectedRows.ElementAt(0), f);
                    var progress = GetProgress(seed, groupid, selectedRows.ElementAt(0), f);
                    var tid = Convert.ToUInt32(f.txtTID.Text, 10);
                    var sid = Convert.ToUInt32(f.txtSID.Text, 10);

                    var sav = (SAV9SV)f.Editor.SAV.Clone();
                    sav.Game = (int)GetGameVersion(seed, groupid, selectedRows.ElementAt(0), f);

                    var encounter = (int)content < 2 ? TeraUtil.GetTeraEncounter(seed, sav, TeraUtil.GetStars(seed, progress), f.Editor.Tera!) :
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!, groupid) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!, groupid);

                    var res = TeraUtil.GenerateTeraEntity(sav, encounter!, content, seed, tid, sid, groupid);
                    var la = new LegalityAnalysis(res);

                    if (!la.Valid)
                    {
                        var la_encounter = la.Results.Where(l => l.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
                        if (content is RaidContent.Event or RaidContent.Event_Mighty)
                            MessageBox.Show($"{strings["GridUtil.ErrorParsing"]}\n{strings["GridUtil.MissingData"]} [{encounter!.Identifier}].\n{strings["GridUtil.CheckWiki"]}");
                        else
                            MessageBox.Show($"{strings["GridUtil.ErrorParsing"]} {strings["GridUtil.Report"]}\n{la.Report()}");
                        return;
                    }

                    f.Editor.PKMEditor!.PopulateFields(res, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show(strings["GridUtil.RowsExceeded"]);
            }
        }
        else
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
        }
    }

    private static GameProgress GetProgress(uint seed, int groupid, DataGridViewRow row, CalculatorForm f)
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
                    content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!);

                    if (encounter is not null)
                    {
                        var rngres = TeraUtil.CalcRNG(seed, Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10), content, encounter, groupid);
                        var success = true;

                        if (rngres != null)
                        {
                            var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode, f.ShinyList);
                            var grid = new GridEntry(row.ConvertToString()).GetStrings();
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

    private static RaidContent GetContent(uint seed, int groupid, DataGridViewRow row, CalculatorForm f)
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
                    content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!);

                    if (encounter is not null)
                    {
                        var rngres = TeraUtil.CalcRNG(seed, Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10), content, encounter, groupid);
                        var success = true;

                        if (rngres != null)
                        {
                            var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode, f.ShinyList);
                            var grid = new GridEntry(row.ConvertToString()).GetStrings();
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

    private static RaidContent GetContent(uint seed, int groupid, DataGridViewRow row, RewardCalcForm f)
    {
        foreach (var accuratesearch in new[] { true, false })
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
                        content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!) :
                            TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!);

                        if (encounter is not null)
                        {
                            var fixedlist = encounter.IsDistribution ? f.Editor.DistFixedRewards : f.Editor.TeraFixedRewards;
                            var lotterylist = encounter.IsDistribution ? f.Editor.DistLotteryRewards : f.Editor.TeraLotteryRewards;

                            List<Reward> list;
                            TeraShiny shiny = TeraShiny.No;

                            if (accuratesearch)
                            {
                                var det = TeraUtil.CalcRNG(seed, sav.TrainerTID7, sav.TrainerSID7, content, encounter, groupid);
                                list = RewardUtil.GetRewardList(det, encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist);
                                shiny = det.Shiny;
                            }
                            else
                            {
                                list = RewardUtil.GetRewardList(seed, encounter.Species, encounter.Stars, encounter.FixedRewardHash, encounter.LotteryRewardHash, fixedlist, lotterylist);
                            }

                            var rngres = new RewardDetails { Seed = seed, Rewards = list, Species = encounter.Species, Stars = encounter.Stars, Shiny = shiny, Calcs = 0 };
                            var success = true;

                            if (rngres != null)
                            {
                                var strlist = new List<string> { $"{seed:X8}" };
                                foreach (var item in rngres.Rewards)
                                    strlist.Add(item.GetItemName(f.Items, f.Editor.Language, true));

                                var entry = strlist.ToArray();
                                var grid = new RewardGridEntry(row.ConvertToString()).GetStrings();
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
        }
        return (RaidContent)f.cmbContent.SelectedIndex;
    }

    private static GameVersion GetGameVersion(uint seed, int groupid, DataGridViewRow row, CalculatorForm f)
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
                    content is RaidContent.Event_Mighty ? TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Mighty!) :
                        TeraUtil.GetDistEncounter(seed, sav, progress, f.Editor.Dist!);

                    if (encounter is not null)
                    {
                        var rngres = TeraUtil.CalcRNG(seed, Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10), content, encounter, groupid);
                        var success = true;

                        if (rngres != null)
                        {
                            var entry = rngres.GetStrings(f.NameList, f.AbilityList, f.NatureList, f.MoveList, f.TypeList, f.FormList, f.GenderListAscii, f.GenderListUnicode, f.ShinyList);
                            var grid = new GridEntry(row.ConvertToString()).GetStrings();
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
        var cellcount = row.Cells.Count;
        var output = new string[cellcount];
        for (var i = 0; i < cellcount; i++)
            output[i] = Convert.ToString(row.Cells[i].Value)!;
        return output;
    }
}
