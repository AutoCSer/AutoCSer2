﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务注册节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false)]
    public partial interface IServerRegistryNode
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(ServerRegistryLog value);
        /// <summary>
        /// 获取服务会话标识ID
        /// </summary>
        /// <returns></returns>
        long GetSessionID();
        /// <summary>
        /// 服务注册回调委托
        /// </summary>
        /// <param name="sessionID">服务会话标识ID</param>
        /// <param name="callback">服务注册日志回调委托</param>
        [ServerMethod(IsPersistence = false, IsCallbackClient = true)]
        void ServiceCallback(long sessionID, MethodKeepCallback<ServerRegistryOperationTypeEnum> callback);
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns>服务注册结果</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ServerRegistryStateEnum Append(ServerRegistryLog log);
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serverName">监视服务名称，空字符串表示所有服务</param>
        /// <param name="callback">服务注册日志回调委托，null 表示在线检查</param>
        [ServerMethod(IsPersistence = false, IsCallbackClient = true)]
#if NetStandard21
        void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog?> callback);
#else
        void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog> callback);
#endif
        /// <summary>
        /// 获取服务主日志
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns>返回 null 表示没有找到服务主日志</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ServerRegistryLog? GetLog(string serverName);
#else
        ServerRegistryLog GetLog(string serverName);
#endif
    }
}