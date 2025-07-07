using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    public sealed class ByteArrayDictionaryNode<KT> : IByteArrayDictionaryNode<KT>
#if NetStandard21
        , IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]?>>
#else
        , IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]>>
#endif
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
#if NetStandard21
        private SnapshotDictionary<KT, byte[]?> dictionary;
#else
        private SnapshotDictionary<KT, byte[]> dictionary;
#endif
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
#if NetStandard21
        ISnapshotEnumerable<BinarySerializeKeyValue<KT, byte[]?>> IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]?>>.SnapshotEnumerable { get { return dictionary.Nodes; } }
#else
        ISnapshotEnumerable<BinarySerializeKeyValue<KT, byte[]>> IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]>>.SnapshotEnumerable { get { return dictionary.Nodes; } }
#endif
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        public ByteArrayDictionaryNode(int capacity = 0, ReusableDictionaryGroupTypeEnum groupType = ReusableDictionaryGroupTypeEnum.HashIndex)
        {
#if NetStandard21
            dictionary = new SnapshotDictionary<KT, byte[]?>(capacity, groupType);
#else
            dictionary = new SnapshotDictionary<KT, byte[]>(capacity, groupType);
#endif
        }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(BinarySerializeKeyValue<KT, byte[]> value)
        {
            dictionary[value.Key] = value.Value;
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
        public bool TryAdd(KT key, ServerByteArray value)
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
        public bool Set(KT key, ServerByteArray value)
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
#if NetStandard21
        public ValueResult<byte[]?> TryGetValue(KT key)
#else
        public ValueResult<byte[]> TryGetValue(KT key)
#endif
        {
            var value = default(byte[]);
            if (key != null && dictionary.TryGetValue(key, (uint)key.GetHashCode(), out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
#if NetStandard21
        public byte[]?[] GetValueArray(KT[] keys)
#else
        public byte[][] GetValueArray(KT[] keys)
#endif
        {
            return dictionary.GetValueArray(keys);
        }
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ResponseParameter TryGetResponseParameter(KT key)
        {
            if (key != null)
            {
                var value = default(byte[]);
                if (dictionary.TryGetValue(key, (uint)key.GetHashCode(), out value)) return (ResponseServerByteArray)value;
            }
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
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
#if NetStandard21
        public ValueResult<byte[]?> GetRemove(KT key)
#else
        public ValueResult<byte[]> GetRemove(KT key)
#endif
        {
            if (key != null)
            {
                var value = default(byte[]);
                if (dictionary.Remove(key, (uint)key.GetHashCode(), out value)) return value;
            }
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        public ResponseParameter GetRemoveResponseParameter(KT key)
        {
            if (key != null)
            {
                var value = default(byte[]);
                if (dictionary.Remove(key, (uint)key.GetHashCode(), out value)) return (ResponseServerByteArray)value;
            }
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
