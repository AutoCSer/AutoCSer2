using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务配置
    /// </summary>
    public class DeployTaskConfig
    {
        /// <summary>
        /// 上传文件缓冲区字节大小，默认为 128KB
        /// </summary>
        public int UploadFileBufferSize = 128 << 10;
        /// <summary>
        /// 上传文件缓冲区数量，默认为 128
        /// </summary>
        public int UploadFileBufferCount = 128;
    }
}
