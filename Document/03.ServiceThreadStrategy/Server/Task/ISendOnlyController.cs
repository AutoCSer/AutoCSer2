using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// Server Task asynchronous unresponsive API sample interface
    /// 服务端 Task 异步 无响应 API 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> Call(int value);
    }
    /// <summary>
    /// Server Task asynchronous unresponsive API sample controller
    /// 服务端 Task 异步 无响应 API 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> ISendOnlyController.Call(int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
