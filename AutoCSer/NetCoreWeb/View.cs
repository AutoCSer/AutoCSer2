using AutoCSer.Extensions;
using AutoCSer.Memory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 数据视图，用于支持 统一规划全局视图，实例类型必须使用 partial 修饰符用于生成静态代码，数据初始化需要定义 Task{AutoCSer.NetCoreWeb.ResponseResult} LoadView 方法，参数定义则对应查询参数
    /// </summary>
    public class View
    {
        /// <summary>
        /// 成功返回值状态任务
        /// </summary>
        public static readonly Task<ResponseResult> SuccessResponseResultTask = Task.FromResult((ResponseResult)ResponseStateEnum.Success);

        /// <summary>
        /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成，不允许重写该实现
        /// </summary>
        protected virtual string defaultRequestPath { get { return string.Empty; } }
        /// <summary>
        /// 获取请求路径，默认为 类型命令空间+类型名称，用于代码生成
        /// </summary>
        protected virtual string requestPath { get { return defaultRequestPath; } }
        /// <summary>
        /// 请求路径
        /// </summary>
        internal string RequestPath { get { return requestPath; } }
        /// <summary>
        /// 默认为空字符串表示不生成参数成员，仅支持英文字母与数字不支持符号，用于代码生成
        /// </summary>
        protected virtual string queryName { get { return string.Empty; } }
        /// <summary>
        /// 生成参数成员名称
        /// </summary>
        internal string QueryName { get { return queryName; } }
        /// <summary>
        /// 最大 POST 字节数，默认为 1MB
        /// </summary>
        protected virtual int maxContentLength { get { return 1 << 20; } }
        /// <summary>
        /// 输出文本编码，默认为 null 表示采用中间件编码 AutoCSer.NetCoreWeb.ViewMiddleware.ResponseEncoding
        /// </summary>
#if NetStandard21
        protected virtual Encoding? responseEncoding { get { return null; } }
#else
        protected virtual Encoding responseEncoding { get { return null; } }
#endif
        /// <summary>
        /// 输出文本编码
        /// </summary>
#if NetStandard21
        internal Encoding? ResponseEncoding { get { return responseEncoding; } }
#else
        internal Encoding ResponseEncoding { get { return responseEncoding; } }
#endif
        /// <summary>
        /// 调用监视超时毫秒数默认为 5000ms
        /// </summary>
        public virtual int MonitorTimeoutMilliseconds { get { return 5000; } }
        /// <summary>
        /// 是否检查数据视图，配合 AutoCSer.NetCoreWeb.IAccessTokenParameter 一般用于 HTTP 头部参数鉴权
        /// </summary>
        protected virtual bool isCheckView { get { return false; } }
        /// <summary>
        /// 默认为 true 表示当参数没有配置 AutoCSer.NetCoreWeb.ParameterConstraintAttribute 时按照默认约束处理（IsParameterConstraint / IsEmpty / IsDefault），设置为 false 则仅检查 AutoCSer.NetCoreWeb.IParameterConstraint 约束
        /// </summary>
        protected virtual bool isDefaultParameterConstraint { get { return true; } }
        /// <summary>
        /// 参数约束配置
        /// </summary>
        internal bool IsDefaultParameterConstraint { get { return isDefaultParameterConstraint; } }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <returns></returns>
        internal async Task Load(HttpContext httpContext, ViewRequest viewInfo)
        {
            long callIdentity = long.MinValue;
            ResponseResult result = ResponseStateEnum.Unknown;
            bool checkVersion = true, isException = true;
            try
            {
                callIdentity = viewInfo.ViewMiddleware.GetCallIdentity(httpContext, this);
                result = await checkView(httpContext, viewInfo);
                if (result.IsSuccess) result = await load(httpContext, viewInfo);
                isException = false;
            }
            catch (Exception exception)
            {
                await AutoCSer.LogHelper.Exception(exception, httpContext.Request.GetDisplayUrl());
                result.State = ResponseStateEnum.Exception;
                checkVersion = false;
            }
            finally
            {
                if (callIdentity != long.MinValue) viewInfo.ViewMiddleware.OnCallCompleted(callIdentity, isException);
                if (!result.IsSuccess) await responseError(httpContext, viewInfo, result, checkVersion);
            }
        }
        /// <summary>
        /// 初始化加载数据（基本操作用代码生成组件处理）
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <returns></returns>
        protected virtual Task<ResponseResult> load(HttpContext httpContext, ViewRequest viewInfo)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 检查数据视图，一般用于 HTTP 头部参数鉴权
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <returns></returns>
        protected virtual Task<ResponseResult> checkView(HttpContext httpContext, ViewRequest viewInfo)
        {
            return isCheckView ? viewInfo.ViewMiddleware.Check(httpContext, this) : SuccessResponseResultTask;
        }
        /// <summary>
        /// 获取视图数据输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected ViewResponse getResponse()
        {
            return new ViewResponse(this);
        }
        /// <summary>
        /// 输出错误状态
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <param name="result">错误状态</param>
        /// <param name="checkVersion">是否检查静态版本信息</param>
        /// <returns></returns>
        protected async Task responseError(HttpContext httpContext, ViewRequest viewInfo, ResponseResult result, bool checkVersion)
        {
            ViewResponse response = getResponse();
            try
            {
                Encoding encoding = responseEncoding ?? viewInfo.ViewMiddleware.ResponseEncoding;
                ByteArrayBuffer buffer = response.Error(ref result, httpContext.Request.Query[viewInfo.ViewMiddleware.CallbackQueryName], encoding, true);
                try
                {
                    HttpResponse httpResponse = httpContext.Response;
                    httpResponse.ContentType = ResponseContentType.GetJavaScript(encoding);
                    await viewInfo.ViewMiddleware.Response(httpContext, buffer.Buffer.notNull().Buffer, buffer.StartIndex, buffer.CurrentIndex, checkVersion);
                }
                finally { buffer.Free(); }
            }
            finally { response.Free(); }
        }
        /// <summary>
        /// 开始输出
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <param name="response"></param>
        /// <returns>输出流</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected CharStream responseStart(HttpContext httpContext, ViewRequest viewInfo, ref ViewResponse response)
        {
            return response.Start(httpContext.Request.Query[viewInfo.ViewMiddleware.CallbackQueryName], true);
        }
        /// <summary>
        /// 结束输出
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <param name="response">视图数据输出</param>
        /// <returns></returns>
        protected async Task responseEnd(HttpContext httpContext, ViewRequest viewInfo, ViewResponse response)
        {
            Encoding encoding = responseEncoding ?? viewInfo.ViewMiddleware.ResponseEncoding;
            ByteArrayBuffer buffer = response.End(encoding);
            try
            {
                httpContext.Response.ContentType = ResponseContentType.GetJavaScript(encoding);
                await viewInfo.ViewMiddleware.Response(httpContext, buffer.Buffer.notNull().Buffer, buffer.StartIndex, buffer.CurrentIndex, viewInfo.Attribute.IsStaticVersion);
            }
            finally { buffer.Free(); }
        }
        /// <summary>
        /// 获取查询参数对象
        /// </summary>
        /// <typeparam name="T">查询参数对象类型</typeparam>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="viewInfo">数据视图信息</param>
        /// <returns></returns>
#if NetStandard21
        protected virtual async Task<ResponseResult<T?>> getParameter<T>(HttpContext httpContext, ViewRequest viewInfo)
#else
        protected virtual async Task<ResponseResult<T>> getParameter<T>(HttpContext httpContext, ViewRequest viewInfo)
#endif
        {
#if NetStandard21
            ResponseResult<T?> result = default(T);
#else
            ResponseResult<T> result = default(T);
#endif
            if (string.Compare(httpContext.Request.Method, "POST", true) == 0)
            {
                PostTypeEnum postType = viewInfo.ViewMiddleware.GetPostType(httpContext, out Encoding contentTypeEncoding);
                if (postType != PostTypeEnum.None) result = await getPostParameter<T>(httpContext, postType, contentTypeEncoding);
            }
            if (result.IsSuccess)
            {
                string key = viewInfo.ViewMiddleware.JsonQueryName; 
                if (!string.IsNullOrEmpty(key))
                {
                    string queryString = httpContext.Request.Query[key].ToString();
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        AutoCSer.Json.DeserializeResult deserializeResult = AutoCSer.JsonDeserializer.Deserialize(queryString, ref result.Result);
                        if (deserializeResult.State == Json.DeserializeStateEnum.Success) return result;
#if NetStandard21
                        return new ResponseResult<T?>(ResponseStateEnum.JsonDeserializeFail, $"GET 查询 {key} 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
#else
                        return new ResponseResult<T>(ResponseStateEnum.JsonDeserializeFail, $"GET 查询 {key} 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
#endif
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取查询参数对象
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="postType">POST 数据类型</param>
        /// <param name="contentTypeEncoding">POST 数据文本编码类型</param>
        /// <returns></returns>
#if NetStandard21
        private async Task<ResponseResult<T?>> getPostParameter<T>(HttpContext httpContext, PostTypeEnum postType, Encoding contentTypeEncoding)
#else
        private async Task<ResponseResult<T>> getPostParameter<T>(HttpContext httpContext, PostTypeEnum postType, Encoding contentTypeEncoding)
#endif
        {
            long? contentLength = httpContext.Request.ContentLength;
            if (contentLength.HasValue)
            {
                long length = contentLength.Value;
                if (length <= maxContentLength)
                {
                    if (length > 0)
                    {
                        int size = (int)length;
                        ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(size), stringBuffer = default(ByteArrayBuffer);
                        try
                        {
                            int index = 0;
                            do
                            {
                                int readSize = await httpContext.Request.Body.ReadAsync(buffer.Buffer.notNull().Buffer, buffer.StartIndex + index, size - index);
                                if (readSize <= 0) return ResponseStateEnum.ReadBodySizeError;
                                index += readSize;
                            }
                            while (index != size);
                            switch (postType)
                            {
                                case PostTypeEnum.Form:
                                case PostTypeEnum.FormData:
                                    return default(T);
                                case PostTypeEnum.Unknown:
                                    switch (buffer.Buffer.notNull().Buffer[buffer.StartIndex])
                                    {
                                        case (byte)'{': postType = PostTypeEnum.Json; break;
                                        case (byte)'<': postType = PostTypeEnum.Xml; break;
                                    }
                                    break;
                            }
                            switch (postType)
                            {
                                case PostTypeEnum.Json:
                                case PostTypeEnum.Xml:
                                    buffer.CurrentIndex = size;
                                    if (contentTypeEncoding.CodePage != AutoCSer.Common.UnicodeCodePage)
                                    {
                                        int stringSize = contentTypeEncoding.GetCharCount(buffer.Buffer.notNull().Buffer, buffer.StartIndex, size);
                                        stringBuffer = ByteArrayPool.GetBuffer(stringSize << 1);
                                        stringBuffer.CurrentIndex = stringSize;
                                    }
                                    else
                                    {
                                        stringBuffer = buffer;
                                        stringBuffer.CurrentIndex >>= 1;
                                    }
                                    ViewMiddleware.GetPostString(ref buffer, ref stringBuffer, contentTypeEncoding);
                                    switch (postType)
                                    {
                                        case PostTypeEnum.Json: return jsonDeserialize<T>(ref stringBuffer);
                                        case PostTypeEnum.Xml: return xmlDeserialize<T>(ref stringBuffer);
                                    }
                                    break;
                            }
                        }
                        finally { buffer.FreeCopy(ref stringBuffer); }
                    }
                    return default(T);
                }
            }
            return ResponseStateEnum.ContentLengthOutOfRange;
        }

        /// <summary>
        /// 获取数据分割符字符串
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private static SubString getBoundary(string contentType, int startIndex)
        {
            if (startIndex < contentType.Length)
            {
                startIndex = new SubString(startIndex, contentType.Length - startIndex, contentType).IndexLower('b');
                if (startIndex >= 0 && (startIndex + 9) < contentType.Length && new SubString(startIndex, 9, contentType).LowerEquals("boundary="))
                {
                    int size = contentType.Length - (startIndex += 9);
                    return new SubString(startIndex, size, contentType);
                }
            }
            return default(SubString);
        }
        /// <summary>
        /// JSON 反序列化查询参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringBuffer"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static ResponseResult<T?> jsonDeserialize<T>(ref ByteArrayBuffer stringBuffer)
#else
        private unsafe static ResponseResult<T> jsonDeserialize<T>(ref ByteArrayBuffer stringBuffer)
#endif
        {
            var value = default(T);
            fixed (byte* stringBufferFixed = stringBuffer.GetFixedBuffer())
            {
                AutoCSer.Json.DeserializeResult deserializeResult = AutoCSer.JsonDeserializer.UnsafeDeserialize((char*)(stringBufferFixed + stringBuffer.StartIndex), stringBuffer.CurrentIndex, ref value);
                if (deserializeResult.State == Json.DeserializeStateEnum.Success) return value;
#if NetStandard21
                return new ResponseResult<T?>(ResponseStateEnum.JsonDeserializeFail, $"POST 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
#else
                return new ResponseResult<T>(ResponseStateEnum.JsonDeserializeFail, $"POST 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
#endif
            }
        }
        /// <summary>
        /// XML 反序列化查询参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringBuffer"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static ResponseResult<T?> xmlDeserialize<T>(ref ByteArrayBuffer stringBuffer)
#else
        private unsafe static ResponseResult<T> xmlDeserialize<T>(ref ByteArrayBuffer stringBuffer)
#endif
        {
            var value = default(T);
            fixed (byte* stringBufferFixed = stringBuffer.GetFixedBuffer())
            {
                AutoCSer.Xml.DeserializeResult deserializeResult = AutoCSer.XmlDeserializer.UnsafeDeserialize((char*)(stringBufferFixed + stringBuffer.StartIndex), stringBuffer.CurrentIndex, ref value);
                if (deserializeResult.State == AutoCSer.Xml.DeserializeStateEnum.Success) return value;
#if NetStandard21
                return new ResponseResult<T?>(ResponseStateEnum.XmlDeserializeFail, $"POST 数据 XML 反序列化位置 {deserializeResult.Index} 失败 {deserializeResult.State}");
#else
                return new ResponseResult<T>(ResponseStateEnum.XmlDeserializeFail, $"POST 数据 XML 反序列化位置 {deserializeResult.Index} 失败 {deserializeResult.State}");
#endif
            }
        }
        /// <summary>
        /// 获取数据视图初始化加载方法信息
        /// </summary>
        /// <param name="type">数据视图类型</param>
        /// <returns></returns>
#if NetStandard21
        internal static MethodInfo? GetLoadMethod(Type type)
#else
        internal static MethodInfo GetLoadMethod(Type type)
#endif
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.ReturnType == typeof(Task<ResponseResult>) && !method.IsAbstract && !method.IsGenericMethod && method.Name == "LoadView")
                {
                    return method;
                }
            }
            return null;
        }

        /// <summary>
        /// 默认空数据视图
        /// </summary>
        internal static readonly View Null = new View();
    }
}
