using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Stack node interface (Last in, first out)
    /// 栈节点接口（后进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsLocalClient = true, IsReturnValueNode = ServerNodeAttribute.DefaultIsReturnValueNode)]
    public partial interface IStackNode<T>
    {
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Determine whether there is matching data (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// Add the data to the stack
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        void Push(T value);
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        ValueResult<T> TryPop();
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> TryPeek();
    }
}
