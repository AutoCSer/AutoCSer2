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
    /// JSON API 请求实例
    /// </summary>
    public abstract class JsonApiRequest : ViewMiddlewareRequest
    {
        /// <summary>
        /// JSON API 代理控制器类型
        /// </summary>
        public readonly Type ControllerType;
        /// <summary>
        /// JSON API 方法名称
        /// </summary>
        public readonly string MethodName;
        /// <summary>
        /// JSON API 自定义配置
        /// </summary>
        internal readonly JsonApiAttribute Attribute;
        /// <summary>
        /// JSON API 方法信息标记
        /// </summary>
        internal readonly JsonApiFlags Flags;
        /// <summary>
        /// JSON API 单例代理控制器
        /// </summary>
#if NetStandard21
        private readonly JsonApiController? controller;
#else
        private readonly JsonApiController controller;
#endif
        /// <summary>
        /// 创建 JSON API 代理控制器委托
        /// </summary>
        private readonly Func<JsonApiController> createController;
        /// <summary>
        /// 请求信息
        /// </summary>
        internal override string RequestInfo { get { return $"{ControllerType.fullName()}.{MethodName}"; } }
        /// <summary>
        /// JSON API 请求实例
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="createController">创建 JSON API 代理控制器委托</param>
        /// <param name="method">JSON API 方法信息</param>
        /// <param name="controllerAttribute">JSON API 代理控制器自定义配置</param>
        /// <param name="attribute">JSON API 自定义配置</param>
        /// <param name="flags">JSON API 方法信息标记</param>
        internal JsonApiRequest(ViewMiddleware viewMiddleware, Func<JsonApiController> createController, MethodInfo method, JsonApiControllerAttribute controllerAttribute, JsonApiAttribute attribute, JsonApiFlags flags) : base(viewMiddleware)
        {
            this.createController = createController;
            switch (attribute.SingletonEnum)
            {
                case JsonApiSingletonEnum.Controller:
                    if (controllerAttribute.IsSingleton) controller = createController();
                    //else this.createController = createController;
                    break;
                //case JsonApiSingletonEnum.New: this.createController = createController; break;
                case JsonApiSingletonEnum.Singleton: controller = createController(); break;
            }
            ControllerType = (controller ?? createController()).GetType();
            MethodName = method.Name;
            Attribute = attribute;
            Flags = flags;
        }
        /// <summary>
        /// 加载请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="type">数据视图请求类型</param>
        /// <returns></returns>
        internal override async Task Request(HttpContext httpContext, ViewRequestTypeEnum type)
        {
            HttpRequest request = httpContext.Request;
            ByteArrayBuffer buffer = default(ByteArrayBuffer), stringBuffer = default(ByteArrayBuffer);
            bool isReadPostString = false, checkVersion = !Attribute.CheckReferer && (Flags & (JsonApiFlags.IsCheckRequest | JsonApiFlags.IsAccessTokenParameter)) == 0;
            ResponseResult result = ResponseStateEnum.Unknown;
            long callIdentity = long.MinValue;
            var postString = default(string);
            try
            {
                bool isPost = string.Compare(request.Method, "POST", true) == 0;
                if (isPost) checkVersion = false;
                else if (Attribute.OnlyPost)
                {
                    result.State = ResponseStateEnum.OnlyPost;
                    return;
                }
                callIdentity = ViewMiddleware.GetCallIdentity(httpContext, this);
                if (Attribute.CheckReferer)
                {
                    result = ViewMiddleware.CheckReferer(request.Headers["Referer"]);
                    if (!result.IsSuccess) return;
                }
                if ((Flags & JsonApiFlags.IsCheckRequest) != 0)
                {
                    result = await ViewMiddleware.Check(httpContext, this);
                    if (!result.IsSuccess) return;
                }
                //string jsonQueryName = ViewMiddleware.JsonQueryName;
                //if (!string.IsNullOrEmpty(jsonQueryName))
                //{
                //    string queryString = reqeust.Query[jsonQueryName];
                //    if (!string.IsNullOrEmpty(queryString))
                //    {
                //        AutoCSer.Json.DeserializeResult deserializeResult = JsonDeserialize(queryString);
                //        if (deserializeResult.State != AutoCSer.Json.DeserializeStateEnum.Success)
                //        {
                //            result = new ResponseResult(ResponseStateEnum.JsonDeserializeFail, $"GET 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
                //            return;
                //        }
                //    }
                //}
                if (isPost && (Flags & JsonApiFlags.IsPostParameter) != 0)
                {
                    switch(ViewMiddleware.GetPostType(httpContext, out Encoding contentTypeEncoding))
                    {
                        case PostTypeEnum.None:
                        case PostTypeEnum.Json:
                            break;
                        default:
                            result.State = ResponseStateEnum.NotSupportPostType;
                            return;
                    }
                    long? contentLength = httpContext.Request.ContentLength;
                    if (!contentLength.HasValue)
                    {
                        result.State = ResponseStateEnum.ContentLengthOutOfRange;
                        return;
                    }
                    long length = contentLength.Value;
                    if (length > Attribute.MaxContentLength)
                    {
                        result.State = ResponseStateEnum.ContentLengthOutOfRange;
                        return;
                    }
                    if (length > 0)
                    {
                        int size = (int)length;
                        buffer = ByteArrayPool.GetBuffer(size);
                        isReadPostString = true;
                        int index = 0;
                        do
                        {
                            int readSize = await httpContext.Request.Body.ReadAsync(buffer.Buffer.notNull().Buffer, buffer.StartIndex + index, size - index);
                            if (readSize <= 0)
                            {
                                result.State = ResponseStateEnum.ReadBodySizeError;
                                return;
                            }
                            index += readSize;
                        }
                        while (index != size);
                        isReadPostString = false;
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
                        AutoCSer.Json.DeserializeResult deserializeResult = JsonDeserialize(ref stringBuffer);
                        if (deserializeResult.State != AutoCSer.Json.DeserializeStateEnum.Success)
                        {
                            result = new ResponseResult(ResponseStateEnum.JsonDeserializeFail, $"POST 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
                            return;
                        }
                    }
                    //using (StreamReader reader = new StreamReader(request.Body))
                    //{
                    //    postString = await reader.ReadToEndAsync();
                    //    isReadPostString = false;
                    //    if (!string.IsNullOrEmpty(postString))
                    //    {
                    //        AutoCSer.Json.DeserializeResult deserializeResult = JsonDeserialize(postString);
                    //        if (deserializeResult.State != AutoCSer.Json.DeserializeStateEnum.Success)
                    //        {
                    //            result = new ResponseResult(ResponseStateEnum.JsonDeserializeFail, $"POST 数据 JSON 反序列化位置 {deserializeResult.Index.toString()} 失败 {deserializeResult.State}");
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        result.State = ResponseStateEnum.EmptyRequestBody;
                    //        await AutoCSer.LogHelper.Debug($"{request.GetDisplayUrl()} 没有读取到 POST 数据");
                    //    }
                    //}
                }
                if ((Flags & JsonApiFlags.IsCheckParameter) != 0)
                {
                    ParameterChecker checker = default(ParameterChecker);
                    CheckParameter(ref checker);
                    if (checker.Message != null)
                    {
                        result = checker.ErrorResult;
                        return;
                    }
                }
                if ((Flags & JsonApiFlags.IsAccessTokenParameter) != 0)
                {
                    result = await CheckAccessTokenParameter();
                    if (!result.IsSuccess) return;
                }
                if (Attribute.IsLog)
                {
                    postString = GetPostString(ref stringBuffer);
                    await AutoCSer.LogHelper.Info(postString.Length == 0 ? request.GetDisplayUrl() : (request.GetDisplayUrl() + "\r\n" + postString));
                }
                result = await call(httpContext, controller ?? createController(), checkVersion &= !Attribute.IsStaticVersion);
            }
            catch (Microsoft.AspNetCore.Connections.ConnectionResetException exception)
            {
                result.State = ResponseStateEnum.Exception;
                if (isReadPostString) await AutoCSer.LogHelper.Exception(exception, $"{request.GetDisplayUrl()} 读取 POST 数据断线异常", LogLevelEnum.Debug | LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            catch (Exception exception)
            {
                result.State = ResponseStateEnum.Exception;
                if (isReadPostString) await AutoCSer.LogHelper.Exception(exception, $"{request.GetDisplayUrl()} 读取 POST 数据异常", LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                else
                {
                    if (postString == null) postString = GetPostString(ref stringBuffer);
                    await AutoCSer.LogHelper.Exception(exception, postString.Length == 0 ? request.GetDisplayUrl() : (request.GetDisplayUrl() + "\r\n" + postString));
                }
            }
            finally
            {
                if (callIdentity != long.MinValue) ViewMiddleware.OnCalled(callIdentity);
                buffer.FreeCopy(ref stringBuffer);
                if (!result.IsSuccess) await ViewMiddleware.ResponseError(httpContext, result, Attribute.IsResponseJavaScript, checkVersion);
            }
        }
        /// <summary>
        /// JSON 反序列化参数
        /// </summary>
        /// <param name="postBuffer"></param>
        /// <returns></returns>
        public virtual AutoCSer.Json.DeserializeResult JsonDeserialize(ref ByteArrayBuffer postBuffer)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 检查传参
        /// </summary>
        /// <param name="checker"></param>
        public virtual void CheckParameter(ref ParameterChecker checker)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 参数鉴权
        /// </summary>
        /// <returns></returns>
        public virtual Task<ResponseResult> CheckAccessTokenParameter()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// API 调用
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="controller"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        protected abstract Task<ResponseResult> call(HttpContext httpContext, JsonApiController controller, bool checkVersion);
        /// <summary>
        /// 加载请求
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="requestPath">请求路径</param>
        /// <param name="startIndex">模板解析开始位置</param>
        /// <returns></returns>
        internal Task Request(HttpContext httpContext, string requestPath, int startIndex)
        {
            RouteParameter routeParameter = new RouteParameter(requestPath, startIndex);
            GetRouteParameter(ref routeParameter);
            ResponseResult result = routeParameter.End();
            if (result.IsSuccess) return Request(httpContext, ViewRequestTypeEnum.JsonApi);
            return ViewMiddleware.ResponseError(httpContext, result, Attribute.IsResponseJavaScript, true);
        }
        /// <summary>
        /// 获取路由参数
        /// </summary>
        /// <param name="routeParameter">路由参数解析</param>
        public virtual void GetRouteParameter(ref RouteParameter routeParameter)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 获取 POST 字符串
        /// </summary>
        /// <param name="postBuffer"></param>
        /// <returns></returns>
        internal unsafe static string GetPostString(ref ByteArrayBuffer postBuffer)
        {
            if (postBuffer.CurrentIndex != 0)
            {
                fixed (byte* stringBufferFixed = postBuffer.GetFixedBuffer())
                {
                    return new string((char*)(stringBufferFixed + postBuffer.StartIndex), 0, postBuffer.CurrentIndex);
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// JSON 反序列化参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="postBuffer"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal unsafe static AutoCSer.Json.DeserializeResult JsonDeserializeParameter<T>(ref ByteArrayBuffer postBuffer, ref T parameter)
            where T : struct
        {
            fixed (byte* stringBufferFixed = postBuffer.GetFixedBuffer())
            {
                return AutoCSer.JsonDeserializer.UnsafeDeserialize((char*)(stringBufferFixed + postBuffer.StartIndex), postBuffer.CurrentIndex, ref parameter);
            }
        }
    }
    /// <summary>
    /// JSON API 请求实例
    /// </summary>
    /// <typeparam name="T">API 方法返回值类型</typeparam>
    internal abstract class JsonApiRequest<T> : JsonApiRequest
    {
        /// <summary>
        /// JSON API 请求实例
        /// </summary>
        /// <param name="viewMiddleware">数据视图中间件</param>
        /// <param name="createController">创建 JSON API 代理控制器委托</param>
        /// <param name="method">JSON API 方法信息</param>
        /// <param name="controllerAttribute">JSON API 代理控制器自定义配置</param>
        /// <param name="attribute">JSON API 自定义配置</param>
        /// <param name="flags">JSON API 方法信息标记</param>
        protected JsonApiRequest(ViewMiddleware viewMiddleware, Func<JsonApiController> createController, MethodInfo method, JsonApiControllerAttribute controllerAttribute, JsonApiAttribute attribute, JsonApiFlags flags)
            : base(viewMiddleware, createController, method, controllerAttribute, attribute, flags)
        {
        }
        /// <summary>
        /// API 调用
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public abstract Task<T> Call(JsonApiController controller);
    }
}
