using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// 服务端 Task 异步 API 一次性响应 回调委托 API 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// 一次性响应 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// 服务端 Task 异步 API 一次性响应 回调委托 API 示例控制器
    /// </summary>
    internal sealed class CallbackController : ICallbackController
    {
        /// <summary>
        /// 一次性响应 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 AutoCSer.Net.CommandServerCallback{T} 或者 AutoCSer.Net.CommandServerCallback</param>
        /// <returns>返回值类型必须为 Task</returns>
        System.Threading.Tasks.Task ICallbackController.Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            Console.WriteLine(left + right);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
