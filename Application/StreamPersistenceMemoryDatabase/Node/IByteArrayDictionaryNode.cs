﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点接口
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    [ServerNode(MethodIndexEnumType = typeof(ByteArrayDictionaryNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IByteArrayDictionaryNode<KT> where KT : IEquatable<KT>
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotAdd(KeyValue<KT, byte[]> value);
        /// <summary>
        /// 清除所有数据并重建容器 持久化参数检查
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        bool RenewBeforePersistence(int capacity);
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Renew(int capacity = 0);
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> TryAddBeforePersistence(KT key, ServerByteArray value);
        /// <summary>
        /// 尝试添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(KT key, ServerByteArray value);
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> SetBeforePersistence(KT key, ServerByteArray value);
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(KT key, ServerByteArray value);
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryGetValue(KT key);
#else
        ValueResult<byte[]> TryGetValue(KT key);
#endif
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryGetResponseParameter(KT key);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(KT key);
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> RemoveBeforePersistence(KT key);
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        bool Remove(KT key);
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<ValueResult<byte[]>> GetRemoveBeforePersistence(KT key);
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
#if NetStandard21
        ValueResult<byte[]?> GetRemove(KT key);
#else
        ValueResult<byte[]> GetRemove(KT key);
#endif
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<ResponseParameter> GetRemoveResponseParameterBeforePersistence(KT key);
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        ResponseParameter GetRemoveResponseParameter(KT key);
    }
}