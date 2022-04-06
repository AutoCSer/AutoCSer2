using AutoCSer.Net;
using System;
using System.Reflection;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// 反向日志收集客户端
        /// </summary>
        private readonly ReverseLogCollectionClient client;
        /// <summary>
        /// 命令客户端配置
        /// </summary>
        /// <param name="client"></param>
        /// <param name="hostEndPoint"></param>
        public CommandClientConfig(ReverseLogCollectionClient client, ref HostEndPoint hostEndPoint)
        {
            this.client = client;
            Host = hostEndPoint;
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        }
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(AutoCSer.Net.CommandClient client)
        {
            return new CommandClientSocketEvent(client, this.client);
        }
    }
}
