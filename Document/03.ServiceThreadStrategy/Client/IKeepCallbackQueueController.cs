using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// Keep response API client queue callback API sample interface
    /// 持续响应 API 客户端队列回调 API 示例接口
    /// </summary>
    public interface IKeepCallbackQueueController
    {
        /// <summary>
        /// Keep response API client queue callback API sample
        /// 持续响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or AutoCSer.Net.CommandClientKeepCallbackQueue{T} or AutoCSer.Net.CommandClientKeepCallbackQueue
        /// 最后一个参数类型必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue{T} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback);
        /// <summary>
        /// Keep response API client queue callback API sample
        /// 持续响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or AutoCSer.Net.CommandClientKeepCallbackQueue{T} or AutoCSer.Net.CommandClientKeepCallbackQueue
        /// 最后一个参数类型必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue{T} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int left, int right, AutoCSer.Net.CommandClientKeepCallbackQueue<int> callback);
    }
}
