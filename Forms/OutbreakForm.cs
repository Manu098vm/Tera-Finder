using PKHeX.Core;

namespace TeraFinder.Forms
{
    public partial class OutbreakForm : Form
    {
        public SAV9SV SAV = null!;
        public List<MassOutbreak> MassOutbreaks = new();
        public string Language = null!;

        private ConnectionForm? Connection = null;
        private Image DefBackground = null!;
        private Size DefSize = new(0, 0);
        private bool Loaded = false;
        private string[] SpeciesList = null!;
        private string[] FormsList = null!;
        private string[] TypesList = null!;
        private string[] GenderList = null!;

        public OutbreakForm(SAV9SV sav, string language)
        {
            InitializeComponent();
            SAV = sav;
            Language = language;

            this.TranslateInterface(Language);

            for (var i = 1; i <= 8; i++)
                MassOutbreaks.Add(new MassOutbreak(SAV, i));

            DefBackground = pictureBox.BackgroundImage!;
            DefSize = pictureBox.Size;
            SpeciesList = GameInfo.GetStrings(Language).specieslist;
            FormsList = GameInfo.GetStrings(Language).forms;
            TypesList = GameInfo.GetStrings(Language).types;
            GenderList = GameInfo.GenderSymbolUnicode.ToArray();

            for (var i = 0; i < 8; i++)
                cmbOutbreaks.Items[i] += SpeciesList[SpeciesConverter.GetNational9((ushort)MassOutbreaks[i].Species)];

            cmbSpecies.Items.AddRange(SpeciesList);
            cmbOutbreaks.SelectedIndex = 0;
        }

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
                cmbOutbreaks.Items[index] = $"Mass Outbreak {index + 1} - {SpeciesList[species]}";
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
            }
        }

        private void numMaxSpawn_ValueChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                outbreak.MaxSpawns = (int)numMaxSpawn.Value;
            }
        }

        private void numKO_ValueChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                outbreak.NumKO = (int)numKO.Value;
            }
        }

        private void chkFound_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (chkFound.Checked)
                    outbreak.Found = true;
                else
                    outbreak.Found = false;
            }
        }

        private void chkEnabled_CheckChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
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
            }
        }

        private void txtCenterX_TextChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (outbreak.LocationCenter is not null)
                {
                    try
                    {
                        outbreak.LocationCenter.X = Convert.ToSingle(txtCenterX.Text);
                        imgMap.SetMapPoint(outbreak.LocationCenter);
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
                    try
                    {
                        outbreak.LocationCenter.Y = Convert.ToSingle(txtCenterX.Text);
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
                    try
                    {
                        outbreak.LocationCenter.Z = Convert.ToSingle(txtCenterX.Text);
                        imgMap.SetMapPoint(outbreak.LocationCenter);
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
                    try
                    {
                        outbreak.LocationDummy.X = Convert.ToSingle(txtDummyX.Text);
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
                    try
                    {
                        outbreak.LocationDummy.Y = Convert.ToSingle(txtDummyY.Text);
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
                    try
                    {
                        outbreak.LocationDummy.Z = Convert.ToSingle(txtDummyZ.Text);
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
                    outbreak.DumpToJson(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while parsing:\n{ex.Message}");
            }
        }

        private void injectFromJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var outbreak = MassOutbreaks[cmbOutbreaks.SelectedIndex];
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outbreak.RestoreFromJson(openFileDialog.FileName);
                    cmbOutbreaks_IndexChanged(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while parsing:\n{ex.Message}");
            }
        }
    }
}