using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 HashBytes 字典 节点
    /// </summary>
    public sealed class HashBytesFragmentDictionaryNode : IHashBytesFragmentDictionaryNode
#if NetStandard21
        , IEnumerableSnapshot<BinarySerializeKeyValue<byte[], byte[]?>>
#else
        , IEnumerableSnapshot<BinarySerializeKeyValue<byte[], byte[]>>
#endif
    {
        /// <summary>
        /// 256 基分片 HashBytes 字典
        /// </summary>
#if NetStandard21
        private readonly FragmentSnapshotDictionary256<HashBytes, byte[]?> dictionary = new FragmentSnapshotDictionary256<HashBytes, byte[]?>();
#else
        private readonly FragmentSnapshotDictionary256<HashBytes, byte[]> dictionary = new FragmentSnapshotDictionary256<HashBytes, byte[]>();
#endif
        /// <summary>
        /// 快照集合
        /// </summary>
#if NetStandard21
        ISnapshotEnumerable<BinarySerializeKeyValue<byte[], byte[]?>> IEnumerableSnapshot<BinarySerializeKeyValue<byte[], byte[]?>>.SnapshotEnumerable { get { return new FragmentSnapshotDictionaryEnumerable256<byte[]?>(dictionary); } }
#else
        ISnapshotEnumerable<BinarySerializeKeyValue<byte[], byte[]>> IEnumerableSnapshot<BinarySerializeKeyValue<byte[], byte[]>>.SnapshotEnumerable { get { return new FragmentSnapshotDictionaryEnumerable256<byte[]>(dictionary); } }
#endif
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void SnapshotAdd(BinarySerializeKeyValue<byte[], byte[]?> value)
#else
        public void SnapshotAdd(BinarySerializeKeyValue<byte[], byte[]> value)
#endif
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
        public ValueResult<byte[]?> TryGetValue(ServerByteArray key)
#else
        public ValueResult<byte[]> TryGetValue(ServerByteArray key)
#endif
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer != null)
            {
                var value = default(byte[]);
                if (dictionary.TryGetValue(keyBuffer, out value)) return value;
            }
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
        public ResponseParameter TryGetResponseParameter(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer != null)
            {
                var value = default(byte[]);
                if (dictionary.TryGetValue(keyBuffer, out value)) return (ResponseServerByteArray)value;
            }
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
        public bool TryAdd(ServerByteArray key, ServerByteArray value)
        {
            var keyBuffer = key.Buffer;
            return keyBuffer != null && dictionary.TryAdd(keyBuffer, value.Buffer);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public bool Set(ServerByteArray key, ServerByteArray value)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer != null)
            {
                dictionary[keyBuffer] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            return keyBuffer != null && dictionary.ContainsKey(keyBuffer);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            return keyBuffer != null && dictionary.Remove(keyBuffer);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> GetRemove(ServerByteArray key)
#else
        public ValueResult<byte[]> GetRemove(ServerByteArray key)
#endif
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer != null)
            {
                var value = default(byte[]);
                if (dictionary.Remove(keyBuffer, out value)) return value;
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
        public ResponseParameter GetRemoveResponseParameter(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer != null)
            {
                var value = default(byte[]);
                if (dictionary.Remove(keyBuffer, out value)) return (ResponseServerByteArray)value;
            }
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
