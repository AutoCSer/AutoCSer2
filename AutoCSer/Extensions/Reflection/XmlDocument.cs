using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace AutoCSer.Reflection
{
    /// <summary>
    /// XML 文档注释
    /// </summary>
    internal static class XmlDocument
    {
        /// <summary>
        /// XML解析配置
        /// </summary>
        private static readonly AutoCSer.XmlDeserializeConfig xmlParserConfig = new AutoCSer.XmlDeserializeConfig { BootNodeName = "doc", IsAttribute = true, IsTempString = true };
        /// <summary>
        /// 程序集信息集合
        /// </summary>
        private static Dictionary<HashObject<Assembly>, XmlDocumentAssembly> assemblys = DictionaryCreator.CreateHashObject<Assembly, XmlDocumentAssembly>();
        /// <summary>
        /// 程序集信息集合访问锁
        /// </summary>
        private static readonly object assemblyLock = new object();
        /// <summary>
        /// 最后访问的程序集信息
        /// </summary>
        private static XmlDocumentAssembly lastAssembly;
        /// <summary>
        /// 获取程序集信息
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private unsafe static XmlDocumentAssembly get(Assembly assembly)
        {
            if (assembly == null || assembly.IsDynamic) return null;
            XmlDocumentAssembly value = lastAssembly;
            if (value != null && value.Assembly.Value == assembly) return value;
            Monitor.Enter(assemblyLock);
            try
            {
                if (assemblys.TryGetValue(assembly, out value)) return value;
                string fileName = assembly.Location;
                if (fileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(fileName = fileName.Substring(0, fileName.Length - 3) + "xml"))
                    {
                        XmlNode xmlNode = AutoCSer.XmlDeserializer.Deserialize<XmlNode>(File.ReadAllText(fileName, Encoding.UTF8), xmlParserConfig)["members"];
                        if (xmlNode.Type == XmlNodeTypeEnum.Node)
                        {
                            fixed (char* nameFixed = "name")
                            {
                                value = new XmlDocumentAssembly(assembly);
                                Range attribute = default(Range);
                                foreach (KeyValue<SubString, XmlNode> node in xmlNode.Nodes)
                                {
                                    if (node.Value.Type == XmlNodeTypeEnum.Node && node.Key.Equals("member"))
                                    {
                                        if (node.Value.GetAttribute(nameFixed, 4, ref attribute) && attribute.Length > 2)
                                        {
                                            value.LoadMember(new SubString(attribute.StartIndex, attribute.Length, node.Key.String), node.Value);
                                        }
                                    }
                                }
                            }
                        }
                        else AutoCSer.LogHelper.ErrorIgnoreException("XML文档解析失败 " + fileName, LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                    }
                    else AutoCSer.LogHelper.ErrorIgnoreException("没有找到XML文档注释 " + fileName, LogLevelEnum.Error | LogLevelEnum.AutoCSer);
                }
                assemblys.Add(assembly, value);
            }
            finally { Monitor.Exit(assemblyLock); }
            lastAssembly = value;
            return value;
        }
        /// <summary>
        /// 清理程序集信息集合
        /// </summary>
        private static void clearAssemblys()
        {
            if (assemblys.Count != 0)
            {
                Monitor.Enter(assemblyLock);
                try
                {
                    if (assemblys.Count != 0) assemblys = DictionaryCreator.CreateHashObject<Assembly, XmlDocumentAssembly>();
                }
                finally { Monitor.Exit(assemblyLock); }
            }
            lastAssembly = null;
        }
        /// <summary>
        /// 获取类型描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Get(Type type)
        {
            XmlDocumentAssembly assembly = get(type.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(type);
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Get(FieldInfo field)
        {
            XmlDocumentAssembly assembly = get(field.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(field);
        }
        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Get(PropertyInfo property)
        {
            XmlDocumentAssembly assembly = get(property.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(property);
        }
        /// <summary>
        /// 获取方法描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Get(MethodInfo method)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(method);
        }
        /// <summary>
        /// 获取方法返回值描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string GetReturn(MethodInfo method)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetReturn(method);
        }
        /// <summary>
        /// 获取参数描述
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Get(MethodInfo method, ParameterInfo parameter)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.Get(method, parameter);
        }

        static XmlDocument()
        {
            AutoCSer.Memory.Common.AddClearCache(clearAssemblys, 60 * 60);
        }
    }
}
