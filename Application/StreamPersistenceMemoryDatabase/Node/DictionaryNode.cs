using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    public sealed class DictionaryNode<KT, VT> : IDictionaryNode<KT, VT>, IEnumerableSnapshot<KeyValue<KT, VT>>
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
        private SnapshotDictionary<KT, VT> dictionary;
        /// <summary>
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<KeyValue<KT, VT>> IEnumerableSnapshot<KeyValue<KT, VT>>.SnapshotEnumerable { get { return dictionary.Nodes; } }
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        /// <param name="groupType">可重用字典重组操作类型</param>
        public DictionaryNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex)
        {
            dictionary = new SnapshotDictionary<KT, VT>(capacity, groupType);
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
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
            dictionary.Renew(capacity);
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
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, VT value)
        {
            return key != null && dictionary.TryAdd(key, (uint)key.GetHashCode(), value);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
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
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            dictionary.ClearArray();
        }
        /// <summary>
        /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            dictionary.ClearCount();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key, (uint)key.GetHashCode());
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key, (uint)key.GetHashCode());
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>删除关键字数量</returns>
        public int RemoveKeys(KT[] keys)
        {
            return dictionary.RemoveKeys(keys);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
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
}
