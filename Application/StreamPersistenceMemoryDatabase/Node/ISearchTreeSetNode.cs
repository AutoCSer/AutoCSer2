﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二叉搜索树集合节点接口
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [ServerNode(MethodIndexEnumType = typeof(SearchTreeSetNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface ISearchTreeSetNode<T> where T : IComparable<T>
    {
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
        /// <param name="value">关键字</param>
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
        /// <param name="value">关键字</param>
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
        /// <param name="value">关键字</param>
        /// <returns>是否删除成功</returns>
        bool Remove(T value);
        /// <summary>
        /// 获取第一个数据
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetFrist();
        /// <summary>
        /// 获取最后一个数据
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetLast();
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(T value);
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        [ServerMethod(IsPersistence = false)]
        int CountLess(T value);
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        [ServerMethod(IsPersistence = false)]
        int CountThan(T value);
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetByIndex(int index);
    }
}