using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Binary search tree dictionary node
    /// 二叉搜索树字典节点
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="ST">Snapshot data type
    /// 快照数据类型</typeparam>
    public abstract class SearchTreeDictionaryNode<KT, VT, ST>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// Binary search tree dictionary
        /// 二叉搜索树字典
        /// </summary>
        protected AutoCSer.SearchTree.Dictionary<KT, VT> dictionary;
        /// <summary>
        /// Binary search tree dictionary node
        /// 二叉搜索树字典节点
        /// </summary>
        public SearchTreeDictionaryNode()
        {
            dictionary = new SearchTree.Dictionary<KT, VT>();
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return dictionary.Count;
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<ST> array, ref LeftArray<ST> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// Get the number of node data
        /// 获取节点数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dictionary.Count;
        }
        /// <summary>
        /// Get the tree height has a time complexity of O(n)
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        public int GetHeight()
        {
            return dictionary.Height;
        }
        /// <summary>
        /// Clear the data
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
        public bool Set(KT key, VT value)
        {
            return key != null && dictionary.Set(key, value);
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Whether new data has been added
        /// 是否添加了新数据</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        public int RemoveKeys(KT[] keys)
        {
            return dictionary.RemoveKeys(keys);
        }
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (dictionary.Remove(key, out value)) return value;
            }
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Determines if the keyword exists
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Whether the keyword exists
        /// 是否存在关键字</returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Target data
        /// 目标数据</returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && dictionary.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public VT[] GetValueArray(KT[] keys)
        {
            return dictionary.GetValueArray(keys);
        }
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        public int IndexOf(KT key)
        {
            return key != null ? dictionary.IndexOf(key) : -1;
        }
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        public int CountLess(KT key)
        {
            return key != null ? dictionary.CountLess(key) : -1;
        }
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        public int CountThan(KT key)
        {
            return key != null ? dictionary.CountThan(key) : -1;
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        public ValueResult<VT> TryGetValueByIndex(int index)
        {
            var value = default(VT);
            if ((uint)index < (uint)dictionary.Count && dictionary.TryGetValueByIndex(index, out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get the first keyword
        /// 获取第一个关键字
        /// </summary>
        /// <returns>The first keyword
        /// 第一个关键字</returns>
        public ValueResult<KT> TryGetFirstKey()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue.Key;
            return default(ValueResult<KT>);
        }
        /// <summary>
        /// Get the last keyword
        /// 获取最后一个关键字
        /// </summary>
        /// <returns>The last keyword
        /// 最后一个关键字</returns>
        public ValueResult<KT> TryGetLastKey()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue.Key;
            return default(ValueResult<KT>);
        }
        /// <summary>
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        /// <returns>The first data
        /// 第一个数据</returns>
        public ValueResult<VT> TryGetFirstValue()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue.Value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        /// <returns>The last data
        /// 最后一个数据</returns>
        public ValueResult<VT> TryGetLastValue()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue.Value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get a collection of data based on the range
        /// 根据范围获取数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <returns></returns>
        public IEnumerable<ValueResult<VT>> GetValues(int skipCount, byte getCount)
        {
            if (skipCount >= 0) return dictionary.GetValues(skipCount, getCount).Cast();
            return ValueResult<VT>.NullEnumerable;
        }
    }
    /// <summary>
    /// Binary search tree dictionary node
    /// 二叉搜索树字典节点
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
#if AOT
    public abstract class SearchTreeDictionaryNode<KT, VT> : SearchTreeDictionaryNode<KT, VT, KeyValue<KT, VT>>, ISnapshot<KeyValue<KT, VT>>
#else
    public sealed class SearchTreeDictionaryNode<KT, VT> : SearchTreeDictionaryNode<KT, VT, KeyValue<KT, VT>>, ISearchTreeDictionaryNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
#endif
        where KT : IComparable<KT>
    {
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        public SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult(KeyValue<KT, VT>[] snapshotArray, object customObject)
        {
            return new SnapshotResult<KeyValue<KT, VT>>(snapshotArray, dictionary.KeyValues, dictionary.Count);
        }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary.Set(value.Key, value.Value);
        }
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetKeyValueByIndex(int index)
        {
            if ((uint)index < (uint)dictionary.Count) return dictionary.At(index);
            return default(ValueResult<KeyValue<KT, VT>>);
        }
        /// <summary>
        /// Get the first pair of data
        /// 获取第一对数据
        /// </summary>
        /// <returns>The first pair of data
        /// 第一对数据</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetFirstKeyValue()
        {
            if (dictionary.Count != 0) return dictionary.FristKeyValue;
            return default(ValueResult<KeyValue<KT, VT>>);
        }
        /// <summary>
        /// Get the last pair of data
        /// 获取最后一对数据
        /// </summary>
        /// <returns>The last pair of data
        /// 最后一对数据</returns>
        public ValueResult<KeyValue<KT, VT>> TryGetLastKeyValue()
        {
            if (dictionary.Count != 0) return dictionary.LastKeyValue;
            return default(ValueResult<KeyValue<KT, VT>>);
        }
    }
}
