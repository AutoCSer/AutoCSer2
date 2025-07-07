using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// Server-side Task asynchronous read and write queue one-time response callback delegate API sample interface
    /// 服务端 Task 异步读写队列 一次性响应 回调委托 API 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be AutoCSer.Net.CommandServerCallback{T} or AutoCSer.Net.CommandServerCallback
        /// 最后一个参数类型必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// The server-side Task asynchronous read and write queue one-time response callback delegate API sample controller
    /// 服务端 Task 异步读写队列 一次性响应 回调委托 API 示例控制器
    /// </summary>
    internal sealed class CallbackController : ICallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The last parameter type must be AutoCSer.Net.CommandServerCallback{T} or AutoCSer.Net.CommandServerCallback
        /// 最后一个参数类型必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task ICallbackController.Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            Console.WriteLine(left + right);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
