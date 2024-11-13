using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// 程序集 XML 文档注释信息
    /// </summary>
    internal unsafe sealed class XmlDocumentAssembly
    {
        /// <summary>
        /// 程序集
        /// </summary>
        internal readonly HashObject<Assembly> Assembly;
        /// <summary>
        /// 类型：类、接口、结构、枚举、委托
        /// </summary>
        private readonly Dictionary<HashSubString, XmlNode> types = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<XmlNode>();
        /// <summary>
        /// 类型集合
        /// </summary>
        private readonly Dictionary<HashObject<Type>, XmlNode> typeNodes = DictionaryCreator.CreateHashObject<Type, XmlNode>();
        /// <summary>
        /// 类型集合访问锁
        /// </summary>
        private readonly object typeNodeLock = new object();
        /// <summary>
        /// 类型名称流
        /// </summary>
        private AutoCSer.Reflection.TypeNameBuilder typeNameBuilder = new AutoCSer.Reflection.TypeNameBuilder(Reflection.TypeNameBuildEnum.XmlDocument);
        /// <summary>
        /// 类型名称临时字符串
        /// </summary>
        private readonly string typeNameString = AutoCSer.Common.AllocateString(4096);
        /// <summary>
        /// 类型名称访问锁
        /// </summary>
        private readonly object typeNameLock = new object();
        /// <summary>
        /// 字段
        /// </summary>
        private readonly Dictionary<HashSubString, XmlNode> fields = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<XmlNode>();
        /// <summary>
        /// 属性（包括索引程序或其他索引属性）
        /// </summary>
        private readonly Dictionary<HashSubString, XmlNode> properties = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<XmlNode>();
        /// <summary>
        /// 方法（包括一些特殊方法，例如构造函数、运算符等）
        /// </summary>
        private readonly Dictionary<HashSubString, XmlNode> methods = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<XmlNode>();
        /// <summary>
        /// 方法集合
        /// </summary>
        private readonly Dictionary<HashObject<MethodInfo>, XmlNode> methodNodes = DictionaryCreator.CreateHashObject<MethodInfo, XmlNode>();
        /// <summary>
        /// 方法集合访问锁
        /// </summary>
        private readonly object methodNodeLock = new object();
        /// <summary>
        /// 程序集 XML 文档注释信息
        /// </summary>
        /// <param name="assembly"></param>
        internal XmlDocumentAssembly(Assembly assembly)
        {
            Assembly = assembly;
        }
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private XmlNode get(Type type)
        {
            if (type == null) return default(XmlNode);
            HashObject<Type> hashType = type;
            Monitor.Enter(typeNodeLock);
            try
            {
                XmlNode node;
                if (typeNodes.TryGetValue(hashType, out node)) return node;
                Monitor.Enter(typeNameLock);
                try
                {
                    HashSubString typeName = type.getTypeFullName(ref typeNameBuilder, typeNameString);
                    if (types.TryGetValue(typeName, out node)) types.Remove(typeName);
                }
                finally { Monitor.Exit(typeNameLock); }
                typeNodes[hashType] = node;
                return node;
            }
            finally { Monitor.Exit(typeNodeLock); }
        }
        /// <summary>
        /// 获取类型描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetSummary(Type type)
        {
            return get(get(type), "summary");
        }
        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private XmlNode get(FieldInfo field)
        {
            if (field == null) return default(XmlNode);
            SubString fieldName;
            CharStream nameStream = typeNameBuilder.NameStream;
            Monitor.Enter(typeNameLock);
            try
            {
                fixed (char* nameFixed = typeNameString)
                {
                    nameStream.Reset(nameFixed, typeNameString.Length << 1);
                    using (nameStream)
                    {
                        field.DeclaringType.notNull().typeFullName(ref typeNameBuilder);
                        nameStream.Write('.');
                        nameStream.SimpleWrite(field.Name);
                        fieldName = TypeNameExtension.GetName(ref typeNameBuilder, typeNameString, nameFixed);
                    }
                }
            }
            finally { Monitor.Exit(typeNameLock); }
            XmlNode node;
            fields.TryGetValue(fieldName, out node);
            return node;
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetSummary(FieldInfo field)
        {
            return get(get(field), "summary");
        }
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private XmlNode get(PropertyInfo property)
        {
            if (property == null) return default(XmlNode);
            SubString propertyName;
            CharStream nameStream = typeNameBuilder.NameStream;
            Monitor.Enter(typeNameLock);
            try
            {
                fixed (char* nameFixed = typeNameString)
                {
                    nameStream.Reset(nameFixed, typeNameString.Length << 1);
                    using (nameStream)
                    {
                        property.DeclaringType.notNull().typeFullName(ref typeNameBuilder);
                        nameStream.Write('.');
                        nameStream.SimpleWrite(property.Name);
                        propertyName = TypeNameExtension.GetName(ref typeNameBuilder, typeNameString, nameFixed);
                    }
                }
            }
            finally { Monitor.Exit(typeNameLock); }
            XmlNode node;
            properties.TryGetValue(propertyName, out node);
            return node;
        }
        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetSummary(PropertyInfo property)
        {
            return get(get(property), "summary");
        }
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private XmlNode get(MethodInfo method)
        {
            if (method == null) return default(XmlNode);
            HashObject<MethodInfo> hashMethod = method;
            Monitor.Enter(methodNodeLock);
            try
            {
                XmlNode node;
                if (methodNodes.TryGetValue(hashMethod, out node)) return node;
                SubString methodName;
                CharStream nameStream = typeNameBuilder.NameStream;
                Monitor.Enter(typeNameLock);
                try
                {
                    fixed (char* nameFixed = typeNameString)
                    {
                        nameStream.Reset(nameFixed, typeNameString.Length << 1);
                        using (nameStream)
                        {
                            var declaringType = method.DeclaringType.notNull();
                            declaringType.typeFullName(ref typeNameBuilder);
                            nameStream.Write('.');
                            string name = method.Name;
                            if (name[0] == '.')
                            {
                                nameStream.Write('#');
                                nameStream.Write(name, 1, name.Length - 1);
                            }
                            else nameStream.SimpleWrite(name);
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length != 0)
                            {
                                bool isFirst = true;
                                var genericArguments = declaringType.IsGenericTypeDefinition ? declaringType.GetGenericArguments() : null;
                                nameStream.Write('(');
                                foreach (ParameterInfo parameter in parameters)
                                {
                                    if (isFirst) isFirst = false;
                                    else nameStream.Write(',');
                                    int typeIndex = genericArguments == null ? -1 : Array.IndexOf(genericArguments, parameter.ParameterType);
                                    if (typeIndex >= 0)
                                    {
                                        nameStream.Write(AutoCSer.Reflection.TypeNameBuilder.GenericSplit);
                                        nameStream.WriteString(typeIndex);
                                    }
                                    else parameter.ParameterType.typeFullName(ref typeNameBuilder);
                                }
                                nameStream.Write(')');
                            }
                            formatName(nameStream.Char, nameStream.CurrentChar);
                            methodName = TypeNameExtension.GetName(ref typeNameBuilder, typeNameString, nameFixed);
                        }
                    }
                }
                finally { Monitor.Exit(typeNameLock); }
                methods.TryGetValue(methodName, out node);
                methodNodes[hashMethod] = node;
                return node;
            }
            finally { Monitor.Exit(methodNodeLock); }
        }
        /// <summary>
        /// 获取方法描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetSummary(MethodInfo method)
        {
            return get(get(method), "summary");
        }
        /// <summary>
        /// 获取方法返回值描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string GetReturn(MethodInfo method)
        {
            return get(get(method), "returns");
        }
        /// <summary>
        /// 获取参数描述
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public unsafe string Get(MethodInfo method, ParameterInfo parameter)
        {
            XmlNode xmlNode = get(method);
            if (xmlNode.Type == XmlNodeTypeEnum.Node)
            {
                string parameterName = parameter.Name.notNull();
                Range attribute = default(Range);
                fixed (char* nameFixed = "name", parameterFixed = parameterName)
                {
                    foreach (KeyValue<SubString, XmlNode> node in xmlNode.Nodes)
                    {
                        if (node.Value.Type != XmlNodeTypeEnum.Node && node.Key.Equals("param")
                            && node.Value.GetAttribute(nameFixed, 4, ref attribute)
                            && attribute.Length == parameterName.Length)
                        {
                            fixed (char* attributeFixed = node.Key.GetFixedBuffer())
                            {
                                if (AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)parameterFixed, (byte*)(attributeFixed + attribute.StartIndex), parameterName.Length << 1))
                                {
                                    return node.Value.String.Length == 0 ? string.Empty : node.Value.String.ToString().notNull();
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 加载数据记录
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="node"></param>
        internal void LoadMember(SubString name, XmlNode node)
        {//https://msdn.microsoft.com/zh-cn/library/fsbx0t7x(v=vs.80).aspx
            //对于泛型类型，类型名称后跟反勾号，再跟一个数字，指示泛型类型参数的个数。例如，
            //<member name="T:SampleClass`2"> 是定义为 public class SampleClass<T, U> 的类型的标记。
            if (name[1] == ':')
            {
                char code = name[0];
                switch (code & 7)
                {
                    case 'T' & 7://类型：类、接口、结构、枚举、委托
                        if (code == 'T') types[name.GetSub(2)] = node;
                        break;
                    case 'F' & 7://字段
                        if (code == 'F') fields[name.GetSub(2)] = node;
                        break;
                    case 'P' & 7://属性（包括索引程序或其他索引属性）
                        if (code == 'P') properties[name.GetSub(2)] = node;
                        break;
                    case 'M' & 7://方法（包括一些特殊方法，例如构造函数、运算符等）
                        if (code == 'M') methods[name.GetSub(2)] = node;
                        break;
                        //case 'E' & 7://事件
                        //    break;
                        //case 'N' & 7://命名空间
                        //case '!' & 7://错误字符串
                        //break;
                }
            }
        }
        /// <summary>
        /// 获取节点字符串
        /// </summary>
        /// <param name="node">成员节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static string get(XmlNode node, string name)
        {
            return (node = node[name]).String.Length != 0 ? node.String.ToString().notNull() : string.Empty;
        }
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void formatName(char* start, char* end)
        {
            do
            {
                if (*start == '&') *start = '@';
            }
            while (++start != end);
        }
    }
}
