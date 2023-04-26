namespace TeraFinder.Forms
{
    partial class ConnectionForm
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
            components = new System.ComponentModel.Container();
            grpDevice = new GroupBox();
            numPort = new NumericUpDown();
            lblPort = new Label();
            radioUSB = new RadioButton();
            lblAddress = new Label();
            radioWiFi = new RadioButton();
            txtAddress = new TextBox();
            btnConnect = new Button();
            toolTip = new ToolTip(components);
            btnDisconnect = new Button();
            chkOutbreaks = new CheckBox();
            grpDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            // 
            // grpDevice
            // 
            grpDevice.Controls.Add(numPort);
            grpDevice.Controls.Add(lblPort);
            grpDevice.Controls.Add(radioUSB);
            grpDevice.Controls.Add(lblAddress);
            grpDevice.Controls.Add(radioWiFi);
            grpDevice.Controls.Add(txtAddress);
            grpDevice.Location = new Point(12, 12);
            grpDevice.Name = "grpDevice";
            grpDevice.Size = new Size(356, 109);
            grpDevice.TabIndex = 0;
            grpDevice.TabStop = false;
            grpDevice.Text = "Connection";
            // 
            // numPort
            // 
            numPort.Location = new Point(263, 67);
            numPort.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(64, 27);
            numPort.TabIndex = 5;
            numPort.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(219, 70);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(38, 20);
            lblPort.TabIndex = 4;
            lblPort.Text = "Port:";
            // 
            // radioUSB
            // 
            radioUSB.AutoSize = true;
            radioUSB.Location = new Point(214, 26);
            radioUSB.Name = "radioUSB";
            radioUSB.Size = new Size(57, 24);
            radioUSB.TabIndex = 1;
            radioUSB.TabStop = true;
            radioUSB.Text = "USB";
            radioUSB.UseVisualStyleBackColor = true;
            radioUSB.CheckedChanged += radioUSB_CheckedChanged;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(21, 70);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(65, 20);
            lblAddress.TabIndex = 3;
            lblAddress.Text = "Address:";
            // 
            // radioWiFi
            // 
            radioWiFi.AutoSize = true;
            radioWiFi.Checked = true;
            radioWiFi.Location = new Point(103, 26);
            radioWiFi.Name = "radioWiFi";
            radioWiFi.Size = new Size(65, 24);
            radioWiFi.TabIndex = 0;
            radioWiFi.TabStop = true;
            radioWiFi.Text = "Wi-Fi";
            radioWiFi.UseVisualStyleBackColor = true;
            radioWiFi.CheckedChanged += radioWiFi_CheckedChanged;
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(92, 67);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(125, 27);
            txtAddress.TabIndex = 1;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(12, 157);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(356, 52);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 10000;
            toolTip.InitialDelay = 500;
            toolTip.IsBalloon = true;
            toolTip.ReshowDelay = 100;
            toolTip.ToolTipTitle = "Mass Outbreaks Data";
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(12, 213);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(356, 52);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // chkOutbreaks
            // 
            chkOutbreaks.AutoSize = true;
            chkOutbreaks.Location = new Point(12, 127);
            chkOutbreaks.Name = "chkOutbreaks";
            chkOutbreaks.Size = new Size(265, 24);
            chkOutbreaks.TabIndex = 4;
            chkOutbreaks.Text = "Download Mass Outbreaks Data (?)";
            chkOutbreaks.UseVisualStyleBackColor = true;
            // 
            // ConnectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(375, 274);
            Controls.Add(chkOutbreaks);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(grpDevice);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionForm";
            ShowIcon = false;
            Text = "Remote Connection";
            grpDevice.ResumeLayout(false);
            grpDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpDevice;
        private RadioButton radioUSB;
        private RadioButton radioWiFi;
        private Label lblPort;
        private Label lblAddress;
        private TextBox txtAddress;
        private Button btnConnect;
        private NumericUpDown numPort;
        private ToolTip toolTip;
        private Button btnDisconnect;
        private CheckBox chkOutbreaks;
    }
}