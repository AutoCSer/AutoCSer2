using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化文件重建队列回调任务类型
    /// </summary>
    internal enum PersistenceRebuilderCallbackTypeEnum : byte
    {
        /// <summary>
        /// 关闭重建操作
        /// </summary>
        Close,
        /// <summary>
        /// 持久化下一个节点
        /// </summary>
        NextNode,
        /// <summary>
        /// 检查调用队列
        /// </summary>
        CheckQueue,
        /// <summary>
        /// 重建完成
        /// </summary>
        Completed
    }
}
