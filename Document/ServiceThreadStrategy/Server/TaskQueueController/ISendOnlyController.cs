using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 仅执行 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        Task<AutoCSer.Net.CommandServerSendOnly> Call(int value);
    }
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 仅执行 示例控制器
    /// </summary>
    internal sealed class SendOnlyController : AutoCSer.Net.CommandServerTaskQueueService<int>, ISendOnlyController
    {
        /// <summary>
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">服务端异步调用队列任务</param>
        /// <param name="key">队列关键字</param>
        public SendOnlyController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        Task<AutoCSer.Net.CommandServerSendOnly> ISendOnlyController.Call(int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {Key}.{value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
