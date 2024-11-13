using AutoCSer.CommandService.ServiceRegistry;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.ServiceRegistry
{
    /// <summary>
    /// 服务注册服务会话对象操作
    /// </summary>
    internal sealed class CommandListenerSession : ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService>
    {
        /// <summary>
        /// 最后访问的会话对象
        /// </summary>
#if NetStandard21
        private ServiceRegisterSocketSession? lastSession;
#else
        private ServiceRegisterSocketSession lastSession;
#endif
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        IServiceRegisterSocketSession? ICommandListenerGetSession<IServiceRegisterSocketSession>.TryGetSessionObject(CommandServerSocket socket)
#else
        IServiceRegisterSocketSession ICommandListenerGetSession<IServiceRegisterSocketSession>.TryGetSessionObject(CommandServerSocket socket)
#endif
        {
            var session = this.lastSession;
            if (session != null && object.ReferenceEquals(session.Socket, socket)) return session;
            session = socket.SessionObject.castType<ServiceRegisterSocketSession>();
            if (session != null) lastSession = session;
            return session;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="service">默认服务注册</param>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        IServiceRegisterSocketSession ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService>.CreateSessionObject(ServiceRegistryService service, CommandServerSocket socket)
        {
            ServiceRegisterSocketSession session = new ServiceRegisterSocketSession(service, socket);
            socket.SessionObject = lastSession = session;
            return session;
        }

        /// <summary>
        /// 默认服务注册服务会话对象操作
        /// </summary>
        internal static readonly CommandListenerSession Default = new CommandListenerSession();
    }
}
namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册会话对象操作接口
    /// </summary>
    public interface IServiceRegisterSocketSession
    {
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        ServiceRegisterSocketSession ServiceRegisterSocketSession { get; }
    }
    /// <summary>
    /// 服务注册会话对象
    /// </summary>
    public class ServiceRegisterSocketSession : AutoCSer.Threading.QueueTaskNode, IServiceRegisterSocketSession
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        internal readonly ServiceRegistryService Service;
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        internal readonly CommandServerSocket Socket;
        /// <summary>
        /// 会话ID
        /// </summary>
        internal readonly long SessionID;
        /// <summary>
        /// 命令服务套接字关闭处理委托
        /// </summary>
#if NetStandard21
        private Action? onSocketClosedHandle;
#else
        private Action onSocketClosedHandle;
#endif
        /// <summary>
        /// 服务在线检查回调委托
        /// </summary>
#if NetStandard21
        internal CommandServerKeepCallback? CheckCallback;
#else
        internal CommandServerKeepCallback CheckCallback;
#endif
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
        ServiceRegisterSocketSession IServiceRegisterSocketSession.ServiceRegisterSocketSession { get { return this; } }
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="socket">命令服务套接字</param>
        public ServiceRegisterSocketSession(ServiceRegistryService service, CommandServerSocket socket)
        {
            this.Service = service;
            this.Socket = socket;
            SessionID = service.IdentityGenerator.GetNext();
        }
        /// <summary>
        /// 套接字关闭或者掉线处理
        /// </summary>
        public override void RunTask()
        {
            if (onSocketClosedHandle != null) Socket.RemoveOnClosed(onSocketClosedHandle);
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
                if (!Socket.IsClose)
                {
                    logAssemblers.Add(logAssembler);
                    if (logAssemblers.Count == 1)
                    {
                        if (onSocketClosedHandle == null) onSocketClosedHandle = SetDropped;
                        Socket.SetOnClosed(onSocketClosedHandle);
                    }
                    if (!Socket.IsClose) return;
                }
                SetDropped();
            }
            logAssembler.SessionDropped();
        }
        /// <summary>
        /// 设置掉线
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetDropped()
        {
            if (IsDropped == 0 && Interlocked.CompareExchange(ref IsDropped, 1, 0) == 0) Service.Controller.CallQueue.notNull().AddOnly(this);
        }
        /// <summary>
        /// 设置掉线并返回会话ID
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
