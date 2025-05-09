using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化文件重建队列回调任务类型
    /// </summary>
    internal enum PersistenceRebuilderCallbackTypeEnum : byte
    {
        /// <summary>
        /// 持久化下一个节点
        /// </summary>
        NextNode,
        /// <summary>
        /// 获取节点快照数据
        /// </summary>
        GetSnapshotResult,
        /// <summary>
        /// 检查调用队列
        /// </summary>
        CheckQueue,
        /// <summary>
        /// 关闭重建操作
        /// </summary>
        Close,
        /// <summary>
        /// 关闭重建操作（输出失败日志）
        /// </summary>
        CloseLog,
        /// <summary>
        /// 关闭版本重建
        /// </summary>
        CloseVersion,
#if !AOT
        /// <summary>
        /// 重建完成
        /// </summary>
        Completed
#endif
    }
}
