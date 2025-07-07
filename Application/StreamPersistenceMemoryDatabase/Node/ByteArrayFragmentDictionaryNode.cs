using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片字典 节点
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    public sealed class ByteArrayFragmentDictionaryNode<KT> : IByteArrayFragmentDictionaryNode<KT>
#if NetStandard21
        , IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]?>>
#else
        , IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]>>
#endif
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 256 基分片字典
        /// </summary>
#if NetStandard21
        private readonly FragmentSnapshotDictionary256<KT, byte[]?> dictionary = new FragmentSnapshotDictionary256<KT, byte[]?>();
#else
        private readonly FragmentSnapshotDictionary256<KT, byte[]> dictionary = new FragmentSnapshotDictionary256<KT, byte[]>();
#endif
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
#if NetStandard21
        ISnapshotEnumerable<BinarySerializeKeyValue<KT, byte[]?>> IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]?>>.SnapshotEnumerable { get { return dictionary.GetBinarySerializeKeyValueSnapshot(); } }
#else
        ISnapshotEnumerable<BinarySerializeKeyValue<KT, byte[]>> IEnumerableSnapshot<BinarySerializeKeyValue<KT, byte[]>>.SnapshotEnumerable { get { return dictionary.GetBinarySerializeKeyValueSnapshot(); } }
#endif
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
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count() { return dictionary.Count; }
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
            if (key != null && dictionary.TryGetValue(key, out value)) return value;
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
            var value = default(byte[]);
            if (key != null && dictionary.TryGetValue(key, out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// Clear the data (retain the fragmented array)
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            dictionary.ClearArray();
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool TryAdd(KT key, ServerByteArray value)
        {
            return key != null && dictionary.TryAdd(key, value);
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
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
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
                if (dictionary.Remove(key, out value)) return value;
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
                if (dictionary.Remove(key, out value)) return (ResponseServerByteArray)value;
            }
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
