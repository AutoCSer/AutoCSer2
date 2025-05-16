using AutoCSer.Metadata;
using System;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 二进制序列化信息
    /// </summary>
    public struct BinarySerializeMethodInfo
    {
        /// <summary>
        /// 枚举数组的元素类型
        /// </summary>
        internal Type EnumArrayElementType;
        /// <summary>
        /// 泛型参数
        /// </summary>
        internal Type GenericType;
        /// <summary>
        /// 泛型参数
        /// </summary>
        internal Type GenericType1;
        /// <summary>
        /// 泛型参数
        /// </summary>
        internal Type GenericType2;
        ///// <summary>
        ///// 是否代码生成模式调用
        ///// </summary>
        //internal bool IsCodeGenerator;
        /// <summary>
        /// 是否自定义序列化接口
        /// </summary>
        internal bool IsCusotm;
        /// <summary>
        /// 是否可空数组
        /// </summary>
        internal bool IsNullableArray;
        /// <summary>
        /// 是否结构体数组
        /// </summary>
        internal bool IsStructArray;
        /// <summary>
        /// 是否字典
        /// </summary>
        internal bool IsDictionary;
        /// <summary>
        /// 是否集合
        /// </summary>
        internal bool IsCollection;
        /// <summary>
        /// 是否 JSON 序列化
        /// </summary>
        internal bool IsJson;
        /// <summary>
        /// 是否需要泛型反射处理
        /// </summary>
        internal bool IsGenericReflection;
        /// <summary>
        /// 是否简单序列化
        /// </summary>
        internal bool IsSimpleSerialize;
        /// <summary>
        /// 设置字典泛型参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        internal void SetDictionary(Type type, Type interfaceType)
        {
            IsDictionary = true;
            GenericType = type;
            Type[] types = interfaceType.GetGenericArguments();
            GenericType1 = types[0];
            GenericType2 = types[1];
        }
        /// <summary>
        /// 设置集合泛型参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        internal void SetCollection(Type type, Type interfaceType)
        {
            IsCollection = true;
            GenericType = type;
            GenericType1 = interfaceType.GetGenericArguments()[0];
        }
    }
}
