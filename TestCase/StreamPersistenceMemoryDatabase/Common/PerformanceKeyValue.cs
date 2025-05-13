using System;

#pragma warning disable
namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct PerformanceKeyValue
    {
        public int Key;
        public int Value;
        public PerformanceKeyValue(BinarySerializeKeyValue<int, int> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
        }
        public PerformanceKeyValue(KeyValue<int, int> keyValue)
        {
            Key = keyValue.Key;
            Value = keyValue.Value;
        }
    }
}
