using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序字典节点
    /// </summary>
    /// <typeparam name="KT">排序关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public class SortedDictionaryNode<KT, VT> : ISortedDictionaryNode<KT, VT>, ISnapshot<KeyValue<KT, VT>>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 排序字典
        /// </summary>
        private SortedDictionary<KT, VT> dictionary;
        /// <summary>
        /// 排序字典节点
        /// </summary>
        public SortedDictionaryNode()
        {
            dictionary = new SortedDictionary<KT, VT>();
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<KeyValue<KT, VT>> GetSnapshotArray()
        {
            return ServerNode.GetSnapshotArray(dictionary);
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, VT> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return dictionary.Count;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(VT value)
        {
            return dictionary.ContainsValue(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ValueResult<VT> TryGetValue(KT key)
        {
            var value = default(VT);
            if (key != null && dictionary.TryGetValue(key, out value)) return value;
            return default(ValueResult<VT>);
        }
    }
}
