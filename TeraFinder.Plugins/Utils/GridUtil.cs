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
                    if (EncounterRaidTF9.TryGenerateTeraDetails(seed, encounters, version, progress, eventProgress, content, map, id32, groupid, out var enc, out var result))
                    {
                        if (!enc.GeneratePK9(result.Value, id32, version, f.Editor.SAV.OT, f.Editor.SAV.Language, f.Editor.SAV.Gender, out var pk9, out var la))
                        {
                            var la_encounter = la.Results.Where(l => l.Identifier is CheckIdentifier.Encounter).FirstOrDefault();
                            if (content is RaidContent.Event or RaidContent.Event_Mighty)
                                MessageBox.Show($"{strings["GridUtil.ErrorParsing"]}\n{strings["GridUtil.MissingData"]} [{enc.Identifier}].\n{strings["GridUtil.CheckWiki"]}");
                            else
                                MessageBox.Show($"{strings["GridUtil.ErrorParsing"]} {strings["GridUtil.Report"]}\n{la.Report()}");
                            return;
                        }

                        f.Editor.PKMEditor!.PopulateFields(pk9!, true);
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
