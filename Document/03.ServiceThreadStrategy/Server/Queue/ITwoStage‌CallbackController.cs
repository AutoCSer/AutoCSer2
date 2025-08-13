using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Queue
{
    /// <summary>
    /// The server-side synchronization queue thread two-stage response API sample interface
    /// 服务端 同步队列线程 二阶段响应 API 示例接口
    /// </summary>
    public interface ITwoStage‌CallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be void</returns>
        void Callback(AutoCSer.Net.CommandServerCallQueue queue, int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback);
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be void</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        void CallbackCount(AutoCSer.Net.CommandServerCallQueue queue, int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallbackCount<int> keepCallback);
    }
    /// <summary>
    /// The server-side synchronous queue thread two-stage response API sample controller
    /// 服务端 同步队列线程 二阶段响应 API 示例控制器
    /// </summary>
    internal sealed class TwoStage‌CallbackController : ITwoStage‌CallbackController
    {
        /// <summary>
        /// Callback delegate API example
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallback{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>The return value type must be void</returns>
        void ITwoStage‌CallbackController.Callback(AutoCSer.Net.CommandServerCallQueue queue, int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallback<int> keepCallback)
        {
            Task.TwoStage‌CallbackController.Callback(left, count, callback, keepCallback);
        }
        /// <summary>
        /// Callback count delegate API example
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback wrapper in the first stage, the type of the penultimate parameter must be AutoCSer.Net.CommandServerCallback{T}
        /// 第一阶段的回调委托包装，倒数第二个参数类型必须是 AutoCSer.Net.CommandServerCallback{T}</param>
        /// <param name="keepCallback">For the callback delegate wrapper of the second stage of continuous response, the last parameter type must be AutoCSer.Net.CommandServerKeepCallbackCount{T}
        /// 第二阶段持续响应的回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>The return value type must be void</returns>
        void ITwoStage‌CallbackController.CallbackCount(AutoCSer.Net.CommandServerCallQueue queue, int left, int count, AutoCSer.Net.CommandServerCallback<TwoStageCallbackParameter> callback, AutoCSer.Net.CommandServerKeepCallbackCount<int> keepCallback)
        {
            Task.TwoStage‌CallbackController.CallbackCount(left, count, callback, keepCallback).AutoCSerExtensions().Wait();
        }
    }
}
