using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// One-time response API client callback API sample interface
    /// 一次性响应 API 客户端回调 API 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// One-time response API client callback API sample
        /// 一次性响应 API 客户端回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}} or Action{AutoCSer.Net.CommandClientReturnValue} or AutoCSer.Net.CommandClientCallback{T} or AutoCSer.Net.CommandClientCallback
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}} 或者 Action{AutoCSer.Net.CommandClientReturnValue} 或者 AutoCSer.Net.CommandClientCallback{T} 或者 AutoCSer.Net.CommandClientCallback</param>
        /// <returns>The return value type must be AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>> callback);
        /// <summary>
        /// One-time response API client callback API sample
        /// 一次性响应 API 客户端回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}} or Action{AutoCSer.Net.CommandClientReturnValue} or AutoCSer.Net.CommandClientCallback{T} or AutoCSer.Net.CommandClientCallback
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}} 或者 Action{AutoCSer.Net.CommandClientReturnValue} 或者 AutoCSer.Net.CommandClientCallback{T} 或者 AutoCSer.Net.CommandClientCallback</param>
        /// <returns>The return value type must be AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int left, int right, AutoCSer.Net.CommandClientCallback<int> callback);
    }
}
