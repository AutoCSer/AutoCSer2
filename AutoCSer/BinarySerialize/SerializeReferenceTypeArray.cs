using AutoCSer.Metadata;
using System;
using System.Collections.Generic;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化引用类型数组
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SerializeReferenceTypeArray
    {
        /// <summary>
        /// 序列化引用类型数组
        /// </summary>
        internal LeftArray<GenericType> TypeArray;
        /// <summary>
        /// 序列化引用类型集合
        /// </summary>
        private readonly HashSet<HashType> types;
        /// <summary>
        /// 序列化引用类型数组
        /// </summary>
        /// <param name="genericType"></param>
        internal SerializeReferenceTypeArray(GenericType genericType)
        {
            TypeArray = new LeftArray<GenericType>(0);
            types = HashSetCreator.CreateHashType();
            if (!genericType.CurrentType.IsValueType) Append(genericType);
        }
        /// <summary>
        /// 添加引用类型
        /// </summary>
        /// <param name="genericType"></param>
        /// <returns></returns>
        internal bool Append(GenericType genericType)
        {
            Type type = genericType.CurrentType;
            do
            {
                if (!types.Add(type)) return false;
                if (type == typeof(object)) break;
                type = type.BaseType;
            }
            while (type != null);
            TypeArray.Add(genericType);
            return true;
        }
    }
}
