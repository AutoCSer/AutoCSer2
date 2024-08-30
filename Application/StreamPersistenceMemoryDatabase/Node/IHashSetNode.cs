using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 哈希表节点接口
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [ServerNode(MethodIndexEnumType = typeof(HashSetNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IHashSetNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 清除所有数据并重建容器 持久化参数检查
        /// </summary>
        /// <returns>返回 true 表示需要继续调用持久化方法</returns>
        bool RenewBeforePersistence();
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Renew();
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 添加数据 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> AddBeforePersistence(T value);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(IsSnapshotMethod = true, IsIgnorePersistenceCallbackException = true)]
        bool Add(T value);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        ValueResult<bool> RemoveBeforePersistence(T value);
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        bool Remove(T value);
    }
}
