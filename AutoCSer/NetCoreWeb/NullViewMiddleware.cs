using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 默认空数据视图中间件
    /// </summary>
    internal sealed class NullViewMiddleware : ViewMiddleware
    {
        /// <summary>
        /// 默认空请求处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task nullRequestDelegate(HttpContext context)
        {
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 默认空请求处理委托
        /// </summary>
        internal static readonly RequestDelegate NullRequestDelegate = nullRequestDelegate;
        /// <summary>
        /// 默认空数据视图中间件
        /// </summary>
        internal static readonly NullViewMiddleware Null = new NullViewMiddleware();
    }
}
