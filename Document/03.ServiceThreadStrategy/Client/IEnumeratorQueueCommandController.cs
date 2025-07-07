using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// Keep response API client queue await API sample interface
    /// 持续响应 API 客户端队列 await API 示例接口
    /// </summary>
    public interface IEnumeratorQueueCommandController
    {
        /// <summary>
        /// Keep response API client queue await API sample
        /// 持续响应 API 客户端队列 await API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.EnumeratorQueueCommand or AutoCSer.Net.EnumeratorQueueCommand{T}</returns>
        AutoCSer.Net.EnumeratorQueueCommand<int> Callback(int left, int right);
    }
}
