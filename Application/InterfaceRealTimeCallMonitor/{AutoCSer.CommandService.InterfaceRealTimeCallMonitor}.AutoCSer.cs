//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 接口实时调用监视服务接口 客户端接口
        /// </summary>
        public partial interface IInterfaceRealTimeCallMonitorServiceClientController
        {
            /// <summary>
            /// 接口监视服务在线检查
            /// </summary>
            AutoCSer.Net.EnumeratorCommand Check();
            /// <summary>
            /// 调用完成
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="isException">接口是否执行异常</param>
            AutoCSer.Net.SendOnlyCommand Completed(long callIdentity, bool isException);
            /// <summary>
            /// 获取未完成调用数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<int> GetCount();
            /// <summary>
            /// 获取异常调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetException();
            /// <summary>
            /// 获取超时调用数据
            /// </summary>
            /// <returns>实时调用时间戳信息回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeout();
            /// <summary>
            /// 获取指定数量的超时调用
            /// </summary>
            /// <param name="count">获取数量</param>
            /// <returns>超时调用回调</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallTimestamp> GetTimeoutCalls(int count);
            /// <summary>
            /// 获取超时调用数量
            /// </summary>
            /// <returns>超时调用数量</returns>
            AutoCSer.Net.ReturnCommand<int> GetTimeoutCount();
            /// <summary>
            /// 设置自定义调用步骤
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="step">自定义调用步骤</param>
            AutoCSer.Net.SendOnlyCommand SetStep(long callIdentity, int step);
            /// <summary>
            /// 新增一个实时调用信息
            /// </summary>
            /// <param name="callIdentity">调用标识</param>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <param name="timeoutMilliseconds">超时毫秒数</param>
            /// <param name="type">调用类型</param>
            AutoCSer.Net.SendOnlyCommand Start(long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type);
        }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 异常调用统计信息节点接口 客户端节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientNode(typeof(AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IExceptionStatisticsNode))]
        public partial interface IExceptionStatisticsNodeClientNode
        {
            /// <summary>
            /// 添加异常调用时间
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <param name="callTime">异常调用时间</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Append(string callType, string callName, System.DateTime callTime);
            /// <summary>
            /// 获取异常调用总次数
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<long> GetCount();
            /// <summary>
            /// 获取指定数量调用异常统计信息
            /// </summary>
            /// <param name="count">获取调用异常统计信息数量</param>
            /// <returns></returns>
            System.Threading.Tasks.Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponse<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallExceptionStatistics>> GetManyStatistics(int count);
            /// <summary>
            /// 获取调用异常统计信息
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            /// <returns>异常统计信息，失败返回 null</returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics> GetStatistics(string callType, string callName);
            /// <summary>
            /// 获取异常统计信息数量
            /// </summary>
            /// <returns></returns>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameterAwaiter<int> GetStatisticsCount();
            /// <summary>
            /// 移除异常统计信息
            /// </summary>
            /// <param name="callType">调用接口类型</param>
            /// <param name="callName">调用接口方法名称</param>
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResultAwaiter Remove(string callType, string callName);
        }
}namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
        /// <summary>
        /// 异常调用统计信息节点接口
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodIndex(typeof(IExceptionStatisticsNodeMethodEnum))]
        public partial interface IExceptionStatisticsNode { }
        /// <summary>
        /// 异常调用统计信息节点接口 节点方法序号映射枚举类型
        /// </summary>
        public enum IExceptionStatisticsNodeMethodEnum
        {
            /// <summary>
            /// [0] 添加异常调用时间
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// System.DateTime callTime 异常调用时间
            /// </summary>
            Append = 0,
            /// <summary>
            /// [1] 获取异常调用总次数
            /// 返回值 long 
            /// </summary>
            GetCount = 1,
            /// <summary>
            /// [2] 获取指定数量调用异常统计信息
            /// int count 获取调用异常统计信息数量
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.CallExceptionStatistics 
            /// </summary>
            GetManyStatistics = 2,
            /// <summary>
            /// [3] 获取调用异常统计信息
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// 返回值 AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics 异常统计信息，失败返回 null
            /// </summary>
            GetStatistics = 3,
            /// <summary>
            /// [4] 获取异常统计信息数量
            /// 返回值 int 
            /// </summary>
            GetStatisticsCount = 4,
            /// <summary>
            /// [5] 快照设置数据
            /// AutoCSer.BinarySerializeKeyValue{long,AutoCSer.CommandService.InterfaceRealTimeCallMonitor.ExceptionStatistics} value 数据
            /// </summary>
            SnapshotSet = 5,
            /// <summary>
            /// [6] 快照设置数据
            /// AutoCSer.LeftArray{string} stringArray 数据
            /// </summary>
            SnapshotSetStringArray = 6,
            /// <summary>
            /// [7] 移除异常统计信息
            /// string callType 调用接口类型
            /// string callName 调用接口方法名称
            /// </summary>
            Remove = 7,
        }
}
#endif