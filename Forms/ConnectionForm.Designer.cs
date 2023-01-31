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
            this.components = new System.ComponentModel.Container();
            this.grpDevice = new System.Windows.Forms.GroupBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.radioUSB = new System.Windows.Forms.RadioButton();
            this.lblAddress = new System.Windows.Forms.Label();
            this.radioWiFi = new System.Windows.Forms.RadioButton();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chkEventData = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.grpDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // grpDevice
            // 
            this.grpDevice.Controls.Add(this.numPort);
            this.grpDevice.Controls.Add(this.lblPort);
            this.grpDevice.Controls.Add(this.radioUSB);
            this.grpDevice.Controls.Add(this.lblAddress);
            this.grpDevice.Controls.Add(this.radioWiFi);
            this.grpDevice.Controls.Add(this.txtAddress);
            this.grpDevice.Location = new System.Drawing.Point(12, 12);
            this.grpDevice.Name = "grpDevice";
            this.grpDevice.Size = new System.Drawing.Size(356, 109);
            this.grpDevice.TabIndex = 0;
            this.grpDevice.TabStop = false;
            this.grpDevice.Text = "Connection";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(263, 67);
            this.numPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(64, 27);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(219, 70);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(38, 20);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";
            // 
            // radioUSB
            // 
            this.radioUSB.AutoSize = true;
            this.radioUSB.Location = new System.Drawing.Point(214, 26);
            this.radioUSB.Name = "radioUSB";
            this.radioUSB.Size = new System.Drawing.Size(57, 24);
            this.radioUSB.TabIndex = 1;
            this.radioUSB.TabStop = true;
            this.radioUSB.Text = "USB";
            this.radioUSB.UseVisualStyleBackColor = true;
            this.radioUSB.CheckedChanged += new System.EventHandler(this.radioUSB_CheckedChanged);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(21, 70);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(65, 20);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address:";
            // 
            // radioWiFi
            // 
            this.radioWiFi.AutoSize = true;
            this.radioWiFi.Checked = true;
            this.radioWiFi.Location = new System.Drawing.Point(103, 26);
            this.radioWiFi.Name = "radioWiFi";
            this.radioWiFi.Size = new System.Drawing.Size(65, 24);
            this.radioWiFi.TabIndex = 0;
            this.radioWiFi.TabStop = true;
            this.radioWiFi.Text = "Wi-Fi";
            this.radioWiFi.UseVisualStyleBackColor = true;
            this.radioWiFi.CheckedChanged += new System.EventHandler(this.radioWiFi_CheckedChanged);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(92, 67);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(125, 27);
            this.txtAddress.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(10, 157);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(356, 52);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chkEventData
            // 
            this.chkEventData.AutoSize = true;
            this.chkEventData.Location = new System.Drawing.Point(21, 127);
            this.chkEventData.Name = "chkEventData";
            this.chkEventData.Size = new System.Drawing.Size(289, 24);
            this.chkEventData.TabIndex = 2;
            this.chkEventData.Text = "Download Event Data from Remote (?)";
            this.chkEventData.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipTitle = "Event Data";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(12, 215);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(354, 52);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 275);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.chkEventData);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.grpDevice);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionForm";
            this.ShowIcon = false;
            this.Text = "Remote Connection";
            this.grpDevice.ResumeLayout(false);
            this.grpDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private CheckBox chkEventData;
        private ToolTip toolTip;
        private Button btnDisconnect;
    }
}