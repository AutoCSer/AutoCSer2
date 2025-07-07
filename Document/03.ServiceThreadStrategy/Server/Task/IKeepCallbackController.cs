using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// Server-side Task asynchronous continuous response API sample interface
    /// 服务端 Task 异步 持续响应 API 示例接口
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
    /// Server Task Asynchronous continuous response API sample controller
    /// 服务端 Task 异步 持续响应 API 示例控制器
    /// </summary>
    internal sealed class KeepCallbackController : IKeepCallbackController
    {
        /// <summary>
        /// Example of callback delegate
        /// 回调委托示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        internal static void Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            do
            {
                if (!callback.Callback(left)) break;
            }
            while (left++ != right);
        }
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
            Callback(left, right, callback);
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Callback count delegate example
        /// 回调计数委托示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal static async System.Threading.Tasks.Task CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            do
            {
                if (!await callback.CallbackAsync(left)) return;
            }
            while (left++ != right);
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
            return CallbackCount(left, right, callback);
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
        /// Example of asynchronous stream
        /// 异步流示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static async IAsyncEnumerable<int> AsyncEnumerable(int left, int right)
        {
            do
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                yield return left;
            }
            while (left++ != right);
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
            return AsyncEnumerable(left, right);
        }
    }
}
