using AutoCSer.Memory;
using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    internal static class TypeNameExtension
    {
        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameBuilder"></param>
        /// <param name="tempName"></param>
        /// <returns></returns>
        internal unsafe static SubString getTypeFullName(this Type type, ref TypeNameBuilder typeNameBuilder, string tempName)
        {
            fixed (char* nameFixed = tempName)
            {
                typeNameBuilder.NameStream.Reset(nameFixed, tempName.Length << 1);
                using (typeNameBuilder.NameStream)
                {
                    typeFullName(type, ref typeNameBuilder);
                    return GetName(ref typeNameBuilder, tempName, nameFixed);
                }
            }
        }
        /// <summary>
        /// 写入类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameBuilder"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void typeFullName(this Type type, ref TypeNameBuilder typeNameBuilder)
        {
            typeFullName(type, ref typeNameBuilder, EmptyArray<Type>.Array);
        }
        /// <summary>
        /// 写入类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeNameBuilder"></param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        internal static void typeFullName(this Type type, ref TypeNameBuilder typeNameBuilder, Type[] genericArguments)
        {
            if (type.IsArray) typeNameBuilder.Array(type, genericArguments, true);
            else if (type.IsGenericType) typeNameBuilder.GenericFullName(type, genericArguments);
            else if (!typeNameBuilder.XmlDocumentGenericTypeParameter(type, genericArguments))
            {
                var reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    CharStream nameStream = typeNameBuilder.NameStream;
                    nameStream.SimpleWrite(type.Namespace.notNull());
                    nameStream.Write('.');
                    nameStream.SimpleWrite(type.Name);
                }
                else typeNameBuilder.ReflectedType(type, reflectedType);
            }
        }
        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="typeNameBuilder"></param>
        /// <param name="tempName"></param>
        /// <param name="nameFixed"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static SubString GetName(ref TypeNameBuilder typeNameBuilder, string tempName, char* nameFixed)
        {
            CharStream nameStream = typeNameBuilder.NameStream;
            if (nameStream.Data.Pointer.Data == nameFixed) return new SubString(0, nameStream.Data.Pointer.CurrentIndex >> 1, tempName);
            return nameStream.ToString();
        }
    }
}
