namespace TeraFinder.Plugins;

partial class ProgressForm
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
        cmbProgress = new ComboBox();
        btnApplyProgress = new Button();
        grpGameProgress = new GroupBox();
        grpRaidMighty = new GroupBox();
        btnApplyRaid7 = new Button();
        chkCaptured = new CheckBox();
        cmbMightyIndex = new ComboBox();
        grpGameProgress.SuspendLayout();
        grpRaidMighty.SuspendLayout();
        SuspendLayout();
        // 
        // cmbProgress
        // 
        cmbProgress.FormattingEnabled = true;
        cmbProgress.Items.AddRange(new object[] { "Beginning", "UnlockedTeraRaids", "Unlocked3Stars", "Unlocked4Stars", "Unlocked5Stars", "Unlocked6Stars" });
        cmbProgress.Location = new Point(8, 32);
        cmbProgress.Name = "cmbProgress";
        cmbProgress.Size = new Size(203, 28);
        cmbProgress.TabIndex = 1;
        // 
        // btnApplyProgress
        // 
        btnApplyProgress.Location = new Point(7, 68);
        btnApplyProgress.Name = "btnApplyProgress";
        btnApplyProgress.Size = new Size(203, 43);
        btnApplyProgress.TabIndex = 3;
        btnApplyProgress.Text = "Apply";
        btnApplyProgress.UseVisualStyleBackColor = true;
        btnApplyProgress.Click += btnApplyProgress_Click;
        // 
        // grpGameProgress
        // 
        grpGameProgress.Controls.Add(btnApplyProgress);
        grpGameProgress.Controls.Add(cmbProgress);
        grpGameProgress.Location = new Point(11, 1);
        grpGameProgress.Name = "grpGameProgress";
        grpGameProgress.Size = new Size(217, 123);
        grpGameProgress.TabIndex = 4;
        grpGameProgress.TabStop = false;
        grpGameProgress.Text = "Current Game Progress:";
        // 
        // grpRaidMighty
        // 
        grpRaidMighty.Controls.Add(btnApplyRaid7);
        grpRaidMighty.Controls.Add(chkCaptured);
        grpRaidMighty.Controls.Add(cmbMightyIndex);
        grpRaidMighty.Location = new Point(11, 129);
        grpRaidMighty.Name = "grpRaidMighty";
        grpRaidMighty.Size = new Size(217, 139);
        grpRaidMighty.TabIndex = 5;
        grpRaidMighty.TabStop = false;
        grpRaidMighty.Text = "Re-Catch Mighty Entity";
        // 
        // btnApplyRaid7
        // 
        btnApplyRaid7.Location = new Point(8, 91);
        btnApplyRaid7.Name = "btnApplyRaid7";
        btnApplyRaid7.Size = new Size(202, 43);
        btnApplyRaid7.TabIndex = 9;
        btnApplyRaid7.Text = "Apply";
        btnApplyRaid7.UseVisualStyleBackColor = true;
        btnApplyRaid7.Click += btnApplyRaid7_Click;
        // 
        // chkCaptured
        // 
        chkCaptured.AutoSize = true;
        chkCaptured.Location = new Point(58, 61);
        chkCaptured.Name = "chkCaptured";
        chkCaptured.Size = new Size(92, 24);
        chkCaptured.TabIndex = 7;
        chkCaptured.Text = "Captured";
        chkCaptured.UseVisualStyleBackColor = true;
        // 
        // cmbMightyIndex
        // 
        cmbMightyIndex.FormattingEnabled = true;
        cmbMightyIndex.Location = new Point(8, 27);
        cmbMightyIndex.Name = "cmbMightyIndex";
        cmbMightyIndex.Size = new Size(203, 28);
        cmbMightyIndex.TabIndex = 6;
        cmbMightyIndex.SelectedIndexChanged += cmbMightyIndex_IndexChanged;
        // 
        // ProgressForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(235, 275);
        Controls.Add(grpRaidMighty);
        Controls.Add(grpGameProgress);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ProgressForm";
        ShowIcon = false;
        Text = "Flag Editor";
        grpGameProgress.ResumeLayout(false);
        grpRaidMighty.ResumeLayout(false);
        grpRaidMighty.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
    private ComboBox cmbProgress;
    private Button btnApplyProgress;
    private GroupBox grpGameProgress;
    private GroupBox grpRaidMighty;
    private Button btnApplyRaid7;
    private CheckBox chkCaptured;
    private ComboBox cmbMightyIndex;
}