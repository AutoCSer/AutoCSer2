﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server.AsyncTaskQueue
{
    /// <summary>
    /// 服务端控制器 async Task 读写队列调用 同步返回数据 示例接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 16)]
    public interface ISynchronousController
    {
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> SynchronousReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2);
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task SynchronousCall(CommandServerCallTaskLowPriorityQueue queue, int parameter1, int parameter2);
    }
    /// <summary>
    /// 服务端控制器 async Task 读写队列调用 同步返回数据 示例接口实例
    /// </summary>
    internal sealed class SynchronousController : ISynchronousController
    {
        /// <summary>
        /// 同步返回数据
        /// </summary>
        /// <param name="socket">当前套接字连接上下文，必须是第一个参数，允许不定义该参数</param>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        /// <returns>必须是 async Task</returns>
        Task<int> ISynchronousController.SynchronousReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int parameter1, int parameter2)
        {
            return Task.FromResult(parameter1 + parameter2);
        }
        /// <summary>
        /// 无返回值同步调用
        /// </summary>
        /// <param name="queue">当前执行队列上下文，必须定义在所有数据参数之前</param>
        /// <param name="parameter1">参数</param>
        /// <param name="parameter2">参数</param>
        Task ISynchronousController.SynchronousCall(CommandServerCallTaskLowPriorityQueue queue, int parameter1, int parameter2)
        {
            Console.WriteLine(parameter1 + parameter2);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
