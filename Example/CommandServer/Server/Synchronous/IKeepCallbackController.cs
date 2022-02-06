using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Server.Synchronous
{
    /// <summary>
    /// 服务端 IO线程同步调用 保持回调委托返回数据 示例接口
    /// </summary>
    public interface IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        void CallbackReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallback<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        void CallbackCall(int parameter, CommandServerKeepCallback callback);

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        void CallbackCountReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback);
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        void CallbackCountCall(int parameter, CommandServerKeepCallbackCount callback);
    }
    /// <summary>
    /// 服务端 IO线程同步调用 保持回调委托返回数据 示例接口实例
    /// </summary>
    internal sealed class KeepCallbackController : IKeepCallbackController
    {
        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        void IKeepCallbackController.CallbackReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallback<int> callback)
        {
            for (int value = parameter1 + parameter2, endValue = value + 4; value != endValue; callback.Callback(value++)) ;
            callback.CancelKeep();
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">保持回调委托包装，必须是最后一个参数</param>
        void IKeepCallbackController.CallbackCall(int parameter, CommandServerKeepCallback callback)
        {
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) callback.Callback();
            callback.CancelKeep();
        }

        /// <summary>
        /// 保持回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        void IKeepCallbackController.CallbackCountReturn(CommandServerSocket socket, int parameter1, int parameter2, CommandServerKeepCallbackCount<int> callback)
        {
            for (int value = parameter1 + parameter2, endValue = value + 4; value != endValue; callback.CallbackAsync(value++).Wait()) ;
            callback.CancelKeep();
        }
        /// <summary>
        /// 保持回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <param name="callback">等待计数模式 保持回调委托包装，必须是最后一个参数</param>
        void IKeepCallbackController.CallbackCountCall(int parameter, CommandServerKeepCallbackCount callback)
        {
            Console.WriteLine(parameter);
            for (int value = 4; value != 0; --value) callback.CallbackAsync().Wait();
            callback.CancelKeep();
        }
    }
}
