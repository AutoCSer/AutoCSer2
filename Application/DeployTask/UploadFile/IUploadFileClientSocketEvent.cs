using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 文件上传客户端接口
    /// </summary>
    public interface IUploadFileClientSocketEvent
    {
        /// <summary>
        /// 文件上传客户端接口
        /// </summary>
        IUploadFileServiceClientController UploadFileClient { get; }
    }
}
