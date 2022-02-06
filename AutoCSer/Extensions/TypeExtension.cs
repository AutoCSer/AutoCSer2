using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// 根据类型获取代码名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>代码名称</returns>
        public static string fullName(this Type type)
        {
            return TypeNameBuilder.GetFullName(type);
        }
        /// <summary>
        /// 根据成员属性获取自定义属性
        /// </summary>
        /// <typeparam name="T">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="inherit">是否查找继承链</param>
        /// <returns>自定义属性</returns>
        internal static T customAttribute<T>(this Type type, bool inherit = false)
            where T : Attribute
        {
            foreach (T attribute in type.GetCustomAttributes(typeof(T), inherit)) return attribute;
            return null;
        }
        /// <summary>
        /// 不需要需要初始化的类型集合
        /// </summary>
        private static readonly HashSet<HashType> noInitobjTypes;
        /// <summary>
        /// 是否需要初始化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isInitobj(this Type type)
        {
            if (type.IsEnum || noInitobjTypes.Contains(type)) return false;
            return true;
        }
        /// <summary>
        /// 类型是否不支持序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool isSerializeNotSupport(this Type type)
        {
            return type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type);
        }

        static TypeExtension()
        {
            noInitobjTypes = HashSetCreator.CreateHashType();
            noInitobjTypes.Add(typeof(bool));
            noInitobjTypes.Add(typeof(bool?));
            noInitobjTypes.Add(typeof(byte));
            noInitobjTypes.Add(typeof(byte?));
            noInitobjTypes.Add(typeof(sbyte));
            noInitobjTypes.Add(typeof(sbyte?));
            noInitobjTypes.Add(typeof(short));
            noInitobjTypes.Add(typeof(short?));
            noInitobjTypes.Add(typeof(ushort));
            noInitobjTypes.Add(typeof(ushort?));
            noInitobjTypes.Add(typeof(int));
            noInitobjTypes.Add(typeof(int?));
            noInitobjTypes.Add(typeof(uint));
            noInitobjTypes.Add(typeof(uint?));
            noInitobjTypes.Add(typeof(long));
            noInitobjTypes.Add(typeof(long?));
            noInitobjTypes.Add(typeof(ulong));
            noInitobjTypes.Add(typeof(ulong?));
            noInitobjTypes.Add(typeof(float));
            noInitobjTypes.Add(typeof(float?));
            noInitobjTypes.Add(typeof(double));
            noInitobjTypes.Add(typeof(double?));
            noInitobjTypes.Add(typeof(decimal));
            noInitobjTypes.Add(typeof(decimal?));
            noInitobjTypes.Add(typeof(char));
            noInitobjTypes.Add(typeof(char?));
        }
    }
}
