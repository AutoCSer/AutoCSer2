using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// JSON API 请求实例
    /// </summary>
    internal abstract class JsonApiResultRequest : JsonApiRequest<ResponseResult>
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
        protected JsonApiResultRequest(ViewMiddleware viewMiddleware, Func<JsonApiController> createController, MethodInfo method, JsonApiControllerAttribute controllerAttribute, JsonApiAttribute attribute, JsonApiFlags flags)
            : base(viewMiddleware, createController, method, controllerAttribute, attribute, flags)
        {
        }
        /// <summary>
        /// API 调用
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="controller"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        protected override async Task<ResponseResult> call(HttpContext httpContext, JsonApiController controller, bool checkVersion)
        {
            ResponseResult result = await Call(controller);
            if (result.IsSuccess) await ViewMiddleware.ResponseSuccess(httpContext, Attribute.IsResponseJavaScript, checkVersion);
            return result;
        }
    }
    /// <summary>
    /// JSON API 请求实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class JsonApiResultRequest<T> : JsonApiRequest<ResponseResult<T>>
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
        protected JsonApiResultRequest(ViewMiddleware viewMiddleware, Func<JsonApiController> createController, MethodInfo method, JsonApiControllerAttribute controllerAttribute, JsonApiAttribute attribute, JsonApiFlags flags)
            : base(viewMiddleware, createController, method, controllerAttribute, attribute, flags)
        {
        }
        /// <summary>
        /// API 调用
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        /// <param name="controller"></param>
        /// <param name="checkVersion"></param>
        /// <returns></returns>
        protected override async Task<ResponseResult> call(HttpContext httpContext, JsonApiController controller, bool checkVersion)
        {
            ResponseResult<T> result = await Call(controller);
            if (result.IsSuccess)
            {
                await ViewMiddleware.ResponseSuccess(httpContext, result.Result, Attribute.IsResponseJavaScript, checkVersion);
                return ResponseStateEnum.Success;
            }
            return result;
        }
    }
#if DEBUG && NetStandard21
#pragma warning disable
    internal struct JsonApiResultRequestParameter
    {
        public int value;
    }
    internal sealed class JsonApiControllerIL : JsonApiController
    {
        public Task<ResponseResult> Call(int value) { return null; }
    }
    internal sealed class JsonApiResultRequestIL : JsonApiResultRequest
    {
        private JsonApiResultRequestParameter parameter;
        public JsonApiResultRequestIL(ViewMiddleware viewMiddleware, Func<JsonApiController> createController, MethodInfo method, JsonApiControllerAttribute controllerAttribute, JsonApiAttribute attribute)
            : base(viewMiddleware, createController, method, controllerAttribute, attribute, 0)
        {
        }
        public override AutoCSer.Json.DeserializeResult JsonDeserialize(ref AutoCSer.Memory.ByteArrayBuffer postBuffer)
        {
            return JsonApiRequest.JsonDeserializeParameter(ref postBuffer, ref parameter);
        }
        public override void CheckParameter(ref ParameterChecker checker)
        {
            if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(parameter.value, nameof(parameter.value), "summary", ref checker)) return;
            if (!AutoCSer.NetCoreWeb.ParameterChecker.CheckEquatable(parameter.value, nameof(parameter.value), "summary", ref checker)) return;
        }
        public override Task<ResponseResult> CheckAccessTokenParameter()
        {
            return ViewMiddleware.CheckAccessTokenParameter(ViewMiddleware, parameter.value);
        }
        public override void GetRouteParameter(ref RouteParameter routeParameter)
        {
            if (routeParameter.Get("", ref parameter.value) && routeParameter.Get("", ref parameter.value)) ;
        }
        public override Task<ResponseResult> Call(JsonApiController controller)
        {
            return ((JsonApiControllerIL)controller).Call(parameter.value);
        }
    }
#endif
}
