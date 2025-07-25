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
            { "GridUtil.NoCatch", "" },
        }.TranslateInnerStrings(language);
    }

    public static void SaveAllTxt(this DataGridView dataGrid, string language, bool saveAsCsv = false)
    {
        var strings = GenerateDictionary(language);

        if (dataGrid.Rows.Count == 0)
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = saveAsCsv ? "CSV (*.csv)|*.csv" : "TXT (*.txt)|*.txt",
            FileName = saveAsCsv ? "terafinder.csv" : "terafinder.txt"
        };
        if (sfd.ShowDialog() is not DialogResult.OK)
            return;

        if (File.Exists(sfd.FileName))
        {
            try
            {
                File.Delete(sfd.FileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.Message}");
                return;
            }
        }

        try
        {
            WriteDataGridToFile(dataGrid, sfd.FileName, saveAsCsv ? "," : "\t");
            MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}");
        }
    }

    private static void WriteDataGridToFile(DataGridView dataGrid, string fileName, string delimiter)
    {
        using var writer = new StreamWriter(fileName, false, Encoding.UTF8);
        var columnNames = string.Join(delimiter, dataGrid.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText));
        writer.WriteLine(columnNames);

        foreach (DataGridViewRow row in dataGrid.Rows)
        {
            var rowValues = row.Cells.Cast<DataGridViewCell>().Select(cell => Convert.ToString(cell.Value));
            writer.WriteLine(string.Join(delimiter, rowValues));
        }
    }

    public static void SaveSelectedTxt(this DataGridView dataGrid, string language, bool saveAsCsv = false)
    {
        var strings = GenerateDictionary(language);

        if (dataGrid.SelectedCells.Count == 0)
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
            return;
        }

        var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct().ToArray();
        if (selectedRows.Length == 0)
        {
            MessageBox.Show(strings["GridUtil.NoData"]);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = saveAsCsv ? "CSV (*.csv)|*.csv" : "TXT (*.txt)|*.txt",
            FileName = saveAsCsv ? "terafinder.csv" : "terafinder.txt"
        };
        if (sfd.ShowDialog() is not DialogResult.OK)
            return;

        if (File.Exists(sfd.FileName))
        {
            try
            {
                File.Delete(sfd.FileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"{ex.Message}");
                return;
            }
        }

        try
        {
            WriteSelectedRowsToFile(selectedRows!, dataGrid, sfd.FileName, saveAsCsv ? "," : "\t");
            MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}");
        }
    }

    private static void WriteSelectedRowsToFile(DataGridViewRow[] selectedRows, DataGridView dataGrid, string fileName, string delimiter)
    {
        using var writer = new StreamWriter(fileName, false, Encoding.UTF8);
        var columnNames = string.Join(delimiter, dataGrid.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText));
        writer.WriteLine(columnNames);

        foreach (var row in selectedRows)
        {
            var rowValues = row.Cells.Cast<DataGridViewCell>().Select(cell => Convert.ToString(cell.Value));
            writer.WriteLine(string.Join(delimiter, rowValues));
        }
    }

    public static void SaveSelectedPk9(this DataGridView dataGrid, CalculatorForm f, string language, TeraRaidMapParent map)
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
                    var groupid = Convert.ToByte(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 1].Value.ToString()!, 10);
                    var content = (RaidContent)f.cmbContent.SelectedIndex;
                    var progress = (GameProgress)f.cmbProgress.SelectedIndex;
                    var eventProgress = EventUtil.GetEventStageFromProgress(progress);
                    var version = f.cmbGame.SelectedIndex == 0 ? GameVersion.SL : GameVersion.VL;
                    var id32 = TidUtil.GetID32(Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10));

                    var encounters = f.Editor.GetCurrentEncounters(content, map);
                    if (EncounterRaidTF9.TryGenerateTeraDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, out var enc, out var result))
                    {
                        if (enc.CanBeCaught)
                        {
                            var checkActiveHandler = ParseSettings.Settings.Handler.CheckActiveHandler;
                            ParseSettings.Settings.Handler.CheckActiveHandler = false;
                            if (!enc.GeneratePK9(result.Value, id32, version, f.Editor.SAV.OT, f.Editor.SAV.Language, f.Editor.SAV.Gender, out var pk9, out var la))
                            {
                                var la_encounter = la.Results.Where(l => l.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
                                if (content is RaidContent.Event or RaidContent.Event_Mighty)
                                    MessageBox.Show($"{strings["GridUtil.ErrorParsing"]}\n{strings["GridUtil.MissingData"]} [{enc.Identifier}].\n{strings["GridUtil.CheckWiki"]}");
                                else
                                    MessageBox.Show($"{strings["GridUtil.ErrorParsing"]} {strings["GridUtil.Report"]}\n{la.Report()}");
                                ParseSettings.Settings.Handler.CheckActiveHandler = checkActiveHandler;
                                return;
                            }

                            var sfd = new SaveFileDialog
                            {
                                Filter = "PK9 (*.pk9)|*.pk9",
                                FileName = $"{pk9!.FileName}",
                            };

                            if (sfd.ShowDialog() is DialogResult.OK)
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
                                File.WriteAllBytes(sfd.FileName, pk9.Data);
                                MessageBox.Show($"{strings["GridUtil.Exported"]} {sfd.FileName}");
                            }

                            ParseSettings.Settings.Handler.CheckActiveHandler = checkActiveHandler;
                        }
                        else
                        {
                            MessageBox.Show($"{strings["GridUtil.NoCatch"]}");
                        }
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

    public static void ViewRewards(this DataGridView dataGrid, CalculatorForm f, string language, TeraRaidMapParent map)
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
                    var groupid = Convert.ToByte(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 1].Value.ToString()!, 10);
                    var content = (RaidContent)f.cmbContent.SelectedIndex;
                    var progress = (GameProgress)f.cmbProgress.SelectedIndex;
                    var eventProgress = EventUtil.GetEventStageFromProgress(progress);
                    var id32 = TidUtil.GetID32(Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10));
                    var version = f.cmbGame.SelectedIndex == 0 ? GameVersion.SL : GameVersion.VL;

                    var encounters = f.Editor.GetCurrentEncounters(content, map);
                    
                    EncounterRaidTF9.TryGenerateRewardDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, 0, out var rng, out var lvl0);
                    EncounterRaidTF9.TryGenerateRewardDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, 1, out _, out var lvl1);
                    EncounterRaidTF9.TryGenerateRewardDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, 2, out _, out var lvl2);
                    EncounterRaidTF9.TryGenerateRewardDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, 3, out _, out var lvl3);

                    new RewardListForm(f.Editor.Language, (MoveType)rng.Value.TeraType, lvl0.Value.Rewards, lvl1.Value.Rewards, lvl2.Value.Rewards, lvl3.Value.Rewards).ShowDialog();
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
                    var groupid = Convert.ToByte(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 1].Value.ToString()!, 10);
                    var content = (RaidContent)f.cmbContent.SelectedIndex;

                    if (content is RaidContent.Event or RaidContent.Event_Mighty)
                    {
                        if (f.Editor.CurrTera is not null)
                        {
                            var tera = f.Editor.CurrTera;
                            if (tera.Value.EventIndex != groupid)
                            {
                                MessageBox.Show($"{strings["GridUtil.MismatchGroupID"].Replace("{editorIndex}", $"{tera.Value.EventIndex}").Replace("{resultIndex}", $"{groupid}")}");
                                return;
                            }
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
                    var groupid = Convert.ToInt32(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 1].Value.ToString()!, 10);
                    var content = (RaidContent)f.cmbContent.SelectedIndex;

                    if (content is RaidContent.Event or RaidContent.Event_Mighty)
                    {
                        if (f.Editor.CurrTera is not null)
                        {
                            var tera = f.Editor.CurrTera;
                            if (tera.Value.EventIndex != groupid)
                            {
                                MessageBox.Show($"{strings["GridUtil.MismatchGroupID"].Replace("{editorIndex}", $"{tera.Value.EventIndex}").Replace("{resultIndex}", $"{groupid}")}");
                                return;
                            }
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

    public static void SendSelectedPk9Editor(this DataGridView dataGrid, CalculatorForm f, string language, TeraRaidMapParent map)
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
                    var groupid = Convert.ToByte(selectedRows.ElementAt(0).Cells[selectedRows.ElementAt(0).Cells.Count - 1].Value.ToString()!, 10);
                    var content = (RaidContent)f.cmbContent.SelectedIndex;
                    var progress = (GameProgress)f.cmbProgress.SelectedIndex;
                    var eventProgress = EventUtil.GetEventStageFromProgress(progress);
                    var version = f.cmbGame.SelectedIndex == 0 ? GameVersion.SL : GameVersion.VL;
                    var id32 = TidUtil.GetID32(Convert.ToUInt32(f.txtTID.Text, 10), Convert.ToUInt32(f.txtSID.Text, 10));

                    var encounters = f.Editor.GetCurrentEncounters(content, map);
                    var checkActiveHandler = ParseSettings.Settings.Handler.CheckActiveHandler;
                    ParseSettings.Settings.Handler.CheckActiveHandler = false;

                    if (EncounterRaidTF9.TryGenerateTeraDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, out var enc, out var result))
                    {
                        if (enc.CanBeCaught)
                        {
                            if (!enc.GeneratePK9(result.Value, id32, version, f.Editor.SAV.OT, f.Editor.SAV.Language, f.Editor.SAV.Gender, out var pk9, out var la))
                            {
                                var la_encounter = la.Results.Where(l => l.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
                                if (content is RaidContent.Event or RaidContent.Event_Mighty)
                                    MessageBox.Show($"{strings["GridUtil.ErrorParsing"]}\n{strings["GridUtil.MissingData"]} [{enc.Identifier}].\n{strings["GridUtil.CheckWiki"]}");
                                else
                                    MessageBox.Show($"{strings["GridUtil.ErrorParsing"]} {strings["GridUtil.Report"]}\n{la.Report()}");

                                ParseSettings.Settings.Handler.CheckActiveHandler = checkActiveHandler;
                                return;
                            }

                            ParseSettings.Settings.Handler.CheckActiveHandler = checkActiveHandler;
                            f.Editor.PKMEditor!.PopulateFields(pk9!, true);
                        }
                        else
                        {
                            MessageBox.Show($"{strings["GridUtil.NoCatch"]}");
                        }
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

    public static void CopySeed(this DataGridView dataGrid, string language)
    {
        var strings = GenerateDictionary(language);
        var selectedRows = dataGrid.SelectedCells.Cast<DataGridViewCell>().Select(cell => cell.OwningRow).Distinct();
        var count = selectedRows.Count();

        if (count == 1)
            try { Clipboard.SetText(selectedRows.ElementAt(0).Cells[0].Value.ToString()!); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        else
            MessageBox.Show(strings["GridUtil.RowsExceeded"]);
    }
}
