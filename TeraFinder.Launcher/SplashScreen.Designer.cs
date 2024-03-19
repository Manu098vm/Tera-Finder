namespace TeraFinder.Launcher
{
    partial class SplashScreen
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
            pictureSplash = new PictureBox();
            lblName = new Label();
            lblInfo = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureSplash).BeginInit();
            SuspendLayout();
            // 
            // pictureSplash
            // 
            pictureSplash.BackgroundImage = Properties.Resources.splash;
            pictureSplash.Location = new Point(8, 7);
            pictureSplash.Name = "pictureSplash";
            pictureSplash.Size = new Size(106, 63);
            pictureSplash.TabIndex = 0;
            pictureSplash.TabStop = false;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(122, 14);
            lblName.Name = "lblName";
            lblName.Size = new Size(178, 20);
            lblName.TabIndex = 1;
            lblName.Text = "Tera Finder is initializing...";
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(147, 43);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(118, 20);
            lblInfo.TabIndex = 2;
            lblInfo.Text = "www.manu.tools";
            // 
            // SplashScreen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 77);
            ControlBox = false;
            Controls.Add(lblInfo);
            Controls.Add(lblName);
            Controls.Add(pictureSplash);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "SplashScreen";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tera Finder";
            ((System.ComponentModel.ISupportInitialize)pictureSplash).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureSplash;
        private Label lblName;
        private Label lblInfo;
    }
}