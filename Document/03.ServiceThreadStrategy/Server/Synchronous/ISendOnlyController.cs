using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// Server IO thread synchronization unresponsive API sample interface
    /// 服务端 IO 线程同步 无响应 API 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly Call(int value);
    }
    /// <summary>
    /// Server IO thread synchronization unresponsive API example controller
    /// 服务端 IO 线程同步 无响应 API 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly ISendOnlyController.Call(int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.Null;
        }
    }
}
