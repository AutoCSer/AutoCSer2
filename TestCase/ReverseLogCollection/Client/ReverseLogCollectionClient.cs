using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollection;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 反向日志收集客户端
    /// </summary>
    internal class ReverseLogCollectionClient : IDisposable
    {
        /// <summary>
        /// 服务注册客户端组件
        /// </summary>
        private readonly ReverseLogCollectionClientServiceRegistrar serviceRegistrar;
        /// <summary>
        /// 命令客户端配置
        /// </summary>
        protected readonly CommandClientConfig commandClientConfig;
        /// <summary>
        /// 命令客户端
        /// </summary>
        private CommandClient commandClient;
        /// <summary>
        /// 获取日志保持回调命令
        /// </summary>
        private KeepCallbackCommand logCallbackKeepCallbackCommand;
        /// <summary>
        /// 是否释放资源
        /// </summary>
        private bool isDispose;
        /// <summary>
        /// 反向日志收集客户端
        /// </summary>
        /// <param name="serviceRegistrar"></param>
        /// <param name="hostEndPoint"></param>
        internal ReverseLogCollectionClient(ReverseLogCollectionClientServiceRegistrar serviceRegistrar, ref HostEndPoint hostEndPoint)
        {
            commandClientConfig = new CommandClientConfig(this, ref hostEndPoint);
            this.serviceRegistrar = serviceRegistrar;
        }
        /// <summary>
        /// 启动客户端
        /// </summary>
        internal void Start()
        {
            commandClient = new CommandClient(commandClientConfig
                , CommandClientInterfaceControllerCreator.GetCreator<ITimestampVerifyClient, ITimestampVerifyService>()
                , CommandClientInterfaceControllerCreator.GetCreator<IReverseLogCollectionClient<LogInfo>, IReverseLogCollectionService<LogInfo>>()
                );
        }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="client"></param>
        internal void OnMethodVerified(IReverseLogCollectionClient<LogInfo> client)
        {
            if (!isDispose) logCallbackKeepCallbackCommand = client.LogCallback(logCallback);
        }
        /// <summary>
        /// 日志收集回调
        /// </summary>
        /// <param name="log"></param>
        /// <param name="keepCallbackCommand"></param>
        private void logCallback(CommandClientReturnValue<LogInfo> log, KeepCallbackCommand keepCallbackCommand)
        {
            if (log.IsSuccess) serviceRegistrar.LogCallback(log.Value);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDispose = true;
            commandClient?.Dispose();
        }
    }
}
