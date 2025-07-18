﻿using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server registration node
    /// 服务注册节点
    /// </summary>
    public sealed class ServerRegistryNode : MethodParameterCreatorNode<IServerRegistryNode, ServerRegistryLog>, IServerRegistryNode, ISnapshot<ServerRegistryLog>
    {
        /// <summary>
        /// 默认冷启动会话 35 秒数超时，因为客户端连接超时可能是 30 秒
        /// </summary>
        internal const int DefaultLoadTimeoutSeconds = 35;

        /// <summary>
        /// 服务名称集合
        /// </summary>
        private readonly Dictionary<string, ServerRegistryLogAssembler> logAssemblers;
        /// <summary>
        /// 服务会话信息集合
        /// </summary>
        private readonly Dictionary<long, ServerRegistrySession> sessions;
        /// <summary>
        /// 服务注册日志回调委托集合
        /// </summary>
#if NetStandard21
        private LeftArray<MethodKeepCallback<ServerRegistryLog?>> logCallbacks;
#else
        private LeftArray<MethodKeepCallback<ServerRegistryLog>> logCallbacks;
#endif
        /// <summary>
        /// 初始化日志回调完成
        /// </summary>
        private readonly ServerRegistryLog loadedLog;
        /// <summary>
        /// 当前已分配服务会话标识ID
        /// </summary>
        private long currentSessionID;
        /// <summary>
        /// 冷启动会话超时时间戳
        /// </summary>
        internal long LoadTimeoutTimestamp;
        /// <summary>
        /// 冷启动会话超时秒数
        /// </summary>
        private readonly int loadTimeoutSeconds;
        /// <summary>
        /// Server registration node
        /// 服务注册节点
        /// </summary>
        /// <param name="loadTimeoutSeconds">Cold start session timeout seconds
        /// 冷启动会话超时秒数</param>
        public ServerRegistryNode(int loadTimeoutSeconds = DefaultLoadTimeoutSeconds)
        {
            logAssemblers = DictionaryCreator<string>.Create<ServerRegistryLogAssembler>();
            sessions = DictionaryCreator.CreateLong<ServerRegistrySession>();
#if NetStandard21
            logCallbacks = new LeftArray<MethodKeepCallback<ServerRegistryLog?>>(0);
#else
            logCallbacks = new LeftArray<MethodKeepCallback<ServerRegistryLog>>(0);
#endif
            loadedLog = new ServerRegistryLog(string.Empty, ServerRegistryOperationTypeEnum.CallbackLoaded);
            this.loadTimeoutSeconds = Math.Max(loadTimeoutSeconds, 1);
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override IServerRegistryNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IServerRegistryNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values) logAssembler.Loaded();
            LoadTimeoutTimestamp = Stopwatch.GetTimestamp() + Date.GetTimestampBySeconds(loadTimeoutSeconds);
            return null;
        }
        /// <summary>
        /// Processing operations after node removal
        /// 节点移除后的处理操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values) logAssembler.OnRemoved();
            logAssemblers.Clear();
            foreach (ServerRegistrySession session in sessions.Values) session.OnRemoved();
            sessions.Clear();
            foreach (var callback in logCallbacks) callback.CancelKeep();
            logCallbacks.SetEmpty();
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            int count = 1;
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values) count += logAssembler.SnapshotLogCount;
            return count;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        public SnapshotResult<ServerRegistryLog> GetSnapshotResult(ServerRegistryLog[] snapshotArray, object customObject)
        {
            SnapshotResult<ServerRegistryLog> result = new SnapshotResult<ServerRegistryLog>(snapshotArray.Length);
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values)
            {
                foreach(ServerRegistryLog log in logAssembler.SnapshotLogs) result.Add(snapshotArray, log);
            }
            result.Add(snapshotArray, new ServerRegistryLog(currentSessionID, string.Empty, ServerRegistryOperationTypeEnum.LoadCurrentSessionID));
            return result;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(ServerRegistryLog value)
        {
            if (value.OperationType != ServerRegistryOperationTypeEnum.LoadCurrentSessionID)
            {
                var session = default(ServerRegistrySession);
                if (!sessions.TryGetValue(value.SessionID, out session)) sessions.Add(value.SessionID, session = new ServerRegistrySession());
                string serverName = value.ServerName;
                var logAssembler = default(ServerRegistryLogAssembler);
                if (!logAssemblers.TryGetValue(serverName, out logAssembler)) logAssemblers.Add(serverName, logAssembler = new ServerRegistryLogAssembler(this, serverName));
                logAssembler.Load(session, value);
            }
            else currentSessionID = value.SessionID;
        }
        /// <summary>
        /// Get the server session identity
        /// 获取服务会话标识
        /// </summary>
        /// <returns></returns>
        public long GetSessionID()
        {
            return ++currentSessionID;
        }
        /// <summary>
        /// The server registration callback delegate is mainly used to register components to check the online state of the server
        /// 服务注册回调委托，主要用于注册组件检查服务的在线状态
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="callback">Server registration log operation type
        /// 服务注册日志操作类型</param>
        public void ServerCallback(long sessionID, MethodKeepCallback<ServerRegistryOperationTypeEnum> callback)
        {
            var session = default(ServerRegistrySession);
            if (sessions.TryGetValue(sessionID, out session)) session.Set(callback);
            else sessions.Add(sessionID, new ServerRegistrySession(callback));
        }
        /// <summary>
        /// Add the server registration log
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns>Server registration status
        /// 服务注册状态</returns>
        public ServerRegistryStateEnum Append(ServerRegistryLog log)
        {
            if (log == null) return ServerRegistryStateEnum.Unknown;
            string serverName = log.ServerName;
            if (string.IsNullOrEmpty(serverName)) return ServerRegistryStateEnum.UnsupportedServerName;
            switch (log.OperationType)
            {
                case ServerRegistryOperationTypeEnum.ClusterNode:
                case ServerRegistryOperationTypeEnum.ClusterMain:
                case ServerRegistryOperationTypeEnum.Singleton:
                    var session = default(ServerRegistrySession);
                    if (!sessions.TryGetValue(log.SessionID, out session))
                    {
                        if (StreamPersistenceMemoryDatabaseService.IsLoaded) return ServerRegistryStateEnum.NotFoundServerSessionID;
                        sessions.Add(log.SessionID, session = new ServerRegistrySession());
                    }
                    if (!session.IsCallback && StreamPersistenceMemoryDatabaseService.IsLoaded) return ServerRegistryStateEnum.NotFoundServerSessionCallback;
                    var logAssembler = default(ServerRegistryLogAssembler);
                    if (!logAssemblers.TryGetValue(serverName, out logAssembler)) logAssemblers.Add(serverName, logAssembler = new ServerRegistryLogAssembler(this, serverName));
                    return logAssembler.Append(session, log);
                case ServerRegistryOperationTypeEnum.Logout:
                    if (logAssemblers.TryGetValue(serverName, out logAssembler)) return logAssembler.Logout(log);
                    return ServerRegistryStateEnum.NotFoundServerName;
                default: return ServerRegistryStateEnum.UnrecognizableOperationType;
            }
        }
        /// <summary>
        /// Gets the server registration log
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serverName">Monitor the server name. An empty string represents all servers
        /// 监视服务名称，空字符串表示所有服务</param>
        /// <param name="callback">The server registration log returns null to indicate an online check
        /// 服务注册日志，返回 null 表示在线检查</param>
#if NetStandard21
        public void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog?> callback)
#else
        public void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog> callback)
#endif
        {
            if (string.IsNullOrEmpty(serverName))
            {
#if NetStandard21
                MethodKeepCallback<ServerRegistryLog?>.Callback(ref logCallbacks, null);
#else
                MethodKeepCallback<ServerRegistryLog>.Callback(ref logCallbacks, null);
#endif
                foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values)
                {
                    if (!logAssembler.Callback(callback)) return;
                }
                if (callback.Callback(loadedLog)) logCallbacks.Add(callback);
            }
            else
            {
                var logAssembler = default(ServerRegistryLogAssembler);
                if (!logAssemblers.TryGetValue(serverName, out logAssembler)) logAssemblers.Add(serverName, logAssembler = new ServerRegistryLogAssembler(this, serverName));
                logAssembler.Append(callback);
            }
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="log"></param>
        /// <param name="isPersistenceLostContact"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog?>> callbacks, ServerRegistryLog log, bool isPersistenceLostContact = true)
#else
        internal void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog>> callbacks, ServerRegistryLog log, bool isPersistenceLostContact = true)
#endif
        {
#if NetStandard21
            MethodKeepCallback<ServerRegistryLog?>.Callback(ref callbacks, log);
            MethodKeepCallback<ServerRegistryLog?>.Callback(ref logCallbacks, log);
#else
            MethodKeepCallback<ServerRegistryLog>.Callback(ref callbacks, log);
            MethodKeepCallback<ServerRegistryLog>.Callback(ref logCallbacks, log);
#endif
            if (log.OperationType == ServerRegistryOperationTypeEnum.LostContact && isPersistenceLostContact && StreamPersistenceMemoryDatabaseService.IsLoaded)
            {
                StreamPersistenceMemoryDatabaseMethodParameterCreator.LostContact(log.SessionID, log.ServerName);
            }
        }
        /// <summary>
        /// Get the main log of the server
        /// 获取服务主日志
        /// </summary>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        /// <returns>Returning null indicates that the server main log was not found
        /// 返回 null 表示没有找到服务主日志</returns>
#if NetStandard21
        public ServerRegistryLog? GetLog(string serverName)
#else
        public ServerRegistryLog GetLog(string serverName)
#endif
        {
            if (!string.IsNullOrEmpty(serverName))
            {
                var logAssembler = default(ServerRegistryLogAssembler);
                if (logAssemblers.TryGetValue(serverName, out logAssembler)) return logAssembler.MainLog?.Log;
            }
            return null;
        }
        /// <summary>
        /// Check the online status of the server
        /// 检查服务在线状态
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        /// <param name="isPersistenceLostContact"></param>
        private void check(long sessionID, string serverName, bool isPersistenceLostContact)
        {
            if (!string.IsNullOrEmpty(serverName))
            {
                var logAssembler = default(ServerRegistryLogAssembler);
                if (logAssemblers.TryGetValue(serverName, out logAssembler)) logAssembler.Check(sessionID, isPersistenceLostContact);
            }
        }
        /// <summary>
        /// Check the online status of the server
        /// 检查服务在线状态
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        public void Check(long sessionID, string serverName)
        {
            check(sessionID, serverName, true);
        }
        /// <summary>
        /// Persistent operations for server disconnection
        /// 服务失联的持久化操作
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        public void LostContact(long sessionID, string serverName)
        {
            check(sessionID, serverName, false);
        }
    }
}
