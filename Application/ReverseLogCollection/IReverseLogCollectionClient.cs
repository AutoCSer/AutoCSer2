using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 反向日志收集服务客户端
    /// </summary>
    /// <typeparam name="T">日志数据类型</typeparam>
    public interface IReverseLogCollectionClient<T>
    {
        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="callback">获取日志回调委托</param>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackType.Synchronous)]
        KeepCallbackCommand LogCallback(Action<CommandClientReturnValue<T>, KeepCallbackCommand> callback);
    }
}
