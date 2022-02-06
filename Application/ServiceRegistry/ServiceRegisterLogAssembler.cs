using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册日志组装
    /// </summary>
    internal class ServiceRegisterLogAssembler
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        internal readonly ServiceRegistry ServiceRegistry;
        /// <summary>
        /// 主日志
        /// </summary>
        internal ServiceRegistrySessionLog MainLog;
        /// <summary>
        /// 附加日志
        /// </summary>
        private LeftArray<ServiceRegistrySessionLog> logs;
        /// <summary>
        /// 注册服务最大版本号
        /// </summary>
        private uint maxVersion;
        /// <summary>
        /// 服务会话集合
        /// </summary>
        private readonly Dictionary<long, ServiceRegistrySessionCallback> callbacks = DictionaryCreator.CreateLong<ServiceRegistrySessionCallback>();
        /// <summary>
        /// 服务注册日志组装
        /// </summary>
        /// <param name="serviceRegistry"></param>
        internal ServiceRegisterLogAssembler(ServiceRegistry serviceRegistry)
        {
            ServiceRegistry = serviceRegistry;
            logs = new LeftArray<ServiceRegistrySessionLog>(0);
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
            ServiceRegisterOperationType operationType = log.OperationType;
            switch (operationType)
            {
                case ServiceRegisterOperationType.ClusterNode:
                case ServiceRegisterOperationType.ClusterMain:
                case ServiceRegisterOperationType.Singleton:
                    uint versioin = log.Version;
                    if (versioin < maxVersion) return new ServiceRegisterResponse(ServiceRegisterState.VersionTooLow);
                    if (versioin != maxVersion)
                    {
                        logs.Clear();
                        MainLog = null;
                        maxVersion = versioin;
                    }
                    if (MainLog == null)
                    {
                        ServiceRegistrySession session = ServiceRegistry.GetSession(socket);
                        MainLog = new ServiceRegistrySessionLog(session, log);
                        ServiceRegistry.Callback(callbacks, log);
                        session.Regiser(this);
                        return new ServiceRegisterResponse(log.ServiceID);
                    }
                    checkSessionDropped();
                    if (MainLog == null)
                    {
                        ServiceRegistrySession session = ServiceRegistry.GetSession(socket);
                        MainLog = new ServiceRegistrySessionLog(session, log);
                        ServiceRegistry.Callback(callbacks, log);
                        session.Regiser(this);
                        return new ServiceRegisterResponse(log.ServiceID);
                    }
                    switch (operationType)
                    {
                        case ServiceRegisterOperationType.ClusterNode:
                        case ServiceRegisterOperationType.ClusterMain:
                            switch (MainLog.Log.OperationType)
                            {
                                case ServiceRegisterOperationType.ClusterNode:
                                case ServiceRegisterOperationType.ClusterMain:
                                    break;
                                default: return new ServiceRegisterResponse(ServiceRegisterState.OperationTypeConflict);
                            }
                            ServiceRegistrySession session = ServiceRegistry.GetSession(socket);
                            ServiceRegistrySessionLog sessionLog = new ServiceRegistrySessionLog(session, log);
                            if (operationType == ServiceRegisterOperationType.ClusterNode) logs.Add(sessionLog);
                            else
                            {
                                logs.Add(MainLog);
                                MainLog = sessionLog;
                            }
                            ServiceRegistry.Callback(callbacks, log);
                            session.Regiser(this);
                            return new ServiceRegisterResponse(log.ServiceID);
                        default:
                            if (MainLog.Log.OperationType != ServiceRegisterOperationType.Singleton) return new ServiceRegisterResponse(ServiceRegisterState.OperationTypeConflict);
                            while (logs.TryPop(out ServiceRegistrySessionLog removeLog)) ;
                            session = ServiceRegistry.GetSession(socket);
                            logs.Add(new ServiceRegistrySessionLog(session, log));
                            session.Regiser(this);
                            ServiceRegistry.Callback(callbacks, MainLog.Log.CreateOffline());
                            AutoCSer.Threading.SecondTimer.TaskArray.Append(new ServiceRegisterLogAssemblerQueueNode(this).AppendQueueNode, (int)log.TimeoutSeconds + 1);
                            return new ServiceRegisterResponse(log.ServiceID);
                    }
                case ServiceRegisterOperationType.Logout: return logout(log);
                case ServiceRegisterOperationType.Clear:
                    logs.Clear();
                    MainLog = null;
                    ServiceRegistry.Callback(callbacks, log);
                    return new ServiceRegisterResponse(ServiceRegisterState.Success);
                default: return new ServiceRegisterResponse(ServiceRegisterState.UnrecognizableOperationType);
            }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private ServiceRegisterResponse logout(ServiceRegisterLog log)
        {
            if (MainLog == null) return new ServiceRegisterResponse(ServiceRegisterState.NotFoundServiceID);
            long serviceID = log.ServiceID;
            if (serviceID == 0) return new ServiceRegisterResponse(ServiceRegisterState.NotFoundServiceID);
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
            foreach (ServiceRegistrySessionLog nextLog in logs)
            {
                if (nextLog.Log.ServiceID == serviceID)
                {
                    logs.RemoveAtToEnd(logIndex);
                    ServiceRegistry.Callback(callbacks, log);
                    return new ServiceRegisterResponse(serviceID);
                }
                ++logIndex;
            }
            return new ServiceRegisterResponse(ServiceRegisterState.NotFoundServiceID);
        }
        /// <summary>
        /// 单例主服务超时下线
        /// </summary>
        internal void SingletonTimeout()
        {
            if (!logs.TryPop(out ServiceRegistrySessionLog newMainLog) || newMainLog.Session.CheckDropped()) return;
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
                if (MainLog.Log.OperationType == ServiceRegisterOperationType.Singleton) logs.RemoveAllToEnd(ServiceRegistrySessionLog.CheckSessionDropped);
                else
                {
                    foreach (ServiceRegistrySessionLog droppedLog in logs.GetRemoveAllToEnd(ServiceRegistrySessionLog.CheckSessionDropped))
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
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
                if (MainLog.Log.OperationType != ServiceRegisterOperationType.Singleton)
                {
                    foreach (ServiceRegistrySessionLog log in logs)
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
            ServiceRegistrySession session = ServiceRegistry.GetSession(socket);
            callbacks[session.SessionID] = new ServiceRegistrySessionCallback(session, callback);
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
                if (MainLog.Log.OperationType != ServiceRegisterOperationType.Singleton)
                {
                    foreach (ServiceRegistrySessionLog log in logs)
                    {
                        if (!callback.Callback(log.Log)) return false;
                    }
                }
            }
            return true;
        }
    }
}
