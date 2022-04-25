using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.ServiceRegister
{
    /// <summary>
    /// 服务注册会话对象
    /// </summary>
    internal class Session : AutoCSer.Net.CommandServerCallQueueCustomNode
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        internal readonly ServiceRegistryService ServiceRegistry;
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        private readonly CommandServerSocket socket;
        /// <summary>
        /// 会话ID
        /// </summary>
        internal readonly long SessionID;
        /// <summary>
        /// 命令服务套接字关闭处理委托
        /// </summary>
        private Action onSocketClosedHandle;
        /// <summary>
        /// 服务在线检查回调委托
        /// </summary>
        internal CommandServerKeepCallback CheckCallback;
        /// <summary>
        /// 注册服务集合
        /// </summary>
        private LeftArray<LogAssembler> logAssemblers = new LeftArray<LogAssembler>(0);
        /// <summary>
        /// 是否已经掉线
        /// </summary>
        private int IsDropped;
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        /// <param name="serviceRegistry"></param>
        /// <param name="socket">命令服务套接字</param>
        /// <param name="sessionID">会话ID</param>
        internal Session(ServiceRegistryService serviceRegistry, CommandServerSocket socket, long sessionID)
        {
            this.ServiceRegistry = serviceRegistry;
            this.socket = socket;
            SessionID = sessionID;
        }
        /// <summary>
        /// 套接字关闭或者掉线处理
        /// </summary>
        public override void RunTask()
        {
            if (onSocketClosedHandle != null) socket.OnClosed -= onSocketClosedHandle;
            foreach (LogAssembler logAssembler in logAssemblers.PopAll()) logAssembler.SessionDropped();
        }
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="logAssembler"></param>
        internal void Regiser(LogAssembler logAssembler)
        {
            if (IsDropped == 0)
            {
                if (logAssemblers.IndexOf(logAssembler) >= 0) return;
                if (!socket.IsClose)
                {
                    logAssemblers.Add(logAssembler);
                    if (logAssemblers.Count == 1)
                    {
                        if (onSocketClosedHandle == null) onSocketClosedHandle = SetDropped;
                        socket.SetOnClosed(onSocketClosedHandle);
                    }
                    if (!socket.IsClose) return;
                }
                SetDropped();
            }
            logAssembler.SessionDropped();
        }
        /// <summary>
        /// 设置掉线
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDropped()
        {
            if (IsDropped == 0 && Interlocked.CompareExchange(ref IsDropped, 1, 0) == 0) ServiceRegistry.Controller.AddQueue(this);
        }
        /// <summary>
        /// 设置掉线并返回会话ID
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal long SetDroppedSessionID()
        {
            SetDropped();
            return SessionID;
        }
        /// <summary>
        /// 检查是否掉线
        /// </summary>
        /// <returns></returns>
        internal bool CheckDropped()
        {
            if (IsDropped == 0)
            {
                if (CheckCallback != null && CheckCallback.Callback()) return false;
                SetDropped();
            }
            return true;
        }
    }
}
