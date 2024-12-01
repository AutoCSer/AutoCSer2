using System;

namespace AutoCSer.CommandService.FileSynchronous
{
    /// <summary>
    /// 拉取文件客户端接口
    /// </summary>
    public interface IPullFileClientSocketEvent
    {
        /// <summary>
        /// 拉取文件客户端接口
        /// </summary>
        IPullFileServiceClientController PullFileClient { get; }
    }
}
