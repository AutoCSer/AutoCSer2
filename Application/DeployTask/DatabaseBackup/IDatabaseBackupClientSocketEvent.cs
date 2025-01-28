using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 数据库备份客户端套接字事件
    /// </summary>
    public interface IDatabaseBackupClientSocketEvent
    {
        /// <summary>
        /// 数据库备份客户端接口
        /// </summary>
        IDatabaseBackupServiceClientController DatabaseBackupClient { get; }
    }
}
