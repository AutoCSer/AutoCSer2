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
    /// 服务注册节点
    /// </summary>
    public sealed class ServerRegistryNode : ContextNode<IServerRegistryNode, ServerRegistryLog>, IServerRegistryNode, ISnapshot<ServerRegistryLog>
    {
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
        /// 服务注册节点
        /// </summary>
        /// <param name="loadTimeoutSeconds">冷启动会话超时秒数</param>
        public ServerRegistryNode(int loadTimeoutSeconds = 30)
        {
            logAssemblers = DictionaryCreator.CreateAny<string, ServerRegistryLogAssembler>();
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
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IServerRegistryNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IServerRegistryNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values) logAssembler.Loaded();
            LoadTimeoutTimestamp = Stopwatch.GetTimestamp() + Date.GetTimestampBySeconds(loadTimeoutSeconds);
            return this;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            int count = 1;
            foreach (ServerRegistryLogAssembler logAssembler in logAssemblers.Values) count += logAssembler.SnapshotLogCount;
            return count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
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
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
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
        /// 获取服务会话标识ID
        /// </summary>
        /// <returns></returns>
        public long GetSessionID()
        {
            return ++currentSessionID;
        }
        /// <summary>
        /// 服务注册回调委托
        /// </summary>
        /// <param name="sessionID">服务会话标识ID</param>
        /// <param name="callback">服务注册日志回调委托</param>
        public void ServiceCallback(long sessionID, MethodKeepCallback<ServerRegistryOperationTypeEnum> callback)
        {
            var session = default(ServerRegistrySession);
            if (sessions.TryGetValue(sessionID, out session)) session.Set(callback);
            else sessions.Add(sessionID, new ServerRegistrySession(callback));
        }
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns>服务注册结果</returns>
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
                    if (!sessions.TryGetValue(log.SessionID, out session)) return ServerRegistryStateEnum.NotFoundServerSessionID;
                    if (!session.IsCallback) return ServerRegistryStateEnum.NotFoundServerSessionCallback;
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
        /// 获取服务注册日志
        /// </summary>
        /// <param name="serverName">监视服务名称，空字符串表示所有服务</param>
        /// <param name="callback">服务注册日志回调委托，null 表示在线检查</param>
#if NetStandard21
        public void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog?> callback)
#else
        public void LogCallback(string serverName, MethodKeepCallback<ServerRegistryLog> callback)
#endif
        {
            if (string.IsNullOrEmpty(serverName))
            {
                Callback(ref logCallbacks);
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog?>> callbacks, ServerRegistryLog log)
#else
        internal void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog>> callbacks, ServerRegistryLog log)
#endif
        {
            callback(ref callbacks, log);
            callback(ref logCallbacks, log);
        }
        /// <summary>
        /// 服务注册日志回调在线检查
        /// </summary>
        /// <param name="callbacks"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog?>> callbacks)
#else
        internal static void Callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog>> callbacks)
#endif
        {
            callback(ref callbacks, null);
        }
        /// <summary>
        /// 服务注册日志回调
        /// </summary>
        /// <param name="callbacks"></param>
        /// <param name="log"></param>
#if NetStandard21
        private static void callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog?>> callbacks, ServerRegistryLog? log)
#else
        private static void callback(ref LeftArray<MethodKeepCallback<ServerRegistryLog>> callbacks, ServerRegistryLog log)
#endif
        {
            int count = callbacks.Length;
            if (count != 0)
            {
                var callbackArray = callbacks.Array;
                do
                {
                    if (!callbackArray[--count].Callback(log)) callbacks.RemoveAtToEnd(count);
                }
                while (count != 0);
            }
        }
        /// <summary>
        /// 获取服务主日志
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <returns>返回 null 表示没有找到服务主日志</returns>
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
    }
}