using PKHeX.Core;
using SysBot.Base;
using TeraFinder.Core;
using TeraFinder.RemoteExecutor;
using TeraFinder.Plugins.Properties;
using System.Buffers.Binary;

namespace TeraFinder.Plugins;

public partial class ConnectionForm : Form
{
    public DeviceExecutor Executor = null!;

    private readonly SAV9SV SAV = null!;
    private bool Connected = false;
    private int CurrentProgress = 0;
    private int MaxProgress = 0;

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

        toolTipOutbreakMain.SetToolTip(chkOutbreaksMain, Strings["ToolTipSyncOutbreaksMain"]);
        toolTipOutbreakDLC1.SetToolTip(chkOutbreaksDLC, Strings["ToolTipSyncOutbreaksDLC1"]);
        toolTipOutbreakDLC2.SetToolTip(chkOutbreaksDLC2, Strings["ToolTipSyncOutbreaksDLC2"]);
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
            { "ToolTipSyncOutbreaksMain", "Syncronize Paldea Outbreaks data from the remote device. Might require a sgnificant amount of time." },
            { "ToolTipSyncOutbreaksDLC1", "Syncronize Kitakami Outbreaks data from the remote device. Might require a sgnificant amount of time." },
            { "ToolTipSyncOutbreaksDLC2", "Syncronize Blueberry Outbreaks data from the remote device. Might require a sgnificant amount of time." },
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
        const int ConnectionStepsRaids = 10;
        const int ConnectionStepsOutbreaksMain = 128;
        const int ConnectionStepsOutbreakDLC = 100;
        const int ConnectionStepsOutbreakDLC2 = 107;

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

       // try
       // {
            MaxProgress = ConnectionStepsRaids;
            if (chkOutbreaksMain.Checked) MaxProgress += ConnectionStepsOutbreaksMain;
            if (chkOutbreaksDLC.Checked) MaxProgress += ConnectionStepsOutbreakDLC;
            if (chkOutbreaksDLC2.Checked) MaxProgress += ConnectionStepsOutbreakDLC2;
            CurrentProgress = 0;

            await Executor.Connect(token).ConfigureAwait(false);
            UpdateProgress(CurrentProgress++, MaxProgress);
            var version = await Executor.ReadGame(token).ConfigureAwait(false);
            UpdateProgress(CurrentProgress++, MaxProgress);
            SAV.Version = version;
            var mystatusblock = SAV.Accessor.FindOrDefault(BlockDefinitions.KMyStatus.Key);
            mystatusblock.ChangeData((byte[]?)await Executor.ReadBlock(BlockDefinitions.KMyStatus, token).ConfigureAwait(false));
            UpdateProgress(CurrentProgress++, MaxProgress);
            var raidpaldeablock = SAV.Accessor.FindOrDefault(BlockDefinitions.KTeraRaidPaldea.Key);
            raidpaldeablock.ChangeData((byte[]?)await Executor.ReadBlock(BlockDefinitions.KTeraRaidPaldea, token).ConfigureAwait(false));
            var raidkitakamiblock = SAV.Accessor.FindOrDefault(BlockDefinitions.KTeraRaidDLC.Key);
            raidkitakamiblock.ChangeData((byte[]?)await Executor.ReadBlock(BlockDefinitions.KTeraRaidDLC, token).ConfigureAwait(false));
            UpdateProgress(CurrentProgress++, MaxProgress);
            var progress = await Executor.ReadGameProgress(token).ConfigureAwait(false);
            UpdateProgress(CurrentProgress++, MaxProgress);
            ProgressForm.EditProgress(SAV, progress);
            await DownloadEventData(token).ConfigureAwait(false);
            var raidSevenStar = SAV.Accessor.FindOrDefault(BlockDefinitions.KSevenStarRaidsCapture.Key);
            raidSevenStar.ChangeData((byte[]?)await Executor.ReadBlock(BlockDefinitions.KSevenStarRaidsCapture, token).ConfigureAwait(false));
            UpdateProgress(CurrentProgress++, MaxProgress);
            if (chkOutbreaksMain.Checked){ await DownloadOutbreaksMainData(token).ConfigureAwait(false); await DownloadOutbreaksBCMainData(token).ConfigureAwait(false); }
            if (chkOutbreaksDLC.Checked) { await DownloadOutbreaksDLCData(token).ConfigureAwait(false); await DownloadEventOutbreaksDLCData(token).ConfigureAwait(false); }
            if (chkOutbreaksDLC2.Checked){ await DownloadOutbreaksDLC2Data(token).ConfigureAwait(false); await DownloadEventOutbreaksDLC2Data(token).ConfigureAwait(false);  }

            UpdateProgress(MaxProgress, MaxProgress);
            MessageBox.Show(Strings["ConnectionSuccess"]);
            Log($"{Strings["ExecutorConnected"]} {version} - {SAV.OT} ({SAV.TrainerTID7}) [{progress}]");
            EnableConnectButton(Strings["ActionConnected"]);
            EnableDisconnectButton();
            EnableCheckBox();
            EnableGrpDevice();
            SafeClose();
        //}
       /* catch (Exception ex)
        {
            Disconnect();
            MessageBox.Show($"{Strings["ConnectionFailed"]}\n{Strings["ConnectionFailedAux"]}\n{ex.Message}");
            return;
        }*/
    }

    private async Task DownloadEventData(CancellationToken token)
    {
        var KBCATEventRaidIdentifier = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATEventRaidIdentifier.Key);
        var raidIdentifierBlock = (byte[]?)await Executor.ReadBlock(BlockDefinitions.KBCATEventRaidIdentifier, token).ConfigureAwait(false);
        var identifier = BinaryPrimitives.ReadUInt32LittleEndian(raidIdentifierBlock);

        if (KBCATEventRaidIdentifier.Type is not SCTypeCode.None)
            KBCATEventRaidIdentifier.ChangeData(raidIdentifierBlock);
        else
            BlockUtil.EditBlock(KBCATEventRaidIdentifier, SCTypeCode.Object, raidIdentifierBlock);

        UpdateProgress(CurrentProgress++, MaxProgress);


        if (identifier > 0)
        {
            var KBCATFixedRewardItemArray = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATFixedRewardItemArray.Key);
            var rewardItemBlock = (byte[]?)await Executor.ReadBlock(BlockDefinitions.KBCATFixedRewardItemArray, token).ConfigureAwait(false);

            if (KBCATFixedRewardItemArray.Type is not SCTypeCode.None)
                KBCATFixedRewardItemArray.ChangeData(rewardItemBlock);
            else
                BlockUtil.EditBlock(KBCATFixedRewardItemArray, SCTypeCode.Object, rewardItemBlock);
        }

        UpdateProgress(CurrentProgress++, MaxProgress);

        if (identifier > 0)
        {
            var KBCATLotteryRewardItemArray = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATLotteryRewardItemArray.Key);
            var lotteryItemBlock = (byte[]?)await Executor.ReadBlock(BlockDefinitions.KBCATLotteryRewardItemArray, token).ConfigureAwait(false);

            if (KBCATLotteryRewardItemArray.Type is not SCTypeCode.None)
                KBCATLotteryRewardItemArray.ChangeData(lotteryItemBlock);
            else
                BlockUtil.EditBlock(KBCATLotteryRewardItemArray, SCTypeCode.Object, lotteryItemBlock);
        }

        UpdateProgress(CurrentProgress++, MaxProgress);

        if (identifier > 0)
        {
            var KBCATRaidEnemyArray = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATRaidEnemyArray.Key);
            var raidEnemyBlock = (byte[]?)await Executor.ReadBlock(BlockDefinitions.KBCATRaidEnemyArray, token).ConfigureAwait(false);

            if (KBCATRaidEnemyArray.Type is not SCTypeCode.None)
                KBCATRaidEnemyArray.ChangeData(raidEnemyBlock);
            else
                BlockUtil.EditBlock(KBCATRaidEnemyArray, SCTypeCode.Object, raidEnemyBlock);
        }

        UpdateProgress(CurrentProgress++, MaxProgress);

        if (identifier > 0)
        {
            var KBCATRaidPriorityArray = SAV.Accessor.FindOrDefault(BlockDefinitions.KBCATRaidPriorityArray.Key);
            var raidPriorityBlock = (byte[]?)await Executor.ReadBlock(BlockDefinitions.KBCATRaidPriorityArray, token).ConfigureAwait(false);

            if (KBCATRaidPriorityArray.Type is not SCTypeCode.None)
                KBCATRaidPriorityArray.ChangeData(raidPriorityBlock);
            else
                BlockUtil.EditBlock(KBCATRaidPriorityArray, SCTypeCode.Object, raidPriorityBlock);
        }

        UpdateProgress(CurrentProgress++, MaxProgress);
    }


    private async Task DownloadOutbreaksMainData(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakMainNumActive.Key);
        var KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakMainNumActive, token).ConfigureAwait(false);

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakMainNumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 8; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainCenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainDummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainSpecies")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainForm")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainFound")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainNumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}MainTotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private async Task DownloadOutbreaksBCMainData(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakBCMainNumActive.Key);
        var KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakBCMainNumActive, token).ConfigureAwait(false);

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakBCMainNumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 10; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainCenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainDummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false);

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainSpecies")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainForm")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainFound")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainNumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}MainTotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            var KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!;

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private async Task DownloadOutbreaksDLCData(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakDLC1NumActive.Key);
        byte? KMassOutbreakAmountData;
        try { KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakDLC1NumActive, token).ConfigureAwait(false); }
        catch (ArgumentOutOfRangeException) { KMassOutbreakAmountData = 0; }

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakDLC1NumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 4; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1CenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakCenterPosData;
            try { KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakCenterPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1DummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakDummyPosData;
            try { KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakDummyPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1Species")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            uint KMassOutbreakSpeciesData;
            try { KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakSpeciesData = 0; }

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1Form")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte KMassOutbreakFormData;
            try { KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFormData = 0; }

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1Found")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            bool KMassOutbreakFoundData;
            try { KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFoundData = false; }

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1NumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakNumKOedData;
            try { KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakNumKOedData = 0; }

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC1TotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakTotalSpawnsData;
            try { KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakTotalSpawnsData = 0; }

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private async Task DownloadEventOutbreaksDLCData(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakBCDLC1NumActive.Key);
        byte? KMassOutbreakAmountData;
        try { KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakBCDLC1NumActive, token).ConfigureAwait(false); }
        catch (ArgumentOutOfRangeException) { KMassOutbreakAmountData = 0; }

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakBCDLC1NumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 10; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1CenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakCenterPosData;
            try { KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakCenterPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1DummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakDummyPosData;
            try { KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakDummyPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1Species")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            uint KMassOutbreakSpeciesData;
            try { KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakSpeciesData = 0; }

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1Form")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte KMassOutbreakFormData;
            try { KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFormData = 0; }

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1Found")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            bool KMassOutbreakFoundData;
            try { KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFoundData = false; }

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1NumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakNumKOedData;
            try { KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakNumKOedData = 0; }

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC1TotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakTotalSpawnsData;
            try { KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakTotalSpawnsData = 0; }

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private async Task DownloadOutbreaksDLC2Data(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakDLC2NumActive.Key);
        byte? KMassOutbreakAmountData;
        try { KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakDLC2NumActive, token).ConfigureAwait(false); }
        catch (ArgumentOutOfRangeException) { KMassOutbreakAmountData = 0; }

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakDLC2NumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 5; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2CenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakCenterPosData;
            try { KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakCenterPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2DummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakDummyPosData;
            try { KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakDummyPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2Species")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            uint KMassOutbreakSpeciesData;
            try { KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakSpeciesData = 0; }

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2Form")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte KMassOutbreakFormData;
            try { KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFormData = 0; }

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2Found")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            bool KMassOutbreakFoundData;
            try { KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFoundData = false; }

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2NumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakNumKOedData;
            try { KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakNumKOedData = 0; }

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreak0{i}DLC2TotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakTotalSpawnsData;
            try { KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakTotalSpawnsData = 0; }

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private async Task DownloadEventOutbreaksDLC2Data(CancellationToken token)
    {
        var KMassOutbreakAmount = SAV.Accessor.FindOrDefault(BlockDefinitions.KOutbreakBCDLC2NumActive.Key);
        byte? KMassOutbreakAmountData;
        try { KMassOutbreakAmountData = (byte?)await Executor.ReadBlock(BlockDefinitions.KOutbreakBCDLC2NumActive, token).ConfigureAwait(false); }
        catch (ArgumentOutOfRangeException) { KMassOutbreakAmountData = 0; }

        if (KMassOutbreakAmount.Type is not SCTypeCode.None)
            KMassOutbreakAmount.ChangeData((new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());
        else
            BlockUtil.EditBlock(KMassOutbreakAmount, BlockDefinitions.KOutbreakBCDLC2NumActive.Type, (new byte[] { (byte)KMassOutbreakAmountData! }).AsSpan());

        UpdateProgress(CurrentProgress++, MaxProgress);

        for (var i = 1; i <= 10; i++)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2CenterPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakCenterPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakCenterPosData;
            try { KMassOutbreakCenterPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakCenterPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakCenterPos.Type is not SCTypeCode.None)
                KMassOutbreakCenterPos.ChangeData(KMassOutbreakCenterPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakCenterPos, blockInfo.Type, KMassOutbreakCenterPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2DummyPos")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakDummyPos = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte[]? KMassOutbreakDummyPosData;
            try { KMassOutbreakDummyPosData = (byte[]?)await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false); }
            catch (ArgumentOutOfRangeException) { KMassOutbreakDummyPosData = new byte[blockInfo.Size]; }

            if (KMassOutbreakDummyPos.Type is not SCTypeCode.None)
                KMassOutbreakDummyPos.ChangeData(KMassOutbreakDummyPosData);
            else
                BlockUtil.EditBlock(KMassOutbreakDummyPos, blockInfo.Type, KMassOutbreakDummyPosData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2Species")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakSpecies = SAV.Accessor.FindOrDefault(blockInfo.Key);
            uint KMassOutbreakSpeciesData;
            try { KMassOutbreakSpeciesData = (uint)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakSpeciesData = 0; }

            if (KMassOutbreakSpecies.Type is not SCTypeCode.None)
                KMassOutbreakSpecies.SetValue(KMassOutbreakSpeciesData);
            else
                BlockUtil.EditBlock(KMassOutbreakSpecies, blockInfo.Type, KMassOutbreakSpeciesData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2Form")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakForm = SAV.Accessor.FindOrDefault(blockInfo.Key);
            byte KMassOutbreakFormData;
            try { KMassOutbreakFormData = (byte)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFormData = 0; }

            if (KMassOutbreakForm.Type is not SCTypeCode.None)
                KMassOutbreakForm.SetValue(KMassOutbreakFormData);
            else
                BlockUtil.EditBlock(KMassOutbreakForm, blockInfo.Type, KMassOutbreakFormData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2Found")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakFound = SAV.Accessor.FindOrDefault(blockInfo.Key);
            bool KMassOutbreakFoundData;
            try { KMassOutbreakFoundData = (bool)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakFoundData = false; }

            if (KMassOutbreakFound.Type is not SCTypeCode.None)
                KMassOutbreakFound.ChangeBooleanType(KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
            else
            {
                BlockUtil.EditBlockType(KMassOutbreakFound, KMassOutbreakFoundData ? SCTypeCode.Bool2 : SCTypeCode.Bool1);
                BlockUtil.AddBlockToFakeSAV(SAV, KMassOutbreakFound);
            }

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2NumKOed")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakNumKOed = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakNumKOedData;
            try { KMassOutbreakNumKOedData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakNumKOedData = 0; }

            if (KMassOutbreakNumKOed.Type is not SCTypeCode.None)
                KMassOutbreakNumKOed.SetValue(KMassOutbreakNumKOedData);
            else
                BlockUtil.EditBlock(KMassOutbreakNumKOed, blockInfo.Type, KMassOutbreakNumKOedData);

            UpdateProgress(CurrentProgress++, MaxProgress);

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{i:00}DLC2TotalSpawns")!.GetValue(new BlockDefinition())!;
            var KMassOutbreakTotalSpawns = SAV.Accessor.FindOrDefault(blockInfo.Key);
            int KMassOutbreakTotalSpawnsData;
            try { KMassOutbreakTotalSpawnsData = (int)(await Executor.ReadBlock(blockInfo, token).ConfigureAwait(false))!; }
            catch (ArgumentOutOfRangeException) { KMassOutbreakTotalSpawnsData = 0; }

            if (KMassOutbreakTotalSpawns.Type is not SCTypeCode.None)
                KMassOutbreakTotalSpawns.SetValue(KMassOutbreakTotalSpawnsData);
            else
                BlockUtil.EditBlock(KMassOutbreakTotalSpawns, blockInfo.Type, KMassOutbreakTotalSpawnsData);

            UpdateProgress(CurrentProgress++, MaxProgress);
        }
    }
    private void UpdateProgress(int currProgress, int maxProgress)
    {
        var value = (100 * currProgress) / maxProgress;
        if (progressBar.InvokeRequired)
            progressBar.Invoke(() => progressBar.Value = value);
        else if(value > 100)
            progressBar.Value = 100;
        else 
            progressBar.Value = value;
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
        if (chkOutbreaksMain.InvokeRequired)
            chkOutbreaksMain.Invoke(() => { chkOutbreaksMain.Enabled = false; });
        else chkOutbreaksMain.Enabled = false;
        if (chkOutbreaksDLC.InvokeRequired)
            chkOutbreaksDLC.Invoke(() => { chkOutbreaksDLC.Enabled = false; });
        else chkOutbreaksDLC.Enabled = false;
        if (chkOutbreaksDLC2.InvokeRequired)
            chkOutbreaksDLC2.Invoke(() => { chkOutbreaksDLC2.Enabled = false; });
        else chkOutbreaksDLC2.Enabled = false;
    }

    private void EnableCheckBox()
    {
        if (chkOutbreaksMain.InvokeRequired)
            chkOutbreaksMain.Invoke(() => { chkOutbreaksMain.Enabled = true; });
        else chkOutbreaksMain.Enabled = true;
        if (chkOutbreaksDLC.InvokeRequired)
            chkOutbreaksDLC.Invoke(() => { chkOutbreaksDLC.Enabled = true; });
        else chkOutbreaksDLC.Enabled = true;
        if (chkOutbreaksDLC2.InvokeRequired)
            chkOutbreaksDLC2.Invoke(() => { chkOutbreaksDLC2.Enabled = true; });
        else chkOutbreaksDLC2.Enabled = true;
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
        UpdateProgress(0, 100);
    }
}