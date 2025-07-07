using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// One-time response API client synchronization API sample interface
    /// 一次性响应 API 客户端同步 API 示例接口
    /// </summary>
    public interface ICommandClientReturnValueController
    {
        /// <summary>
        /// One-time response API client synchronization API sample
        /// 一次性响应 API 客户端同步 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandClientReturnValue or AutoCSer.Net.CommandClientReturnValue{T}</returns>
        AutoCSer.Net.CommandClientReturnValue<int> Add(int left, int right);
    }
}
