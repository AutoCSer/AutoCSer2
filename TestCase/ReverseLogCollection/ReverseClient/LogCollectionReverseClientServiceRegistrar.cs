using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseClient
{
    /// <summary>
    /// 服务注册客户端组件
    /// </summary>
    internal sealed class LogCollectionReverseClientServiceRegistrar :  AutoCSer.CommandService.ServiceRegistryCommandClientServiceRegistrar
    {
        /// <summary>
        /// 反向日志收集服务客户端集合
        /// </summary>
        private readonly Dictionary<HostEndPoint, LogCollectionReverseService> clients = new Dictionary<HostEndPoint, LogCollectionReverseService>();
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config"></param>
        private LogCollectionReverseClientServiceRegistrar(ServiceRegistryClient client, AutoCSer.Net.CommandClientConfig config) : base(null, client, config)
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
            if (log == null)
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
                if (!clients.ContainsKey(hostEndPoint)) clients.Add(hostEndPoint, new LogCollectionReverseService(ref hostEndPoint));
            }
            else if ((changedType & ServiceRegisterLogClientChangedTypeEnum.Delete) != 0)
            {
                HostEndPoint hostEndPoint = log.HostEndPoint;
                if (clients.Remove(hostEndPoint, out LogCollectionReverseService client))
                {
                    using (client)
                    {
                        foreach (ServiceRegisterLog checkLog in ServiceRegisterLogs)
                        {
                            if (checkLog.CheckHostPort(log)) return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建服务注册客户端组件
        /// </summary>
        /// <param name="commandClientConfig"></param>
        /// <returns></returns>
        internal static async Task<LogCollectionReverseClientServiceRegistrar> Create(ReverseLogCollectionCommon.ServiceRegistryCommandClientConfig commandClientConfig)
        {
            ServiceRegistryClient client = ServiceRegistryClient.Get(commandClientConfig);
            LogCollectionReverseClientServiceRegistrar serviceRegistrar = new LogCollectionReverseClientServiceRegistrar(client, commandClientConfig);
            await serviceRegistrar.getAssembler();
            return serviceRegistrar;
        }
    }
}
