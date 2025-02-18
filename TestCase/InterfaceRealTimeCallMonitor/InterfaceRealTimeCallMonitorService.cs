using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 接口实时调用监视服务
    /// </summary>
    internal sealed class InterfaceRealTimeCallMonitorService : AutoCSer.CommandService.InterfaceRealTimeCallMonitorService
    {
        /// <summary>
        /// 接口实时调用监视服务
        /// </summary>
        /// <param name="listener">SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession[AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IInterfaceMonitorSession]</param>
        public InterfaceRealTimeCallMonitorService(CommandListener listener) : base(listener) { }
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

    }
}
