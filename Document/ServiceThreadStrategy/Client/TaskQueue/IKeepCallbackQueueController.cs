using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 持续响应 API 客户端示例接口
    /// </summary>
    public interface IKeepCallbackQueueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queueKey">队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue{T} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int queueKey, int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback);
    }
}
