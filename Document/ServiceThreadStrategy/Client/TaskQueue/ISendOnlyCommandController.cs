using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 服务端仅执行 API 客户端示例接口
    /// </summary>
    public interface ISendOnlyCommandController
    {
        /// <summary>
        /// 服务端仅执行 API 客户端
        /// </summary>
        /// <param name="queueKey">队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.SendOnlyCommand</returns>
        AutoCSer.Net.SendOnlyCommand Call(int queueKey, int value);
    }
}
