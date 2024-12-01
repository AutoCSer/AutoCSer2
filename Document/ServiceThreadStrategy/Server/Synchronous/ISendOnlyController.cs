using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// 服务端 IO 线程同步 API 仅执行 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly Call(int value);
    }
    /// <summary>
    /// 服务端 IO 线程同步 API 仅执行 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly ISendOnlyController.Call(int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.Null;
        }
    }
}
