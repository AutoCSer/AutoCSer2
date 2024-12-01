using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册客户端接口
    /// </summary>
    public partial interface IServiceRegistryServiceClientController
    {
        /// <summary>
        /// 设置服务会话在线检查回调委托
        /// </summary>
        /// <param name="callback">服务会话在线检查回调委托</param>
        /// <returns></returns>
        KeepCallbackCommand CheckCallback(CommandClientKeepCallback callback);
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
        /// <param name="callback">服务注册日志回调委托，返回 null 表示初始化加载完毕</param>
        /// <returns></returns>
#if NetStandard21
        KeepCallbackCommand LogCallback(string serviceName, Action<CommandClientReturnValue<ServiceRegisterLog?>, KeepCallbackCommand> callback);
#else
        KeepCallbackCommand LogCallback(string serviceName, Action<CommandClientReturnValue<ServiceRegisterLog>, KeepCallbackCommand> callback);
#endif
    }
}
