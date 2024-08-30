using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图中间件请求实例
    /// </summary>
    public abstract class ViewMiddlewareRequest
    {
        /// <summary>
        /// 数据视图中间件
        /// </summary>
        public readonly ViewMiddleware ViewMiddleware;
        /// <summary>
        /// 请求信息
        /// </summary>
        internal abstract string RequestInfo { get; }
        /// <summary>
        /// 数据视图信息
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        protected ViewMiddlewareRequest(ViewMiddleware viewMiddleware)
        {
            ViewMiddleware = viewMiddleware;
        }
        /// <summary>
        /// 加载请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="type">数据视图请求类型</param>
        /// <returns></returns>
        internal abstract Task Request(HttpContext httpContext, ViewRequestTypeEnum type);

        /// <summary>
        /// 获取数据视图中间件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ViewMiddleware GetViewMiddleware(ViewMiddlewareRequest request)
        {
            return request.ViewMiddleware;
        }
    }
}
