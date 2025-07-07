using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Synchronous
{
    /// <summary>
    /// Server IO thread synchronization one-time response callback delegate API sample interface
    /// 服务端 IO 线程同步 一次性响应 回调委托 API 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// Callback delegate API example, supporting ref/out parameters
        /// 回调委托 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The callback commission wrapper must be the last parameter
        /// 回调委托包装，必须是最后一个参数</param>
        void Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// Server IO thread synchronization one-time response callback delegate API sample controller
    /// 服务端 IO 线程同步 一次性响应 回调委托 API 示例控制器
    /// </summary>
    internal sealed class CallbackController: ICallbackController
    {
        /// <summary>
        /// Callback delegate API example, supporting ref/out parameters
        /// 回调委托 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">The callback commission wrapper must be the last parameter
        /// 回调委托包装，必须是最后一个参数</param>
        void ICallbackController.Add(int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            Console.WriteLine(left + right);
        }
    }
}
