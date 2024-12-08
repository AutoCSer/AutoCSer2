using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueueController
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 一次性响应 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// 服务端 Task 异步队列控制器 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class CallbackController : AutoCSer.Net.CommandServerTaskQueueService<int>, ICallbackController
    {
        /// <summary>
        /// 服务端 Task 异步队列控制器
        /// </summary>
        /// <param name="task">服务端异步调用队列任务</param>
        /// <param name="key">队列关键字</param>
        public CallbackController(AutoCSer.Net.CommandServerCallTaskQueueNode task, int key) : base(task, key) { }
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        System.Threading.Tasks.Task ICallbackController.Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
