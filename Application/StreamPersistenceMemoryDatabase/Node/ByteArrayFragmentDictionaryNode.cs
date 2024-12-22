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
        , ISnapshot<KeyValue<KT, byte[]?>>
#else
        , ISnapshot<KeyValue<KT, byte[]>>
#endif
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 256 基分片字典
        /// </summary>
#if NetStandard21
        private readonly FragmentDictionary256<KT, byte[]?> dictionary = new FragmentDictionary256<KT, byte[]?>();
#else
        private readonly FragmentDictionary256<KT, byte[]> dictionary = new FragmentDictionary256<KT, byte[]>();
#endif
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
#if NetStandard21
        public LeftArray<KeyValue<KT, byte[]?>> GetSnapshotArray()
#else
        public LeftArray<KeyValue<KT, byte[]>> GetSnapshotArray()
#endif
        {
#if NetStandard21
            if (dictionary.Count == 0) return new LeftArray<KeyValue<KT, byte[]?>>(0);
            LeftArray<KeyValue<KT, byte[]?>> array = new LeftArray<KeyValue<KT, byte[]?>>(dictionary.Count);
#else
            if (dictionary.Count == 0) return new LeftArray<KeyValue<KT, byte[]>>(0);
            LeftArray<KeyValue<KT, byte[]>> array = new LeftArray<KeyValue<KT, byte[]>>(dictionary.Count);
#endif
            foreach (var value in dictionary.KeyValues) array.Array[array.Length++].Set(value.Key, value.Value);
            return array;
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<KT, byte[]> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count() { return dictionary.Count; }
        /// <summary>
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
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            dictionary.ClearArray();
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, ServerByteArray value)
        {
            return key != null && dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
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
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(KT key)
        {
            return key != null && dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(KT key)
        {
            return key != null && dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
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
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
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
