﻿using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// 抽象类型与接口类型支持 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsBaseType = true)]
    class RealType : AbstractType, InterfaceType
    {
        /// <summary>
        /// 抽象类型与接口类型支持 配置参数，只有在可信任环境中才允许设置为 IsRealType = true，否则将产生 new 任意类型的安全漏洞
        /// </summary>
        private static readonly AutoCSer.BinarySerializeConfig realTypeConfig = new AutoCSer.BinarySerializeConfig { IsRealType = true };
        /// <summary>
        /// 抽象类型与接口类型支持 配置参数，只有在可信任环境中才允许设置为 IsRealType = true，否则将产生 new 任意类型的安全漏洞
        /// </summary>
        private static readonly AutoCSer.BinarySerialize.DeserializeConfig realTypeDeserializeConfig = new AutoCSer.BinarySerialize.DeserializeConfig { IsRealType = true };
        /// <summary>
        /// 抽象类型与接口类型支持 测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            AbstractType value = new RealType { Value = 1 };
            byte[] data = AutoCSer.BinarySerializer.Serialize(value, realTypeConfig);
            var newValue = AutoCSer.BinaryDeserializer.Deserialize<AbstractType>(data, realTypeDeserializeConfig);
            if (newValue == null || newValue.Value != 1)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            InterfaceType value2 = new RealType { Value = 2 };
            data = AutoCSer.BinarySerializer.Serialize(value2, realTypeConfig);
            var newValue2 = AutoCSer.BinaryDeserializer.Deserialize<InterfaceType>(data, realTypeDeserializeConfig) as AbstractType;
            if (newValue2 == null || newValue2.Value != 2)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
    /// <summary>
    /// 抽象类型支持 示例
    /// </summary>
    abstract class AbstractType
    {
        /// <summary>
        /// 数据
        /// </summary>
        public int Value;
    }
    /// <summary>
    /// 接口类型支持 示例
    /// </summary>
    interface InterfaceType
    {
    }
}