using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Stack node interface (Last in, first out)
    /// 栈节点接口（后进先出）
    /// </summary>
    [ServerNode]
    public partial interface IByteArrayStackNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
#if NetStandard21
        void SnapshotAdd(byte[]? value);
#else
        void SnapshotAdd(byte[] value);
#endif
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
        /// Add the data to the stack
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Push(ServerByteArray value);
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
#if NetStandard21
        ValueResult<byte[]?> TryPop();
#else
        ValueResult<byte[]> TryPop();
#endif
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        ResponseParameter TryPopResponseParameter();
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryPeek();
#else
        ValueResult<byte[]> TryPeek();
#endif
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryPeekResponseParameter();
    }
}
