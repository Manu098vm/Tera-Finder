namespace TeraFinder
{
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
            this.cmbDens = new System.Windows.Forms.ComboBox();
            this.GrpRaidInfo = new System.Windows.Forms.GroupBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.cmbContent = new System.Windows.Forms.ComboBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.lblSeed = new System.Windows.Forms.Label();
            this.chkLP = new System.Windows.Forms.CheckBox();
            this.grpPkmInfo = new System.Windows.Forms.GroupBox();
            this.grpMoves = new System.Windows.Forms.GroupBox();
            this.txtMove4 = new System.Windows.Forms.TextBox();
            this.txtMove2 = new System.Windows.Forms.TextBox();
            this.txtMove3 = new System.Windows.Forms.TextBox();
            this.txtMove1 = new System.Windows.Forms.TextBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.btnRewards = new System.Windows.Forms.Button();
            this.lblStarSymbols = new System.Windows.Forms.Label();
            this.lblSpecies = new System.Windows.Forms.Label();
            this.lblTera = new System.Windows.Forms.Label();
            this.lblShiny = new System.Windows.Forms.Label();
            this.lblAbility = new System.Windows.Forms.Label();
            this.lblNature = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblSpe = new System.Windows.Forms.Label();
            this.lblSpD = new System.Windows.Forms.Label();
            this.lblSpA = new System.Windows.Forms.Label();
            this.lblDef = new System.Windows.Forms.Label();
            this.lblAtk = new System.Windows.Forms.Label();
            this.lblHP = new System.Windows.Forms.Label();
            this.txtSpe = new System.Windows.Forms.TextBox();
            this.txtSpD = new System.Windows.Forms.TextBox();
            this.txtSpA = new System.Windows.Forms.TextBox();
            this.txtDef = new System.Windows.Forms.TextBox();
            this.txtAtk = new System.Windows.Forms.TextBox();
            this.txtHP = new System.Windows.Forms.TextBox();
            this.btnOpenCalculator = new System.Windows.Forms.Button();
            this.btnDx = new System.Windows.Forms.Button();
            this.btnSx = new System.Windows.Forms.Button();
            this.btnOpenRewardCalculator = new System.Windows.Forms.Button();
            this.imgMap = new System.Windows.Forms.PictureBox();
            this.GrpRaidInfo.SuspendLayout();
            this.grpPkmInfo.SuspendLayout();
            this.grpMoves.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMap)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDens
            // 
            this.cmbDens.FormattingEnabled = true;
            this.cmbDens.Location = new System.Drawing.Point(45, 14);
            this.cmbDens.Name = "cmbDens";
            this.cmbDens.Size = new System.Drawing.Size(316, 28);
            this.cmbDens.TabIndex = 1;
            this.cmbDens.SelectedIndexChanged += new System.EventHandler(this.cmbDens_IndexChanged);
            this.cmbDens.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbDens_KeyPress);
            // 
            // GrpRaidInfo
            // 
            this.GrpRaidInfo.Controls.Add(this.lblContent);
            this.GrpRaidInfo.Controls.Add(this.cmbContent);
            this.GrpRaidInfo.Controls.Add(this.chkActive);
            this.GrpRaidInfo.Controls.Add(this.txtSeed);
            this.GrpRaidInfo.Controls.Add(this.lblSeed);
            this.GrpRaidInfo.Controls.Add(this.chkLP);
            this.GrpRaidInfo.Location = new System.Drawing.Point(8, 56);
            this.GrpRaidInfo.Name = "GrpRaidInfo";
            this.GrpRaidInfo.Size = new System.Drawing.Size(390, 123);
            this.GrpRaidInfo.TabIndex = 2;
            this.GrpRaidInfo.TabStop = false;
            this.GrpRaidInfo.Text = "Raid Info";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(7, 80);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(64, 20);
            this.lblContent.TabIndex = 7;
            this.lblContent.Text = "Content:";
            // 
            // cmbContent
            // 
            this.cmbContent.FormattingEnabled = true;
            this.cmbContent.Items.AddRange(new object[] {
            "Standard",
            "Black",
            "Event",
            "Event-Mighty"});
            this.cmbContent.Location = new System.Drawing.Point(91, 77);
            this.cmbContent.Name = "cmbContent";
            this.cmbContent.Size = new System.Drawing.Size(131, 28);
            this.cmbContent.TabIndex = 6;
            this.cmbContent.SelectedIndexChanged += new System.EventHandler(this.cmbContent_IndexChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(250, 35);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(103, 24);
            this.chkActive.TabIndex = 3;
            this.chkActive.Text = "Den Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(91, 35);
            this.txtSeed.MaxLength = 8;
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(131, 27);
            this.txtSeed.TabIndex = 3;
            this.txtSeed.TextChanged += new System.EventHandler(this.txtSeed_TextChanged);
            this.txtSeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeed_KeyPress);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(11, 38);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(45, 20);
            this.lblSeed.TabIndex = 4;
            this.lblSeed.Text = "Seed:";
            // 
            // chkLP
            // 
            this.chkLP.AutoSize = true;
            this.chkLP.Location = new System.Drawing.Point(250, 79);
            this.chkLP.Name = "chkLP";
            this.chkLP.Size = new System.Drawing.Size(117, 24);
            this.chkLP.TabIndex = 5;
            this.chkLP.Text = "LP Harvested";
            this.chkLP.UseVisualStyleBackColor = true;
            this.chkLP.CheckedChanged += new System.EventHandler(this.chkLP_CheckedChanged);
            // 
            // grpPkmInfo
            // 
            this.grpPkmInfo.Controls.Add(this.grpMoves);
            this.grpPkmInfo.Controls.Add(this.lblLevel);
            this.grpPkmInfo.Controls.Add(this.btnRewards);
            this.grpPkmInfo.Controls.Add(this.lblStarSymbols);
            this.grpPkmInfo.Controls.Add(this.lblSpecies);
            this.grpPkmInfo.Controls.Add(this.lblTera);
            this.grpPkmInfo.Controls.Add(this.lblShiny);
            this.grpPkmInfo.Controls.Add(this.lblAbility);
            this.grpPkmInfo.Controls.Add(this.lblNature);
            this.grpPkmInfo.Controls.Add(this.lblGender);
            this.grpPkmInfo.Controls.Add(this.pictureBox);
            this.grpPkmInfo.Controls.Add(this.lblSpe);
            this.grpPkmInfo.Controls.Add(this.lblSpD);
            this.grpPkmInfo.Controls.Add(this.lblSpA);
            this.grpPkmInfo.Controls.Add(this.lblDef);
            this.grpPkmInfo.Controls.Add(this.lblAtk);
            this.grpPkmInfo.Controls.Add(this.lblHP);
            this.grpPkmInfo.Controls.Add(this.txtSpe);
            this.grpPkmInfo.Controls.Add(this.txtSpD);
            this.grpPkmInfo.Controls.Add(this.txtSpA);
            this.grpPkmInfo.Controls.Add(this.txtDef);
            this.grpPkmInfo.Controls.Add(this.txtAtk);
            this.grpPkmInfo.Controls.Add(this.txtHP);
            this.grpPkmInfo.Location = new System.Drawing.Point(8, 185);
            this.grpPkmInfo.Name = "grpPkmInfo";
            this.grpPkmInfo.Size = new System.Drawing.Size(390, 339);
            this.grpPkmInfo.TabIndex = 3;
            this.grpPkmInfo.TabStop = false;
            this.grpPkmInfo.Text = "Pokémon Info";
            // 
            // grpMoves
            // 
            this.grpMoves.Controls.Add(this.txtMove4);
            this.grpMoves.Controls.Add(this.txtMove2);
            this.grpMoves.Controls.Add(this.txtMove3);
            this.grpMoves.Controls.Add(this.txtMove1);
            this.grpMoves.Location = new System.Drawing.Point(4, 220);
            this.grpMoves.Name = "grpMoves";
            this.grpMoves.Size = new System.Drawing.Size(380, 110);
            this.grpMoves.TabIndex = 22;
            this.grpMoves.TabStop = false;
            this.grpMoves.Text = "Moves";
            // 
            // txtMove4
            // 
            this.txtMove4.Enabled = false;
            this.txtMove4.Location = new System.Drawing.Point(206, 70);
            this.txtMove4.Name = "txtMove4";
            this.txtMove4.Size = new System.Drawing.Size(143, 27);
            this.txtMove4.TabIndex = 22;
            this.txtMove4.Text = "None";
            this.txtMove4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMove2
            // 
            this.txtMove2.Enabled = false;
            this.txtMove2.Location = new System.Drawing.Point(206, 28);
            this.txtMove2.Name = "txtMove2";
            this.txtMove2.Size = new System.Drawing.Size(143, 27);
            this.txtMove2.TabIndex = 23;
            this.txtMove2.Text = "None";
            this.txtMove2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMove3
            // 
            this.txtMove3.Enabled = false;
            this.txtMove3.Location = new System.Drawing.Point(31, 70);
            this.txtMove3.Name = "txtMove3";
            this.txtMove3.Size = new System.Drawing.Size(143, 27);
            this.txtMove3.TabIndex = 24;
            this.txtMove3.Text = "None";
            this.txtMove3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMove1
            // 
            this.txtMove1.Enabled = false;
            this.txtMove1.Location = new System.Drawing.Point(33, 28);
            this.txtMove1.Name = "txtMove1";
            this.txtMove1.Size = new System.Drawing.Size(143, 27);
            this.txtMove1.TabIndex = 25;
            this.txtMove1.Text = "None";
            this.txtMove1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(306, 84);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(29, 20);
            this.lblLevel.TabIndex = 21;
            this.lblLevel.Text = "Lvl.";
            // 
            // btnRewards
            // 
            this.btnRewards.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRewards.Location = new System.Drawing.Point(292, 19);
            this.btnRewards.Name = "btnRewards";
            this.btnRewards.Size = new System.Drawing.Size(75, 31);
            this.btnRewards.TabIndex = 20;
            this.btnRewards.Text = "Rewards";
            this.btnRewards.UseVisualStyleBackColor = true;
            this.btnRewards.Visible = false;
            this.btnRewards.Click += new System.EventHandler(this.btnRewards_Click);
            // 
            // lblStarSymbols
            // 
            this.lblStarSymbols.AutoSize = true;
            this.lblStarSymbols.Location = new System.Drawing.Point(293, 185);
            this.lblStarSymbols.Name = "lblStarSymbols";
            this.lblStarSymbols.Size = new System.Drawing.Size(74, 20);
            this.lblStarSymbols.TabIndex = 19;
            this.lblStarSymbols.Text = "☆☆☆☆☆";
            // 
            // lblSpecies
            // 
            this.lblSpecies.AutoSize = true;
            this.lblSpecies.Location = new System.Drawing.Point(107, 25);
            this.lblSpecies.Name = "lblSpecies";
            this.lblSpecies.Size = new System.Drawing.Size(62, 20);
            this.lblSpecies.TabIndex = 18;
            this.lblSpecies.Text = "Species:";
            // 
            // lblTera
            // 
            this.lblTera.AutoSize = true;
            this.lblTera.Location = new System.Drawing.Point(107, 91);
            this.lblTera.Name = "lblTera";
            this.lblTera.Size = new System.Drawing.Size(71, 20);
            this.lblTera.TabIndex = 17;
            this.lblTera.Text = "TeraType:";
            // 
            // lblShiny
            // 
            this.lblShiny.AutoSize = true;
            this.lblShiny.Location = new System.Drawing.Point(107, 157);
            this.lblShiny.Name = "lblShiny";
            this.lblShiny.Size = new System.Drawing.Size(51, 20);
            this.lblShiny.TabIndex = 16;
            this.lblShiny.Text = "Shiny: ";
            // 
            // lblAbility
            // 
            this.lblAbility.AutoSize = true;
            this.lblAbility.Location = new System.Drawing.Point(107, 59);
            this.lblAbility.Name = "lblAbility";
            this.lblAbility.Size = new System.Drawing.Size(59, 20);
            this.lblAbility.TabIndex = 15;
            this.lblAbility.Text = "Ability: ";
            // 
            // lblNature
            // 
            this.lblNature.AutoSize = true;
            this.lblNature.Location = new System.Drawing.Point(107, 124);
            this.lblNature.Name = "lblNature";
            this.lblNature.Size = new System.Drawing.Size(61, 20);
            this.lblNature.TabIndex = 14;
            this.lblNature.Text = "Nature: ";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(107, 189);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(60, 20);
            this.lblGender.TabIndex = 13;
            this.lblGender.Text = "Gender:";
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox.BackgroundImage = global::TeraFinder.Properties.Resources._000;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(292, 107);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(75, 75);
            this.pictureBox.TabIndex = 12;
            this.pictureBox.TabStop = false;
            // 
            // lblSpe
            // 
            this.lblSpe.AutoSize = true;
            this.lblSpe.Location = new System.Drawing.Point(9, 189);
            this.lblSpe.Name = "lblSpe";
            this.lblSpe.Size = new System.Drawing.Size(37, 20);
            this.lblSpe.TabIndex = 11;
            this.lblSpe.Text = "Spe:";
            // 
            // lblSpD
            // 
            this.lblSpD.AutoSize = true;
            this.lblSpD.Location = new System.Drawing.Point(6, 157);
            this.lblSpD.Name = "lblSpD";
            this.lblSpD.Size = new System.Drawing.Size(40, 20);
            this.lblSpD.TabIndex = 10;
            this.lblSpD.Text = "SpD:";
            // 
            // lblSpA
            // 
            this.lblSpA.AutoSize = true;
            this.lblSpA.Location = new System.Drawing.Point(7, 124);
            this.lblSpA.Name = "lblSpA";
            this.lblSpA.Size = new System.Drawing.Size(39, 20);
            this.lblSpA.TabIndex = 9;
            this.lblSpA.Text = "SpA:";
            // 
            // lblDef
            // 
            this.lblDef.AutoSize = true;
            this.lblDef.Location = new System.Drawing.Point(10, 91);
            this.lblDef.Name = "lblDef";
            this.lblDef.Size = new System.Drawing.Size(36, 20);
            this.lblDef.TabIndex = 8;
            this.lblDef.Text = "Def:";
            // 
            // lblAtk
            // 
            this.lblAtk.AutoSize = true;
            this.lblAtk.Location = new System.Drawing.Point(11, 59);
            this.lblAtk.Name = "lblAtk";
            this.lblAtk.Size = new System.Drawing.Size(34, 20);
            this.lblAtk.TabIndex = 7;
            this.lblAtk.Text = "Atk:";
            // 
            // lblHP
            // 
            this.lblHP.AutoSize = true;
            this.lblHP.Location = new System.Drawing.Point(15, 29);
            this.lblHP.Name = "lblHP";
            this.lblHP.Size = new System.Drawing.Size(31, 20);
            this.lblHP.TabIndex = 6;
            this.lblHP.Text = "HP:";
            // 
            // txtSpe
            // 
            this.txtSpe.Enabled = false;
            this.txtSpe.Location = new System.Drawing.Point(54, 187);
            this.txtSpe.Name = "txtSpe";
            this.txtSpe.Size = new System.Drawing.Size(31, 27);
            this.txtSpe.TabIndex = 5;
            // 
            // txtSpD
            // 
            this.txtSpD.Enabled = false;
            this.txtSpD.Location = new System.Drawing.Point(54, 155);
            this.txtSpD.Name = "txtSpD";
            this.txtSpD.Size = new System.Drawing.Size(31, 27);
            this.txtSpD.TabIndex = 4;
            // 
            // txtSpA
            // 
            this.txtSpA.Enabled = false;
            this.txtSpA.Location = new System.Drawing.Point(54, 121);
            this.txtSpA.Name = "txtSpA";
            this.txtSpA.Size = new System.Drawing.Size(31, 27);
            this.txtSpA.TabIndex = 3;
            // 
            // txtDef
            // 
            this.txtDef.Enabled = false;
            this.txtDef.Location = new System.Drawing.Point(54, 88);
            this.txtDef.Name = "txtDef";
            this.txtDef.Size = new System.Drawing.Size(31, 27);
            this.txtDef.TabIndex = 2;
            // 
            // txtAtk
            // 
            this.txtAtk.Enabled = false;
            this.txtAtk.Location = new System.Drawing.Point(54, 55);
            this.txtAtk.Name = "txtAtk";
            this.txtAtk.Size = new System.Drawing.Size(31, 27);
            this.txtAtk.TabIndex = 1;
            // 
            // txtHP
            // 
            this.txtHP.Enabled = false;
            this.txtHP.Location = new System.Drawing.Point(54, 21);
            this.txtHP.Name = "txtHP";
            this.txtHP.Size = new System.Drawing.Size(31, 27);
            this.txtHP.TabIndex = 0;
            // 
            // btnOpenCalculator
            // 
            this.btnOpenCalculator.Location = new System.Drawing.Point(8, 530);
            this.btnOpenCalculator.Name = "btnOpenCalculator";
            this.btnOpenCalculator.Size = new System.Drawing.Size(192, 52);
            this.btnOpenCalculator.TabIndex = 4;
            this.btnOpenCalculator.Text = "Raid Calculator";
            this.btnOpenCalculator.UseVisualStyleBackColor = true;
            this.btnOpenCalculator.Click += new System.EventHandler(this.btnOpenCalculator_Click);
            // 
            // btnDx
            // 
            this.btnDx.Location = new System.Drawing.Point(367, 12);
            this.btnDx.Name = "btnDx";
            this.btnDx.Size = new System.Drawing.Size(31, 31);
            this.btnDx.TabIndex = 5;
            this.btnDx.Text = "ᐅ";
            this.btnDx.UseVisualStyleBackColor = true;
            this.btnDx.Click += new System.EventHandler(this.btnDx_Click);
            // 
            // btnSx
            // 
            this.btnSx.Location = new System.Drawing.Point(8, 12);
            this.btnSx.Name = "btnSx";
            this.btnSx.Size = new System.Drawing.Size(31, 31);
            this.btnSx.TabIndex = 20;
            this.btnSx.Text = "ᐊ";
            this.btnSx.UseVisualStyleBackColor = true;
            this.btnSx.Click += new System.EventHandler(this.btnSx_Click);
            // 
            // btnOpenRewardCalculator
            // 
            this.btnOpenRewardCalculator.Location = new System.Drawing.Point(206, 530);
            this.btnOpenRewardCalculator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenRewardCalculator.Name = "btnOpenRewardCalculator";
            this.btnOpenRewardCalculator.Size = new System.Drawing.Size(192, 52);
            this.btnOpenRewardCalculator.TabIndex = 21;
            this.btnOpenRewardCalculator.Text = "Reward Calculator";
            this.btnOpenRewardCalculator.UseVisualStyleBackColor = true;
            this.btnOpenRewardCalculator.Click += new System.EventHandler(this.btnOpenRewardCalculator_Click);
            // 
            // imgMap
            // 
            this.imgMap.BackgroundImage = global::TeraFinder.Properties.Resources.world;
            this.imgMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imgMap.Location = new System.Drawing.Point(404, 12);
            this.imgMap.Name = "imgMap";
            this.imgMap.Size = new System.Drawing.Size(570, 570);
            this.imgMap.TabIndex = 22;
            this.imgMap.TabStop = false;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 589);
            this.Controls.Add(this.imgMap);
            this.Controls.Add(this.btnOpenRewardCalculator);
            this.Controls.Add(this.btnDx);
            this.Controls.Add(this.btnSx);
            this.Controls.Add(this.btnOpenCalculator);
            this.Controls.Add(this.grpPkmInfo);
            this.Controls.Add(this.GrpRaidInfo);
            this.Controls.Add(this.cmbDens);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tera Raid Editor";
            this.GrpRaidInfo.ResumeLayout(false);
            this.GrpRaidInfo.PerformLayout();
            this.grpPkmInfo.ResumeLayout(false);
            this.grpPkmInfo.PerformLayout();
            this.grpMoves.ResumeLayout(false);
            this.grpMoves.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgMap)).EndInit();
            this.ResumeLayout(false);

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
    }
}