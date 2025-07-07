using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// Server Task asynchronous queue controller unresponsive API sample interface
    /// 服务端 Task 异步队列控制器 无响应 API 示例接口
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
    /// Server Task asynchronous queue controller unresponsive API sample controller
    /// 服务端 Task 异步队列控制器 无响应 API 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : AutoCSer.Net.CommandServerTaskQueueService<int>, ISendOnlyController
    {
        /// <summary>
        /// Server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">The server asynchronously invokes the queue task
        /// 服务端异步调用队列任务</param>
        /// <param name="key">Queue keyword</param>
        public SendOnlyController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> ISendOnlyController.Call(int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {Key}.{value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
