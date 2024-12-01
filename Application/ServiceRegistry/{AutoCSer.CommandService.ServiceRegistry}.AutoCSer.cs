//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.CommandService
{
        /// <summary>
        /// 端口注册服务 客户端接口
        /// </summary>
        public partial interface IPortRegistryServiceClientController
        {
            /// <summary>
            /// 释放端口标识
            /// </summary>
            /// <param name="portIdentity"></param>
            AutoCSer.Net.SendOnlyCommand FreePort(AutoCSer.CommandService.PortIdentity portIdentity);
            /// <summary>
            /// 获取一个空闲端口标识
            /// </summary>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.PortIdentity> GetPort();
            /// <summary>
            /// 设置端口标识在线检查回调委托
            /// </summary>
            /// <param name="portIdentity"></param>
            AutoCSer.Net.EnumeratorCommand SetCallback(AutoCSer.CommandService.PortIdentity portIdentity);
            /// <summary>
            /// 断线重连设置端口标识
            /// </summary>
            /// <param name="portIdentity"></param>
            /// <returns></returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.PortIdentity> SetPort(AutoCSer.CommandService.PortIdentity portIdentity);
        }
}namespace AutoCSer.CommandService
{
        /// <summary>
        /// 服务注册接口 客户端接口
        /// </summary>
        public partial interface IServiceRegistryServiceClientController
        {
            /// <summary>
            /// 添加服务注册日志
            /// </summary>
            /// <param name="log"></param>
            /// <returns>服务注册结果</returns>
            AutoCSer.Net.ReturnCommand<AutoCSer.CommandService.ServiceRegisterResponse> Append(AutoCSer.CommandService.ServiceRegisterLog log);
            /// <summary>
            /// 设置服务会话在线检查回调委托
            /// </summary>
            AutoCSer.Net.EnumeratorCommand CheckCallback();
            /// <summary>
            /// 获取服务注册日志
            /// </summary>
            /// <param name="serviceName">监视服务名称，null 标识所有服务</param>
            /// <returns>服务注册日志回调委托，返回 null 表示初始化加载完毕</returns>
            AutoCSer.Net.EnumeratorCommand<AutoCSer.CommandService.ServiceRegisterLog> LogCallback(string serviceName);
        }
}
#endif