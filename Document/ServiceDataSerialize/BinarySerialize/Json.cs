using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 二进制混杂 JSON 序列化 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    class Json
    {
        /// <summary>
        /// 自动属性
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 自动属性
        /// </summary>
        public string? String { get; set; }

        /// <summary>
        /// 二进制混杂 JSON 序列化 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new Json { Value = 1, String = nameof(String) });
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
    /// 二进制混杂 JSON 序列化 示例 反序列化数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    class JsonDeserialize
    {
        /// <summary>
        /// 反序列化字段
        /// </summary>
        public string? String;
    }
}
