using PKHeX.Core;
using System.Reflection;
using static System.Buffers.Binary.BinaryPrimitives;

namespace TeraFinder
{
    public class GameCoordinates
    {
        private SCBlock Coordinates = null!;

        public float X { get => ReadSingleLittleEndian(Coordinates.Data.AsSpan()); set => SetX(value); }
        public float Y { get => ReadSingleLittleEndian(Coordinates.Data.AsSpan()[4..]); set => SetY(value); }
        public float Z { get => ReadSingleLittleEndian(Coordinates.Data.AsSpan()[8..]); set => SetZ(value); }

        public GameCoordinates(SCBlock coordinates) => Coordinates = coordinates;

        private void SetX(float x) => SetCoordinates(0, x);
        private void SetY(float y) => SetCoordinates(4, y);
        private void SetZ(float z) => SetCoordinates(8, z);

        public ReadOnlySpan<byte> GetCoordinates() => Coordinates.Data.AsSpan();
        private void SetCoordinates(int index, float value) => WriteSingleLittleEndian(Coordinates.Data.AsSpan()[index..], value);
    }

    public class MassOutbreak
    {
        protected int ID;
        protected SAV9SV SAV = null!;

        public GameCoordinates? LocationCenter { get; private set; }
        public GameCoordinates? LocationDummy { get; private set; }
        public bool Found { get => GetFound(); set => SetFound(value); }
        public uint Species { get => GetSpecies(); set => SetSpecies(value); }
        public byte Form { get => GetForm(); set => SetForm(value); }
        public int NumKO { get => GetNumKO(); set => SetNumKO(value); }
        public int MaxSpawns { get => GetMaxSpawns(); set => SetMaxSpawns(value); }

        public MassOutbreak(SAV9SV sav, int id)
        {
            ID = id;
            SAV = sav;

            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}CenterPos")!.GetValue(new DataBlock())!;
            var block = sav.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Array)
                LocationCenter = new GameCoordinates(block);
            else
                LocationCenter = null;

            blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}DummyPos")!.GetValue(new DataBlock())!;
            block = sav.Accessor.GetBlockSafe(blockInfo.Key);

            if(block.Type is SCTypeCode.Array)
                LocationDummy = new GameCoordinates(block);
            else
                LocationDummy = null;
        }

        private bool GetFound()
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Found")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type.IsBoolean() && block.Type is SCTypeCode.Bool2)
                return true;

            return false;
        }

        private void SetFound(bool value)
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Found")!.GetValue(new DataBlock())!; ;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type.IsBoolean())
            {
                if (value)
                    block.ChangeBooleanType(SCTypeCode.Bool2);
                else
                    block.ChangeBooleanType(SCTypeCode.Bool1);
            }
        }

        private uint GetSpecies()
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Species")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.UInt32)
                return (uint)block.GetValue();

            return 0;
        }

        private void SetSpecies(uint value)
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Species")!.GetValue(new DataBlock())!; ;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if(block.Type is SCTypeCode.UInt32)
                block.SetValue(value);
        }

        private byte GetForm()
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Form")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Byte or SCTypeCode.SByte)
                return (byte)block.GetValue();

            return 0;
        }

        private void SetForm(byte value)
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}Form")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Byte or SCTypeCode.SByte)
                block.SetValue(value);
        }

        private int GetNumKO()
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}NumKOed")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                return (int)block.GetValue();

            return 0;
        }

        private void SetNumKO(int value)
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}NumKOed")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                block.SetValue(value);
        }

        private int GetMaxSpawns()
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}TotalSpawns")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                return (int)block.GetValue();

            return 0;
        }

        private void SetMaxSpawns(int value)
        {
            var blockInfo = (DataBlock)typeof(Blocks).GetField($"KMassOutbreak0{ID}TotalSpawns")!.GetValue(new DataBlock())!;
            var block = SAV.Accessor.GetBlockSafe(blockInfo.Key);

            if (block.Type is SCTypeCode.Int32)
                block.SetValue(value);
        }
    }
}