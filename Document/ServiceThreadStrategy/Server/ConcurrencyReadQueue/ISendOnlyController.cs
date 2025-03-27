using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.ConcurrencyReadQueue
{
    /// <summary>
    /// 服务端 支持并行读的同步队列 API 仅执行 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly Call(AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue, int value);
    }
    /// <summary>
    /// 服务端 支持并行读的同步队列 API 仅执行 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly ISendOnlyController.Call(AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue, int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.Null;
        }
    }
}
