using AutoCSer.Extensions;
using AutoCSer.Reflection;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 参数帮助文档视图数据
    /// </summary>
    public sealed class JsonApiParameterHelpView
    {
        /// <summary>
        /// API 方法信息
        /// </summary>
        public readonly JsonApiHelpView Method;
        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly HttpMethodParameter parameter;
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get { return parameter.Parameter.Name.notNull(); } }
        /// <summary>
        /// 代理控制器方法参数约束类型
        /// </summary>
        public ParameterConstraintTypeEnum ConstraintType { get { return parameter.ConstraintType; } }
        /// <summary>
        /// 是否模板参数
        /// </summary>
        public bool IsTemplateParameter { get { return parameter.IsTemplateParameter; } }
        /// <summary>
        /// 参数文档描述
        /// </summary>
        public string Summary;
        /// <summary>
        /// 参数类型
        /// </summary>
        public readonly TypeHelpView Type;
        /// <summary>
        /// JSON API 参数帮助文档视图数据
        /// </summary>
        /// <param name="method">API 方法信息</param>
        /// <param name="parameter">参数信息</param>
        internal JsonApiParameterHelpView(JsonApiHelpView method, HttpMethodParameter parameter)
        {
            Method = method;
            this.parameter = parameter;
            Summary = XmlDocument.Get(Method.Method.Method, parameter.Parameter);
            Type = method.Controller.ViewMiddleware.GetTypeHelpView(parameter.Parameter.ParameterType);
            if (string.IsNullOrEmpty(Summary)) Summary = Type.Summary;
        }
    }
}
