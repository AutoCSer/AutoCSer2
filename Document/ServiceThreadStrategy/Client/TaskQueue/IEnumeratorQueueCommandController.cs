using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 持续响应 API 客户端示例接口
    /// </summary>
    public interface IEnumeratorQueueCommandController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queueKey">队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.EnumeratorQueueCommand 或者 AutoCSer.Net.EnumeratorQueueCommand{T}</returns>
        AutoCSer.Net.EnumeratorQueueCommand<int> Callback(int queueKey, int left, int right);
    }
}
