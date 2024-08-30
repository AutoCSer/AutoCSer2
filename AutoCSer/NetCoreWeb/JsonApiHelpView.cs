using AutoCSer.Reflection;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 帮助文档视图数据
    /// </summary>
    public sealed class JsonApiHelpView
    {
        /// <summary>
        /// JSON API 控制器
        /// </summary>
        public readonly JsonApiControllerHelpView Controller;
        /// <summary>
        /// JSON API 方法信息
        /// </summary>
        internal readonly JsonApiMethod Method;
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get { return Method.Method.Name; } }
        /// <summary>
        /// 请求路由路径
        /// </summary>
        public string RoutePath { get { return Method.RoutePath; } }
        /// <summary>
        /// API 方法文档描述
        /// </summary>
        public string Summary;
        /// <summary>
        /// 返回值数据类型
        /// </summary>
        public readonly TypeHelpView ReturnType;
        /// <summary>
        /// 返回值文档描述
        /// </summary>
        public readonly string ReturnSummary;
        /// <summary>
        /// 参数集合
        /// </summary>
        public readonly JsonApiParameterHelpView[] Parameters;
        /// <summary>
        /// JSON API 帮助文档视图数据
        /// </summary>
        /// <param name="controller">JSON API 控制器</param>
        /// <param name="method">JSON API 方法信息</param>
        internal JsonApiHelpView(JsonApiControllerHelpView controller, JsonApiMethod method)
        {
            Controller = controller;
            this.Method = method;
            ReturnType = controller.ViewMiddleware.GetTypeHelpView(method.HelpReturnType);
            Summary = XmlDocument.Get(method.Method);
            ReturnSummary = XmlDocument.GetReturn(method.Method);
            if (string.IsNullOrEmpty(ReturnSummary)) ReturnSummary = ReturnType.Summary;
            LeftArray<JsonApiParameterHelpView> parameters = new LeftArray<JsonApiParameterHelpView>(method.Parameters.Length);
            foreach (HttpMethodParameter parameter in method.HttpMethodParameters) parameters.Add(new JsonApiParameterHelpView(this, parameter));
            Parameters = parameters.ToArray();
        }
        /// <summary>
        /// 加载类型视图数据
        /// </summary>
        internal void LoadTypeView()
        {
            ReturnType.LoadTypeView();
            foreach (JsonApiParameterHelpView parameter in Parameters) parameter.Type.LoadTypeView();
        }
    }
}
