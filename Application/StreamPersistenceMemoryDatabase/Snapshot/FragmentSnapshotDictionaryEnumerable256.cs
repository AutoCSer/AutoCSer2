using AutoCSer.Memory;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 快照字典快照集合
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class FragmentSnapshotDictionaryEnumerable256<KT, VT> : ISnapshotEnumerable<KeyValue<KT, VT>>, ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>>, ISnapshotEnumerable<VT>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 256 基分片 快照字典
        /// </summary>
        private readonly FragmentSnapshotDictionary256<KT, VT> dictionary;
        /// <summary>
        /// 快照字典节点数组集合
        /// </summary>
        private LeftArray<SnapshotDictionaryNodeArray<KT, VT>> snapshotArray;
        /// <summary>
        /// 256 基分片 快照字典快照集合
        /// </summary>
        /// <param name="dictionary">256 基分片 快照字典</param>
        internal FragmentSnapshotDictionaryEnumerable256(FragmentSnapshotDictionary256<KT, VT> dictionary)
        {
            this.dictionary = dictionary;
            snapshotArray.SetEmpty();
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<BinarySerializeKeyValue<KT, VT>> ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>>.SnapshotValues
        {
            get
            {
                int count = snapshotArray.Length;
                if (count != 0)
                {
                    foreach (SnapshotDictionaryNodeArray<KT, VT> node in snapshotArray.Array)
                    {
                        foreach (BinarySerializeKeyValue<KT, VT> value in ((ISnapshotEnumerable<BinarySerializeKeyValue<KT, VT>>)node).SnapshotValues) yield return value;
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<KeyValue<KT, VT>> ISnapshotEnumerable<KeyValue<KT, VT>>.SnapshotValues
        {
            get
            {
                int count = snapshotArray.Length;
                if (count != 0)
                {
                    foreach (SnapshotDictionaryNodeArray<KT, VT> node in snapshotArray.Array)
                    {
                        foreach (KeyValue<KT, VT> value in ((ISnapshotEnumerable<KeyValue<KT, VT>>)node).SnapshotValues) yield return value;
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<VT> ISnapshotEnumerable<VT>.SnapshotValues
        {
            get
            {
                int count = snapshotArray.Length;
                if (count != 0)
                {
                    foreach (SnapshotDictionaryNodeArray<KT, VT> node in snapshotArray.Array)
                    {
                        foreach (VT value in ((ISnapshotEnumerable<VT>)node).SnapshotValues) yield return value;
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
            foreach (var dictionary in this.dictionary.Dictionarys)
            {
                if (dictionary != null && dictionary.Count != 0)
                {
                    dictionary.Nodes.GetSnapshotValueArray();
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
            foreach (var dictionary in this.dictionary.Dictionarys)
            {
                if (dictionary != null)
                {
                    if (dictionary.Count != 0)
                    {
                        dictionary.Nodes.GetSnapshotResult();
                        snapshotArray.Add(dictionary.Nodes);
                    }
                    else dictionary.Nodes.CloseSnapshot();
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
                foreach (SnapshotDictionaryNodeArray<KT, VT> node in snapshotArray.Array)
                {
                    node.CloseSnapshot();
                    if (--count == 0) break;
                }
                snapshotArray.SetEmpty();
            }
        }
    }
    /// <summary>
    /// 256 基分片 快照字典快照集合
    /// </summary>
    /// <typeparam name="VT"></typeparam>
    internal sealed class FragmentSnapshotDictionaryEnumerable256<VT> : ISnapshotEnumerable<BinarySerializeKeyValue<byte[], VT>>
    {
        /// <summary>
        /// 256 基分片 快照字典
        /// </summary>
        private readonly FragmentSnapshotDictionary256<HashBytes, VT> dictionary;
        /// <summary>
        /// 快照字典节点数组集合
        /// </summary>
        private LeftArray<SnapshotDictionaryNodeArray<VT>> snapshotArray;
        /// <summary>
        /// 256 基分片 快照字典快照集合
        /// </summary>
        /// <param name="dictionary">256 基分片 快照字典</param>
        internal FragmentSnapshotDictionaryEnumerable256(FragmentSnapshotDictionary256<HashBytes, VT> dictionary)
        {
            this.dictionary = dictionary;
            snapshotArray.SetEmpty();
        }
        /// <summary>
        /// Get the collection of snapshot objects
        /// 获取快照对象集合
        /// </summary>
        IEnumerable<BinarySerializeKeyValue<byte[], VT>> ISnapshotEnumerable<BinarySerializeKeyValue<byte[], VT>>.SnapshotValues
        {
            get
            {
                int count = snapshotArray.Length;
                if (count != 0)
                {
                    foreach (SnapshotDictionaryNodeArray<VT> node in snapshotArray.Array)
                    {
                        foreach (BinarySerializeKeyValue<byte[], VT> value in node.SnapshotValues) yield return value;
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
            foreach (var dictionary in this.dictionary.Dictionarys)
            {
                if (dictionary != null && dictionary.Count != 0)
                {
                    dictionary.Nodes.GetSnapshotValueArray();
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
            foreach (var dictionary in this.dictionary.Dictionarys)
            {
                if (dictionary != null)
                {
                    if (dictionary.Count != 0)
                    {
                        SnapshotDictionaryNodeArray<VT> node = new SnapshotDictionaryNodeArray<VT>(dictionary.Nodes);
                        node.GetSnapshotResult();
                        snapshotArray.Add(node);
                    }
                    else dictionary.Nodes.CloseSnapshot();
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
                foreach (SnapshotDictionaryNodeArray<VT> node in snapshotArray.Array)
                {
                    node.CloseSnapshot();
                    if (--count == 0) break;
                }
                snapshotArray.SetEmpty();
            }
        }
    }
}
