using AutoCSer.Memory;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 快照哈希表快照集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class FragmentSnapshotHashSetEnumerable256<T> : ISnapshotEnumerable<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 256 基分片 快照哈希表
        /// </summary>
        private readonly FragmentSnapshotHashSet256<T> hashSet;
        /// <summary>
        /// 快照哈希表节点数组集合
        /// </summary>
        private LeftArray<SnapshotHashSetNodeArray<T>> snapshotArray;
        /// <summary>
        /// 256 基分片 快照哈希表快照集合
        /// </summary>
        /// <param name="hashSet">256 基分片 快照哈希表</param>
        internal FragmentSnapshotHashSetEnumerable256(FragmentSnapshotHashSet256<T> hashSet)
        {
            this.hashSet = hashSet;
            snapshotArray.SetEmpty();
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<T> ISnapshotEnumerable<T>.SnapshotValues
        {
            get
            {
                int count = snapshotArray.Length;
                if (count != 0)
                {
                    foreach (SnapshotHashSetNodeArray<T> node in snapshotArray.Array)
                    {
                        foreach (T value in node.SnapshotValues) yield return value;
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// Get the array of pre-applied snapshot containers
        /// 获取预申请快照容器数组
        /// </summary>
        public void GetSnapshotValueArray()
        {
            int count = 0;
            foreach (var hashSet in this.hashSet.HashSets)
            {
                if (hashSet != null && hashSet.Count != 0)
                {
                    hashSet.Nodes.GetSnapshotValueArray();
                    ++count;
                }
            }
            snapshotArray.PrepLength(count);
        }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        public void GetSnapshotResult()
        {
            foreach (var hashSet in this.hashSet.HashSets)
            {
                if (hashSet != null)
                {
                    if (hashSet.Count != 0)
                    {
                        hashSet.Nodes.GetSnapshotResult();
                        snapshotArray.Add(hashSet.Nodes);
                    }
                    else hashSet.Nodes.CloseSnapshot();
                }
            }
        }
        /// <summary>
        /// Close the snapshot operation
        /// 关闭快照操作
        /// </summary>
        public void CloseSnapshot()
        {
            int count = snapshotArray.Length;
            if (count != 0)
            {
                foreach (SnapshotHashSetNodeArray<T> node in snapshotArray.Array)
                {
                    node.CloseSnapshot();
                    if (--count == 0) break;
                }
                snapshotArray.SetEmpty();
            }
        }
    }
}
