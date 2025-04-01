using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照字典节点数组
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    internal sealed class SnapshotDictionaryNodeArray<KT, VT> : ISnapshotEnumerable<KeyValue<KT, VT>>, ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>>, ISnapshotEnumerable<VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 快照字典
        /// </summary>
        private readonly SnapshotDictionary<KT, VT> dictionary;
        /// <summary>
        /// 快照字典节点集合
        /// </summary>
        internal readonly ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[] Nodes;
        /// <summary>
        /// 快照数据集合
        /// </summary>
        private volatile SnapshotDictionarySnapshotNode<KT, VT>[] snapshotNodes;
        /// <summary>
        /// 超预申请快照数据
        /// </summary>
        private volatile SnapshotDictionarySnapshotNode<KT, VT>[] newSnapshotNodes;
        /// <summary>
        /// 快照数据数量
        /// </summary>
        private volatile int snapshotCount;
        /// <summary>
        /// 快照字典节点数组
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="capacity"></param>
        internal SnapshotDictionaryNodeArray(SnapshotDictionary<KT, VT> dictionary, int capacity)
        {
            this.dictionary = dictionary;
            Nodes = new ReusableHashNode<BinarySerializeKeyValue<KT, VT>>[capacity];
            newSnapshotNodes = snapshotNodes = EmptyArray<SnapshotDictionarySnapshotNode<KT, VT>>.Array;
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<KeyValue<KT, VT>> ISnapshotEnumerable<KeyValue<KT, VT>>.SnapshotValues
        {
            get
            {
                int index = 0;
                for (int snapshotCount = Math.Min(this.snapshotCount, snapshotNodes.Length); index != snapshotCount; ++index)
                {
                    BinarySerializeKeyValue<KT, VT> keyValue = Nodes[index].Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = snapshotNodes[index];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.KeyValue : keyValue.GetKeyValue();
                }
                for (int newIndex = 0; index != this.snapshotCount; ++index, ++newIndex)
                {
                    BinarySerializeKeyValue<KT, VT> keyValue = Nodes[index].Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = newSnapshotNodes[newIndex];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.KeyValue : keyValue.GetKeyValue();
                }
            }
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<BinarySerializeKeyValue<KT, VT>> ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>>.SnapshotValues
        {
            get
            {
                int index = 0;
                for (int snapshotCount = Math.Min(this.snapshotCount, snapshotNodes.Length); index != snapshotCount; ++index)
                {
                    BinarySerializeKeyValue<KT, VT> keyValue = Nodes[index].Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = snapshotNodes[index];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.BinarySerializeKeyValue : keyValue;
                }
                for (int newIndex = 0; index != this.snapshotCount; ++index, ++newIndex)
                {
                    BinarySerializeKeyValue<KT, VT> keyValue = Nodes[index].Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = newSnapshotNodes[newIndex];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.BinarySerializeKeyValue : keyValue;
                }
            }
        }
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<VT> ISnapshotEnumerable<VT>.SnapshotValues
        {
            get
            {
                int index = 0;
                for (int snapshotCount = Math.Min(this.snapshotCount, snapshotNodes.Length); index != snapshotCount; ++index)
                {
                    VT value = Nodes[index].Value.Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = snapshotNodes[index];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.Value : value;
                }
                for (int newIndex = 0; index != this.snapshotCount; ++index, ++newIndex)
                {
                    VT value = Nodes[index].Value.Value;
                    SnapshotDictionarySnapshotNode<KT, VT> snapshotValue = newSnapshotNodes[newIndex];
                    yield return snapshotValue.IsSnapshot ? snapshotValue.Value : value;
                }
            }
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        public void GetSnapshotValueArray()
        {
            int count = dictionary.Count;
            if (count != 0) snapshotNodes = new SnapshotDictionarySnapshotNode<KT, VT>[count];
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        public void GetSnapshotResult()
        {
            int count = dictionary.Count, newSize = count - snapshotNodes.Length;
            if (newSize > 0) newSnapshotNodes = new SnapshotDictionarySnapshotNode<KT, VT>[newSize];
            snapshotCount = count;
        }
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        public void CloseSnapshot()
        {
            snapshotCount = 0;
            newSnapshotNodes = snapshotNodes = EmptyArray<SnapshotDictionarySnapshotNode<KT, VT>>.Array;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        internal void SetValue(int index, VT value)
        {
            if (index < snapshotCount)
            {
                SnapshotDictionarySnapshotNode<KT, VT>[] snapshotNodes = this.snapshotNodes, newSnapshotNodes = this.newSnapshotNodes;
                if (index < snapshotCount)
                {
                    int newIndex = index - snapshotNodes.Length;
                    if (newIndex < 0) snapshotNodes[index].TrySet(Nodes[index].Value.Key, value);
                    else newSnapshotNodes[newIndex].TrySet(Nodes[index].Value.Key, value);
                }
            }
            Nodes[index].Value.Value = value;
        }
        /// <summary>
        /// 设置快照数据
        /// </summary>
        /// <param name="index"></param>
        internal void TrySetSnapshotKeyValue(int index)
        {
            if (index < snapshotCount)
            {
                SnapshotDictionarySnapshotNode<KT, VT>[] snapshotNodes = this.snapshotNodes, newSnapshotNodes = this.newSnapshotNodes;
                if (index < snapshotCount)
                {
                    int newIndex = index - snapshotNodes.Length;
                    if (newIndex < 0) snapshotNodes[index].TrySet(ref Nodes[index].Value);
                    else newSnapshotNodes[newIndex].TrySet(ref Nodes[index].Value);
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
                Array.Clear(Nodes, 0, dictionary.NodeArrayClearCount);
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 快照字典节点数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    internal sealed class SnapshotDictionaryNodeArray<T> : ISnapshotEnumerable<BinarySerializeKeyValue<byte[], T>>
    {
        /// <summary>
        /// 快照字典节点数组
        /// </summary>
        private readonly ISnapshotEnumerable<BinarySerializeKeyValue<HashBytes, T>> array;
        /// <summary>
        /// 获取快照对象集合
        /// </summary>
        public IEnumerable<BinarySerializeKeyValue<byte[], T>> SnapshotValues
        {
            get
            {
                foreach (BinarySerializeKeyValue<HashBytes, T> value in array.SnapshotValues)
                {
                    yield return new BinarySerializeKeyValue<byte[], T>(value.Key.SubArray.Array, value.Value);
                }
            }
        }
        /// <summary>
        /// 快照字典节点数组
        /// </summary>
        /// <param name="array"></param>
        internal SnapshotDictionaryNodeArray(ISnapshotEnumerable<BinarySerializeKeyValue<HashBytes, T>> array)
        {
            this.array = array;
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        public void GetSnapshotValueArray()
        {
            array.GetSnapshotValueArray();
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        public void GetSnapshotResult()
        {
            array.GetSnapshotResult();
        }
        /// <summary>
        /// 关闭快照操作
        /// </summary>
        public void CloseSnapshot()
        {
            array.CloseSnapshot();
        }
    }
}
