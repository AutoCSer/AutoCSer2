using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 函数参数信息
    /// </summary>
    internal sealed class MethodParameter
    {
        /// <summary>
        /// 定义方法
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// 参数信息
        /// </summary>
        public ParameterInfo Parameter { get; private set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ExtensionType ParameterType { get; private set; }
        /// <summary>
        /// 参数索引位置
        /// </summary>
        public int ParameterIndex { get; private set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public readonly string ParameterName;
        /// <summary>
        /// 参数连接逗号，最后一个参数为null
        /// </summary>
        public string ParameterJoin;
        /// <summary>
        /// 参数连接名称，最后一个参数不带逗号
        /// </summary>
        public string ParameterJoinName { get { return ParameterName + ParameterJoin; } }
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
                if (xmlDocument == null)
                {
                    xmlDocument = Parameter == null ? string.Empty : AutoCSer.Reflection.XmlDocument.Get(method, Parameter);
                }
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// XML文档注释代码字符串
        /// </summary>
        public string XmlDocumentCodeString
        {
            get
            {
                string xmlDocument = XmlDocument;
                if (xmlDocument == null) return null;
                return xmlDocument.IndexOf('"') == -1 ? xmlDocument : xmlDocument.Replace(@"""",@"\""");
            }
        }
        /// <summary>
        /// 是否引用参数
        /// </summary>
        public bool IsRef;
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool IsOut { get; private set; }
        /// <summary>
        /// ref / out 前缀字符串
        /// </summary>
        public string RefOutString
        {
            get
            {
                if (IsOut) return "out ";
                return IsRef ? "ref " : null;
            }
        }
        /// <summary>
        /// 参数信息
        /// </summary>
        /// <param name="method">函数信息</param>
        /// <param name="parameter">参数信息</param>
        /// <param name="index">参数索引位置</param>
        /// <param name="isLast">是否最后一个参数</param>
        private MethodParameter(MethodInfo method, ParameterInfo parameter, int index, bool isLast)
        {
            this.method = method;
            Parameter = parameter;
            ParameterIndex = index;
            Type parameterType = parameter.ParameterType;
            if (parameterType.IsByRef)
            {
                if (parameter.IsOut) IsOut = true;
                else IsRef = true;
                ParameterType = parameterType.GetElementType();
            }
            else ParameterType = parameterType;
            ParameterName = Parameter.Name;
            ParameterJoin = isLast ? null : ", ";
        }

        /// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <returns>参数信息集合</returns>
        internal static MethodParameter[] Get(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.isEmpty()) return EmptyArray<MethodParameter>.Array;
            int index = 0;
            return parameters.getArray(value => new MethodParameter(method, value, index, ++index == parameters.Length));
        }
        /// <summary>
        /// 获取方法参数信息集合
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>参数信息集合</returns>
        internal static MethodParameter[] Get(MethodInfo method, int startIndex, int endIndex)
        {
            int count = endIndex - startIndex;
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length == 0 || count <= 0) return EmptyArray<MethodParameter>.Array;
            int index = 0;
            return new SubArray<ParameterInfo>(startIndex, count, parameters)
                .GetArray(value => new MethodParameter(method, value, index, ++index == count));
        }
    }
}
