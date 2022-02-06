using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 服务注册日志
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, IsAnonymousFields = true)]
    public class ServiceRegisterLog
    {
        /// <summary>
        /// 服务标识ID
        /// </summary>
        public long ServiceID { get; internal set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; internal set; }
        /// <summary>
        /// 主机名称或者IP地址
        /// </summary>
        public string Host { get; internal set; }
        /// <summary>
        /// 服务版本号，高版本上限将踢掉所有低版本节点
        /// </summary>
        public uint Version { get; internal set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public ushort Port { get; internal set; }
        /// <summary>
        /// 服务注册日志操作类型
        /// </summary>
        public ServiceRegisterOperationType OperationType { get; internal set; }
        /// <summary>
        /// 单例服务强制上线等待秒数
        /// </summary>
        public byte TimeoutSeconds { get; internal set; }
        /// <summary>
        /// 服务注册日志
        /// </summary>
        public ServiceRegisterLog() { }
        /// <summary>
        /// 服务注册日志
        /// </summary>
        /// <param name="config">命令服务端配置</param>
        /// <param name="endPoint">服务监听地址</param>
        /// <param name="operationType">服务注册日志操作类型</param>
        /// <param name="timeoutSeconds">单例服务强制上线等待秒数</param>
        /// <param name="version">服务版本号，高版本上限将踢掉所有低版本节点</param>
        public ServiceRegisterLog(AutoCSer.Net.CommandServerConfig config, IPEndPoint endPoint, ServiceRegisterOperationType operationType = ServiceRegisterOperationType.ClusterMain, byte timeoutSeconds = 0, uint version = 0)
        {
            ServiceName = config.ServiceName;
            Host = endPoint.Address.ToString();
            Port = (ushort)endPoint.Port;
            OperationType = operationType;
            Version = version;
            TimeoutSeconds = timeoutSeconds;
        }
        /// <summary>
        /// 创建服务注销日志
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ServiceRegisterLog CreateLogout()
        {
            return new ServiceRegisterLog
            {
                ServiceID = ServiceID,
                ServiceName = ServiceName,
                OperationType = ServiceRegisterOperationType.Logout
            };
        }
        /// <summary>
        /// 创建失联服务日志
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServiceRegisterLog CreateLostContact()
        {
            return new ServiceRegisterLog
            {
                ServiceID = ServiceID,
                ServiceName = ServiceName,
                OperationType = ServiceRegisterOperationType.LostContact
            };
        }
        /// <summary>
        /// 创建通知单例服务下线日志
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ServiceRegisterLog CreateOffline()
        {
            return new ServiceRegisterLog
            {
                ServiceID = ServiceID,
                ServiceName = ServiceName,
                OperationType = ServiceRegisterOperationType.Offline
            };
        }
        /// <summary>
        /// 获取服务监听地址
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public IPEndPoint GetIPEndPoint()
        {
            return new AutoCSer.Net.HostEndPoint(Port, Host).IPEndPoint;
        }
    }
}
