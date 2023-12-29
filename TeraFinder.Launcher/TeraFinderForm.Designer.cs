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
            grpSAV = new GroupBox();
            txtSAV = new TextBox();
            btnLoad = new Button();
            btnExport = new Button();
            grpTools = new GroupBox();
            grpStaticTools = new GroupBox();
            btnStartRewardCalc = new Button();
            btnStartCalculator = new Button();
            btnStartFinder = new Button();
            grpSavTools = new GroupBox();
            btnEditOutbreaks = new Button();
            btnEditGame = new Button();
            btnImportNews = new Button();
            btnStartEditor = new Button();
            openFileDialog = new OpenFileDialog();
            menuStrip1 = new MenuStrip();
            remoteConnectToolStripMenuItem = new ToolStripMenuItem();
            btnRemoteConnect = new ToolStripMenuItem();
            languageToolStrip = new ToolStripMenuItem();
            cmbLanguage = new ToolStripComboBox();
            lblEvent = new Label();
            grpSAV.SuspendLayout();
            grpTools.SuspendLayout();
            grpStaticTools.SuspendLayout();
            grpSavTools.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // grpSAV
            // 
            grpSAV.Controls.Add(txtSAV);
            grpSAV.Controls.Add(btnLoad);
            grpSAV.Controls.Add(btnExport);
            grpSAV.Location = new Point(10, 32);
            grpSAV.Margin = new Padding(3, 2, 3, 2);
            grpSAV.Name = "grpSAV";
            grpSAV.Padding = new Padding(3, 2, 3, 2);
            grpSAV.Size = new Size(405, 92);
            grpSAV.TabIndex = 0;
            grpSAV.TabStop = false;
            grpSAV.Text = "Save File";
            // 
            // txtSAV
            // 
            txtSAV.Location = new Point(79, 55);
            txtSAV.Margin = new Padding(3, 2, 3, 2);
            txtSAV.Name = "txtSAV";
            txtSAV.ReadOnly = true;
            txtSAV.Size = new Size(246, 23);
            txtSAV.TabIndex = 4;
            txtSAV.TextAlign = HorizontalAlignment.Center;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(79, 24);
            btnLoad.Margin = new Padding(3, 2, 3, 2);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(120, 26);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Load Save File";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(204, 24);
            btnExport.Margin = new Padding(3, 2, 3, 2);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 26);
            btnExport.TabIndex = 3;
            btnExport.Text = "Export Save File";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // grpTools
            // 
            grpTools.Controls.Add(grpStaticTools);
            grpTools.Controls.Add(grpSavTools);
            grpTools.Location = new Point(10, 128);
            grpTools.Margin = new Padding(3, 2, 3, 2);
            grpTools.Name = "grpTools";
            grpTools.Padding = new Padding(3, 2, 3, 2);
            grpTools.Size = new Size(405, 248);
            grpTools.TabIndex = 1;
            grpTools.TabStop = false;
            grpTools.Text = "Tools";
            // 
            // grpStaticTools
            // 
            grpStaticTools.Controls.Add(btnStartRewardCalc);
            grpStaticTools.Controls.Add(btnStartCalculator);
            grpStaticTools.Controls.Add(btnStartFinder);
            grpStaticTools.Location = new Point(5, 148);
            grpStaticTools.Margin = new Padding(3, 2, 3, 2);
            grpStaticTools.Name = "grpStaticTools";
            grpStaticTools.Padding = new Padding(3, 2, 3, 2);
            grpStaticTools.Size = new Size(395, 94);
            grpStaticTools.TabIndex = 3;
            grpStaticTools.TabStop = false;
            grpStaticTools.Text = "Standalone Tools";
            // 
            // btnStartRewardCalc
            // 
            btnStartRewardCalc.Location = new Point(266, 32);
            btnStartRewardCalc.Name = "btnStartRewardCalc";
            btnStartRewardCalc.Size = new Size(120, 40);
            btnStartRewardCalc.TabIndex = 7;
            btnStartRewardCalc.Text = "Reward Calculator";
            btnStartRewardCalc.UseVisualStyleBackColor = true;
            btnStartRewardCalc.Click += btnStartRewardCalc_Click;
            // 
            // btnStartCalculator
            // 
            btnStartCalculator.Location = new Point(140, 32);
            btnStartCalculator.Margin = new Padding(3, 2, 3, 2);
            btnStartCalculator.Name = "btnStartCalculator";
            btnStartCalculator.Size = new Size(120, 40);
            btnStartCalculator.TabIndex = 2;
            btnStartCalculator.Text = "Raid Calculator";
            btnStartCalculator.UseVisualStyleBackColor = true;
            btnStartCalculator.Click += btnStartCalculator_Click;
            // 
            // btnStartFinder
            // 
            btnStartFinder.Location = new Point(14, 32);
            btnStartFinder.Margin = new Padding(3, 2, 3, 2);
            btnStartFinder.Name = "btnStartFinder";
            btnStartFinder.Size = new Size(120, 40);
            btnStartFinder.TabIndex = 6;
            btnStartFinder.Text = "Seed Checker";
            btnStartFinder.UseVisualStyleBackColor = true;
            btnStartFinder.Click += btnStartFinder_Click;
            // 
            // grpSavTools
            // 
            grpSavTools.Controls.Add(btnEditOutbreaks);
            grpSavTools.Controls.Add(btnEditGame);
            grpSavTools.Controls.Add(btnImportNews);
            grpSavTools.Controls.Add(btnStartEditor);
            grpSavTools.Location = new Point(5, 20);
            grpSavTools.Margin = new Padding(3, 2, 3, 2);
            grpSavTools.Name = "grpSavTools";
            grpSavTools.Padding = new Padding(3, 2, 3, 2);
            grpSavTools.Size = new Size(395, 124);
            grpSavTools.TabIndex = 2;
            grpSavTools.TabStop = false;
            grpSavTools.Text = "SAV Tools";
            // 
            // btnEditOutbreaks
            // 
            btnEditOutbreaks.Location = new Point(201, 74);
            btnEditOutbreaks.Margin = new Padding(3, 2, 3, 2);
            btnEditOutbreaks.Name = "btnEditOutbreaks";
            btnEditOutbreaks.Size = new Size(188, 40);
            btnEditOutbreaks.TabIndex = 6;
            btnEditOutbreaks.Text = "Mass Outbreak Viewer/Editor";
            btnEditOutbreaks.UseVisualStyleBackColor = true;
            btnEditOutbreaks.Click += button1_Click;
            // 
            // btnEditGame
            // 
            btnEditGame.Location = new Point(201, 30);
            btnEditGame.Margin = new Padding(3, 2, 3, 2);
            btnEditGame.Name = "btnEditGame";
            btnEditGame.Size = new Size(188, 40);
            btnEditGame.TabIndex = 4;
            btnEditGame.Text = "Edit Game Flags";
            btnEditGame.UseVisualStyleBackColor = true;
            btnEditGame.Click += btnEditGame_Click;
            // 
            // btnImportNews
            // 
            btnImportNews.Location = new Point(5, 30);
            btnImportNews.Margin = new Padding(3, 2, 3, 2);
            btnImportNews.Name = "btnImportNews";
            btnImportNews.Size = new Size(188, 40);
            btnImportNews.TabIndex = 3;
            btnImportNews.Text = "Import Poké Portal News";
            btnImportNews.UseVisualStyleBackColor = true;
            btnImportNews.Click += btnImportNews_Click;
            // 
            // btnStartEditor
            // 
            btnStartEditor.Location = new Point(5, 74);
            btnStartEditor.Margin = new Padding(3, 2, 3, 2);
            btnStartEditor.Name = "btnStartEditor";
            btnStartEditor.Size = new Size(188, 40);
            btnStartEditor.TabIndex = 5;
            btnStartEditor.Text = "Raid Viewer/Editor";
            btnStartEditor.UseVisualStyleBackColor = true;
            btnStartEditor.Click += btnStartEditor_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { remoteConnectToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(426, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // remoteConnectToolStripMenuItem
            // 
            remoteConnectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btnRemoteConnect, languageToolStrip });
            remoteConnectToolStripMenuItem.Name = "remoteConnectToolStripMenuItem";
            remoteConnectToolStripMenuItem.Size = new Size(46, 20);
            remoteConnectToolStripMenuItem.Text = "Tools";
            // 
            // btnRemoteConnect
            // 
            btnRemoteConnect.Name = "btnRemoteConnect";
            btnRemoteConnect.Size = new Size(216, 22);
            btnRemoteConnect.Text = "Connect To Remote Device";
            btnRemoteConnect.Click += btnRemoteConnect_Click;
            // 
            // languageToolStrip
            // 
            languageToolStrip.DropDownItems.AddRange(new ToolStripItem[] { cmbLanguage });
            languageToolStrip.Name = "languageToolStrip";
            languageToolStrip.Size = new Size(216, 22);
            languageToolStrip.Text = "Default Language";
            // 
            // cmbLanguage
            // 
            cmbLanguage.Items.AddRange(new object[] { "日本語", "English", "Français", "Italiano", "Deutsch", "Español", "한국어", "简体中文", "繁體中文" });
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(121, 23);
            cmbLanguage.SelectedIndexChanged += LanguageChanged;
            // 
            // lblEvent
            // 
            lblEvent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEvent.AutoSize = true;
            lblEvent.Location = new Point(91, 15);
            lblEvent.Name = "lblEvent";
            lblEvent.RightToLeft = RightToLeft.No;
            lblEvent.Size = new Size(193, 15);
            lblEvent.TabIndex = 3;
            lblEvent.Text = "Poké Portal News Event: [00000000]";
            lblEvent.TextAlign = ContentAlignment.TopRight;
            // 
            // TeraFinderForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(426, 380);
            Controls.Add(lblEvent);
            Controls.Add(grpTools);
            Controls.Add(grpSAV);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TeraFinderForm";
            RightToLeft = RightToLeft.No;
            Text = "Tera Finder ";
            EnabledChanged += FormEnabledChanged;
            DragDrop += FileDragDrop;
            DragEnter += FileDragEnter;
            grpSAV.ResumeLayout(false);
            grpSAV.PerformLayout();
            grpTools.ResumeLayout(false);
            grpStaticTools.ResumeLayout(false);
            grpSavTools.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStripMenuItem languageToolStrip;
        private ToolStripComboBox cmbLanguage;
        private Button btnEditOutbreaks;
    }
}