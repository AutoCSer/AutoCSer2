using AutoCSer.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图请求实例
    /// </summary>
    public sealed class ViewRequest : ViewMiddlewareRequest
    {
        /// <summary>
        /// 数据视图类型
        /// </summary>
        public readonly Type Type;
        /// <summary>
        /// 请求信息
        /// </summary>
        internal override string RequestInfo { get { return Type.fullName(); } }
        /// <summary>
        /// 获取数据视图实例委托
        /// </summary>
        private readonly Func<View> getView;
        /// <summary>
        /// 数据视图自定义属性
        /// </summary>
        internal readonly ViewAttribute Attribute;
        /// <summary>
        /// 单例数据视图实例
        /// </summary>
        private readonly View view;
        /// <summary>
        /// 是否参数鉴权
        /// </summary>
        public readonly bool IsAccessTokenParameter;
        /// <summary>
        /// 获取数据视图实例
        /// </summary>
        internal View View
        {
            get { return view ?? getView(); }
        }
        /// <summary>
        /// 请求路径
        /// </summary>
        internal string RequestPath
        {
            get { return View.RequestPath; }
        }
        /// <summary>
        /// 数据视图信息
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="type">数据视图类型</param>
        /// <param name="getView">获取数据视图实例委托</param>
        /// <param name="parameterType">调用方法第一个参数类型</param>
        /// <param name="parameterName">调用方法第一个参数名称</param>
        public ViewRequest(ViewMiddleware viewMiddleware, Type type, Func<View> getView, Type parameterType = null, string parameterName = null) : base(viewMiddleware)
        {
            Type = type;
            this.getView = getView;
            Attribute = (ViewAttribute)type.GetCustomAttribute(typeof(ViewAttribute), false) ?? ViewAttribute.Default;
            if (Attribute.IsSingleton) view = getView();
            IsAccessTokenParameter = parameterType != null && ViewMiddleware.IsAccessTokenParameter(parameterType, parameterName);
        }
        /// <summary>
        /// 加载请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="type">数据视图请求类型</param>
        /// <returns></returns>
        internal override Task Request(HttpContext httpContext, ViewRequestTypeEnum type)
        {
            return View.Load(httpContext, this);
        }
    }
}
