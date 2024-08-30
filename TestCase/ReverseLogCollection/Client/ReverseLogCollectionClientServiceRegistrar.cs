using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollection;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 服务注册客户端组件
    /// </summary>
    internal sealed class ReverseLogCollectionClientServiceRegistrar :  AutoCSer.CommandService.ServiceRegistryCommandClientServiceRegistrar
    {
        /// <summary>
        /// 反向日志收集服务客户端集合
        /// </summary>
        private readonly Dictionary<HostEndPoint, ReverseLogCollectionClient> clients = new Dictionary<HostEndPoint, ReverseLogCollectionClient>();
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config"></param>
        private ReverseLogCollectionClientServiceRegistrar(ServiceRegistryClient client, AutoCSer.Net.CommandClientConfig config) : base(null, client, config)
        {
        }
        /// <summary>
        /// 触发服务更变回调（网络 IO 线程同步回调，不允许阻塞线程）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
        public override bool Callback(ServiceRegisterLog log, ServiceRegisterLogClientChangedTypeEnum changedType)
        {
            if(log == null)
            {
                Console.WriteLine(changedType);
                foreach(ServiceRegisterLog loadLog in ServiceRegisterLogs) callback(loadLog, changedType);
                return true;
            }
            callback(log, changedType);
            return true;
        }
        /// <summary>
        /// 触发服务更变回调（网络 IO 线程同步回调，不允许阻塞线程）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
        private void callback(ServiceRegisterLog log, ServiceRegisterLogClientChangedTypeEnum changedType)
        {
            Console.WriteLine($"{changedType} {log.Host}:{log.Port}");
            if ((changedType & (ServiceRegisterLogClientChangedTypeEnum.Main | ServiceRegisterLogClientChangedTypeEnum.Append)) != 0)
            {
                HostEndPoint hostEndPoint = log.HostEndPoint;
                if (!clients.ContainsKey(hostEndPoint))
                {
                    ReverseLogCollectionClient client = new ReverseLogCollectionClient(this, ref hostEndPoint);
                    clients.Add(hostEndPoint, client);
                    client.Start();
                }
            }
            else if ((changedType & ServiceRegisterLogClientChangedTypeEnum.Delete) != 0)
            {
                HostEndPoint hostEndPoint = log.HostEndPoint;
                if (!clients.TryGetValue(hostEndPoint, out ReverseLogCollectionClient client))
                {
                    foreach (ServiceRegisterLog checkLog in ServiceRegisterLogs)
                    {
                        if (checkLog.CheckHostPort(log)) return;
                    }
                    clients.Remove(hostEndPoint);
                    client.Dispose();
                }
            }
        }
        /// <summary>
        /// 日志收集回调
        /// </summary>
        /// <param name="log"></param>
        public void LogCallback(LogInfo log)
        {
            Console.WriteLine($"{log.LogTime.toString()} {log.Message}");
        }

        /// <summary>
        /// 创建服务注册客户端组件
        /// </summary>
        /// <param name="commandClientConfig"></param>
        /// <returns></returns>
        internal static async Task<ReverseLogCollectionClientServiceRegistrar> Create(ReverseLogCollection.ServiceRegistryCommandClientConfig commandClientConfig)
        {
            ServiceRegistryClient client = ServiceRegistryClient.Get(commandClientConfig);
            ReverseLogCollectionClientServiceRegistrar serviceRegistrar = new ReverseLogCollectionClientServiceRegistrar(client, new AutoCSer.Net.CommandClientConfig { ServiceName = commandClientConfig.ServiceName });
            await serviceRegistrar.getAssembler();
            return serviceRegistrar;
        }
    }
}
