using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 持续响应 API 客户端示例接口
    /// </summary>
    public interface IKeepCallbackQueueController
    {
        /// <summary>
        /// 一次性响应 API 示例
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback">Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 Action{AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue{T} 或者 AutoCSer.Net.CommandClientKeepCallbackQueue</param>
        /// <returns>返回值类型必须为 AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int left, int right, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand> callback);
    }
}
