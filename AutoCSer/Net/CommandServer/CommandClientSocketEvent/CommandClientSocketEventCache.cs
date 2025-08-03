using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client socket event caching
    /// 命令客户端套接字事件缓存
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CommandClientSocketEventCache<T>
        where T : CommandClientSocketEvent
    {
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        public readonly T SocketEvent;
        /// <summary>
        /// Command client
        /// </summary>
        public readonly CommandClient Client;
        /// <summary>
        /// Command client socket event caching
        /// 命令客户端套接字事件缓存
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEventCache(CommandClient client)
        {
            Client = client;
            SocketEvent = (T)client.SocketEvent;
        }
        /// <summary>
        /// Command client socket event caching
        /// 命令客户端套接字事件缓存
        /// </summary>
        /// <param name="config">Command the client configuration
        /// 命令客户端配置</param>
        public CommandClientSocketEventCache(CommandClientConfig config) : this(new CommandClient(config)) { }
        /// <summary>
        /// Gets the command client socket event cache
        /// 获取命令客户端套接字事件缓存
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CommandClientSocketEventCache<T> Create(CommandReverseListenerConfig config)
        {
            CommandReverseListener listener = new CommandReverseListener(config);
            listener.Start().AutoCSerNotWait();
            return new CommandClientSocketEventCache<T>(listener.CommandClient);
        }
    }
}
