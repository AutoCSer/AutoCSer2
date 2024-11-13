using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册客户端组件
    /// </summary>
    public class ServiceRegistryCommandClientServiceRegistrar : AutoCSer.Net.CommandClientServiceRegistrar
    {
        /// <summary>
        /// 服务注册客户端
        /// </summary>
        private readonly ServiceRegistryClient serviceRegistryClient;
        /// <summary>
        /// 命令客户端配置
        /// </summary>
        private readonly CommandClientConfig commandClientConfig;
        /// <summary>
        /// 服务注册日志客户端组装
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal ServiceRegisterLogClientAssembler Assembler;
        /// <summary>
        /// 服务日志集合
        /// </summary>
        public IEnumerable<ServiceRegisterLog> ServiceRegisterLogs { get { return Assembler.Logs; } }
        /// <summary>
        /// 等待服务监听地址定时间隔秒数
        /// </summary>
        protected virtual byte waitServerEndPointSeconds { get { return 1; } }
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="client"></param>
        /// <param name="config"></param>
        protected ServiceRegistryCommandClientServiceRegistrar(CommandClient commandClient, ServiceRegistryClient client, CommandClientConfig config) : base(commandClient)
        {
            serviceRegistryClient = client;
            commandClientConfig = config;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            Assembler.Remove(this);
        }
        /// <summary>
        /// 获取服务注册日志客户端组装
        /// </summary>
        /// <returns></returns>
        protected async Task getAssembler()
        {
            Assembler = await serviceRegistryClient.GetAssembler(commandClientConfig.ServiceName.notNull());
            Assembler.Append(this);
        }
        /// <summary>
        /// 获取服务监听地址
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public override async Task<IPEndPoint?> GetServerEndPoint()
#else
        public override async Task<IPEndPoint> GetServerEndPoint()
#endif
        {
            var log = Assembler.MainLog;
            if (log != null) return log.HostEndPoint.IPEndPoint;
            await (new ServiceRegistryWaitServerEndPointTask(this, Math.Max(waitServerEndPointSeconds, (byte)1))).TryAppendTaskArrayAsync();
            return null;
        }
        /// <summary>
        /// 等待服务监听地址
        /// </summary>
        /// <returns>是否需要取消定时任务</returns>
        internal async Task<bool> WaitServerEndPoint()
        {
            //if (Assembler.MainLog != null) return true;
            return await client.WaitServerEndPoint();
        }
        /// <summary>
        /// 触发服务更变回调（网络 IO 线程同步回调，不允许阻塞线程）
        /// </summary>
        /// <param name="log"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
#if NetStandard21
        public virtual bool Callback(ServiceRegisterLog? log, ServiceRegisterLogClientChangedTypeEnum changedType)
#else
        public virtual bool Callback(ServiceRegisterLog log, ServiceRegisterLogClientChangedTypeEnum changedType)
#endif
        {
            if (client.IsDisposed) return false;
            if ((changedType & ServiceRegisterLogClientChangedTypeEnum.Main) != 0)
            {
                var mainLog = Assembler.MainLog;
                if (mainLog != null) client.ServerEndPointChanged(mainLog.HostEndPoint.IPEndPoint);
            }
            return true;
        }

        /// <summary>
        /// 创建服务注册客户端组件
        /// </summary>
        /// <param name="commandClient"></param>
        /// <param name="client"></param>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ServiceRegistryCommandClientServiceRegistrar?> Create(CommandClient commandClient, ServiceRegistryClient client, CommandClientConfig config)
#else
        public static async Task<ServiceRegistryCommandClientServiceRegistrar> Create(CommandClient commandClient, ServiceRegistryClient client, CommandClientConfig config)
#endif
        {
            if (config.ServiceName != null)
            {
                ServiceRegistryCommandClientServiceRegistrar serviceRegistrar = new ServiceRegistryCommandClientServiceRegistrar(commandClient, client, config);
                await serviceRegistrar.getAssembler();
                return serviceRegistrar;
            }
            await config.Log.Error("缺少注册服务名称配置", LogLevelEnum.AutoCSer | LogLevelEnum.Error | LogLevelEnum.Fatal);
            return null;
        }
    }
}
