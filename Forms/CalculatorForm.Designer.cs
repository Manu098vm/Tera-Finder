﻿namespace TeraFinder
{
    partial class CalculatorForm
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
            this.bgWorkerSearch = new System.ComponentModel.BackgroundWorker();
            this.grpRaidDetails = new System.Windows.Forms.GroupBox();
            this.showresults = new System.Windows.Forms.CheckBox();
            this.numFrames = new System.Windows.Forms.NumericUpDown();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblFrames = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.cmbContent = new System.Windows.Forms.ComboBox();
            this.lblSeed = new System.Windows.Forms.Label();
            this.cmbProgress = new System.Windows.Forms.ComboBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.cmbEC = new System.Windows.Forms.ComboBox();
            this.lblEC = new System.Windows.Forms.Label();
            this.cmbNature = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cmbTeraType = new System.Windows.Forms.ComboBox();
            this.cmbSpecies = new System.Windows.Forms.ComboBox();
            this.cmbStars = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.cmbShiny = new System.Windows.Forms.ComboBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblShiny = new System.Windows.Forms.Label();
            this.lblAbility = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTera = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbAbility = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lblSpecies = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblNature = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblStars = new System.Windows.Forms.Label();
            this.nHpMax = new System.Windows.Forms.NumericUpDown();
            this.lblMaxIv = new System.Windows.Forms.Label();
            this.nHpMin = new System.Windows.Forms.NumericUpDown();
            this.nAtkMin = new System.Windows.Forms.NumericUpDown();
            this.lblMinIv = new System.Windows.Forms.Label();
            this.nAtkMax = new System.Windows.Forms.NumericUpDown();
            this.nDefMin = new System.Windows.Forms.NumericUpDown();
            this.nSpaMin = new System.Windows.Forms.NumericUpDown();
            this.lblSpe = new System.Windows.Forms.Label();
            this.nSpdMin = new System.Windows.Forms.NumericUpDown();
            this.lblSpd = new System.Windows.Forms.Label();
            this.lblSpa = new System.Windows.Forms.Label();
            this.lblDef = new System.Windows.Forms.Label();
            this.lblAtk = new System.Windows.Forms.Label();
            this.lblHp = new System.Windows.Forms.Label();
            this.nSpeMin = new System.Windows.Forms.NumericUpDown();
            this.nDefMax = new System.Windows.Forms.NumericUpDown();
            this.nSpaMax = new System.Windows.Forms.NumericUpDown();
            this.nSpdMax = new System.Windows.Forms.NumericUpDown();
            this.nSpeMax = new System.Windows.Forms.NumericUpDown();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.bgWorkerFilter = new System.ComponentModel.BackgroundWorker();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpGameInfo = new System.Windows.Forms.GroupBox();
            this.txtSID = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.txtTID = new System.Windows.Forms.TextBox();
            this.lblTID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGame = new System.Windows.Forms.ComboBox();
            this.grpRaidDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrames)).BeginInit();
            this.grpFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nHpMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHpMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAtkMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAtkMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDefMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpdMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDefMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpaMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpdMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.grpGameInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgWorkerSearch
            // 
            this.bgWorkerSearch.WorkerReportsProgress = true;
            this.bgWorkerSearch.WorkerSupportsCancellation = true;
            // 
            // grpRaidDetails
            // 
            this.grpRaidDetails.Controls.Add(this.showresults);
            this.grpRaidDetails.Controls.Add(this.numFrames);
            this.grpRaidDetails.Controls.Add(this.btnSearch);
            this.grpRaidDetails.Controls.Add(this.lblFrames);
            this.grpRaidDetails.Controls.Add(this.lblContent);
            this.grpRaidDetails.Controls.Add(this.txtSeed);
            this.grpRaidDetails.Controls.Add(this.cmbContent);
            this.grpRaidDetails.Controls.Add(this.lblSeed);
            this.grpRaidDetails.Location = new System.Drawing.Point(10, 9);
            this.grpRaidDetails.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRaidDetails.Name = "grpRaidDetails";
            this.grpRaidDetails.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRaidDetails.Size = new System.Drawing.Size(293, 184);
            this.grpRaidDetails.TabIndex = 0;
            this.grpRaidDetails.TabStop = false;
            this.grpRaidDetails.Text = "Raid Details";
            // 
            // showresults
            // 
            this.showresults.AutoSize = true;
            this.showresults.Location = new System.Drawing.Point(104, 116);
            this.showresults.Name = "showresults";
            this.showresults.Size = new System.Drawing.Size(112, 19);
            this.showresults.TabIndex = 9;
            this.showresults.Text = "Show All Results";
            this.showresults.UseVisualStyleBackColor = true;
            // 
            // numFrames
            // 
            this.numFrames.Location = new System.Drawing.Point(104, 68);
            this.numFrames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numFrames.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numFrames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrames.Name = "numFrames";
            this.numFrames.Size = new System.Drawing.Size(152, 23);
            this.numFrames.TabIndex = 8;
            this.numFrames.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(23, 137);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(234, 34);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblFrames
            // 
            this.lblFrames.AutoSize = true;
            this.lblFrames.Location = new System.Drawing.Point(23, 69);
            this.lblFrames.Name = "lblFrames";
            this.lblFrames.Size = new System.Drawing.Size(64, 15);
            this.lblFrames.TabIndex = 1;
            this.lblFrames.Text = "Max Calcs:";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(23, 93);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(53, 15);
            this.lblContent.TabIndex = 2;
            this.lblContent.Text = "Content:";
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(104, 42);
            this.txtSeed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSeed.MaxLength = 8;
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(153, 23);
            this.txtSeed.TabIndex = 1;
            this.txtSeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSeed_KeyPress);
            // 
            // cmbContent
            // 
            this.cmbContent.FormattingEnabled = true;
            this.cmbContent.Items.AddRange(new object[] {
            "Standard",
            "Black",
            "Event",
            "Event-Mighty"});
            this.cmbContent.Location = new System.Drawing.Point(104, 92);
            this.cmbContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbContent.Name = "cmbContent";
            this.cmbContent.Size = new System.Drawing.Size(153, 23);
            this.cmbContent.TabIndex = 1;
            this.cmbContent.SelectedIndexChanged += new System.EventHandler(this.cmbContent_IndexChanged);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(23, 45);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(35, 15);
            this.lblSeed.TabIndex = 1;
            this.lblSeed.Text = "Seed:";
            // 
            // cmbProgress
            // 
            this.cmbProgress.FormattingEnabled = true;
            this.cmbProgress.Items.AddRange(new object[] {
            "Beginning",
            "UnlockedTeraRaids",
            "Unlocked3Stars",
            "Unlocked4Stars",
            "Unlocked5Stars",
            "Unlocked6Stars"});
            this.cmbProgress.Location = new System.Drawing.Point(88, 59);
            this.cmbProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProgress.Name = "cmbProgress";
            this.cmbProgress.Size = new System.Drawing.Size(153, 23);
            this.cmbProgress.TabIndex = 7;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(18, 63);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(55, 15);
            this.lblProgress.TabIndex = 6;
            this.lblProgress.Text = "Progress:";
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.cmbEC);
            this.grpFilters.Controls.Add(this.lblEC);
            this.grpFilters.Controls.Add(this.cmbNature);
            this.grpFilters.Controls.Add(this.btnReset);
            this.grpFilters.Controls.Add(this.cmbTeraType);
            this.grpFilters.Controls.Add(this.cmbSpecies);
            this.grpFilters.Controls.Add(this.cmbStars);
            this.grpFilters.Controls.Add(this.btnApply);
            this.grpFilters.Controls.Add(this.cmbShiny);
            this.grpFilters.Controls.Add(this.cmbGender);
            this.grpFilters.Controls.Add(this.lblShiny);
            this.grpFilters.Controls.Add(this.lblAbility);
            this.grpFilters.Controls.Add(this.label5);
            this.grpFilters.Controls.Add(this.lblTera);
            this.grpFilters.Controls.Add(this.lblGender);
            this.grpFilters.Controls.Add(this.label6);
            this.grpFilters.Controls.Add(this.label11);
            this.grpFilters.Controls.Add(this.cmbAbility);
            this.grpFilters.Controls.Add(this.label12);
            this.grpFilters.Controls.Add(this.lblSpecies);
            this.grpFilters.Controls.Add(this.label13);
            this.grpFilters.Controls.Add(this.lblNature);
            this.grpFilters.Controls.Add(this.label14);
            this.grpFilters.Controls.Add(this.lblStars);
            this.grpFilters.Controls.Add(this.nHpMax);
            this.grpFilters.Controls.Add(this.lblMaxIv);
            this.grpFilters.Controls.Add(this.nHpMin);
            this.grpFilters.Controls.Add(this.nAtkMin);
            this.grpFilters.Controls.Add(this.lblMinIv);
            this.grpFilters.Controls.Add(this.nAtkMax);
            this.grpFilters.Controls.Add(this.nDefMin);
            this.grpFilters.Controls.Add(this.nSpaMin);
            this.grpFilters.Controls.Add(this.lblSpe);
            this.grpFilters.Controls.Add(this.nSpdMin);
            this.grpFilters.Controls.Add(this.lblSpd);
            this.grpFilters.Controls.Add(this.lblSpa);
            this.grpFilters.Controls.Add(this.lblDef);
            this.grpFilters.Controls.Add(this.lblAtk);
            this.grpFilters.Controls.Add(this.lblHp);
            this.grpFilters.Controls.Add(this.nSpeMin);
            this.grpFilters.Controls.Add(this.nDefMax);
            this.grpFilters.Controls.Add(this.nSpaMax);
            this.grpFilters.Controls.Add(this.nSpdMax);
            this.grpFilters.Controls.Add(this.nSpeMax);
            this.grpFilters.Location = new System.Drawing.Point(583, 9);
            this.grpFilters.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpFilters.Size = new System.Drawing.Size(809, 184);
            this.grpFilters.TabIndex = 4;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Filters";
            // 
            // cmbEC
            // 
            this.cmbEC.FormattingEnabled = true;
            this.cmbEC.Items.AddRange(new object[] {
            "Any",
            "EC % 100 = 0"});
            this.cmbEC.Location = new System.Drawing.Point(334, 112);
            this.cmbEC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbEC.Name = "cmbEC";
            this.cmbEC.Size = new System.Drawing.Size(170, 23);
            this.cmbEC.TabIndex = 34;
            // 
            // lblEC
            // 
            this.lblEC.AutoSize = true;
            this.lblEC.Location = new System.Drawing.Point(258, 115);
            this.lblEC.Name = "lblEC";
            this.lblEC.Size = new System.Drawing.Size(27, 15);
            this.lblEC.TabIndex = 33;
            this.lblEC.Text = "EC: ";
            // 
            // cmbNature
            // 
            this.cmbNature.FormattingEnabled = true;
            this.cmbNature.Items.AddRange(new object[] {
            "Hardy",
            "Lonely",
            "Brave",
            "Adamant",
            "Naughty",
            "Bold",
            "Docile",
            "Relaxed",
            "Impish",
            "Lax",
            "Timid",
            "Hasty",
            "Serious",
            "Jolly",
            "Naive",
            "Modest",
            "Mild",
            "Quiet",
            "Bashful",
            "Rash",
            "Calm",
            "Gentle",
            "Sassy",
            "Careful",
            "Quirky",
            "Any"});
            this.cmbNature.Location = new System.Drawing.Point(629, 57);
            this.cmbNature.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbNature.Name = "cmbNature";
            this.cmbNature.Size = new System.Drawing.Size(126, 23);
            this.cmbNature.TabIndex = 32;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(398, 143);
            this.btnReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(136, 28);
            this.btnReset.TabIndex = 31;
            this.btnReset.Text = "Reset Filters";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cmbTeraType
            // 
            this.cmbTeraType.FormattingEnabled = true;
            this.cmbTeraType.Items.AddRange(new object[] {
            "Any",
            "Normal",
            "Fighting",
            "Flying",
            "Poison",
            "Ground",
            "Rock",
            "Bug",
            "Ghost",
            "Steel",
            "Fire",
            "Water",
            "Grass",
            "Electric",
            "Psychic",
            "Ice",
            "Dragon",
            "Dark",
            "Fairy"});
            this.cmbTeraType.Location = new System.Drawing.Point(334, 85);
            this.cmbTeraType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTeraType.Name = "cmbTeraType";
            this.cmbTeraType.Size = new System.Drawing.Size(170, 23);
            this.cmbTeraType.TabIndex = 30;
            // 
            // cmbSpecies
            // 
            this.cmbSpecies.FormattingEnabled = true;
            this.cmbSpecies.Items.AddRange(new object[] {
            "Any"});
            this.cmbSpecies.Location = new System.Drawing.Point(334, 57);
            this.cmbSpecies.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbSpecies.Name = "cmbSpecies";
            this.cmbSpecies.Size = new System.Drawing.Size(170, 23);
            this.cmbSpecies.TabIndex = 29;
            this.cmbSpecies.SelectedIndexChanged += new System.EventHandler(this.cmbSpecies_IndexChanged);
            // 
            // cmbStars
            // 
            this.cmbStars.FormattingEnabled = true;
            this.cmbStars.Items.AddRange(new object[] {
            "Any",
            "1S ☆",
            "2S ☆☆",
            "3S ☆☆☆",
            "4S ☆☆☆☆",
            "5S ☆☆☆☆☆",
            "6S ☆☆☆☆☆☆",
            "7S ☆☆☆☆☆☆☆"});
            this.cmbStars.Location = new System.Drawing.Point(334, 29);
            this.cmbStars.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbStars.Name = "cmbStars";
            this.cmbStars.Size = new System.Drawing.Size(170, 23);
            this.cmbStars.TabIndex = 28;
            this.cmbStars.SelectedIndexChanged += new System.EventHandler(this.cmbStars_IndexChanged);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(549, 143);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(138, 28);
            this.btnApply.TabIndex = 25;
            this.btnApply.Text = "Apply Filters";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cmbShiny
            // 
            this.cmbShiny.FormattingEnabled = true;
            this.cmbShiny.Items.AddRange(new object[] {
            "Any",
            "No",
            "Yes",
            "Only Star",
            "Only Square"});
            this.cmbShiny.Location = new System.Drawing.Point(629, 112);
            this.cmbShiny.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbShiny.Name = "cmbShiny";
            this.cmbShiny.Size = new System.Drawing.Size(126, 23);
            this.cmbShiny.TabIndex = 24;
            // 
            // cmbGender
            // 
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "Any",
            "♂️",
            "♀️"});
            this.cmbGender.Location = new System.Drawing.Point(629, 85);
            this.cmbGender.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(126, 23);
            this.cmbGender.TabIndex = 23;
            // 
            // lblShiny
            // 
            this.lblShiny.AutoSize = true;
            this.lblShiny.Location = new System.Drawing.Point(553, 115);
            this.lblShiny.Name = "lblShiny";
            this.lblShiny.Size = new System.Drawing.Size(39, 15);
            this.lblShiny.TabIndex = 22;
            this.lblShiny.Text = "Shiny:";
            // 
            // lblAbility
            // 
            this.lblAbility.AutoSize = true;
            this.lblAbility.Location = new System.Drawing.Point(553, 32);
            this.lblAbility.Name = "lblAbility";
            this.lblAbility.Size = new System.Drawing.Size(44, 15);
            this.lblAbility.TabIndex = 21;
            this.lblAbility.Text = "Ability:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "~";
            // 
            // lblTera
            // 
            this.lblTera.AutoSize = true;
            this.lblTera.Location = new System.Drawing.Point(258, 87);
            this.lblTera.Name = "lblTera";
            this.lblTera.Size = new System.Drawing.Size(58, 15);
            this.lblTera.TabIndex = 8;
            this.lblTera.Text = "Tera Type:";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(553, 87);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(48, 15);
            this.lblGender.TabIndex = 20;
            this.lblGender.Text = "Gender:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "~";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(143, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 15);
            this.label11.TabIndex = 13;
            this.label11.Text = "~";
            // 
            // cmbAbility
            // 
            this.cmbAbility.FormattingEnabled = true;
            this.cmbAbility.Items.AddRange(new object[] {
            "Any",
            "(1)",
            "(2)",
            "(H)"});
            this.cmbAbility.Location = new System.Drawing.Point(629, 29);
            this.cmbAbility.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAbility.Name = "cmbAbility";
            this.cmbAbility.Size = new System.Drawing.Size(126, 23);
            this.cmbAbility.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(143, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 15);
            this.label12.TabIndex = 14;
            this.label12.Text = "~";
            // 
            // lblSpecies
            // 
            this.lblSpecies.AutoSize = true;
            this.lblSpecies.Location = new System.Drawing.Point(258, 59);
            this.lblSpecies.Name = "lblSpecies";
            this.lblSpecies.Size = new System.Drawing.Size(49, 15);
            this.lblSpecies.TabIndex = 9;
            this.lblSpecies.Text = "Species:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(143, 57);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 15);
            this.label13.TabIndex = 15;
            this.label13.Text = "~";
            // 
            // lblNature
            // 
            this.lblNature.AutoSize = true;
            this.lblNature.Location = new System.Drawing.Point(553, 59);
            this.lblNature.Name = "lblNature";
            this.lblNature.Size = new System.Drawing.Size(46, 15);
            this.lblNature.TabIndex = 7;
            this.lblNature.Text = "Nature:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(143, 34);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 15);
            this.label14.TabIndex = 16;
            this.label14.Text = "~";
            // 
            // lblStars
            // 
            this.lblStars.AutoSize = true;
            this.lblStars.Location = new System.Drawing.Point(258, 32);
            this.lblStars.Name = "lblStars";
            this.lblStars.Size = new System.Drawing.Size(35, 15);
            this.lblStars.TabIndex = 10;
            this.lblStars.Text = "Stars:";
            // 
            // nHpMax
            // 
            this.nHpMax.Location = new System.Drawing.Point(161, 31);
            this.nHpMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nHpMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nHpMax.Name = "nHpMax";
            this.nHpMax.Size = new System.Drawing.Size(48, 23);
            this.nHpMax.TabIndex = 11;
            // 
            // lblMaxIv
            // 
            this.lblMaxIv.AutoSize = true;
            this.lblMaxIv.Location = new System.Drawing.Point(161, 12);
            this.lblMaxIv.Name = "lblMaxIv";
            this.lblMaxIv.Size = new System.Drawing.Size(43, 15);
            this.lblMaxIv.TabIndex = 5;
            this.lblMaxIv.Text = "Max IV";
            // 
            // nHpMin
            // 
            this.nHpMin.Location = new System.Drawing.Point(89, 31);
            this.nHpMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nHpMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nHpMin.Name = "nHpMin";
            this.nHpMin.Size = new System.Drawing.Size(48, 23);
            this.nHpMin.TabIndex = 10;
            // 
            // nAtkMin
            // 
            this.nAtkMin.Location = new System.Drawing.Point(89, 56);
            this.nAtkMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nAtkMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nAtkMin.Name = "nAtkMin";
            this.nAtkMin.Size = new System.Drawing.Size(48, 23);
            this.nAtkMin.TabIndex = 9;
            // 
            // lblMinIv
            // 
            this.lblMinIv.AutoSize = true;
            this.lblMinIv.Location = new System.Drawing.Point(89, 14);
            this.lblMinIv.Name = "lblMinIv";
            this.lblMinIv.Size = new System.Drawing.Size(41, 15);
            this.lblMinIv.TabIndex = 6;
            this.lblMinIv.Text = "Min IV";
            // 
            // nAtkMax
            // 
            this.nAtkMax.Location = new System.Drawing.Point(161, 56);
            this.nAtkMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nAtkMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nAtkMax.Name = "nAtkMax";
            this.nAtkMax.Size = new System.Drawing.Size(48, 23);
            this.nAtkMax.TabIndex = 8;
            // 
            // nDefMin
            // 
            this.nDefMin.Location = new System.Drawing.Point(89, 80);
            this.nDefMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nDefMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nDefMin.Name = "nDefMin";
            this.nDefMin.Size = new System.Drawing.Size(48, 23);
            this.nDefMin.TabIndex = 7;
            // 
            // nSpaMin
            // 
            this.nSpaMin.Location = new System.Drawing.Point(89, 105);
            this.nSpaMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpaMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpaMin.Name = "nSpaMin";
            this.nSpaMin.Size = new System.Drawing.Size(48, 23);
            this.nSpaMin.TabIndex = 6;
            // 
            // lblSpe
            // 
            this.lblSpe.AutoSize = true;
            this.lblSpe.Location = new System.Drawing.Point(50, 156);
            this.lblSpe.Name = "lblSpe";
            this.lblSpe.Size = new System.Drawing.Size(29, 15);
            this.lblSpe.TabIndex = 11;
            this.lblSpe.Text = "Spe:";
            // 
            // nSpdMin
            // 
            this.nSpdMin.Location = new System.Drawing.Point(89, 130);
            this.nSpdMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpdMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpdMin.Name = "nSpdMin";
            this.nSpdMin.Size = new System.Drawing.Size(48, 23);
            this.nSpdMin.TabIndex = 5;
            // 
            // lblSpd
            // 
            this.lblSpd.AutoSize = true;
            this.lblSpd.Location = new System.Drawing.Point(50, 133);
            this.lblSpd.Name = "lblSpd";
            this.lblSpd.Size = new System.Drawing.Size(31, 15);
            this.lblSpd.TabIndex = 16;
            this.lblSpd.Text = "SpD:";
            // 
            // lblSpa
            // 
            this.lblSpa.AutoSize = true;
            this.lblSpa.Location = new System.Drawing.Point(50, 108);
            this.lblSpa.Name = "lblSpa";
            this.lblSpa.Size = new System.Drawing.Size(31, 15);
            this.lblSpa.TabIndex = 15;
            this.lblSpa.Text = "SpA:";
            // 
            // lblDef
            // 
            this.lblDef.AutoSize = true;
            this.lblDef.Location = new System.Drawing.Point(50, 83);
            this.lblDef.Name = "lblDef";
            this.lblDef.Size = new System.Drawing.Size(28, 15);
            this.lblDef.TabIndex = 14;
            this.lblDef.Text = "Def:";
            // 
            // lblAtk
            // 
            this.lblAtk.AutoSize = true;
            this.lblAtk.Location = new System.Drawing.Point(50, 58);
            this.lblAtk.Name = "lblAtk";
            this.lblAtk.Size = new System.Drawing.Size(28, 15);
            this.lblAtk.TabIndex = 13;
            this.lblAtk.Text = "Atk:";
            // 
            // lblHp
            // 
            this.lblHp.AutoSize = true;
            this.lblHp.Location = new System.Drawing.Point(50, 32);
            this.lblHp.Name = "lblHp";
            this.lblHp.Size = new System.Drawing.Size(26, 15);
            this.lblHp.TabIndex = 12;
            this.lblHp.Text = "HP:";
            // 
            // nSpeMin
            // 
            this.nSpeMin.Location = new System.Drawing.Point(89, 154);
            this.nSpeMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpeMin.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpeMin.Name = "nSpeMin";
            this.nSpeMin.Size = new System.Drawing.Size(48, 23);
            this.nSpeMin.TabIndex = 4;
            // 
            // nDefMax
            // 
            this.nDefMax.Location = new System.Drawing.Point(161, 80);
            this.nDefMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nDefMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nDefMax.Name = "nDefMax";
            this.nDefMax.Size = new System.Drawing.Size(48, 23);
            this.nDefMax.TabIndex = 3;
            // 
            // nSpaMax
            // 
            this.nSpaMax.Location = new System.Drawing.Point(161, 105);
            this.nSpaMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpaMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpaMax.Name = "nSpaMax";
            this.nSpaMax.Size = new System.Drawing.Size(48, 23);
            this.nSpaMax.TabIndex = 2;
            // 
            // nSpdMax
            // 
            this.nSpdMax.Location = new System.Drawing.Point(161, 130);
            this.nSpdMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpdMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpdMax.Name = "nSpdMax";
            this.nSpdMax.Size = new System.Drawing.Size(48, 23);
            this.nSpdMax.TabIndex = 1;
            // 
            // nSpeMax
            // 
            this.nSpeMax.Location = new System.Drawing.Point(161, 154);
            this.nSpeMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nSpeMax.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nSpeMax.Name = "nSpeMax";
            this.nSpeMax.Size = new System.Drawing.Size(48, 23);
            this.nSpeMax.TabIndex = 0;
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(10, 197);
            this.dataGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.RowHeadersWidth = 51;
            this.dataGrid.RowTemplate.Height = 29;
            this.dataGrid.Size = new System.Drawing.Size(1342, 354);
            this.dataGrid.TabIndex = 5;
            // 
            // bgWorkerFilter
            // 
            this.bgWorkerFilter.WorkerReportsProgress = true;
            this.bgWorkerFilter.WorkerSupportsCancellation = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 582);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1381, 19);
            this.progressBar.TabIndex = 6;
            // 
            // grpGameInfo
            // 
            this.grpGameInfo.Controls.Add(this.txtSID);
            this.grpGameInfo.Controls.Add(this.lblSID);
            this.grpGameInfo.Controls.Add(this.txtTID);
            this.grpGameInfo.Controls.Add(this.lblTID);
            this.grpGameInfo.Controls.Add(this.label2);
            this.grpGameInfo.Controls.Add(this.cmbProgress);
            this.grpGameInfo.Controls.Add(this.lblProgress);
            this.grpGameInfo.Controls.Add(this.cmbGame);
            this.grpGameInfo.Location = new System.Drawing.Point(309, 9);
            this.grpGameInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpGameInfo.Name = "grpGameInfo";
            this.grpGameInfo.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpGameInfo.Size = new System.Drawing.Size(269, 184);
            this.grpGameInfo.TabIndex = 7;
            this.grpGameInfo.TabStop = false;
            this.grpGameInfo.Text = "Game Info";
            // 
            // txtSID
            // 
            this.txtSID.Location = new System.Drawing.Point(88, 115);
            this.txtSID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(153, 23);
            this.txtSID.TabIndex = 11;
            // 
            // lblSID
            // 
            this.lblSID.AutoSize = true;
            this.lblSID.Location = new System.Drawing.Point(18, 117);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(27, 15);
            this.lblSID.TabIndex = 10;
            this.lblSID.Text = "SID:";
            // 
            // txtTID
            // 
            this.txtTID.Location = new System.Drawing.Point(88, 87);
            this.txtTID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTID.Name = "txtTID";
            this.txtTID.Size = new System.Drawing.Size(153, 23);
            this.txtTID.TabIndex = 9;
            // 
            // lblTID
            // 
            this.lblTID.AutoSize = true;
            this.lblTID.Location = new System.Drawing.Point(18, 89);
            this.lblTID.Name = "lblTID";
            this.lblTID.Size = new System.Drawing.Size(27, 15);
            this.lblTID.TabIndex = 8;
            this.lblTID.Text = "TID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Game:";
            // 
            // cmbGame
            // 
            this.cmbGame.FormattingEnabled = true;
            this.cmbGame.Items.AddRange(new object[] {
            "Scarlet",
            "Violet"});
            this.cmbGame.Location = new System.Drawing.Point(88, 30);
            this.cmbGame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbGame.Name = "cmbGame";
            this.cmbGame.Size = new System.Drawing.Size(153, 23);
            this.cmbGame.TabIndex = 0;
            // 
            // CalculatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 562);
            this.Controls.Add(this.grpGameInfo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.grpRaidDetails);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CalculatorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Raid Calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.grpRaidDetails.ResumeLayout(false);
            this.grpRaidDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFrames)).EndInit();
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nHpMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHpMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAtkMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nAtkMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDefMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpdMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDefMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpaMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpdMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.grpGameInfo.ResumeLayout(false);
            this.grpGameInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgWorkerSearch;
        private GroupBox grpRaidDetails;
        private TextBox txtSeed;
        private Label lblSeed;
        private ComboBox cmbContent;
        private Label lblContent;
        private Label lblFrames;
        private GroupBox grpFilters;
        private Button btnSearch;
        private Label label5;
        private Label lblTera;
        private Label label6;
        private Label lblSpecies;
        private Label label11;
        private Label lblStars;
        private Label label12;
        private Label label13;
        private Label label14;
        private NumericUpDown nHpMax;
        private Label lblMaxIv;
        private NumericUpDown nHpMin;
        private NumericUpDown nAtkMin;
        private Label lblMinIv;
        private NumericUpDown nAtkMax;
        private NumericUpDown nDefMin;
        private NumericUpDown nSpaMin;
        private Label lblSpe;
        private NumericUpDown nSpdMin;
        private Label lblSpd;
        private Label lblSpa;
        private Label lblDef;
        private Label lblAtk;
        private Label lblHp;
        private NumericUpDown nSpeMin;
        private NumericUpDown nDefMax;
        private NumericUpDown nSpaMax;
        private NumericUpDown nSpdMax;
        private NumericUpDown nSpeMax;
        private Label lblNature;
        private Label lblAbility;
        private Label lblShiny;
        private Label lblGender;
        private ComboBox cmbShiny;
        private ComboBox cmbGender;
        private ComboBox cmbAbility;
        private DataGridView dataGrid;
        private Button btnApply;
        private System.ComponentModel.BackgroundWorker bgWorkerFilter;
        private ComboBox cmbTeraType;
        private ComboBox cmbSpecies;
        private ComboBox cmbStars;
        private Button btnReset;
        private ComboBox cmbProgress;
        private Label lblProgress;
        private NumericUpDown numFrames;
        private ComboBox cmbNature;
        private ProgressBar progressBar;
        private GroupBox grpGameInfo;
        private Label label2;
        private ComboBox cmbGame;
        private TextBox txtSID;
        private Label lblSID;
        private TextBox txtTID;
        private Label lblTID;
        private ComboBox cmbEC;
        private Label lblEC;
        public CheckBox showresults;
    }
}