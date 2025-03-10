using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 接口实时调用监视服务接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(MethodIndexEnumType = typeof(InterfaceRealTimeCallMonitorServiceMethodEnum), MethodIndexEnumTypeCodeGeneratorPath = "", IsAutoMethodIndex = false)]
    public interface IInterfaceRealTimeCallMonitorService
    {
        /// <summary>
        /// 获取未完成调用数量
        /// </summary>
        /// <returns></returns>
        int GetCount();
        /// <summary>
        /// 接口监视服务在线检查
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">在线检查回调</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void Check(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback callback);
        /// <summary>
        /// 获取超时调用数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">实时调用时间戳信息回调</param>
        void GetTimeout(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<CallTimestamp> callback);
        /// <summary>
        /// 获取异常调用数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">实时调用时间戳信息回调</param>
        void GetException(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<CallTimestamp> callback);
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
        CommandServerSendOnly Start(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type);
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="isException">接口是否执行异常</param>
        /// <returns></returns>
        CommandServerSendOnly Completed(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, bool isException);
        /// <summary>
        /// 设置自定义调用步骤
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="step">自定义调用步骤</param>
        /// <returns></returns>
        CommandServerSendOnly SetStep(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, int step);
        /// <summary>
        /// 获取超时调用数量
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns>超时调用数量</returns>
        int GetTimeoutCount(CommandServerSocket socket, CommandServerCallQueue queue);
        /// <summary>
        /// 获取指定数量的超时调用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="count">获取数量</param>
        /// <param name="callback">超时调用回调</param>
        void GetTimeoutCalls(CommandServerSocket socket, CommandServerCallQueue queue, int count, CommandServerKeepCallback<CallTimestamp> callback);
    }
}
