using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传服务端会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<UploadFileService, UploadFileService>
    {
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        UploadFileService ICommandServerSocketSessionObject<UploadFileService, UploadFileService>.TryGetSessionObject(CommandServerSocket socket)
        {
            return (UploadFileService)socket.SessionObject;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="cacheService"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        UploadFileService ICommandServerSocketSessionObject<UploadFileService, UploadFileService>.CreateSessionObject(UploadFileService cacheService, CommandServerSocket socket)
        {
            socket.SessionObject = cacheService;
            return cacheService;
        }
        /// <summary>
        /// 默认文件上传服务端会话对象操作
        /// </summary>
        internal static readonly CommandServerSocketSessionObject Default = new CommandServerSocketSessionObject();
    }
}
