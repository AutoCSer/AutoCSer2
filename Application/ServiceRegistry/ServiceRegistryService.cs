using AutoCSer.CommandService.ServiceRegister;
using AutoCSer.CommandService.ServiceRegistry;
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
    public class ServiceRegistryService : CommandServerBindController, IServiceRegistryService
    {
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
        private readonly Dictionary<HashString, LogAssembler> services = DictionaryCreator.CreateHashString<LogAssembler>();
        /// <summary>
        /// 获取服务注册日志回调集合
        /// </summary>
        private readonly Dictionary<long, SessionCallback> callbacks = DictionaryCreator.CreateLong<SessionCallback>();
        /// <summary>
        /// 需要移除的服务会话集合
        /// </summary>
        private static LeftArray<long> removeCallbackSessionID = new LeftArray<long>(0);
        /// <summary>
        /// 默认服务注册
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="identityGenerator">服务标识生成器</param>
        public ServiceRegistryService(CommandListener listener, DistributedMillisecondIdentityGenerator identityGenerator = null)
        {
            this.listener = listener;
            IdentityGenerator = identityGenerator
             ?? ((ConfigObject<DistributedMillisecondIdentityGenerator>)AutoCSer.Configuration.Common.Get(typeof(DistributedMillisecondIdentityGenerator)))?.Value
             ?? new DistributedMillisecondIdentityGenerator();
        }
        /// <summary>
        /// 获取当前套接字连接会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal Session GetSession(CommandServerSocket socket)
        {
            object sessionObject = socket.SessionObject;
            if (sessionObject != null) return (Session)sessionObject;

            Session session = new Session(this, socket, IdentityGenerator.GetNext());
            socket.SessionObject = session;
            return session;
        }
        /// <summary>
        /// 设置会话掉线
        /// </summary>
        /// <param name="socket"></param>
        internal void SetDropped(CommandServerSocket socket)
        {
            object sessionObject = socket.SessionObject;
            if (sessionObject != null) ((Session)sessionObject).SetDropped();
        }
        /// <summary>
        /// 设置服务会话在线检查回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">服务会话在线检查回调委托</param>
        public void CheckCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback callback)
        {
            GetSession(socket).CheckCallback = callback;
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
        private bool checkServiceName(ref HashString serviceName) { return !string.IsNullOrEmpty(serviceName.ToString()); }
        /// <summary>
        /// 获取服务注册日志组装
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="assembler"></param>
        /// <returns></returns>
        private ServiceRegisterResponse getAssembler(string serviceName, out LogAssembler assembler)
        {
            HashString serviceNameHashString = serviceName;
            if (!checkServiceName(ref serviceNameHashString))
            {
                assembler = null;
                return new ServiceRegisterResponse(ServiceRegisterState.UnsupportedServiceName);
            }
            if (!services.TryGetValue(serviceNameHashString, out assembler)) services.Add(serviceNameHashString, assembler = createAssembler());
            return new ServiceRegisterResponse(ServiceRegisterState.Success);
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="log"></param>
        internal void Callback(Dictionary<long, SessionCallback> callbacks, ServiceRegisterLog log)
        {
            removeCallbackSessionID.Clear();
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
            LogAssembler assembler;
            ServiceRegisterResponse response = getAssembler(log.ServiceName, out assembler);
            return response.State == ServiceRegisterState.Success ? assembler.Append(socket, log) : response;
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
        /// <param name="callback">服务注册日志回调委托</param>
        public void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, string serviceName, CommandServerKeepCallback<ServiceRegisterLog> callback)
        {
            if (serviceName != null)
            {
                LogAssembler assembler;
                ServiceRegisterResponse response = getAssembler(serviceName, out assembler);
                if (response.State != ServiceRegisterState.Success)
                {
                    callback.CancelKeep(CommandServerCall.GetCustom((byte)response.State));
                    return;
                }
                assembler.Append(socket, callback);
                return;
            }
            foreach (LogAssembler assembler in services.Values)
            {
                if (!assembler.Callback(callback))
                {
                    SetDropped(socket);
                    return;
                }
            }
            if (!callback.Callback((ServiceRegisterLog)null))
            {
                SetDropped(socket);
                return;
            }
            Session session = GetSession(socket);
            long sessionID = session.SessionID;
            SessionCallback sessionCallback;
            callbacks.TryGetValue(sessionID, out sessionCallback);
            callbacks[sessionID] = new SessionCallback(session, callback);
            sessionCallback.CancelKeep(ServiceRegisterState.NewLogCallback);
        }
    }
}
