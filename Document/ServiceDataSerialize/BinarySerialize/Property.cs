using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 自动属性匿名字段设置 示例
    /// </summary>
    class Property
    {
        /// <summary>
        /// 自动属性
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// 自动属性匿名字段设置 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            byte[] data = AutoCSer.BinarySerializer.Serialize(new Property { Value = 1 });
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<Property>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
