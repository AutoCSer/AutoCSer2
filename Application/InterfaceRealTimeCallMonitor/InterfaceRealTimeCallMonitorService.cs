using AutoCSer.CommandService.InterfaceRealTimeCallMonitor;
using AutoCSer.Diagnostics;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 接口实时调用监视服务
    /// </summary>
    public class InterfaceRealTimeCallMonitorService : AutoCSer.Threading.SecondTimerTaskArrayNode, IInterfaceRealTimeCallMonitorService, ICommandServerBindController, IDisposable
    {
        /// <summary>
        /// 会话对象操作接口
        /// </summary>
        private readonly ICommandListenerSession<IInterfaceMonitorSession> socketSessionObject;
        /// <summary>
        /// 服务端时间戳
        /// </summary>
        public readonly ServerTimestamp ServerTimestamp;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private CommandServerCallQueue commandServerCallQueue;
        /// <summary>
        /// 实时调用信息集合
        /// </summary>
        private readonly Dictionary<CallIdentity, CallInfo> calls;
        /// <summary>
        /// 超时调用回调委托集合
        /// </summary>
        private CommandServerKeepCallback<CallTimestamp>.Link timeoutCallbacks;
        /// <summary>
        /// 异常调用回调委托集合
        /// </summary>
        private CommandServerKeepCallback<CallTimestamp>.Link exceptionCallbacks;
        /// <summary>
        /// 待删除实时调用标识
        /// </summary>
        private LeftArray<CallIdentity> removeIdentitys;
        /// <summary>
        /// 当前分配实时调用监视标识
        /// </summary>
        internal long CurrentMonitorIdentity;
        /// <summary>
        /// 是否正在检查超时
        /// </summary>
        private int isCheckTimeout;
        /// <summary>
        /// 接口实时调用监视服务
        /// </summary>
        /// <param name="listener">SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession[AutoCSer.CommandService.InterfaceRealTimeCallMonitor.IInterfaceMonitorSession]</param>
        /// <param name="checkTimeoutSeconds">超时检查执行间隔秒数</param>
        public InterfaceRealTimeCallMonitorService(CommandListener listener, int checkTimeoutSeconds = 5) : base(AutoCSer.Threading.SecondTimer.TaskArray, Math.Max(checkTimeoutSeconds, 1), Threading.SecondTimerTaskThreadModeEnum.Synchronous, Threading.SecondTimerKeepModeEnum.Before, Math.Max(checkTimeoutSeconds, 1))
        {
            ServerTimestamp = new ServerTimestamp(AutoCSer.Date.TimestampPerSecond);
            CurrentMonitorIdentity = DateTime.UtcNow.Ticks;
            removeIdentitys.SetEmpty();
            calls = DictionaryCreator<CallIdentity>.Create<CallInfo>();
            socketSessionObject = listener.GetSessionObject<ICommandListenerSession<IInterfaceMonitorSession>>() ?? CommandListenerSession.Default;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            KeepSeconds = 0;
            timeoutCallbacks.CancelKeep();
            exceptionCallbacks.CancelKeep();
            foreach (CallInfo call in calls.Values) call.InterfaceMonitor.CancelCallback();
        }
        /// <summary>
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void ICommandServerBindController.Bind(CommandServerController controller)
        {
            commandServerCallQueue = controller.CallQueue.notNull();
            AppendTaskArray();
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal override void OnTimer()
        {
            if (Interlocked.CompareExchange(ref isCheckTimeout, 1, 0) == 0) commandServerCallQueue.AddOnly(new TimeoutCallback(this));
        }
        /// <summary>
        /// 超时检查回调
        /// </summary>
        internal void CheckTimeout()
        {
            try
            {
                getTimeoutCount();
            }
            finally { Interlocked.Exchange(ref isCheckTimeout, 0); }
        }
        /// <summary>
        /// 获取未完成调用数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return calls.Count;
        }
        /// <summary>
        /// 获取套接字绑定的实时调用监视信息
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>实时调用监视信息</returns>
        private InterfaceMonitor getInterfaceMonitor(CommandServerSocket socket)
        {
            var session = socketSessionObject.TryGetSessionObject(socket);
            if (session == null) session = socketSessionObject.CreateSessionObject(socket);
            var interfaceMonitor = session.InterfaceMonitor;
            if (interfaceMonitor == null) session.InterfaceMonitor = interfaceMonitor = new InterfaceMonitor(this);
            return interfaceMonitor;
        }
        /// <summary>
        /// 接口监视服务在线检查回调
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">在线检查回调</param>
        public virtual void Check(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback callback)
        {
            var cancelCallback = callback;
            try
            {
                cancelCallback = getInterfaceMonitor(socket).Set(callback);
            }
            finally { cancelCallback?.CancelKeep(); }
        }
        /// <summary>
        /// 获取超时调用数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">实时调用时间戳信息回调</param>
        public void GetTimeout(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<CallTimestamp> callback)
        {
            timeoutCallbacks.PushHead(callback);
        }
        /// <summary>
        /// 获取异常调用数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">实时调用时间戳信息回调</param>
        public void GetException(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback<CallTimestamp> callback)
        {
            exceptionCallbacks.PushHead(callback);
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
        public virtual CommandServerSendOnly Start(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, string callType, string callName, int timeoutMilliseconds, ushort type)
        {
            InterfaceMonitor interfaceMonitor = getInterfaceMonitor(socket);
            calls.Add(new CallIdentity(interfaceMonitor.Identity, callIdentity), new CallInfo(interfaceMonitor, new CallData(callType, callName, timeoutMilliseconds, type)));
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// 调用完成
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="isException">接口是否执行异常</param>
        /// <returns></returns>
        public virtual CommandServerSendOnly Completed(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, bool isException)
        {
            CallInfo call;
            InterfaceMonitor interfaceMonitor = getInterfaceMonitor(socket);
            if (calls.Remove(new CallIdentity(interfaceMonitor.Identity, callIdentity), out call))
            {
                CallData callData = call.CallData;
                if (callData.Completed(isException)) timeout(callData);
                if (isException) exception(callData);
            }
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// 设置自定义调用步骤
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callIdentity">调用标识</param>
        /// <param name="step">自定义调用步骤</param>
        /// <returns></returns>
        public virtual CommandServerSendOnly SetStep(CommandServerSocket socket, CommandServerCallQueue queue, long callIdentity, int step)
        {
            CallInfo call;
            InterfaceMonitor interfaceMonitor = getInterfaceMonitor(socket);
            if (calls.TryGetValue(new CallIdentity(interfaceMonitor.Identity, callIdentity), out call))
            {
                CallData callData = call.CallData;
                if (callData.SetStep(step)) timeout(callData);
            }
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// 调用超时处理
        /// </summary>
        /// <param name="callData">实时调用信息序列化数据</param>
        protected virtual void timeout(CallData callData)
        {
            timeoutCallbacks.Callback(new CallTimestamp(this, callData));
        }
        /// <summary>
        /// 调用异常处理
        /// </summary>
        /// <param name="callData">实时调用信息序列化数据</param>
        protected virtual void exception(CallData callData)
        {
            exceptionCallbacks.Callback(new CallTimestamp(this, callData));
        }
        /// <summary>
        /// 获取超时调用数量
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns>超时调用数量</returns>
        public int GetTimeoutCount(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            return getTimeoutCount();
        }
        /// <summary>
        /// 获取超时调用数量
        /// </summary>
        /// <returns>超时调用数量</returns>
        private int getTimeoutCount()
        {
            int count = 0;
            long timestamp = Stopwatch.GetTimestamp();
            foreach (KeyValuePair<CallIdentity, CallInfo> call in calls)
            {
                if (checkTimeout(call.Value, call.Key, timestamp)) ++count;
            }
            removeTimeout();
            return count;
        }
        /// <summary>
        /// 调用超时检查
        /// </summary>
        /// <param name="call"></param>
        /// <param name="callIdentity"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private bool checkTimeout(CallInfo call, CallIdentity callIdentity, long timestamp)
        {
            CallData callData = call.CallData;
            if (callData.CheckTimeout(timestamp))
            {
                if (call.InterfaceMonitor.IsCallback())
                {
                    if (callData.CheckTimeout()) timeout(callData);
                }
                else removeIdentitys.Add(callIdentity);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取指定数量的超时调用
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="count">获取数量</param>
        /// <param name="callback">超时调用回调</param>
        public void GetTimeoutCalls(CommandServerSocket socket, CommandServerCallQueue queue, int count, CommandServerKeepCallback<CallTimestamp> callback)
        {
            long timestamp = Stopwatch.GetTimestamp();
            foreach (KeyValuePair<CallIdentity, CallInfo> call in calls)
            {
                if (checkTimeout(call.Value, call.Key, timestamp))
                {
                    if (!callback.VirtualCallback(new CallTimestamp(this, call.Value.CallData)) || --count <= 0) return;
                }
            }
            removeTimeout();
        }
        /// <summary>
        /// 删除不在线的超时调用
        /// </summary>
        private void removeTimeout()
        {
            foreach (CallIdentity identity in removeIdentitys) calls.Remove(identity);
            removeIdentitys.Length = 0;
        }
    }
}
