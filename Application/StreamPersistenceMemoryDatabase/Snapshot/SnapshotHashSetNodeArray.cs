using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照哈希表节点数组
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    internal sealed class SnapshotHashSetNodeArray<T> : ISnapshotEnumerable<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 快照哈希表
        /// </summary>
        private readonly SnapshotHashSet<T> hashSet;
        /// <summary>
        /// 快照哈希表节点集合
        /// </summary>
        internal readonly ReusableHashNode<T>[] Nodes;
        /// <summary>
        /// 快照数据集合
        /// </summary>
        private volatile SnapshotArrayNode<T>[] snapshotNodes;
        /// <summary>
        /// 超预申请快照数据
        /// </summary>
        private volatile SnapshotArrayNode<T>[] newSnapshotNodes;
        /// <summary>
        /// 快照数据数量
        /// </summary>
        private volatile int snapshotCount;
        /// <summary>
        /// 快照哈希表节点数组
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="capacity"></param>
        internal SnapshotHashSetNodeArray(SnapshotHashSet<T> dictionary, int capacity)
        {
            this.hashSet = dictionary;
            Nodes = new ReusableHashNode<T>[capacity];
            newSnapshotNodes = snapshotNodes = EmptyArray<SnapshotArrayNode<T>>.Array;
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        public IEnumerable<T> SnapshotValues
        {
            get
            {
                int index = 0;
                for (int snapshotCount = Math.Min(this.snapshotCount, snapshotNodes.Length); index != snapshotCount; ++index)
                {
                    T value = Nodes[index].Value;
                    SnapshotArrayNode<T> snapshotValue = snapshotNodes[index];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.Value : value;
                }
                for (int newIndex = 0; index != this.snapshotCount; ++index, ++newIndex)
                {
                    T value = Nodes[index].Value;
                    SnapshotArrayNode<T> snapshotValue = newSnapshotNodes[newIndex];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.Value : value;
                }
            }
        }
        /// <summary>
        /// Get the array of pre-applied snapshot containers
        /// 获取预申请快照容器数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetSnapshotValueArray()
        {
            int count = hashSet.Count;
            if (count != 0) snapshotNodes = new SnapshotArrayNode<T>[count];
        }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetSnapshotResult()
        {
            int count = hashSet.Count, newSize = count - snapshotNodes.Length;
            if (newSize > 0) newSnapshotNodes = new SnapshotArrayNode<T>[newSize];
            snapshotCount = count;
        }
        /// <summary>
        /// Close the snapshot operation
        /// 关闭快照操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CloseSnapshot()
        {
            snapshotCount = 0;
            newSnapshotNodes = snapshotNodes = EmptyArray<SnapshotArrayNode<T>>.Array;
        }
        ///// <summary>
        ///// 设置数据
        ///// </summary>
        ///// <param name="index"></param>
        ///// <param name="value"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SetValue(int index, T value)
        //{
        //    TrySetSnapshotValue(index);
        //    Nodes[index].Value = value;
        //}
        /// <summary>
        /// 设置快照数据
        /// </summary>
        /// <param name="index"></param>
        internal void TrySetSnapshotValue(int index)
        {
            if (index < snapshotCount)
            {
                SnapshotArrayNode<T>[] snapshotNodes = this.snapshotNodes, newSnapshotNodes = this.newSnapshotNodes;
                if (index < snapshotCount)
                {
                    int newIndex = index - snapshotNodes.Length;
                    if (newIndex < 0) snapshotNodes[index].TrySet(Nodes[index].Value);
                    else newSnapshotNodes[newIndex].TrySet(Nodes[index].Value);
                }
            }
        }
        /// <summary>
        /// 清理数组
        /// </summary>
        /// <returns></returns>
        internal bool ClearArray()
        {
            if (snapshotCount == 0)
            {
                Array.Clear(Nodes, 0, hashSet.NodeArrayClearCount);
                return true;
            }
            return false;
        }
    }
}
