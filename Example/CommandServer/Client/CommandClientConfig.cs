using System;
using AutoCSer.Net;

namespace AutoCSer.Example.CommandServer.Client
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// 获取命令客户端套接字事件，默认为 new CommandClientSocketEvent(commandClient)，客户端初始化时一次性调用
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient commandClient)
        {
            return new CommandClientSocketEvent(commandClient);
        }
    }
}
