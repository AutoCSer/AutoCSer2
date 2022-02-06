using System;
using AutoCSer.Net;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    /// <typeparam name="T">主控制器接口类型</typeparam>
    internal sealed class CommandClientConfig<T> : AutoCSer.Net.CommandClientConfig
        where T : class
    {
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new CommandClientSocketEvent<T>(client);
        }
    }
}
