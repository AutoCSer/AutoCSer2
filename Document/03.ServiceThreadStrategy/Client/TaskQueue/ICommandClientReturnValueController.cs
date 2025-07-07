using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// A one-time response API client interface example of the server-side Task asynchronous queue controller
    /// 服务端 Task 异步队列控制器 一次性响应 API 客户端接口示例
    /// </summary>
    public interface ICommandClientReturnValueController
    {
        /// <summary>
        /// A one-time response API client synchronization API example of the server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器 一次性响应 API 客户端同步 API 示例
        /// </summary>
        /// <param name="queueKey">The queue keyword parameter must be the first parameter and its name must be queueKey
        /// 队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandClientReturnValue or AutoCSer.Net.CommandClientReturnValue{T}</returns>
        AutoCSer.Net.CommandClientReturnValue<int> Add(int queueKey, int left, int right);
    }
}
