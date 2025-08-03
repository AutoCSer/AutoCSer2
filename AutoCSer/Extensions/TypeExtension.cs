using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// 根据类型获取代码名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buildEnum">类型名称输出类型</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        /// <returns>代码名称</returns>
#if NetStandard21
        internal static string? fullName(this Type type, TypeNameBuildEnum buildEnum = TypeNameBuildEnum.Code, bool isGenericParameterTypeName = true)
#else
        internal static string fullName(this Type type, TypeNameBuildEnum buildEnum = TypeNameBuildEnum.Code, bool isGenericParameterTypeName = true)
#endif
        {
            return TypeNameBuilder.GetFullName(type, buildEnum, isGenericParameterTypeName);
        }
        ///// <summary>
        ///// 不需要需要初始化的类型集合
        ///// </summary>
        //private static readonly HashSet<HashObject<System.Type>> noInitobjTypes;
        ///// <summary>
        ///// 是否需要初始化
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static bool isInitobj(this Type type)
        //{
        //    if (type.IsEnum || noInitobjTypes.Contains(type)) return false;
        //    return true;
        //}
        /// <summary>
        /// 类型是否不支持序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isSerializeNotSupport(this Type type)
        {
            return type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type);
        }
        /// <summary>
        /// 类型是否不支持序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isSerializeNotSupportOrArrayRank(this Type type)
        {
            return type.IsInterface || type.IsPointer || (type.IsArray && type.GetArrayRank() != 1) || typeof(Delegate).IsAssignableFrom(type);
        }
        /// <summary>
        /// 成员类型是否忽略序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isIgnoreSerialize(this Type type)
        {
            return type.IsPointer || (type.IsArray && type.GetArrayRank() != 1) || typeof(Delegate).IsAssignableFrom(type);
        }
        /// <summary>
        /// 判断是否可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isValueTypeNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        /// <summary>
        /// 判断是否可空值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static bool isNullable(this Type type)
        {
            return type.IsValueType && isValueTypeNullable(type);
        }
        /// <summary>
        /// 获取可空类型的值类型
        /// </summary>
        /// <param name="type">可空类型</param>
        /// <returns>值类型,失败返回null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Type? getNullableType(this Type type)
#else
        internal static Type getNullableType(this Type type)
#endif
        {
            return type.isNullable() ? type.GetGenericArguments()[0] : null;
        }
        /// <summary>
        /// 获取所有泛型接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> getGenericInterface(this Type type)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType) yield return interfaceType;
            }
        }
        /// <summary>
        /// 根据指定泛型定义接口类型获取泛型接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericDefinitionInterfaceType">泛型定义接口类型</param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? getGenericInterfaceType(this Type type, Type genericDefinitionInterfaceType)
#else
        internal static Type getGenericInterfaceType(this Type type, Type genericDefinitionInterfaceType)
#endif
        {
            if (type.IsInterface && isGenericInterfaceType(type, genericDefinitionInterfaceType)) return type;
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (isGenericInterfaceType(interfaceType, genericDefinitionInterfaceType)) return interfaceType;
            }
            return null;
        }
        /// <summary>
        /// 判断是否指定泛型定义接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericDefinitionInterfaceType"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool isGenericInterfaceType(this Type type, Type genericDefinitionInterfaceType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericDefinitionInterfaceType;
        }
        /// <summary>
        /// 获取泛型定义类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Type getGenericTypeDefinition(this Type type)
        {
            return !type.IsGenericType || type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
        }

        //static TypeExtension()
        //{
        //    noInitobjTypes = HashSetCreator.CreateHashObject<System.Type>();
        //    noInitobjTypes.Add(typeof(bool));
        //    noInitobjTypes.Add(typeof(bool?));
        //    noInitobjTypes.Add(typeof(byte));
        //    noInitobjTypes.Add(typeof(byte?));
        //    noInitobjTypes.Add(typeof(sbyte));
        //    noInitobjTypes.Add(typeof(sbyte?));
        //    noInitobjTypes.Add(typeof(short));
        //    noInitobjTypes.Add(typeof(short?));
        //    noInitobjTypes.Add(typeof(ushort));
        //    noInitobjTypes.Add(typeof(ushort?));
        //    noInitobjTypes.Add(typeof(int));
        //    noInitobjTypes.Add(typeof(int?));
        //    noInitobjTypes.Add(typeof(uint));
        //    noInitobjTypes.Add(typeof(uint?));
        //    noInitobjTypes.Add(typeof(long));
        //    noInitobjTypes.Add(typeof(long?));
        //    noInitobjTypes.Add(typeof(ulong));
        //    noInitobjTypes.Add(typeof(ulong?));
        //    noInitobjTypes.Add(typeof(float));
        //    noInitobjTypes.Add(typeof(float?));
        //    noInitobjTypes.Add(typeof(double));
        //    noInitobjTypes.Add(typeof(double?));
        //    noInitobjTypes.Add(typeof(decimal));
        //    noInitobjTypes.Add(typeof(decimal?));
        //    noInitobjTypes.Add(typeof(char));
        //    noInitobjTypes.Add(typeof(char?));
        //    noInitobjTypes.Add(typeof(DateTime));
        //    noInitobjTypes.Add(typeof(DateTime?));
        //    noInitobjTypes.Add(typeof(Guid));
        //    noInitobjTypes.Add(typeof(Guid?));
        //}
    }
}
