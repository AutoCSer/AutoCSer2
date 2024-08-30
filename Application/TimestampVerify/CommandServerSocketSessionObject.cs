using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<TimestampVerifySession>
    {
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.TryGetSessionObject(CommandServerSocket socket)
        {
            return (TimestampVerifySession)socket.SessionObject;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session = new TimestampVerifySession();
            socket.SessionObject = session;
            return session;
        }
        /// <summary>
        /// 默认基于递增登录时间戳验证的服务认证会话对象操作
        /// </summary>
        internal static readonly CommandServerSocketSessionObject Default = new CommandServerSocketSessionObject();
    }
}
