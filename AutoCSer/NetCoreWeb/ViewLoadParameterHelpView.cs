using AutoCSer.Reflection;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图初始化加载数据方法 LoadView 参数视图数据
    /// </summary>
    public sealed class ViewLoadParameterHelpView
    {
        /// <summary>
        /// 数据视图请求信息帮助类视图数据
        /// </summary>
        private readonly ViewHelpView view;
        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly ParameterInfo parameter;
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get { return parameter.Name; } }
        /// <summary>
        /// 参数文档描述
        /// </summary>
        private string summary;
        /// <summary>
        /// 参数文档描述
        /// </summary>
        public string Summary
        {
            get
            {
                if (summary == null)
                {
                    summary = XmlDocument.Get(view.Method, parameter);
                    if (string.IsNullOrEmpty(summary)) summary = Type.Summary;
                }
                return summary;
            }
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        private TypeHelpView type;
        /// <summary>
        /// 参数类型
        /// </summary>
        public TypeHelpView Type
        {
            get
            {
                if (type == null) type = view.Request.ViewMiddleware.GetTypeHelpView(parameter.ParameterType);
                return type;
            }
        }
        /// <summary>
        /// 数据视图初始化加载数据方法 LoadView 参数视图数据
        /// </summary>
        /// <param name="view">数据视图请求信息帮助类视图数据</param>
        /// <param name="parameter">参数信息</param>
        internal ViewLoadParameterHelpView(ViewHelpView view, ParameterInfo parameter)
        {
            this.view = view;
            this.parameter = parameter;
        }
    }
}
