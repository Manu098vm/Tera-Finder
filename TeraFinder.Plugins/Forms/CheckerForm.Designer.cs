namespace TeraFinder.Plugins;

partial class CheckerForm
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
        lblEC = new Label();
        lblScale = new Label();
        lblPID = new Label();
        lblHP = new Label();
        lblWeight = new Label();
        lblHeight = new Label();
        lblSpe = new Label();
        lblSPA = new Label();
        lblSPD = new Label();
        lblDef = new Label();
        lblAtk = new Label();
        btnCalc = new Button();
        txtEC = new TextBox();
        txtPID = new TextBox();
        numScale = new NumericUpDown();
        numHP = new NumericUpDown();
        numWeight = new NumericUpDown();
        numHeight = new NumericUpDown();
        numSpe = new NumericUpDown();
        numSpD = new NumericUpDown();
        numSpA = new NumericUpDown();
        numAtk = new NumericUpDown();
        numDef = new NumericUpDown();
        grpSeed = new GroupBox();
        txtSeed = new TextBox();
        lblTera = new Label();
        cmbTera = new ComboBox();
        lblNature = new Label();
        cmbNature = new ComboBox();
        lblTID = new Label();
        lblSID = new Label();
        txtTid = new TextBox();
        txtSid = new TextBox();
        lblSpecies = new Label();
        cmbSpecies = new ComboBox();
        ((System.ComponentModel.ISupportInitialize)numScale).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHP).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numWeight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSpe).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSpD).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSpA).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numAtk).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDef).BeginInit();
        grpSeed.SuspendLayout();
        SuspendLayout();
        // 
        // lblEC
        // 
        lblEC.AutoSize = true;
        lblEC.Location = new Point(13, 115);
        lblEC.Name = "lblEC";
        lblEC.Size = new Size(29, 20);
        lblEC.TabIndex = 0;
        lblEC.Text = "EC:";
        // 
        // lblScale
        // 
        lblScale.AutoSize = true;
        lblScale.Location = new Point(13, 513);
        lblScale.Name = "lblScale";
        lblScale.Size = new Size(47, 20);
        lblScale.TabIndex = 1;
        lblScale.Text = "Scale:";
        // 
        // lblPID
        // 
        lblPID.AutoSize = true;
        lblPID.Location = new Point(13, 149);
        lblPID.Name = "lblPID";
        lblPID.Size = new Size(35, 20);
        lblPID.TabIndex = 2;
        lblPID.Text = "PID:";
        // 
        // lblHP
        // 
        lblHP.AutoSize = true;
        lblHP.Location = new Point(13, 181);
        lblHP.Name = "lblHP";
        lblHP.Size = new Size(31, 20);
        lblHP.TabIndex = 3;
        lblHP.Text = "HP:";
        // 
        // lblWeight
        // 
        lblWeight.AutoSize = true;
        lblWeight.Location = new Point(13, 479);
        lblWeight.Name = "lblWeight";
        lblWeight.Size = new Size(59, 20);
        lblWeight.TabIndex = 4;
        lblWeight.Text = "Weight:";
        // 
        // lblHeight
        // 
        lblHeight.AutoSize = true;
        lblHeight.Location = new Point(13, 447);
        lblHeight.Name = "lblHeight";
        lblHeight.Size = new Size(57, 20);
        lblHeight.TabIndex = 5;
        lblHeight.Text = "Height:";
        // 
        // lblSpe
        // 
        lblSpe.AutoSize = true;
        lblSpe.Location = new Point(13, 345);
        lblSpe.Name = "lblSpe";
        lblSpe.Size = new Size(37, 20);
        lblSpe.TabIndex = 6;
        lblSpe.Text = "Spe:";
        // 
        // lblSPA
        // 
        lblSPA.AutoSize = true;
        lblSPA.Location = new Point(13, 279);
        lblSPA.Name = "lblSPA";
        lblSPA.Size = new Size(39, 20);
        lblSPA.TabIndex = 7;
        lblSPA.Text = "SpA:";
        // 
        // lblSPD
        // 
        lblSPD.AutoSize = true;
        lblSPD.Location = new Point(13, 311);
        lblSPD.Name = "lblSPD";
        lblSPD.Size = new Size(40, 20);
        lblSPD.TabIndex = 8;
        lblSPD.Text = "SpD:";
        // 
        // lblDef
        // 
        lblDef.AutoSize = true;
        lblDef.Location = new Point(13, 246);
        lblDef.Name = "lblDef";
        lblDef.Size = new Size(36, 20);
        lblDef.TabIndex = 9;
        lblDef.Text = "Def:";
        // 
        // lblAtk
        // 
        lblAtk.AutoSize = true;
        lblAtk.Location = new Point(13, 213);
        lblAtk.Name = "lblAtk";
        lblAtk.Size = new Size(34, 20);
        lblAtk.TabIndex = 10;
        lblAtk.Text = "Atk:";
        // 
        // btnCalc
        // 
        btnCalc.Location = new Point(13, 561);
        btnCalc.Name = "btnCalc";
        btnCalc.Size = new Size(187, 39);
        btnCalc.TabIndex = 11;
        btnCalc.Text = "Calculate Seed";
        btnCalc.UseVisualStyleBackColor = true;
        btnCalc.Click += btnCalc_Click;
        // 
        // txtEC
        // 
        txtEC.Location = new Point(76, 111);
        txtEC.MaxLength = 8;
        txtEC.Name = "txtEC";
        txtEC.Size = new Size(125, 27);
        txtEC.TabIndex = 22;
        txtEC.KeyPress += txt_KeyPress;
        // 
        // txtPID
        // 
        txtPID.Location = new Point(76, 145);
        txtPID.MaxLength = 8;
        txtPID.Name = "txtPID";
        txtPID.Size = new Size(125, 27);
        txtPID.TabIndex = 23;
        txtPID.KeyPress += txt_KeyPress;
        // 
        // numScale
        // 
        numScale.Location = new Point(76, 511);
        numScale.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        numScale.Name = "numScale";
        numScale.Size = new Size(125, 27);
        numScale.TabIndex = 24;
        // 
        // numHP
        // 
        numHP.Location = new Point(76, 178);
        numHP.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numHP.Name = "numHP";
        numHP.Size = new Size(125, 27);
        numHP.TabIndex = 25;
        // 
        // numWeight
        // 
        numWeight.Location = new Point(76, 478);
        numWeight.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        numWeight.Name = "numWeight";
        numWeight.Size = new Size(125, 27);
        numWeight.TabIndex = 26;
        // 
        // numHeight
        // 
        numHeight.Location = new Point(76, 445);
        numHeight.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        numHeight.Name = "numHeight";
        numHeight.Size = new Size(125, 27);
        numHeight.TabIndex = 27;
        // 
        // numSpe
        // 
        numSpe.Location = new Point(76, 343);
        numSpe.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numSpe.Name = "numSpe";
        numSpe.Size = new Size(125, 27);
        numSpe.TabIndex = 28;
        // 
        // numSpD
        // 
        numSpD.Location = new Point(76, 310);
        numSpD.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numSpD.Name = "numSpD";
        numSpD.Size = new Size(125, 27);
        numSpD.TabIndex = 29;
        // 
        // numSpA
        // 
        numSpA.Location = new Point(76, 277);
        numSpA.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numSpA.Name = "numSpA";
        numSpA.Size = new Size(125, 27);
        numSpA.TabIndex = 30;
        // 
        // numAtk
        // 
        numAtk.Location = new Point(76, 211);
        numAtk.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numAtk.Name = "numAtk";
        numAtk.Size = new Size(125, 27);
        numAtk.TabIndex = 31;
        // 
        // numDef
        // 
        numDef.Location = new Point(76, 245);
        numDef.Maximum = new decimal(new int[] { 31, 0, 0, 0 });
        numDef.Name = "numDef";
        numDef.Size = new Size(125, 27);
        numDef.TabIndex = 32;
        // 
        // grpSeed
        // 
        grpSeed.Controls.Add(txtSeed);
        grpSeed.Location = new Point(13, 605);
        grpSeed.Name = "grpSeed";
        grpSeed.Size = new Size(187, 91);
        grpSeed.TabIndex = 33;
        grpSeed.TabStop = false;
        grpSeed.Text = "Calculated Seed";
        // 
        // txtSeed
        // 
        txtSeed.Location = new Point(6, 41);
        txtSeed.MaxLength = 50;
        txtSeed.Name = "txtSeed";
        txtSeed.ReadOnly = true;
        txtSeed.Size = new Size(175, 27);
        txtSeed.TabIndex = 34;
        // 
        // lblTera
        // 
        lblTera.AutoSize = true;
        lblTera.Location = new Point(13, 414);
        lblTera.Name = "lblTera";
        lblTera.Size = new Size(40, 20);
        lblTera.TabIndex = 36;
        lblTera.Text = "Tera:";
        // 
        // cmbTera
        // 
        cmbTera.FormattingEnabled = true;
        cmbTera.Items.AddRange(new object[] { "Normal", "Fighting", "Flying", "Poison", "Ground", "Rock", "Bug", "Ghost", "Steel", "Fire", "Water", "Grass", "Electric", "Psychic", "Ice", "Dragon", "Dark", "Fairy" });
        cmbTera.Location = new Point(76, 411);
        cmbTera.Name = "cmbTera";
        cmbTera.Size = new Size(125, 28);
        cmbTera.TabIndex = 37;
        // 
        // lblNature
        // 
        lblNature.AutoSize = true;
        lblNature.Location = new Point(13, 381);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(57, 20);
        lblNature.TabIndex = 38;
        lblNature.Text = "Nature:";
        // 
        // cmbNature
        // 
        cmbNature.FormattingEnabled = true;
        cmbNature.Items.AddRange(new object[] { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed", "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful", "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky" });
        cmbNature.Location = new Point(76, 377);
        cmbNature.Name = "cmbNature";
        cmbNature.Size = new Size(125, 28);
        cmbNature.TabIndex = 39;
        // 
        // lblTID
        // 
        lblTID.AutoSize = true;
        lblTID.Location = new Point(13, 53);
        lblTID.Name = "lblTID";
        lblTID.Size = new Size(35, 20);
        lblTID.TabIndex = 40;
        lblTID.Text = "TID:";
        // 
        // lblSID
        // 
        lblSID.AutoSize = true;
        lblSID.Location = new Point(13, 82);
        lblSID.Name = "lblSID";
        lblSID.Size = new Size(35, 20);
        lblSID.TabIndex = 41;
        lblSID.Text = "SID:";
        // 
        // txtTid
        // 
        txtTid.Location = new Point(76, 46);
        txtTid.MaxLength = 6;
        txtTid.Name = "txtTid";
        txtTid.Size = new Size(125, 27);
        txtTid.TabIndex = 42;
        txtTid.KeyPress += txtID_KeyPress;
        // 
        // txtSid
        // 
        txtSid.Location = new Point(76, 79);
        txtSid.MaxLength = 4;
        txtSid.Name = "txtSid";
        txtSid.Size = new Size(125, 27);
        txtSid.TabIndex = 43;
        txtSid.KeyPress += txtID_KeyPress;
        // 
        // lblSpecies
        // 
        lblSpecies.AutoSize = true;
        lblSpecies.Location = new Point(13, 15);
        lblSpecies.Name = "lblSpecies";
        lblSpecies.Size = new Size(62, 20);
        lblSpecies.TabIndex = 44;
        lblSpecies.Text = "Species:";
        // 
        // cmbSpecies
        // 
        cmbSpecies.FormattingEnabled = true;
        cmbSpecies.Location = new Point(76, 12);
        cmbSpecies.Name = "cmbSpecies";
        cmbSpecies.Size = new Size(125, 28);
        cmbSpecies.TabIndex = 45;
        // 
        // CheckerForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(212, 703);
        Controls.Add(cmbSpecies);
        Controls.Add(lblSpecies);
        Controls.Add(txtSid);
        Controls.Add(txtTid);
        Controls.Add(lblSID);
        Controls.Add(lblTID);
        Controls.Add(cmbNature);
        Controls.Add(lblNature);
        Controls.Add(cmbTera);
        Controls.Add(lblTera);
        Controls.Add(grpSeed);
        Controls.Add(numDef);
        Controls.Add(numAtk);
        Controls.Add(numSpA);
        Controls.Add(numSpD);
        Controls.Add(numSpe);
        Controls.Add(numHeight);
        Controls.Add(numWeight);
        Controls.Add(numHP);
        Controls.Add(numScale);
        Controls.Add(txtPID);
        Controls.Add(txtEC);
        Controls.Add(btnCalc);
        Controls.Add(lblAtk);
        Controls.Add(lblDef);
        Controls.Add(lblSPD);
        Controls.Add(lblSPA);
        Controls.Add(lblSpe);
        Controls.Add(lblHeight);
        Controls.Add(lblWeight);
        Controls.Add(lblHP);
        Controls.Add(lblPID);
        Controls.Add(lblScale);
        Controls.Add(lblEC);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CheckerForm";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Seed Checker";
        ((System.ComponentModel.ISupportInitialize)numScale).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHP).EndInit();
        ((System.ComponentModel.ISupportInitialize)numWeight).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSpe).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSpD).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSpA).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAtk).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDef).EndInit();
        grpSeed.ResumeLayout(false);
        grpSeed.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblEC;
    private Label lblScale;
    private Label lblPID;
    private Label lblHP;
    private Label lblWeight;
    private Label lblHeight;
    private Label lblSpe;
    private Label lblSPA;
    private Label lblSPD;
    private Label lblDef;
    private Label lblAtk;
    private Button btnCalc;
    private TextBox txtEC;
    private TextBox txtPID;
    private NumericUpDown numScale;
    private NumericUpDown numHP;
    private NumericUpDown numWeight;
    private NumericUpDown numHeight;
    private NumericUpDown numSpe;
    private NumericUpDown numSpD;
    private NumericUpDown numSpA;
    private NumericUpDown numAtk;
    private NumericUpDown numDef;
    private GroupBox grpSeed;
    private TextBox txtSeed;
    private Label lblTera;
    private ComboBox cmbTera;
    private Label lblNature;
    private ComboBox cmbNature;
    private Label lblTID;
    private Label lblSID;
    private TextBox txtTid;
    private TextBox txtSid;
    private Label lblSpecies;
    private ComboBox cmbSpecies;
}