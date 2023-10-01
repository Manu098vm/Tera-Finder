namespace TeraFinder.Plugins;

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
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatorForm));
        grpRaidDetails = new GroupBox();
        lblMap = new Label();
        cmbMap = new ComboBox();
        showresults = new CheckBox();
        numFrames = new NumericUpDown();
        btnSearch = new Button();
        lblFrames = new Label();
        lblContent = new Label();
        txtSeed = new TextBox();
        cmbContent = new ComboBox();
        lblSeed = new Label();
        lblFound = new Label();
        cmbProgress = new ComboBox();
        lblProgress = new Label();
        grpFilters = new GroupBox();
        label1 = new Label();
        numScaleMax = new NumericUpDown();
        numScaleMin = new NumericUpDown();
        lblScale = new Label();
        cmbEC = new ComboBox();
        lblEC = new Label();
        cmbNature = new ComboBox();
        btnReset = new Button();
        cmbTeraType = new ComboBox();
        cmbSpecies = new ComboBox();
        cmbStars = new ComboBox();
        btnApply = new Button();
        cmbShiny = new ComboBox();
        cmbGender = new ComboBox();
        lblShiny = new Label();
        lblAbility = new Label();
        label5 = new Label();
        lblTera = new Label();
        lblGender = new Label();
        label6 = new Label();
        label11 = new Label();
        cmbAbility = new ComboBox();
        label12 = new Label();
        lblSpecies = new Label();
        label13 = new Label();
        lblNature = new Label();
        label14 = new Label();
        lblStars = new Label();
        nHpMax = new NumericUpDown();
        lblMaxIv = new Label();
        nHpMin = new NumericUpDown();
        nAtkMin = new NumericUpDown();
        lblMinIv = new Label();
        nAtkMax = new NumericUpDown();
        nDefMin = new NumericUpDown();
        nSpaMin = new NumericUpDown();
        lblSpe = new Label();
        nSpdMin = new NumericUpDown();
        lblSpd = new Label();
        lblSpa = new Label();
        lblDef = new Label();
        lblAtk = new Label();
        lblHp = new Label();
        nSpeMin = new NumericUpDown();
        nDefMax = new NumericUpDown();
        nSpaMax = new NumericUpDown();
        nSpdMax = new NumericUpDown();
        nSpeMax = new NumericUpDown();
        dataGrid = new DataGridView();
        grpGameInfo = new GroupBox();
        txtSID = new TextBox();
        lblSID = new Label();
        txtTID = new TextBox();
        lblTID = new Label();
        lblGame = new Label();
        cmbGame = new ComboBox();
        toolTip = new ToolTip(components);
        contextMenuStrip = new ContextMenuStrip(components);
        btnViewRewards = new ToolStripMenuItem();
        btnSaveAll = new ToolStripMenuItem();
        btnSave = new ToolStripMenuItem();
        btnSavePk9 = new ToolStripMenuItem();
        btnToPkmEditor = new ToolStripMenuItem();
        btnSendToEditor = new ToolStripMenuItem();
        btnCopySeed = new ToolStripMenuItem();
        grpRaidDetails.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFrames).BeginInit();
        grpFilters.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numScaleMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numScaleMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nHpMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nHpMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nAtkMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nAtkMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nDefMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpaMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpdMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpeMin).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nDefMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpaMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpdMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nSpeMax).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
        grpGameInfo.SuspendLayout();
        contextMenuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // grpRaidDetails
        // 
        grpRaidDetails.Controls.Add(lblMap);
        grpRaidDetails.Controls.Add(cmbMap);
        grpRaidDetails.Controls.Add(showresults);
        grpRaidDetails.Controls.Add(numFrames);
        grpRaidDetails.Controls.Add(btnSearch);
        grpRaidDetails.Controls.Add(lblFrames);
        grpRaidDetails.Controls.Add(lblContent);
        grpRaidDetails.Controls.Add(txtSeed);
        grpRaidDetails.Controls.Add(cmbContent);
        grpRaidDetails.Controls.Add(lblSeed);
        grpRaidDetails.Location = new Point(11, 12);
        grpRaidDetails.Margin = new Padding(3, 4, 3, 4);
        grpRaidDetails.Name = "grpRaidDetails";
        grpRaidDetails.Padding = new Padding(3, 4, 3, 4);
        grpRaidDetails.Size = new Size(335, 245);
        grpRaidDetails.TabIndex = 0;
        grpRaidDetails.TabStop = false;
        grpRaidDetails.Text = "Raid Settings";
        // 
        // lblMap
        // 
        lblMap.AutoSize = true;
        lblMap.Location = new Point(16, 36);
        lblMap.Name = "lblMap";
        lblMap.Size = new Size(69, 20);
        lblMap.TabIndex = 10;
        lblMap.Text = "Location:";
        // 
        // cmbMap
        // 
        cmbMap.FormattingEnabled = true;
        cmbMap.Location = new Point(119, 33);
        cmbMap.Name = "cmbMap";
        cmbMap.Size = new Size(174, 28);
        cmbMap.TabIndex = 0;
        cmbMap.SelectedIndexChanged += cmbMap_IndexChanged;
        // 
        // showresults
        // 
        showresults.AutoSize = true;
        showresults.Location = new Point(25, 171);
        showresults.Margin = new Padding(3, 4, 3, 4);
        showresults.Name = "showresults";
        showresults.Size = new Size(160, 24);
        showresults.TabIndex = 4;
        showresults.Text = "Show All Results (?)";
        showresults.UseVisualStyleBackColor = true;
        // 
        // numFrames
        // 
        numFrames.Location = new Point(119, 103);
        numFrames.Margin = new Padding(3, 4, 3, 4);
        numFrames.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
        numFrames.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numFrames.Name = "numFrames";
        numFrames.Size = new Size(174, 27);
        numFrames.TabIndex = 2;
        numFrames.Value = new decimal(new int[] { 100000, 0, 0, 0 });
        // 
        // btnSearch
        // 
        btnSearch.Location = new Point(16, 196);
        btnSearch.Margin = new Padding(3, 4, 3, 4);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(277, 40);
        btnSearch.TabIndex = 5;
        btnSearch.Text = "Search";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        // 
        // lblFrames
        // 
        lblFrames.AutoSize = true;
        lblFrames.Location = new Point(16, 105);
        lblFrames.Name = "lblFrames";
        lblFrames.Size = new Size(78, 20);
        lblFrames.TabIndex = 1;
        lblFrames.Text = "Max Calcs:";
        // 
        // lblContent
        // 
        lblContent.AutoSize = true;
        lblContent.Location = new Point(16, 138);
        lblContent.Name = "lblContent";
        lblContent.Size = new Size(64, 20);
        lblContent.TabIndex = 2;
        lblContent.Text = "Content:";
        // 
        // txtSeed
        // 
        txtSeed.Location = new Point(119, 68);
        txtSeed.Margin = new Padding(3, 4, 3, 4);
        txtSeed.MaxLength = 8;
        txtSeed.Name = "txtSeed";
        txtSeed.Size = new Size(174, 27);
        txtSeed.TabIndex = 1;
        txtSeed.KeyPress += TxtSeed_KeyPress;
        // 
        // cmbContent
        // 
        cmbContent.FormattingEnabled = true;
        cmbContent.Items.AddRange(new object[] { "Standard", "Black", "Event", "Event-Mighty" });
        cmbContent.Location = new Point(119, 135);
        cmbContent.Margin = new Padding(3, 4, 3, 4);
        cmbContent.Name = "cmbContent";
        cmbContent.Size = new Size(174, 28);
        cmbContent.TabIndex = 3;
        cmbContent.SelectedIndexChanged += cmbContent_IndexChanged;
        // 
        // lblSeed
        // 
        lblSeed.AutoSize = true;
        lblSeed.Location = new Point(16, 71);
        lblSeed.Name = "lblSeed";
        lblSeed.Size = new Size(45, 20);
        lblSeed.TabIndex = 1;
        lblSeed.Text = "Seed:";
        // 
        // lblFound
        // 
        lblFound.AutoSize = true;
        lblFound.Location = new Point(21, 213);
        lblFound.Name = "lblFound";
        lblFound.Size = new Size(53, 20);
        lblFound.TabIndex = 12;
        lblFound.Text = "Found:";
        lblFound.Visible = false;
        // 
        // cmbProgress
        // 
        cmbProgress.FormattingEnabled = true;
        cmbProgress.Items.AddRange(new object[] { "Beginning", "UnlockedTeraRaids", "Unlocked3Stars", "Unlocked4Stars", "Unlocked5Stars", "Unlocked6Stars" });
        cmbProgress.Location = new Point(101, 79);
        cmbProgress.Margin = new Padding(3, 4, 3, 4);
        cmbProgress.Name = "cmbProgress";
        cmbProgress.Size = new Size(174, 28);
        cmbProgress.TabIndex = 7;
        // 
        // lblProgress
        // 
        lblProgress.AutoSize = true;
        lblProgress.Location = new Point(21, 84);
        lblProgress.Name = "lblProgress";
        lblProgress.Size = new Size(68, 20);
        lblProgress.TabIndex = 6;
        lblProgress.Text = "Progress:";
        // 
        // grpFilters
        // 
        grpFilters.Controls.Add(label1);
        grpFilters.Controls.Add(numScaleMax);
        grpFilters.Controls.Add(numScaleMin);
        grpFilters.Controls.Add(lblScale);
        grpFilters.Controls.Add(cmbEC);
        grpFilters.Controls.Add(lblEC);
        grpFilters.Controls.Add(cmbNature);
        grpFilters.Controls.Add(btnReset);
        grpFilters.Controls.Add(cmbTeraType);
        grpFilters.Controls.Add(cmbSpecies);
        grpFilters.Controls.Add(cmbStars);
        grpFilters.Controls.Add(btnApply);
        grpFilters.Controls.Add(cmbShiny);
        grpFilters.Controls.Add(cmbGender);
        grpFilters.Controls.Add(lblShiny);
        grpFilters.Controls.Add(lblAbility);
        grpFilters.Controls.Add(label5);
        grpFilters.Controls.Add(lblTera);
        grpFilters.Controls.Add(lblGender);
        grpFilters.Controls.Add(label6);
        grpFilters.Controls.Add(label11);
        grpFilters.Controls.Add(cmbAbility);
        grpFilters.Controls.Add(label12);
        grpFilters.Controls.Add(lblSpecies);
        grpFilters.Controls.Add(label13);
        grpFilters.Controls.Add(lblNature);
        grpFilters.Controls.Add(label14);
        grpFilters.Controls.Add(lblStars);
        grpFilters.Controls.Add(nHpMax);
        grpFilters.Controls.Add(lblMaxIv);
        grpFilters.Controls.Add(nHpMin);
        grpFilters.Controls.Add(nAtkMin);
        grpFilters.Controls.Add(lblMinIv);
        grpFilters.Controls.Add(nAtkMax);
        grpFilters.Controls.Add(nDefMin);
        grpFilters.Controls.Add(nSpaMin);
        grpFilters.Controls.Add(lblSpe);
        grpFilters.Controls.Add(nSpdMin);
        grpFilters.Controls.Add(lblSpd);
        grpFilters.Controls.Add(lblSpa);
        grpFilters.Controls.Add(lblDef);
        grpFilters.Controls.Add(lblAtk);
        grpFilters.Controls.Add(lblHp);
        grpFilters.Controls.Add(nSpeMin);
        grpFilters.Controls.Add(nDefMax);
        grpFilters.Controls.Add(nSpaMax);
        grpFilters.Controls.Add(nSpdMax);
        grpFilters.Controls.Add(nSpeMax);
        grpFilters.Location = new Point(666, 12);
        grpFilters.Margin = new Padding(3, 4, 3, 4);
        grpFilters.Name = "grpFilters";
        grpFilters.Padding = new Padding(3, 4, 3, 4);
        grpFilters.Size = new Size(811, 245);
        grpFilters.TabIndex = 3;
        grpFilters.TabStop = false;
        grpFilters.Text = "Filters";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(376, 199);
        label1.Name = "label1";
        label1.Size = new Size(19, 20);
        label1.TabIndex = 39;
        label1.Text = "~";
        // 
        // numScaleMax
        // 
        numScaleMax.Location = new Point(401, 197);
        numScaleMax.Margin = new Padding(3, 4, 3, 4);
        numScaleMax.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        numScaleMax.Name = "numScaleMax";
        numScaleMax.Size = new Size(55, 27);
        numScaleMax.TabIndex = 31;
        numScaleMax.Value = new decimal(new int[] { 255, 0, 0, 0 });
        // 
        // numScaleMin
        // 
        numScaleMin.Location = new Point(315, 197);
        numScaleMin.Margin = new Padding(3, 4, 3, 4);
        numScaleMin.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        numScaleMin.Name = "numScaleMin";
        numScaleMin.Size = new Size(55, 27);
        numScaleMin.TabIndex = 30;
        // 
        // lblScale
        // 
        lblScale.AutoSize = true;
        lblScale.Location = new Point(255, 199);
        lblScale.Name = "lblScale";
        lblScale.Size = new Size(47, 20);
        lblScale.TabIndex = 35;
        lblScale.Text = "Scale:";
        // 
        // cmbEC
        // 
        cmbEC.FormattingEnabled = true;
        cmbEC.Items.AddRange(new object[] { "Any", "EC % 100 = 0" });
        cmbEC.Location = new Point(341, 145);
        cmbEC.Margin = new Padding(3, 4, 3, 4);
        cmbEC.Name = "cmbEC";
        cmbEC.Size = new Size(194, 28);
        cmbEC.TabIndex = 25;
        // 
        // lblEC
        // 
        lblEC.AutoSize = true;
        lblEC.Location = new Point(255, 149);
        lblEC.Name = "lblEC";
        lblEC.Size = new Size(33, 20);
        lblEC.TabIndex = 33;
        lblEC.Text = "EC: ";
        // 
        // cmbNature
        // 
        cmbNature.FormattingEnabled = true;
        cmbNature.Items.AddRange(new object[] { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky", "Any" });
        cmbNature.Location = new Point(657, 72);
        cmbNature.Margin = new Padding(3, 4, 3, 4);
        cmbNature.Name = "cmbNature";
        cmbNature.Size = new Size(143, 28);
        cmbNature.TabIndex = 27;
        // 
        // btnReset
        // 
        btnReset.Location = new Point(485, 191);
        btnReset.Margin = new Padding(3, 4, 3, 4);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(140, 36);
        btnReset.TabIndex = 32;
        btnReset.Text = "Reset Filters";
        btnReset.UseVisualStyleBackColor = true;
        btnReset.Click += btnReset_Click;
        // 
        // cmbTeraType
        // 
        cmbTeraType.FormattingEnabled = true;
        cmbTeraType.Items.AddRange(new object[] { "Any", "Normal", "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Ghost", "Steel", "Fire", "Water", "Grass", "Electric", "Psychic", "Ice", "Dragon", "Dark", "Fairy" });
        cmbTeraType.Location = new Point(341, 109);
        cmbTeraType.Margin = new Padding(3, 4, 3, 4);
        cmbTeraType.Name = "cmbTeraType";
        cmbTeraType.Size = new Size(194, 28);
        cmbTeraType.TabIndex = 24;
        // 
        // cmbSpecies
        // 
        cmbSpecies.FormattingEnabled = true;
        cmbSpecies.Items.AddRange(new object[] { "Any" });
        cmbSpecies.Location = new Point(341, 72);
        cmbSpecies.Margin = new Padding(3, 4, 3, 4);
        cmbSpecies.Name = "cmbSpecies";
        cmbSpecies.Size = new Size(194, 28);
        cmbSpecies.TabIndex = 23;
        // 
        // cmbStars
        // 
        cmbStars.FormattingEnabled = true;
        cmbStars.Items.AddRange(new object[] { "Any", "1S ☆", "2S ☆☆", "3S ☆☆☆", "4S ☆☆☆☆", "5S ☆☆☆☆☆", "6S ☆☆☆☆☆☆", "7S ☆☆☆☆☆☆☆" });
        cmbStars.Location = new Point(341, 35);
        cmbStars.Margin = new Padding(3, 4, 3, 4);
        cmbStars.Name = "cmbStars";
        cmbStars.Size = new Size(194, 28);
        cmbStars.TabIndex = 22;
        cmbStars.SelectedIndexChanged += cmbStars_IndexChanged;
        // 
        // btnApply
        // 
        btnApply.Location = new Point(657, 191);
        btnApply.Margin = new Padding(3, 4, 3, 4);
        btnApply.Name = "btnApply";
        btnApply.Size = new Size(143, 36);
        btnApply.TabIndex = 33;
        btnApply.Text = "Apply Filters";
        btnApply.UseVisualStyleBackColor = true;
        btnApply.Click += btnApply_Click;
        // 
        // cmbShiny
        // 
        cmbShiny.FormattingEnabled = true;
        cmbShiny.Items.AddRange(new object[] { "Any", "No", "Yes", "Only Star", "Only Square" });
        cmbShiny.Location = new Point(657, 145);
        cmbShiny.Margin = new Padding(3, 4, 3, 4);
        cmbShiny.Name = "cmbShiny";
        cmbShiny.Size = new Size(143, 28);
        cmbShiny.TabIndex = 29;
        // 
        // cmbGender
        // 
        cmbGender.FormattingEnabled = true;
        cmbGender.Items.AddRange(new object[] { "Any", "♂️", "♀️" });
        cmbGender.Location = new Point(657, 109);
        cmbGender.Margin = new Padding(3, 4, 3, 4);
        cmbGender.Name = "cmbGender";
        cmbGender.Size = new Size(143, 28);
        cmbGender.TabIndex = 28;
        // 
        // lblShiny
        // 
        lblShiny.AutoSize = true;
        lblShiny.Location = new Point(570, 149);
        lblShiny.Name = "lblShiny";
        lblShiny.Size = new Size(47, 20);
        lblShiny.TabIndex = 22;
        lblShiny.Text = "Shiny:";
        // 
        // lblAbility
        // 
        lblAbility.AutoSize = true;
        lblAbility.Location = new Point(570, 39);
        lblAbility.Name = "lblAbility";
        lblAbility.Size = new Size(55, 20);
        lblAbility.TabIndex = 21;
        lblAbility.Text = "Ability:";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(142, 208);
        label5.Name = "label5";
        label5.Size = new Size(19, 20);
        label5.TabIndex = 11;
        label5.Text = "~";
        // 
        // lblTera
        // 
        lblTera.AutoSize = true;
        lblTera.Location = new Point(255, 112);
        lblTera.Name = "lblTera";
        lblTera.Size = new Size(75, 20);
        lblTera.TabIndex = 8;
        lblTera.Text = "Tera Type:";
        // 
        // lblGender
        // 
        lblGender.AutoSize = true;
        lblGender.Location = new Point(570, 112);
        lblGender.Name = "lblGender";
        lblGender.Size = new Size(60, 20);
        lblGender.TabIndex = 20;
        lblGender.Text = "Gender:";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(142, 175);
        label6.Name = "label6";
        label6.Size = new Size(19, 20);
        label6.TabIndex = 12;
        label6.Text = "~";
        // 
        // label11
        // 
        label11.AutoSize = true;
        label11.Location = new Point(142, 141);
        label11.Name = "label11";
        label11.Size = new Size(19, 20);
        label11.TabIndex = 13;
        label11.Text = "~";
        // 
        // cmbAbility
        // 
        cmbAbility.FormattingEnabled = true;
        cmbAbility.Items.AddRange(new object[] { "Any", "(1)", "(2)", "(H)" });
        cmbAbility.Location = new Point(657, 35);
        cmbAbility.Margin = new Padding(3, 4, 3, 4);
        cmbAbility.Name = "cmbAbility";
        cmbAbility.Size = new Size(143, 28);
        cmbAbility.TabIndex = 26;
        // 
        // label12
        // 
        label12.AutoSize = true;
        label12.Location = new Point(142, 111);
        label12.Name = "label12";
        label12.Size = new Size(19, 20);
        label12.TabIndex = 14;
        label12.Text = "~";
        // 
        // lblSpecies
        // 
        lblSpecies.AutoSize = true;
        lblSpecies.Location = new Point(255, 75);
        lblSpecies.Name = "lblSpecies";
        lblSpecies.Size = new Size(62, 20);
        lblSpecies.TabIndex = 9;
        lblSpecies.Text = "Species:";
        // 
        // label13
        // 
        label13.AutoSize = true;
        label13.Location = new Point(142, 76);
        label13.Name = "label13";
        label13.Size = new Size(19, 20);
        label13.TabIndex = 15;
        label13.Text = "~";
        // 
        // lblNature
        // 
        lblNature.AutoSize = true;
        lblNature.Location = new Point(570, 75);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(57, 20);
        lblNature.TabIndex = 7;
        lblNature.Text = "Nature:";
        // 
        // label14
        // 
        label14.AutoSize = true;
        label14.Location = new Point(142, 45);
        label14.Name = "label14";
        label14.Size = new Size(19, 20);
        label14.TabIndex = 16;
        label14.Text = "~";
        // 
        // lblStars
        // 
        lblStars.AutoSize = true;
        lblStars.Location = new Point(255, 39);
        lblStars.Name = "lblStars";
        lblStars.Size = new Size(44, 20);
        lblStars.TabIndex = 10;
        lblStars.Text = "Stars:";
        // 
        // nHpMax
        // 
        nHpMax.Location = new Point(162, 42);
        nHpMax.Margin = new Padding(3, 4, 3, 4);
        nHpMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nHpMax.Name = "nHpMax";
        nHpMax.Size = new Size(55, 27);
        nHpMax.TabIndex = 11;
        // 
        // lblMaxIv
        // 
        lblMaxIv.AutoSize = true;
        lblMaxIv.Location = new Point(158, 19);
        lblMaxIv.Name = "lblMaxIv";
        lblMaxIv.Size = new Size(54, 20);
        lblMaxIv.TabIndex = 5;
        lblMaxIv.Text = "Max IV";
        // 
        // nHpMin
        // 
        nHpMin.Location = new Point(81, 42);
        nHpMin.Margin = new Padding(3, 4, 3, 4);
        nHpMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nHpMin.Name = "nHpMin";
        nHpMin.Size = new Size(55, 27);
        nHpMin.TabIndex = 10;
        // 
        // nAtkMin
        // 
        nAtkMin.Location = new Point(81, 75);
        nAtkMin.Margin = new Padding(3, 4, 3, 4);
        nAtkMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nAtkMin.Name = "nAtkMin";
        nAtkMin.Size = new Size(55, 27);
        nAtkMin.TabIndex = 12;
        // 
        // lblMinIv
        // 
        lblMinIv.AutoSize = true;
        lblMinIv.Location = new Point(77, 19);
        lblMinIv.Name = "lblMinIv";
        lblMinIv.Size = new Size(51, 20);
        lblMinIv.TabIndex = 6;
        lblMinIv.Text = "Min IV";
        // 
        // nAtkMax
        // 
        nAtkMax.Location = new Point(162, 75);
        nAtkMax.Margin = new Padding(3, 4, 3, 4);
        nAtkMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nAtkMax.Name = "nAtkMax";
        nAtkMax.Size = new Size(55, 27);
        nAtkMax.TabIndex = 13;
        // 
        // nDefMin
        // 
        nDefMin.Location = new Point(81, 107);
        nDefMin.Margin = new Padding(3, 4, 3, 4);
        nDefMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nDefMin.Name = "nDefMin";
        nDefMin.Size = new Size(55, 27);
        nDefMin.TabIndex = 14;
        // 
        // nSpaMin
        // 
        nSpaMin.Location = new Point(81, 140);
        nSpaMin.Margin = new Padding(3, 4, 3, 4);
        nSpaMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpaMin.Name = "nSpaMin";
        nSpaMin.Size = new Size(55, 27);
        nSpaMin.TabIndex = 16;
        // 
        // lblSpe
        // 
        lblSpe.AutoSize = true;
        lblSpe.Location = new Point(38, 208);
        lblSpe.Name = "lblSpe";
        lblSpe.Size = new Size(37, 20);
        lblSpe.TabIndex = 11;
        lblSpe.Text = "Spe:";
        // 
        // nSpdMin
        // 
        nSpdMin.Location = new Point(81, 173);
        nSpdMin.Margin = new Padding(3, 4, 3, 4);
        nSpdMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpdMin.Name = "nSpdMin";
        nSpdMin.Size = new Size(55, 27);
        nSpdMin.TabIndex = 18;
        // 
        // lblSpd
        // 
        lblSpd.AutoSize = true;
        lblSpd.Location = new Point(36, 176);
        lblSpd.Name = "lblSpd";
        lblSpd.Size = new Size(40, 20);
        lblSpd.TabIndex = 16;
        lblSpd.Text = "SpD:";
        // 
        // lblSpa
        // 
        lblSpa.AutoSize = true;
        lblSpa.Location = new Point(36, 144);
        lblSpa.Name = "lblSpa";
        lblSpa.Size = new Size(39, 20);
        lblSpa.TabIndex = 15;
        lblSpa.Text = "SpA:";
        // 
        // lblDef
        // 
        lblDef.AutoSize = true;
        lblDef.Location = new Point(36, 111);
        lblDef.Name = "lblDef";
        lblDef.Size = new Size(36, 20);
        lblDef.TabIndex = 14;
        lblDef.Text = "Def:";
        // 
        // lblAtk
        // 
        lblAtk.AutoSize = true;
        lblAtk.Location = new Point(36, 76);
        lblAtk.Name = "lblAtk";
        lblAtk.Size = new Size(34, 20);
        lblAtk.TabIndex = 13;
        lblAtk.Text = "Atk:";
        // 
        // lblHp
        // 
        lblHp.AutoSize = true;
        lblHp.Location = new Point(36, 44);
        lblHp.Name = "lblHp";
        lblHp.Size = new Size(31, 20);
        lblHp.TabIndex = 12;
        lblHp.Text = "HP:";
        // 
        // nSpeMin
        // 
        nSpeMin.Location = new Point(81, 206);
        nSpeMin.Margin = new Padding(3, 4, 3, 4);
        nSpeMin.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpeMin.Name = "nSpeMin";
        nSpeMin.Size = new Size(55, 27);
        nSpeMin.TabIndex = 20;
        // 
        // nDefMax
        // 
        nDefMax.Location = new Point(162, 107);
        nDefMax.Margin = new Padding(3, 4, 3, 4);
        nDefMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nDefMax.Name = "nDefMax";
        nDefMax.Size = new Size(55, 27);
        nDefMax.TabIndex = 15;
        // 
        // nSpaMax
        // 
        nSpaMax.Location = new Point(162, 140);
        nSpaMax.Margin = new Padding(3, 4, 3, 4);
        nSpaMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpaMax.Name = "nSpaMax";
        nSpaMax.Size = new Size(55, 27);
        nSpaMax.TabIndex = 17;
        // 
        // nSpdMax
        // 
        nSpdMax.Location = new Point(162, 173);
        nSpdMax.Margin = new Padding(3, 4, 3, 4);
        nSpdMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpdMax.Name = "nSpdMax";
        nSpdMax.Size = new Size(55, 27);
        nSpdMax.TabIndex = 19;
        // 
        // nSpeMax
        // 
        nSpeMax.Location = new Point(162, 206);
        nSpeMax.Margin = new Padding(3, 4, 3, 4);
        nSpeMax.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        nSpeMax.Name = "nSpeMax";
        nSpeMax.Size = new Size(55, 27);
        nSpeMax.TabIndex = 21;
        // 
        // dataGrid
        // 
        dataGrid.AllowUserToAddRows = false;
        dataGrid.AllowUserToDeleteRows = false;
        dataGrid.AllowUserToResizeRows = false;
        dataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGrid.Location = new Point(11, 264);
        dataGrid.Margin = new Padding(3, 4, 3, 4);
        dataGrid.Name = "dataGrid";
        dataGrid.ReadOnly = true;
        dataGrid.RowHeadersVisible = false;
        dataGrid.RowHeadersWidth = 51;
        dataGrid.RowTemplate.Height = 29;
        dataGrid.Size = new Size(1466, 541);
        dataGrid.TabIndex = 34;
        dataGrid.MouseUp += dataGrid_MouseUp;
        // 
        // grpGameInfo
        // 
        grpGameInfo.Controls.Add(lblFound);
        grpGameInfo.Controls.Add(txtSID);
        grpGameInfo.Controls.Add(lblSID);
        grpGameInfo.Controls.Add(txtTID);
        grpGameInfo.Controls.Add(lblTID);
        grpGameInfo.Controls.Add(lblGame);
        grpGameInfo.Controls.Add(cmbProgress);
        grpGameInfo.Controls.Add(lblProgress);
        grpGameInfo.Controls.Add(cmbGame);
        grpGameInfo.Location = new Point(353, 12);
        grpGameInfo.Margin = new Padding(3, 4, 3, 4);
        grpGameInfo.Name = "grpGameInfo";
        grpGameInfo.Padding = new Padding(3, 4, 3, 4);
        grpGameInfo.Size = new Size(307, 245);
        grpGameInfo.TabIndex = 2;
        grpGameInfo.TabStop = false;
        grpGameInfo.Text = "Game Info";
        // 
        // txtSID
        // 
        txtSID.Location = new Point(101, 153);
        txtSID.Margin = new Padding(3, 4, 3, 4);
        txtSID.Name = "txtSID";
        txtSID.Size = new Size(174, 27);
        txtSID.TabIndex = 9;
        txtSID.KeyPress += txtID_KeyPress;
        // 
        // lblSID
        // 
        lblSID.AutoSize = true;
        lblSID.Location = new Point(21, 156);
        lblSID.Name = "lblSID";
        lblSID.Size = new Size(35, 20);
        lblSID.TabIndex = 10;
        lblSID.Text = "SID:";
        // 
        // txtTID
        // 
        txtTID.Location = new Point(101, 116);
        txtTID.Margin = new Padding(3, 4, 3, 4);
        txtTID.Name = "txtTID";
        txtTID.Size = new Size(174, 27);
        txtTID.TabIndex = 8;
        txtTID.KeyPress += txtID_KeyPress;
        // 
        // lblTID
        // 
        lblTID.AutoSize = true;
        lblTID.Location = new Point(21, 119);
        lblTID.Name = "lblTID";
        lblTID.Size = new Size(35, 20);
        lblTID.TabIndex = 8;
        lblTID.Text = "TID:";
        // 
        // lblGame
        // 
        lblGame.AutoSize = true;
        lblGame.Location = new Point(21, 45);
        lblGame.Name = "lblGame";
        lblGame.Size = new Size(51, 20);
        lblGame.TabIndex = 1;
        lblGame.Text = "Game:";
        // 
        // cmbGame
        // 
        cmbGame.FormattingEnabled = true;
        cmbGame.Items.AddRange(new object[] { "Scarlet", "Violet" });
        cmbGame.Location = new Point(101, 40);
        cmbGame.Margin = new Padding(3, 4, 3, 4);
        cmbGame.Name = "cmbGame";
        cmbGame.Size = new Size(174, 28);
        cmbGame.TabIndex = 6;
        // 
        // toolTip
        // 
        toolTip.AutoPopDelay = 10000;
        toolTip.InitialDelay = 500;
        toolTip.IsBalloon = true;
        toolTip.ReshowDelay = 100;
        toolTip.ShowAlways = true;
        toolTip.ToolTipTitle = "Show All Results";
        // 
        // contextMenuStrip
        // 
        contextMenuStrip.ImageScalingSize = new Size(20, 20);
        contextMenuStrip.Items.AddRange(new ToolStripItem[] { btnViewRewards, btnSaveAll, btnSave, btnSavePk9, btnToPkmEditor, btnSendToEditor, btnCopySeed });
        contextMenuStrip.Name = "contextMenuStrip";
        contextMenuStrip.Size = new Size(313, 200);
        // 
        // btnViewRewards
        // 
        btnViewRewards.Name = "btnViewRewards";
        btnViewRewards.Size = new Size(312, 24);
        btnViewRewards.Text = "View Rewards";
        btnViewRewards.Click += btnViewRewards_Click;
        // 
        // btnSaveAll
        // 
        btnSaveAll.Name = "btnSaveAll";
        btnSaveAll.Size = new Size(312, 24);
        btnSaveAll.Text = "Save All Results as TXT";
        btnSaveAll.Click += btnSaveAll_Click;
        // 
        // btnSave
        // 
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(312, 24);
        btnSave.Text = "Save Selected Results as TXT";
        btnSave.Click += btnSave_Click;
        // 
        // btnSavePk9
        // 
        btnSavePk9.Name = "btnSavePk9";
        btnSavePk9.Size = new Size(312, 24);
        btnSavePk9.Text = "Save Selected Result as PK9";
        btnSavePk9.Click += btnSavePk9_Click;
        // 
        // btnToPkmEditor
        // 
        btnToPkmEditor.Name = "btnToPkmEditor";
        btnToPkmEditor.Size = new Size(312, 24);
        btnToPkmEditor.Text = "Send Selected Result to PKM Editor";
        btnToPkmEditor.Click += btnToPkmEditor_Click;
        // 
        // btnSendToEditor
        // 
        btnSendToEditor.Name = "btnSendToEditor";
        btnSendToEditor.Size = new Size(312, 24);
        btnSendToEditor.Text = "Send Selected Result to Raid Editor";
        btnSendToEditor.Click += btnSendToEditor_Click;
        // 
        // btnCopySeed
        // 
        btnCopySeed.Name = "btnCopySeed";
        btnCopySeed.Size = new Size(312, 24);
        btnCopySeed.Text = "Copy Seed";
        btnCopySeed.Click += btnCopySeed_Click;
        // 
        // CalculatorForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1483, 811);
        Controls.Add(grpGameInfo);
        Controls.Add(dataGrid);
        Controls.Add(grpFilters);
        Controls.Add(grpRaidDetails);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(3, 4, 3, 4);
        Name = "CalculatorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = " Raid Calculator";
        FormClosing += Form_FormClosing;
        grpRaidDetails.ResumeLayout(false);
        grpRaidDetails.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFrames).EndInit();
        grpFilters.ResumeLayout(false);
        grpFilters.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numScaleMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)numScaleMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nHpMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)nHpMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nAtkMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nAtkMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)nDefMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpaMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpdMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpeMin).EndInit();
        ((System.ComponentModel.ISupportInitialize)nDefMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpaMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpdMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)nSpeMax).EndInit();
        ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
        grpGameInfo.ResumeLayout(false);
        grpGameInfo.PerformLayout();
        contextMenuStrip.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
    private GroupBox grpRaidDetails;
    private TextBox txtSeed;
    private Label lblSeed;
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
    private ComboBox cmbTeraType;
    private ComboBox cmbSpecies;
    private ComboBox cmbStars;
    private Button btnReset;
    private Label lblProgress;
    private NumericUpDown numFrames;
    private ComboBox cmbNature;
    private GroupBox grpGameInfo;
    private Label lblGame;
    private Label lblSID;
    private Label lblTID;
    private ComboBox cmbEC;
    private Label lblEC;
    public CheckBox showresults;
    private Label label1;
    private NumericUpDown numScaleMax;
    private NumericUpDown numScaleMin;
    private Label lblScale;
    private ToolTip toolTip;
    private ContextMenuStrip contextMenuStrip;
    private ToolStripMenuItem btnSaveAll;
    private ToolStripMenuItem btnSave;
    private ToolStripMenuItem btnToPkmEditor;
    private ToolStripMenuItem btnSendToEditor;
    private ToolStripMenuItem btnSavePk9;
    public TextBox txtSID;
    public TextBox txtTID;
    public ComboBox cmbContent;
    public ComboBox cmbProgress;
    public ComboBox cmbGame;
    private ToolStripMenuItem btnViewRewards;
    private Label lblFound;
    private Label lblMap;
    private ComboBox cmbMap;
    private ToolStripMenuItem btnCopySeed;
}