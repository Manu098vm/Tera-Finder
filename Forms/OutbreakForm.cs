using PKHeX.Core;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace TeraFinder.Forms
{
    public partial class OutbreakForm : Form
    {
        public SAV9SV SAV = null!;
        public List<MassOutbreak> MassOutbreaks = new();
        public string Language = null!;
        private Dictionary<string, string> Strings = null!;

        private ConnectionForm? Connection;

        private Image DefBackground = null!;
        private Size DefSize = new(0, 0);
        private bool Loaded = false;
        private string[] SpeciesList = null!;
        private string[] FormsList = null!;
        private string[] TypesList = null!;
        private string[] GenderList = null!;

        public OutbreakForm(SAV9SV sav, string language, ConnectionForm? connection)
        {
            InitializeComponent();
            SAV = sav;
            Language = language;

            this.TranslateInterface(Language);
            GenerateDictionary();
            TranslateDictionary(Language);

            for (var i = 1; i <= 8; i++)
                MassOutbreaks.Add(new MassOutbreak(SAV, i));

            DefBackground = pictureBox.BackgroundImage!;
            DefSize = pictureBox.Size;
            SpeciesList = GameInfo.GetStrings(Language).specieslist;
            FormsList = GameInfo.GetStrings(Language).forms;
            TypesList = GameInfo.GetStrings(Language).types;
            GenderList = GameInfo.GenderSymbolUnicode.ToArray();

            for (var i = 0; i < 8; i++)
                cmbOutbreaks.Items[i] = $"{Strings["OutBreakForm.MassOutbreakName"]} {i+1} - " +
                    $"{SpeciesList[SpeciesConverter.GetNational9((ushort)MassOutbreaks[i].Species)]}";

            cmbSpecies.Items.AddRange(SpeciesList);
            cmbOutbreaks.SelectedIndex = 0;

            Connection = connection;
        }

        private void GenerateDictionary()
        {
            Strings = new Dictionary<string, string>
            {
                { "OutBreakForm.MassOutbreakName", "Mass Outbreak" },
                { "OutbreakForm.DeviceDisconnected", "Device disconnected." },
                { "OutbreakForm.ErrorParsing", "Error while parsing:" },
                { "OutbreakForm.LoadDefault", "Do you want to load default legal data for {species}?" },
            };
        }

        private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

        private void cmbOutbreaks_IndexChanged(object sender, EventArgs e)
        {
            Loaded = false;

            if (cmbOutbreaks.SelectedIndex == 0)
                btnPrev.Enabled = false;
            else
                btnPrev.Enabled = true;

            if (cmbOutbreaks.SelectedIndex == cmbOutbreaks.Items.Count - 1)
                btnNext.Enabled = false;
            else
                btnNext.Enabled = true;

            var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];

            var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
            if (species != cmbSpecies.SelectedIndex)
                cmbSpecies.SelectedIndex = species;
            else
                cmbSpecies_IndexChanged(this, EventArgs.Empty);

            numMaxSpawn.Value = outbreak.MaxSpawns > 0 ? outbreak.MaxSpawns : 1;
            numKO.Value = outbreak.NumKO;

            chkEnabled.Checked = outbreak.Enabled;
            chkFound.Checked = outbreak.Found;

            txtCenterX.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.X : 0)}";
            txtCenterY.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.Y : 0)}";
            txtCenterZ.Text = $"{(outbreak.LocationCenter is not null ? outbreak.LocationCenter.Z : 0)}";

            txtDummyX.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.X : 0)}";
            txtDummyY.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.Y : 0)}";
            txtDummyZ.Text = $"{(outbreak.LocationDummy is not null ? outbreak.LocationDummy.Z : 0)}";

            if (outbreak.LocationCenter is not null)
                imgMap.SetMapPoint(outbreak.LocationCenter);
            else imgMap.ResetMap();

            Loaded = true;
        }

        private void cmbSpecies_IndexChanged(object sender, EventArgs e)
        {

            cmbForm.Items.Clear();
            var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
            var toExpect = outbreak.Species;
            var species = (ushort)cmbSpecies.SelectedIndex;
            var formlist = FormConverter.GetFormList(species, TypesList, FormsList, GenderList, EntityContext.Gen9);
            if (formlist.Length == 0 || (formlist.Length == 1 && formlist[0].Equals("")))
                cmbForm.Items.Add("---");
            else
                cmbForm.Items.AddRange(formlist);

            if (Loaded)
            {
                outbreak.Species = SpeciesConverter.GetInternal9(species);
                cmbForm.SelectedIndex = 0;
                var index = cmbOutbreaks.SelectedIndex;
                cmbOutbreaks.Items[index] = $"{Strings["OutBreakForm.MassOutbreakName"]} {index + 1} - {SpeciesList[species]}";

                if (Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}Species")!.GetValue(new DataBlock())!;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Species, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
            else
            {
                cmbForm.SelectedIndex = outbreak.Form;
            }


            pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
            if (pictureBox.Image is not null)
            {
                pictureBox.BackgroundImage = null;
                pictureBox.Size = pictureBox.Image.Size;
            }
            else
            {
                pictureBox.BackgroundImage = DefBackground;
                pictureBox.Size = DefSize;
            }
        }

        private void cmbForm_IndexChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                var toExpect = outbreak.Form;
                var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
                outbreak.Form = (byte)cmbForm.SelectedIndex;

                pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
                if (pictureBox.Image is not null)
                {
                    pictureBox.BackgroundImage = null;
                    pictureBox.Size = pictureBox.Image.Size;
                }
                else
                {
                    pictureBox.BackgroundImage = DefBackground;
                    pictureBox.Size = DefSize;
                }

                if (Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}Form")!.GetValue(new DataBlock())!;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Form, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
        }

        private void numMaxSpawn_ValueChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                var toExpect = outbreak.MaxSpawns;
                outbreak.MaxSpawns = (int)numMaxSpawn.Value;

                if (Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}TotalSpawns")!.GetValue(new DataBlock())!;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.MaxSpawns, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
        }

        private void numKO_ValueChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                var toExpect = outbreak.NumKO;
                outbreak.NumKO = (int)numKO.Value;

                if (Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}NumKOed")!.GetValue(new DataBlock())!;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.NumKO, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
        }

        private void chkFound_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                var toExpect = outbreak.Found;

                if (chkFound.Checked)
                    outbreak.Found = true;
                else
                    outbreak.Found = false;

                if(Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}Found")!.GetValue(new DataBlock())!;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(outbreak.Found, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
        }

        private void chkEnabled_CheckChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                var toExpect = (byte)outbreak.GetAmountAvailable();

                if (chkEnabled.Checked)
                    outbreak.Enabled = true;
                else
                    outbreak.Enabled = false;

                var species = SpeciesConverter.GetNational9((ushort)outbreak.Species);
                pictureBox.Image = ImagesUtil.GetSimpleSprite(species, outbreak.Form, outbreak.Enabled);
                if (pictureBox.Image is not null)
                {
                    pictureBox.BackgroundImage = null;
                    pictureBox.Size = pictureBox.Image.Size;
                }
                else
                {
                    pictureBox.BackgroundImage = DefBackground;
                    pictureBox.Size = DefSize;
                }

                if (Connection is not null && Connection.IsConnected())
                {
                    var success = false;
                    var value = (byte)outbreak.GetAmountAvailable();
                    var blockInfo = Blocks.KMassOutbreakAmount;
                    Task.Run(async () => { success = await Connection.Executor.WriteBlock(value, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                    if (!success)
                    {
                        Connection.Disconnect();
                        MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                    }
                }
            }
        }

        private void txtCenterX_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationCenter is not null)
                {
                    var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationCenter.X = Convert.ToSingle(txtCenterX.Text);
                        imgMap.SetMapPoint(outbreak.LocationCenter);

                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex + 1}CenterPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        imgMap.ResetMap();
                    }
                }
            }
        }

        private void txtCenterY_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationCenter is not null)
                {
                    var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationCenter.Y = Convert.ToSingle(txtCenterY.Text);
                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex + 1}CenterPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void txtCenterZ_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationCenter is not null)
                {
                    var toExpect = outbreak.LocationCenter.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationCenter.Z = Convert.ToSingle(txtCenterZ.Text);
                        imgMap.SetMapPoint(outbreak.LocationCenter);

                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationCenter.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex + 1}CenterPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        imgMap.ResetMap();
                    }
                }
            }
        }

        private void txtDummyX_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationDummy is not null)
                {
                    var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationDummy.X = Convert.ToSingle(txtDummyX.Text);
                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex+1}DummyPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void txtDummyY_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationDummy is not null)
                {
                    var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationDummy.Y = Convert.ToSingle(txtDummyY.Text);
                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex + 1}DummyPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void txtDummyZ_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationDummy is not null)
                {
                    var toExpect = outbreak.LocationDummy.GetCoordinates().ToArray();
                    try
                    {
                        outbreak.LocationDummy.Z = Convert.ToSingle(txtDummyZ.Text);
                        if (Connection is not null && Connection.IsConnected())
                        {
                            var success = false;
                            var toInject = outbreak.LocationDummy.GetCoordinates().ToArray();
                            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{cmbOutbreaks.SelectedIndex + 1}DummyPos")!.GetValue(new DataBlock())!;
                            Task.Run(async () => { success = await Connection.Executor.WriteBlock(toInject, blockInfo, new CancellationToken(), toExpect).ConfigureAwait(false); }).Wait();

                            if (!success)
                            {
                                Connection.Disconnect();
                                MessageBox.Show(Strings["OutbreakForm.DeviceDisconnected"]);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            cmbOutbreaks.SelectedIndex++;
            cmbOutbreaks.Focus();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
            cmbOutbreaks.SelectedIndex--;
            cmbOutbreaks.Focus();
        }

        private void dumpToJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                saveFileDialog.FileName = $"{SpeciesConverter.GetNational9((ushort)outbreak.Species)}";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (outbreak.LocationCenter is not null && outbreak.LocationDummy is not null)
                    {
                        var json = "{\n" +
                            "\t\"LocationCenter\": \"" + BitConverter.ToString(outbreak.LocationCenter.GetCoordinates().ToArray()).Replace("-", string.Empty) + "\",\n" +
                            "\t\"LocationDummy\": \"" + BitConverter.ToString(outbreak.LocationDummy.GetCoordinates().ToArray()).Replace("-", string.Empty) + "\",\n" +
                            "\t\"Species\": " + SpeciesConverter.GetNational9((ushort)outbreak.Species) + ",\n" +
                            "\t\"Form\": " + outbreak.Form + ",\n" +
                            "\t\"MaxSpawns\": " + outbreak.MaxSpawns + "\n" +
                        "}";

                        File.WriteAllText(saveFileDialog.FileName, json);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings["OutbreakForm.ErrorParsing"]}\n{ex.Message}");
            }
        }

        private void injectFromJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var simpleOutbreak = JsonSerializer.Deserialize<JsonNode>(File.ReadAllText(openFileDialog.FileName))!;
                    var locationCenter = Convert.FromHexString(simpleOutbreak["LocationCenter"]!.GetValue<string>());
                    var locationDummy = Convert.FromHexString(simpleOutbreak["LocationDummy"]!.GetValue<string>());
                    var species = SpeciesConverter.GetInternal9(simpleOutbreak["Species"]!.GetValue<ushort>());
                    var form = simpleOutbreak["Form"]!.GetValue<byte>();
                    var maxSpawns = simpleOutbreak["MaxSpawns"]!.GetValue<int>();

                    var block = SAV.Accessor.GetBlockSafe(Blocks.KMassOutbreak01CenterPos.Key);
                    GameCoordinates coordC, coordD;
                    if (block is not null)
                    {
                        coordC = new GameCoordinates(block);
                        coordD = new GameCoordinates(block);
                        coordC.SetCoordinates(locationCenter);
                        coordD.SetCoordinates(locationDummy);

                        cmbSpecies.SelectedIndex = species;
                        cmbForm.SelectedIndex = form;
                        numMaxSpawn.Value = maxSpawns;
                        numKO.Value = numKO.Value >= maxSpawns ? 0 : numKO.Value;
                        txtCenterX.Text = $"{coordC.X}";
                        txtCenterY.Text = $"{coordC.Y}";
                        txtCenterZ.Text = $"{coordC.Z}";
                        txtDummyX.Text = $"{coordD.X}";
                        txtDummyY.Text = $"{coordD.Y}";
                        txtDummyZ.Text = $"{coordD.Z}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings["OutbreakForm.ErrorParsing"]}\n{ex.Message}");
            }
        }
    }
}