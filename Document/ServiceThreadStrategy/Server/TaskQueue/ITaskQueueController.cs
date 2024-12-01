using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// 服务端控制器 Task 异步读写队列 API 示例接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 16)]
    public interface ITaskQueueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> Add(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task Callback(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task CallbackCount(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task{IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        Task<IEnumerable<int>> Enumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// 服务端异步流 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        IAsyncEnumerable<int> AsyncEnumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right);

        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        Task<AutoCSer.Net.CommandServerSendOnly> Call(AutoCSer.Net.CommandServerCallTaskQueue queue, int value);
    }
    /// <summary>
    /// 服务端控制器 Task 异步读写队列 API 示例控制器
    /// </summary>
    internal sealed class TaskQueueController: ITaskQueueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> ITaskQueueController.Add(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }

        /// <summary>
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task ITaskQueueController.Callback(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task ITaskQueueController.CallbackCount(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            return Task.KeepCallbackController.CallbackCount(left, right, callback);
        }
        /// <summary>
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task{IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        Task<IEnumerable<int>> ITaskQueueController.Enumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(Enumerable.Range(left, right - left + 1));
        }

        /// <summary>
        /// 服务端异步流 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        IAsyncEnumerable<int> ITaskQueueController.AsyncEnumerable(AutoCSer.Net.CommandServerCallTaskQueue queue, int left, int right)
        {
            return Task.KeepCallbackController.AsyncEnumerable(left, right);
        }

        /// <summary>
        /// 仅执行 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>返回值类型必须为 Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        Task<AutoCSer.Net.CommandServerSendOnly> ITaskQueueController.Call(AutoCSer.Net.CommandServerCallTaskQueue queue, int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
