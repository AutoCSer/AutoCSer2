using AutoCSer.CodeGenerator.Extensions;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员类型
    /// </summary>
    internal sealed class ExtensionType
    {
        /// <summary>
        /// 数字类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> numberTypes = new HashSet<HashObject<Type>>(new HashObject<Type>[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char) });
        /// <summary>
        /// JavaScript ToString 重定向类型集合
        /// </summary>
        private static readonly HashSet<HashObject<Type>> javaScriptToStringTypes = new HashSet<HashObject<Type>>(new HashObject<Type>[] { typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char) });

        /// <summary>
        /// 类型
        /// </summary>
        internal Type Type { get; private set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        private string typeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get
            {
                if (typeName == null) typeName = Type?.getName();
                return typeName;
            }
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        private string fullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        public string FullName
        {
            get
            {
                if (fullName == null) fullName = Type?.fullName();
                return fullName;
            }
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        private string typeOnlyName;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeOnlyName
        {
            get
            {
                if (typeOnlyName == null) typeOnlyName =Type?.getOnlyName();
                return typeOnlyName;
            }
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        private string xmlFullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        public string XmlFullName
        {
            get
            {
                if (xmlFullName == null) xmlFullName = Type?.fullName(Reflection.TypeNameBuildEnum.OutputXml);
                return xmlFullName;
            }
        }
        /// <summary>
        /// 泛型定义类型名称
        /// </summary>
        public string GenericDefinitionFullName
        {
            get
            {
                if (fullName == null) fullName = Type?.fullName(Reflection.TypeNameBuildEnum.Code, false);
                return fullName;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        /// <param name="type">类型</param>
        private ExtensionType(Type type)
        {
            Type = type;
        }
        /// <summary>
        /// 是否引用类型
        /// </summary>
        public bool IsNull
        {
            get { return Type == null || !Type.IsValueType || Type.isNullable(); }
        }
        /// <summary>
        /// 是否值类型(排除可空类型)
        /// </summary>
        public bool IsStruct
        {
            get { return Type.isStruct() && !Type.isValueTypeNullable(); }
        }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsString
        {
            get { return Type == typeof(string); }
        }
        /// <summary>
        /// 是否字符串
        /// </summary>
        public bool IsSubString
        {
            get { return Type == typeof(SubString); }
        }
        /// <summary>
        /// 是否逻辑类型(包括可空类型)
        /// </summary>
        public bool IsBool
        {
            get { return Type == typeof(bool) || Type == typeof(bool?); }
        }
        /// <summary>
        /// 是否数字类型
        /// </summary>
        public bool IsNumber
        {
            get
            {
                return numberTypes.Contains(Type);
            }
        }
        /// <summary>
        /// 是否时间类型(包括可空类型)
        /// </summary>
        public bool IsDateTime
        {
            get { return Type == typeof(DateTime) || Type == typeof(DateTime?); }
        }
        /// <summary>
        /// 可空类型的值类型
        /// </summary>
        private ExtensionType nullableType;
        /// <summary>
        /// 可空类型的值类型
        /// </summary>
        public ExtensionType NullableType
        {
            get
            {
                if (nullableType == null) nullableType = Type.getNullableType();
                return nullableType.Type != null ? nullableType : null;
            }
        }
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        private ExtensionType enumerableType;
        /// <summary>
        /// 可枚举泛型类型
        /// </summary>
        public ExtensionType EnumerableType
        {
            get
            {
                if (enumerableType == null)
                {
                    if (!IsString)
                    {
                        Type enumerableInterfaceType = Type.getGenericInterfaceType(typeof(IEnumerable<>));
                        if (enumerableInterfaceType != null)
                        {
                            enumerableArgumentType = enumerableInterfaceType.GetGenericArguments()[0];
                            enumerableType = enumerableInterfaceType;
                        }
                    }
                    if (enumerableType == null) enumerableType = Null;
                }
                return enumerableType.Type != null ? enumerableType : null;
            }
        }
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        private ExtensionType enumerableArgumentType;
        /// <summary>
        /// 可枚举泛型参数类型
        /// </summary>
        public ExtensionType EnumerableArgumentType
        {
            get
            {
                return EnumerableType != null ? enumerableArgumentType : null;
            }
        }
#if !DotNet45
        /// <summary>
        /// 客户端视图绑定类型
        /// </summary>
        private AutoCSer.NetCoreWeb.ViewClientTypeAttribute netCoreWebViewClientTypeAttribute;
        /// <summary>
        /// 客户端视图绑定类型
        /// </summary>
        public AutoCSer.NetCoreWeb.ViewClientTypeAttribute NetCoreWebViewClientTypeAttribute
        {
            get
            {
                if (netCoreWebViewClientTypeAttribute == null)
                {
                    netCoreWebViewClientTypeAttribute = (AutoCSer.NetCoreWeb.ViewClientTypeAttribute)Type.GetCustomAttribute(typeof(AutoCSer.NetCoreWeb.ViewClientTypeAttribute), true) ?? AutoCSer.NetCoreWeb.ViewClientTypeAttribute.Null;
                }
                return object.ReferenceEquals(netCoreWebViewClientTypeAttribute, AutoCSer.NetCoreWeb.ViewClientTypeAttribute.Null) ? null : netCoreWebViewClientTypeAttribute;
            }
        }
#endif
        /// <summary>
        /// 是否 JavaScript ToString 重定向类型
        /// </summary>
        public bool IsJavaScriptToString
        {
            get { return javaScriptToStringTypes.Contains(NullableType?.Type ?? Type); }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        private string xmlDocument;
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string XmlDocument
        {
            get
            {
                if (xmlDocument == null) xmlDocument = AutoCSer.Reflection.XmlDocument.Get(Type);
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// 获取类名称定义
        /// </summary>
        /// <param name="typeNameSuffix">生成类型名称后缀</param>
        /// <returns></returns>
        public string GetTypeNameDefinition(string typeNameSuffix = null) { return Type?.getDefinitionString(typeNameSuffix); }

        /// <summary>
        /// 空类型
        /// </summary>
        private static readonly ExtensionType Null = new ExtensionType(null);
        /// <summary>
        /// 成员类型隐式转换集合
        /// </summary>
        private static Dictionary<HashObject<System.Type>, ExtensionType> types = DictionaryCreator.CreateHashObject<System.Type, ExtensionType>();
        /// <summary>
        /// 隐式转换集合转换锁
        /// </summary>
        private static readonly object typeLock = new object();
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员类型</returns>
        public static implicit operator ExtensionType(Type type)
        {
            if (type == null) return Null;
            ExtensionType value;
            Monitor.Enter(typeLock);
            try
            {
                if (!types.TryGetValue(type, out value)) types.Add(type, value = new ExtensionType(type));
            }
            finally { Monitor.Exit(typeLock); }
            return value;
        }
    }
}
