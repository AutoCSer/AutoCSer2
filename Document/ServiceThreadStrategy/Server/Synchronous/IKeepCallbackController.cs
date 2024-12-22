using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// 服务端 IO 线程同步 API 持续响应 示例接口
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
        void Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        [AutoCSer.Net.CommandServerMethod(KeepCallbackOutputCount = 1 << 10)]
        void CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback);
    }
    /// <summary>
    /// 服务端 IO 线程同步 API 持续响应 示例控制器
    /// </summary>
    internal sealed class KeepCallbackController : IKeepCallbackController
    {
        /// <summary>
        /// 回调委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallback{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        void IKeepCallbackController.Callback(int left, int right, AutoCSer.Net.CommandServerKeepCallback<int> callback)
        {
            Task.KeepCallbackController.Callback(left, right, callback);
        }
        /// <summary>
        /// 回调计数委托 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，最后一个参数类型必须是 AutoCSer.Net.CommandServerKeepCallbackCount{T}</param>
        /// <returns>返回值类型必须为 Task</returns>
        void IKeepCallbackController.CallbackCount(int left, int right, AutoCSer.Net.CommandServerKeepCallbackCount<int> callback)
        {
            Task.KeepCallbackController.CallbackCount(left, right, callback).wait();
        }
    }
}
