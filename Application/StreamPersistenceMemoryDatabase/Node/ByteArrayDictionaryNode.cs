using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public sealed class ByteArrayDictionaryNode<KT> : IByteArrayDictionaryNode<KT>
#if NetStandard21
        , ISnapshot<BinarySerializeKeyValue<KT, byte[]?>>
#else
        , ISnapshot<BinarySerializeKeyValue<KT, byte[]>>
#endif
        where KT : IEquatable<KT>
    {
        /// <summary>
        /// 字典
        /// </summary>
#if NetStandard21
        private Dictionary<KT, byte[]?> dictionary;
#else
        private Dictionary<KT, byte[]> dictionary;
#endif
        /// <summary>
        /// 字典节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public ByteArrayDictionaryNode(int capacity = 0)
        {
#if NetStandard21
            dictionary = DictionaryCreator<KT>.Create<byte[]?>(capacity);
#else
            dictionary = DictionaryCreator<KT>.Create<byte[]>(capacity);
#endif
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return dictionary.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
#if NetStandard21
        public SnapshotResult<BinarySerializeKeyValue<KT, byte[]?>> GetSnapshotResult(BinarySerializeKeyValue<KT, byte[]?>[] snapshotArray, object customObject)
#else
        public SnapshotResult<BinarySerializeKeyValue<KT, byte[]>> GetSnapshotResult(BinarySerializeKeyValue<KT, byte[]>[] snapshotArray, object customObject)
#endif
        {
            return ServerNode.GetSnapshotResult(dictionary, snapshotArray);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
#if NetStandard21
        public void SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<KT, byte[]?>> array, ref LeftArray<BinarySerializeKeyValue<KT, byte[]?>> newArray) { }
#else
        public void SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<KT, byte[]>> array, ref LeftArray<BinarySerializeKeyValue<KT, byte[]>> newArray) { }
#endif
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        public void SnapshotAdd(BinarySerializeKeyValue<KT, byte[]> value)
        {
            dictionary[value.Key] = value.Value;
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
#if NetStandard21
            dictionary = DictionaryCreator<KT>.Create<byte[]?>(capacity);
#else
            dictionary = DictionaryCreator<KT>.Create<byte[]>(capacity);
#endif
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
            if (key != null)
            {
                var value = default(byte[]);
                if (dictionary.TryGetValue(key, out value)) return (ResponseServerByteArray)value;
            }
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
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
        /// <returns>是否删除成功</returns>
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
