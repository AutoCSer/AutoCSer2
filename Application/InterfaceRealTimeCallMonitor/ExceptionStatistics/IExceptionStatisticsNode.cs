using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 异常调用统计信息节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false)]
    public partial interface IExceptionStatisticsNode
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="stringArray">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSetStringArray(LeftArray<string> stringArray);
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSet(BinarySerializeKeyValue<long, ExceptionStatistics> value);
        /// <summary>
        /// 获取异常调用总次数
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        long GetCount();
        /// <summary>
        /// 获取异常统计信息数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetStatisticsCount();
        /// <summary>
        /// 添加异常调用时间
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <param name="callTime">异常调用时间</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Append(string callType, string callName, DateTime callTime);
        /// <summary>
        /// 移除异常统计信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Remove(string callType, string callName);
        /// <summary>
        /// 获取调用异常统计信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <returns>异常统计信息，失败返回 null</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ExceptionStatistics? GetStatistics(string callType, string callName);
#else
        ExceptionStatistics GetStatistics(string callType, string callName);
#endif
        /// <summary>
        /// 获取指定数量调用异常统计信息
        /// </summary>
        /// <param name="count">获取调用异常统计信息数量</param>
        /// <param name="callback">获取数量调用异常统计信息回调委托</param>
        [ServerMethod(IsPersistence = false)]
        void GetManyStatistics(int count, MethodKeepCallback<CallExceptionStatistics> callback);
    }
}
