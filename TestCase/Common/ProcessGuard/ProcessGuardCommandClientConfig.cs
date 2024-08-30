using AutoCSer.Net;
using System;
using System.Reflection;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class ProcessGuardCommandClientConfig : AutoCSer.CommandService.ProcessGuardCommandClientConfig
    {
        /// <summary>
        /// 命令客户端配置
        /// </summary>
        internal ProcessGuardCommandClientConfig()
        {
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        }
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new ProcessGuardClientSocketEvent(client);
        }
    }
}
