﻿using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Server.Task
{
    /// <summary>
    /// 服务端 Task 异步 API 一次性响应 示例接口
    /// </summary>
    public interface ISynchronousController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> Add(int left, int right);
    }
    /// <summary>
    /// 服务端 Task 异步 API 一次性响应 示例控制器
    /// </summary>
    internal sealed class SynchronousController: ISynchronousController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 Task 或者 Task{T}</returns>
        Task<int> ISynchronousController.Add(int left, int right)
        {
            return System.Threading.Tasks.Task.FromResult(left + right);
        }
    }
}