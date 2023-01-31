namespace TeraFinder.Forms
{
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
            this.cmbProgress = new System.Windows.Forms.ComboBox();
            this.btnApplyProgress = new System.Windows.Forms.Button();
            this.grpGameProgress = new System.Windows.Forms.GroupBox();
            this.grpRaidMighty = new System.Windows.Forms.GroupBox();
            this.btnApplyRaid7 = new System.Windows.Forms.Button();
            this.chkDefeated = new System.Windows.Forms.CheckBox();
            this.chkCaptured = new System.Windows.Forms.CheckBox();
            this.cmbMightyIndex = new System.Windows.Forms.ComboBox();
            this.grpGameProgress.SuspendLayout();
            this.grpRaidMighty.SuspendLayout();
            this.SuspendLayout();
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
            this.cmbProgress.Location = new System.Drawing.Point(8, 32);
            this.cmbProgress.Name = "cmbProgress";
            this.cmbProgress.Size = new System.Drawing.Size(203, 28);
            this.cmbProgress.TabIndex = 1;
            // 
            // btnApplyProgress
            // 
            this.btnApplyProgress.Location = new System.Drawing.Point(7, 68);
            this.btnApplyProgress.Name = "btnApplyProgress";
            this.btnApplyProgress.Size = new System.Drawing.Size(203, 43);
            this.btnApplyProgress.TabIndex = 3;
            this.btnApplyProgress.Text = "Apply";
            this.btnApplyProgress.UseVisualStyleBackColor = true;
            this.btnApplyProgress.Click += new System.EventHandler(this.btnApplyProgress_Click);
            // 
            // grpGameProgress
            // 
            this.grpGameProgress.Controls.Add(this.btnApplyProgress);
            this.grpGameProgress.Controls.Add(this.cmbProgress);
            this.grpGameProgress.Location = new System.Drawing.Point(11, 1);
            this.grpGameProgress.Name = "grpGameProgress";
            this.grpGameProgress.Size = new System.Drawing.Size(217, 123);
            this.grpGameProgress.TabIndex = 4;
            this.grpGameProgress.TabStop = false;
            this.grpGameProgress.Text = "Current Game Progress:";
            // 
            // grpRaidMighty
            // 
            this.grpRaidMighty.Controls.Add(this.btnApplyRaid7);
            this.grpRaidMighty.Controls.Add(this.chkDefeated);
            this.grpRaidMighty.Controls.Add(this.chkCaptured);
            this.grpRaidMighty.Controls.Add(this.cmbMightyIndex);
            this.grpRaidMighty.Location = new System.Drawing.Point(11, 129);
            this.grpRaidMighty.Name = "grpRaidMighty";
            this.grpRaidMighty.Size = new System.Drawing.Size(217, 139);
            this.grpRaidMighty.TabIndex = 5;
            this.grpRaidMighty.TabStop = false;
            this.grpRaidMighty.Text = "Re-Catch Mighty Entity";
            // 
            // btnApplyRaid7
            // 
            this.btnApplyRaid7.Location = new System.Drawing.Point(8, 91);
            this.btnApplyRaid7.Name = "btnApplyRaid7";
            this.btnApplyRaid7.Size = new System.Drawing.Size(202, 43);
            this.btnApplyRaid7.TabIndex = 9;
            this.btnApplyRaid7.Text = "Apply";
            this.btnApplyRaid7.UseVisualStyleBackColor = true;
            this.btnApplyRaid7.Click += new System.EventHandler(this.btnApplyRaid7_Click);
            // 
            // chkDefeated
            // 
            this.chkDefeated.AutoSize = true;
            this.chkDefeated.Location = new System.Drawing.Point(119, 63);
            this.chkDefeated.Name = "chkDefeated";
            this.chkDefeated.Size = new System.Drawing.Size(93, 24);
            this.chkDefeated.TabIndex = 8;
            this.chkDefeated.Text = "Defeated";
            this.chkDefeated.UseVisualStyleBackColor = true;
            // 
            // chkCaptured
            // 
            this.chkCaptured.AutoSize = true;
            this.chkCaptured.Location = new System.Drawing.Point(15, 63);
            this.chkCaptured.Name = "chkCaptured";
            this.chkCaptured.Size = new System.Drawing.Size(92, 24);
            this.chkCaptured.TabIndex = 7;
            this.chkCaptured.Text = "Captured";
            this.chkCaptured.UseVisualStyleBackColor = true;
            // 
            // cmbMightyIndex
            // 
            this.cmbMightyIndex.FormattingEnabled = true;
            this.cmbMightyIndex.Location = new System.Drawing.Point(8, 27);
            this.cmbMightyIndex.Name = "cmbMightyIndex";
            this.cmbMightyIndex.Size = new System.Drawing.Size(203, 28);
            this.cmbMightyIndex.TabIndex = 6;
            this.cmbMightyIndex.SelectedIndexChanged += new System.EventHandler(this.cmbMightyIndex_IndexChanged);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 275);
            this.Controls.Add(this.grpRaidMighty);
            this.Controls.Add(this.grpGameProgress);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.Text = "Flag Editor";
            this.grpGameProgress.ResumeLayout(false);
            this.grpRaidMighty.ResumeLayout(false);
            this.grpRaidMighty.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ComboBox cmbProgress;
        private Button btnApplyProgress;
        private GroupBox grpGameProgress;
        private GroupBox grpRaidMighty;
        private Button btnApplyRaid7;
        private CheckBox chkDefeated;
        private CheckBox chkCaptured;
        private ComboBox cmbMightyIndex;
    }
}