using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Examples of circular reference support
    /// 循环引用支持 示例
    /// </summary>
    class LoopReference
    {
        /// <summary>
        /// Circular reference members
        /// 循环引用成员
        /// </summary>
        LoopReference? Reference;

        /// <summary>
        /// Circular references support testing
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            LoopReference value = new LoopReference();
            value.Reference = value;//Construct circular references

            byte[] data = AutoCSer.BinarySerializer.Serialize(value);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<LoopReference>(data);

            if (newValue == null || !object.ReferenceEquals(newValue.Reference, newValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
