using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// Server-side Task asynchronous queue controller one-time response API sample interface
    /// 服务端 Task 异步队列控制器 一次性响应 API 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// One-time response API example
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task or System.Threading.Tasks.Task{T}</returns>
        System.Threading.Tasks.Task<int> Add(int left, int right);
    }
    /// <summary>
    /// Server-side Task asynchronous queue controller one-time response API sample controller
    /// 服务端 Task 异步队列控制器 一次性响应 API 示例控制器
    /// </summary>
    internal sealed class SynchronousController : AutoCSer.Net.CommandServerTaskQueueService<int>, ISynchronousController
    {
        /// <summary>
        /// Server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">The server asynchronously invokes the queue task
        /// 服务端异步调用队列任务</param>
        /// <param name="key">Queue keyword</param>
        public SynchronousController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// One-time response API example
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task or System.Threading.Tasks.Task{T}</returns>
        System.Threading.Tasks.Task<int> ISynchronousController.Add(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }
    }
}
