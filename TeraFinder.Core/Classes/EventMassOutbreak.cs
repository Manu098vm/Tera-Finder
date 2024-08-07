using PKHeX.Core;
using System.Text.Json.Nodes;
using System.Text.Json;
using TeraFinder.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder.Core
{
    public class EventMassOutbreak : IOutbreak
    {
        public int ID { get; }
        public SAV9SV SAV { get; init; }
        protected sbyte AmountAvailable { get => GetAmountAvailable(); set => SetAmountAvailable(value); }
        public string LocationMap { get; set; }

        public GameCoordinates? LocationCenter { get; set; }
        public GameCoordinates? LocationDummy { get; set; }
        public bool Found { get => GetFound(); set => SetFound(value); }
        public bool Enabled { get => GetEnabled(); set => SetEnabled(value); }
        public uint Species { get => GetSpecies(); set => SetSpecies(value); }
        public byte Form { get => GetForm(); set => SetForm(value); }
        public int NumKO { get => GetNumKO(); set => SetNumKO(value); }
        public int MaxSpawns { get => GetMaxSpawns(); set => SetMaxSpawns(value); }

        public EventMassOutbreak(SAV9SV sav, int id, TeraRaidMapParent map)
        {
            ID = id;
            SAV = sav;
            LocationMap = map switch { TeraRaidMapParent.Kitakami => "DLC1", TeraRaidMapParent.Blueberry => "DLC2", _ => "Main" };

            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}CenterPos")!.GetValue(new BlockDefinition())!;
            var block = sav.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Array)
                LocationCenter = new GameCoordinates(block);
            else
                LocationCenter = null;

            blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}DummyPos")!.GetValue(new BlockDefinition())!;
            block = sav.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Array)
                LocationDummy = new GameCoordinates(block);
            else
                LocationDummy = null;
        }


        public FakeOutbreak Clone() => new(this);

        public void DumpTojson(string path)
        {
            if (LocationCenter is not null && LocationDummy is not null)
            {
                var json = "{\n" +
                    "\t\"LocationCenter\": \"" + BitConverter.ToString([.. LocationCenter.GetCoordinates()]).Replace("-", string.Empty) + "\",\n" +
                    "\t\"LocationDummy\": \"" + BitConverter.ToString([.. LocationDummy.GetCoordinates()]).Replace("-", string.Empty) + "\",\n" +
                    "\t\"Species\": " + SpeciesConverter.GetNational9((ushort)Species) + ",\n" +
                    "\t\"Form\": " + Form + ",\n" +
                    "\t\"MaxSpawns\": " + MaxSpawns + "\n" +
                "}";

                File.WriteAllText(path, json);
            }
        }

        public void RestoreFromJson(string json)
        {
            var simpleOutbreak = JsonSerializer.Deserialize<JsonNode>(json)!;
            var locationCenter = Convert.FromHexString(simpleOutbreak[nameof(LocationCenter)]!.GetValue<string>());
            var locationDummy = Convert.FromHexString(simpleOutbreak[nameof(LocationDummy)]!.GetValue<string>());
            var species = SpeciesConverter.GetInternal9(simpleOutbreak[nameof(Species)]!.GetValue<ushort>());
            var form = simpleOutbreak[nameof(Form)]!.GetValue<byte>();
            var maxSpawns = simpleOutbreak[nameof(MaxSpawns)]!.GetValue<int>();

            LocationCenter!.SetCoordinates(locationCenter);
            LocationDummy!.SetCoordinates(locationDummy);
            Species = species;
            Form = form;
            MaxSpawns = maxSpawns;
        }

        public sbyte GetAmountAvailable()
        {
            var info = LocationMap switch { "DLC1" => BlockDefinitions.KOutbreakBCDLC1NumActive, "DLC2" => BlockDefinitions.KOutbreakBCDLC2NumActive, "Main" => BlockDefinitions.KOutbreakBCMainNumActive };
            var block = SAV.Accessor.GetBlockSafe(info.Key);

            if (block.Type is SCTypeCode.SByte)
                return (sbyte)block.GetValue();

            return 0;
        }

        public void SetAmountAvailable(sbyte value)
        {
            var info = LocationMap switch { "DLC1" => BlockDefinitions.KOutbreakBCDLC1NumActive, "DLC2" => BlockDefinitions.KOutbreakBCDLC2NumActive, "Main" => BlockDefinitions.KOutbreakBCMainNumActive };
            var block = SAV.Accessor.GetBlockSafe(info.Key);

            if (block.Type is SCTypeCode.SByte)
                block.ChangeData((new byte[] { (byte)value }).AsSpan());
        }

        public bool GetEnabled()
        {
            return ID <= AmountAvailable;
        }

        public void SetEnabled(bool value)
        {
            if (value && AmountAvailable < ID)
                AmountAvailable = (sbyte)ID;
            else if (!value && AmountAvailable >= ID)
                AmountAvailable = (sbyte)(ID - 1);
        }

        public bool GetFound()
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Found")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.FindOrDefault(blockInfo.Key);

            if (block.Type.IsBoolean() && block.Type is SCTypeCode.Bool2)
                return true;

            return false;
        }

        public void SetFound(bool value)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Found")!.GetValue(new BlockDefinition())!; ;
            var block = SAV.Accessor.FindOrDefault(blockInfo.Key);

            if (block.Type.IsBoolean())
            {
                if (value)
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    block.ChangeBooleanType(SCTypeCode.Bool1);
            }
        }

        public uint GetSpecies()
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Species")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.UInt32)
                return (uint)block.GetValue();

            return 0;
        }

        public void SetSpecies(uint value)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Species")!.GetValue(new BlockDefinition())!; ;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.UInt32)
                block.SetValue(value);
        }

        public byte GetForm()
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Form")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Byte or SCTypeCode.SByte)
                return (byte)block.GetValue();

            return 0;
        }

        public void SetForm(byte value)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}Form")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Byte or SCTypeCode.SByte)
                block.SetValue(value);
        }

        public int GetNumKO()
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}NumKOed")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                return (int)block.GetValue();

            return 0;
        }

        public void SetNumKO(int value)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}NumKOed")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                block.SetValue(value);
        }

        public int GetMaxSpawns()
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}TotalSpawns")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                return (int)block.GetValue();

            return 0;
        }

        public void SetMaxSpawns(int value)
        {
            var blockInfo = (BlockDefinition)typeof(BlockDefinitions).GetField($"KOutbreakBC{ID:00}{LocationMap}TotalSpawns")!.GetValue(new BlockDefinition())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                block.SetValue(value);
        }
    }
}
