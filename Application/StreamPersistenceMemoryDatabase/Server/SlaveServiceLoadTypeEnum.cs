using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库从节点服务端加载数据类型
    /// </summary>
    [Flags]
    internal enum SlaveServiceLoadTypeEnum
    {
        /// <summary>
        /// 修复节点方法信息
        /// </summary>
        RepairNodeMethod = 1,
        /// <summary>
        /// 持久化文件
        /// </summary>
        PersistenceFile = 2,
        /// <summary>
        /// 持久化回调异常位置文件
        /// </summary>
        PersistenceCallbackExceptionPositionFile = 4
    }
}
