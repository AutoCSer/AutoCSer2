using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 64 位自增ID 节点
    /// </summary>
    public sealed class IdentityGeneratorNode : IIdentityGeneratorNode, ISnapshot<long>
    {
        /// <summary>
        /// 当前分配 ID
        /// </summary>
        private long identity;
        /// <summary>
        /// 位图节点
        /// </summary>
        /// <param name="identity">当前分配 ID</param>
        public IdentityGeneratorNode(long identity)
        {
            this.identity = identity > 0 ? identity : 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<long> GetSnapshotArray()
        {
            return new LeftArray<long>(new long[] { identity });
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="identity"></param>
        public void SnapshotSet(long identity)
        {
            this.identity = identity;
        }
        /// <summary>
        /// 获取下一个自增ID
        /// </summary>
        /// <returns>下一个自增ID，失败返回负数</returns>
        public long Next()
        {
            return identity > 0 ? identity++ : long.MinValue;
        }
        /// <summary>
        /// 获取自增 ID 分段
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <returns>自增 ID 分段</returns>
        public IdentityFragment NextFragment(int count)
        {
            if (count > 0 && identity + count > 0) return new IdentityFragment(ref identity, count);
            return default(IdentityFragment);
        }
    }
}
