using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// Server-side Task asynchronous read and write queue one-time response API sample interface
    /// 服务端 Task 异步读写队列 一次性响应 API 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// One-time response API example
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task or System.Threading.Tasks.Task{T}</returns>
        System.Threading.Tasks.Task<int> Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right);
    }
    /// <summary>
    /// Server-side Task asynchronous read and write queue one-time response API sample controller
    /// 服务端 Task 异步读写队列 一次性响应 API 示例控制器
    /// </summary>
    internal sealed class SynchronousController : ISynchronousController
    {
        /// <summary>
        /// One-time response API example
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task or System.Threading.Tasks.Task{T}</returns>
        System.Threading.Tasks.Task<int> ISynchronousController.Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }
    }
}
