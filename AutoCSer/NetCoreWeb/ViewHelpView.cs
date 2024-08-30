using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图请求信息帮助类视图数据
    /// </summary>
    public sealed class ViewHelpView
    {
        /// <summary>
        /// 数据视图请求实例
        /// </summary>
        internal readonly ViewRequest Request;
        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath
        {
            get { return Request.RequestPath; }
        }
        /// <summary>
        /// 数据视图类型
        /// </summary>
        private TypeHelpView type;
        /// <summary>
        /// 数据视图类型
        /// </summary>
        public TypeHelpView Type
        {
            get
            {
                if (type == null) type = Request.ViewMiddleware.GetTypeHelpView(Request.Type);
                return type;
            }
        }
        /// <summary>
        /// 初始化加载数据 LoadView 方法信息
        /// </summary>
        internal MethodInfo Method;
        /// <summary>
        /// 初始化加载数据方法 LoadView 参数集合
        /// </summary>
        private ViewLoadParameterHelpView[] parameters;
        /// <summary>
        /// 初始化加载数据方法 LoadView 参数集合
        /// </summary>
        public ViewLoadParameterHelpView[] Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    Method = View.GetLoadMethod(Request.Type);
                    if (Method != null)
                    {
                        int parameterIndex = 0;
                        ParameterInfo[] parameters = Method.GetParameters();
                        if (parameterIndex != parameters.Length && parameters[parameterIndex].ParameterType == typeof(HttpContext)) ++parameterIndex;
                        if (parameterIndex != parameters.Length && parameters[parameterIndex].ParameterType == typeof(ViewMiddleware)) ++parameterIndex;
                        LeftArray<ViewLoadParameterHelpView> parameterHelpViews = new LeftArray<ViewLoadParameterHelpView>(parameters.Length - parameterIndex);
                        if (parameterHelpViews.Array.Length != 0)
                        {
                            do
                            {
                                parameterHelpViews.Add(new ViewLoadParameterHelpView(this, parameters[parameterIndex]));
                            }
                            while (++parameterIndex != parameters.Length);
                        }
                        this.parameters = parameterHelpViews.ToArray();
                    }
                    else this.parameters = EmptyArray<ViewLoadParameterHelpView>.Array;
                }
                return this.parameters;
            }
        }
        /// <summary>
        /// 数据视图请求信息帮助类视图数据
        /// </summary>
        /// <param name="request">数据视图请求实例</param>
        internal ViewHelpView(ViewRequest request)
        {
            this.Request = request;
        }
        /// <summary>
        /// 加载类型视图数据
        /// </summary>
        internal void LoadTypeView()
        {
            Type.LoadTypeView();
            foreach (ViewLoadParameterHelpView parameter in Parameters) parameter.Type.LoadTypeView();
        }
    }
}
