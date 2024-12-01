using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 服务端一次性响应 API 客户端示例接口
    /// </summary>
    public interface IReturnQueueCommandController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queueKey">队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.ReturnCommand 或者 AutoCSer.Net.ReturnCommand{T}</returns>
        AutoCSer.Net.ReturnQueueCommand<int> Add(int queueKey, int left, int right);
    }
}
