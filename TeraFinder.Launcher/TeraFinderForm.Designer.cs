namespace TeraFinder.Launcher
{
    partial class TeraFinderForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeraFinderForm));
            this.grpSAV = new System.Windows.Forms.GroupBox();
            this.txtSAV = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.grpTools = new System.Windows.Forms.GroupBox();
            this.grpStaticTools = new System.Windows.Forms.GroupBox();
            this.btnStartRewardCalc = new System.Windows.Forms.Button();
            this.btnStartCalculator = new System.Windows.Forms.Button();
            this.btnStartFinder = new System.Windows.Forms.Button();
            this.grpSavTools = new System.Windows.Forms.GroupBox();
            this.btnEditGame = new System.Windows.Forms.Button();
            this.btnImportNews = new System.Windows.Forms.Button();
            this.btnStartEditor = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.remoteConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoteConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.lblEvent = new System.Windows.Forms.Label();
            this.grpSAV.SuspendLayout();
            this.grpTools.SuspendLayout();
            this.grpStaticTools.SuspendLayout();
            this.grpSavTools.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSAV
            // 
            this.grpSAV.Controls.Add(this.txtSAV);
            this.grpSAV.Controls.Add(this.btnLoad);
            this.grpSAV.Controls.Add(this.btnExport);
            this.grpSAV.Location = new System.Drawing.Point(12, 43);
            this.grpSAV.Name = "grpSAV";
            this.grpSAV.Size = new System.Drawing.Size(463, 123);
            this.grpSAV.TabIndex = 0;
            this.grpSAV.TabStop = false;
            this.grpSAV.Text = "Save File";
            // 
            // txtSAV
            // 
            this.txtSAV.Location = new System.Drawing.Point(90, 73);
            this.txtSAV.Name = "txtSAV";
            this.txtSAV.ReadOnly = true;
            this.txtSAV.Size = new System.Drawing.Size(281, 27);
            this.txtSAV.TabIndex = 4;
            this.txtSAV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(90, 32);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(137, 35);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load Save File";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(233, 32);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(137, 35);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export Save File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // grpTools
            // 
            this.grpTools.Controls.Add(this.grpStaticTools);
            this.grpTools.Controls.Add(this.grpSavTools);
            this.grpTools.Location = new System.Drawing.Point(12, 171);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(463, 291);
            this.grpTools.TabIndex = 1;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Tools";
            // 
            // grpStaticTools
            // 
            this.grpStaticTools.Controls.Add(this.btnStartRewardCalc);
            this.grpStaticTools.Controls.Add(this.btnStartCalculator);
            this.grpStaticTools.Controls.Add(this.btnStartFinder);
            this.grpStaticTools.Location = new System.Drawing.Point(6, 157);
            this.grpStaticTools.Name = "grpStaticTools";
            this.grpStaticTools.Size = new System.Drawing.Size(451, 125);
            this.grpStaticTools.TabIndex = 3;
            this.grpStaticTools.TabStop = false;
            this.grpStaticTools.Text = "Standalone Tools";
            // 
            // btnStartRewardCalc
            // 
            this.btnStartRewardCalc.Location = new System.Drawing.Point(304, 43);
            this.btnStartRewardCalc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnStartRewardCalc.Name = "btnStartRewardCalc";
            this.btnStartRewardCalc.Size = new System.Drawing.Size(137, 53);
            this.btnStartRewardCalc.TabIndex = 7;
            this.btnStartRewardCalc.Text = "Reward Calculator";
            this.btnStartRewardCalc.UseVisualStyleBackColor = true;
            this.btnStartRewardCalc.Click += new System.EventHandler(this.btnStartRewardCalc_Click);
            // 
            // btnStartCalculator
            // 
            this.btnStartCalculator.Location = new System.Drawing.Point(160, 43);
            this.btnStartCalculator.Name = "btnStartCalculator";
            this.btnStartCalculator.Size = new System.Drawing.Size(137, 53);
            this.btnStartCalculator.TabIndex = 2;
            this.btnStartCalculator.Text = "Raid Calculator";
            this.btnStartCalculator.UseVisualStyleBackColor = true;
            this.btnStartCalculator.Click += new System.EventHandler(this.btnStartCalculator_Click);
            // 
            // btnStartFinder
            // 
            this.btnStartFinder.Location = new System.Drawing.Point(16, 43);
            this.btnStartFinder.Name = "btnStartFinder";
            this.btnStartFinder.Size = new System.Drawing.Size(137, 53);
            this.btnStartFinder.TabIndex = 6;
            this.btnStartFinder.Text = "Seed Checker";
            this.btnStartFinder.UseVisualStyleBackColor = true;
            this.btnStartFinder.Click += new System.EventHandler(this.btnStartFinder_Click);
            // 
            // grpSavTools
            // 
            this.grpSavTools.Controls.Add(this.btnEditGame);
            this.grpSavTools.Controls.Add(this.btnImportNews);
            this.grpSavTools.Controls.Add(this.btnStartEditor);
            this.grpSavTools.Location = new System.Drawing.Point(6, 27);
            this.grpSavTools.Name = "grpSavTools";
            this.grpSavTools.Size = new System.Drawing.Size(451, 125);
            this.grpSavTools.TabIndex = 2;
            this.grpSavTools.TabStop = false;
            this.grpSavTools.Text = "SAV Tools";
            // 
            // btnEditGame
            // 
            this.btnEditGame.Location = new System.Drawing.Point(16, 40);
            this.btnEditGame.Name = "btnEditGame";
            this.btnEditGame.Size = new System.Drawing.Size(137, 53);
            this.btnEditGame.TabIndex = 4;
            this.btnEditGame.Text = "Edit Game Flags";
            this.btnEditGame.UseVisualStyleBackColor = true;
            this.btnEditGame.Click += new System.EventHandler(this.btnEditGame_Click);
            // 
            // btnImportNews
            // 
            this.btnImportNews.Location = new System.Drawing.Point(160, 40);
            this.btnImportNews.Name = "btnImportNews";
            this.btnImportNews.Size = new System.Drawing.Size(137, 53);
            this.btnImportNews.TabIndex = 3;
            this.btnImportNews.Text = "Import Poké Portal News";
            this.btnImportNews.UseVisualStyleBackColor = true;
            this.btnImportNews.Click += new System.EventHandler(this.btnImportNews_Click);
            // 
            // btnStartEditor
            // 
            this.btnStartEditor.Location = new System.Drawing.Point(304, 40);
            this.btnStartEditor.Name = "btnStartEditor";
            this.btnStartEditor.Size = new System.Drawing.Size(137, 53);
            this.btnStartEditor.TabIndex = 5;
            this.btnStartEditor.Text = "Raid Viewer/Editor";
            this.btnStartEditor.UseVisualStyleBackColor = true;
            this.btnStartEditor.Click += new System.EventHandler(this.btnStartEditor_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteConnectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(487, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // remoteConnectToolStripMenuItem
            // 
            this.remoteConnectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemoteConnect});
            this.remoteConnectToolStripMenuItem.Name = "remoteConnectToolStripMenuItem";
            this.remoteConnectToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.remoteConnectToolStripMenuItem.Text = "Tools";
            // 
            // btnRemoteConnect
            // 
            this.btnRemoteConnect.Name = "btnRemoteConnect";
            this.btnRemoteConnect.Size = new System.Drawing.Size(271, 26);
            this.btnRemoteConnect.Text = "Connect To Remote Device";
            this.btnRemoteConnect.Click += new System.EventHandler(this.btnRemoteConnect_Click);
            // 
            // lblEvent
            // 
            this.lblEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEvent.AutoSize = true;
            this.lblEvent.Location = new System.Drawing.Point(232, 20);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEvent.Size = new System.Drawing.Size(243, 20);
            this.lblEvent.TabIndex = 3;
            this.lblEvent.Text = "Poké Portal News Event: [00000000]";
            this.lblEvent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TeraFinderForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 471);
            this.Controls.Add(this.lblEvent);
            this.Controls.Add(this.grpTools);
            this.Controls.Add(this.grpSAV);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeraFinderForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Tera Finder ";
            this.EnabledChanged += new System.EventHandler(this.FormEnabledChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            this.grpSAV.ResumeLayout(false);
            this.grpSAV.PerformLayout();
            this.grpTools.ResumeLayout(false);
            this.grpStaticTools.ResumeLayout(false);
            this.grpSavTools.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox grpSAV;
        private GroupBox grpTools;
        private GroupBox grpStaticTools;
        private Button btnStartFinder;
        private Button btnStartCalculator;
        private GroupBox grpSavTools;
        private Button btnEditGame;
        private Button btnImportNews;
        private Button btnStartEditor;
        private TextBox txtSAV;
        private Button btnLoad;
        private Button btnExport;
        private OpenFileDialog openFileDialog;
        private Button btnStartRewardCalc;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem remoteConnectToolStripMenuItem;
        private ToolStripMenuItem btnRemoteConnect;
        private Label lblEvent;
    }
}