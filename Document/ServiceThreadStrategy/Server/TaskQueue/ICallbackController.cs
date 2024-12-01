using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步读写队列 API 一次性响应 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// 一次性响应 回调委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// 服务端 Task 异步读写队列 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class CallbackController : ICallbackController
    {
        /// <summary>
        /// 一次性响应 回调委托 API 示例
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task ICallbackController.Add(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            Console.WriteLine(left + right);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
