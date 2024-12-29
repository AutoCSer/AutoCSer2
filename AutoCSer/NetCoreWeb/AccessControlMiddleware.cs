using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 跨域访问支持中间件（应该放在应用中间件的最前面）
    /// </summary>
    public sealed class AccessControlMiddleware : Middleware
    {
        /// <summary>
        /// 跨域请求头部参数 Access-Control-Allow-Origin
        /// </summary>
        private readonly StringValues accessOrigin = "*";
        /// <summary>
        /// 跨域响应头部参数 Access-Control-Allow-Methods
        /// </summary>
        private readonly StringValues accessMethods = "POST, PUT, DELETE";
        /// <summary>
        /// 跨域响应头部参数 Access-Control-Allow-Headers
        /// </summary>
        private readonly StringValues accessHeaders = "Content-Type, Accept";
        /// <summary>
        /// 跨域响应头部参数 Access-Control-Max-Age
        /// </summary>
        private readonly StringValues accessMaxAge = "1728000";
        /// <summary>
        /// 跨域访问支持中间件
        /// </summary>
        /// <param name="nextRequest"></param>
        public AccessControlMiddleware(RequestDelegate nextRequest) : base(nextRequest) { }
        /// <summary>
        /// 跨域访问处理
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            IHeaderDictionary headers = httpContext.Response.Headers;
            headers["Access-Control-Allow-Origin"] = accessOrigin;
            if (string.Compare(httpContext.Request.Method, "OPTIONS", true) != 0) return nextRequest(httpContext);
            headers["Access-Control-Allow-Methods"] = accessMethods;
            headers["Access-Control-Allow-Headers"] = accessHeaders;
            headers["Access-Control-Max-Age"] = accessMaxAge;
            return AutoCSer.Common.CompletedTask;
        }
    }
}
