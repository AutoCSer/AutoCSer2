using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 持续响应 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        System.Threading.Tasks.Task CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task{IEnumerable{T}}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        Task<IEnumerable<int>> Enumerable(int left, int right);

        /// <summary>
        /// 服务端异步流 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 IAsyncEnumerable{T}</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        IAsyncEnumerable<int> AsyncEnumerable(int left, int right);
    }
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 持续响应 示例控制器
    /// </summary>
    internal sealed class KeepCallbackController : AutoCSer.Net.CommandServerTaskQueueService<int>, IKeepCallbackController
    {
        /// <summary>
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">服务端异步调用队列任务</param>
        /// <param name="key">队列关键字</param>
        public KeepCallbackController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task IKeepCallbackController.Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task IKeepCallbackController.CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            return Task.KeepCallbackController.CallbackCount(left, right, callback);
        }
        /// <summary>
        /// 集合封装 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task{IEnumerable{T}}</returns>
        Task<IEnumerable<int>> IKeepCallbackController.Enumerable(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(Enumerable.Range(left, right - left + 1));
        }

        /// <summary>
        /// 服务端异步流 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 IAsyncEnumerable{T}</returns>
        IAsyncEnumerable<int> IKeepCallbackController.AsyncEnumerable(int left, int right)
        {
            return Task.KeepCallbackController.AsyncEnumerable(left, right);
        }
    }
}
