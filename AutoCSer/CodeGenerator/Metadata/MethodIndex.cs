using AutoCSer.Metadata;
using System;
using System.Reflection;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员方法
    /// </summary>
    internal sealed class MethodIndex : MemberIndex
    {
        /// <summary>
        /// 成员方法信息
        /// </summary>
        public MethodInfo Method { get; private set; }
        ///// <summary>
        ///// 返回值类型
        ///// </summary>
        //public readonly ExtensionType MethodReturnType;
        /// <summary>
        /// 参数集合
        /// </summary>
        public MethodParameter[] Parameters { get; private set; }
        /// <summary>
        /// 参数数量
        /// </summary>
        public int ParameterCount { get { return Parameters.Length; } }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName
        {
            get { return Method.Name; }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string XmlDocument
        {
            get
            {
                if (xmlDocument == null) xmlDocument = AutoCSer.Reflection.XmlDocument.Get(Method);
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string CodeGeneratorXmlDocument
        {
            get { return AutoCSer.Reflection.XmlDocument.CodeGeneratorFormat(XmlDocument); }
        }
        /// <summary>
        /// 返回值XML文档注释
        /// </summary>
        private string returnXmlDocument;
        /// <summary>
        /// 返回值XML文档注释
        /// </summary>
        public string ReturnXmlDocument
        {
            get
            {
                if (returnXmlDocument == null) returnXmlDocument = AutoCSer.Reflection.XmlDocument.GetReturn(Method);
                return returnXmlDocument.Length == 0 ? null : returnXmlDocument;
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        public string CodeGeneratorReturnXmlDocument
        {
            get { return AutoCSer.Reflection.XmlDocument.CodeGeneratorFormat(ReturnXmlDocument); }
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        internal MethodIndex(MethodInfo method) : this(method, MemberFiltersEnum.PublicInstance, 0)
        {
            //if (method.ReturnType != typeof(void)) MethodReturnType = method.ReturnType;
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        internal MethodIndex(MethodInfo method, MemberFiltersEnum filter, int index)
            : base(method, filter, index)
        {
            Method = method;
            //if (method.ReturnType != typeof(void)) MethodReturnType = method.ReturnType;
            Parameters = MethodParameter.Get(method);
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        /// <param name="parameterStartIndex"></param>
        /// <param name="parameterEndIndex"></param>
        internal MethodIndex(MethodInfo method, MemberFiltersEnum filter, int index, int parameterStartIndex, int parameterEndIndex)
            : base(method, filter, index)
        {
            Method = method;
            //if (method.ReturnType != typeof(void)) MethodReturnType = method.ReturnType;
            Parameters = MethodParameter.Get(method, parameterStartIndex, parameterEndIndex);
        }
        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object GetValue(object value)
        {
            return Method.Invoke(value, null);
        }
    }
}
