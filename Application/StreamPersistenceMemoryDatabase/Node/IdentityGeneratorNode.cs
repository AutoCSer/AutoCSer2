using System;
using System.Collections.Generic;

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
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<long> GetSnapshotResult(long[] snapshotArray, object customObject)
        {
            snapshotArray[0] = identity;
            return new SnapshotResult<long>(1);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<long> array, ref LeftArray<long> newArray) { }
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
