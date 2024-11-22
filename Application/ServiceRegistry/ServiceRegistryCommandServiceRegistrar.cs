using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

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
        private readonly CommandServerConfigBase baseConfig;
        /// <summary>
        /// 命令服务端配置
        /// </summary>
        private readonly IServiceRegistryCommandServerConfig config;
        /// <summary>
        /// 端口注册客户端
        /// </summary>
        private readonly PortRegistryClient portRegistryClient;
        /// <summary>
        /// 服务注册日志客户端组装
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal ServiceRegisterLogClientAssembler Assembler;
        /// <summary>
        /// 服务注册日志
        /// </summary>
#if NetStandard21
        internal ServiceRegisterLog? ServiceRegisterLog;
#else
        internal ServiceRegisterLog ServiceRegisterLog;
#endif
        /// <summary>
        /// 是否已经发送服务注册日志
        /// </summary>
        internal bool IsServiceRegisterLog;
        /// <summary>
        /// 服务注册组件
        /// </summary>
        /// <param name="server">命令服务</param>
        /// <param name="client"></param>
        /// <param name="baseConfig"></param>
        /// <param name="config"></param>
        /// <param name="portRegistryClient"></param>
        protected ServiceRegistryCommandServiceRegistrar(CommandListenerBase server, ServiceRegistryClient client, CommandServerConfigBase baseConfig, IServiceRegistryCommandServerConfig config, PortRegistryClient portRegistryClient) 
            : base(server)
        {
            serviceRegistryClient = client;
            this.baseConfig = baseConfig;
            this.config = config;
            this.portRegistryClient = portRegistryClient;
        }
        /// <summary>
        /// 获取服务注册日志客户端组装
        /// </summary>
        /// <returns></returns>
        private async Task getAssembler()
        {
            Assembler = await serviceRegistryClient.GetAssembler(baseConfig.ServiceName.notNull());
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            Assembler.Remove(this).Wait();
            if (portRegistryClient != null) portRegistryClient.Free(server);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override async ValueTask DisposeAsync()
        {
            await Assembler.Remove(this);
            if (portRegistryClient != null) portRegistryClient.Free(server);
        }
        /// <summary>
        /// 服务监听成功
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override async Task OnListened(IPEndPoint endPoint)
        {
            ServiceRegisterLog = config.GetServiceRegisterLog(endPoint);
            await Assembler.Append(this);
            if (portRegistryClient != null && !await portRegistryClient.SetCallback(server, portIdentity))
            {
                await AutoCSer.LogHelper.Debug($"{baseConfig.ServiceName} 设置端口标识在线检查回调委托失败");
            }
        }
        /// <summary>
        /// 端口标识
        /// </summary>
        private PortIdentity portIdentity;
        /// <summary>
        /// 获取服务监听端口号
        /// </summary>
        /// <returns></returns>
        public override async Task<ushort> GetHostPort()
        {
            if (portRegistryClient == null) return await base.GetHostPort();
            CommandClientReturnValue<PortIdentity> portIdentity = await portRegistryClient.GetPortIdentity();
            if (!portIdentity.IsSuccess) throw new InvalidOperationException($"{baseConfig.ServiceName} 获取端口标识失败 {portIdentity.ReturnType}");
            if (portIdentity.Value.Identity == 0) throw new InvalidOperationException($"{baseConfig.ServiceName} 获取端口标识失败");
            this.portIdentity = portIdentity.Value;
            return this.portIdentity.Port;
        }
        /// <summary>
        /// 通知单例服务下线
        /// </summary>
        /// <param name="serviceID"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Offline(long serviceID)
        {
            if (ServiceRegisterLog.notNull().ServiceID == serviceID) Offline();
        }
        /// <summary>
        /// 单例服务定时尝试上线
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckSingleton()
        {
            if (server.IsDisposed) return false;
            ServiceRegisterLog log = ServiceRegisterLog.notNull();
            if (log.ServiceID == 0) return false;
            AutoCSer.Threading.SecondTimer.TaskArray.Append(checkSingleton, (int)log.TimeoutSeconds + 1);
            return true;
        }
        /// <summary>
        /// 单例服务定时尝试上线
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        /// <param name="portRegistryClient"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<ServiceRegistryCommandServiceRegistrar?> Create(CommandListenerBase server, ServiceRegistryClient client, ServiceRegistryCommandServerConfig config, PortRegistryClient portRegistryClient)
#else
        public static Task<ServiceRegistryCommandServiceRegistrar> Create(CommandListenerBase server, ServiceRegistryClient client, ServiceRegistryCommandServerConfig config, PortRegistryClient portRegistryClient)
#endif
        {
            return Create(server, client, config, config, portRegistryClient);
        }
        /// <summary>
        /// 创建服务注册组件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        /// <param name="baseConfig"></param>
        /// <param name="config"></param>
        /// <param name="portRegistryClient"></param>
        /// <returns></returns>
#if NetStandard21
        public static async Task<ServiceRegistryCommandServiceRegistrar?> Create(CommandListenerBase server, ServiceRegistryClient client, CommandServerConfigBase baseConfig, IServiceRegistryCommandServerConfig config, PortRegistryClient portRegistryClient)
#else
        public static async Task<ServiceRegistryCommandServiceRegistrar> Create(CommandListenerBase server, ServiceRegistryClient client, CommandServerConfigBase baseConfig, IServiceRegistryCommandServerConfig config, PortRegistryClient portRegistryClient)
#endif
        {
            if (baseConfig.ServiceName != null)
            {
                ServiceRegistryCommandServiceRegistrar serviceRegistrar = new ServiceRegistryCommandServiceRegistrar(server, client, baseConfig, config, portRegistryClient);
                await serviceRegistrar.getAssembler();
                return serviceRegistrar;
            }
            await baseConfig.Log.Error("缺少注册服务名称配置", LogLevelEnum.AutoCSer | LogLevelEnum.Error | LogLevelEnum.Fatal);
            return null;
        }
    }
}
