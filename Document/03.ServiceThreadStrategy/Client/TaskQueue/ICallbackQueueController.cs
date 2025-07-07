using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// A one-time response API client interface example of the server-side Task asynchronous queue controller
    /// 服务端 Task 异步队列控制器 一次性响应 API 客户端接口示例
    /// </summary>
    public interface ICallbackQueueController
    {
        /// <summary>
        /// A one-time response API client queue callback API example of the server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器 一次性响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="queueKey">The queue keyword parameter must be the first parameter and its name must be queueKey
        /// 队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} or AutoCSer.Net.CommandClientCallbackQueueNode{T} or AutoCSer.Net.CommandClientCallbackQueueNode
        /// 最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} 或者 AutoCSer.Net.CommandClientCallbackQueueNode{T} 或者 AutoCSer.Net.CommandClientCallbackQueueNode</param>
        /// <returns>The return value type must be AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int queueKey, int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue> callback);
    }
}
