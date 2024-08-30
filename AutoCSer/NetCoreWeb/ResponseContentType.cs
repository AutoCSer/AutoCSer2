using System;
using System.Text;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 输出类型字符串
    /// </summary>
    internal sealed class ResponseContentType
    {
        /// <summary>
        /// 输出文本编码编号
        /// </summary>
        private readonly int encodingCodePage;
        /// <summary>
        /// 输出类型字符串
        /// </summary>
        private readonly string contentType;
        /// <summary>
        /// 输出类型字符串
        /// </summary>
        /// <param name="encoding">输出文本编码</param>
        /// <param name="contentType">输出类型字符串</param>
        internal ResponseContentType(Encoding encoding, string contentType)
        {
            encodingCodePage = encoding.CodePage;
            this.contentType = contentType;
        }

        /// <summary>
        /// JavaScript 输出类型字符串
        /// </summary>
        private const string javascriptString = "text/javascript; charset=";
        /// <summary>
        /// JavaScript 输出类型字符串
        /// </summary>
        private static ResponseContentType javascript = new ResponseContentType(Encoding.UTF8, javascriptString + Encoding.UTF8.WebName);
        /// <summary>
        /// 获取 JavaScript 输出类型字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal static string GetJavaScript(Encoding encoding)
        {
            ResponseContentType contentType = javascript;
            if (contentType.encodingCodePage == encoding.CodePage) return contentType.contentType;
            javascript = contentType = new ResponseContentType(encoding, javascriptString + encoding.WebName);
            return contentType.contentType;
        }
        /// <summary>
        /// HTML 输出类型字符串
        /// </summary>
        private const string htmlString = "text/html; charset=";
        /// <summary>
        /// HTML 输出类型字符串
        /// </summary>
        private static ResponseContentType html = new ResponseContentType(Encoding.UTF8, htmlString + Encoding.UTF8.WebName);
        /// <summary>
        /// 获取 HTML 输出类型字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        internal static string GetHtml(Encoding encoding)
        {
            ResponseContentType contentType = html;
            if (contentType.encodingCodePage == encoding.CodePage) return contentType.contentType;
            html = contentType = new ResponseContentType(encoding, htmlString + encoding.WebName);
            return contentType.contentType;
        }
    }
}
