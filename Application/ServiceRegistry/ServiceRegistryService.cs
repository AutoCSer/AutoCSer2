using AutoCSer.CommandService.ServiceRegistry;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 默认服务注册
    /// </summary>
    [CommandServerController(InterfaceType = typeof(IServiceRegistryService))]
    public class ServiceRegistryService : CommandServerBindController, IServiceRegistryService
    {
        /// <summary>
        /// 服务注册会话对象操作接口
        /// </summary>
        private readonly ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService> socketSessionObject;
        /// <summary>
        /// 命令服务
        /// </summary>
        private  readonly CommandListener listener;
        /// <summary>
        /// 服务标识生成器
        /// </summary>
        internal readonly DistributedMillisecondIdentityGenerator IdentityGenerator;
        /// <summary>
        /// 服务名称集合
        /// </summary>
        private readonly Dictionary<string, LogAssembler> services = DictionaryCreator.CreateAny<string, LogAssembler>();
        /// <summary>
        /// 获取服务注册日志回调集合
        /// </summary>
        private readonly Dictionary<long, SessionCallback> callbacks = AutoCSer.Extensions.DictionaryCreator.CreateLong<SessionCallback>();
        /// <summary>
        /// 需要移除的服务会话集合
        /// </summary>
        private static LeftArray<long> removeCallbackSessionID = new LeftArray<long>(0);
        /// <summary>
        /// 默认服务注册
        /// </summary>
        /// <param name="listener">SessionObject 必须实现 AutoCSer.Net.ICommandListenerSession[AutoCSer.CommandService.IServiceRegisterSocketSession, AutoCSer.CommandService.ServiceRegistryService]</param>
        /// <param name="identityGenerator">服务标识生成器</param>
#if NetStandard21
        public ServiceRegistryService(CommandListener listener, DistributedMillisecondIdentityGenerator? identityGenerator = null)
#else
        public ServiceRegistryService(CommandListener listener, DistributedMillisecondIdentityGenerator identityGenerator = null)
#endif
        {
            this.listener = listener;
            IdentityGenerator = identityGenerator
             ?? AutoCSer.Configuration.Common.Get<DistributedMillisecondIdentityGenerator>()?.Value
             ?? new DistributedMillisecondIdentityGenerator();
            socketSessionObject = listener.GetSessionObject<ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService>>() ?? AutoCSer.CommandService.ServiceRegistry.CommandListenerSession.Default;
        }
        /// <summary>
        /// 获取当前套接字连接会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServiceRegisterSocketSession GetOrCreateSession(CommandServerSocket socket)
        {
            IServiceRegisterSocketSession session = socketSessionObject.TryGetSessionObject(socket) ?? socketSessionObject.CreateSessionObject(this, socket);
            return session.ServiceRegisterSocketSession;
        }
        /// <summary>
        /// 设置会话掉线
        /// </summary>
        /// <param name="socket"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetDropped(CommandServerSocket socket)
        {
            socketSessionObject.TryGetSessionObject(socket)?.ServiceRegisterSocketSession.SetDropped();
        }
        /// <summary>
        /// 设置服务会话在线检查回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">服务会话在线检查回调委托</param>
        public void CheckCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback callback)
        {
            GetOrCreateSession(socket).CheckCallback = callback;
        }
        /// <summary>
        /// 创建服务注册日志组装
        /// </summary>
        /// <returns></returns>
        private LogAssembler createAssembler() { return new LogAssembler(this); }
        /// <summary>
        /// 检查服务名称是否可用
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private bool checkServiceName(string serviceName) { return !string.IsNullOrEmpty(serviceName.ToString()); }
        /// <summary>
        /// 获取服务注册日志组装
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="assembler"></param>
        /// <returns></returns>
#if NetStandard21
        private ServiceRegisterResponse getAssembler(string serviceName, out LogAssembler? assembler)
#else
        private ServiceRegisterResponse getAssembler(string serviceName, out LogAssembler assembler)
#endif
        {
            if (!checkServiceName(serviceName))
            {
                assembler = null;
                return new ServiceRegisterResponse(ServiceRegisterStateEnum.UnsupportedServiceName);
            }
            if (!services.TryGetValue(serviceName, out assembler)) services.Add(serviceName, assembler = createAssembler());
            return new ServiceRegisterResponse(ServiceRegisterStateEnum.Success);
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="log"></param>
        internal void Callback(Dictionary<long, SessionCallback> callbacks, ServiceRegisterLog log)
        {
            removeCallbackSessionID.ClearLength();
            callback(callbacks, log);
            callback(this.callbacks, log);
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="log"></param>
        private void callback(Dictionary<long, SessionCallback> callbacks, ServiceRegisterLog log)
        {
            foreach (SessionCallback callback in callbacks.Values)
            {
                long sessionID = callback.Callback(log);
                if (sessionID != 0) removeCallbackSessionID.Add(sessionID);
            }
            foreach (long sessionID in removeCallbackSessionID.PopAll()) callbacks.Remove(sessionID);
        }
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log"></param>
        /// <returns>服务注册结果</returns>
        public ServiceRegisterResponse Append(CommandServerSocket socket, CommandServerCallQueue queue, ServiceRegisterLog log)
        {
            var assembler = default(LogAssembler);
            ServiceRegisterResponse response = getAssembler(log.ServiceName, out assembler);
            return response.State == ServiceRegisterStateEnum.Success ? assembler.notNull().Append(socket, log) : response;
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
        /// <param name="callback">服务注册日志回调委托，返回 null 表示初始化加载完毕</param>
#if NetStandard21
        public void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, string serviceName, CommandServerKeepCallback<ServiceRegisterLog?> callback)
#else
        public void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, string serviceName, CommandServerKeepCallback<ServiceRegisterLog> callback)
#endif
        {
            if (serviceName != null)
            {
                var assembler = default(LogAssembler);
                ServiceRegisterResponse response = getAssembler(serviceName, out assembler);
                if (response.State != ServiceRegisterStateEnum.Success)
                {
                    callback.CancelKeep(CommandServerCall.GetCustom((byte)response.State));
                    return;
                }
                assembler.notNull().Append(socket, callback);
                return;
            }
            foreach (LogAssembler assembler in services.Values)
            {
#pragma warning disable CS8620
                if (!assembler.Callback(callback))
#pragma warning restore CS8620
                {
                    SetDropped(socket);
                    return;
                }
            }
            if (!callback.Callback(default(ServiceRegisterLog)))
            {
                SetDropped(socket);
                return;
            }
            ServiceRegisterSocketSession session = GetOrCreateSession(socket);
            long sessionID = session.SessionID;
            SessionCallback sessionCallback;
            callbacks.TryGetValue(sessionID, out sessionCallback);
#pragma warning disable CS8620
            callbacks[sessionID] = new SessionCallback(session, callback);
#pragma warning restore CS8620
            sessionCallback.CancelKeep(ServiceRegisterStateEnum.NewLogCallback);
        }
    }
}
