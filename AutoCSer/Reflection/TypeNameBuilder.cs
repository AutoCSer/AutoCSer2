using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 类型代码名称生成器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct TypeNameBuilder
    {
        /// <summary>
        /// 类型名称泛型分隔符
        /// </summary>
        internal const char GenericSplit = '`';

        /// <summary>
        /// 名称缓存
        /// </summary>
        internal CharStream NameStream;
        /// <summary>
        /// 类型名称输出类型
        /// </summary>
        internal TypeNameBuildEnum TypeNameEnum;
        /// <summary>
        /// 是否 XML
        /// </summary>
        private bool isXml
        {
            get
            {
                switch (TypeNameEnum)
                {
                    case TypeNameBuildEnum.XmlDocument:
                    case TypeNameBuildEnum.OutputXml:
                        return true;
                    default: return false;
                }
            }
        }
        /// <summary>
        /// 类型代码名称生成器
        /// </summary>
        /// <param name="typeNameEnum"></param>
        internal TypeNameBuilder(TypeNameBuildEnum typeNameEnum)
        {
            NameStream = new CharStream(default(UnmanagedPoolPointer));
            this.TypeNameEnum = typeNameEnum;
        }
        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        /// <returns>类型名称</returns>
        internal string GetTypeFullName(Type type, Type[] genericArguments, bool isGenericParameterTypeName = true)
        {
            if (type.IsArray)
            {
                NameStream.TrySetData(UnmanagedPool.Tiny);
                using (NameStream)
                {
                    Array(type, genericArguments, true);
                    return NameStream.ToString();
                }
            }
            if (type.IsGenericType)
            {
                NameStream.TrySetData(UnmanagedPool.Tiny);
                using (NameStream)
                {
                    GenericFullName(type, genericArguments, isGenericParameterTypeName);
                    return NameStream.ToString();
                }
            }
            var reflectedType = type.ReflectedType;
            if (reflectedType == null) return type.Namespace + "." + type.Name;
            else
            {
                NameStream.TrySetData(UnmanagedPool.Tiny);
                using (NameStream)
                {
                    this.ReflectedType(type, reflectedType);
                    return NameStream.ToString();
                }
            }
        }
        /// <summary>
        /// 数组处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        /// <param name="isFullName">是否全称</param>
        internal unsafe void Array(Type type, Type[] genericArguments, bool isFullName)
        {
            UnmanagedPoolPointer buffer = UnmanagedPool.Tiny.GetPoolPointer();
            try
            {
                int* currentRank = buffer.Pointer.Int, endRank = (int*)buffer.Pointer.End;
                do
                {
                    if (currentRank == endRank) throw new IndexOutOfRangeException();
                    *currentRank++ = type.GetArrayRank();
                }
                while ((type = type.GetElementType().notNull()).IsArray);
                if (isFullName) getFullName(type, genericArguments);
                else getNameNoArray(type);
                while (currentRank != buffer.Pointer.Int)
                {
                    NameStream.Write('[');
                    int rank = *--currentRank;
                    if (--rank != 0) NameStream.WriteString(rank);
                    NameStream.Write(']');
                }
            }
            finally { buffer.PushOnly(); }
        }
        /// <summary>
        /// 任意类型处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        private void getFullName(Type type, Type[] genericArguments)
        {
            var value = default(string);
            if (TypeNameEnum != TypeNameBuildEnum.XmlDocument && TypeNames.TryGetValue(type, out value)) NameStream.SimpleWrite(value);
            else if (type.IsGenericParameter)
            {
                if (!XmlDocumentGenericTypeParameter(type, genericArguments)) NameStream.SimpleWrite(type.Name);
            }
            else if (type.IsArray) Array(type, genericArguments, true);
            else if (type.IsGenericType) GenericFullName(type, genericArguments);
            else
            {
                var reflectedType = type.ReflectedType;
                if (reflectedType == null)
                {
                    NameStream.SimpleWrite(type.Namespace.notNull());
                    NameStream.Write('.');
                    NameStream.SimpleWrite(type.Name);
                }
                else this.ReflectedType(type, reflectedType);
            }
        }
        /// <summary>
        /// 任意类型处理
        /// </summary>
        /// <param name="type">类型</param>
        private void getNameNoArray(Type type)
        {
            var value = default(string);
            if (TypeNameEnum != TypeNameBuildEnum.XmlDocument && TypeNames.TryGetValue(type, out value)) NameStream.SimpleWrite(value);
            else if (type.IsGenericParameter) NameStream.SimpleWrite(type.Name);
            else if (type.IsGenericType) GenericName(type);
            else NameStream.SimpleWrite(type.Name);
        }
        /// <summary>
        /// 泛型处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="nameSuffix">类名称后缀</param>
#if NetStandard21
        internal void GenericName(Type type, string? nameSuffix = null)
#else
        internal void GenericName(Type type, string nameSuffix = null)
#endif
        {
            string name = type.Name;
            int splitIndex = name.IndexOf(GenericSplit);
            var reflectedType = type.ReflectedType;
            if (reflectedType == null)
            {
                NameStream.Write(name, 0, splitIndex);
                if (!string.IsNullOrEmpty(nameSuffix)) NameStream.SimpleWrite(nameSuffix);
                genericParameter(type, EmptyArray<Type>.Array);
                return;
            }
            if (splitIndex == -1)
            {
                NameStream.SimpleWrite(name);
                return;
            }
            Type[] parameterTypes = type.GetGenericArguments();
            int parameterIndex = 0;
            do
            {
                if (reflectedType.IsGenericType)
                {
                    int parameterCount = reflectedType.GetGenericArguments().Length;
                    if (parameterCount != parameterTypes.Length)
                    {
                        parameterIndex = parameterCount;
                        break;
                    }
                }
                reflectedType = reflectedType.ReflectedType;
            }
            while (reflectedType != null);
            NameStream.Write(name, 0, splitIndex);
            if (!string.IsNullOrEmpty(nameSuffix)) NameStream.SimpleWrite(nameSuffix);
            genericParameter(parameterTypes, parameterIndex, parameterTypes.Length);
        }
        /// <summary>
        /// 泛型处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        internal void GenericFullName(Type type, Type[] genericArguments, bool isGenericParameterTypeName = true)
        {
            var reflectedType = type.ReflectedType;
            if (reflectedType == null)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    getFullName(type.GetGenericArguments()[0], genericArguments);
                    NameStream.Write('?');
                    return;
                }
                string name = type.Name;
                NameStream.SimpleWrite(type.Namespace.notNull());
                NameStream.Write('.');
                if (TypeNameEnum == TypeNameBuildEnum.XmlDocument && type.IsGenericTypeDefinition) NameStream.WriteNotNull(name);
                else
                {
                    NameStream.Write(name, 0, name.IndexOf(GenericSplit));
                    genericParameter(type, genericArguments, isGenericParameterTypeName);
                }
                return;
            }
            LeftArray<Type> reflectedTypeList = new LeftArray<Type>(sizeof(int));
            do
            {
                reflectedTypeList.Add(reflectedType);
                reflectedType = reflectedType.ReflectedType;
            }
            while (reflectedType != null);
            Type[] reflectedTypeArray = reflectedTypeList.Array;
            int reflectedTypeIndex = reflectedTypeList.Length - 1;
            reflectedType = reflectedTypeArray[reflectedTypeIndex];
            NameStream.SimpleWrite(reflectedType.Namespace.notNull());
            Type[] parameterTypes = type.GetGenericArguments();
            int parameterIndex = 0;
            bool isType = true;
            do
            {
                NameStream.Write('.');
                if (reflectedType.IsGenericType)
                {
                    string name = reflectedType.Name;
                    int splitIndex = name.IndexOf(GenericSplit);
                    if (splitIndex != -1)
                    {
                        NameStream.Write(name, 0, splitIndex);
                        int parameterCount = reflectedType.GetGenericArguments().Length;
                        genericParameter(parameterTypes, parameterIndex, parameterCount, isGenericParameterTypeName);
                        parameterIndex = parameterCount;
                    }
                    else NameStream.SimpleWrite(name);
                }
                else NameStream.SimpleWrite(reflectedType.Name);
                if (reflectedTypeIndex == 0)
                {
                    if (isType)
                    {
                        reflectedType = type;
                        isType = false;
                    }
                    else return;
                }
                else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
            }
            while (true);
        }
        /// <summary>
        /// XML 文档泛型类型参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericArguments"></param>
        /// <returns></returns>
        internal bool XmlDocumentGenericTypeParameter(Type type, Type[] genericArguments)
        {
            if (TypeNameEnum == TypeNameBuildEnum.XmlDocument)
            {
                int typeIndex = genericArguments.Length == 0 ? -1 : System.Array.IndexOf(genericArguments, type);
                if (typeIndex >= 0)
                {
                    NameStream.Write(AutoCSer.Reflection.TypeNameBuilder.GenericSplit);
                    NameStream.WriteString(typeIndex);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 泛型参数处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="genericArguments">泛型类型参数集合</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        private void genericParameter(Type type, Type[] genericArguments, bool isGenericParameterTypeName = true)
        {
            bool isXml = this.isXml;
            NameStream.Write(isXml ? '{' : '<');
            int index = 0;
            foreach (Type parameter in type.GetGenericArguments())
            {
                if (index != 0) NameStream.Write(',');
                if (isGenericParameterTypeName) getFullName(parameter, genericArguments);
                ++index;
            }
            NameStream.Write(isXml ? '}' : '>');
        }
        /// <summary>
        /// 泛型参数处理
        /// </summary>
        /// <param name="parameterTypes">参数类型集合</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        private void genericParameter(Type[] parameterTypes, int startIndex, int endIndex, bool isGenericParameterTypeName = true)
        {
            bool isXml = this.isXml;
            NameStream.Write(isXml ? '{' : '<');
            if (isGenericParameterTypeName) getFullName(parameterTypes[startIndex], parameterTypes);
            while (++startIndex != endIndex)
            {
                NameStream.Write(',');
                if (isGenericParameterTypeName) getFullName(parameterTypes[startIndex], parameterTypes);
            }
            NameStream.Write(isXml ? '}' : '>');
        }
        /// <summary>
        /// 嵌套类型处理
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="reflectedType">上层类型</param>
#if NetStandard21
        internal void ReflectedType(Type type, [MaybeNull] Type reflectedType)
#else
        internal void ReflectedType(Type type, Type reflectedType)
#endif
        {
            LeftArray<Type> reflectedTypeList = new LeftArray<Type>(sizeof(int));
            do
            {
                reflectedTypeList.Add(reflectedType);
                reflectedType = reflectedType.ReflectedType;
            }
            while (reflectedType != null);
            Type[] reflectedTypeArray = reflectedTypeList.Array;
            int reflectedTypeIndex = reflectedTypeList.Length - 1;
            reflectedType = reflectedTypeArray[reflectedTypeIndex];
            NameStream.SimpleWrite(reflectedType.Namespace.notNull());
            do
            {
                NameStream.Write('.');
                NameStream.SimpleWrite(reflectedType.Name);
                if (reflectedTypeIndex == 0)
                {
                    NameStream.Write('.');
                    NameStream.SimpleWrite(type.Name);
                    return;
                }
                else reflectedType = reflectedTypeArray[--reflectedTypeIndex];
            }
            while (true);
        }

        /// <summary>
        /// 根据类型获取代码名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="buildEnum">类型名称输出类型</param>
        /// <param name="isGenericParameterTypeName">是否输出泛型参数类型名称</param>
        /// <returns>代码名称</returns>
#if NetStandard21
        internal static string? GetFullName(Type type, TypeNameBuildEnum buildEnum = TypeNameBuildEnum.Code, bool isGenericParameterTypeName = true)
#else
        internal static string GetFullName(Type type, TypeNameBuildEnum buildEnum = TypeNameBuildEnum.Code, bool isGenericParameterTypeName = true)
#endif
        {
            var value = default(string);
            if (TypeNames.TryGetValue(type, out value)) return value;
            if (type.IsGenericParameter) return isGenericParameterTypeName ? type.Name : null;
            return new TypeNameBuilder(buildEnum).GetTypeFullName(type, EmptyArray<Type>.Array, isGenericParameterTypeName);
        }
        /// <summary>
        /// 类型代码名称集合
        /// </summary>
        internal static readonly Dictionary<HashObject<System.Type>, string> TypeNames;

        static TypeNameBuilder()
        {
            TypeNames = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, string>();
            TypeNames.Add(typeof(bool), "bool");
            TypeNames.Add(typeof(byte), "byte");
            TypeNames.Add(typeof(sbyte), "sbyte");
            TypeNames.Add(typeof(short), "short");
            TypeNames.Add(typeof(ushort), "ushort");
            TypeNames.Add(typeof(int), "int");
            TypeNames.Add(typeof(uint), "uint");
            TypeNames.Add(typeof(long), "long");
            TypeNames.Add(typeof(ulong), "ulong");
            TypeNames.Add(typeof(float), "float");
            TypeNames.Add(typeof(double), "double");
            TypeNames.Add(typeof(decimal), "decimal");
            TypeNames.Add(typeof(char), "char");
            TypeNames.Add(typeof(string), "string");
            TypeNames.Add(typeof(object), "object");
            TypeNames.Add(typeof(void), "void");
        }
    }
}
