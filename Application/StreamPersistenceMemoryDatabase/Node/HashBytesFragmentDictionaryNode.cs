﻿using AutoCSer.Extensions;
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
        , ISnapshot<KeyValue<byte[], byte[]?>>
#else
        , ISnapshot<KeyValue<byte[], byte[]>>
#endif
    {
        /// <summary>
        /// 256 基分片 HashBytes 字典
        /// </summary>
#if NetStandard21
        private readonly HashBytesFragmentDictionary256<byte[]?> dictionary = new HashBytesFragmentDictionary256<byte[]?>();
#else
        private readonly HashBytesFragmentDictionary256<byte[]> dictionary = new HashBytesFragmentDictionary256<byte[]>();
#endif
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
#if NetStandard21
        public LeftArray<KeyValue<byte[], byte[]?>> GetSnapshotArray()
#else
        public LeftArray<KeyValue<byte[], byte[]>> GetSnapshotArray()
#endif
        {
#if NetStandard21
            if (dictionary.Count == 0) return new LeftArray<KeyValue<byte[], byte[]?>>(0);
            LeftArray<KeyValue<byte[], byte[]?>> array = new LeftArray<KeyValue<byte[], byte[]?>>(dictionary.Count);
#else
            if (dictionary.Count == 0) return new LeftArray<KeyValue<byte[], byte[]>>(0);
            LeftArray<KeyValue<byte[], byte[]>> array = new LeftArray<KeyValue<byte[], byte[]>>(dictionary.Count);
#endif
            foreach (var value in dictionary.KeyValues) array.Array[array.Length++].Set(value.Key.SubArray.Array, value.Value);
            return array;
        }
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(KeyValue<byte[], byte[]> value)
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
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryAddBeforePersistence(ServerByteArray key, ServerByteArray value)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer == null || dictionary.ContainsKey(keyBuffer)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(ServerByteArray key, ServerByteArray value)
        {
            return dictionary.TryAdd(key.Buffer.notNull(), value.Buffer);
        }
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBeforePersistence(ServerByteArray key, ServerByteArray value)
        {
            if (key.Buffer == null) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public bool Set(ServerByteArray key, ServerByteArray value)
        {
            dictionary[key.Buffer.notNull()] = value;
            return true;
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
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer == null || !dictionary.ContainsKey(keyBuffer)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(ServerByteArray key)
        {
            return dictionary.Remove(key.Buffer.notNull());
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<byte[]>> GetRemoveBeforePersistence(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer == null || !dictionary.ContainsKey(keyBuffer)) return default(ValueResult<byte[]>);
            return default(ValueResult<ValueResult<byte[]>>);
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
            var value = default(byte[]);
            if (dictionary.Remove(key.Buffer.notNull(), out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ResponseParameter> GetRemoveResponseParameterBeforePersistence(ServerByteArray key)
        {
            var keyBuffer = key.Buffer;
            if (keyBuffer == null || !dictionary.ContainsKey(keyBuffer)) return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
            return default(ValueResult<ResponseParameter>);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        public ResponseParameter GetRemoveResponseParameter(ServerByteArray key)
        {
            var value = default(byte[]);
            if (dictionary.Remove(key.Buffer.notNull(), out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}