using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTaskQueueContext
{
    /// <summary>
    /// 服务端 async Task 读写队列调用上下文 同步返回数据 示例接口
    /// </summary>
    public interface ISynchronousKeyController
    {
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        [CommandServerMethod(IsLowPriorityTaskQueue = true)]
        Task<int> SynchronousReturn(int parameter1, int parameter2);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task SynchronousCall(int parameter1, int parameter2);
    }
    /// <summary>
    /// 服务端 async Task 读写队列调用上下文 同步返回数据 示例接口实例
    /// </summary>
    internal sealed class SynchronousKeyController : CommandServerTaskQueueService<int>, ISynchronousKeyController
    {
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task">服务端异步调用队列任务</param>
        /// <param name="key">队列关键字</param>
        public SynchronousKeyController(CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> ISynchronousKeyController.SynchronousReturn(int parameter1, int parameter2)
        {
            return Task.FromResult(parameter1 + parameter2);
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        Task ISynchronousKeyController.SynchronousCall(int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
