using AutoCSer.CommandService;
using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 接口实时调用监视服务
    /// </summary>
    internal sealed class InterfaceRealTimeCallMonitorService : AutoCSer.CommandService.InterfaceRealTimeCallMonitorService
    {
        /// <summary>
        /// 异常调用统计信息客户端
        /// </summary>
        private ExceptionStatisticsClient exceptionStatisticsClient;
        /// <summary>
        /// 接口实时调用监视服务
        /// </summary>
        /// <param name="listener">SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession[AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IInterfaceMonitorSession]</param>
        public InterfaceRealTimeCallMonitorService(CommandListener listener) : base(listener)
        {
            exceptionStatisticsClient = new ExceptionStatisticsClient();
        }
        /// <summary>
        /// 新增一个实时调用信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <param name="timeoutMilliseconds">超时毫秒数</param>
        /// <param name="type">调用类型</param>
        /// <returns></returns>
        public override CommandServerSendOnly Start(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type)
        {
            Console.WriteLine($"{callIdentity} -> {callType}.{callName}");
            return base.Start(socket, queue, callIdentity, callType, callName, timeoutMilliseconds, type);
        }
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="isException">接口是否执行异常</param>
        /// <returns></returns>
        public override CommandServerSendOnly Completed(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, bool isException)
        {
            Console.WriteLine($"{callIdentity} {nameof(Completed)}");
            return base.Completed(socket, queue, callIdentity, isException);
        }
        /// <summary>
        /// 异常调用统计信息客户端节点
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private ExceptionStatisticsClient getExceptionStatisticsNode(DateTime dateTime)
        {
            uint date = dateTime.toIntDate();
            if (date <= exceptionStatisticsClient.Date) return exceptionStatisticsClient;
            return exceptionStatisticsClient = new ExceptionStatisticsClient(date);
        }
        /// <summary>
        /// 调用超时处理
        /// </summary>
        /// <param name="callData">实时调用信息序列化数据</param>
        protected override void timeout(CallData callData)
        {
            base.timeout(callData);
            DateTime startTime = ServerTimestamp.GetTime(callData.StartTimestamp);
            getExceptionStatisticsNode(startTime).AppendTimeoutNode(callData, startTime);
        }
        /// <summary>
        /// 调用异常处理
        /// </summary>
        /// <param name="callData">实时调用信息序列化数据</param>
        protected override void exception(CallData callData)
        {
            base.exception(callData);
            DateTime startTime = ServerTimestamp.GetTime(callData.StartTimestamp);
            getExceptionStatisticsNode(startTime).AppendException(callData, startTime);
        }
    }
}
