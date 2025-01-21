using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务注册日志组装
    /// </summary>
    internal sealed class ServerRegistryLogAssembler
    {
        /// <summary>
        /// 服务注册节点
        /// </summary>
        internal readonly ServerRegistryNode Node;
        /// <summary>
        /// 初始化日志回调完成
        /// </summary>
        private readonly ServerRegistryLog loadedLog;
        /// <summary>
        /// 附加日志集合
        /// </summary>
        private LeftArray<ServerRegistrySessionLog> logs;
        /// <summary>
        /// 主日志
        /// </summary>
#if NetStandard21
        internal ServerRegistrySessionLog? MainLog;
#else
        internal ServerRegistrySessionLog MainLog;
#endif
        /// <summary>
        /// 获取快照日志数量
        /// </summary>
        internal int SnapshotLogCount
        {
            get
            {
                if (MainLog != null) return logs.Length + 1;
                return 0;
            }
        }
        /// <summary>
        /// 获取快照日志集合
        /// </summary>
        internal IEnumerable<ServerRegistryLog> SnapshotLogs
        {
            get
            {
                if (MainLog != null)
                {
                    yield return MainLog.Log;
                    if (logs.Length != 0)
                    {
                        foreach (ServerRegistrySessionLog log in logs) yield return log.Log;
                    }
                }
            }
        }
        /// <summary>
        /// 服务注册日志回调委托集合
        /// </summary>
#if NetStandard21
        private LeftArray<MethodKeepCallback<ServerRegistryLog?>> callbacks;
#else
        private LeftArray<MethodKeepCallback<ServerRegistryLog>> callbacks;
#endif
        /// <summary>
        /// 注册服务最大版本号
        /// </summary>
        private uint maxVersion;
        /// <summary>
        /// 服务注册日志组装
        /// </summary>
        /// <param name="node"></param>
        /// <param name="serverName"></param>
        internal ServerRegistryLogAssembler(ServerRegistryNode node, string serverName)
        {
            this.Node = node;
            loadedLog = new ServerRegistryLog(serverName, ServerRegistryOperationTypeEnum.CallbackLoaded);
            logs = new LeftArray<ServerRegistrySessionLog>(0);
#if NetStandard21
            callbacks = new LeftArray<MethodKeepCallback<ServerRegistryLog?>>(0);
#else
            callbacks = new LeftArray<MethodKeepCallback<ServerRegistryLog>>(0);
#endif
        }
        /// <summary>
        /// 冷启动初始化加载数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Load(ServerRegistrySession session, ServerRegistryLog log)
        {
            if (MainLog == null) MainLog = new ServerRegistrySessionLog(session, log);
            else logs.Add(new ServerRegistrySessionLog(session, log));
        }
        /// <summary>
        /// 冷启动初始化完毕
        /// </summary>
        internal void Loaded()
        {
            if (MainLog != null && MainLog.Log.OperationType == ServerRegistryOperationTypeEnum.Singleton && logs.Length != 0)
            {
                ServerRegistrySessionLog log = logs.Array[0];
                AutoCSer.Threading.SecondTimer.TaskArray.Append(new ServerRegistryWaitOfflineQueueNode(this, MainLog, log).AppendQueueNode, (int)log.Log.TimeoutSeconds + 1);
            }
        }
        /// <summary>
        /// 获取日志索引位置
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        private int getLogIndex(long sessionID)
        {
            if (MainLog != null)
            {
                if (MainLog.Log.SessionID == sessionID) return -1;
                int count = logs.Length;
                if (count != 0)
                {
                    ServerRegistrySessionLog[] logArray = logs.Array;
                    do
                    {
                        if (logArray[--count].Log.SessionID == sessionID) return count;
                    }
                    while (count != 0);
                }
            }
            return int.MinValue;
        }
        /// <summary>
        /// 添加服务注册日志
        /// </summary>
        /// <param name="session"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        internal ServerRegistryStateEnum Append(ServerRegistrySession session, ServerRegistryLog log)
        {
            if (getLogIndex(log.SessionID) != int.MinValue) return ServerRegistryStateEnum.Success;
            uint versioin = log.Version;
            if (versioin < maxVersion) return ServerRegistryStateEnum.VersionTooLow;
            if (versioin != maxVersion)
            {
                logs.ClearLength();
                MainLog = null;
                maxVersion = versioin;
            }
            bool isLoaded = Node.StreamPersistenceMemoryDatabaseService.IsLoaded;
            if (isLoaded) checkSession();
            if (MainLog == null)
            {
                MainLog = new ServerRegistrySessionLog(session, log);
                if (isLoaded) Node.Callback(ref callbacks, log);
                return ServerRegistryStateEnum.Success;
            }
            ServerRegistryOperationTypeEnum operationType = log.OperationType;
            switch (operationType)
            {
                case ServerRegistryOperationTypeEnum.ClusterNode:
                case ServerRegistryOperationTypeEnum.ClusterMain:
                    switch (MainLog.Log.OperationType)
                    {
                        case ServerRegistryOperationTypeEnum.ClusterNode:
                        case ServerRegistryOperationTypeEnum.ClusterMain:
                            break;
                        default: return ServerRegistryStateEnum.OperationTypeConflict;
                    }
                    var sessionLog = new ServerRegistrySessionLog(session, log);
                    if (operationType == ServerRegistryOperationTypeEnum.ClusterNode) logs.Add(sessionLog);
                    else
                    {
                        logs.Add(MainLog);
                        MainLog = sessionLog;
                    }
                    if (isLoaded) Node.Callback(ref callbacks, log);
                    return ServerRegistryStateEnum.Success;
                case ServerRegistryOperationTypeEnum.Singleton:
                    if (MainLog.Log.OperationType != ServerRegistryOperationTypeEnum.Singleton) return ServerRegistryStateEnum.OperationTypeConflict;
                    if (isLoaded)
                    {
                        logs.TryPop(out sessionLog);
                        if (log.TimeoutSeconds > 0 && MainLog.Session.Offline()) log.TimeoutSeconds = 0;
                        if (log.TimeoutSeconds <= 0)
                        {
                            MainLog = new ServerRegistrySessionLog(session, log);
                            Node.Callback(ref callbacks, log);
                        }
                        else
                        {
                            logs.Add(sessionLog = new ServerRegistrySessionLog(session, log));
                            AutoCSer.Threading.SecondTimer.TaskArray.Append(new ServerRegistryWaitOfflineQueueNode(this, MainLog, sessionLog).AppendQueueNode, (int)log.TimeoutSeconds + 1);
                        }
                    }
                    else logs.Add(sessionLog = new ServerRegistrySessionLog(session, log));
                    return ServerRegistryStateEnum.Success;
                default: return ServerRegistryStateEnum.UnrecognizableOperationType;
            }
        }
        /// <summary>
        /// 单例服务超时强制下线
        /// </summary>
        /// <param name="mainLog"></param>
        /// <param name="log"></param>
        internal void SingletonTimeout(ServerRegistrySessionLog mainLog, ServerRegistrySessionLog log)
        {
            if (object.ReferenceEquals(mainLog, MainLog) && logs.Length != 0 && object.ReferenceEquals(log, logs.Array[0]))
            {
                log.Log.TimeoutSeconds = 0;
                MainLog = log;
                logs.PopOnly();
                Node.Callback(ref callbacks, log.Log);
            }
        }
        /// <summary>
        /// 添加服务注册日志回调委托
        /// </summary>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Append(MethodKeepCallback<ServerRegistryLog?> callback)
#else
        internal void Append(MethodKeepCallback<ServerRegistryLog> callback)
#endif
        {
            ServerRegistryNode.Callback(ref callbacks);
            if(Callback(callback)) callbacks.Add(callback);
        }
        /// <summary>
        /// 初始化服务注册日志回调委托
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Callback(MethodKeepCallback<ServerRegistryLog?> callback)
#else
        internal bool Callback(MethodKeepCallback<ServerRegistryLog> callback)
#endif
        {
            checkSession();
            if (MainLog != null)
            {
                if (!callback.Callback(MainLog.Log)) return false;
                if (MainLog.Log.OperationType != ServerRegistryOperationTypeEnum.Singleton)
                {
                    foreach (ServerRegistrySessionLog log in logs)
                    {
                        if (!callback.Callback(log.Log)) return false;
                    }
                }
            }
            return callback.Callback(loadedLog);
        }
        /// <summary>
        /// 服务会话在线检查
        /// </summary>
        private void checkSession()
        {
            int count = logs.Length;
            if (count != 0)
            {
                ServerRegistrySessionLog[] logArray = logs.Array;
                do
                {
                    ServerRegistrySessionLog sessionLog = logArray[--count];
                    if (!sessionLog.Session.Check(Node))
                    {
                        logs.RemoveAtToEnd(count);
                        Node.Callback(ref callbacks, sessionLog.Log.CreateLostContact());
                    }
                }
                while (count != 0);
            }
            if (MainLog != null && !MainLog.Session.Check(Node))
            {
                ServerRegistrySessionLog mainLog = MainLog;
                logs.TryPop(out MainLog);
                Node.Callback(ref callbacks, mainLog.Log.CreateLostContact());
            }
        }
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        internal ServerRegistryStateEnum Logout(ServerRegistryLog log)
        {
            int index = getLogIndex(log.SessionID);
            if (index == -1)
            {
                logs.TryPop(out MainLog);
                Node.Callback(ref callbacks, log);
                return ServerRegistryStateEnum.Success;
            }
            if (index >= 0)
            {
                logs.RemoveAtToEnd(index);
                Node.Callback(ref callbacks, log);
                return ServerRegistryStateEnum.Success;
            }
            return ServerRegistryStateEnum.NotFoundServerSessionID;
        }
        /// <summary>
        /// 数据库节点移除处理
        /// </summary>
        internal void OnRemoved()
        {
            foreach (var callback in callbacks) callback.CancelKeep();
        }
        /// <summary>
        /// 服务会话在线检查
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="isPersistenceLostContact"></param>
        internal void Check(long sessionID, bool isPersistenceLostContact)
        {
            int index = getLogIndex(sessionID);
            if (index == -1)
            {
                ServerRegistrySessionLog log = MainLog.notNull();
                if (!log.Session.Check(Node))
                {
                    logs.TryPop(out MainLog);
                    Node.Callback(ref callbacks, log.Log.CreateLostContact(), isPersistenceLostContact);
                }
            }
            else if (index >= 0)
            {
                ServerRegistrySessionLog log = logs.Array[index];
                if (!log.Session.Check(Node))
                {
                    logs.RemoveAtToEnd(index);
                    Node.Callback(ref callbacks, log.Log.CreateLostContact(), isPersistenceLostContact);
                }
            }
        }
    }
}
