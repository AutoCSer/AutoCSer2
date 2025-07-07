using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="ST">Snapshot data type
    /// 快照数据类型</typeparam>
    public abstract class DictionaryNode<KT, VT, ST>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
        protected SnapshotDictionary<KT, VT> dictionary;
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        protected DictionaryNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex)
        {
            dictionary = new SnapshotDictionary<KT, VT>(capacity, groupType);
        }
        /// <summary>
        /// Clear all data and rebuild the container (to solve the problem of low performance of the clear call when the data volume is large)
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">Initialize the size of the new container
        /// 新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            dictionary.Renew(capacity);
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dictionary.Count;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, (uint)key.GetHashCode(), value);
        }
        /// <summary>
        /// Force the data to be set and overwrite if the keyword already exists
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        public bool Set(KT key, VT value)
        {
            if (key != null)
            {
                dictionary[key] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && dictionary.TryGetValue(key, (uint)key.GetHashCode(), out value)) return value;
            return default(ValueResult<VT>);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
#if NetStandard21
        public VT?[] GetValueArray(KT[] keys)
#else
        public VT[] GetValueArray(KT[] keys)
#endif
        {
            return dictionary.GetValueArray(keys);
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            dictionary.ClearArray();
        }
        /// <summary>
        /// Reusable dictionaries reset data locations (The presence of reference type data can cause memory leaks)
        /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            dictionary.ClearCount();
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key, (uint)key.GetHashCode());
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key, (uint)key.GetHashCode());
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
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        public ValueResult<VT> GetRemove(KT key)
        {
            if (key != null)
            {
                var value = default(VT);
                if (dictionary.Remove(key, (uint)key.GetHashCode(), out value)) return value;
            }
            return default(ValueResult<VT>);
        }
    }
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
#if AOT
    public abstract class DictionaryNode<KT, VT> : DictionaryNode<KT, VT, KeyValue<KT, VT>>, IEnumerableSnapshot<KeyValue<KT, VT>>
#else
    public sealed class DictionaryNode<KT, VT> : DictionaryNode<KT, VT, KeyValue<KT, VT>>, IDictionaryNode<KT, VT>, IEnumerableSnapshot<KeyValue<KT, VT>>
#endif
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<KeyValue<KT, VT>> IEnumerableSnapshot<KeyValue<KT, VT>>.SnapshotEnumerable { get { return dictionary.Nodes; } }
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        public DictionaryNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex) : base(capacity, groupType) { }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary[value.Key] = value.Value;
        }
    }
}
