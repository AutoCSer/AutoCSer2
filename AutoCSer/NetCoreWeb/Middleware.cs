using Microsoft.AspNetCore.Http;
using System;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 中间件
    /// </summary>
    public abstract class Middleware
    {
        /// <summary>
        /// 获取下一个请求
        /// </summary>
        protected readonly RequestDelegate nextRequest;
        /// <summary>
        /// 获取 POST 字符串中间件
        /// </summary>
        /// <param name="nextRequest"></param>
        protected Middleware(RequestDelegate nextRequest)
        {
            this.nextRequest = nextRequest;
        }
    }
}
