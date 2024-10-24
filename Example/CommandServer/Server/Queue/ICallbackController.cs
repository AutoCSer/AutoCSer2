﻿using AutoCSer.Net;
using System;

namespace AutoCSer.Example.CommandServer.Server.Queue
{
    /// <summary>
    /// 服务端 同步队列线程调用 回调委托返回数据 示例接口
    /// </summary>
    public interface ICallbackController
    {
        /// <summary>
        /// 回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void CallbackReturn(CommandServerSocket socket, CommandServerCallQueue queue, int parameter1, int parameter2, CommandServerCallback<int> callback);
        /// <summary>
        /// 回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void CallbackCall(CommandServerCallLowPriorityQueue queue, int parameter1, int parameter2, CommandServerCallback callback);
    }
    /// <summary>
    /// 服务端 同步队列线程调用 回调委托返回数据 示例接口实例
    /// </summary>
    internal sealed class CallbackController : ICallbackController
    {
        /// <summary>
        /// 回调委托返回数据，返回值类型必须为 void
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void ICallbackController.CallbackReturn(CommandServerSocket socket, CommandServerCallQueue queue, int parameter1, int parameter2, CommandServerCallback<int> callback)
        {
            callback.Callback(parameter1 + parameter2);
        }
        /// <summary>
        /// 回调委托无返回值，返回值类型必须为 void
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <param name="callback">回调委托包装，必须是最后一个参数</param>
        void ICallbackController.CallbackCall(CommandServerCallLowPriorityQueue queue, int parameter1, int parameter2, CommandServerCallback callback)
        {
            Console.WriteLine(parameter1 + parameter2);
            callback.Callback();
        }
    }
}
