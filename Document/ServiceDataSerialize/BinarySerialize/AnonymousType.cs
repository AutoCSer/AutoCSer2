using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 匿名类型序列化 示例
    /// </summary>
    class AnonymousType
    {
        /// <summary>
        /// 匿名类型序列化 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new { Value = 1 });
            var newValue = new { Value = 0 };
            AutoCSer.BinaryDeserializer.Deserialize(data, ref newValue);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
