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
        internal IServiceRegistryClient Client;
        /// <summary>
        /// 会话在线检查保持回调
        /// </summary>
        private CommandKeepCallback checkKeepCallback;
        /// <summary>
        /// 服务注册日志客户端组装集合
        /// </summary>
        private readonly Dictionary<HashString, ServiceRegisterLogClientAssembler> logAssemblers = DictionaryCreator.CreateHashString<ServiceRegisterLogClientAssembler>();
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
            CommandClient = commandClientConfig.CreateCommandClient();
        }
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async Task<bool> CheckCallback(IServiceRegistryClient client)
        {
            CommandKeepCallback checkKeepCallback = await client.CheckCallback(CommandClientKeepCallback.EmptyCallback);
            if (checkKeepCallback == null) return false;
            await logAssemblersLock.EnterAsync();
            try
            {
                Client = client;
                this.checkKeepCallback = checkKeepCallback;
                if (logAssemblers.Count != 0)
                {
                    foreach (KeyValuePair<HashString, ServiceRegisterLogClientAssembler> logAssembler in logAssemblers.getArray())
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
            HashString serviceNameKey = serviceName;
            ServiceRegisterLogClientAssembler logAssembler;
            await logAssemblersLock.EnterAsync();
            try
            {
                if (!logAssemblers.TryGetValue(serviceNameKey, out logAssembler))
                {
                    logAssembler = new ServiceRegisterLogClientAssembler(this, serviceName);
                    await logAssembler.LogCallback();
                    logAssemblers.Add(serviceNameKey, logAssembler);
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
        public virtual async Task ServiceRestierAgainLogCallbackFail(string serviceName)
        {
            await CommandClient.Log.Error($"服务注册客户端重连获取服务 {serviceName} 注册日志失败", LogLevel.AutoCSer | LogLevel.Fatal | LogLevel.Error);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public virtual async Task ServiceRestierAgainFail(string serviceName, CommandClientReturnValue returnValue)
        {
            await CommandClient.Log.Error($"服务注册客户端重连注册服务 {serviceName} 失败 {returnValue.IsSuccess} {returnValue.ErrorMessage}", LogLevel.AutoCSer | LogLevel.Fatal | LogLevel.Error);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task ServiceRestierAgainFail(string serviceName, ServiceRegisterState state)
        {
            await CommandClient.Log.Error($"服务注册客户端重连注册服务 {serviceName} 失败 {state}", LogLevel.AutoCSer | LogLevel.Fatal | LogLevel.Error);
        }
        /// <summary>
        /// 不可识别服务注册日志操作类型
        /// </summary>
        /// <param name="log"></param>
        public virtual void UnrecognizableOperationType(ServiceRegisterLog log)
        {
            CatchTask.AddIgnoreException(CommandClient.Log.Error($"不可识别服务 {log.ServiceName} 注册日志操作类型 {log.OperationType}", LogLevel.AutoCSer | LogLevel.Fatal | LogLevel.Error));
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public virtual async Task ServiceRestierLogoutFail(string serviceName, CommandClientReturnValue returnValue)
        {
            await CommandClient.Log.Debug($"服务 {serviceName} 注销失败 {returnValue.ReturnType} {returnValue.ErrorMessage}", LogLevel.AutoCSer | LogLevel.Debug | LogLevel.Warn | LogLevel.Info);
        }
        /// <summary>
        /// 服务注册客户端重连注册服务失败
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task ServiceRestierLogoutFail(string serviceName, ServiceRegisterState state)
        {
            await CommandClient.Log.Debug($"服务 {serviceName} 注销失败 {state}", LogLevel.AutoCSer | LogLevel.Debug | LogLevel.Warn | LogLevel.Info);
        }

        /// <summary>
        /// 服务注册客户端集合
        /// </summary>
        private static readonly Dictionary<HostEndPoint, ServiceRegistryClient> clients = DictionaryCreator.CreateEndPoint<ServiceRegistryClient>();
        /// <summary>
        /// 获取服务注册客户端
        /// </summary>
        /// <param name="commandClientConfig">注册服务命令客户端配置</param>
        /// <returns>服务注册客户端</returns>
        public static ServiceRegistryClient Get(ServiceRegistryCommandClientConfig commandClientConfig)
        {
            ServiceRegistryClient client;
            Monitor.Enter(clients);
            try
            {
                if (!clients.TryGetValue(commandClientConfig.Host, out client)) clients.Add(commandClientConfig.Host, client = new ServiceRegistryClient(commandClientConfig));
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
            CommandClientSocket socket = await client.CommandClient.GetSocketAsync();
            if (socket == null) await logConfig.Log.Error($"服务注册客户端初始化失败 {commandClientConfig.Host.Host}:{commandClientConfig.Host.Port}", LogLevel.AutoCSer | LogLevel.Error | LogLevel.Fatal);
            return client;
        }
    }
}
