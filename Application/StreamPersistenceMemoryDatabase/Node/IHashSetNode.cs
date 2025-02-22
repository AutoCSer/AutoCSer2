using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 哈希表节点接口
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IHashSetNode<T> where T : IEquatable<T>
    {
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
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
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
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(T value);
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        T[] GetArray();
    }
}
