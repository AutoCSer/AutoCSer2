using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册客户端
    /// </summary>
    public class ServiceRegistryClient
    {
        /// <summary>
        /// 命令客户端
        /// </summary>
        public readonly CommandClient CommandClient;
        /// <summary>
        /// 服务注册客户端接口
        /// </summary>
#if NetStandard21
        internal IServiceRegistryServiceClientController? Client;
#else
        internal IServiceRegistryServiceClientController Client;
#endif
        /// <summary>
        /// 会话在线检查保持回调
        /// </summary>
#if NetStandard21
        private CommandKeepCallback? checkKeepCallback;
#else
        private CommandKeepCallback checkKeepCallback;
#endif
        /// <summary>
        /// 服务注册日志客户端组装集合
        /// </summary>
        private readonly Dictionary<string, ServiceRegisterLogClientAssembler> logAssemblers = DictionaryCreator.CreateAny<string, ServiceRegisterLogClientAssembler>();
        /// <summary>
        /// 服务注册日志客户端组装集合访问锁
        /// </summary>
        private AutoCSer.Threading.SemaphoreSlimLock logAssemblersLock = new Threading.SemaphoreSlimLock(1, 1);
        /// <summary>
        /// 服务注册客户端
        /// </summary>
        /// <param name="commandClientConfig">注册服务命令客户端配置</param>
        private ServiceRegistryClient(ServiceRegistryCommandClientConfig commandClientConfig)
        {
            commandClientConfig.Client = this;
            CommandClient = new CommandClient(commandClientConfig);
        }
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async Task<bool> CheckCallback(IServiceRegistryServiceClientController client)
        {
            var checkKeepCallback = await client.CheckCallback(CommandClientKeepCallback.EmptyCallback);
            if (checkKeepCallback == null) return false;
            await logAssemblersLock.EnterAsync();
            try
            {
                Client = client;
                this.checkKeepCallback = checkKeepCallback;
                if (logAssemblers.Count != 0)
                {
                    foreach(ServiceRegisterLogClientAssembler logAssembler in logAssemblers.Values) await logAssembler.CheckServiceRegisterLog();
                    foreach (KeyValuePair<string, ServiceRegisterLogClientAssembler> logAssembler in logAssemblers.getArray())
                    {
                        ServiceRegisterLogClientAssembler newLogAssembler = new ServiceRegisterLogClientAssembler(logAssembler.Value);
                        await newLogAssembler.LogCallback();
                        logAssemblers[logAssembler.Key] = newLogAssembler;
                    }
                }
            }
            finally { logAssemblersLock.Exit(); }
            return true;
        }
        /// <summary>
        /// 获取服务注册日志客户端组装
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task<ServiceRegisterLogClientAssembler> GetAssembler(string serviceName)
        {
            var logAssembler = default(ServiceRegisterLogClientAssembler);
            await logAssemblersLock.EnterAsync();
            try
            {
                if (!logAssemblers.TryGetValue(serviceName, out logAssembler))
                {
                    logAssembler = new ServiceRegisterLogClientAssembler(this, serviceName);
                    await logAssembler.LogCallback();
                    logAssemblers.Add(serviceName, logAssembler);
                }
            }
            finally { logAssemblersLock.Exit(); }
            return logAssembler;
        }
        /// <summary>
        /// 服务注册客户端重连获取服务注册日志失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public virtual Task ServiceRestierAgainLogCallbackFail(string serviceName)
        {
            return CommandClient.Log.Error($"服务注册客户端重连获取服务 {serviceName} 注册日志失败", LogLevelEnum.AutoCSer | LogLevelEnum.Fatal | LogLevelEnum.Error);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public virtual Task ServiceRestierAgainFail(string serviceName, CommandClientReturnValue returnValue)
        {
            return CommandClient.Log.Error($"服务注册客户端重连注册服务 {serviceName} 失败 {returnValue.IsSuccess} {returnValue.ErrorMessage}", LogLevelEnum.AutoCSer | LogLevelEnum.Fatal | LogLevelEnum.Error);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Task ServiceRestierAgainFail(string serviceName, ServiceRegisterStateEnum state)
        {
            return CommandClient.Log.Error($"服务注册客户端重连注册服务 {serviceName} 失败 {state}", LogLevelEnum.AutoCSer | LogLevelEnum.Fatal | LogLevelEnum.Error);
        }
        /// <summary>
        /// 不可识别服务注册日志操作类型
        /// </summary>
        /// <param name="log"></param>
        public virtual void UnrecognizableOperationType(ServiceRegisterLog log)
        {
            CommandClient.Log.ErrorIgnoreException($"不可识别服务 {log.ServiceName} 注册日志操作类型 {log.OperationType}", LogLevelEnum.AutoCSer | LogLevelEnum.Fatal | LogLevelEnum.Error);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public virtual Task ServiceRestierLogoutFail(string serviceName, CommandClientReturnValue returnValue)
        {
            return CommandClient.Log.Debug($"服务 {serviceName} 注销失败 {returnValue.ReturnType} {returnValue.ErrorMessage}", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn | LogLevelEnum.Info);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual Task ServiceRestierLogoutFail(string serviceName, ServiceRegisterStateEnum state)
        {
            return CommandClient.Log.Debug($"服务 {serviceName} 注销失败 {state}", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn | LogLevelEnum.Info);
        }

        /// <summary>
        /// 服务注册客户端集合
        /// </summary>
        private static readonly Dictionary<HostEndPoint, ServiceRegistryClient> clients = AutoCSer.Extensions.DictionaryCreator.CreateEndPoint<ServiceRegistryClient>();
        /// <summary>
        /// 获取服务注册客户端
        /// </summary>
        /// <param name="commandClientConfig">注册服务命令客户端配置</param>
        /// <returns>服务注册客户端</returns>
        public static ServiceRegistryClient Get(ServiceRegistryCommandClientConfig commandClientConfig)
        {
            var client = default(ServiceRegistryClient);
            Monitor.Enter(clients);
            if (clients.TryGetValue(commandClientConfig.Host, out client))
            {
                Monitor.Exit(clients);
                return client;
            }
            try
            {
                clients.Add(commandClientConfig.Host, client = new ServiceRegistryClient(commandClientConfig));
            }
            finally { Monitor.Exit(clients); }
            return client;
        }
        ///// <summary>
        ///// 获取服务注册客户端
        ///// </summary>
        ///// <param name="host"></param>
        ///// <param name="port"></param>
        ///// <returns></returns>
        //public static ServiceRegistryClient Get(string host, ushort port)
        //{
        //    HostEndPoint hostEndPoint = new HostEndPoint(port, host);
        //    ServiceRegistryClient client;
        //    Monitor.Enter(clients);
        //    try
        //    {
        //        if (!clients.TryGetValue(hostEndPoint, out client)) clients.Add(hostEndPoint, client = new ServiceRegistryClient(new ServiceRegistryCommandClientConfig { Host = hostEndPoint }));
        //    }
        //    finally { Monitor.Exit(clients); }
        //    return client;
        //}
        /// <summary>
        /// 获取服务注册客户端
        /// </summary>
        /// <param name="commandClientConfig"></param>
        /// <param name="logConfig"></param>
        /// <returns></returns>
        public static async Task<ServiceRegistryClient> Get(ServiceRegistryCommandClientConfig commandClientConfig, CommandServerConfigBase logConfig)
        {
            ServiceRegistryClient client = Get(commandClientConfig);
            var socket = await client.CommandClient.GetSocketAsync();
            if (socket == null) await logConfig.Log.Error($"服务注册客户端初始化失败 {commandClientConfig.Host.Host}:{commandClientConfig.Host.Port}", LogLevelEnum.AutoCSer | LogLevelEnum.Error | LogLevelEnum.Fatal);
            return client;
        }
    }
}
