using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端会话绑定日志流持久化内存数据库服务
    /// </summary>
    public abstract class CommandServerSocketSessionObjectService
    {
        /// <summary>
        /// 日志流持久化内存数据库服务配置
        /// </summary>
        private static readonly StreamPersistenceMemoryDatabaseConfig streamPersistenceMemoryDatabaseConfig = ((ConfigObject<StreamPersistenceMemoryDatabaseConfig>)AutoCSer.Configuration.Common.Get(typeof(StreamPersistenceMemoryDatabaseConfig)))?.Value ?? new StreamPersistenceMemoryDatabaseConfig();
        /// <summary>
        /// 单服务实例
        /// </summary>
        private static CommandServerSocketSessionObjectService singleService;
        /// <summary>
        /// 是否进程内单服务
        /// </summary>
        private static readonly bool isSingleService = streamPersistenceMemoryDatabaseConfig.IsSingleService;
        /// <summary>
        /// 是否输出反序列化错误日志
        /// </summary>
        private static bool isDeserializeLog = true;
        /// <summary>
        /// 服务数量
        /// </summary>
        private static int serviceCount;

        /// <summary>
        /// 日志流持久化内存数据库服务端配置
        /// </summary>
        internal readonly StreamPersistenceMemoryDatabaseServiceConfig Config;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        internal CommandServerCallQueue CommandServerCallQueue;
        /// <summary>
        /// 持久化文件信息
        /// </summary>
        public readonly FileInfo PersistenceFileInfo;
        /// <summary>
        /// 持久化回调异常位置文件信息
        /// </summary>
        public readonly FileInfo PersistenceCallbackExceptionPositionFileInfo;
        /// <summary>
        /// 生成服务端节点集合
        /// </summary>
        private readonly Dictionary<HashObject<Type>, ServerNodeCreator> nodeCreators;
        /// <summary>
        /// 生成服务端节点集合访问锁
        /// </summary>
        protected readonly object nodeCreatorLock;
        /// <summary>
        /// 待加载修复方法节点集合
        /// </summary>
        internal Dictionary<ulong, RepairNodeMethodLoader> RepairNodeMethodLoaders;
        /// <summary>
        /// 持久化回调异常位置写入缓冲区
        /// </summary>
        internal readonly byte[] PersistenceDataPositionBuffer;
        /// <summary>
        /// 根节点集合
        /// </summary>
        protected readonly Dictionary<HashString, ServerNode> nodeDictionary;
        /// <summary>
        /// 正在创建节点的关键字
        /// </summary>
        protected readonly HashSet<HashString> createKeys;
        /// <summary>
        /// 持久化回调完成等待
        /// </summary>
        protected ManualResetEvent serviceCallbackWait;
        /// <summary>
        /// 节点集合
        /// </summary>
        internal NodeIdentity[] Nodes;
        /// <summary>
        /// 当前执行的调用方法与参数信息
        /// </summary>
        internal MethodParameter CurrentMethodParameter;
        /// <summary>
        /// 调用持久化链表
        /// </summary>
        internal MethodParameter.YieldQueue PersistenceQueue;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal AutoCSer.Threading.OnceAutoWaitHandle PersistenceWaitHandle;
        /// <summary>
        /// 日志流持久化文件重建
        /// </summary>
        internal PersistenceRebuilder Rebuilder;
        /// <summary>
        /// 从节点客户端信息
        /// </summary>
        internal ServiceSlave Slave;
        /// <summary>
        /// 已加载加载修复节点方法
        /// </summary>
        internal RepairNodeMethod LoadedRepairNodeMethod;
        /// <summary>
        /// 持久化流重建起始位置
        /// </summary>
        internal ulong RebuildPosition;
        /// <summary>
        /// 持久化流已写入位置
        /// </summary>
        internal long PersistencePosition;
        /// <summary>
        /// 持久化流重建绝对结束位置 rebuildPosition + persistencePosition
        /// </summary>
        internal ulong RebuildPersistenceEndPosition { get { return RebuildPosition + (ulong)PersistencePosition; } }
        /// <summary>
        /// 持久化回调异常位置文件已写入位置
        /// </summary>
        internal long PersistenceCallbackExceptionFilePosition;
        /// <summary>
        /// 当前分配节点索引
        /// </summary>
        internal int NodeIndex;
        /// <summary>
        /// 未完成持久化回调次数
        /// </summary>
        protected int serviceCallbackCount;
        /// <summary>
        /// 快照事务关系节点版本
        /// </summary>
        internal int SnapshotTransactionNodeVersion;
        /// <summary>
        /// 是否主节点
        /// </summary>
        internal readonly bool IsMaster;
        /// <summary>
        /// 是否允许创建从节点
        /// </summary>
        internal readonly bool CanCreateSlave;
        /// <summary>
        /// 是否备份客户端
        /// </summary>
        internal virtual bool IsBackup { get { return false; } }
        /// <summary>
        /// 当前调用是否持久化
        /// </summary>
        internal bool CurrentCallIsPersistence;
        /// <summary>
        /// 是否已经加载完数据
        /// </summary>
        internal bool IsLoaded;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal bool IsDisposed;
        /// <summary>
        /// 服务端会话绑定日志流持久化内存数据库服务
        /// </summary>
        /// <param name="config">日志流持久化内存数据库服务端配置</param>
        /// <param name="isMaster">是否主节点</param>
        protected CommandServerSocketSessionObjectService(StreamPersistenceMemoryDatabaseServiceConfig config, bool isMaster)
        {
            if (Interlocked.Increment(ref serviceCount) == 1)
            {
                if (isSingleService) singleService = this;
            }
            else if (isSingleService)
            {
                throw new InvalidOperationException($"当前配置 {typeof(StreamPersistenceMemoryDatabaseConfig).fullName()}.{nameof(streamPersistenceMemoryDatabaseConfig.IsSingleService)} 为进程内单服务，不允许生成多个日志流持久化内存数据库服务");
            }

            PersistenceFileInfo = new FileInfo(config.PersistenceFileName);
            if (!string.Equals(PersistenceFileInfo.Extension, StreamPersistenceMemoryDatabaseServiceConfig.PersistenceExtensionName, StringComparison.OrdinalIgnoreCase))
            {
                PersistenceFileInfo = new FileInfo(PersistenceFileInfo.FullName + StreamPersistenceMemoryDatabaseServiceConfig.PersistenceExtensionName);
            }
            PersistenceCallbackExceptionPositionFileInfo = new FileInfo(PersistenceFileInfo.FullName + StreamPersistenceMemoryDatabaseServiceConfig.PersistenceCallbackExceptionPositionExtensionName);

            Config = config;
            IsMaster = isMaster;
            CanCreateSlave = isMaster & config.CanCreateSlave;
            NodeIndex = 1;
            CurrentCallIsPersistence = true;
            nodeDictionary = DictionaryCreator.CreateHashString<ServerNode>();
            createKeys = HashSetCreator.CreateHashString();
            nodeCreatorLock = new object();
            nodeCreators = DictionaryCreator.CreateHashObject<Type, ServerNodeCreator>();
            RepairNodeMethodLoaders = AutoCSer.Extensions.DictionaryCreator.CreateULong<RepairNodeMethodLoader>();
            PersistenceDataPositionBuffer = AutoCSer.Common.Config.GetArray(Math.Max(ServiceLoader.FileHeadSize, sizeof(long)));
            Nodes = new NodeIdentity[sizeof(int)];
        }
        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns>当前执行的持久化回调</returns>
        internal MethodParameter AppendNode(ServerNode node, string key)
        {
            HashString hashKey = key;
            nodeDictionary.Add(hashKey, node);
            if (Nodes[node.Index.Index].Set(node, node.Index.Identity)) createKeys.Remove(hashKey);
            else nodeDictionary.Remove(hashKey);
            return CurrentMethodParameter;
        }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal InputMethodParameter CreateInputMethodParameter(NodeIndex index, int methodIndex, out CallStateEnum state)
        {
            if ((uint)index.Index < (uint)NodeIndex)
            {
                ServerNode node = Nodes[index.Index].CheckGet(index.Identity);
                if (node != null) return node.CreateInputMethodParameter(methodIndex, out state);
                state = CallStateEnum.NodeIdentityNotMatch;
            }
            else state = CallStateEnum.NodeIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// 添加持久化调用方法与参数信息
        /// </summary>
        /// <param name="methodParameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushPersistenceMethodParameter(MethodParameter methodParameter)
        {
            if (PersistenceQueue.IsPushHead(methodParameter)) PersistenceWaitHandle.Set();
            if (IsDisposed) PersistenceException(PersistenceQueue.GetClear());
        }
        /// <summary>
        /// 添加持久化调用方法与参数信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodParameter"></param>
        /// <param name="callback"></param>
        internal void PushPersistenceMethodParameter<T>(MethodParameter methodParameter, ref T callback)
             where T : class
        {
            if (PersistenceQueue.IsPushHead(methodParameter))
            {
                callback = null;
                PersistenceWaitHandle.Set();
            }
            else callback = null;
            if (IsDisposed) PersistenceException(PersistenceQueue.GetClear());
        }
        /// <summary>
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
        /// 持久化回调
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="persistencePosition"></param>
        internal void PersistenceCallback(MethodParameter head, MethodParameter end, long persistencePosition)
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
                            head = head.PersistenceCallback();
                        }
                        return;
                    }
                    catch (Exception exception)
                    {
                        if (!exceptionRepeat.IsRepeat(exception)) AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    head = head.LinkNext;
                }
                while (head != end);
            }
            finally
            {
                if (Interlocked.Decrement(ref serviceCallbackCount) == 0) serviceCallbackWait.Set();
                ServiceSlave slave = Slave;
                if (slave != null)
                {
                    do
                    {
                        if (slave.SetPersistencePosition(persistencePosition))
                        {
                            ServiceSlave left = slave, right = slave.LinkNext;
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
            }
        }
        /// <summary>
        /// 持久化结束释放队列
        /// </summary>
        /// <param name="head"></param>
        /// <param name="end"></param>
        /// <param name="state"></param>
        internal void PersistenceException(MethodParameter head, MethodParameter end = null, CallStateEnum state = CallStateEnum.Disposed)
        {
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            do
            {
                try
                {
                    while (head != end) head = head.PersistenceCallback(state);
                    return;
                }
                catch (Exception exception)
                {
                    if (!exceptionRepeat.IsRepeat(exception)) AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                }
                head = head.LinkNext;
            }
            while (head != end);
        }
        /// <summary>
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

            ServiceSlave slave = Slave;
            if (slave != null)
            {
                do
                {
                    if (slave.AppendPersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition))
                    {
                        ServiceSlave left = slave, right = slave.LinkNext;
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
        }
        /// <summary>
        /// 添加修复接口方法文件
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        /// <returns></returns>
        internal async Task AppendRepairNodeMethod(RepairNodeMethod repairNodeMethod)
        {
            DirectoryInfo typeDirectory = new DirectoryInfo(Path.Combine(PersistenceFileInfo.Directory.FullName, Config.RepairNodeMethodDirectoryName, repairNodeMethod.TypeDirectoryName));
            await AutoCSer.Common.Config.TryCreateDirectory(typeDirectory);
            DirectoryInfo methodDirectory = new DirectoryInfo(Path.Combine(typeDirectory.FullName, repairNodeMethod.MethodDirectoryName));
            DirectoryInfo backupMethodDirectory = new DirectoryInfo(methodDirectory.FullName + ".bak");
            await AutoCSer.Common.Config.TryCreateDirectory(backupMethodDirectory);
            FileInfo assemblyFile = new FileInfo(Path.Combine(backupMethodDirectory.FullName, Config.RepairNodeMethodAssemblyFileName));
#if DotNet45 || NetStandard2
            using (FileStream assemblyStream = await AutoCSer.Common.Config.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#else
            await using (FileStream assemblyStream = await AutoCSer.Common.Config.CreateFileStream(assemblyFile.FullName, FileMode.Create, FileAccess.Write))
#endif
            {
                SubArray<byte> rawAssembly = repairNodeMethod.RawAssembly;
                await assemblyStream.WriteAsync(rawAssembly.Array, rawAssembly.Start, rawAssembly.Length);
            }
            assemblyFile.LastWriteTimeUtc = repairNodeMethod.RepairNodeMethodFile.LastWriteTime;
            FileInfo methodNameFile = new FileInfo(Path.Combine(backupMethodDirectory.FullName, Config.RepairNodeMethodNameFileName));
            await AutoCSer.Common.Config.WriteFileAllText(methodNameFile.FullName, AutoCSer.JsonSerializer.Serialize(repairNodeMethod.MethodName), Encoding.UTF8);
            methodNameFile.LastWriteTimeUtc = repairNodeMethod.RepairNodeMethodFile.LastWriteTime;
            await AutoCSer.Common.Config.TryDeleteDirectory(methodDirectory);
            await AutoCSer.Common.Config.DirectoryMove(backupMethodDirectory, methodDirectory.FullName);
            if (!IsBackup)
            {
                Type type;
                if (repairNodeMethod.RemoteType.TryGet(out type)) GenericType.Get(type).AppendRepairNodeMethodLoader(this, methodDirectory, repairNodeMethod.RepairNodeMethodDirectory);
                else await AutoCSer.LogHelper.Fatal($"没有找到节点类型 {repairNodeMethod.RemoteType.AssemblyName} + {repairNodeMethod.RemoteType.Name}");
            }
        }
        /// <summary>
        /// 添加待加载修复方法节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="methodDirectory"></param>
        /// <param name="repairNodeMethodDirectory"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void AppendRepairNodeMethodLoader<T>(CommandServerSocketSessionObjectService service, DirectoryInfo methodDirectory, RepairNodeMethodDirectory repairNodeMethodDirectory)
        {
            service.appendRepairNodeMethodLoader<T>(methodDirectory, ref repairNodeMethodDirectory);
        }
        /// <summary>
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
        /// 获取生成服务端节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal ServerNodeCreator GetNodeCreator<T>()
        {
            ServerNodeCreator nodeCreator;
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
        /// <summary>
        /// 添加已加载加载修复节点方法
        /// </summary>
        /// <param name="repairNodeMethod"></param>
        internal void AppendLoadedRepairNodeMethod(RepairNodeMethod repairNodeMethod)
        {
            repairNodeMethod.LinkNext = LoadedRepairNodeMethod;
            LoadedRepairNodeMethod = repairNodeMethod;

            ServiceSlave slave = Slave;
            if (slave != null)
            {
                do
                {
                    if (slave.AppendRepairNodeMethod(repairNodeMethod))
                    {
                        ServiceSlave left = slave, right = slave.LinkNext;
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
        /// 关闭数据加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="isRetry"></param>
        internal abstract void CloseLoader(SlaveLoader loader, bool isRetry);

        /// <summary>
        /// 从反序列化上下文中获取日志流持久化内存数据库服务端对象
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        internal static CommandServerSocketSessionObjectService GetSessionObject(AutoCSer.BinaryDeserializer deserializer)
        {
            CommandServerSocketSessionObjectService service = singleService;
            if (service != null) return service;
            CommandServerSocket socket = (CommandServerSocket)deserializer.Context;
            ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService> sessionObject = socket.SessionObject as ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>;
            if (sessionObject != null)
            {
                service = sessionObject.TryGetSessionObject(socket);
                if (service != null) return service;
                isDeserializeLog = false;
                socket.Server.Log.ErrorIgnoreException($"日志流持久化内存数据库服务无法从套接字自定义会话对象中获取日志流持久化内存数据库服务端对象，请确认类型 {socket.SessionObject.GetType().fullName()} 是否正确实现 {typeof(ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>).fullName()}.{nameof(ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>.CreateSessionObject)}", LogLevelEnum.Error | LogLevelEnum.Fatal);
            }
            else if (isDeserializeLog)
            {
                isDeserializeLog = false;
                if (socket.SessionObject == null) socket.Server.Log.ErrorIgnoreException($"日志流持久化内存数据库服务缺少套接字自定义会话对象，请在初始化阶段调用 {typeof(IStreamPersistenceMemoryDatabaseClient).fullName()}.{nameof(IStreamPersistenceMemoryDatabaseClient.CreateSessionObject)}", LogLevelEnum.Error | LogLevelEnum.Fatal);
                else socket.Server.Log.ErrorIgnoreException($"日志流持久化内存数据库服务套接字自定义会话对象类型错误 {socket.SessionObject.GetType().fullName()} 未实现接口 {typeof(ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService>).fullName()}", LogLevelEnum.Error | LogLevelEnum.Fatal);
            }
            return null;
        }
    }
}
