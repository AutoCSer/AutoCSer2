using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Examples of automatically implemented property
    /// 自动属性 示例
    /// </summary>
    class Property
    {
        /// <summary>
        /// Automatically implementing property
        /// 自动属性
        /// </summary>
        int Value { get; set; }

        /// <summary>
        /// Automatically implement property testing
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
