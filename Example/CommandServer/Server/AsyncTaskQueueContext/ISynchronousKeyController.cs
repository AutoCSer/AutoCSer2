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
    internal sealed class SynchronousKeyController : CommandServerTaskQueue<int>, ISynchronousKeyController
    {
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public SynchronousKeyController(CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        async Task<int> ISynchronousKeyController.SynchronousReturn(int parameter1, int parameter2)
        {
            await Task.Yield();
            return parameter1 + parameter2;
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        async Task ISynchronousKeyController.SynchronousCall(int parameter1, int parameter2)
        {
            await Task.Yield();
            Console.WriteLine(parameter1 + parameter2);
        }
    }
}
