﻿using PKHeX.Core;
using SysBot.Base;
using TeraFinder.Properties;

namespace TeraFinder.Forms
{
    public partial class ConnectionForm : Form
    {
        public DeviceExecutor Executor = null!;

        private SAV9SV SAV = null!;
        private bool Connected = false;

        private Dictionary<string, string> Strings = null!;

        public ConnectionForm(SAV9SV sav, string language)
        {
            InitializeComponent();
            GenerateDictionary();
            TranslateDictionary(language);
            SAV = sav;
            txtAddress.Text = Settings.Default.address;
            numPort.Value = Settings.Default.port;
            if (Settings.Default.protocol)
                radioUSB.Checked = true;

            toolTip.SetToolTip(chkOutbreaks, Strings["ToolTipSyncOutbreaks"]);
        }

        private void GenerateDictionary()
        {
            Strings = new Dictionary<string, string>
            {
                { "ActionConnect", "Connect" },
                { "ActionConnecting", "Connecting..." },
                { "ActionConnected", "Connected" },
                { "NoProtocol", "No valid protocol" },
                { "ConnectionSuccess", "Successfully connected." },
                { "ConnectionFailed", "Could not connect." },
                { "ConnectionFailedAux", "Please try saving in-game and reboot the game." },
                { "ExecutorConnected", "Executor succesfully connected:" },
                { "DisconnectionSuccess", "Device disconnected." },
                { "ToolTipSyncOutbreaks", "Syncronize Outbreaks data from the remote device. Might require a sgnificant amount of time." }
            };
        }

        private void TranslateDictionary(string language) => Strings = Strings.TranslateInnerStrings(language);

        public bool IsConnected() => Connected;

        public void Disconnect()
        {
            try { Executor.Disconnect(); }
            catch { }
            Connected = false;
            EnableConnectButton(Strings["ActionConnect"]);
            DisableDisconnectButton();
            EnableGrpDevice();
            EnableCheckBox();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            Connected = false;
            DisableConnectButton();
            DisableDisconnectButton();
            DisableGrpDevice();
            DisableCheckBox();
            var token = new CancellationToken();

            try
            {
                var config = GetProtocol() switch
                {
                    SwitchProtocol.USB => new SwitchConnectionConfig { Port = (int)numPort.Value, Protocol = SwitchProtocol.USB },
                    SwitchProtocol.WiFi => new SwitchConnectionConfig { IP = txtAddress.Text, Port = (int)numPort.Value, Protocol = SwitchProtocol.WiFi },
                    _ => throw new ArgumentOutOfRangeException(Strings["NoProtocol"]),
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
                mystatusblock.ChangeData((byte[]?)await Executor.ReadBlock(Blocks.KMyStatus, token).ConfigureAwait(false));
                var raidblock = SAV.Accessor.FindOrDefault(Blocks.KTeraRaids.Key);
                raidblock.ChangeData((byte[]?)await Executor.ReadBlock(Blocks.KTeraRaids, token).ConfigureAwait(false));
                var progress = await Executor.ReadGameProgress(token).ConfigureAwait(false);
                ProgressForm.EditProgress(SAV, progress);
                await DownloadEventData(token).ConfigureAwait(false);
                if (chkOutbreaks.Checked) await DownloadOutbreakData(token).ConfigureAwait(false);

                MessageBox.Show(Strings["ConnectionSuccess"]);
                Log($"{Strings["ExecutorConnected"]} {version} - {SAV.OT} ({SAV.TrainerTID7}) [{progress}]");
                EnableConnectButton(Strings["ActionConnected"]);
                EnableDisconnectButton();
                EnableCheckBox();
                EnableGrpDevice();
                SafeClose();
            }
            catch (Exception ex)
            {
                Disconnect();
                MessageBox.Show($"{Strings["ConnectionFailed"]}\n{Strings["ConnectionFailedAux"]}\n{ex.Message}");
                return;
            }
        }

        private async Task DownloadEventData(CancellationToken token)
        {
            var KBCATEventRaidIdentifier = SAV.Accessor.FindOrDefault(Blocks.KBCATEventRaidIdentifier.Key);
            var raidIdentifierBlock = (byte[]?)await Executor.ReadBlock(Blocks.KBCATEventRaidIdentifier, token).ConfigureAwait(false);

            if (KBCATEventRaidIdentifier.Type is not SCTypeCode.None)
                KBCATEventRaidIdentifier.ChangeData(raidIdentifierBlock);
            else
                BlockUtil.EditBlock(KBCATEventRaidIdentifier, SCTypeCode.Object, raidIdentifierBlock);

            var KBCATFixedRewardItemArray = SAV.Accessor.FindOrDefault(Blocks.KBCATFixedRewardItemArray.Key);
            var rewardItemBlock = (byte[]?)await Executor.ReadBlock(Blocks.KBCATFixedRewardItemArray, token).ConfigureAwait(false);

            if (KBCATFixedRewardItemArray.Type is not SCTypeCode.None)
                KBCATFixedRewardItemArray.ChangeData(rewardItemBlock);
            else
                BlockUtil.EditBlock(KBCATFixedRewardItemArray, SCTypeCode.Object, rewardItemBlock);

            var KBCATLotteryRewardItemArray = SAV.Accessor.FindOrDefault(Blocks.KBCATLotteryRewardItemArray.Key);
            var lotteryItemBlock = (byte[]?)await Executor.ReadBlock(Blocks.KBCATLotteryRewardItemArray, token).ConfigureAwait(false);

            if (KBCATLotteryRewardItemArray.Type is not SCTypeCode.None)
                KBCATLotteryRewardItemArray.ChangeData(lotteryItemBlock);
            else
                BlockUtil.EditBlock(KBCATLotteryRewardItemArray, SCTypeCode.Object, lotteryItemBlock);

            var KBCATRaidEnemyArray = SAV.Accessor.FindOrDefault(Blocks.KBCATRaidEnemyArray.Key);
            var raidEnemyBlock = (byte[]?)await Executor.ReadBlock(Blocks.KBCATRaidEnemyArray, token).ConfigureAwait(false);

            if (KBCATRaidEnemyArray.Type is not SCTypeCode.None)
                KBCATRaidEnemyArray.ChangeData(raidEnemyBlock);
            else
                BlockUtil.EditBlock(KBCATRaidEnemyArray, SCTypeCode.Object, raidEnemyBlock);

            var KBCATRaidPriorityArray = SAV.Accessor.FindOrDefault(Blocks.KBCATRaidPriorityArray.Key);
            var raidPriorityBlock = (byte[]?)await Executor.ReadBlock(Blocks.KBCATRaidPriorityArray, token).ConfigureAwait(false);

            if (KBCATRaidPriorityArray.Type is not SCTypeCode.None)
                KBCATRaidPriorityArray.ChangeData(raidPriorityBlock);
            else
                BlockUtil.EditBlock(KBCATRaidPriorityArray, SCTypeCode.Object, raidPriorityBlock);
        }

        private async Task DownloadOutbreakData(CancellationToken token)
        {
            var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(Blocks.KMassOutbreakAmount.Key);
            var KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(Blocks.KMassOutbreakAmount, token).ConfigureAwait(false);

            if (KMassOutbreakAmount.Type is not SCTypeCode.None)
                KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
            else
                BlockUtil.EditBlock(KMassOutbreakAmount, Blocks.KMassOutbreakAmount.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

            for (var i = 1; i <= 8; i++)
            {
                var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}CenterPos")!.GetValue(new DataBlock())!;
                var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

                if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                    KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
                else
                    BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}DummyPos")!.GetValue(new DataBlock())!;
                var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

                if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                    KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
                else
                    BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}Species")!.GetValue(new DataBlock())!;
                var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

                if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                    KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
                else
                    BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}Form")!.GetValue(new DataBlock())!;
                var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

                if (KMassOutbreakForm.Type is not SCTypeCode.None)
                    KMassOutbreakForm.SetValue(KMassOutbreakFormData);
                else
                    BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}Found")!.GetValue(new DataBlock())!;
                var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

                if (KMassOutbreakFound.Type is not SCTypeCode.None)
                    KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                else
                {
                    BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                    BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
                }

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}NumKOed")!.GetValue(new DataBlock())!;
                var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

                if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                    KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
                else
                    BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

                blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{i}TotalSpawns")!.GetValue(new DataBlock())!;
                var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
                var KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

                if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                    KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
                else
                    BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);
            }
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
                btnConnect.Invoke(() => { btnConnect.Text = Strings["ActionConnecting"]; });
                btnConnect.Invoke(() => { btnConnect.Enabled = false; });
            }
            else
            {
                btnConnect.Text = Strings["ActionConnecting"];
                btnConnect.Enabled = false;
            }
        }

        private void EnableConnectButton(string text)
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

        private void DisableCheckBox()
        {
            if (chkOutbreaks.InvokeRequired)
            {
                chkOutbreaks.Invoke(() => { chkOutbreaks.Enabled = false; });
            }
            else
            {
                chkOutbreaks.Enabled = false;
            }
        }

        private void EnableCheckBox()
        {
            if (chkOutbreaks.InvokeRequired)
            {
                chkOutbreaks.Invoke(() => { chkOutbreaks.Enabled = true; });
            }
            else
            {
                chkOutbreaks.Enabled = true;
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
            if (grpDevice.InvokeRequired)
            {
                grpDevice.Invoke(() => { grpDevice.Enabled = false; });
            }
            else
            {
                grpDevice.Enabled = false;
            }
        }

        private void SafeClose()
        {
            if (InvokeRequired)
            {
                Invoke(Close);
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
            Disconnect();
            MessageBox.Show(Strings["DisconnectionSuccess"]);
        }
    }
}