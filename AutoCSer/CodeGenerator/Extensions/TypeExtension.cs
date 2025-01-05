using AutoCSer.Memory;
using AutoCSer.Reflection;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Extensions
{
    /// <summary>
    /// 类型扩展操作
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// 是否值类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否值类型</returns>
        internal static bool isStruct(this Type type)
        {
            return type != null && type.IsValueType && !type.IsEnum;
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        internal static string getName(this Type type)
        {
            if (type == null) return null;
            string value;
            if (TypeNameBuilder.TypeNames.TryGetValue(type, out value)) return value;
            if (type.IsGenericParameter) return type.Name;
            if (type.IsArray)
            {
                TypeNameBuilder typeNameBuilder = new TypeNameBuilder();
                using (typeNameBuilder.NameStream = new CharStream(UnmanagedPool.Tiny))
                {
                    typeNameBuilder.Array(type, EmptyArray<Type>.Array, false);
                    return typeNameBuilder.NameStream.ToString();
                }
            }
            if (type.IsGenericType)
            {
                TypeNameBuilder typeNameBuilder = new TypeNameBuilder();
                using (typeNameBuilder.NameStream = new CharStream(UnmanagedPool.Tiny))
                {
                    typeNameBuilder.GenericName(type);
                    return typeNameBuilder.NameStream.ToString();
                }
            }
            return type.Name;
        }
        /// <summary>
        /// 根据类型获取可用名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型名称</returns>
        internal static string getOnlyName(this Type type)
        {
            string value;
            if (AutoCSer.Reflection.TypeNameBuilder.TypeNames.TryGetValue(type, out value)) return value;
            value = type.Name;
            if (type.IsGenericTypeDefinition)
            {
                int index = value.IndexOf(AutoCSer.Reflection.TypeNameBuilder.GenericSplit);
                if (index != -1) value = value.Substring(0, index);
            }
            return value;
        }
        /// <summary>
        /// 获取代码生成类型定义字符串
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nameSuffix">类名称后缀</param>
        /// <returns></returns>
        internal static string getDefinitionString(this Type type, string nameSuffix = null)
        {
            if (type == null) return null;
            TypeNameBuilder typeNameBuilder = new TypeNameBuilder();
            using (CharStream nameStream = typeNameBuilder.NameStream = new CharStream(UnmanagedPool.Tiny))
            {
                if (type.IsNested)
                {
                    if (type.IsNestedPublic) nameStream.SimpleWrite("public ");
                    else if (type.IsNestedPrivate) nameStream.SimpleWrite("private ");
                    else if (type.IsNestedAssembly) nameStream.SimpleWrite("internal ");
                }
                else nameStream.SimpleWrite(type.IsPublic ? "public " : "internal ");
                if (type.IsEnum) nameStream.SimpleWrite("enum ");
                else
                {
                    if (type.IsInterface) nameStream.SimpleWrite("partial interface ");
                    else if (type.IsAbstract) nameStream.SimpleWrite(type.IsSealed ? "static " : "abstract ");
                    else if (type.IsValueType) nameStream.SimpleWrite("partial struct ");
                    else nameStream.SimpleWrite("partial class ");
                }
                if (type.IsGenericType) typeNameBuilder.GenericName(type, nameSuffix);
                else
                {
                    nameStream.SimpleWrite(type.Name);
                    if (!string.IsNullOrEmpty(nameSuffix)) nameStream.SimpleWrite(nameSuffix);
                }
                return typeNameBuilder.NameStream.ToString();
            }
        }
    }
}
