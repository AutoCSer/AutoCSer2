using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.DatabaseBackupClient
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new CommandClientSocketEvent(client);
        }
    }
}
