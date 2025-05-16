using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令客户端套接字事件缓存
    /// </summary>
    /// <typeparam name="T">命令客户端套接字事件类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientSocketEventCache<T>
        where T : CommandClientSocketEvent
    {
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public readonly T SocketEvent;
        /// <summary>
        /// 命令客户端
        /// </summary>
        public readonly ICommandClient Client;
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEventCache(ICommandClient client)
        {
            Client = client;
            SocketEvent = (T)client.SocketEvent;
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="config">命令客户端配置</param>
        public CommandClientSocketEventCache(CommandClientConfig config) : this(new CommandClient(config)) { }
        /// <summary>
        /// 获取命令客户端套接字事件缓存
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CommandClientSocketEventCache<T> Create(CommandReverseListenerConfig config)
        {
            CommandReverseListener listener = new CommandReverseListener(config);
            listener.Start().NotWait();
            return new CommandClientSocketEventCache<T>(listener.CommandClient);
        }
    }
}
