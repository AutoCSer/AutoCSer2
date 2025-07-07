using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// Server controller Task asynchronous read/write queue API sample interface
    /// 服务端控制器 Task 异步读写队列 API 示例接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 16)]
    public partial interface ITaskQueueController
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
        System.Threading.Tasks.Task<int> Add(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task Callback(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task CallbackCount(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// Collection encapsulation API example
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<int>> Enumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// Example of asynchronous stream API
        /// 异步流 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Collections.Generic.IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Collections.Generic.IAsyncEnumerable<int> AsyncEnumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> Call(AutoCSer.Net.CommandServerCallTaskQueue queue, int value);
    }
    /// <summary>
    /// Server controller Task asynchronous read/write queue API example controller
    /// 服务端控制器 Task 异步读写队列 API 示例控制器
    /// </summary>
    internal sealed class TaskQueueController: ITaskQueueController
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
        System.Threading.Tasks.Task<int> ITaskQueueController.Add(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }

        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        System.Threading.Tasks.Task ITaskQueueController.Callback(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">For the callback delegate wrapper, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be System.Threading.Tasks.Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task ITaskQueueController.CallbackCount(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            return Task.KeepCallbackController.CallbackCount(left, right, callback);
        }
        /// <summary>
        /// Collection encapsulation API example
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<int>> ITaskQueueController.Enumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(Enumerable.Range(left, right - left + 1));
        }

        /// <summary>
        /// Example of asynchronous stream API
        /// 异步流 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>The return value type must be System.Collections.Generic.IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Collections.Generic.IAsyncEnumerable<int> ITaskQueueController.AsyncEnumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return Task.KeepCallbackController.AsyncEnumerable(left, right);
        }

        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> ITaskQueueController.Call(AutoCSer.Net.CommandServerCallTaskQueue queue, int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
