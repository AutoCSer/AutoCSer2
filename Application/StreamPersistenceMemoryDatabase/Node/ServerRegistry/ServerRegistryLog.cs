﻿using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server Registration Log
    /// 服务注册日志
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed class ServerRegistryLog
    {
        /// <summary>
        /// Server session Identifier
        /// 服务会话标识ID
        /// </summary>
        public readonly long SessionID;
        /// <summary>
        /// Server name
        /// 服务名称
        /// </summary>
        public readonly string ServerName;
        /// <summary>
        /// Host name or IP address
        /// 主机名称或者 IP 地址
        /// </summary>
        public readonly string Host;
        /// <summary>
        /// Cluster node number
        /// 集群节点编号
        /// </summary>
        public readonly uint NodeIndex;
        /// <summary>
        /// Service version number: When a higher version is launched, all lower version nodes will be kicked out
        /// 服务版本号，高版本上线将踢掉所有低版本节点
        /// </summary>
        public readonly uint Version;
        /// <summary>
        /// Port number
        /// 端口号
        /// </summary>
        public readonly ushort Port;
        /// <summary>
        /// Server host and port information
        /// 服务主机与端口信息
        /// </summary>
        public AutoCSer.Net.HostEndPoint HostEndPoint
        {
            get
            {
                return !string.IsNullOrEmpty(Host) ? new AutoCSer.Net.HostEndPoint(Port, Host) : new AutoCSer.Net.HostEndPoint(Port);
            }
        }
        /// <summary>
        /// Server registration log operation type
        /// 服务注册日志操作类型
        /// </summary>
        public readonly ServerRegistryOperationTypeEnum OperationType;
        /// <summary>
        /// The number of seconds for a singleton service to be forced to go online
        /// 单例服务强制上线等待秒数
        /// </summary>
        public byte TimeoutSeconds;
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        private ServerRegistryLog()
        {
#if NetStandard21
            ServerName = Host = string.Empty;
#endif
        }
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="operationType"></param>
        internal ServerRegistryLog(string serverName, ServerRegistryOperationTypeEnum operationType)
        {
            ServerName = serverName;
            OperationType = operationType;
#if NetStandard21
            Host = string.Empty;
#endif
        }
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="serverName"></param>
        /// <param name="operationType"></param>
        internal ServerRegistryLog(long sessionID, string serverName, ServerRegistryOperationTypeEnum operationType)
        {
            SessionID = sessionID;
            ServerName = serverName;
            OperationType = operationType;
#if NetStandard21
            Host = string.Empty;
#endif
        }
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="serverName"></param>
        /// <param name="operationType"></param>
        /// <param name="endPoint"></param>
        /// <param name="nodeIndex"></param>
        /// <param name="version"></param>
        /// <param name="timeoutSeconds"></param>
        public ServerRegistryLog(long sessionID, string serverName, ServerRegistryOperationTypeEnum operationType, HostEndPoint endPoint, uint nodeIndex = 0, uint version = 0, byte timeoutSeconds = 0)
        {
            SessionID = sessionID;
            ServerName = serverName;
            Host = endPoint.Host;
            Port = endPoint.Port;
            NodeIndex = nodeIndex;
            Version = version;
            OperationType = operationType;
            TimeoutSeconds = timeoutSeconds;
        }
        /// <summary>
        /// Determine whether it is the server restart log
        /// 判断是否服务重启日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsNewEquals(ServerRegistryLog log)
        {
            return Port == log.Port && Host == log.Host && OperationType == log.OperationType && NodeIndex == log.NodeIndex;
        }
        /// <summary>
        /// Create a server disconnection log
        /// 创建服务失联日志
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerRegistryLog CreateLostContact()
        {
            return new ServerRegistryLog(SessionID, ServerName, ServerRegistryOperationTypeEnum.LostContact);
        }
        /// <summary>
        /// Create the log of the logout server
        /// 创建注销服务日志
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerRegistryLog CreateLogout()
        {
            return new ServerRegistryLog(SessionID, ServerName, ServerRegistryOperationTypeEnum.Logout);
        }
    }
}
