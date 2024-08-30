using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 节点快照功能接口
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public interface ISnapshot<T>
    {
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        LeftArray<T> GetSnapshotArray();
    }
}
