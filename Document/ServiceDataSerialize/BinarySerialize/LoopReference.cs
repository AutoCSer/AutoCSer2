using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 循环引用支持 示例
    /// </summary>
    class LoopReference
    {
        /// <summary>
        /// 循环引用成员
        /// </summary>
        LoopReference? Reference;

        /// <summary>
        /// 循环引用支持 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            LoopReference value = new LoopReference();
            value.Reference = value;//构造循环引用

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
