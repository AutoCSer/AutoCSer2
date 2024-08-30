using AutoCSer.NetCoreWeb;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.NetCoreWeb
{
    /// <summary>
    /// JSON API 控制器示例，必须派生自 AutoCSer.NetCoreWeb.JsonApiController
    /// </summary>
    //[JsonApiController] //默认路由为 /Example
    public sealed class ExampleController : JsonApiController
    {
        /// <summary>
        /// 无参调用示例
        /// </summary>
        /// <returns></returns>
        //[JsonApi] //默认路由为 /Example/CallState
        public Task<ResponseResult> CallState()
        {
            return Task.FromResult((ResponseResult)ResponseStateEnum.Success);
        }
        /// <summary>
        /// GET 请求示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [JsonApi("")]//空字符串则表示 GET 请求所有参数，等价于 /Example/GetResult/{left}/{right}
        public Task<ResponseResult<int>> GetResult(int left, int right)
        {
            return Task.FromResult((ResponseResult<int>)(left + right));
        }
        /// <summary>
        /// 仅支持 POST 请求示例，安全敏感操作必选项，避免跨域 GET 调用攻击
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [JsonApi(OnlyPost = true)]//仅支持 POST 请求，默认路由为 /Example/OnlyPost
        public Task<ResponseResult<int>> OnlyPost(int left, int right)
        {
            return Task.FromResult((ResponseResult<int>)(left + right));
        }
        /// <summary>
        /// GET / POST 混合传参示例
        /// </summary>
        /// <param name="left">GET 传参，POST 传参优先，也就是说 POST 传参可以覆盖 GET 传参</param>
        /// <param name="right">POST 传参</param>
        /// <returns></returns>
        [JsonApi("GetPost/{left}")]//路由为 /Example/GetPost/{left}
        public Task<ResponseResult<int>> GetPost(int left, [ParameterConstraint(IsDefault = true)] int right)
        {
            return Task.FromResult((ResponseResult<int>)(left + right));
        }
        /// <summary>
        /// 忽略控制器路由
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [JsonApi("/IgnoreControllerRoute/{left}/{right}")]// / 开始表示忽略控制器路由
        public Task<ResponseResult<int>> IgnoreControllerRoute(int left, int right)
        {
            return Task.FromResult((ResponseResult<int>)(left + right));
        }

        /// <summary>
        /// 收集客户端错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [JsonApi(ViewMiddleware.DefaultErrorRequestPath)]
        public async Task<ResponseResult> ClientError(ClientMessage message)
        {
            await AutoCSer.LogHelper.Error(AutoCSer.JsonSerializer.Serialize(message));
            return ResponseStateEnum.Success;
        }
    }
}
