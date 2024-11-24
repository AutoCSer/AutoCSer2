using AutoCSer.Memory.ObjectRoot;
using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 序列化操作基类 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsBaseType = true)]
    class BaseType
    {
        /// <summary>
        /// 基类数据字段
        /// </summary>
        public int Value;

        /// <summary>
        /// 序列化操作基类 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            SonType value = new SonType { Value = 1, SonValue = 2 };
            byte[] data = AutoCSer.BinarySerializer.Serialize(value);

            var newValue = AutoCSer.BinaryDeserializer.Deserialize<SonType>(data);
            if (newValue == null || newValue.Value != 1 || newValue.SonValue != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
    /// <summary>
    /// 派生类型
    /// </summary>
    class SonType : BaseType
    {
        /// <summary>
        /// 派生类型数据字段
        /// </summary>
        public int SonValue;
    }
}
