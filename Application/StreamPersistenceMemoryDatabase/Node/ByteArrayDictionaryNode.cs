using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    public class ByteArrayDictionaryNode<KT> : ContextNode<IByteArrayDictionaryNode<KT>>, IByteArrayDictionaryNode<KT>
#if NetStandard21
        , ISnapshot<KeyValue<KT, byte[]?>>
#else
        , ISnapshot<KeyValue<KT, byte[]>>
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
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
#if NetStandard21
        public LeftArray<KeyValue<KT, byte[]?>> GetSnapshotArray()
#else
        public LeftArray<KeyValue<KT, byte[]>> GetSnapshotArray()
#endif
        {
            return ServerNode.GetSnapshotArray(dictionary);
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
        /// 清除所有数据并重建容器 持久化参数检查
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool RenewBeforePersistence(int capacity)
        {
            streamPersistenceMemoryDatabaseService.SetBeforePersistenceMethodParameterCustomSessionObject(DictionaryCreator<KT>.Create<byte[]>(capacity));
            return true;
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        public void Renew(int capacity = 0)
        {
#if NetStandard21
            dictionary = streamPersistenceMemoryDatabaseService?.GetBeforePersistenceMethodParameterCustomSessionObject().castType<Dictionary<KT, byte[]?>>() ?? DictionaryCreator<KT>.Create<byte[]?>(capacity);
#else
            dictionary = streamPersistenceMemoryDatabaseService?.GetBeforePersistenceMethodParameterCustomSessionObject().castType<Dictionary<KT, byte[]>>() ?? DictionaryCreator<KT>.Create<byte[]>(capacity);
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
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> TryAddBeforePersistence(KT key, ServerByteArray value)
        {
            if (key == null || dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool TryAdd(KT key, ServerByteArray value)
        {
            return dictionary.TryAdd(key, value);
        }
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> SetBeforePersistence(KT key, ServerByteArray value)
        {
            if (key == null) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        public bool Set(KT key, ServerByteArray value)
        {
            dictionary[key] = value;
            return true;
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
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(KT key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KT key)
        {
            return dictionary.Remove(key);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<byte[]>> GetRemoveBeforePersistence(KT key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return default(ValueResult<byte[]>);
            return default(ValueResult<ValueResult<byte[]>>);
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
            var value = default(byte[]);
            if (dictionary.Remove(key, out value)) return value;
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
        public ValueResult<ResponseParameter> GetRemoveResponseParameterBeforePersistence(KT key)
        {
            if (key == null || !dictionary.ContainsKey(key)) return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
            return default(ValueResult<ResponseParameter>);
        }
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        public ResponseParameter GetRemoveResponseParameter(KT key)
        {
            var value = default(byte[]);
            if (dictionary.Remove(key, out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
