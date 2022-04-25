using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册接口
    /// </summary>
    public interface IServiceRegistryService
    {
        /// <summary>
        /// 设置服务会话在线检查回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="callback">服务会话在线检查回调委托</param>
        [CommandServerMethod(IsInitobj = false, AutoCancelKeep = false)]
        void CheckCallback(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerKeepCallback callback);
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="log"></param>
        /// <returns>服务注册结果</returns>
        ServiceRegisterResponse Append(CommandServerSocket socket, CommandServerCallQueue queue, ServiceRegisterLog log);
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
        /// <param name="callback">服务注册日志回调委托</param>
        [CommandServerMethod(IsOutputPool = true, AutoCancelKeep = false)]
        void LogCallback(CommandServerSocket socket, CommandServerCallQueue queue, string serviceName, CommandServerKeepCallback<ServiceRegisterLog> callback);
    }
}
