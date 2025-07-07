using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// Server-side Task asynchronous one-time response API sample interface
    /// 服务端 Task 异步 一次性响应 API 示例接口
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
    /// Server Task asynchronous one-time response API sample controller
    /// 服务端 Task 异步 一次性响应 API 示例控制器
    /// </summary>
    internal sealed class SynchronousController: ISynchronousController
    {
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
