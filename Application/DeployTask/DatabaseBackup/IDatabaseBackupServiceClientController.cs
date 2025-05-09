using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 数据库备份服务接口 客户端接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IDatabaseBackupService))]
    public partial interface IDatabaseBackupServiceClientController
    {
    }
}
