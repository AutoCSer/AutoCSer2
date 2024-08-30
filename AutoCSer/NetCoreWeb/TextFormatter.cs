using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 文本输出
    /// </summary>
    public sealed class TextFormatter : IOutputFormatter
    {
        /// <summary>
        /// 文本
        /// </summary>
        public const string ContentType = "text/plain; charset=utf-8";

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

            string text = context.Object as string;
            if (!string.IsNullOrEmpty(text)) await response.WriteAsync(text, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 默认文本输出
        /// </summary>
        internal static readonly TextFormatter Default = new TextFormatter();
    }
}
