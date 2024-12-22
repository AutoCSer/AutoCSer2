using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 栈节点（后进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(MethodIndexEnumType = typeof(StackNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface IStackNode<T>
    {
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsSnapshotMethod = true, IsIgnorePersistenceCallbackException = true)]
        void Push(T value);
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        ValueResult<T> TryPop();
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> TryPeek();
    }
}
