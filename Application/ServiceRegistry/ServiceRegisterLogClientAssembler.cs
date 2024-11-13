using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册日志客户端组装
    /// </summary>
    public sealed class ServiceRegisterLogClientAssembler
    {
        /// <summary>
        /// 监视服务名称
        /// </summary>
        private readonly string serviceName;
        /// <summary>
        /// 服务注册客户端
        /// </summary>
        private readonly ServiceRegistryClient serviceRegistryClient;
        /// <summary>
        /// 获取服务注册日志保持回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? logKeepCallback;
#else
        private CommandKeepCallback logKeepCallback;
#endif
        /// <summary>
        /// 主日志
        /// </summary>
#if NetStandard21
        private ServiceRegisterLog? mainLog;
#else
        private ServiceRegisterLog mainLog;
#endif
        /// <summary>
        /// 主服务日志
        /// </summary>
#if NetStandard21
        public ServiceRegisterLog? MainLog { get { return mainLog; } }
#else
        public ServiceRegisterLog MainLog { get { return mainLog; } }
#endif
        /// <summary>
        /// 附加日志
        /// </summary>
        private LeftArray<ServiceRegisterLog> logs;
        /// <summary>
        /// 服务日志集合
        /// </summary>
        public IEnumerable<ServiceRegisterLog> Logs
        {
            get
            {
                if (mainLog != null)
                {
                    yield return mainLog;
                    foreach (ServiceRegisterLog log in logs) yield return log;
                }
            }
        }
        /// <summary>
        /// 服务日志数量
        /// </summary>
        public int LogCount
        {
            get { return mainLog != null ? logs.Count + 1 : 0; }
        }
        /// <summary>
        /// 注册服务最大版本号
        /// </summary>
        private uint maxVersion;
        /// <summary>
        /// 日志是否初始化加载完毕
        /// </summary>
        private bool isLoaded;
        ///// <summary>
        ///// 上一个连接的服务注册日志客户端组装
        ///// </summary>
        //private readonly ServiceRegisterLogClientAssembler lastLogAssembler;
        /// <summary>
        /// 服务注册客户端组件集合
        /// </summary>
        private LeftArray<ServiceRegistryCommandClientServiceRegistrar> clientServiceRegistrars;
        /// <summary>
        /// 服务注册客户端组件集合访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock clientServiceRegistrarsLock;
        /// <summary>
        /// 服务注册服务端组件集合
        /// </summary>
        private LeftArray<ServiceRegistryCommandServiceRegistrar> serviceRegistrars;
        /// <summary>
        /// 服务注册服务端组件集合访问锁
        /// </summary>
        private AutoCSer.Threading.SleepFlagSpinLock serviceRegistrarsLock;
        /// <summary>
        /// 服务注册日志客户端组装
        /// </summary>
        /// <param name="serviceRegistryClient">服务注册客户端</param>
        /// <param name="serviceName">监视服务名称</param>
        internal ServiceRegisterLogClientAssembler(ServiceRegistryClient serviceRegistryClient, string serviceName)
        {
            this.serviceRegistryClient = serviceRegistryClient;
            this.serviceName = serviceName;
            serviceRegistrars = new LeftArray<ServiceRegistryCommandServiceRegistrar>(0);
            clientServiceRegistrars = new LeftArray<ServiceRegistryCommandClientServiceRegistrar>(0);
            logs = new LeftArray<ServiceRegisterLog>(0);
        }
        /// <summary>
        /// 服务注册日志客户端组装
        /// </summary>
        /// <param name="lastLogAssembler">上一个连接的服务注册日志客户端组装</param>
        internal ServiceRegisterLogClientAssembler(ServiceRegisterLogClientAssembler lastLogAssembler)
        {
            //this.lastLogAssembler = lastLogAssembler;
            serviceRegistryClient = lastLogAssembler.serviceRegistryClient;
            serviceName = lastLogAssembler.serviceName;
            lastLogAssembler.getServiceRegistrars(this);
            logs = new LeftArray<ServiceRegisterLog>(0);
            foreach (ServiceRegistryCommandServiceRegistrar serviceRegistrar in serviceRegistrars)
            {
                serviceRegistrar.Assembler = this;
            }
            foreach (ServiceRegistryCommandClientServiceRegistrar serviceRegistrar in clientServiceRegistrars)
            {
                serviceRegistrar.Assembler = this;
            }
        }
        /// <summary>
        /// 释放资源并获取服务注册组件集合
        /// </summary>
        /// <param name="newLogAssembler"></param>
        private void getServiceRegistrars(ServiceRegisterLogClientAssembler newLogAssembler)
        {
            logKeepCallback?.Dispose();

            serviceRegistrarsLock.Enter();
            newLogAssembler.serviceRegistrars = serviceRegistrars;
            serviceRegistrars.SetEmpty();
            serviceRegistrarsLock.Exit();

            clientServiceRegistrarsLock.Enter();
            newLogAssembler.clientServiceRegistrars = clientServiceRegistrars;
            clientServiceRegistrars.SetEmpty();
            clientServiceRegistrarsLock.Exit();

            if (mainLog != null && mainLog.OperationType == ServiceRegisterOperationTypeEnum.Singleton) newLogAssembler.mainLog = mainLog;
        }
        /// <summary>
        /// 添加服务注册服务端组件
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        /// <returns></returns>
        internal async Task Append(ServiceRegistryCommandServiceRegistrar serviceRegistrar)
        {
            serviceRegistrarsLock.Enter();
            if (serviceRegistrars.FreeCount == 0)
            {
                serviceRegistrarsLock.SleepFlag = 1;
                try
                {
                    serviceRegistrars.Add(serviceRegistrar);
                }
                finally { serviceRegistrarsLock.ExitSleepFlag(); }
            }
            else
            {
                serviceRegistrars.Add(serviceRegistrar);
                serviceRegistrarsLock.Exit();
            }
            var log = serviceRegistrar.ServiceRegisterLog;
            if (log != null) await append(log);
        }
        /// <summary>
        /// 移除服务注册服务端组件
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        /// <returns></returns>
        public async Task Remove(ServiceRegistryCommandServiceRegistrar serviceRegistrar)
        {
            serviceRegistrarsLock.Enter();
            serviceRegistrars.Remove(serviceRegistrar);
            serviceRegistrarsLock.Exit();

            var log = serviceRegistrar.ServiceRegisterLog;
            if (log != null && log.ServiceID != 0 && serviceRegistryClient.Client != null)
            {
                AutoCSer.Net.CommandClientReturnValue<ServiceRegisterResponse> serviceRegister = await serviceRegistryClient.Client.Append(log.CreateLogout());
                if (serviceRegister.IsSuccess)
                {
                    if (serviceRegister.Value.State != ServiceRegisterStateEnum.Success) await serviceRegistryClient.ServiceRestierLogoutFail(serviceName, serviceRegister.Value.State);
                }
                else await serviceRegistryClient.ServiceRestierLogoutFail(serviceName, serviceRegister.ReturnType);
            }
        }
        /// <summary>
        /// 添加服务注册客户端组件
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        internal void Append(ServiceRegistryCommandClientServiceRegistrar serviceRegistrar)
        {
            clientServiceRegistrarsLock.Enter();
            if (clientServiceRegistrars.FreeCount == 0)
            {
                clientServiceRegistrarsLock.SleepFlag = 1;
                try
                {
                    clientServiceRegistrars.Add(serviceRegistrar);
                }
                finally { clientServiceRegistrarsLock.ExitSleepFlag(); }
            }
            else
            {
                clientServiceRegistrars.Add(serviceRegistrar);
                clientServiceRegistrarsLock.Exit();
            }
        }
        /// <summary>
        /// 移除服务注册客户端组件
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        internal void Remove(ServiceRegistryCommandClientServiceRegistrar serviceRegistrar)
        {
            clientServiceRegistrarsLock.Enter();
            clientServiceRegistrars.Remove(serviceRegistrar);
            clientServiceRegistrarsLock.Exit();
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <returns></returns>
        internal async Task LogCallback()
        {
            if (serviceRegistryClient.Client != null)
            {
                logKeepCallback = await serviceRegistryClient.Client.LogCallback(serviceName, logCallback);
                if (logKeepCallback != null)
                {
                    LeftArray<ServiceRegistryCommandServiceRegistrar> singletonServiceRegistrars = new LeftArray<ServiceRegistryCommandServiceRegistrar>(0);
                    foreach (ServiceRegistryCommandServiceRegistrar serviceRegistrar in serviceRegistrars)
                    {
                        ServiceRegisterLog log = serviceRegistrar.ServiceRegisterLog.notNull();
                        if (log.ServiceID != 0)
                        {
                            if (log.OperationType == ServiceRegisterOperationTypeEnum.Singleton)
                            {
                                if (mainLog != null && log.ServiceID == mainLog.ServiceID)
                                {
                                    await append(log);
                                    singletonServiceRegistrars.SetEmpty();
                                    break;
                                }
                                else singletonServiceRegistrars.Add(serviceRegistrar);
                            }
                            else await append(log);
                        }
                    }
                    foreach (ServiceRegistryCommandServiceRegistrar serviceRegistrar in singletonServiceRegistrars)
                    {
                        if (serviceRegistrar.CheckSingleton()) break;
                    }
                    return;
                }
            }
            await serviceRegistryClient.ServiceRestierAgainLogCallbackFail(serviceName);
        }
        /// <summary>
        /// 单例服务定时尝试上线
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckSingleton(ServiceRegistryCommandServiceRegistrar serviceRegistrar)
        {
            if (mainLog == null) append(serviceRegistrar.ServiceRegisterLog.notNull()).NotWait();
        }
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private async Task append(ServiceRegisterLog log)
        {
            if (serviceRegistryClient.Client != null)
            {
                AutoCSer.Net.CommandClientReturnValue<ServiceRegisterResponse> serviceRegister = await serviceRegistryClient.Client.Append(log);
                if (serviceRegister.IsSuccess)
                {
                    if (serviceRegister.Value.State == ServiceRegisterStateEnum.Success) log.ServiceID = serviceRegister.Value.ServiceID;
                    else await serviceRegistryClient.ServiceRestierAgainFail(serviceName, serviceRegister.Value.State);
                }
                else await serviceRegistryClient.ServiceRestierAgainFail(serviceName, serviceRegister.ReturnType);
            }
        }
        /// <summary>
        /// 获取服务注册日志回调
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="keepCallbackCommand"></param>
#if NetStandard21
        private void logCallback(CommandClientReturnValue<ServiceRegisterLog?> returnValue, KeepCallbackCommand keepCallbackCommand)
#else
        private void logCallback(CommandClientReturnValue<ServiceRegisterLog> returnValue, KeepCallbackCommand keepCallbackCommand)
#endif
        {
            if (returnValue.IsSuccess)
            {
                var log = returnValue.Value;
                if (log != null)
                {
                    ServiceRegisterOperationTypeEnum operationType = log.OperationType;
                    switch (operationType)
                    {
                        case ServiceRegisterOperationTypeEnum.ClusterNode:
                        case ServiceRegisterOperationTypeEnum.ClusterMain:
                        case ServiceRegisterOperationTypeEnum.Singleton:
                            uint versioin = log.Version;
                            if (isLoaded)
                            {
                                ServiceRegisterLogClientChangedTypeEnum changedType = 0;
                                if (versioin != maxVersion)
                                {
                                    if (mainLog != null)
                                    {
                                        changedType |= ServiceRegisterLogClientChangedTypeEnum.Delete;
                                        logs.ClearLength();
                                        mainLog = null;
                                    }
                                    maxVersion = versioin;
                                }
                                if (mainLog != null)
                                {
                                    switch (operationType)
                                    {
                                        case ServiceRegisterOperationTypeEnum.ClusterNode:
                                        case ServiceRegisterOperationTypeEnum.ClusterMain:
                                            if (operationType == ServiceRegisterOperationTypeEnum.ClusterNode)
                                            {
                                                logs.Add(log);
                                                callback(log, ServiceRegisterLogClientChangedTypeEnum.Append);
                                                return;
                                            }
                                            logs.Add(mainLog);
                                            break;
                                    }
                                }
                                callback(mainLog = log, ServiceRegisterLogClientChangedTypeEnum.Main | changedType);
                            }
                            else if (mainLog == null)
                            {
                                mainLog = log;
                                maxVersion = versioin;
                            }
                            else logs.Add(log);
                            return;
                        case ServiceRegisterOperationTypeEnum.Logout:  logout(log); return;
                        case ServiceRegisterOperationTypeEnum.Offline:
                            foreach (ServiceRegistryCommandServiceRegistrar serviceRegistrar in serviceRegistrars)
                            {
                                serviceRegistrar.Offline(log.ServiceID);
                            }
                            return;
                        case ServiceRegisterOperationTypeEnum.LostContact: lostContact(log); return;
                        case ServiceRegisterOperationTypeEnum.Clear:
                            if (mainLog == null) tryCallback(log, 0);
                            else
                            {
                                logs.ClearLength();
                                mainLog = null;
                                tryCallback(log, ServiceRegisterLogClientChangedTypeEnum.Main | ServiceRegisterLogClientChangedTypeEnum.Delete);
                            }
                            return;
                        default: serviceRegistryClient.UnrecognizableOperationType(log); return;
                    }
                }
                else if (!isLoaded)
                {
                    isLoaded = true;
                    if (mainLog != null) callback(null, ServiceRegisterLogClientChangedTypeEnum.Main);
                }
            }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private void logout(ServiceRegisterLog log)
        {
            if (mainLog == null) return;
            long serviceID = log.ServiceID;
            if (mainLog.ServiceID == serviceID)
            {
                logs.TryPop(out mainLog);
                tryCallback(log, ServiceRegisterLogClientChangedTypeEnum.Main | ServiceRegisterLogClientChangedTypeEnum.Delete);
                return;
            }
            int logIndex = 0;
            foreach (ServiceRegisterLog nextLog in logs)
            {
                if (nextLog.ServiceID == serviceID)
                {
                    logs.RemoveAtToEnd(logIndex);
                    tryCallback(log, ServiceRegisterLogClientChangedTypeEnum.Delete);
                    return;
                }
                ++logIndex;
            }
        }
        /// <summary>
        /// 最后失联服务日志
        /// </summary>
#if NetStandard21
        private ServiceRegisterLog? lostContactLog;
#else
        private ServiceRegisterLog lostContactLog;
#endif
        /// <summary>
        /// 服务失联
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private void lostContact(ServiceRegisterLog log)
        {
            long serviceID = log.ServiceID;
            foreach (ServiceRegistryCommandServiceRegistrar serviceRegistrar in serviceRegistrars)
            {
                ServiceRegisterLog serviceRegisterLog = serviceRegistrar.ServiceRegisterLog.notNull();
                if (serviceRegisterLog.ServiceID == serviceID)
                {
                    append(serviceRegisterLog).NotWait();
                    break;
                }
            }
            if (mainLog == null) return;
            if (mainLog.ServiceID == serviceID)
            {
                lostContactLog = mainLog;
                logs.TryPop(out mainLog);
                tryCallback(log, ServiceRegisterLogClientChangedTypeEnum.Main | ServiceRegisterLogClientChangedTypeEnum.Delete | ServiceRegisterLogClientChangedTypeEnum.LostContact);
                return;
            }
            int logIndex = 0;
            foreach (ServiceRegisterLog nextLog in logs)
            {
                if (nextLog.ServiceID == serviceID)
                {
                    lostContactLog = nextLog;
                    logs.RemoveAtToEnd(logIndex);
                    tryCallback(log, ServiceRegisterLogClientChangedTypeEnum.Delete | ServiceRegisterLogClientChangedTypeEnum.LostContact);
                    return;
                }
                ++logIndex;
            }
        }
        /// <summary>
        /// 尝试触发服务更变回调
        /// </summary>
        /// <param name="log"></param>
        /// <param name="changedType"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void tryCallback(ServiceRegisterLog log, ServiceRegisterLogClientChangedTypeEnum changedType)
        {
            if (isLoaded) callback(log, changedType);
        }
        /// <summary>
        /// 触发服务更变回调
        /// </summary>
        /// <param name="log"></param>
        /// <param name="changedType"></param>
#if NetStandard21
        private void callback(ServiceRegisterLog? log, ServiceRegisterLogClientChangedTypeEnum changedType)
#else
        private void callback(ServiceRegisterLog log, ServiceRegisterLogClientChangedTypeEnum changedType)
#endif
        {
            if (clientServiceRegistrars.Count == 0) return;
            LeftArray<ServiceRegistryCommandClientServiceRegistrar> removeServiceRegistrars = new LeftArray<ServiceRegistryCommandClientServiceRegistrar>(0);
            foreach (ServiceRegistryCommandClientServiceRegistrar serviceRegistrar in clientServiceRegistrars)
            {
                if (!serviceRegistrar.Callback(log, changedType)) removeServiceRegistrars.Add(serviceRegistrar);
            }
            if (removeServiceRegistrars.Count == 0) return;
            if (removeServiceRegistrars.Count == clientServiceRegistrars.Count)
            {
                clientServiceRegistrarsLock.Enter();
                clientServiceRegistrars.ClearLength();
                clientServiceRegistrarsLock.Exit();
                return;
            }
            clientServiceRegistrarsLock.ExitSleepFlag();
            try
            {
                foreach (ServiceRegistryCommandClientServiceRegistrar serviceRegistrar in removeServiceRegistrars)
                {
                    clientServiceRegistrars.Remove(serviceRegistrar);
                }
            }
            finally { clientServiceRegistrarsLock.ExitSleepFlag(); }
        }
    }
}
