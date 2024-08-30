using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 文件上传客户端接口
    /// </summary>
    public interface IUploadFileClientSocketEvent
    {
        /// <summary>
        /// 文件上传客户端接口
        /// </summary>
        IUploadFileClient UploadFileClient { get; }
    }
}
