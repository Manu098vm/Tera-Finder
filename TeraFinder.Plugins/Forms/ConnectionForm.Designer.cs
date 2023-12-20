namespace TeraFinder.Plugins;

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
        toolTipOutbreakMain = new ToolTip(components);
        btnDisconnect = new Button();
        chkOutbreaksMain = new CheckBox();
        progressBar = new ProgressBar();
        chkOutbreaksDLC = new CheckBox();
        toolTipOutbreakDLC1 = new ToolTip(components);
        chkOutbreaksDLC2 = new CheckBox();
        toolTipOutbreakDLC2 = new ToolTip(components);
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
        grpDevice.Location = new Point(10, 12);
        grpDevice.Name = "grpDevice";
        grpDevice.Size = new Size(403, 109);
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
        btnConnect.Location = new Point(10, 217);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(403, 52);
        btnConnect.TabIndex = 1;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        btnConnect.Click += btnConnect_Click;
        // 
        // toolTipOutbreakMain
        // 
        toolTipOutbreakMain.AutoPopDelay = 10000;
        toolTipOutbreakMain.InitialDelay = 500;
        toolTipOutbreakMain.IsBalloon = true;
        toolTipOutbreakMain.ReshowDelay = 100;
        toolTipOutbreakMain.ToolTipTitle = "Mass Outbreaks Data";
        // 
        // btnDisconnect
        // 
        btnDisconnect.Enabled = false;
        btnDisconnect.Location = new Point(10, 273);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(403, 52);
        btnDisconnect.TabIndex = 3;
        btnDisconnect.Text = "Disconnect";
        btnDisconnect.UseVisualStyleBackColor = true;
        btnDisconnect.Click += btnDisconnect_Click;
        // 
        // chkOutbreaksMain
        // 
        chkOutbreaksMain.AutoSize = true;
        chkOutbreaksMain.Location = new Point(10, 127);
        chkOutbreaksMain.Name = "chkOutbreaksMain";
        chkOutbreaksMain.Size = new Size(313, 24);
        chkOutbreaksMain.TabIndex = 4;
        chkOutbreaksMain.Text = "Download Mass Outbreaks Paldea Data (?)";
        chkOutbreaksMain.UseVisualStyleBackColor = true;
        // 
        // progressBar
        // 
        progressBar.Location = new Point(10, 331);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(403, 22);
        progressBar.TabIndex = 5;
        // 
        // chkOutbreaksDLC
        // 
        chkOutbreaksDLC.AutoSize = true;
        chkOutbreaksDLC.Location = new Point(10, 157);
        chkOutbreaksDLC.Name = "chkOutbreaksDLC";
        chkOutbreaksDLC.Size = new Size(327, 24);
        chkOutbreaksDLC.TabIndex = 6;
        chkOutbreaksDLC.Text = "Download Mass Outbreaks Kitakami Data (?)";
        chkOutbreaksDLC.UseVisualStyleBackColor = true;
        // 
        // toolTipOutbreakDLC1
        // 
        toolTipOutbreakDLC1.AutoPopDelay = 10000;
        toolTipOutbreakDLC1.InitialDelay = 500;
        toolTipOutbreakDLC1.IsBalloon = true;
        toolTipOutbreakDLC1.ReshowDelay = 100;
        toolTipOutbreakDLC1.ToolTipTitle = "Mass Outbreaks Data";
        // 
        // chkOutbreaksDLC2
        // 
        chkOutbreaksDLC2.AutoSize = true;
        chkOutbreaksDLC2.Location = new Point(10, 187);
        chkOutbreaksDLC2.Name = "chkOutbreaksDLC2";
        chkOutbreaksDLC2.Size = new Size(332, 24);
        chkOutbreaksDLC2.TabIndex = 7;
        chkOutbreaksDLC2.Text = "Download Mass Outbreaks Blueberry Data (?)";
        chkOutbreaksDLC2.UseVisualStyleBackColor = true;
        // 
        // toolTipOutbreakDLC2
        // 
        toolTipOutbreakDLC2.AutoPopDelay = 10000;
        toolTipOutbreakDLC2.InitialDelay = 500;
        toolTipOutbreakDLC2.IsBalloon = true;
        toolTipOutbreakDLC2.ReshowDelay = 100;
        toolTipOutbreakDLC2.ToolTipTitle = "Mass Outbreaks Data";
        // 
        // ConnectionForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(425, 359);
        Controls.Add(chkOutbreaksDLC2);
        Controls.Add(chkOutbreaksDLC);
        Controls.Add(progressBar);
        Controls.Add(chkOutbreaksMain);
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
    private ToolTip toolTipOutbreakMain;
    private Button btnDisconnect;
    private CheckBox chkOutbreaksMain;
    private ProgressBar progressBar;
    private CheckBox chkOutbreaksDLC;
    private ToolTip toolTipOutbreakDLC1;
    private CheckBox chkOutbreaksDLC2;
    private ToolTip toolTipOutbreakDLC2;
}