﻿using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.TaskQueue
{
    /// <summary>
    /// Server Task asynchronous read/write queue unresponsive API example interface
    /// 服务端 Task 异步读写队列 无响应 API 示例接口
    /// </summary>
    public interface ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> Call(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int value);
    }
    /// <summary>
    /// Server Task asynchronous read/write queue unresponsive API example controller
    /// 服务端 Task 异步读写队列 无响应 API 示例控制器
    /// </summary>
    internal sealed class SendOnlyController: ISendOnlyController
    {
        /// <summary>
        /// Unresponsive API example
        /// 无响应 API 示例
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="value"></param>
        /// <returns>The return value type must be System.Threading.Tasks.Task{AutoCSer.Net.CommandServerSendOnly}</returns>
        System.Threading.Tasks.Task<AutoCSer.Net.CommandServerSendOnly> ISendOnlyController.Call(AutoCSer.Net.CommandServerCallTaskQueue<int> queue, int value)
        {
            Console.WriteLine($"{nameof(SendOnlyController)} {queue.Key}.{value}");
            return AutoCSer.Net.CommandServerSendOnly.NullTask;
        }
    }
}
