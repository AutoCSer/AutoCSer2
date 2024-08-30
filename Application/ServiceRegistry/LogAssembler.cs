using AutoCSer.CommandService.ServiceRegistry;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.ServiceRegister
{
    /// <summary>
    /// 服务注册日志组装
    /// </summary>
    internal class LogAssembler
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        internal readonly ServiceRegistryService ServiceRegistry;
        /// <summary>
        /// 主日志
        /// </summary>
        internal SessionLog MainLog;
        /// <summary>
        /// 附加日志
        /// </summary>
        private LeftArray<SessionLog> logs;
        /// <summary>
        /// 注册服务最大版本号
        /// </summary>
        private uint maxVersion;
        /// <summary>
        /// 服务会话集合
        /// </summary>
        private readonly Dictionary<long, SessionCallback> callbacks = AutoCSer.Extensions.DictionaryCreator.CreateLong<SessionCallback>();
        /// <summary>
        /// 服务注册日志组装
        /// </summary>
        /// <param name="serviceRegistry"></param>
        internal LogAssembler(ServiceRegistryService serviceRegistry)
        {
            ServiceRegistry = serviceRegistry;
            logs = new LeftArray<SessionLog>(0);
        }
        ///// <summary>
        ///// 根据服务标识ID获取日志位置，-1 表示主服务
        ///// </summary>
        ///// <param name="serviceID"></param>
        ///// <returns></returns>
        //protected int getLogIndex(long serviceID)
        //{
        //    if (serviceID == 0 || mainLog == null) return -2;
        //    if (mainLog.Log.ServiceID == serviceID) return -1;
        //    int logIndex = 0;
        //    foreach (ServiceRegistrySessionLog log in logs)
        //    {
        //        if (log.Log.ServiceID == serviceID) return logIndex;
        //        ++logIndex;
        //    }
        //    return -2;
        //}
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        internal ServiceRegisterResponse Append(CommandServerSocket socket, ServiceRegisterLog log)
        {
            ServiceRegisterOperationTypeEnum operationType = log.OperationType;
            switch (operationType)
            {
                case ServiceRegisterOperationTypeEnum.ClusterNode:
                case ServiceRegisterOperationTypeEnum.ClusterMain:
                case ServiceRegisterOperationTypeEnum.Singleton:
                    uint versioin = log.Version;
                    if (versioin < maxVersion) return new ServiceRegisterResponse(ServiceRegisterStateEnum.VersionTooLow);
                    if (versioin != maxVersion)
                    {
                        logs.ClearLength();
                        MainLog = null;
                        maxVersion = versioin;
                    }
                    if (MainLog == null)
                    {
                        ServiceRegisterSession session = ServiceRegistry.GetOrCreateSession(socket);
                        MainLog = new SessionLog(session, log);
                        ServiceRegistry.Callback(callbacks, log);
                        session.Regiser(this);
                        return new ServiceRegisterResponse(log.ServiceID);
                    }
                    checkSessionDropped();
                    if (MainLog == null)
                    {
                        ServiceRegisterSession session = ServiceRegistry.GetOrCreateSession(socket);
                        MainLog = new SessionLog(session, log);
                        ServiceRegistry.Callback(callbacks, log);
                        session.Regiser(this);
                        return new ServiceRegisterResponse(log.ServiceID);
                    }
                    switch (operationType)
                    {
                        case ServiceRegisterOperationTypeEnum.ClusterNode:
                        case ServiceRegisterOperationTypeEnum.ClusterMain:
                            switch (MainLog.Log.OperationType)
                            {
                                case ServiceRegisterOperationTypeEnum.ClusterNode:
                                case ServiceRegisterOperationTypeEnum.ClusterMain:
                                    break;
                                default: return new ServiceRegisterResponse(ServiceRegisterStateEnum.OperationTypeConflict);
                            }
                            ServiceRegisterSession session = ServiceRegistry.GetOrCreateSession(socket);
                            SessionLog sessionLog = new SessionLog(session, log);
                            if (operationType == ServiceRegisterOperationTypeEnum.ClusterNode) logs.Add(sessionLog);
                            else
                            {
                                logs.Add(MainLog);
                                MainLog = sessionLog;
                            }
                            ServiceRegistry.Callback(callbacks, log);
                            session.Regiser(this);
                            return new ServiceRegisterResponse(log.ServiceID);
                        default:
                            if (MainLog.Log.OperationType != ServiceRegisterOperationTypeEnum.Singleton) return new ServiceRegisterResponse(ServiceRegisterStateEnum.OperationTypeConflict);
                            SessionLog removeLog;
                            while (logs.TryPop(out removeLog)) ;
                            session = ServiceRegistry.GetOrCreateSession(socket);
                            logs.Add(new SessionLog(session, log));
                            session.Regiser(this);
                            ServiceRegistry.Callback(callbacks, MainLog.Log.CreateOffline());
                            AutoCSer.Threading.SecondTimer.TaskArray.Append(new LogAssemblerQueueNode(this).AppendQueueNode, (int)log.TimeoutSeconds + 1);
                            return new ServiceRegisterResponse(log.ServiceID);
                    }
                case ServiceRegisterOperationTypeEnum.Logout: return logout(log);
                case ServiceRegisterOperationTypeEnum.Clear:
                    logs.ClearLength();
                    MainLog = null;
                    ServiceRegistry.Callback(callbacks, log);
                    return new ServiceRegisterResponse(ServiceRegisterStateEnum.Success);
                default: return new ServiceRegisterResponse(ServiceRegisterStateEnum.UnrecognizableOperationType);
            }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private ServiceRegisterResponse logout(ServiceRegisterLog log)
        {
            if (MainLog == null) return new ServiceRegisterResponse(ServiceRegisterStateEnum.NotFoundServiceID);
            long serviceID = log.ServiceID;
            if (serviceID == 0) return new ServiceRegisterResponse(ServiceRegisterStateEnum.NotFoundServiceID);
            if (MainLog.Log.ServiceID == serviceID)
            {
                ServiceRegistry.Callback(callbacks, log);
                while (logs.TryPop(out MainLog) && MainLog.Session.CheckDropped())
                {
                    ServiceRegistry.Callback(callbacks, MainLog.Log.CreateLostContact());
                }
                return new ServiceRegisterResponse(serviceID);
            }
            int logIndex = 0;
            foreach (SessionLog nextLog in logs)
            {
                if (nextLog.Log.ServiceID == serviceID)
                {
                    logs.RemoveAtToEnd(logIndex);
                    ServiceRegistry.Callback(callbacks, log);
                    return new ServiceRegisterResponse(serviceID);
                }
                ++logIndex;
            }
            return new ServiceRegisterResponse(ServiceRegisterStateEnum.NotFoundServiceID);
        }
        /// <summary>
        /// 单例主服务超时下线
        /// </summary>
        internal void SingletonTimeout()
        {
            SessionLog newMainLog;
            if (!logs.TryPop(out newMainLog) || newMainLog.Session.CheckDropped()) return;
            MainLog = newMainLog;
            ServiceRegistry.Callback(callbacks, newMainLog.Log);
        }
        /// <summary>
        /// 会话掉线检查
        /// </summary>
        private void checkSessionDropped()
        {
            if (logs.Count != 0)
            {
                if (MainLog.Log.OperationType == ServiceRegisterOperationTypeEnum.Singleton) logs.RemoveAllToEnd(SessionLog.CheckSessionDropped);
                else
                {
                    foreach (SessionLog droppedLog in logs.GetRemoveAllToEnd(SessionLog.CheckSessionDropped))
                    {
                        ServiceRegistry.Callback(callbacks, droppedLog.Log.CreateLostContact());
                    }
                }
            }
            if (!MainLog.Session.CheckDropped()) return;
            ServiceRegistry.Callback(callbacks, MainLog.Log.CreateLostContact());
            logs.TryPop(out MainLog);
        }
        /// <summary>
        /// 服务注册会话掉线
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SessionDropped()
        {
            if (MainLog != null) checkSessionDropped();
        }
        /// <summary>
        /// 添加服务注册日志回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callback"></param>
        internal void Append(CommandServerSocket socket, CommandServerKeepCallback<ServiceRegisterLog> callback)
        {
            if (MainLog != null)
            {
                if (!callback.Callback(MainLog.Log))
                {
                    ServiceRegistry.SetDropped(socket);
                    return;
                }
                if (MainLog.Log.OperationType != ServiceRegisterOperationTypeEnum.Singleton)
                {
                    foreach (SessionLog log in logs)
                    {
                        if (!callback.Callback(log.Log))
                        {
                            ServiceRegistry.SetDropped(socket);
                            return;
                        }
                    }
                }
            }
            if (!callback.Callback((ServiceRegisterLog)null))
            {
                ServiceRegistry.SetDropped(socket);
                return;
            }
            ServiceRegisterSession session = ServiceRegistry.GetOrCreateSession(socket);
            callbacks[session.SessionID] = new SessionCallback(session, callback);
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal bool Callback(CommandServerKeepCallback<ServiceRegisterLog> callback)
        {
            if (MainLog != null)
            {
                if (!callback.Callback(MainLog.Log)) return false;
                if (MainLog.Log.OperationType != ServiceRegisterOperationTypeEnum.Singleton)
                {
                    foreach (SessionLog log in logs)
                    {
                        if (!callback.Callback(log.Log)) return false;
                    }
                }
            }
            return true;
        }
    }
}
