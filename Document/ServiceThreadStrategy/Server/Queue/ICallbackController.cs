using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Queue
{
    /// <summary>
    /// 服务端 同步队列线程 API 一次性响应 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// 回调委托 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void Add(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback);
    }
    /// <summary>
    /// 服务端 同步队列线程 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class CallbackController : ICallbackController
    {
        /// <summary>
        /// 回调委托 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void ICallbackController.Add(AutoCSer.Net.CommandServerCallQueue queue, int left, int right, AutoCSer.Net.CommandServerCallback<int> callback)
        {
            callback.Callback(left + right);
            Console.WriteLine(left + right);
        }
    }
}
