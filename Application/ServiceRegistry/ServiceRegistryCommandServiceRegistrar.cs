using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册服务端组件
    /// </summary>
    public class ServiceRegistryCommandServiceRegistrar : AutoCSer.Net.CommandServiceRegistrar
    {
        /// <summary>
        /// 服务注册客户端
        /// </summary>
        private readonly ServiceRegistryClient serviceRegistryClient;
        /// <summary>
        /// 命令服务端配置
        /// </summary>
        private readonly ServiceRegistryCommandServerConfig commandServerConfig;
        /// <summary>
        /// 服务注册日志客户端组装
        /// </summary>
        internal ServiceRegisterLogClientAssembler Assembler;
        /// <summary>
        /// 服务注册日志
        /// </summary>
        internal ServiceRegisterLog ServiceRegisterLog;
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="server">命令服务</param>
        /// <param name="client"></param>
        /// <param name="config"></param>
        protected ServiceRegistryCommandServiceRegistrar(CommandListener server, ServiceRegistryClient client, ServiceRegistryCommandServerConfig config) : base(server)
        {
            serviceRegistryClient = client;
            commandServerConfig = config;
        }
        /// <summary>
        /// 获取服务注册日志客户端组装
        /// </summary>
        /// <returns></returns>
        private async Task getAssembler()
        {
            Assembler = await serviceRegistryClient.GetAssembler(commandServerConfig.ServiceName);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            CatchTask.AddIgnoreException(Assembler.Remove(this));
        }
        /// <summary>
        /// 服务监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override async Task OnListened(IPEndPoint endPoint)
        {
            ServiceRegisterLog = commandServerConfig.GetServiceRegisterLog(endPoint);
            await Assembler.Append(this);
        }
        /// <summary>
        /// 通知单例服务下线
        /// </summary>
        /// <param name="serviceID"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Offline(long serviceID)
        {
            if (ServiceRegisterLog.ServiceID == serviceID) Offline();
        }
        /// <summary>
        /// 单例服务定时尝试上线
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CheckSingleton()
        {
            if (server.IsDisposed || ServiceRegisterLog.ServiceID == 0) return false;
            AutoCSer.Threading.SecondTimer.TaskArray.Append(checkSingleton, (int)ServiceRegisterLog.TimeoutSeconds + 1);
            return true;
        }
        /// <summary>
        /// 单例服务定时尝试上线
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void checkSingleton()
        {
            if (!server.IsDisposed) Assembler.CheckSingleton(this);
        }

        /// <summary>
        /// 创建服务注册组件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task<ServiceRegistryCommandServiceRegistrar> Create(CommandListener server, ServiceRegistryClient client, ServiceRegistryCommandServerConfig config)
        {
            if (config.ServiceName != null)
            {
                ServiceRegistryCommandServiceRegistrar serviceRegistrar = new ServiceRegistryCommandServiceRegistrar(server, client, config);
                await serviceRegistrar.getAssembler();
                return serviceRegistrar;
            }
            await config.Log.Error("缺少注册服务名称配置", LogLevel.AutoCSer | LogLevel.Error | LogLevel.Fatal);
            return null;
        }
    }
}
