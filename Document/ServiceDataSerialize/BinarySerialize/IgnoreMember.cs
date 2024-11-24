using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 忽略成员 示例
    /// </summary>
    class IgnoreMember
    {
        /// <summary>
        /// 公共字段成员
        /// </summary>
        public int Value;
        /// <summary>
        /// 忽略当前成员
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        public int Ignore;

        /// <summary>
        /// 忽略成员 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            IgnoreMember value = new IgnoreMember { Value = 1, Ignore = 2 };

            byte[] data = AutoCSer.BinarySerializer.Serialize(value);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<IgnoreMember>(data);

            if (newValue == null || newValue.Value != 1 || newValue.Ignore != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
