using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// One-time response API client queue callback API sample interface
    /// 一次性响应 API 客户端队列回调 API 示例接口
    /// </summary>
    public interface ICallbackQueueController
    {
        /// <summary>
        /// One-time response API client queue callback API sample
        /// 一次性响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} or AutoCSer.Net.CommandClientCallbackQueueNode{T} or AutoCSer.Net.CommandClientCallbackQueueNode
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} 或者 AutoCSer.Net.CommandClientCallbackQueueNode{T} 或者 AutoCSer.Net.CommandClientCallbackQueueNode</param>
        /// <returns>The return value type must be AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue> callback);
        /// <summary>
        /// One-time response API client queue callback API sample
        /// 一次性响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} or AutoCSer.Net.CommandClientCallbackQueueNode{T} or AutoCSer.Net.CommandClientCallbackQueueNode
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} 或者 AutoCSer.Net.CommandClientCallbackQueueNode{T} 或者 AutoCSer.Net.CommandClientCallbackQueueNode</param>
        /// <returns>The return value type must be AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int left, int right, AutoCSer.Net.CommandClientCallbackQueueNode<int> callback);
    }
}
