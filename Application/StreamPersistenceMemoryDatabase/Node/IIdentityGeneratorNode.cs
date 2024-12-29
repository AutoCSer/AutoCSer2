using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 64 位自增ID 节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IIdentityGeneratorNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="identity"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(long identity);
        /// <summary>
        /// 获取下一个自增ID
        /// </summary>
        /// <returns>下一个自增ID，失败返回负数</returns>
        long Next();
        /// <summary>
        /// 获取自增 ID 分段
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <returns>自增 ID 分段</returns>
        IdentityFragment NextFragment(int count);
    }
}
