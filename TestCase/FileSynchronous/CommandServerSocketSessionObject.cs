using AutoCSer.CommandService;
using AutoCSer.CommandService.FileSynchronous;
using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.FileSynchronous
{
    /// <summary>
    /// 套接字自定义会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<UploadFileService, UploadFileService>, ICommandServerSocketSessionObject<TimestampVerifySession>
    {
        /// <summary>
        /// 获取或者创建套接字自定义会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private CommandServerSocketSessionObject getOrCreate(CommandServerSocket socket)
        {
            CommandServerSocketSessionObject sessionObject = (CommandServerSocketSessionObject)socket.SessionObject;
            if (sessionObject != null) return sessionObject;
            socket.SessionObject = sessionObject = new CommandServerSocketSessionObject();
            return sessionObject;
        }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        private TimestampVerifySession timestampVerifySession;
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.TryGetSessionObject(CommandServerSocket socket)
        {
            return timestampVerifySession;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session = new TimestampVerifySession();
            getOrCreate(socket).timestampVerifySession = session;
            return session;
        }
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private UploadFileService uploadFileService;
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        UploadFileService ICommandServerSocketSessionObject<UploadFileService, UploadFileService>.TryGetSessionObject(CommandServerSocket socket)
        {
            return uploadFileService;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        UploadFileService ICommandServerSocketSessionObject<UploadFileService, UploadFileService>.CreateSessionObject(UploadFileService service, CommandServerSocket socket)
        {
            getOrCreate(socket).uploadFileService = service;
            return service;
        }
    }
}
