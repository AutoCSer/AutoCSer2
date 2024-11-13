using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 哈希表节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public class HashSetNode<T> : ContextNode<IHashSetNode<T>>, IHashSetNode<T>, ISnapshot<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 哈希表
        /// </summary>
        private HashSet<T> hashSet;
        /// <summary>
        /// 哈希表节点
        /// </summary>
        public HashSetNode()
        {
            hashSet = HashSetCreator<T>.Create();
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return hashSet.getLeftArray();
        }
        /// <summary>
        /// 清除所有数据并重建容器 持久化参数检查
        /// </summary>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        public bool RenewBeforePersistence()
        {
            streamPersistenceMemoryDatabaseService.SetBeforePersistenceMethodParameterCustomSessionObject(HashSetCreator<T>.Create());
            return true;
        }
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void Renew()
        {
            hashSet = streamPersistenceMemoryDatabaseService?.GetBeforePersistenceMethodParameterCustomSessionObject().castType<HashSet<T>>() ?? HashSetCreator<T>.Create();
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return hashSet.Count;
        }
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> AddBeforePersistence(T value)
        {
            if (value == null || hashSet.Contains(value)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return hashSet.Add(value);
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            hashSet.Clear();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value);
        }
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(T value)
        {
            if (value == null || !hashSet.Contains(value)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(T value)
        {
            return hashSet.Remove(value);
        }
    }
}
