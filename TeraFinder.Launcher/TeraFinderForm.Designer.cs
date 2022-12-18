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
            this.btnStartFinder = new System.Windows.Forms.Button();
            this.btnStartCalculator = new System.Windows.Forms.Button();
            this.grpSavTools = new System.Windows.Forms.GroupBox();
            this.btnEditGame = new System.Windows.Forms.Button();
            this.btnImportNews = new System.Windows.Forms.Button();
            this.btnStartEditor = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.grpSAV.SuspendLayout();
            this.grpTools.SuspendLayout();
            this.grpStaticTools.SuspendLayout();
            this.grpSavTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSAV
            // 
            this.grpSAV.Controls.Add(this.txtSAV);
            this.grpSAV.Controls.Add(this.btnLoad);
            this.grpSAV.Controls.Add(this.btnExport);
            this.grpSAV.Location = new System.Drawing.Point(12, 12);
            this.grpSAV.Name = "grpSAV";
            this.grpSAV.Size = new System.Drawing.Size(463, 122);
            this.grpSAV.TabIndex = 0;
            this.grpSAV.TabStop = false;
            this.grpSAV.Text = "Save File";
            // 
            // txtSAV
            // 
            this.txtSAV.Location = new System.Drawing.Point(90, 73);
            this.txtSAV.Name = "txtSAV";
            this.txtSAV.ReadOnly = true;
            this.txtSAV.Size = new System.Drawing.Size(280, 27);
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
            this.grpTools.Location = new System.Drawing.Point(12, 140);
            this.grpTools.Name = "grpTools";
            this.grpTools.Size = new System.Drawing.Size(463, 291);
            this.grpTools.TabIndex = 1;
            this.grpTools.TabStop = false;
            this.grpTools.Text = "Tools";
            // 
            // grpStaticTools
            // 
            this.grpStaticTools.Controls.Add(this.btnStartFinder);
            this.grpStaticTools.Controls.Add(this.btnStartCalculator);
            this.grpStaticTools.Location = new System.Drawing.Point(6, 158);
            this.grpStaticTools.Name = "grpStaticTools";
            this.grpStaticTools.Size = new System.Drawing.Size(451, 125);
            this.grpStaticTools.TabIndex = 3;
            this.grpStaticTools.TabStop = false;
            this.grpStaticTools.Text = "Standalone Tools";
            // 
            // btnStartFinder
            // 
            this.btnStartFinder.Location = new System.Drawing.Point(227, 40);
            this.btnStartFinder.Name = "btnStartFinder";
            this.btnStartFinder.Size = new System.Drawing.Size(137, 54);
            this.btnStartFinder.TabIndex = 6;
            this.btnStartFinder.Text = "Seed Finder";
            this.btnStartFinder.UseVisualStyleBackColor = true;
            this.btnStartFinder.Click += new System.EventHandler(this.btnStartFinder_Click);
            // 
            // btnStartCalculator
            // 
            this.btnStartCalculator.Location = new System.Drawing.Point(84, 40);
            this.btnStartCalculator.Name = "btnStartCalculator";
            this.btnStartCalculator.Size = new System.Drawing.Size(137, 54);
            this.btnStartCalculator.TabIndex = 2;
            this.btnStartCalculator.Text = "Raid Calculator";
            this.btnStartCalculator.UseVisualStyleBackColor = true;
            this.btnStartCalculator.Click += new System.EventHandler(this.btnStartCalculator_Click);
            // 
            // grpSavTools
            // 
            this.grpSavTools.Controls.Add(this.btnEditGame);
            this.grpSavTools.Controls.Add(this.btnImportNews);
            this.grpSavTools.Controls.Add(this.btnStartEditor);
            this.grpSavTools.Location = new System.Drawing.Point(6, 26);
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
            this.btnEditGame.Size = new System.Drawing.Size(137, 54);
            this.btnEditGame.TabIndex = 4;
            this.btnEditGame.Text = "Edit Game Flags";
            this.btnEditGame.UseVisualStyleBackColor = true;
            this.btnEditGame.Click += new System.EventHandler(this.btnEditGame_Click);
            // 
            // btnImportNews
            // 
            this.btnImportNews.Location = new System.Drawing.Point(159, 40);
            this.btnImportNews.Name = "btnImportNews";
            this.btnImportNews.Size = new System.Drawing.Size(137, 54);
            this.btnImportNews.TabIndex = 3;
            this.btnImportNews.Text = "Import Poké Portal News";
            this.btnImportNews.UseVisualStyleBackColor = true;
            this.btnImportNews.Click += new System.EventHandler(this.btnImportNews_Click);
            // 
            // btnStartEditor
            // 
            this.btnStartEditor.Location = new System.Drawing.Point(302, 40);
            this.btnStartEditor.Name = "btnStartEditor";
            this.btnStartEditor.Size = new System.Drawing.Size(137, 54);
            this.btnStartEditor.TabIndex = 5;
            this.btnStartEditor.Text = "Raid Viewer/Editor";
            this.btnStartEditor.UseVisualStyleBackColor = true;
            this.btnStartEditor.Click += new System.EventHandler(this.btnStartEditor_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // TeraFinderForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 437);
            this.Controls.Add(this.grpTools);
            this.Controls.Add(this.grpSAV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TeraFinderForm";
            this.Text = "Tera Finder";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            this.grpSAV.ResumeLayout(false);
            this.grpSAV.PerformLayout();
            this.grpTools.ResumeLayout(false);
            this.grpStaticTools.ResumeLayout(false);
            this.grpSavTools.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}