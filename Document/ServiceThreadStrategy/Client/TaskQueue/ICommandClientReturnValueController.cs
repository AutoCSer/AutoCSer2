﻿using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// 服务端 Task 异步队列控制器 服务端一次性响应 API 客户端示例接口
    /// </summary>
    public interface ICommandClientReturnValueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="queueKey">队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CommandClientReturnValue 或者 AutoCSer.Net.CommandClientReturnValue{T}</returns>
        AutoCSer.Net.CommandClientReturnValue<int> Add(int queueKey, int left, int right);
    }
}