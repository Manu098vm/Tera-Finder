using PKHeX.Core;
using SysBot.Base;
using TeraFinder.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace TeraFinder.Forms
{
    public partial class ConnectionForm : Form
    {
        public DeviceExecutor Executor = null!;

        private SAV9SV SAV = null!;
        private bool Connected = false;

        public ConnectionForm(SAV9SV sav)
        {
            InitializeComponent();
            SAV = sav;
            txtAddress.Text = Settings.Default.address;
            numPort.Value = Settings.Default.port;
            if (Settings.Default.protocol)
                radioUSB.Checked = true;
            toolTip.SetToolTip(chkEventData, $"Syncronize event data from the remote device. Might require a sgnificant amount of time.");
        }

        public bool IsConnected() => Connected;

        public void Disconnect()
        {
            try { Executor.Disconnect(); }
            catch { }
            Connected = false;
            EnableConnectButton("Connect");
            DisableDisconnectButton();
            EnableGrpDevice();
            EnableCheckEvent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            Connected = false;
            DisableConnectButton();
            DisableDisconnectButton();
            DisableCheckEvent();
            DisableGrpDevice();
            var token = new CancellationToken();

            try
            {
                var config = GetProtocol() switch
                {
                    SwitchProtocol.USB => new SwitchConnectionConfig { Port = (int)numPort.Value, Protocol = SwitchProtocol.USB },
                    SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = txtAddress.Text, Port = (int)numPort.Value, Protocol = SwitchProtocol.WiFi },
                    _ => throw new ArgumentOutOfRangeException("No valid protocol"),
                };
                var state = new DeviceState
                {
                    Connection = config,
                    InitialRoutine = RoutineType.ReadWrite,
                };
                Executor = new DeviceExecutor(state);
                await Executor.RunAsync(token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Disconnect();
                MessageBox.Show($"{ex.Message}");
                return;
            }

            Connected = true;
            Settings.Default.address = txtAddress.Text;
            Settings.Default.port = (int)numPort.Value;
            Settings.Default.protocol = GetProtocol() switch
            {
                SwitchProtocol.USB => true,
                _ => false,
            };
            Settings.Default.Save();

            try
            {
                await Executor.Connect(token).ConfigureAwait(false);
                var version = await Executor.ReadGameVersion(token).ConfigureAwait(false);
                SAV.Game = (int)version;
                var mystatusblock = SAV.Accessor.FindOrDefault(Blocks.KMyStatus.Key);
                mystatusblock.ChangeData(await Executor.ReadEncryptedBlock(Blocks.KMyStatus, token).ConfigureAwait(false));
                var raidblock = SAV.Accessor.FindOrDefault(Blocks.KTeraRaids.Key);
                raidblock.ChangeData(await Executor.ReadDecryptedBlock(Blocks.KTeraRaids, raidblock.Data.Length, token).ConfigureAwait(false));
                var progress = await Executor.ReadGameProgress(token).ConfigureAwait(false);
                ProgressForm.EditProgress(SAV, progress);

                if (chkEventData.Checked)
                    await DownloadEventData().ConfigureAwait(false);

                MessageBox.Show("Successfully connected.");
                Log($"Executor succesfully connected: {version} - {SAV.OT} ({SAV.TrainerID7}) [{progress}]");
                EnableConnectButton();
                EnableDisconnectButton();
                EnableGrpDevice();
                EnableCheckEvent();
                SafeClose();
            }
            catch (Exception ex)
            {
                Disconnect();
                MessageBox.Show($"Could not connect.\n{ex.Message}");
                return;
            }
        }

        private async Task DownloadEventData()
        {
            var token = new CancellationToken();
            var KBCATEventRaidIdentifier = SAV.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);

            if (KBCATEventRaidIdentifier.Type is SCTypeCode.None)
                BlockUtil.EditBlock(KBCATEventRaidIdentifier, SCTypeCode.Object, BitConverter.GetBytes((uint)1));

            var KBCATFixedRewardItemArray = SAV.Accessor.FindOrDefault(Blocks.KBCATFixedRewardItemArray.Key);
            var rewardItemBlock = await Executor.ReadEncryptedBlock(Blocks.KBCATFixedRewardItemArray, token).ConfigureAwait(false);

            if (KBCATFixedRewardItemArray.Type is not SCTypeCode.None)
                KBCATFixedRewardItemArray.ChangeData(rewardItemBlock);
            else
                BlockUtil.EditBlock(KBCATFixedRewardItemArray, SCTypeCode.Object, rewardItemBlock);

            var KBCATLotteryRewardItemArray = SAV.Accessor.FindOrDefault(Blocks.KBCATLotteryRewardItemArray.Key);
            var lotteryItemBlock = await Executor.ReadEncryptedBlock(Blocks.KBCATLotteryRewardItemArray, token).ConfigureAwait(false);

            if (KBCATLotteryRewardItemArray.Type is not SCTypeCode.None)
                KBCATLotteryRewardItemArray.ChangeData(lotteryItemBlock);
            else
                BlockUtil.EditBlock(KBCATLotteryRewardItemArray, SCTypeCode.Object, lotteryItemBlock);

            var KBCATRaidEnemyArray = SAV.Accessor.FindOrDefault(Blocks.KBCATRaidEnemyArray.Key);
            var raidEnemyBlock = await Executor.ReadEncryptedBlock(Blocks.KBCATRaidEnemyArray, token).ConfigureAwait(false);

            if (KBCATRaidEnemyArray.Type is not SCTypeCode.None)
                KBCATRaidEnemyArray.ChangeData(raidEnemyBlock);
            else
                BlockUtil.EditBlock(KBCATRaidEnemyArray, SCTypeCode.Object, raidEnemyBlock);
        }

        private SwitchProtocol GetProtocol()
        {
            if (radioUSB.Checked)
                return SwitchProtocol.USB;
            return SwitchProtocol.WiFi;
        }

        private void DisableConnectButton()
        {
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(() => { btnConnect.Text = "Connecting..."; });
                btnConnect.Invoke(() => { btnConnect.Enabled = false; });
            }
            else
            {
                btnConnect.Text = "Connecting...";
                btnConnect.Enabled = false;
            }
        }

        private void EnableConnectButton(string text = "Connected")
        {
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(() => { btnConnect.Text = text; });
                btnConnect.Invoke(() => { btnConnect.Enabled = true; });
            }
            else
            {
                btnConnect.Text = text;
                btnConnect.Enabled = true;
            }
        }

        private void EnableDisconnectButton()
        {
            if (btnDisconnect.InvokeRequired)
            {
                btnDisconnect.Invoke(() => { btnDisconnect.Enabled = true; });
            }
            else
            {
                btnDisconnect.Enabled = true;
            }
        }

        private void DisableDisconnectButton()
        {
            if (btnDisconnect.InvokeRequired)
            {
                btnDisconnect.Invoke(() => { btnDisconnect.Enabled = false; });
            }
            else
            {
                btnDisconnect.Enabled = false;
            }
        }

        private void EnableGrpDevice()
        {
            if (grpDevice.InvokeRequired)
            {
                grpDevice.Invoke(() => { grpDevice.Enabled = true; });
            }
            else
            {
                grpDevice.Enabled = true;
            }
        }

        private void DisableGrpDevice()
        {
            if(grpDevice.InvokeRequired)
            {
                grpDevice.Invoke(() => { grpDevice.Enabled = false; });
            }
            else
            {
                grpDevice.Enabled = false;
            }
        }

        private void EnableCheckEvent()
        {
            if(chkEventData.InvokeRequired)
            {
                chkEventData.Invoke(() => { chkEventData.Enabled = true; });
            }
            else
            {
                chkEventData.Enabled = true;
            }
        }

        private void DisableCheckEvent()
        {
            if (chkEventData.InvokeRequired)
            {
                chkEventData.Invoke(() => { chkEventData.Enabled = false; });
            }
            else
            {
                chkEventData.Enabled = false;
            }
        }

        private void SafeClose()
        {
            if (InvokeRequired)
            {
                Invoke(() => { Close(); });
            }
            else
            {
                Close();
            }
        }

        private void radioWiFi_CheckedChanged(object sender, EventArgs e)
        {
            if (radioWiFi.Checked)
                txtAddress.Enabled = true;
        }

        private void radioUSB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioUSB.Checked)
                txtAddress.Enabled = false;
        }

        private void Log(string message)
        {
            if (Connected)
                Executor.Log(message);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisableConnectButton();
            DisableDisconnectButton();
            DisableGrpDevice();
            DisableCheckEvent();
            Disconnect();
            MessageBox.Show("Device disconnected.");
        }
    }
}