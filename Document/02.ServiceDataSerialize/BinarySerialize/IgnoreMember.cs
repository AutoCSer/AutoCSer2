using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Ignore the member example
    /// 忽略成员 示例
    /// </summary>
    class IgnoreMember
    {
        /// <summary>
        /// Ignore the current member
        /// 忽略当前成员
        /// </summary>
        [AutoCSer.BinarySerializeMember(IsIgnoreCurrent = true)]
        public int Ignore;
        /// <summary>
        /// Public field member
        /// </summary>
        public int Value;

        /// <summary>
        /// Ignore the member test
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
