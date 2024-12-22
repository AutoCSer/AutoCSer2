using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 栈节点（后进先出）
    /// </summary>
    [ServerNode(MethodIndexEnumType = typeof(ByteArrayStackNodeMethodEnum), IsAutoMethodIndex = false)]
    public interface IByteArrayStackNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, IsSnapshotMethod = true)]
        void SnapshotAdd(byte[] value);
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
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Push(ServerByteArray value);
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        ValueResult<byte[]?> TryPop();
#else
        ValueResult<byte[]> TryPop();
#endif
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        ResponseParameter TryPopResponseParameter();
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryPeek();
#else
        ValueResult<byte[]> TryPeek();
#endif
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryPeekResponseParameter();
    }
}
