using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传服务端配置
    /// </summary>
    public class UploadFileServiceConfig
    {
        /// <summary>
        /// 套接字自定义会话对象操作
        /// </summary>
        public ICommandServerSocketSessionObject<UploadFileService, UploadFileService> CommandServerSocketSessionObject;
        /// <summary>
        /// 备份文件目录
        /// </summary>
        public string BackupPath;
        /// <summary>
        /// 文件上传操作超时检查秒数，默认为 4，最小值为 1
        /// </summary>
        public int TimeoutSeconds = 4;
        /// <summary>
        /// 文件上传集合容器初始化大小，默认为最小值 1
        /// </summary>
        public int Capacity = 1;

        /// <summary>
        /// 创建文件上传服务
        /// </summary>
        /// <returns>文件上传服务</returns>
        public UploadFileService Create()
        {
            return new UploadFileService(this);
        }
    }
}
