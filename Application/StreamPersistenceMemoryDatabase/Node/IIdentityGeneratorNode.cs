using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 64-bit auto-increment identity node interface
    /// 64 位自增ID 节点接口
    /// </summary>
    [ServerNode(IsLocalClient = true)]
    public partial interface IIdentityGeneratorNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(long identity);
        /// <summary>
        /// Get the next increment identity
        /// 获取下一个自增ID
        /// </summary>
        /// <returns>The next increment identity returns a negative number on failure
        /// 下一个自增ID，失败返回负数</returns>
        long Next();
        /// <summary>
        /// Gets the auto-increment identity segment
        /// 获取自增 ID 分段
        /// </summary>
        /// <param name="count">Get the quantity of data
        /// 获取数据数量</param>
        /// <returns>Auto-increment identity segment
        /// 自增 ID 分段</returns>
        IdentityFragment NextFragment(int count);
    }
}
