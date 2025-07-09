using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if !AOT
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
#endif

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence memory database service
    /// 日志流持久化内存数据库服务
    /// </summary>
    public abstract class StreamPersistenceMemoryDatabaseServiceBase
    {
#if !AOT
        /// <summary>
        /// The default empty collection of nodes for the repair methods to be loaded
        /// 默认空待加载修复方法节点集合
        /// </summary>
        internal static readonly Dictionary<ulong, RepairNodeMethodLoader> NullRepairNodeMethodLoaders = AutoCSer.DictionaryCreator.CreateULong<RepairNodeMethodLoader>();
#endif

        /// <summary>
        /// Log stream persistence in memory database service configuration
        /// 日志流持久化内存数据库服务配置
        /// </summary>
        internal readonly StreamPersistenceMemoryDatabaseServiceConfig Config;
        /// <summary>
        /// The server synchronizes the read and write queues
        /// 服务端同步读写队列
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal CommandServerCallReadWriteQueue CommandServerCallQueue;
        /// <summary>
        /// The main directory of the persistent file
        /// 持久化文件主目录
        /// </summary>
        internal readonly DirectoryInfo PersistenceDirectory;
        /// <summary>
        /// Persistent file information
        /// 持久化文件信息
        /// </summary>
        internal FileInfo PersistenceFileInfo;
        /// <summary>
        /// Persistent callback exception location file information
        /// 持久化回调异常位置文件信息
        /// </summary>
        internal FileInfo PersistenceCallbackExceptionPositionFileInfo;
        /// <summary>
        /// Persistent rebuild file information
        /// 持久化重建文件信息
        /// </summary>
        internal FileInfo PersistenceSwitchFileInfo;
        /// <summary>
        /// Persistent callback exception location rebuild file information
        /// 持久化回调异常位置重建文件信息
        /// </summary>
        internal FileInfo PersistenceCallbackExceptionPositionSwitchFileInfo;
        /// <summary>
        /// Delete the historical persistence file
        /// 删除历史持久化文件
        /// </summary>
#if NetStandard21
        protected RemoveHistoryFile? removeHistoryFile;
#else
        protected RemoveHistoryFile removeHistoryFile;
#endif
        /// <summary>
        /// The collection of generate server-side nodes
        /// 生成服务端节点集合
        /// </summary>
        private readonly Dictionary<HashObject<Type>, ServerNodeCreator> nodeCreators;
        /// <summary>
        /// The access lock of the generate server node collection
        /// 生成服务端节点集合访问锁
        /// </summary>
        protected readonly object nodeCreatorLock;
        /// <summary>
        /// Write buffer for the persistent callback exception location
        /// 持久化回调异常位置写入缓冲区
        /// </summary>
        internal readonly byte[] PersistenceDataPositionBuffer;
        /// <summary>
        /// Root node collection
        /// 根节点集合
        /// </summary>
        protected readonly Dictionary<string, ServerNode> nodeDictionary;
        /// <summary>
        /// The keyword collection for the node being created
        /// 正在创建节点的关键字集合
        /// </summary>
        internal readonly Dictionary<string, CreatingNodeInfo> CreateNodes;
        /// <summary>
        /// The waiting event for the completion of the persistent callback
        /// 持久化回调完成的等待事件
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected ManualResetEvent serviceCallbackWait;
        /// <summary>
        /// Node collection
        /// 节点集合
        /// </summary>
        internal NodeIdentity[] Nodes;
        /// <summary>
        /// The currently executed calling method and parameter information
        /// 当前执行的调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal MethodParameter? CurrentMethodParameter;
#else
        internal MethodParameter CurrentMethodParameter;
#endif
        /// <summary>
        /// A linked list of persistent calls
        /// 持久化调用链表
        /// </summary>
        internal LinkStack<MethodParameter> PersistenceQueue;
        /// <summary>
        /// Persistent waiting events
        /// 持久化等待事件
        /// </summary>
        internal AutoCSer.Threading.OnceAutoWaitHandle PersistenceWaitHandle;
        /// <summary>
        /// Rebuild the log stream persistence file
        /// 重建日志流持久化文件
        /// </summary>
#if NetStandard21
        internal PersistenceRebuilder? Rebuilder;
#else
        internal PersistenceRebuilder Rebuilder;
#endif
#if !AOT
        /// <summary>
        /// The collection of nodes of the repair methods to be loaded
        /// 待加载修复方法节点集合
        /// </summary>
        internal Dictionary<ulong, RepairNodeMethodLoader> RepairNodeMethodLoaders;
        /// <summary>
        /// The loading repair node method has been loaded
        /// 已加载加载修复节点方法
        /// </summary>
#if NetStandard21
        internal RepairNodeMethod? LoadedRepairNodeMethod;
#else
        internal RepairNodeMethod LoadedRepairNodeMethod;
#endif
        /// <summary>
        /// Information from the node client
        /// 从节点客户端信息
        /// </summary>
#if NetStandard21
        internal ServiceSlave? Slave;
#else
        internal ServiceSlave Slave;
#endif
        /// <summary>
        /// Whether to allow the creation of slave nodes
        /// 是否允许创建从节点
        /// </summary>
        internal readonly bool CanCreateSlave;
#endif
        /// <summary>
        /// The starting position of persistent flow rebuild
        /// 持久化流重建起始位置
        /// </summary>
        internal ulong RebuildPosition;
        /// <summary>
        /// The written location of the persistent stream
        /// 持久化流已写入位置
        /// </summary>
        internal long PersistencePosition;
        /// <summary>
        /// The absolute end position of persistent flow rebuild (RebuildPosition + PersistencePosition)
        /// 持久化流重建绝对结束位置（RebuildPosition + PersistencePosition）
        /// </summary>
        internal ulong RebuildPersistenceEndPosition { get { return RebuildPosition + (ulong)PersistencePosition; } }
        /// <summary>
        /// The persistent callback exception location is the written location of the file
        /// 持久化回调异常位置文件已写入位置
        /// </summary>
        internal long PersistenceCallbackExceptionFilePosition;
        /// <summary>
        /// Rebuild the end position of the snapshot
        /// 重建快照结束位置
        /// </summary>
        public long RebuildSnapshotPosition { get; internal set; }
        /// <summary>
        /// The current allocated node index
        /// 当前分配节点索引
        /// </summary>
        internal int NodeIndex;
        /// <summary>
        /// The number of unfinished persistence callbacks
        /// 未完成持久化回调次数
        /// </summary>
        protected int serviceCallbackCount;
        /// <summary>
        /// Snapshot transaction relationship node version
        /// 快照事务关系节点版本
        /// </summary>
        internal int SnapshotTransactionNodeVersion;
        /// <summary>
        /// Is master node
        /// 是否主节点
        /// </summary>
        internal readonly bool IsMaster;
        /// <summary>
        /// Is Backup client
        /// 是否备份客户端
        /// </summary>
        internal virtual bool IsBackup { get { return false; } }
        /// <summary>
        /// Whether the current call is persistent
        /// 当前调用是否持久化
        /// </summary>
        internal bool CurrentCallIsPersistence;
        /// <summary>
        /// Has the data been loaded completely
        /// 是否已经加载完数据
        /// </summary>
        public bool IsLoaded { get; protected set; }
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        public bool IsDisposed { get; protected set; }
        /// <summary>
        /// Persistent type
        /// 持久化类型
        /// </summary>
        internal readonly PersistenceTypeEnum PersistenceType;
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
        /// <param name="config">Configuration of in-memory database service for log stream persistence
        /// 日志流持久化内存数据库服务配置</param>
        /// <param name="isMaster">Is master node
        /// 是否主节点</param>
        protected StreamPersistenceMemoryDatabaseServiceBase(StreamPersistenceMemoryDatabaseServiceConfig config, bool isMaster)
        {
            PersistenceFileInfo = config.GetPersistenceFileInfo();
            PersistenceDirectory = PersistenceFileInfo.Directory.notNull();
            if (!PersistenceDirectory.Exists) PersistenceDirectory.Create();
            PersistenceCallbackExceptionPositionFileInfo = new FileInfo(PersistenceFileInfo.FullName + StreamPersistenceMemoryDatabaseServiceConfig.PersistenceCallbackExceptionPositionExtensionName);
            var persistenceSwitchFileInfo = config.GetPersistenceSwitchFileInfo();
            if (persistenceSwitchFileInfo?.FullName == PersistenceFileInfo.FullName) persistenceSwitchFileInfo = null;
            if (persistenceSwitchFileInfo == null)
            {
                PersistenceSwitchFileInfo = PersistenceFileInfo;
                PersistenceCallbackExceptionPositionSwitchFileInfo = PersistenceCallbackExceptionPositionFileInfo;
            }
            else
            {
                PersistenceSwitchFileInfo = persistenceSwitchFileInfo;
                PersistenceCallbackExceptionPositionSwitchFileInfo = new FileInfo(PersistenceSwitchFileInfo.FullName + StreamPersistenceMemoryDatabaseServiceConfig.PersistenceCallbackExceptionPositionExtensionName);
                if (PersistenceSwitchFileInfo.Exists)
                {
                    if (!PersistenceFileInfo.Exists || PersistenceSwitchFileInfo.LastWriteTimeUtc > PersistenceFileInfo.LastWriteTimeUtc) SwitchPersistenceFileInfo();
                }
                else
                {
                    DirectoryInfo switchDirectory = PersistenceSwitchFileInfo.Directory.notNull();
                    if (!switchDirectory.Exists) switchDirectory.Create();
                }
            }

            Config = config;
            IsMaster = isMaster;
            PersistenceType = config.PersistenceType;
#if !AOT
            CanCreateSlave = (isMaster & config.CanCreateSlave) && PersistenceType == PersistenceTypeEnum.MemoryDatabase;
            RepairNodeMethodLoaders = AutoCSer.DictionaryCreator.CreateULong<RepairNodeMethodLoader>();
#endif
            NodeIndex = 1;
            CurrentCallIsPersistence = true;
            nodeDictionary = DictionaryCreator<string>.Create<ServerNode>();
            CreateNodes = DictionaryCreator<string>.Create<CreatingNodeInfo>();
            nodeCreatorLock = new object();
            nodeCreators = DictionaryCreator.CreateHashObject<Type, ServerNodeCreator>();
            PersistenceDataPositionBuffer = AutoCSer.Common.GetUninitializedArray<byte>(Math.Max(ServiceLoader.FileHeadSize, sizeof(long)));
            Nodes = new NodeIdentity[sizeof(int)];
        }
        /// <summary>
        /// Release node resources
        /// 释放节点资源
        /// </summary>
        internal void NodeDispose()
        {
            foreach(ServerNode node in nodeDictionary.Values) node.NodeDispose();
        }
        /// <summary>
        /// Get node information based on keywords
        /// 根据关键字获取节点信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ServerNode? GetNode(string key)
#else
        public ServerNode GetNode(string key)
#endif
        {
            var node = default(ServerNode);
            return nodeDictionary.TryGetValue(key, out node) ? node : null;
        }
        /// <summary>
        /// Set to delete historical persistent files
        /// 设置删除历史持久化文件
        /// </summary>
        /// <param name="removeHistoryFile"></param>
        /// <returns></returns>
        public bool Set(RemoveHistoryFile removeHistoryFile)
        {
            if (object.ReferenceEquals(this.removeHistoryFile, removeHistoryFile)) return false;
            removeHistoryFile?.Cancel();
            this.removeHistoryFile = removeHistoryFile;
            return true;
        }
        /// <summary>
        /// Switch the persistent file information
        /// 切换持久化文件信息
        /// </summary>
        internal void SwitchPersistenceFileInfo()
        {
            FileInfo switchFileInfo = PersistenceSwitchFileInfo;
            PersistenceSwitchFileInfo = PersistenceFileInfo;
            PersistenceFileInfo = switchFileInfo;
            switchFileInfo = PersistenceCallbackExceptionPositionSwitchFileInfo;
            PersistenceCallbackExceptionPositionSwitchFileInfo = PersistenceCallbackExceptionPositionFileInfo;
            PersistenceCallbackExceptionPositionFileInfo = switchFileInfo;
        }
        /// <summary>
        /// Get the location where the persistent stream has been written
        /// 获取持久化流已写入位置
        /// </summary>
        /// <returns>The written location of the persistent stream
        /// 持久化流已写入位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long GetPersistencePosition() { return PersistencePosition; }
        /// <summary>
        /// Gets the end location of the rebuild snapshot
        /// 获取重建快照结束位置
        /// </summary>
        /// <returns>Rebuild the end position of the snapshot
        /// 重建快照结束位置</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long GetRebuildSnapshotPosition() { return RebuildSnapshotPosition; }
        /// <summary>
        /// Add a new node
        /// 添加新节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns>The currently executed persistent call
        /// 当前执行的持久化调用</returns>
#if NetStandard21
        internal MethodParameter? AppendNode(ServerNode node, string key)
#else
        internal MethodParameter AppendNode(ServerNode node, string key)
#endif
        {
            nodeDictionary.Add(key, node);
            if (Nodes[node.Index.Index].Set(node, node.Index.Identity))
            {
                CreateNodes.Remove(key);
                if (PersistenceType == PersistenceTypeEnum.OnlyPersistence && IsLoaded && !CheckRebuild())
                {
                    AutoCSer.LogHelper.Error($"仅存档模式添加新介蒂安以后调用重建持久化操作失败。");
                }
            }
            else nodeDictionary.Remove(key);
            return CurrentMethodParameter;
        }
        /// <summary>
        /// Add the persistence call method and parameter information (The persistence API persists the request data first and then executes the request to ensure the reliability of persistence and avoid the situation where persistence fails and data is lost after the client is successfully fed back)
        /// 添加持久化调用方法与参数信息（持久化 API 先持久化请求数据再执行请求保证持久化的可靠性，避免出现反馈客户端成功以后出现持久化失败丢失数据的情况）
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <returns></returns>
        internal bool PushPersistenceMethodParameter(MethodParameter methodParameter)
        {
            if (PersistenceType != PersistenceTypeEnum.ScanPersistence)
            {
                if (PersistenceQueue.IsPushHead(methodParameter)) PersistenceWaitHandle.Set();
                if (IsDisposed) PersistenceException(PersistenceQueue.Get());
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add the persistence call method and parameter information (The persistence API persists the request data first and then executes the request to ensure the reliability of persistence and avoid the situation where persistence fails and data is lost after the client is successfully fed back)
        /// 添加持久化调用方法与参数信息（持久化 API 先持久化请求数据再执行请求保证持久化的可靠性，避免出现反馈客户端成功以后出现持久化失败丢失数据的情况）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodParameter"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal void PushPersistenceMethodParameter<T>(MethodParameter methodParameter, [MaybeNull] ref T callback)
#else
        internal void PushPersistenceMethodParameter<T>(MethodParameter methodParameter, ref T callback)
#endif
             where T : class
        {
            if (PersistenceType != PersistenceTypeEnum.ScanPersistence)
            {
                if (PersistenceQueue.IsPushHead(methodParameter))
                {
                    callback = null;
                    PersistenceWaitHandle.Set();
                }
                else callback = null;
                if (IsDisposed) PersistenceException(PersistenceQueue.Get());
            }
        }
        /// <summary>
        /// Set the currently executed calling method and parameter information
        /// 设置当前执行的调用方法与参数信息
        /// </summary>
        /// <param name="methodParameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetCurrentMethodParameter(MethodParameter methodParameter)
        {
            CurrentMethodParameter = methodParameter;
            CurrentCallIsPersistence = false;
        }
        /// <summary>
        /// Persistent callback
        /// 持久化回调
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="persistencePosition"></param>
        /// <param name="checkRebuild"></param>
#if NetStandard21
        internal void PersistenceCallback([MaybeNull]MethodParameter head, MethodParameter? end, long persistencePosition, bool checkRebuild)
#else
        internal void PersistenceCallback(MethodParameter head, MethodParameter end, long persistencePosition, bool checkRebuild)
#endif
        {
            CurrentCallIsPersistence = true;
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            try
            {
                do
                {
                    try
                    {
                        while (head != end)
                        {
                            CurrentMethodParameter = head;
                            head = head.notNull().PersistenceCallback();
                        }
                        return;
                    }
                    catch (Exception exception)
                    {
                        if (!exceptionRepeat.IsRepeat(exception)) AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    head = head.notNull().LinkNext;
                }
                while (head != end);
            }
            finally
            {
                if (Interlocked.Decrement(ref serviceCallbackCount) == 0) serviceCallbackWait.Set();
#if !AOT
                var slave = Slave;
                if (slave != null)
                {
                    do
                    {
                        if (slave.SetPersistencePosition(persistencePosition))
                        {
                            var left = slave;
                            var right = slave.LinkNext;
                            while (right != null)
                            {
                                if (right.SetPersistencePosition(persistencePosition))
                                {
                                    left = right;
                                    right = right.LinkNext;
                                }
                                else
                                {
                                    right = right.LinkNext;
                                    left.LinkNext = right;
                                }
                            }
                            break;
                        }
                    }
                    while ((slave = slave.LinkNext) != null);
                    Slave = slave;
                }
#endif
                if (Rebuilder == null && checkRebuild) CheckRebuild();
            }
        }
        /// <summary>
        /// Rebuild the persistent file
        /// 重建持久化文件
        /// </summary>
        /// <returns></returns>
        internal virtual bool CheckRebuild() { return false; }
        /// <summary>
        /// Persistence exception, Release the call queue
        /// 持久化异常释放调用队列
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="state"></param>
#if NetStandard21
        internal void PersistenceException(MethodParameter? head, MethodParameter? end = null, CallStateEnum state = CallStateEnum.Disposed)
#else
        internal void PersistenceException(MethodParameter head, MethodParameter end = null, CallStateEnum state = CallStateEnum.Disposed)
#endif
        {
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            do
            {
                try
                {
                    while (head != end) head = head.notNull().PersistenceCallback(state);
                    return;
                }
                catch (Exception exception)
                {
                    if (!exceptionRepeat.IsRepeat(exception)) AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                head = head.notNull().LinkNext;
            }
            while (head != end);
        }
        /// <summary>
        /// Write the location of the persistent callback exception data
        /// 写入持久化回调异常数据位置
        /// </summary>
        /// <param name="persistenceCallbackExceptionPosition">持久化异常位置信息</param>
        internal unsafe void WritePersistenceCallbackExceptionPosition(long persistenceCallbackExceptionPosition)
        {
            fixed (byte* buffer = PersistenceDataPositionBuffer) *(long*)buffer = persistenceCallbackExceptionPosition;
            using (FileStream positionStream = new FileStream(PersistenceCallbackExceptionPositionFileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4 << 10, FileOptions.None))
            {
                positionStream.Seek(0, SeekOrigin.End);
                positionStream.Write(PersistenceDataPositionBuffer, 0, sizeof(long));
            }
            PersistenceCallbackExceptionFilePosition += sizeof(long);
#if !AOT
            var slave = Slave;
            if (slave != null)
            {
                do
                {
                    if (slave.AppendPersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition))
                    {
                        var left = slave;
                        var right = slave.LinkNext;
                        while (right != null)
                        {
                            if (right.AppendPersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition))
                            {
                                left = right;
                                right = right.LinkNext;
                            }
                            else
                            {
                                right = right.LinkNext;
                                left.LinkNext = right;
                            }
                        }
                        break;
                    }
                }
                while ((slave = slave.LinkNext) != null);
                Slave = slave;
            }
#endif
        }
        /// <summary>
        /// Get the generate server-side node
        /// 获取生成服务端节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal ServerNodeCreator GetNodeCreator<T>()
        {
            var nodeCreator = default(ServerNodeCreator);
            HashObject<Type> type = typeof(T);
            Monitor.Enter(nodeCreatorLock);
            if (nodeCreators.TryGetValue(type, out nodeCreator))
            {
                Monitor.Exit(nodeCreatorLock);
                return nodeCreator;
            }
            try
            {
                nodeCreators.Add(type, nodeCreator = ServerNodeCreator<T>.Create(this));
            }
            finally { Monitor.Exit(nodeCreatorLock); }
            return nodeCreator;
        }
#if AOT
        ///// <summary>
        ///// 添加待加载修复方法节点方法信息
        ///// </summary>
        //private static readonly MethodInfo appendRepairNodeMethodLoaderMethod = typeof(StreamPersistenceMemoryDatabaseServiceBase).GetMethod(nameof(AppendRepairNodeMethodLoader), BindingFlags.Static | BindingFlags.NonPublic).notNull();
#else
        /// <summary>
        /// Add the repair interface method file
        /// 添加修复接口方法文件
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        /// <returns></returns>
        internal async Task AppendRepairNodeMethod(RepairNodeMethod repairNodeMethod)
        {
            DirectoryInfo typeDirectory = new DirectoryInfo(Path.Combine(PersistenceDirectory.FullName, Config.RepairNodeMethodDirectoryName, repairNodeMethod.TypeDirectoryName));
            await AutoCSer.Common.TryCreateDirectory(typeDirectory);
            DirectoryInfo methodDirectory = new DirectoryInfo(Path.Combine(typeDirectory.FullName, repairNodeMethod.MethodDirectoryName));
            DirectoryInfo backupMethodDirectory = new DirectoryInfo(methodDirectory.FullName + ".bak");
            await AutoCSer.Common.TryCreateDirectory(backupMethodDirectory);
            FileInfo assemblyFile = new FileInfo(Path.Combine(backupMethodDirectory.FullName, Config.RepairNodeMethodAssemblyFileName));
#if NetStandard21
            await using (FileStream assemblyStream = await AutoCSer.Common.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#else
            using (FileStream assemblyStream = await AutoCSer.Common.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#endif
            {
                SubArray<byte> rawAssembly = repairNodeMethod.RawAssembly;
                await assemblyStream.WriteAsync(rawAssembly.Array, rawAssembly.Start, rawAssembly.Length);
            }
            assemblyFile.LastWriteTimeUtc = repairNodeMethod.RepairNodeMethodFile.LastWriteTime;
            FileInfo methodNameFile = new FileInfo(Path.Combine(backupMethodDirectory.FullName, Config.RepairNodeMethodNameFileName));
            await AutoCSer.Common.WriteFileAllText(methodNameFile.FullName, AutoCSer.JsonSerializer.Serialize(repairNodeMethod.MethodName), Encoding.UTF8);
            methodNameFile.LastWriteTimeUtc = repairNodeMethod.RepairNodeMethodFile.LastWriteTime;
            await AutoCSer.Common.TryDeleteDirectory(methodDirectory);
            await AutoCSer.Common.DirectoryMove(backupMethodDirectory, methodDirectory.FullName);
            if (!IsBackup)
            {
                var type = default(Type);
                if (repairNodeMethod.RemoteType.TryGet(out type))
                {
                    //appendRepairNodeMethodLoaderMethod.MakeGenericMethod(type).Invoke(null, new object[] { this, methodDirectory, repairNodeMethod.RepairNodeMethodDirectory });
                    GenericType.Get(type).AppendRepairNodeMethodLoader(this, methodDirectory, repairNodeMethod.RepairNodeMethodDirectory);
                }
                else await AutoCSer.LogHelper.Fatal($"没有找到节点类型 {repairNodeMethod.RemoteType.AssemblyName} + {repairNodeMethod.RemoteType.Name}");
            }
        }
        /// <summary>
        /// Add the node of the repair method to be loaded
        /// 添加待加载修复方法节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="methodDirectory"></param>
        /// <param name="repairNodeMethodDirectory"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendRepairNodeMethodLoader<T>(StreamPersistenceMemoryDatabaseServiceBase service, DirectoryInfo methodDirectory, RepairNodeMethodDirectory repairNodeMethodDirectory)
        {
            service.appendRepairNodeMethodLoader<T>(methodDirectory, ref repairNodeMethodDirectory);
        }
        /// <summary>
        /// Add the node of the repair method to be loaded
        /// 添加待加载修复方法节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodDirectory"></param>
        /// <param name="repairNodeMethodDirectory"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendRepairNodeMethodLoader<T>(DirectoryInfo methodDirectory, ref RepairNodeMethodDirectory repairNodeMethodDirectory)
        {
            AppendRepairNodeMethodLoader(ServerNodeCreator.GetMethodDirectoryPosition(methodDirectory), new RepairNodeMethodLoader<T>(GetNodeCreator<T>(), methodDirectory, ref repairNodeMethodDirectory));
        }
        /// <summary>
        /// Add the node of the repair method to be loaded
        /// 添加待加载修复方法节点
        /// </summary>
        /// <param name="position"></param>
        /// <param name="loader"></param>
        internal void AppendRepairNodeMethodLoader(ulong position, RepairNodeMethodLoader loader)
        {
            Monitor.Enter(nodeCreatorLock);
            try
            {
                RepairNodeMethodLoaders.TryGetValue(position, out loader.LinkNext);
                RepairNodeMethodLoaders[position] = loader;
            }
            finally { Monitor.Exit(nodeCreatorLock); }
        }
        /// <summary>
        /// Add the loaded repair node method
        /// 添加已加载修复节点方法
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        internal void AppendLoadedRepairNodeMethod(RepairNodeMethod repairNodeMethod)
        {
            repairNodeMethod.LinkNext = LoadedRepairNodeMethod;
            LoadedRepairNodeMethod = repairNodeMethod;
            var slave = Slave;
            if (slave != null)
            {
                do
                {
                    if (slave.AppendRepairNodeMethod(repairNodeMethod))
                    {
                        var left = slave;
                        var right = slave.LinkNext;
                        while (right != null)
                        {
                            if (right.AppendRepairNodeMethod(repairNodeMethod))
                            {
                                left = right;
                                right = right.LinkNext;
                            }
                            else
                            {
                                right = right.LinkNext;
                                left.LinkNext = right;
                            }
                        }
                        break;
                    }
                }
                while ((slave = slave.LinkNext) != null);
                Slave = slave;
            }
        }
        /// <summary>
        /// Close the data load
        /// 关闭数据加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="isRetry"></param>
        internal abstract void CloseLoader(SlaveLoader loader, bool isRetry);
#endif

        static StreamPersistenceMemoryDatabaseServiceBase()
        {
#if !AOT
            AutoCSer.Common.Config.AppendRemoteType(typeof(ServerByteArrayMessage));
            AutoCSer.Common.Config.AppendRemoteType(typeof(BinaryMessage<>));
#endif
            //AutoCSer.Common.Config.AppendRemoteType(typeof(HashBytes));
            //AutoCSer.Common.Config.AppendRemoteType(typeof(HashSubString));
            //AutoCSer.Common.Config.AppendRemoteType(typeof(AutoCSer.SearchTree.Set<>));
            //AutoCSer.Common.Config.AppendRemoteType(typeof(AutoCSer.SearchTree.PageArray<>));
            AutoCSer.Common.Config.AppendRemoteTypeAssembly(typeof(HashBytes).Assembly);
            //AutoCSer.Common.Config.AppendRemoteTypeAssembly(typeof(StreamPersistenceMemoryDatabaseServiceBase));
        }
    }
}
