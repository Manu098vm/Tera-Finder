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
            this.cmbProgress.Location = new System.Drawing.Point(7, 24);
            this.cmbProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProgress.Name = "cmbProgress";
            this.cmbProgress.Size = new System.Drawing.Size(178, 23);
            this.cmbProgress.TabIndex = 1;
            // 
            // btnApplyProgress
            // 
            this.btnApplyProgress.Location = new System.Drawing.Point(6, 51);
            this.btnApplyProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApplyProgress.Name = "btnApplyProgress";
            this.btnApplyProgress.Size = new System.Drawing.Size(178, 32);
            this.btnApplyProgress.TabIndex = 3;
            this.btnApplyProgress.Text = "Apply";
            this.btnApplyProgress.UseVisualStyleBackColor = true;
            this.btnApplyProgress.Click += new System.EventHandler(this.btnApplyProgress_Click);
            // 
            // grpGameProgress
            // 
            this.grpGameProgress.Controls.Add(this.btnApplyProgress);
            this.grpGameProgress.Controls.Add(this.cmbProgress);
            this.grpGameProgress.Location = new System.Drawing.Point(10, 1);
            this.grpGameProgress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpGameProgress.Name = "grpGameProgress";
            this.grpGameProgress.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpGameProgress.Size = new System.Drawing.Size(190, 92);
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
            this.grpRaidMighty.Location = new System.Drawing.Point(10, 97);
            this.grpRaidMighty.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRaidMighty.Name = "grpRaidMighty";
            this.grpRaidMighty.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpRaidMighty.Size = new System.Drawing.Size(190, 104);
            this.grpRaidMighty.TabIndex = 5;
            this.grpRaidMighty.TabStop = false;
            this.grpRaidMighty.Text = "Re-Catch Mighty Entity";
            // 
            // btnApplyRaid7
            // 
            this.btnApplyRaid7.Location = new System.Drawing.Point(7, 68);
            this.btnApplyRaid7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApplyRaid7.Name = "btnApplyRaid7";
            this.btnApplyRaid7.Size = new System.Drawing.Size(177, 32);
            this.btnApplyRaid7.TabIndex = 9;
            this.btnApplyRaid7.Text = "Apply";
            this.btnApplyRaid7.UseVisualStyleBackColor = true;
            this.btnApplyRaid7.Click += new System.EventHandler(this.btnApplyRaid7_Click);
            // 
            // chkDefeated
            // 
            this.chkDefeated.AutoSize = true;
            this.chkDefeated.Location = new System.Drawing.Point(111, 47);
            this.chkDefeated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDefeated.Name = "chkDefeated";
            this.chkDefeated.Size = new System.Drawing.Size(73, 19);
            this.chkDefeated.TabIndex = 8;
            this.chkDefeated.Text = "Defeated";
            this.chkDefeated.UseVisualStyleBackColor = true;
            // 
            // chkCaptured
            // 
            this.chkCaptured.AutoSize = true;
            this.chkCaptured.Location = new System.Drawing.Point(13, 47);
            this.chkCaptured.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkCaptured.Name = "chkCaptured";
            this.chkCaptured.Size = new System.Drawing.Size(75, 19);
            this.chkCaptured.TabIndex = 7;
            this.chkCaptured.Text = "Captured";
            this.chkCaptured.UseVisualStyleBackColor = true;
            // 
            // cmbMightyIndex
            // 
            this.cmbMightyIndex.FormattingEnabled = true;
            this.cmbMightyIndex.Location = new System.Drawing.Point(7, 20);
            this.cmbMightyIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbMightyIndex.Name = "cmbMightyIndex";
            this.cmbMightyIndex.Size = new System.Drawing.Size(178, 23);
            this.cmbMightyIndex.TabIndex = 6;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 206);
            this.Controls.Add(this.grpRaidMighty);
            this.Controls.Add(this.grpGameProgress);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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