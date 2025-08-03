using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Queue
{
    /// <summary>
    /// The server-side synchronization queue thread continuous response API sample interface
    /// 服务端 同步队列线程 持续响应 API 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
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
        /// <returns>The return value type must be void</returns>
        void Callback(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
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
        /// <returns>The return value type must be void</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        void CallbackCount(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
    }
    /// <summary>
    /// The server-side synchronous queue thread continuous response API sample controller
    /// 服务端 同步队列线程 持续响应 API 示例控制器
    /// </summary>
    internal sealed class KeepCallbackController: IKeepCallbackController
    {
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
        /// <returns>The return value type must be void</returns>
        void IKeepCallbackController.Callback(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
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
        /// <returns>The return value type must be void</returns>
        void IKeepCallbackController.CallbackCount(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            Task.KeepCallbackController.CallbackCount(left, right, callback).AutoCSerExtensions().Wait();
        }
    }
}
