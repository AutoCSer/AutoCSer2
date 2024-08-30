using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// HTML 输出
    /// </summary>
    public sealed class HtmlFormatter : IOutputFormatter
    {
        /// <summary>
        /// HTML
        /// </summary>
        public const string ContentType = "text/html; charset=utf-8";

        /// <summary>
        /// 判断是否输出
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IOutputFormatter.CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return context.ContentType == ContentType;
        }
        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task IOutputFormatter.WriteAsync(OutputFormatterWriteContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.ContentType = ContentType;

            string html = context.Object as string;
            if (!string.IsNullOrEmpty(html)) await response.WriteAsync(html, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 默认 HTML 输出
        /// </summary>
        internal static readonly HtmlFormatter Default = new HtmlFormatter();
    }
}
