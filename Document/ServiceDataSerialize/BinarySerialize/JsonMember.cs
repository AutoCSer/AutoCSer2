using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// JSON 序列化扩展 示例。即使没有 JSON 序列化字段，也应该预留 JSON 序列化标记。
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMember = true)]
    class JsonMember
    {
        /// <summary>
        /// 二进制序列化字段
        /// </summary>
        public int Value;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsJsonMember = true)]
        public short? Json1;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsJsonMember = true)]
        public short? Json2;

        /// <summary>
        /// JSON 序列化扩展 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            JsonMember value = new JsonMember { Value = 1, Json1 = 2, Json2 = 3 };

            byte[] data = AutoCSer.BinarySerializer.Serialize(value);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<JsonMemberDeserialize>(data);

            if (newValue == null || newValue.Value != 1 || newValue.Json1 != 2 || newValue.Json2 != 3 || newValue.Json3 != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
#pragma warning disable CS0649
    /// <summary>
    /// JSON 序列化扩展 示例 反序列化定义
    /// </summary>
    [AutoCSer.BinarySerialize(IsJsonMember = true)]
    class JsonMemberDeserialize
    {
        /// <summary>
        /// 二进制序列化字段
        /// </summary>
        public int Value;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsJsonMember = true)]
        public int Json1;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsJsonMember = true)]
        public int Json2;
        /// <summary>
        /// JSON 序列化字段
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsJsonMember = true)]
        public int Json3;
    }
}
