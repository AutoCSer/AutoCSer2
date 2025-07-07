using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server registration node interface
    /// 服务注册节点接口
    /// </summary>
    [ServerNode(IsMethodParameterCreator = true)]
    public partial interface IServerRegistryNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ServerRegistryLog value);
        /// <summary>
        /// Get the server session identity
        /// 获取服务会话标识
        /// </summary>
        /// <returns></returns>
        long GetSessionID();
        /// <summary>
        /// The server registration callback delegate is mainly used to register components to check the online state of the server
        /// 服务注册回调委托，主要用于注册组件检查服务的在线状态
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="callback">Server registration log operation type
        /// 服务注册日志操作类型</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void ServerCallback(long sessionID, MethodKeepCallback<ServerRegistryOperationTypeEnum> callback);
        /// <summary>
        /// Add the server registration log
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns>Server registration status
        /// 服务注册状态</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ServerRegistryStateEnum Append(ServerRegistryLog log);
        /// <summary>
        /// Gets the server registration log
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serverName">Monitor the server name. An empty string represents all servers
        /// 监视服务名称，空字符串表示所有服务</param>
        /// <param name="callback">The server registration log returns null to indicate an online check
        /// 服务注册日志，返回 null 表示在线检查</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
#if NetStandard21
        void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog?> callback);
#else
        void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog> callback);
#endif
        /// <summary>
        /// Get the main log of the server
        /// 获取服务主日志
        /// </summary>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        /// <returns>Returning null indicates that the server main log was not found
        /// 返回 null 表示没有找到服务主日志</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ServerRegistryLog? GetLog(string serverName);
#else
        ServerRegistryLog GetLog(string serverName);
#endif
        /// <summary>
        /// Check the online status of the server
        /// 检查服务在线状态
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsSendOnly = true)]
        void Check(long sessionID, string serverName);
        /// <summary>
        /// Persistent operations for server disconnection
        /// 服务失联的持久化操作
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        [ServerMethod(IsClientCall = false, IsIgnorePersistenceCallbackException = true)]
        void LostContact(long sessionID, string serverName);
    }
}
