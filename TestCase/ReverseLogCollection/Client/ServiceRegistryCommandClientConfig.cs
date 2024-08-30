using System;
using System.Reflection;
using AutoCSer.CommandService;
using AutoCSer.Net;

namespace AutoCSer.TestCase.ReverseLogCollection
{
    /// <summary>
    /// 注册服务命令客户端配置
    /// </summary>
    internal sealed class ServiceRegistryCommandClientConfig : AutoCSer.CommandService.ServiceRegistryCommandClientConfig
    {
        /// <summary>
        /// 注册服务命令客户端配置
        /// </summary>
        public ServiceRegistryCommandClientConfig()
        {
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        }
        ///// <summary>
        ///// 创建服务注册客户端
        ///// </summary>
        ///// <returns></returns>
        //public override CommandClient CreateCommandClient()
        //{
        //    return new CommandClient(this
        //        , CommandClientInterfaceControllerCreator.GetCreator<ITimestampVerifyClient, ITimestampVerifyService>()
        //        , CommandClientInterfaceControllerCreator.GetCreator<IServiceRegistryClient, IServiceRegistryService>()
        //        );
        //}
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new ServiceRegistryCommandClientSocketEvent(client, this, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
        }
    }
}
