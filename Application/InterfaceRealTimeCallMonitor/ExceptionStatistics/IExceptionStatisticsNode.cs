using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// The interface of the statistical information node for exception calls
    /// 异常调用统计信息节点接口
    /// </summary>
    [ServerNode]
    public partial interface IExceptionStatisticsNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="stringArray">String cache
        /// 字符串缓存</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetStringArray(LeftArray<string> stringArray);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSet(BinarySerializeKeyValue<long, ExceptionStatistics> value);
        /// <summary>
        /// Remove the current node
        /// 移除当前节点
        /// </summary>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveNode();
        /// <summary>
        /// Get the total number of exception calls
        /// 获取异常调用总次数
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        long GetCount();
        /// <summary>
        /// Get the quantity of exception statistical information
        /// 获取异常统计信息数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetStatisticsCount();
        /// <summary>
        /// Add exception call time
        /// 添加异常调用时间
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        /// <param name="callTime">Exception call time
        /// 异常调用时间</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Append(string callType, string callName, DateTime callTime);
        /// <summary>
        /// Remove exception statistics
        /// 移除异常统计信息
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Remove(string callType, string callName);
        /// <summary>
        /// Get the statistical information of call exceptions
        /// 获取调用异常统计信息
        /// </summary>
        /// <param name="callType">Call interface type
        /// 调用接口类型</param>
        /// <param name="callName">The name of the interface method to be called
        /// 调用接口方法名称</param>
        /// <returns>Exception statistical information, failure returns null
        /// 异常统计信息，失败返回 null</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ExceptionStatistics? GetStatistics(string callType, string callName);
#else
        ExceptionStatistics GetStatistics(string callType, string callName);
#endif
        /// <summary>
        /// Get the statistics of the specified number of call exceptions
        /// 获取指定数量调用异常统计信息
        /// </summary>
        /// <param name="count">The number of get the statistical information of call exceptions
        /// 获取调用异常统计信息数量</param>
        /// <param name="callback">The callback delegation for get the statistical information of the call exception
        /// 获取调用异常统计信息回调委托</param>
        [ServerMethod(IsPersistence = false)]
        void GetManyStatistics(int count, MethodKeepCallback<CallExceptionStatistics> callback);
    }
}
