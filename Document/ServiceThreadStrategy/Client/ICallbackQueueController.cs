﻿using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 服务端一次性响应 API 客户端示例接口
    /// </summary>
    public interface ICallbackQueueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">最后一个参数必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue} 或者 AutoCSer.Net.CommandClientCallbackQueueNode{T} 或者 AutoCSer.Net.CommandClientCallbackQueueNode</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.CallbackCommand</returns>
        AutoCSer.Net.CallbackCommand Add(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue> callback);
    }
}
