using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 注册服务命令客户端配置
    /// </summary>
    public class ServiceRegistryCommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// 服务注册客户端
        /// </summary>
        internal ServiceRegistryClient Client;
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            if (GetSocketEventDelegate != null) return GetSocketEventDelegate(client);
            return new ServiceRegistryCommandClientSocketEvent(client, this);
        }
        ///// <summary>
        ///// 创建服务注册客户端
        ///// </summary>
        ///// <returns></returns>
        //public virtual CommandClient CreateCommandClient()
        //{
        //    return new CommandClient(this
        //        , CommandClientInterfaceControllerCreator.GetCreator<IServiceRegistryClient, IServiceRegistryService>()
        //        );
        //}
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal async Task OnMethodVerified(IServiceRegistryClient client)
        {
            if (Client != null && !await Client.CheckCallback(client))
            {
                await Log.Error($"注册客户端初始化失败 {Host.Host}:{Host.Port}", LogLevelEnum.AutoCSer | LogLevelEnum.Error | LogLevelEnum.Fatal);
            }
        }
    }
}
