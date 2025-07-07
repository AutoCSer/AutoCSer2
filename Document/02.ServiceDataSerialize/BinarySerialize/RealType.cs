using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Examples of support for abstract types and interface types
    /// 抽象类型与接口类型支持 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsBaseType = true)]
    class RealType : AbstractType, InterfaceType
    {
        /// <summary>
        /// Abstract types and interface types support configuration parameters. To set IsRealType = true, the deserialization end needs to be configured to allow this remote type; otherwise, deserialization will fail.
        /// 抽象类型与接口类型支持 配置参数，设置为 IsRealType = true 需要反序列化端配置允许该远程类型，否则将导致反序列化失败
        /// </summary>
        private static readonly AutoCSer.BinarySerializeConfig realTypeConfig = new AutoCSer.BinarySerializeConfig { IsRealType = true };
        /// <summary>
        /// Abstract types and interface types support testing
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            AbstractType value = new RealType { Value = 1 };
            byte[] data = AutoCSer.BinarySerializer.Serialize(value, realTypeConfig);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<AbstractType>(data);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            InterfaceType value2 = new RealType { Value = 2 };
            data = AutoCSer.BinarySerializer.Serialize(value2, realTypeConfig);
            var newValue2 = AutoCSer.BinaryDeserializer.Deserialize<InterfaceType>(data) as AbstractType;
            if (newValue2 == null || newValue2.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
    /// <summary>
    /// Examples of abstract type support
    /// 抽象类型支持 示例
    /// </summary>
    abstract class AbstractType
    {
        /// <summary>
        /// Data
        /// </summary>
        public int Value;
    }
    /// <summary>
    /// Example of interface type support
    /// 接口类型支持 示例
    /// </summary>
    interface InterfaceType
    {
    }
}
