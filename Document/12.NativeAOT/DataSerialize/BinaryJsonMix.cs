using System;

namespace AutoCSer.Document.NativeAOT.DataSerialize
{
    /// <summary>
    /// Example of JSON hybrid binary serialization
    /// JSON 混杂二进制序列化 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    partial class BinaryJsonMix
    {
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        public string? String { get; set; }

        /// <summary>
        /// JSON hybrid binary serialization test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new BinaryJsonMix { Value = 1, String = nameof(String) });
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<BinaryJsonMixDeserialize>(data);
            if (newValue == null || newValue.String != nameof(String))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
#pragma warning disable CS0649
    /// <summary>
    /// JSON mixed binary serialization example deserialization data
    /// JSON 混杂二进制序列化 示例 反序列化数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    partial class BinaryJsonMixDeserialize
    {
        /// <summary>
        /// Deserialization field
        /// 反序列化字段
        /// </summary>
        public string? String;
    }
}
