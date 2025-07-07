using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Example of JSON mixed binary serialization
    /// JSON 混杂二进制序列化 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMix = true)]
    class JsonMix
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
        /// JSON mixed binary serialization test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new JsonMix { Value = 1, String = nameof(String) });
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<JsonDeserialize>(data);
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
    class JsonDeserialize
    {
        /// <summary>
        /// Deserialization field
        /// 反序列化字段
        /// </summary>
        public string? String;
    }
}
