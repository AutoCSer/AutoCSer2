using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// One-time response API client queue await callback API sample interface
    /// 一次性响应 API 客户端队列 await 回调 API 示例接口
    /// </summary>
    public interface IReturnQueueCommandController
    {
        /// <summary>
        /// One-time response API client queue await callback API sample
        /// 一次性响应 API 客户端队列 await 回调 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.ReturnCommand or AutoCSer.Net.ReturnCommand{T}</returns>
        AutoCSer.Net.ReturnQueueCommand<int> Add(int left, int right);
    }
}
