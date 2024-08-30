using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库服务端会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>
    {
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        CommandServerSocketSessionObjectService ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>.TryGetSessionObject(CommandServerSocket socket)
        {
            return (CommandServerSocketSessionObjectService)socket.SessionObject;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        CommandServerSocketSessionObjectService ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>.CreateSessionObject(CommandServerSocketSessionObjectService cacheService, CommandServerSocket socket)
        {
            socket.SessionObject = cacheService;
            return cacheService;
        }
        /// <summary>
        /// 默认日志流持久化内存数据库服务端会话对象操作
        /// </summary>
        internal static readonly CommandServerSocketSessionObject Default = new CommandServerSocketSessionObject();
    }
}
