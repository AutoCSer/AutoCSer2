using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 HashString 字典 节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(MethodIndexEnumType = typeof(HashStringFragmentDictionaryNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IHashStringFragmentDictionaryNode<T>
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotAdd(KeyValue<string, T> value);
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> TryGetValue(string key);
        /// <summary>
        /// 清除数据（保留分片数组）
        /// </summary>
        void Clear();
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        void ClearArray();
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> TryAddBeforePersistence(string key, T value);
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(string key, T value);
        /// <summary>
        /// 强制设置数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> SetBeforePersistence(string key, T value);
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(string key, T value);
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(string key);
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> RemoveBeforePersistence(string key);
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        bool Remove(string key);
        /// <summary>
        /// 删除关键字并返回被删除数据 持久化参数检查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<ValueResult<T>> GetRemoveBeforePersistence(string key);
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        ValueResult<T> GetRemove(string key);
    }
}
