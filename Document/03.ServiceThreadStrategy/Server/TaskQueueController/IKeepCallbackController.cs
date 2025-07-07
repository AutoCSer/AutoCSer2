using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// The server-side Task asynchronous queue controller continuous response API sample interface
    /// 服务端 Task 异步队列控制器 持续响应 API 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// Collection encapsulation API example
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<int>> Enumerable(int left, int right);

        /// <summary>
        /// Example of asynchronous stream API
        /// 异步流 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Collections.Generic.IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Collections.Generic.IAsyncEnumerable<int> AsyncEnumerable(int left, int right);
    }
    /// <summary>
    /// The server-side Task asynchronous queue controller continuous response API sample controller
    /// 服务端 Task 异步队列控制器 持续响应 API 示例控制器
    /// </summary>
    internal sealed class KeepCallbackController : AutoCSer.Net.CommandServerTaskQueueService<int>, IKeepCallbackController
    {
        /// <summary>
        /// Server-side Task asynchronous queue controller
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">The server asynchronously invokes the queue task
        /// 服务端异步调用队列任务</param>
        /// <param name="key">Queue keyword</param>
        public KeepCallbackController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task IKeepCallbackController.Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task IKeepCallbackController.CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            return Task.KeepCallbackController.CallbackCount(left, right, callback);
        }
        /// <summary>
        /// Collection encapsulation API example
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{T}}</returns>
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<int>> IKeepCallbackController.Enumerable(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(Enumerable.Range(left, right - left + 1));
        }

        /// <summary>
        /// Example of asynchronous stream API
        /// 异步流 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Collections.Generic.IAsyncEnumerable{T}</returns>
        System.Collections.Generic.IAsyncEnumerable<int> IKeepCallbackController.AsyncEnumerable(int left, int right)
        {
            return Task.KeepCallbackController.AsyncEnumerable(left, right);
        }
    }
}
