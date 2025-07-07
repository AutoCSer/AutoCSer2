using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// Unresponsive API client sample interface
    /// 无响应 API 客户端示例接口
    /// </summary>
    public interface ISendOnlyCommandController
    {
        /// <summary>
        /// Example of an unresponsive API client API
        /// 无响应 API 客户端 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be AutoCSer.Net.SendOnlyCommand</returns>
        AutoCSer.Net.SendOnlyCommand Call(int value);
    }
}
