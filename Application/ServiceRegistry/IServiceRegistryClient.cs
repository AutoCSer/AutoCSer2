﻿using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册客户端接口
    /// </summary>
    public interface IServiceRegistryClient
    {
        /// <summary>
        /// 设置服务会话在线检查回调委托
        /// </summary>
        /// <param name="callback">服务会话在线检查回调委托</param>
        /// <returns></returns>
        KeepCallbackCommand CheckCallback(CommandClientKeepCallback callback);
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns>服务注册结果</returns>
        ReturnCommand<ServiceRegisterResponse> Append(ServiceRegisterLog log);
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
        /// <param name="callback">服务注册日志回调委托</param>
        /// <returns></returns>
        KeepCallbackCommand LogCallback(string serviceName, Action<CommandClientReturnValue<ServiceRegisterLog>, KeepCallbackCommand> callback);
    }
}
