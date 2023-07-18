namespace TeraFinder.Plugins;

partial class EditorForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
        cmbDens = new ComboBox();
        GrpRaidInfo = new GroupBox();
        lblContent = new Label();
        cmbContent = new ComboBox();
        chkActive = new CheckBox();
        txtSeed = new TextBox();
        lblSeed = new Label();
        chkLP = new CheckBox();
        grpPkmInfo = new GroupBox();
        lblIndex = new Label();
        grpMoves = new GroupBox();
        txtMove4 = new TextBox();
        txtMove2 = new TextBox();
        txtMove3 = new TextBox();
        txtMove1 = new TextBox();
        lblLevel = new Label();
        btnRewards = new Button();
        lblStarSymbols = new Label();
        pictureBox = new PictureBox();
        lblScale = new Label();
        lblSpecies = new Label();
        txtSpD = new TextBox();
        lblGender = new Label();
        txtSpe = new TextBox();
        lblShiny = new Label();
        txtSpA = new TextBox();
        lblTera = new Label();
        txtDef = new TextBox();
        lblNature = new Label();
        lblSpe = new Label();
        lblAbility = new Label();
        lblSpD = new Label();
        lblDef = new Label();
        lblAtk = new Label();
        lblSpA = new Label();
        lblHP = new Label();
        txtAtk = new TextBox();
        txtScale = new TextBox();
        txtHP = new TextBox();
        btnOpenCalculator = new Button();
        btnDx = new Button();
        btnSx = new Button();
        btnOpenRewardCalculator = new Button();
        imgMap = new PictureBox();
        grpMap = new GroupBox();
        menuStrip1 = new MenuStrip();
        toolsToolStripMenuItem = new ToolStripMenuItem();
        oneChickToolStripMenuItem = new ToolStripMenuItem();
        GrpRaidInfo.SuspendLayout();
        grpPkmInfo.SuspendLayout();
        grpMoves.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)imgMap).BeginInit();
        grpMap.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // cmbDens
        // 
        cmbDens.FormattingEnabled = true;
        cmbDens.Location = new Point(37, 26);
        cmbDens.Name = "cmbDens";
        cmbDens.Size = new Size(277, 25);
        cmbDens.TabIndex = 1;
        cmbDens.SelectedIndexChanged += cmbDens_IndexChanged;
        cmbDens.KeyPress += cmbDens_KeyPress;
        // 
        // GrpRaidInfo
        // 
        GrpRaidInfo.Controls.Add(lblContent);
        GrpRaidInfo.Controls.Add(cmbContent);
        GrpRaidInfo.Controls.Add(chkActive);
        GrpRaidInfo.Controls.Add(txtSeed);
        GrpRaidInfo.Controls.Add(lblSeed);
        GrpRaidInfo.Controls.Add(chkLP);
        GrpRaidInfo.Location = new Point(11, 57);
        GrpRaidInfo.Name = "GrpRaidInfo";
        GrpRaidInfo.Size = new Size(341, 98);
        GrpRaidInfo.TabIndex = 2;
        GrpRaidInfo.TabStop = false;
        GrpRaidInfo.Text = "Raid Info";
        // 
        // lblContent
        // 
        lblContent.AutoSize = true;
        lblContent.Location = new Point(6, 68);
        lblContent.Name = "lblContent";
        lblContent.Size = new Size(56, 17);
        lblContent.TabIndex = 7;
        lblContent.Text = "Content:";
        // 
        // cmbContent
        // 
        cmbContent.FormattingEnabled = true;
        cmbContent.Items.AddRange(new object[] { "Standard", "Black", "Event", "Event-Mighty" });
        cmbContent.Location = new Point(80, 65);
        cmbContent.Name = "cmbContent";
        cmbContent.Size = new Size(115, 25);
        cmbContent.TabIndex = 6;
        cmbContent.SelectedIndexChanged += cmbContent_IndexChanged;
        // 
        // chkActive
        // 
        chkActive.AutoSize = true;
        chkActive.Location = new Point(219, 30);
        chkActive.Name = "chkActive";
        chkActive.Size = new Size(88, 21);
        chkActive.TabIndex = 3;
        chkActive.Text = "Den Active";
        chkActive.UseVisualStyleBackColor = true;
        chkActive.CheckedChanged += chkActive_CheckedChanged;
        // 
        // txtSeed
        // 
        txtSeed.Location = new Point(80, 30);
        txtSeed.MaxLength = 8;
        txtSeed.Name = "txtSeed";
        txtSeed.Size = new Size(115, 23);
        txtSeed.TabIndex = 3;
        txtSeed.TextChanged += txtSeed_TextChanged;
        txtSeed.KeyPress += txtSeed_KeyPress;
        // 
        // lblSeed
        // 
        lblSeed.AutoSize = true;
        lblSeed.Location = new Point(10, 31);
        lblSeed.Name = "lblSeed";
        lblSeed.Size = new Size(40, 17);
        lblSeed.TabIndex = 4;
        lblSeed.Text = "Seed:";
        // 
        // chkLP
        // 
        chkLP.AutoSize = true;
        chkLP.Location = new Point(219, 67);
        chkLP.Name = "chkLP";
        chkLP.Size = new Size(103, 21);
        chkLP.TabIndex = 5;
        chkLP.Text = "LP Harvested";
        chkLP.UseVisualStyleBackColor = true;
        chkLP.CheckedChanged += chkLP_CheckedChanged;
        // 
        // grpPkmInfo
        // 
        grpPkmInfo.Controls.Add(lblIndex);
        grpPkmInfo.Controls.Add(grpMoves);
        grpPkmInfo.Controls.Add(lblLevel);
        grpPkmInfo.Controls.Add(btnRewards);
        grpPkmInfo.Controls.Add(lblStarSymbols);
        grpPkmInfo.Controls.Add(pictureBox);
        grpPkmInfo.Controls.Add(lblScale);
        grpPkmInfo.Controls.Add(lblSpecies);
        grpPkmInfo.Controls.Add(txtSpD);
        grpPkmInfo.Controls.Add(lblGender);
        grpPkmInfo.Controls.Add(txtSpe);
        grpPkmInfo.Controls.Add(lblShiny);
        grpPkmInfo.Controls.Add(txtSpA);
        grpPkmInfo.Controls.Add(lblTera);
        grpPkmInfo.Controls.Add(txtDef);
        grpPkmInfo.Controls.Add(lblNature);
        grpPkmInfo.Controls.Add(lblSpe);
        grpPkmInfo.Controls.Add(lblAbility);
        grpPkmInfo.Controls.Add(lblSpD);
        grpPkmInfo.Controls.Add(lblDef);
        grpPkmInfo.Controls.Add(lblAtk);
        grpPkmInfo.Controls.Add(lblSpA);
        grpPkmInfo.Controls.Add(lblHP);
        grpPkmInfo.Controls.Add(txtAtk);
        grpPkmInfo.Controls.Add(txtScale);
        grpPkmInfo.Controls.Add(txtHP);
        grpPkmInfo.Location = new Point(7, 161);
        grpPkmInfo.Name = "grpPkmInfo";
        grpPkmInfo.Size = new Size(341, 324);
        grpPkmInfo.TabIndex = 3;
        grpPkmInfo.TabStop = false;
        grpPkmInfo.Text = "Pokémon Info";
        // 
        // lblIndex
        // 
        lblIndex.AutoSize = true;
        lblIndex.Location = new Point(114, 200);
        lblIndex.Name = "lblIndex";
        lblIndex.Size = new Size(78, 17);
        lblIndex.TabIndex = 24;
        lblIndex.Text = "Event Index:";
        // 
        // grpMoves
        // 
        grpMoves.Controls.Add(txtMove4);
        grpMoves.Controls.Add(txtMove2);
        grpMoves.Controls.Add(txtMove3);
        grpMoves.Controls.Add(txtMove1);
        grpMoves.Location = new Point(4, 222);
        grpMoves.Name = "grpMoves";
        grpMoves.Size = new Size(332, 94);
        grpMoves.TabIndex = 22;
        grpMoves.TabStop = false;
        grpMoves.Text = "Moves";
        // 
        // txtMove4
        // 
        txtMove4.Enabled = false;
        txtMove4.Location = new Point(180, 60);
        txtMove4.Name = "txtMove4";
        txtMove4.Size = new Size(126, 23);
        txtMove4.TabIndex = 22;
        txtMove4.Text = "None";
        txtMove4.TextAlign = HorizontalAlignment.Center;
        // 
        // txtMove2
        // 
        txtMove2.Enabled = false;
        txtMove2.Location = new Point(180, 24);
        txtMove2.Name = "txtMove2";
        txtMove2.Size = new Size(126, 23);
        txtMove2.TabIndex = 23;
        txtMove2.Text = "None";
        txtMove2.TextAlign = HorizontalAlignment.Center;
        // 
        // txtMove3
        // 
        txtMove3.Enabled = false;
        txtMove3.Location = new Point(27, 60);
        txtMove3.Name = "txtMove3";
        txtMove3.Size = new Size(126, 23);
        txtMove3.TabIndex = 24;
        txtMove3.Text = "None";
        txtMove3.TextAlign = HorizontalAlignment.Center;
        // 
        // txtMove1
        // 
        txtMove1.Enabled = false;
        txtMove1.Location = new Point(29, 24);
        txtMove1.Name = "txtMove1";
        txtMove1.Size = new Size(126, 23);
        txtMove1.TabIndex = 25;
        txtMove1.Text = "None";
        txtMove1.TextAlign = HorizontalAlignment.Center;
        // 
        // lblLevel
        // 
        lblLevel.AutoSize = true;
        lblLevel.Location = new Point(268, 72);
        lblLevel.Name = "lblLevel";
        lblLevel.Size = new Size(26, 17);
        lblLevel.TabIndex = 21;
        lblLevel.Text = "Lvl.";
        // 
        // btnRewards
        // 
        btnRewards.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
        btnRewards.Location = new Point(256, 179);
        btnRewards.Name = "btnRewards";
        btnRewards.Size = new Size(66, 26);
        btnRewards.TabIndex = 20;
        btnRewards.Text = "Rewards";
        btnRewards.UseVisualStyleBackColor = true;
        btnRewards.Visible = false;
        btnRewards.Click += btnRewards_Click;
        // 
        // lblStarSymbols
        // 
        lblStarSymbols.AutoSize = true;
        lblStarSymbols.Location = new Point(256, 159);
        lblStarSymbols.Name = "lblStarSymbols";
        lblStarSymbols.Size = new Size(68, 17);
        lblStarSymbols.TabIndex = 19;
        lblStarSymbols.Text = "☆☆☆☆☆☆";
        // 
        // pictureBox
        // 
        pictureBox.BackColor = Color.Transparent;
        pictureBox.BackgroundImage = Properties.Resources._000;
        pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
        pictureBox.BorderStyle = BorderStyle.FixedSingle;
        pictureBox.Location = new Point(256, 93);
        pictureBox.Name = "pictureBox";
        pictureBox.Size = new Size(66, 64);
        pictureBox.TabIndex = 12;
        pictureBox.TabStop = false;
        // 
        // lblScale
        // 
        lblScale.AutoSize = true;
        lblScale.Location = new Point(10, 200);
        lblScale.Name = "lblScale";
        lblScale.Size = new Size(41, 17);
        lblScale.TabIndex = 22;
        lblScale.Text = "Scale:";
        // 
        // lblSpecies
        // 
        lblSpecies.AutoSize = true;
        lblSpecies.Location = new Point(114, 20);
        lblSpecies.Name = "lblSpecies";
        lblSpecies.Size = new Size(55, 17);
        lblSpecies.TabIndex = 18;
        lblSpecies.Text = "Species:";
        // 
        // txtSpD
        // 
        txtSpD.Enabled = false;
        txtSpD.Location = new Point(57, 137);
        txtSpD.Name = "txtSpD";
        txtSpD.Size = new Size(28, 23);
        txtSpD.TabIndex = 4;
        // 
        // lblGender
        // 
        lblGender.AutoSize = true;
        lblGender.Location = new Point(114, 169);
        lblGender.Name = "lblGender";
        lblGender.Size = new Size(54, 17);
        lblGender.TabIndex = 13;
        lblGender.Text = "Gender:";
        // 
        // txtSpe
        // 
        txtSpe.Enabled = false;
        txtSpe.Location = new Point(57, 167);
        txtSpe.Name = "txtSpe";
        txtSpe.Size = new Size(28, 23);
        txtSpe.TabIndex = 5;
        // 
        // lblShiny
        // 
        lblShiny.AutoSize = true;
        lblShiny.Location = new Point(114, 139);
        lblShiny.Name = "lblShiny";
        lblShiny.Size = new Size(45, 17);
        lblShiny.TabIndex = 16;
        lblShiny.Text = "Shiny: ";
        // 
        // txtSpA
        // 
        txtSpA.Enabled = false;
        txtSpA.Location = new Point(57, 106);
        txtSpA.Name = "txtSpA";
        txtSpA.Size = new Size(28, 23);
        txtSpA.TabIndex = 3;
        // 
        // lblTera
        // 
        lblTera.AutoSize = true;
        lblTera.Location = new Point(114, 79);
        lblTera.Name = "lblTera";
        lblTera.Size = new Size(65, 17);
        lblTera.TabIndex = 17;
        lblTera.Text = "TeraType:";
        // 
        // txtDef
        // 
        txtDef.Enabled = false;
        txtDef.Location = new Point(57, 77);
        txtDef.Name = "txtDef";
        txtDef.Size = new Size(28, 23);
        txtDef.TabIndex = 2;
        // 
        // lblNature
        // 
        lblNature.AutoSize = true;
        lblNature.Location = new Point(114, 110);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(55, 17);
        lblNature.TabIndex = 14;
        lblNature.Text = "Nature: ";
        // 
        // lblSpe
        // 
        lblSpe.AutoSize = true;
        lblSpe.Location = new Point(10, 169);
        lblSpe.Name = "lblSpe";
        lblSpe.Size = new Size(33, 17);
        lblSpe.TabIndex = 11;
        lblSpe.Text = "Spe:";
        // 
        // lblAbility
        // 
        lblAbility.AutoSize = true;
        lblAbility.Location = new Point(114, 50);
        lblAbility.Name = "lblAbility";
        lblAbility.Size = new Size(50, 17);
        lblAbility.TabIndex = 15;
        lblAbility.Text = "Ability: ";
        // 
        // lblSpD
        // 
        lblSpD.AutoSize = true;
        lblSpD.Location = new Point(10, 139);
        lblSpD.Name = "lblSpD";
        lblSpD.Size = new Size(35, 17);
        lblSpD.TabIndex = 10;
        lblSpD.Text = "SpD:";
        // 
        // lblDef
        // 
        lblDef.AutoSize = true;
        lblDef.Location = new Point(10, 79);
        lblDef.Name = "lblDef";
        lblDef.Size = new Size(31, 17);
        lblDef.TabIndex = 8;
        lblDef.Text = "Def:";
        // 
        // lblAtk
        // 
        lblAtk.AutoSize = true;
        lblAtk.Location = new Point(10, 50);
        lblAtk.Name = "lblAtk";
        lblAtk.Size = new Size(30, 17);
        lblAtk.TabIndex = 7;
        lblAtk.Text = "Atk:";
        // 
        // lblSpA
        // 
        lblSpA.AutoSize = true;
        lblSpA.Location = new Point(10, 110);
        lblSpA.Name = "lblSpA";
        lblSpA.Size = new Size(34, 17);
        lblSpA.TabIndex = 9;
        lblSpA.Text = "SpA:";
        // 
        // lblHP
        // 
        lblHP.AutoSize = true;
        lblHP.Location = new Point(10, 20);
        lblHP.Name = "lblHP";
        lblHP.Size = new Size(27, 17);
        lblHP.TabIndex = 6;
        lblHP.Text = "HP:";
        // 
        // txtAtk
        // 
        txtAtk.Enabled = false;
        txtAtk.Location = new Point(57, 48);
        txtAtk.Name = "txtAtk";
        txtAtk.Size = new Size(28, 23);
        txtAtk.TabIndex = 1;
        // 
        // txtScale
        // 
        txtScale.Enabled = false;
        txtScale.Location = new Point(57, 196);
        txtScale.Name = "txtScale";
        txtScale.Size = new Size(28, 23);
        txtScale.TabIndex = 23;
        // 
        // txtHP
        // 
        txtHP.Enabled = false;
        txtHP.Location = new Point(57, 18);
        txtHP.Name = "txtHP";
        txtHP.Size = new Size(28, 23);
        txtHP.TabIndex = 0;
        // 
        // btnOpenCalculator
        // 
        btnOpenCalculator.Location = new Point(7, 484);
        btnOpenCalculator.Name = "btnOpenCalculator";
        btnOpenCalculator.Size = new Size(168, 44);
        btnOpenCalculator.TabIndex = 4;
        btnOpenCalculator.Text = "Raid Calculator";
        btnOpenCalculator.UseVisualStyleBackColor = true;
        btnOpenCalculator.Click += btnOpenCalculator_Click;
        // 
        // btnDx
        // 
        btnDx.Location = new Point(321, 24);
        btnDx.Name = "btnDx";
        btnDx.Size = new Size(27, 27);
        btnDx.TabIndex = 5;
        btnDx.Text = "ᐅ";
        btnDx.UseVisualStyleBackColor = true;
        btnDx.Click += btnDx_Click;
        // 
        // btnSx
        // 
        btnSx.Location = new Point(7, 24);
        btnSx.Name = "btnSx";
        btnSx.Size = new Size(27, 27);
        btnSx.TabIndex = 20;
        btnSx.Text = "ᐊ";
        btnSx.UseVisualStyleBackColor = true;
        btnSx.Click += btnSx_Click;
        // 
        // btnOpenRewardCalculator
        // 
        btnOpenRewardCalculator.Location = new Point(180, 483);
        btnOpenRewardCalculator.Name = "btnOpenRewardCalculator";
        btnOpenRewardCalculator.Size = new Size(168, 44);
        btnOpenRewardCalculator.TabIndex = 21;
        btnOpenRewardCalculator.Text = "Reward Calculator";
        btnOpenRewardCalculator.UseVisualStyleBackColor = true;
        btnOpenRewardCalculator.Click += btnOpenRewardCalculator_Click;
        // 
        // imgMap
        // 
        imgMap.BackgroundImage = Properties.Resources.world;
        imgMap.BackgroundImageLayout = ImageLayout.Stretch;
        imgMap.BorderStyle = BorderStyle.Fixed3D;
        imgMap.Location = new Point(4, 0);
        imgMap.Name = "imgMap";
        imgMap.Size = new Size(501, 484);
        imgMap.TabIndex = 22;
        imgMap.TabStop = false;
        // 
        // grpMap
        // 
        grpMap.Controls.Add(imgMap);
        grpMap.Location = new Point(354, 26);
        grpMap.Name = "grpMap";
        grpMap.Size = new Size(511, 497);
        grpMap.TabIndex = 23;
        grpMap.TabStop = false;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(871, 25);
        menuStrip1.TabIndex = 24;
        menuStrip1.Text = "menuStrip1";
        // 
        // toolsToolStripMenuItem
        // 
        toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { oneChickToolStripMenuItem });
        toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        toolsToolStripMenuItem.Size = new Size(52, 21);
        toolsToolStripMenuItem.Text = "Tools";
        // 
        // oneChickToolStripMenuItem
        // 
        oneChickToolStripMenuItem.Name = "oneChickToolStripMenuItem";
        oneChickToolStripMenuItem.Size = new Size(212, 22);
        oneChickToolStripMenuItem.Text = "One click random shiny";
        oneChickToolStripMenuItem.Click += oneChickToolStripMenuItem_Click;
        // 
        // EditorForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(871, 535);
        Controls.Add(grpMap);
        Controls.Add(btnOpenRewardCalculator);
        Controls.Add(btnDx);
        Controls.Add(btnSx);
        Controls.Add(btnOpenCalculator);
        Controls.Add(grpPkmInfo);
        Controls.Add(GrpRaidInfo);
        Controls.Add(cmbDens);
        Controls.Add(menuStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip1;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tera Raid Editor";
        GrpRaidInfo.ResumeLayout(false);
        GrpRaidInfo.PerformLayout();
        grpPkmInfo.ResumeLayout(false);
        grpPkmInfo.PerformLayout();
        grpMoves.ResumeLayout(false);
        grpMoves.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)imgMap).EndInit();
        grpMap.ResumeLayout(false);
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private GroupBox GrpRaidInfo;
    private Label lblSeed;
    private GroupBox grpPkmInfo;
    private Label lblGender;
    private PictureBox pictureBox;
    private Label lblSpe;
    private Label lblSpD;
    private Label lblSpA;
    private Label lblDef;
    private Label lblAtk;
    private Label lblHP;
    private TextBox txtSpe;
    private TextBox txtSpD;
    private TextBox txtSpA;
    private TextBox txtDef;
    private TextBox txtAtk;
    private TextBox txtHP;
    private Label lblShiny;
    private Label lblAbility;
    private Label lblNature;
    private Button btnOpenCalculator;
    private Label lblTera;
    private Label lblSpecies;
    private Label lblStarSymbols;
    public CheckBox chkLP;
    public TextBox txtSeed;
    public CheckBox chkActive;
    private Label lblContent;
    public ComboBox cmbContent;
    private Button btnDx;
    private Button btnSx;
    private Button btnRewards;
    private Button btnOpenRewardCalculator;
    private GroupBox grpMoves;
    private TextBox txtMove4;
    private TextBox txtMove2;
    private TextBox txtMove3;
    private TextBox txtMove1;
    private Label lblLevel;
    private PictureBox imgMap;
    public ComboBox cmbDens;
    private TextBox txtScale;
    private Label lblScale;
    private GroupBox grpMap;
    private Label lblIndex;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem oneChickToolStripMenuItem;
}