using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// A one-time response API client interface example of the server-side Task asynchronous queue controller
    /// 服务端 Task 异步队列控制器 一次性响应 API 客户端接口示例
    /// </summary>
    public interface IReturnCommandController
    {
        /// <summary>
        /// A one-time response API client await API example of the server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器 一次性响应 API 客户端 await API 示例
        /// </summary>
        /// <param name="queueKey">The queue keyword parameter must be the first parameter and its name must be queueKey
        /// 队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.ReturnCommand or AutoCSer.Net.ReturnCommand{T}</returns>
        AutoCSer.Net.ReturnCommand<int> Add(int queueKey, int left, int right);
    }
}
