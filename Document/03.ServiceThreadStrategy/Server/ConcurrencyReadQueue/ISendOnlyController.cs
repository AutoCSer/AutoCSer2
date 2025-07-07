using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.ConcurrencyReadQueue
{
    /// <summary>
    /// The server side supports a synchronous queue for parallel reading and unresponsive API sample interface
    /// 服务端 支持并行读的同步队列 无响应 API 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly Call(AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue, int value);
    }
    /// <summary>
    /// The server side supports a synchronous queue for parallel reading and unresponsive API sample controller
    /// 服务端 支持并行读的同步队列 无响应 API 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be AutoCSer.Net.CommandServerSendOnly</returns>
        AutoCSer.Net.CommandServerSendOnly ISendOnlyController.Call(AutoCSer.Net.CommandServerCallConcurrencyReadQueue queue, int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.Null;
        }
    }
}
