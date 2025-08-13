using System;

namespace AutoCSer.Document.ServiceThreadStrategy.Client.TaskQueue
{
    /// <summary>
    /// Two-stage response API client interface example of the server-side Task asynchronous queue controller
    /// 服务端 Task 异步队列控制器 二阶段响应 API 客户端接口示例
    /// </summary>
    public interface ITwoStageCallbackController
    {
        /// <summary>
        /// Two-stage response API client callback API sample
        /// 二阶段响应 API 客户端回调 API 示例
        /// </summary>
        /// <param name="queueKey">The queue keyword parameter must be the first parameter and its name must be queueKey
        /// 队列关键字参数必须为第一个参数，名称必须为 queueKey</param>
        /// <param name="left"></param>
        /// <param name="count"></param>
        /// <param name="callback">For the callback delegation in the first stage, the type of the penultimate parameter must be Action{AutoCSer.Net.CommandClientReturnValue{T}}
        /// 第一阶段的回调委托，倒数第二个参数类型必须是 Action{AutoCSer.Net.CommandClientReturnValue{T}}</param>
        /// <param name="keepCallback">For the callback delegation with continuous response in the second stage, the last parameter type must be Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand}
        /// 第二阶段持续响应的回调委托，最后一个参数类型必须为 Action{AutoCSer.Net.CommandClientReturnValue{T}, AutoCSer.Net.KeepCallbackCommand}</param>
        /// <returns>The return value type must be AutoCSer.Net.KeepCallbackCommand</returns>
        AutoCSer.Net.KeepCallbackCommand Callback(int queueKey, int left, int count, Action<AutoCSer.Net.CommandClientReturnValue<TwoStageCallbackParameter>> callback, Action<AutoCSer.Net.CommandClientReturnValue<int>, AutoCSer.Net.KeepCallbackCommand> keepCallback);
    }
}
