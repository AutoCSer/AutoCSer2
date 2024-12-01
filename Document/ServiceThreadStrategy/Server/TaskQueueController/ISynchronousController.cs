using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 一次性响应 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> Add(int left, int right);
    }
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class SynchronousController : AutoCSer.Net.CommandServerTaskQueueService<int>, ISynchronousController
    {
        /// <summary>
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">服务端异步调用队列任务</param>
        /// <param name="key">队列关键字</param>
        public SynchronousController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> ISynchronousController.Add(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }
    }
}
