﻿using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.ReadWriteQueue
{
    /// <summary>
    /// Server read/write queue one-time response API sample interface
    /// 服务端 读写队列 一次性响应 API 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// Synchronous API example, supporting ref/out parameters
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int Add(AutoCSer.Net.CommandServerCallReadQueue queue, int left, int right);
    }
    /// <summary>
    /// The server-side read and write queue one-time response API sample controller
    /// 服务端 读写队列 一次性响应 API 示例控制器
    /// </summary>
    internal sealed class SynchronousController: ISynchronousController
    {
        /// <summary>
        /// Synchronous API example, supporting ref/out parameters
        /// 同步 API 示例，支持 ref / out 参数
        /// </summary>
        /// <param name="queue">The current execution queue context must be defined before the first data parameter
        /// 当前执行队列上下文，必须定义在第一个数据参数之前</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        int ISynchronousController.Add(AutoCSer.Net.CommandServerCallReadQueue queue, int left, int right)
        {
            return left + right;
        }
    }
}
