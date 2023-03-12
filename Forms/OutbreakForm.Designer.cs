namespace TeraFinder.Forms
{
    partial class OutbreakForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grpMap = new GroupBox();
            imgMap = new PictureBox();
            cmbOutbreaks = new ComboBox();
            btnNext = new Button();
            btnPrev = new Button();
            pictureBox = new PictureBox();
            grpPkmInfo = new GroupBox();
            cmbForm = new ComboBox();
            cmbSpecies = new ComboBox();
            lblForm = new Label();
            lblSpecies = new Label();
            grpLocation = new GroupBox();
            grpLocationDummy = new GroupBox();
            lblDummyZ = new Label();
            txtDummyZ = new TextBox();
            lblDummyY = new Label();
            txtDummyY = new TextBox();
            txtDummyX = new TextBox();
            lblDummyX = new Label();
            grpLocationCenter = new GroupBox();
            lblCenterZ = new Label();
            lblCenterY = new Label();
            lblCenterX = new Label();
            txtCenterZ = new TextBox();
            txtCenterY = new TextBox();
            txtCenterX = new TextBox();
            grpMassInfo = new GroupBox();
            chkFound = new CheckBox();
            numKO = new NumericUpDown();
            numMaxSpawn = new NumericUpDown();
            lblKoSpawn = new Label();
            lblTotalSpawn = new Label();
            grpMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgMap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            grpPkmInfo.SuspendLayout();
            grpLocation.SuspendLayout();
            grpLocationDummy.SuspendLayout();
            grpLocationCenter.SuspendLayout();
            grpMassInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numKO).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSpawn).BeginInit();
            SuspendLayout();
            // 
            // grpMap
            // 
            grpMap.Controls.Add(imgMap);
            grpMap.Location = new Point(368, 4);
            grpMap.Margin = new Padding(3, 4, 3, 4);
            grpMap.Name = "grpMap";
            grpMap.Padding = new Padding(3, 4, 3, 4);
            grpMap.Size = new Size(584, 594);
            grpMap.TabIndex = 24;
            grpMap.TabStop = false;
            // 
            // imgMap
            // 
            imgMap.BackgroundImage = Properties.Resources.world;
            imgMap.BackgroundImageLayout = ImageLayout.Stretch;
            imgMap.BorderStyle = BorderStyle.Fixed3D;
            imgMap.Location = new Point(6, 16);
            imgMap.Margin = new Padding(3, 4, 3, 4);
            imgMap.Name = "imgMap";
            imgMap.Size = new Size(572, 570);
            imgMap.TabIndex = 22;
            imgMap.TabStop = false;
            // 
            // cmbOutbreaks
            // 
            cmbOutbreaks.FormattingEnabled = true;
            cmbOutbreaks.Items.AddRange(new object[] { "Mass Outbreak 1 - ", "Mass Outbreak 2 - ", "Mass Outbreak 3 - ", "Mass Outbreak 4 - ", "Mass Outbreak 5 - ", "Mass Outbreak 6 - ", "Mass Outbreak 7 - ", "Mass Outbreak 8 - " });
            cmbOutbreaks.Location = new Point(47, 12);
            cmbOutbreaks.Name = "cmbOutbreaks";
            cmbOutbreaks.Size = new Size(279, 28);
            cmbOutbreaks.TabIndex = 25;
            cmbOutbreaks.SelectedIndexChanged += cmbOutbreaks_IndexChanged;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(332, 12);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(32, 28);
            btnNext.TabIndex = 26;
            btnNext.Text = "ᐅ";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(9, 12);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(32, 28);
            btnPrev.TabIndex = 27;
            btnPrev.Text = "ᐊ";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = Color.Transparent;
            pictureBox.BackgroundImage = Properties.Resources._000;
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(265, 34);
            pictureBox.Margin = new Padding(3, 4, 3, 4);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(75, 75);
            pictureBox.TabIndex = 28;
            pictureBox.TabStop = false;
            // 
            // grpPkmInfo
            // 
            grpPkmInfo.Controls.Add(cmbForm);
            grpPkmInfo.Controls.Add(cmbSpecies);
            grpPkmInfo.Controls.Add(lblForm);
            grpPkmInfo.Controls.Add(pictureBox);
            grpPkmInfo.Controls.Add(lblSpecies);
            grpPkmInfo.Location = new Point(9, 46);
            grpPkmInfo.Name = "grpPkmInfo";
            grpPkmInfo.Size = new Size(355, 123);
            grpPkmInfo.TabIndex = 29;
            grpPkmInfo.TabStop = false;
            grpPkmInfo.Text = "Pokémon Info";
            // 
            // cmbForm
            // 
            cmbForm.FormattingEnabled = true;
            cmbForm.Location = new Point(85, 68);
            cmbForm.Name = "cmbForm";
            cmbForm.Size = new Size(151, 28);
            cmbForm.TabIndex = 32;
            cmbForm.SelectedIndexChanged += cmbForm_IndexChanged;
            // 
            // cmbSpecies
            // 
            cmbSpecies.FormattingEnabled = true;
            cmbSpecies.Location = new Point(85, 34);
            cmbSpecies.Name = "cmbSpecies";
            cmbSpecies.Size = new Size(151, 28);
            cmbSpecies.TabIndex = 31;
            cmbSpecies.SelectedIndexChanged += cmbSpecies_IndexChanged;
            // 
            // lblForm
            // 
            lblForm.AutoSize = true;
            lblForm.Location = new Point(6, 71);
            lblForm.Name = "lblForm";
            lblForm.Size = new Size(46, 20);
            lblForm.TabIndex = 30;
            lblForm.Text = "Form:";
            // 
            // lblSpecies
            // 
            lblSpecies.AutoSize = true;
            lblSpecies.Location = new Point(6, 37);
            lblSpecies.Name = "lblSpecies";
            lblSpecies.Size = new Size(62, 20);
            lblSpecies.TabIndex = 29;
            lblSpecies.Text = "Species:";
            // 
            // grpLocation
            // 
            grpLocation.Controls.Add(grpLocationDummy);
            grpLocation.Controls.Add(grpLocationCenter);
            grpLocation.Location = new Point(9, 303);
            grpLocation.Name = "grpLocation";
            grpLocation.Size = new Size(355, 295);
            grpLocation.TabIndex = 33;
            grpLocation.TabStop = false;
            grpLocation.Text = "Location Info";
            // 
            // grpLocationDummy
            // 
            grpLocationDummy.Controls.Add(lblDummyZ);
            grpLocationDummy.Controls.Add(txtDummyZ);
            grpLocationDummy.Controls.Add(lblDummyY);
            grpLocationDummy.Controls.Add(txtDummyY);
            grpLocationDummy.Controls.Add(txtDummyX);
            grpLocationDummy.Controls.Add(lblDummyX);
            grpLocationDummy.Location = new Point(6, 161);
            grpLocationDummy.Name = "grpLocationDummy";
            grpLocationDummy.Size = new Size(343, 129);
            grpLocationDummy.TabIndex = 34;
            grpLocationDummy.TabStop = false;
            grpLocationDummy.Text = "Map Coordinates:";
            // 
            // lblDummyZ
            // 
            lblDummyZ.AutoSize = true;
            lblDummyZ.Location = new Point(25, 99);
            lblDummyZ.Name = "lblDummyZ";
            lblDummyZ.Size = new Size(21, 20);
            lblDummyZ.TabIndex = 2;
            lblDummyZ.Text = "Z:";
            // 
            // txtDummyZ
            // 
            txtDummyZ.Location = new Point(79, 96);
            txtDummyZ.Name = "txtDummyZ";
            txtDummyZ.Size = new Size(179, 27);
            txtDummyZ.TabIndex = 2;
            txtDummyZ.TextChanged += new EventHandler(txtDummyZ_TextChanged);
            // 
            // lblDummyY
            // 
            lblDummyY.AutoSize = true;
            lblDummyY.Location = new Point(25, 66);
            lblDummyY.Name = "lblDummyY";
            lblDummyY.Size = new Size(20, 20);
            lblDummyY.TabIndex = 1;
            lblDummyY.Text = "Y:";
            // 
            // txtDummyY
            // 
            txtDummyY.Location = new Point(79, 63);
            txtDummyY.Name = "txtDummyY";
            txtDummyY.Size = new Size(179, 27);
            txtDummyY.TabIndex = 1;
            txtDummyY.TextChanged += new EventHandler(txtDummyY_TextChanged);

            // 
            // txtDummyX
            // 
            txtDummyX.Location = new Point(79, 30);
            txtDummyX.Name = "txtDummyX";
            txtDummyX.Size = new Size(179, 27);
            txtDummyX.TabIndex = 0;
            txtDummyX.TextChanged += new EventHandler(txtDummyX_TextChanged);
            // 
            // lblDummyX
            // 
            lblDummyX.AutoSize = true;
            lblDummyX.Location = new Point(25, 33);
            lblDummyX.Name = "lblDummyX";
            lblDummyX.Size = new Size(21, 20);
            lblDummyX.TabIndex = 0;
            lblDummyX.Text = "X:";
            // 
            // grpLocationCenter
            // 
            grpLocationCenter.Controls.Add(lblCenterZ);
            grpLocationCenter.Controls.Add(lblCenterY);
            grpLocationCenter.Controls.Add(lblCenterX);
            grpLocationCenter.Controls.Add(txtCenterZ);
            grpLocationCenter.Controls.Add(txtCenterY);
            grpLocationCenter.Controls.Add(txtCenterX);
            grpLocationCenter.Location = new Point(6, 26);
            grpLocationCenter.Name = "grpLocationCenter";
            grpLocationCenter.Size = new Size(343, 129);
            grpLocationCenter.TabIndex = 34;
            grpLocationCenter.TabStop = false;
            grpLocationCenter.Text = "World Coordinates:";
            // 
            // lblCenterZ
            // 
            lblCenterZ.AutoSize = true;
            lblCenterZ.Location = new Point(25, 99);
            lblCenterZ.Name = "lblCenterZ";
            lblCenterZ.Size = new Size(21, 20);
            lblCenterZ.TabIndex = 5;
            lblCenterZ.Text = "Z:";
            // 
            // lblCenterY
            // 
            lblCenterY.AutoSize = true;
            lblCenterY.Location = new Point(25, 66);
            lblCenterY.Name = "lblCenterY";
            lblCenterY.Size = new Size(20, 20);
            lblCenterY.TabIndex = 4;
            lblCenterY.Text = "Y:";
            // 
            // lblCenterX
            // 
            lblCenterX.AutoSize = true;
            lblCenterX.Location = new Point(25, 33);
            lblCenterX.Name = "lblCenterX";
            lblCenterX.Size = new Size(21, 20);
            lblCenterX.TabIndex = 3;
            lblCenterX.Text = "X:";
            // 
            // txtCenterZ
            // 
            txtCenterZ.Location = new Point(79, 96);
            txtCenterZ.Name = "txtCenterZ";
            txtCenterZ.Size = new Size(179, 27);
            txtCenterZ.TabIndex = 2;
            txtCenterZ.TextChanged += new EventHandler(txtCenterZ_TextChanged);
            // 
            // txtCenterY
            // 
            txtCenterY.Location = new Point(79, 63);
            txtCenterY.Name = "txtCenterY";
            txtCenterY.Size = new Size(179, 27);
            txtCenterY.TabIndex = 1;
            txtCenterY.TextChanged += new EventHandler(txtCenterY_TextChanged);
            // 
            // txtCenterX
            // 
            txtCenterX.Location = new Point(79, 30);
            txtCenterX.Name = "txtCenterX";
            txtCenterX.Size = new Size(179, 27);
            txtCenterX.TabIndex = 0;
            txtCenterX.TextChanged += new EventHandler(txtCenterX_TextChanged);
            // 
            // grpMassInfo
            // 
            grpMassInfo.Controls.Add(chkFound);
            grpMassInfo.Controls.Add(numKO);
            grpMassInfo.Controls.Add(numMaxSpawn);
            grpMassInfo.Controls.Add(lblKoSpawn);
            grpMassInfo.Controls.Add(lblTotalSpawn);
            grpMassInfo.Location = new Point(9, 175);
            grpMassInfo.Name = "grpMassInfo";
            grpMassInfo.Size = new Size(355, 122);
            grpMassInfo.TabIndex = 34;
            grpMassInfo.TabStop = false;
            grpMassInfo.Text = "Mass Outbreak Info";
            // 
            // chkFound
            // 
            chkFound.AutoSize = true;
            chkFound.Location = new Point(265, 58);
            chkFound.Name = "chkFound";
            chkFound.Size = new Size(72, 24);
            chkFound.TabIndex = 4;
            chkFound.Text = "Found";
            chkFound.UseVisualStyleBackColor = true;
            chkFound.CheckedChanged += new EventHandler(chkFound_CheckedChanged);
            // 
            // numKO
            // 
            numKO.Location = new Point(127, 73);
            numKO.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numKO.Name = "numKO";
            numKO.Size = new Size(88, 27);
            numKO.TabIndex = 3;
            numKO.ValueChanged += new EventHandler(numKO_ValueChanged);
            // 
            // numMaxSpawn
            // 
            numMaxSpawn.Location = new Point(127, 40);
            numMaxSpawn.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numMaxSpawn.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxSpawn.Name = "numMaxSpawn";
            numMaxSpawn.Size = new Size(88, 27);
            numMaxSpawn.TabIndex = 2;
            numMaxSpawn.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxSpawn.ValueChanged += new EventHandler(numMaxSpawn_ValueChanged);
            // 
            // lblKoSpawn
            // 
            lblKoSpawn.AutoSize = true;
            lblKoSpawn.Location = new Point(20, 75);
            lblKoSpawn.Name = "lblKoSpawn";
            lblKoSpawn.Size = new Size(70, 20);
            lblKoSpawn.TabIndex = 1;
            lblKoSpawn.Text = "Num. KO:";
            // 
            // lblTotalSpawn
            // 
            lblTotalSpawn.AutoSize = true;
            lblTotalSpawn.Location = new Point(20, 42);
            lblTotalSpawn.Name = "lblTotalSpawn";
            lblTotalSpawn.Size = new Size(94, 20);
            lblTotalSpawn.TabIndex = 0;
            lblTotalSpawn.Text = "Max Spawns:";
            // 
            // OutbreakForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(956, 602);
            Controls.Add(grpMassInfo);
            Controls.Add(grpLocation);
            Controls.Add(grpPkmInfo);
            Controls.Add(btnPrev);
            Controls.Add(btnNext);
            Controls.Add(cmbOutbreaks);
            Controls.Add(grpMap);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OutbreakForm";
            ShowIcon = false;
            Text = "Mass Outbreak Editor";
            grpMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)imgMap).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            grpPkmInfo.ResumeLayout(false);
            grpPkmInfo.PerformLayout();
            grpLocation.ResumeLayout(false);
            grpLocationDummy.ResumeLayout(false);
            grpLocationDummy.PerformLayout();
            grpLocationCenter.ResumeLayout(false);
            grpLocationCenter.PerformLayout();
            grpMassInfo.ResumeLayout(false);
            grpMassInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numKO).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSpawn).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpMap;
        private PictureBox imgMap;
        private ComboBox cmbOutbreaks;
        private Button btnNext;
        private Button btnPrev;
        private PictureBox pictureBox;
        private GroupBox grpPkmInfo;
        private Label lblForm;
        private Label lblSpecies;
        private ComboBox cmbForm;
        private ComboBox cmbSpecies;
        private GroupBox grpLocation;
        private GroupBox grpLocationDummy;
        private GroupBox grpLocationCenter;
        private Label lblCenterZ;
        private Label lblCenterY;
        private Label lblCenterX;
        private TextBox txtCenterZ;
        private TextBox txtCenterY;
        private TextBox txtCenterX;
        private Label lblDummyZ;
        private TextBox txtDummyZ;
        private Label lblDummyY;
        private TextBox txtDummyY;
        private TextBox txtDummyX;
        private Label lblDummyX;
        private GroupBox grpMassInfo;
        private CheckBox chkFound;
        private NumericUpDown numKO;
        private NumericUpDown numMaxSpawn;
        private Label lblKoSpawn;
        private Label lblTotalSpawn;
    }
}