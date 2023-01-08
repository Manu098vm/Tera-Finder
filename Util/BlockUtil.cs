using PKHeX.Core;
using System.Reflection;
using System.Runtime.Serialization;

namespace TeraFinder
{
    public static class BlockUtil
    {
        public static SCBlock CreateObjectBlock(uint key, ReadOnlySpan<byte> data)
        {
            var block = (SCBlock)FormatterServices.GetUninitializedObject(typeof(SCBlock));
            var keyInfo = typeof(SCBlock).GetField("Key", BindingFlags.Instance | BindingFlags.Public)!;
            keyInfo.SetValue(block, key);
            var typeInfo = typeof(SCBlock).GetProperty("Type")!;
            typeInfo.SetValue(block, SCTypeCode.Object);
            var dataInfo = typeof(SCBlock).GetField("Data", BindingFlags.Instance | BindingFlags.Public)!;
            dataInfo.SetValue(block, data.ToArray());
            return block;
        }

        public static SCBlock CreateBoolBlock(uint key, SCTypeCode boolean)
        {
            var block = (SCBlock)FormatterServices.GetUninitializedObject(typeof(SCBlock));
            var keyInfo = typeof(SCBlock).GetField("Key", BindingFlags.Instance | BindingFlags.Public)!;
            keyInfo.SetValue(block, key);
            var typeInfo = typeof(SCBlock).GetProperty("Type")!;
            typeInfo.SetValue(block, boolean);
            var dataInfo = typeof(SCBlock).GetField("Data", BindingFlags.Instance | BindingFlags.Public)!;
            dataInfo.SetValue(block, Array.Empty<byte>());
            return block;
        }

        public static void AddBlockToFakeSAV(SAV9SV sav, SCBlock block)
        {
            var list = new List<SCBlock>();
            foreach (var b in sav.Accessor.BlockInfo) list.Add(b);
            list.Add(block);
            var typeInfo = typeof(SAV9SV).GetField("<AllBlocks>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
            typeInfo.SetValue(sav, list);
            typeInfo = typeof(SaveBlockAccessor9SV).GetField("<BlockInfo>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
            typeInfo.SetValue(sav.Blocks, list);
        }

        public static void EditBlock(SCBlock block, SCTypeCode type, ReadOnlySpan<byte> data)
        {
            var typeInfo = typeof(SCBlock).GetProperty("Type")!;
            typeInfo.SetValue(block, type);
            var dataInfo = typeof(SCBlock).GetField("Data", BindingFlags.Instance | BindingFlags.Public)!;
            dataInfo.SetValue(block, data.ToArray());
        }

        public static SCBlock FindOrDefault(this SCBlockAccessor Accessor, uint Key) => Accessor.BlockInfo.FindOrDefault(Key);

        public static SCBlock FindOrDefault(this IReadOnlyList<SCBlock> blocks, uint key)
        {
            var res = blocks.Where(block => block.Key == key).FirstOrDefault();
            return res is not null ? res : CreateBoolBlock(key, SCTypeCode.None);
        }
    }
}