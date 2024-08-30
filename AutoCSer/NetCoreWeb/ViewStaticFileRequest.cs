using AutoCSer.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 静态文件请求实例
    /// </summary>
    internal sealed class ViewStaticFileRequest : ViewMiddlewareRequest
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        private readonly FileInfo file;
        /// <summary>
        /// 请求信息
        /// </summary>
        internal override string RequestInfo { get { return file.FullName; } }
        /// <summary>
        /// ETag 标记
        /// </summary>
        private readonly StringValues eTag;
        /// <summary>
        /// HTTP 状态编码
        /// </summary>
        private int code;
        /// <summary>
        /// 文件数据内容
        /// </summary>
        private byte[] fileData;
        /// <summary>
        /// 静态文件请求实例
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="file">文件信息</param>
        internal ViewStaticFileRequest(ViewMiddleware viewMiddleware, FileInfo file) : base(viewMiddleware)
        {
            this.file = file;
            eTag = "\"" + ((ulong)file.LastWriteTimeUtc.Ticks).toHex() + "\"";
        }
        /// <summary>
        /// 加载请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="type">数据视图请求类型</param>
        /// <returns></returns>
        internal override async Task Request(HttpContext httpContext, ViewRequestTypeEnum type)
        {
            HttpRequest httpRequest = httpContext.Request;
            HttpResponse httpResponse = httpContext.Response;
            if (httpRequest.Headers.TryGetValue("If-None-Match", out StringValues eTag) && this.eTag.Equals(eTag))
            {
                httpResponse.StatusCode = StatusCodes.Status304NotModified;
                return;
            }
            do
            {
                switch (code)
                {
                    case 0:
                        await ViewMiddleware.LoadLock.WaitAsync();
                        try
                        {
                            if (code == 0)
                            {
                                if (await AutoCSer.Common.Config.FileExists(file))
                                {
                                    fileData = await AutoCSer.Common.Config.ReadFileAllBytes(file.FullName);
                                    code = 1;
                                }
                                else
                                {
                                    code = StatusCodes.Status404NotFound;
                                    await AutoCSer.LogHelper.Error($"没有找到文件 {file.FullName}");
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            code = StatusCodes.Status500InternalServerError;
                            await AutoCSer.LogHelper.Exception(exception, file.FullName);
                        }
                        finally { ViewMiddleware.LoadLock.Release(); }
                        break;
                    case 1:
                        string versionQueryName = ViewMiddleware.VersionQueryName;
                        if (!string.IsNullOrEmpty(versionQueryName) && httpContext.Request.Query.ContainsKey(versionQueryName)) httpResponse.Headers.Add("Cache-Control", ViewMiddleware.StaticFileCacheControl);
                        else httpResponse.Headers.Add("ETag", this.eTag);
                        switch (type)
                        {
                            case ViewRequestTypeEnum.Html: httpResponse.ContentType = ResponseContentType.GetHtml(ViewMiddleware.ResponseEncoding); break;
                            case ViewRequestTypeEnum.JavaScript: httpResponse.ContentType = ResponseContentType.GetJavaScript(ViewMiddleware.ResponseEncoding); break;
                        }
                        await ViewMiddleware.Response(httpContext, fileData, 0, fileData.Length, false);
                        return;
                    default: httpContext.Response.StatusCode = code; return;
                }
            }
            while (true);
        }
    }
}
