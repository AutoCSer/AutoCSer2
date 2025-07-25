﻿using AutoCSer.Memory.ObjectRoot;
using System;

namespace AutoCSer.Document.ServiceDataSerialize.BinarySerialize
{
    /// <summary>
    /// Example of base class serialization
    /// 基础类型序列化 示例
    /// </summary>
    [AutoCSer.BinarySerialize(IsBaseType = true)]
    class BaseType
    {
        /// <summary>
        /// Base class data field
        /// 基类数据字段
        /// </summary>
        public int Value;

        /// <summary>
        /// Base class serialization test
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            DerivedType value = new DerivedType { Value = 1, DerivedValue = 2 };
            byte[] data = AutoCSer.BinarySerializer.Serialize(value);

            var newValue = AutoCSer.BinaryDeserializer.Deserialize<DerivedType>(data);
            if (newValue == null || newValue.Value != 1 || newValue.DerivedValue != 0)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
    /// <summary>
    /// Derived type
    /// 派生类型
    /// </summary>
    class DerivedType : BaseType
    {
        /// <summary>
        /// Derived type data field
        /// 派生类型数据字段
        /// </summary>
        public int DerivedValue;
    }
}
