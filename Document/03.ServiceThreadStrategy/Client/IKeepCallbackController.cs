using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// Keep response API client callback API sample interface
    /// 持续响应 API 客户端回调 API 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// Keep response API client callback API sample
        /// 持续响应 API 客户端回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand} or AutoCSer.Net.CommandClientKeepCallback{T} or AutoCSer.Net.CommandClientKeepCallback
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallback{T} 或者 AutoCSer.Net.CommandClientKeepCallback</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.KeepCallbackCommand> callback);
        /// <summary>
        /// Keep response API client callback API sample
        /// 持续响应 API 客户端回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand} or AutoCSer.Net.CommandClientKeepCallback{T} or AutoCSer.Net.CommandClientKeepCallback
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallback{T} 或者 AutoCSer.Net.CommandClientKeepCallback</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int left, int right, AutoCSer.Net.CommandClientKeepCallback<int> callback);
    }
}
