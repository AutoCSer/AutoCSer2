using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// A keep response API client interface example of the server-side Task asynchronous queue controller
    /// 服务端 Task 异步队列控制器 持续响应 API 客户端接口示例
    /// </summary>
    public interface IKeepCallbackQueueController
    {
        /// <summary>
        /// A keep response API client queue callback API example of the server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器 持续响应 API 客户端队列回调 API 示例
        /// </summary>
        /// <param name="queueKey">The queue keyword parameter must be the first parameter and its name must be queueKey
        /// 队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} or AutoCSer.Net.CommandClientKeepCallbackQueue{T} or AutoCSer.Net.CommandClientKeepCallbackQueue
        /// 最后一个参数类型必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue{T} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int queueKey, int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback);
    }
}
