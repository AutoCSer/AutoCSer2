﻿using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence memory database service
    /// 日志流持久化内存数据库服务
    /// </summary>
    public class StreamPersistenceMemoryDatabaseService : StreamPersistenceMemoryDatabaseServiceBase, ICommandServerBindController, IDisposable
#if !AOT
        , IStreamPersistenceMemoryDatabaseService, IReadWriteQueueService
#endif
    {
        /// <summary>
        /// Persistent buffer pool
        /// 持久化缓冲区池
        /// </summary>
        internal readonly ByteArrayPool PersistenceBufferPool;
#if !AOT
        /// <summary>
        /// After the persistent rebuild is completed, turn off the node and wait for the event
        /// 持久化重建完毕关闭从节点等待事件
        /// </summary>
        private System.Threading.AutoResetEvent rebuildCompletedWaitHandle;
        /// <summary>
        /// The timestamp identifier of the last generated slave node
        /// 最后一次生成的从节点时间戳标识
        /// </summary>
        private long slaveClientTimestamp;
#endif
        /// <summary>
        /// The head node of the persistent queue has not been processed
        /// 未处理持久化队列头节点
        /// </summary>
#if NetStandard21
        private MethodParameter? persistenceHead;
#else
        private MethodParameter persistenceHead;
#endif
        /// <summary>
        /// The tail node of the persistent queue has not been processed
        /// 未处理持久化队列尾节点
        /// </summary>
#if NetStandard21
        private MethodParameter? persistenceEnd;
#else
        private MethodParameter persistenceEnd;
#endif
        /// <summary>
        /// Persistent file header version information
        /// 持久化文件头部版本信息
        /// </summary>
        internal uint PersistenceFileHeadVersion;
        /// <summary>
        /// Persistent callback exception location file header version information
        /// 持久化回调异常位置文件头部版本信息
        /// </summary>
        internal uint PersistenceCallbackExceptionPositionFileHeadVersion;
        /// <summary>
        /// Free index collection
        /// 空闲索引集合
        /// </summary>
        private LeftArray<int> freeIndexs;
        /// <summary>
        /// Persistently rebuild and load abnormal node
        /// 持久化重建加载异常节点
        /// </summary>
#if NetStandard21
        private ServerNode? rebuildLoadExceptionNode;
#else
        private ServerNode rebuildLoadExceptionNode;
#endif
        /// <summary>
        /// Is the rebuild persistence waiting for operation
        /// 重建持久化是否正在等待操作
        /// </summary>
        private bool isRebuilderPersistenceWaitting;
        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
        /// <param name="config">Configuration of in-memory database service for log stream persistence
        /// 日志流持久化内存数据库服务配置</param>
        /// <param name="createServiceNode">The delegate that creates the underlying operation node for the service
        /// 创建服务基础操作节点委托</param>
        /// <param name="isMaster">Is master node
        /// 是否主节点</param>
        internal unsafe StreamPersistenceMemoryDatabaseService(StreamPersistenceMemoryDatabaseServiceConfig config, Func<StreamPersistenceMemoryDatabaseService, ServerNode> createServiceNode, bool isMaster) : base(config, isMaster)
        {
            freeIndexs = new LeftArray<int>(sizeof(int));
            Nodes[0].SetFreeIdentity(ServiceNode.ServiceNodeIndex.Identity);
            PersistenceBufferPool = ByteArrayPool.GetPool((BufferSizeBitsEnum)Math.Max((byte)BufferSizeBitsEnum.Kilobyte4, (byte)config.BufferSizeBits));
            createServiceNode(this);
            //createInputMethodParameter = new CreateInputMethodParameter(Nodes[0].Node.notNull());
#if !AOT
            rebuildCompletedWaitHandle = AutoCSer.Common.NullAutoResetEvent;
#endif
            if (isMaster) PersistenceWaitHandle.Set(new object());
        }
        /// <summary>
        /// Load data
        /// </summary>
        internal unsafe void Load()
        {
            switch (PersistenceType)
            {
                case PersistenceTypeEnum.MemoryDatabase:
                case PersistenceTypeEnum.OnlyPersistence:
                    if (PersistenceFileInfo.Exists)
                    {
                        if (PersistenceCallbackExceptionPositionFileInfo.Exists)
                        {
                            if (PersistenceType == PersistenceTypeEnum.OnlyPersistence)
                            {
                                new ServiceLoader(this).Load();
                                while (File.Exists(PersistenceFileInfo.FullName + Config.GetBackupFileNameSuffix() + ".bak")) System.Threading.Thread.Sleep(100);//避免文件名称冲突
                                string backupFileNameSuffix = Config.GetBackupFileNameSuffix() + ".rb";
                                FileInfo persistenceCallbackExceptionPositionFileInfo = new FileInfo(PersistenceCallbackExceptionPositionSwitchFileInfo.FullName + backupFileNameSuffix);
                                FileInfo persistenceFileInfo = new FileInfo(PersistenceSwitchFileInfo.FullName + backupFileNameSuffix);
                                long persistencePosition;
                                fixed (byte* bufferFixed = PersistenceDataPositionBuffer)
                                {
                                    *(uint*)bufferFixed = ServiceLoader.PersistenceCallbackExceptionPositionFileHead;
                                    *(ulong*)(bufferFixed + sizeof(int)) = RebuildPersistenceEndPosition;
                                    using (FileStream fileStream = persistenceCallbackExceptionPositionFileInfo.Create()) fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize);
                                    *(uint*)bufferFixed = ServiceLoader.FieHead;
                                    *(long*)(bufferFixed + (sizeof(int) + sizeof(ulong))) = 0;
                                    using (FileStream fileStream = persistenceFileInfo.Create())
                                    {
                                        fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);

                                        var outputSerializer = default(BinarySerializer);
                                        PersistenceBuffer persistenceBuffer = new PersistenceBuffer(this);
                                        bool isSerializeCopyString = Config.IsSerializeCopyString;
                                        try
                                        {
                                            persistenceBuffer.GetBufferLength();
                                            using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetDefaultContext(CommandServerSocket.CommandServerSocketContext, ref isSerializeCopyString))
                                            {
                                                persistenceBuffer.OutputStream = outputStream;
                                                for (int index = NodeIndex; index > 1;)
                                                {
                                                    var node = Nodes[--index].Node;
                                                    if (node != null)
                                                    {
                                                        fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                                                        {
                                                            persistenceBuffer.SetStart(dataFixed);
                                                            persistenceBuffer.Reset();
                                                            node.CreateNodeMethodParameter.notNull().PersistenceSerialize(outputSerializer, fileStream.Position);
                                                            SubArray<byte> outputData = persistenceBuffer.GetData();
                                                            fileStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            persistenceBuffer.Free();
                                            outputSerializer?.FreeContext(isSerializeCopyString);
                                        }
                                        persistencePosition = fileStream.Position;
                                    }
                                }
                                backupFileNameSuffix = Config.GetBackupFileNameSuffix() + ".bak";
                                File.Move(PersistenceFileInfo.FullName, PersistenceFileInfo.FullName + backupFileNameSuffix);
                                File.Move(PersistenceCallbackExceptionPositionFileInfo.FullName, PersistenceCallbackExceptionPositionFileInfo.FullName + backupFileNameSuffix);
                                File.Move(persistenceCallbackExceptionPositionFileInfo.FullName, PersistenceCallbackExceptionPositionSwitchFileInfo.FullName);
                                File.Move(persistenceFileInfo.FullName, PersistenceSwitchFileInfo.FullName);
                                SetRebuild(persistencePosition, ServiceLoader.ExceptionPositionFileHeadSize, persistencePosition);
                            }
                            long startTimestamp = Stopwatch.GetTimestamp(), count = new ServiceLoader(this).Load();
                            //if (count != 0) Console.WriteLine($"初始化加载 {count} 条持久化数据耗时 {AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp)}ms");
                            nodeLoaded();
                        }
                        else throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetNotFoundExceptionPositionFile(PersistenceCallbackExceptionPositionFileInfo.FullName));
                    }
                    else
                    {
                        if (PersistenceCallbackExceptionPositionFileInfo.Exists)
                        {
                            File.Move(PersistenceCallbackExceptionPositionFileInfo.FullName, PersistenceCallbackExceptionPositionFileInfo.FullName + Config.GetBackupFileNameSuffix() + ".bak");
                        }
                        fixed (byte* bufferFixed = PersistenceDataPositionBuffer)
                        {
                            *(uint*)bufferFixed = PersistenceCallbackExceptionPositionFileHeadVersion = ServiceLoader.PersistenceCallbackExceptionPositionFileHead;
                            *(ulong*)(bufferFixed + sizeof(int)) = 0;
                            using (FileStream fileStream = PersistenceCallbackExceptionPositionFileInfo.Create()) fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize);
                            *(uint*)bufferFixed = PersistenceFileHeadVersion = ServiceLoader.FieHead;
                            *(long*)(bufferFixed + (sizeof(int) + sizeof(ulong))) = 0;
                            using (FileStream fileStream = PersistenceFileInfo.Create()) fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);
                        }
                        PersistenceCallbackExceptionFilePosition = ServiceLoader.ExceptionPositionFileHeadSize;
                        PersistencePosition = ServiceLoader.FileHeadSize;
                    }
                    IsLoaded = true;
                    serviceCallbackWait = new ManualResetEvent(true);
#if !AOT
                    rebuildCompletedWaitHandle = new AutoResetEvent(false);
                    RepairNodeMethodLoaders = NullRepairNodeMethodLoaders;
#endif
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(persistence);
                    Config.RemoveHistoryFile(this);
                    return;
                case PersistenceTypeEnum.ScanPersistence:
                    if (!PersistenceFileInfo.Exists) throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetNotFoundPersistenceFile(PersistenceFileInfo.FullName));
                    if (!PersistenceCallbackExceptionPositionFileInfo.Exists) throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetNotFoundExceptionPositionFile(PersistenceCallbackExceptionPositionFileInfo.FullName));
                    string persistenceFileName = PersistenceFileInfo.Name, exceptionPositionFileName = PersistenceCallbackExceptionPositionFileInfo.Name;
                    DirectoryInfo switchDirectory = PersistenceSwitchFileInfo.Directory.notNull();
                    Dictionary<ulong, PersistenceFile> persistenceFiles = DictionaryCreator.CreateULong<PersistenceFile>();
                    FileInfo[] bakFiles = PersistenceDirectory.GetFiles("*.bak"), switchFiles = PersistenceDirectory.FullName != switchDirectory.FullName ? switchDirectory.GetFiles("*.bak") : EmptyArray<FileInfo>.Array;
                    fixed (byte* bufferFixed = PersistenceDataPositionBuffer)
                    {
                        getPersistenceFile(bakFiles, persistenceFileName, exceptionPositionFileName, persistenceFiles, bufferFixed);
                        getPersistenceFile(switchFiles, persistenceFileName, exceptionPositionFileName, persistenceFiles, bufferFixed);
                    }
                    LeftArray<PersistenceFile> persistenceFileArray = new LeftArray<PersistenceFile>(persistenceFiles.Count);
                    foreach (PersistenceFile persistenceFile in persistenceFiles.Values)
                    {
                        if (persistenceFile.FileInfo != null && persistenceFile.CallbackExceptionPositionFileInfo != null) persistenceFileArray.Add(persistenceFile);
                    }
                    ScanPersistenceServiceLoader serviceLoader = new ScanPersistenceServiceLoader(this);
                    if (persistenceFileArray.Length != 0)
                    {
                        persistenceFileArray.Sort(PersistenceFile.Comparer);
                        foreach (PersistenceFile persistenceFile in persistenceFileArray) serviceLoader.Load(persistenceFile.FileInfo.notNull(), persistenceFile.CallbackExceptionPositionFileInfo.notNull());
                    }
                    serviceLoader.Load(PersistenceFileInfo, PersistenceCallbackExceptionPositionFileInfo);
                    nodeLoaded();
                    IsLoaded = true;
                    return;
                default: throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetServiceLoaderFailed(CallStateEnum.UnknownPersistenceType, string.Empty, 0, 0));
            }
        }
        /// <summary>
        /// Get persistent file information
        /// 获取持久化文件信息
        /// </summary>
        /// <param name="files"></param>
        /// <param name="persistenceFileName"></param>
        /// <param name="exceptionPositionFileName"></param>
        /// <param name="persistenceFiles"></param>
        /// <param name="bufferFixed"></param>
        private unsafe void getPersistenceFile(FileInfo[] files, string persistenceFileName, string exceptionPositionFileName, Dictionary<ulong, PersistenceFile> persistenceFiles, byte* bufferFixed)
        {
            foreach (FileInfo file in files)
            {
                if (file.Length >= ServiceLoader.ExceptionPositionFileHeadSize)
                {
                    string fileName = file.Name;
                    if (fileName.StartsWith(persistenceFileName, StringComparison.Ordinal))
                    {
                        ulong rebuildPosition;
                        PersistenceFile persistenceFile;
                        if (fileName.StartsWith(exceptionPositionFileName, StringComparison.Ordinal))
                        {
                            using (FileStream fileStream = file.OpenRead())
                            {
                                if (fileStream.Read(PersistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize) != ServiceLoader.ExceptionPositionFileHeadSize)
                                {
                                    throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetServiceLoaderExceptionPositionFileHeaderNotMatch(file.FullName));
                                }
                                if (persistenceFiles.TryGetValue(rebuildPosition = *(ulong*)(bufferFixed + sizeof(int)), out persistenceFile))
                                {
                                    if (persistenceFile.CallbackExceptionPositionFileInfo != null)
                                    {
                                        throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetServiceLoaderExceptionPositionFileHeaderNotMatch(file.FullName));
                                    }
                                }
                                else persistenceFile.RebuildPosition = rebuildPosition;
                                persistenceFile.CallbackExceptionPositionFileInfo = file;
                            }
                        }
                        else
                        {
                            using (FileStream fileStream = file.OpenRead())
                            {
                                if (fileStream.Read(PersistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize) != ServiceLoader.ExceptionPositionFileHeadSize)
                                {
                                    throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetServiceLoaderFileHeaderNotMatch(file.FullName));
                                }
                                if (persistenceFiles.TryGetValue(rebuildPosition = *(ulong*)(bufferFixed + sizeof(int)), out persistenceFile))
                                {
                                    if (persistenceFile.FileInfo != null)
                                    {
                                        throw new Exception(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture.Configuration.Default.GetServiceLoaderFileHeaderNotMatch(file.FullName));
                                    }
                                }
                                else persistenceFile.RebuildPosition = rebuildPosition;
                                persistenceFile.FileInfo = file;
                            }
                        }
                        persistenceFiles[rebuildPosition] = persistenceFile;
                    }
                }
            }
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        private void nodeLoaded()
        {
            LeftArray<Task> loadedTasks = new LeftArray<Task>(NodeIndex - 1);
            for (int index = NodeIndex; index > 1;)
            {
                var node = Nodes[--index].Node;
                if (node != null)
                {
                    Task task = node.Loaded();
                    if (!task.IsCompleted) loadedTasks.Add(task);
                }
            }
            if (loadedTasks.Count != 0) load(loadedTasks).Wait();
        }
        /// <summary>
        /// Wait for the loading data
        /// 等待加载数据
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private async Task load(LeftArray<Task> tasks)
        {
            int count = tasks.Count;
            foreach (Task task in tasks.Array)
            {
                await task;
                if (--count == 0) return;
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                dispose();

                PersistenceWaitHandle.Set();
                PersistenceException(PersistenceQueue.Get());
                serviceCallbackWait?.Dispose();
#if !AOT
                rebuildCompletedWaitHandle.setDispose();
#endif
            }
        }
        /// <summary>
        /// Release resources
        /// </summary>
        private void dispose()
        {
            IsDisposed = true;

            if (Rebuilder != null) CommandServerCallQueue.AppendWriteOnly(new PersistenceRebuilderCallback(Rebuilder, PersistenceRebuilderCallbackTypeEnum.Close));
            CommandServerCallQueue.AppendWriteOnly(new ServiceCallback(this, ServiceCallbackTypeEnum.NodeDispose));
            removeHistoryFile?.Cancel();
        }
        /// <summary>
        /// Get the server UTC time
        /// 获取服务端 UTC 时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetUtcNow()
        {
            return AutoCSer.Threading.SecondTimer.SetUtcNow();
        }
        /// <summary>
        /// Gets the collection of all valid nodes (excluding the base operation nodes)
        /// 获取所有有效节点集合（不包括基础操作节点）
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ServerNode> GetNodes()
        {
            for (int index = NodeIndex; index > 1;)
            {
                var node = Nodes[--index].Node;
                if (node != null) yield return node;
            }
        }
        ///// <summary>
        ///// 创建调用方法与参数信息
        ///// </summary>
        //private InputMethodParameter createInputMethodParameter;
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal InputMethodParameter? CreateInputMethodParameter(NodeIndex index, int methodIndex, out CallStateEnum state)
#else
        internal InputMethodParameter CreateInputMethodParameter(NodeIndex index, int methodIndex, out CallStateEnum state)
#endif
        {
            //var parameter = createInputMethodParameter.Clone(index, methodIndex);
            //if (parameter != null && object.ReferenceEquals(parameter.Node, Nodes[index.Index].Node))
            //{
            //    state = CallStateEnum.Success;
            //    return parameter;
            //}
            if ((uint)index.Index < (uint)NodeIndex)
            {
                var node = Nodes[index.Index].Node;
                if (node != null && node.Check(index))
                {
                    //parameter = node.CreateInputMethodParameter(methodIndex, out state);
                    //if (parameter != null) createInputMethodParameter = parameter;
                    return node.CreateInputMethodParameter(methodIndex, out state);
                }
                state = CallStateEnum.NodeIdentityNotMatch;
            }
            else state = CallStateEnum.NodeIndexOutOfRange;
            return null;
        }
        /// <summary>
        /// Bind the command service controller
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void ICommandServerBindController.Bind(CommandServerController controller)
        {
            CommandServerCallQueue = controller.CallConcurrencyReadQueue;
            if (object.ReferenceEquals(controller.Server.CallConcurrencyReadQueue, CommandServerCallQueue)) CommandServerCallQueue = controller.CallReadWriteQueue;
            if (IsMaster) CommandServerCallQueue.AppendWriteOnly(new ServiceCallback(this, ServiceCallbackTypeEnum.Load));
        }
        /// <summary>
        /// Get server node based on node global keywords
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerNode? GetServerNode(string key)
#else
        internal ServerNode GetServerNode(string key)
#endif
        {
            var node = default(ServerNode);
            return nodeDictionary.TryGetValue(key, out node) ? node : null;
        }
        /// <summary>
        /// Set the write location of the persistent file
        /// 设置持久化文件写入位置
        /// </summary>
        /// <param name="persistencePosition"></param>
        /// <param name="persistenceCallbackExceptionFilePosition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetPersistencePosition(long persistencePosition, long persistenceCallbackExceptionFilePosition)
        {
            PersistencePosition = persistencePosition;
            PersistenceCallbackExceptionFilePosition = persistenceCallbackExceptionFilePosition;
        }
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        internal NodeIndex GetNodeIndex(string key, NodeInfo nodeInfo, bool isCreate)
        {
            if (key != null)
            {
                var node = default(ServerNode);
                if (nodeDictionary.TryGetValue(key, out node)) return check(node, nodeInfo);
                if (isCreate)
                {
                    var creatingNodeInfo = default(CreatingNodeInfo);
                    if (!CreateNodes.TryGetValue(key, out creatingNodeInfo))
                    {
                        int index = GetFreeIndex();
                        CreateNodes.Add(key, new CreatingNodeInfo(index, nodeInfo));
                        return new NodeIndex(index, Nodes[index].GetFreeIdentity());
                    }
                    if (creatingNodeInfo.Check(nodeInfo)) return new NodeIndex(creatingNodeInfo.Index, Nodes[creatingNodeInfo.Index].Identity);
                    return new NodeIndex(CallStateEnum.NodeTypeNotMatch);
                }
                return new NodeIndex(CallStateEnum.NotFoundNodeKey);
            }
            return new NodeIndex(CallStateEnum.NullKey);
        }
        /// <summary>
        /// Get node identity (Check the input parameters before the persistence operation)
        /// 获取节点标识（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        internal ValueResult<NodeIndex> GetNodeIndexBeforePersistence(string key, NodeInfo nodeInfo, bool isCreate)
        {
            if (key != null)
            {
                var node = default(ServerNode);
                if (nodeDictionary.TryGetValue(key, out node)) return check(node, nodeInfo);
                if (isCreate)
                {
                    var creatingNodeInfo = default(CreatingNodeInfo);
                    if (!CreateNodes.TryGetValue(key, out creatingNodeInfo)) return default(ValueResult<NodeIndex>);
                    if (creatingNodeInfo.Check(nodeInfo)) return new NodeIndex(creatingNodeInfo.Index, Nodes[creatingNodeInfo.Index].Identity);
                    return new NodeIndex(CallStateEnum.NodeTypeNotMatch);
                }
                return new NodeIndex(CallStateEnum.NotFoundNodeKey);
            }
            return new NodeIndex(CallStateEnum.NullKey);
        }
        /// <summary>
        /// Create node identity
        /// 创建节点标识
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal NodeIndex CreateNodeIndex()
        {
            int index = GetFreeIndex();
            return new NodeIndex(index, Nodes[index].GetCreateIdentity());
        }
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        public virtual NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, string key, NodeInfo nodeInfo, bool isCreate)
        {
            return nodeInfo != null ? GetNodeIndex(key, nodeInfo, isCreate) : new NodeIndex(CallStateEnum.NullNodeInfo);
        }
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="isCreate">Create a free node identity when the keyword does not exist
        /// 关键字不存在时创建空闲节点标识</param>
        /// <returns>When the keyword does not exist, return an free node identifier for creating the node
        /// 关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        public virtual NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallWriteQueue queue, string key, NodeInfo nodeInfo, bool isCreate)
        {
            return nodeInfo != null ? GetNodeIndex(key, nodeInfo, isCreate) : new NodeIndex(CallStateEnum.NullNodeInfo);
        }
        /// <summary>
        /// Check whether the node information matches
        /// 检查节点信息是否匹配
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        private NodeIndex check(ServerNode node, NodeInfo nodeInfo)
        {
            CallStateEnum state = node.CallState;
            if (state == CallStateEnum.Success)
            {
                if(nodeInfo.RemoteType.Equals(new RemoteType(node.NodeCreator.Type))) return node.Index;
                return new NodeIndex(CallStateEnum.NodeTypeNotMatch);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Check whether the node identification matches before creating the node
        /// 创建节点之前检查节点标识是否匹配
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        internal NodeIndex CheckCreateNodeIndex(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            if (key != null)
            {
                var node = default(ServerNode);
                if (nodeDictionary.TryGetValue(key, out node)) return check(node, nodeInfo);
                if ((uint)index.Index < (uint)NodeIndex)
                {
                    if (Nodes[index.Index].CheckFreeIdentity(ref index.Identity)) return index;
                    return new NodeIndex(CallStateEnum.NodeIdentityNotMatch);
                }
                return new NodeIndex(CallStateEnum.NodeIndexOutOfRange);
            }
            return new NodeIndex(CallStateEnum.NullKey);
        }
        /// <summary>
        /// Initialize and load data to create nodes
        /// 初始化加载数据创建节点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        internal void LoadCreateNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            if (index.Index >= Nodes.Length) Nodes = AutoCSer.Common.GetCopyArray(Nodes, Math.Max(Nodes.Length << 1, index.Index + 1));
            while (NodeIndex < index.Index) freeIndexs.Add(NodeIndex++);
            if (NodeIndex == index.Index) ++NodeIndex;
            else
            {
                int removeIndex = freeIndexs.IndexOf(index.Index);
                if (removeIndex >= 0) freeIndexs.RemoveToEnd(removeIndex);
            }
            if (Nodes[index.Index].SetFreeIdentity(index.Identity)) CreateNodes.Add(key, new CreatingNodeInfo(index.Index, nodeInfo));
        }
        ///// <summary>
        ///// 删除节点（持久化操作之前检查输入参数）
        ///// </summary>
        ///// <param name="index">节点索引信息</param>
        ///// <returns>无返回值表示需要继续调用持久化方法</returns>
        //public ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index)
        //{
        //    if (Nodes[index.Index].CheckGet(index.Identity) == null) return false;
        //    freeIndexs.PrepLength(1);
        //    return default(ValueResult<bool>);
        //}
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public bool RemoveNode(NodeIndex index)
        {
            if (index.Index != 0)
            {
                var node = Nodes[index.Index].GetRemove(index.Identity);
                if (node != null)
                {
                    nodeDictionary.Remove(node.Key);
                    freeIndexs.Add(index.Index);
                    try
                    {
                        node.OnRemoved();
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public bool RemoveNode(string key)
        {
            var node = default(ServerNode);
            return nodeDictionary.TryGetValue(key, out node) && RemoveNode(node.Index);
        }
        ///// <summary>
        ///// 获取服务端节点
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal Node GetNode(NodeIndex index)
        //{
        //    return nodes[index.Index].Get(index.Identity);
        //}
        /// <summary>
        /// Get the node index
        /// 获取节点索引
        /// </summary>
        /// <returns></returns>
        internal int GetFreeIndex()
        {
            int index;
            if (freeIndexs.TryPop(out index)) return index;
            if (NodeIndex == Nodes.Length) Nodes = AutoCSer.Common.GetCopyArray(Nodes, NodeIndex << 1);
            return NodeIndex++;
        }
        /// <summary>
        /// Release free node
        /// 释放空闲节点
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveFreeIndex(NodeIndex index)
        {
            freeIndexs.PrepLength(1);
            if (Nodes[index.Index].FreeIdentity(index.Identity))
            {
                freeIndexs.Add(index.Index);
            }
        }
        /// <summary>
        /// Gets the global keyword for all matching nodes
        /// 获取所有匹配节点的全局关键字
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetNodeKeys(NodeInfo nodeInfo, CommandServerKeepCallbackCount<string> callback)
        {
            if (nodeInfo != null)
            {
                foreach (ServerNode node in GetNodes())
                {
                    if (nodeInfo.RemoteType.Equals(new RemoteType(node.NodeCreator.Type)))
                    {
                        if (!await callback.CallbackAsync(node.Key)) return;
                    }
                }
            }
        }
        /// <summary>
        /// Gets the node index information for all matching nodes
        /// 获取所有匹配节点的节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetNodeIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<NodeIndex> callback)
        {
            if (nodeInfo != null)
            {
                foreach (ServerNode node in GetNodes())
                {
                    if (nodeInfo.RemoteType.Equals(new RemoteType(node.NodeCreator.Type)))
                    {
                        if (!await callback.CallbackAsync(node.Index)) return;
                    }
                }
            }
        }
        /// <summary>
        /// Gets the global keyword and node index information of all matching nodes
        /// 获取所有匹配节点的全局关键字与节点索引信息
        /// </summary>
        /// <param name="nodeInfo">The server-side node information to be matched
        /// 待匹配的服务端节点信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetNodeKeyIndexs(NodeInfo nodeInfo, CommandServerKeepCallbackCount<BinarySerializeKeyValue<string, NodeIndex>> callback)
        {
            if (nodeInfo != null)
            {
                foreach (ServerNode node in GetNodes())
                {
                    if (nodeInfo.RemoteType.Equals(new RemoteType(node.NodeCreator.Type)))
                    {
                        if (!await callback.CallbackAsync(new BinarySerializeKeyValue<string, NodeIndex>(node.Key, node.Index))) return;
                    }
                }
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        internal void Call(NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        var node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            state = node.Call(methodIndex, ref refCallback);
                            return;
                        }
                        state = CallStateEnum.NodeIdentityNotMatch;
                    }
                    else state = CallStateEnum.NodeIndexOutOfRange;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.SynchronousCallback(state); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Call(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            Call(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Call(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            Call(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            Call(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            Call(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        internal void CallOutput(NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        var node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            state = node.CallOutput(methodIndex, ref refCallback);
                            return;
                        }
                        else state = CallStateEnum.NodeIdentityNotMatch;
                    }
                    else state = CallStateEnum.NodeIndexOutOfRange;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.SynchronousCallback(ResponseParameter.CallStates[(byte)state]); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallOutput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallOutput(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallOutput(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallOutput(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallOutputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallOutput(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallOutputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallOutput(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        public void CallInput(ref RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((CallInputMethodParameter)parameter.MethodParameter.notNull()).CallInput(ref refCallback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.SynchronousCallback(state); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallInput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInput(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallInput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallInput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallInput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        public void CallInputOutput(ref RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((CallInputOutputMethodParameter)parameter.MethodParameter.notNull()).CallInputOutput(ref refCallback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.SynchronousCallback(ResponseParameter.CallStates[(byte)state]); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputOutput(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallInputOutput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputOutput(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallInputOutput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The callback of reutrn parameter
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputOutputWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallInputOutput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputOutputWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallInputOutput(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter)
        {
            if (!IsDisposed && parameter.CallState == CallStateEnum.Success) ((SendOnlyMethodParameter)parameter.MethodParameter.notNull()).SendOnly();
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter)
        {
            if (!IsDisposed && parameter.CallState == CallStateEnum.Success) ((SendOnlyMethodParameter)parameter.MethodParameter.notNull()).SendOnly();
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly SendOnlyWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter)
        {
            if (!IsDisposed && parameter.CallState == CallStateEnum.Success) ((SendOnlyMethodParameter)parameter.MethodParameter.notNull()).SendOnly();
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly SendOnlyWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter)
        {
            if (!IsDisposed && parameter.CallState == CallStateEnum.Success) ((SendOnlyMethodParameter)parameter.MethodParameter.notNull()).SendOnly();
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        internal void KeepCallback(NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        var node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            state = node.KeepCallback(methodIndex, ref refCallback);
                            return;
                        }
                        else state = CallStateEnum.NodeIdentityNotMatch;
                    }
                    else state = CallStateEnum.NodeIndexOutOfRange;
                }
                else state = CallStateEnum.Disposed;
            }
            finally
            {
                refCallback?.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state));
            }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void KeepCallback(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            KeepCallback(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void KeepCallback(CommandServerSocket socket, CommandServerCallReadQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            KeepCallback(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void KeepCallbackWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            KeepCallback(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void KeepCallbackWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            KeepCallback(index, methodIndex, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="callback"></param>
        internal void InputKeepCallback(ref RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((InputKeepCallbackMethodParameter)parameter.MethodParameter.notNull()).InputKeepCallback(ref refCallback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { refCallback?.CallbackCancelKeep(new KeepCallbackResponseParameter(state)); }
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void InputKeepCallback(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            InputKeepCallback(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void InputKeepCallback(CommandServerSocket socket, CommandServerCallReadQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            InputKeepCallback(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void InputKeepCallbackWrite(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            InputKeepCallback(ref parameter, callback);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">Request parameters
        /// 请求参数</param>
        /// <param name="callback">The return parameters of the keep callback
        /// 返回参数回调</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void InputKeepCallbackWrite(CommandServerSocket socket, CommandServerCallWriteQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            InputKeepCallback(ref parameter, callback);
        }
        /// <summary>
        /// Set a custom state object
        /// 设置自定义状态对象
        /// </summary>
        /// <param name="sessionObject"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetBeforePersistenceMethodParameterCustomSessionObject(object sessionObject)
        {
            CurrentMethodParameter?.SetBeforePersistenceCustomSessionObject(sessionObject);
        }
        /// <summary>
        /// Get the custom state object
        /// 获取自定义状态对象
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public object? GetBeforePersistenceMethodParameterCustomSessionObject()
#else
        public object GetBeforePersistenceMethodParameterCustomSessionObject()
#endif
        {
            var beforePersistenceMethodParameter = (CurrentMethodParameter as InputMethodParameter)?.BeforePersistenceMethodParameter;
            return (beforePersistenceMethodParameter ?? CurrentMethodParameter)?.GetBeforePersistenceCustomSessionObject();
        }
        /// <summary>
        /// Persistence
        /// 持久化
        /// </summary>
        private unsafe void persistence()
        {
            bool isRebuild = false, isSerializeCopyString = Config.IsSerializeCopyString;
            var head = default(MethodParameter);
            var end = default(MethodParameter);
            var current = default(MethodParameter);
            var outputSerializer = default(BinarySerializer);
            PersistenceCallback serviceCallback;
            PersistenceBuffer persistenceBuffer = new PersistenceBuffer(this);
            try
            {
                using (FileStream persistenceStream = new FileStream(PersistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.Read, persistenceBuffer.SendBufferMaxSize, FileOptions.None))
                {
                    persistenceStream.Seek(0, SeekOrigin.End);
                    if (PersistencePosition != persistenceStream.Length)
                    {
                        AutoCSer.LogHelper.ErrorIgnoreException($"文件流 {PersistenceFileInfo.FullName} 长度 {persistenceStream.Length} 与写入位置 {PersistencePosition} 不匹配", LogLevelEnum.Exception | LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
                        return;
                    }
                    persistenceBuffer.GetBufferLength();
                    SubArray<byte> outputData = default(SubArray<byte>);
                    using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetDefaultContext(CommandServerSocket.CommandServerSocketContext, ref isSerializeCopyString))
                    {
                        persistenceBuffer.OutputStream = outputStream;
                        do
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                            RESET:
                                persistenceBuffer.Reset();
                            WAIT:
                                if (persistenceHead == null) PersistenceWaitHandle.Wait();
                                if (IsDisposed) return;
                                if (isRebuilderPersistenceWaitting)
                                {
                                    var rebuilder = Rebuilder;
                                    if (rebuilder != null)
                                    {
#if AOT
                                        serviceCallbackWait.WaitOne();
#else
                                        var completedCallback = CanCreateSlave ? new PersistenceRebuilderCallback(rebuilder, PersistenceRebuilderCallbackTypeEnum.Completed) : null;
                                        serviceCallbackWait.WaitOne();
                                        if (Slave != null)
                                        {
                                            CommandServerCallQueue.AppendWriteOnly(completedCallback.notNull());
                                            rebuildCompletedWaitHandle.WaitOne();
                                        }
#endif
                                        persistenceStream.Dispose();

                                        if (rebuilder.QueuePersistence())
                                        {
                                            //if (head != null)
                                            //{
                                            //    PersistenceQueue.IsPushHead(head, end.notNull());
                                            //    head = null;
                                            //}
                                            persistenceHead = head;
                                            persistenceEnd = end;
                                            head = null;
                                            if (!PersistenceQueue.IsEmpty) PersistenceWaitHandle.IsWait = 1;
                                            isRebuilderPersistenceWaitting = false;
                                            Rebuilder = null;
                                            AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(persistence);
                                            isRebuild = true;
                                        }
                                        return;
                                    }
                                    isRebuilderPersistenceWaitting = false;
                                }
                                if (persistenceHead != null)
                                {
                                    current = head = persistenceHead;
                                    end = persistenceEnd;
                                    persistenceEnd = persistenceHead = null;
                                }
                                else if (head == null)
                                {
                                    AutoCSer.Threading.ThreadYield.YieldOnly();
                                    if ((current = PersistenceQueue.GetQueue(out end)) == null)
                                    {
                                        if (IsDisposed) return;
                                        goto WAIT;
                                    }
                                    head = current;
                                }
                                else if (current == null)
                                {
                                    AutoCSer.Threading.ThreadYield.YieldOnly();
#pragma warning disable CS8601
                                    PersistenceQueue.GetQueueToEnd(ref current, ref end);
#pragma warning restore CS8601
                                    if (current == null)
                                    {
                                        if (IsDisposed) return;
                                        goto SETDATA;
                                    }
                                }
                                else
                                {
                                    AutoCSer.Threading.ThreadYield.YieldOnly();
#pragma warning disable CS8601
                                    PersistenceQueue.GetQueueToEnd(ref end);
#pragma warning restore CS8601
                                }
                            LOOP:
                                do
                                {
                                    try
                                    {
                                        do
                                        {
                                            if (persistenceBuffer.TrySetCurrentIndex())
                                            {
                                                current = current.PersistenceSerialize(outputSerializer, PersistencePosition + persistenceBuffer.Count);
                                                if (persistenceBuffer.CheckResizeError()) goto SETDATA;
                                            }
                                            else
                                            {
                                                current = current.PersistenceSerialize(outputSerializer, PersistencePosition);
                                                if (persistenceBuffer.CheckDataStart()) goto SETDATA;
                                            }
                                        }
                                        while (current != null);
                                        break;
                                    }
                                    catch (Exception exception)
                                    {
                                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                                    }
                                    persistenceBuffer.RestoreCurrentIndex();
                                    current = current.notNull().PersistenceCallbackIgnoreException(CallStateEnum.PersistenceSerializeException);
                                }
                                while (current != null);
                                if (persistenceBuffer.Count == 0)
                                {
                                    head = null;
                                    goto WAIT;
                                }
                                if (!PersistenceQueue.IsEmpty) goto WAIT;
                                SETDATA:
                                try
                                {
                                    outputData = persistenceBuffer.GetData();
                                    serviceCallback = new PersistenceCallback(head.notNull(), current);
                                    if (Rebuilder != null && !isRebuilderPersistenceWaitting) Thread.Sleep(0);
                                }
                                catch (Exception exception)
                                {
                                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                                    PersistenceException(head, current, CallStateEnum.PersistenceSerializeException);
                                    goto FREE;
                                }
                                try
                                {
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                    persistenceStream.Flush();
                                    //Flush the persistent request data to the storage device and then execute the request to ensure the reliability of persistence and avoid the situation where data is lost due to persistence failure after the client is successfully fed back
                                    //持久化请求数据 Flush 到储存设备然后再执行请求保证持久化的可靠性，避免出现反馈客户端成功以后出现持久化失败丢失数据的情况
                                    PersistencePosition = persistenceStream.Position;
                                    if (Interlocked.Increment(ref serviceCallbackCount) == 1) serviceCallbackWait.Reset();
                                    serviceCallback.PersistencePosition = PersistencePosition;
                                    serviceCallback.CheckRebuild = Rebuilder == null && rebuildLoadExceptionNode == null && Config.CheckRebuild(this);
                                    CommandServerCallQueue.AppendWriteOnly(serviceCallback);
                                    if (Rebuilder != null && !isRebuilderPersistenceWaitting) Thread.Sleep(0);
                                }
                                catch (Exception exception)
                                {
                                    AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                                    PersistenceException(head, current, CallStateEnum.PersistenceWriteException);
                                    try
                                    {
                                        if (PersistencePosition != persistenceStream.Position)
                                        {
                                            persistenceStream.Seek(PersistencePosition, SeekOrigin.Begin);
                                            persistenceStream.SetLength(PersistencePosition);
                                        }
                                    }
                                    catch (Exception seekException)
                                    {
                                        AutoCSer.LogHelper.ExceptionIgnoreException(seekException, $"文件流写入位置 {PersistencePosition} 重置失败", LogLevelEnum.Exception | LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
                                        return;
                                    }
                                }
                            FREE:
                                head = current;
                                if (!persistenceBuffer.CheckNewBuffer())
                                {
                                    if (current == null) goto RESET;
                                    persistenceBuffer.Reset();
                                    goto LOOP;
                                }
                                //if (head != null)
                                //{
                                //    if (PersistenceQueue.IsPushHead(head, end.notNull())) PersistenceWaitHandle.Set();
                                //    head = null;
                                //}
                                persistenceHead = head;
                                persistenceEnd = end;
                                head = null;
                            }
                        }
                        while (true);
                    }
                }
            }
            catch (Exception exception)
            {
                AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
            }
            finally
            {
                if (!isRebuild) dispose();
                persistenceBuffer.Free();
                outputSerializer?.FreeContext(isSerializeCopyString);
                if (!isRebuild)
                {
                    PersistenceException(head);
                    PersistenceException(PersistenceQueue.Get());
                    head = persistenceHead;
                    persistenceHead = null;
                    PersistenceException(persistenceHead);
                }
            }
        }
        /// <summary>
        /// Initialize and load the data
        /// 初始化加载数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="methodIndex"></param>
        /// <param name="deserializer"></param>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallStateEnum Load(NodeIndex index, int methodIndex, BinaryDeserializer deserializer, SubArray<byte> data, out ServerNode? node)
#else
        internal CallStateEnum Load(NodeIndex index, int methodIndex, BinaryDeserializer deserializer, SubArray<byte> data, out ServerNode node)
#endif
        {
            if ((uint)index.Index < (uint)NodeIndex)
            {
                node = Nodes[index.Index].Get(index.Identity);
                if (node != null)
                {
                    if ((uint)methodIndex < (uint)node.NodeCreator.Methods.Length)
                    {
                        var method = node.NodeCreator.Methods[methodIndex];
                        if (method != null)
                        {
                            var serverNodeMethod = node.NodeCreator.NodeMethods[methodIndex].notNull();
                            if (serverNodeMethod.LoadPersistenceMethodIndex >= 0) method = node.NodeCreator.Methods[serverNodeMethod.LoadPersistenceMethodIndex].notNull();
                            switch (method.CallType)
                            {
                                case CallTypeEnum.Call:
                                case CallTypeEnum.CallOutput:
                                case CallTypeEnum.Callback:
                                case CallTypeEnum.KeepCallback:
                                case CallTypeEnum.Enumerable:
                                    if (data.Length == 0) break;
                                    node.SetPersistenceCallbackException();
                                    return CallStateEnum.LoadParameterSizeError;
                                case CallTypeEnum.CallInput:
                                case CallTypeEnum.SendOnly:
                                case CallTypeEnum.CallInputOutput:
                                case CallTypeEnum.InputCallback:
                                case CallTypeEnum.InputKeepCallback:
                                case CallTypeEnum.InputEnumerable:
                                    if (data.Length != 0) break;
                                    node.SetPersistenceCallbackException();
                                    return CallStateEnum.LoadParameterSizeError;
                            }
                            CallStateEnum state = node.CallState;
                            if (state == CallStateEnum.Success)
                            {
                                switch (method.CallType)
                                {
                                    case CallTypeEnum.Call: return ((CallMethod)method).LoadCall(node);
                                    case CallTypeEnum.CallOutput:
                                    case CallTypeEnum.Callback:
                                        return ((CallOutputMethod)method).LoadCall(node);
                                    case CallTypeEnum.CallInput:
                                        CallInputMethod callInputMethod = (CallInputMethod)method;
                                        CallInputMethodParameter callInputMethodParameter = callInputMethod.CreateInputParameter(node).notNullCastType<CallInputMethodParameter>();
                                        data.MoveStart(-sizeof(int));
                                        if (callInputMethodParameter.Deserialize(deserializer, ref data))
                                        {
                                            CurrentMethodParameter = callInputMethodParameter;
                                            return callInputMethod.LoadCall(callInputMethodParameter);
                                        }
                                        break;
                                    case CallTypeEnum.CallInputOutput:
                                    case CallTypeEnum.InputCallback:
                                        CallInputOutputMethod callInputOutputMethod = (CallInputOutputMethod)method;
                                        CallInputOutputMethodParameter callInputOutputMethodParameter = callInputOutputMethod.CreateInputParameter(node).notNullCastType<CallInputOutputMethodParameter>();
                                        data.MoveStart(-sizeof(int));
                                        if (callInputOutputMethodParameter.Deserialize(deserializer, ref data))
                                        {
                                            CurrentMethodParameter = callInputOutputMethodParameter;
                                            return callInputOutputMethod.LoadCall(callInputOutputMethodParameter);
                                        }
                                        break;
                                    case CallTypeEnum.SendOnly:
                                        SendOnlyMethod sendOnlyMethod = (SendOnlyMethod)method;
                                        SendOnlyMethodParameter sendOnlyMethodParameter = sendOnlyMethod.CreateInputParameter(node).notNullCastType<SendOnlyMethodParameter>();
                                        data.MoveStart(-sizeof(int));
                                        if (sendOnlyMethodParameter.Deserialize(deserializer, ref data))
                                        {
                                            CurrentMethodParameter = sendOnlyMethodParameter;
                                            return sendOnlyMethod.LoadCall(sendOnlyMethodParameter);
                                        }
                                        break;
                                    case CallTypeEnum.KeepCallback:
                                    case CallTypeEnum.Enumerable:
                                        return ((KeepCallbackMethod)method).LoadCall(node);
                                    case CallTypeEnum.InputKeepCallback:
                                    case CallTypeEnum.InputEnumerable:
                                        InputKeepCallbackMethod inputKeepCallbackMethod = (InputKeepCallbackMethod)method;
                                        InputKeepCallbackMethodParameter inputKeepCallbackMethodParameter = inputKeepCallbackMethod.CreateInputParameter(node).notNullCastType<InputKeepCallbackMethodParameter>();
                                        data.MoveStart(-sizeof(int));
                                        if (inputKeepCallbackMethodParameter.Deserialize(deserializer, ref data))
                                        {
                                            CurrentMethodParameter = inputKeepCallbackMethodParameter;
                                            return inputKeepCallbackMethod.LoadCall(inputKeepCallbackMethodParameter);
                                        }
                                        break;
                                }
                                return CallStateEnum.LoadParameterDeserializeError;
                            }
                            return CallStateEnum.Success;
                        }
                        return CallStateEnum.NotFoundMethod;
                    }
                    return CallStateEnum.MethodIndexOutOfRange;
                }
                return CallStateEnum.NodeIdentityNotMatch;
            }
            node = null;
            return CallStateEnum.NodeIndexOutOfRange;
        }
        /// <summary>
        /// Set the location information of the rebuild file
        /// 设置重建文件位置信息
        /// </summary>
        /// <param name="persistencePosition"></param>
        /// <param name="persistenceCallbackExceptionFilePosition"></param>
        /// <param name="rebuildSnapshotPosition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRebuild(long persistencePosition, long persistenceCallbackExceptionFilePosition, long rebuildSnapshotPosition)
        {
            SwitchPersistenceFileInfo();
            RebuildPosition += (ulong)PersistencePosition;
            PersistencePosition = persistencePosition;
            PersistenceCallbackExceptionFilePosition = persistenceCallbackExceptionFilePosition;
            RebuildSnapshotPosition = rebuildSnapshotPosition;
        }
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <returns></returns>
        internal RebuildResult Rebuild()
        {
            if (!IsDisposed)
            {
                if (Rebuilder == null)
                {
                    if (PersistencePosition != ServiceLoader.FileHeadSize)
                    {
                        if (CheckRebuild()) return new RebuildResult(CallStateEnum.Success);
                        return new RebuildResult(rebuildLoadExceptionNode.notNull());
                    }
                    return new RebuildResult(CallStateEnum.Success);
                }
                return new RebuildResult(CallStateEnum.PersistenceRebuilding);
            }
            return new RebuildResult(CallStateEnum.Disposed);
        }
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue)
        {
            return Rebuild();
        }
        /// <summary>
        /// Rebuild the persistent file (clear invalid data), and note that nodes that do not support snapshots will be discarded
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallWriteQueue queue)
        {
            return Rebuild();
        }
        /// <summary>
        /// Rebuild the persistent file
        /// 重建持久化文件
        /// </summary>
        /// <returns></returns>
        internal override bool CheckRebuild()
        {
            if (rebuildLoadExceptionNode == null && PersistenceType != PersistenceTypeEnum.ScanPersistence)
            {
                rebuildLoadExceptionNode = new PersistenceRebuilder(this).LoadExceptionNode;
                if (rebuildLoadExceptionNode == null) return true;
                AutoCSer.LogHelper.FatalIgnoreException($"内存数据库节点 {rebuildLoadExceptionNode.Key} => {rebuildLoadExceptionNode.NodeCreator.Type.fullName()} 初始化加载执行异常，持久化操作被中断", LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
            }
            return false;
        }
        /// <summary>
        /// Persistent file rebuild is abnormal and has been turned off
        /// 持久化文件重建异常并已关闭
        /// </summary>
        public virtual void RebuildError() { }
        /// <summary>
        /// Set the version information of the header of the persistent file
        /// 设置持久化文件头部版本信息
        /// </summary>
        /// <param name="persistenceFileHeadVersion"></param>
        /// <param name="rebuildPosition"></param>
        /// <param name="rebuildSnapshotPosition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetPersistenceFileHeadVersion(uint persistenceFileHeadVersion, ulong rebuildPosition, long rebuildSnapshotPosition)
        {
            PersistenceFileHeadVersion = persistenceFileHeadVersion;
            RebuildPosition = rebuildPosition;
            this.RebuildSnapshotPosition = rebuildSnapshotPosition;
        }
        /// <summary>
        /// Set up the rebuild persistence waiting operation
        /// 设置重建持久化等待操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRebuilderPersistenceWaitting()
        {
            isRebuilderPersistenceWaitting = true;
            PersistenceWaitHandle.Set();
        }
#if !AOT
        /// <summary>
        /// Initialize the loading repair method
        /// 初始化加载修复方法
        /// </summary>
        /// <param name="position"></param>
        internal void LoadRepairNodeMethod(long position)
        {
            var loader = default(RepairNodeMethodLoader);
            Monitor.Enter(nodeCreatorLock);
            if (!RepairNodeMethodLoaders.Remove(RebuildPosition + (ulong)position, out loader)) Monitor.Exit(nodeCreatorLock);
            else
            {
                Monitor.Exit(nodeCreatorLock);
                do
                {
                    var repairNodeMethod = loader.LoadRepair();
                    if (repairNodeMethod != null)
                    {
                        repairNodeMethod.LinkNext = LoadedRepairNodeMethod;
                        LoadedRepairNodeMethod = repairNodeMethod;
                    }
                }
                while ((loader = loader.LinkNext) != null);
            }
        }
        /// <summary>
        /// After the persistent reconstruction is completed, shut down the slave nodes
        /// 持久化重建完毕关闭从节点
        /// </summary>
        internal void RebuildCompleted()
        {
            var slave = Slave;
            Slave = null;
            do
            {
                try
                {
                    while (slave != null)
                    {
                        slave.Close();
                        slave = slave.LinkNext;
                    }
                    break;
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
            while ((slave = slave.notNull().LinkNext) != null);
            rebuildCompletedWaitHandle.Set();
        }
        /// <summary>
        /// Get the repair method information
        /// 获取修复方法信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly"></param>
        /// <param name="methodName"></param>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
#if NetStandard21
        private CallStateEnum getRepairMethod(NodeIndex index, byte[] rawAssembly, ref RepairNodeMethodName methodName, out ServerNode? node, out MethodInfo? method, out ServerMethodAttribute? methodAttribute)
#else
        private CallStateEnum getRepairMethod(NodeIndex index, byte[] rawAssembly, ref RepairNodeMethodName methodName, out ServerNode node, out MethodInfo method, out ServerMethodAttribute methodAttribute)
#endif
        {
            method = null;
            methodAttribute = null;
            if ((uint)index.Index < (uint)NodeIndex)
            {
                node = Nodes[index.Index].Node;
                if (node != null && node.Check(index)) return ServerNodeCreator.GetRepairMethod(rawAssembly, ref methodName, out method, out methodAttribute);
                return CallStateEnum.NodeIdentityNotMatch;
            }
            node = null;
            return CallStateEnum.NodeIndexOutOfRange;
        }
        /// <summary>
        /// Fix the interface method error and force overwriting the original interface method call. Except for the first parameter being the operation node object, the method definition must be consistent
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node, and the method number must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual void RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                var node = default(ServerNode);
                var method = default(MethodInfo);
                var methodAttribute = default(ServerMethodAttribute);
                state = getRepairMethod(index, rawAssembly, ref methodName, out node, out method, out methodAttribute);
                if (state == CallStateEnum.Success)
                {
                    node.notNull().Repair(rawAssembly, method.notNull(), methodAttribute.notNull(), callback);
                    state = CallStateEnum.Callbacked;
                }
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// Bind a new method to dynamically add interface functionality. The initial state of the new method number must be free
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">Assembly file data
        /// 程序集文件数据</param>
        /// <param name="methodName">The name of the repair method must be a static method. The first parameter must be the interface type of the operation node. The method number and other necessary configuration information must be configured using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex
        /// 修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual void BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!methodName.Name.EndsWith(ServerNodeMethod.BeforePersistenceMethodNameSuffix, StringComparison.Ordinal))
                {
                    var node = default(ServerNode);
                    var method = default(MethodInfo);
                    var methodAttribute = default(ServerMethodAttribute);
                    state = getRepairMethod(index, rawAssembly, ref methodName, out node, out method, out methodAttribute);
                    if (state == CallStateEnum.Success)
                    {
                        node.notNull().Bind(rawAssembly, method.notNull(), methodAttribute.notNull(), callback);
                        state = CallStateEnum.Callbacked;
                    }
                }
                else state = CallStateEnum.BindMethodNotSupportBeforePersistence;
            }
            finally 
            {
                if (state != CallStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// Get the timestamp identifier of the slave node
        /// 获取从节点时间戳标识
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long GetSlaveClientTimestamp()
        {
            long timestamp = Stopwatch.GetTimestamp();
            timestamp += (timestamp ^ slaveClientTimestamp).logicalInversion();
            //if (timestamp == slaveClientTimestamp) ++timestamp;
            slaveClientTimestamp = timestamp;
            return timestamp;
        }
        /// <summary>
        /// Create a slave node
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="isBackup">Is the backup client
        /// 是否备份客户端</param>
        /// <returns>Verify the timestamp from the node, and a negative number represents the CallStateEnum error status
        /// 从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        protected long createSlave(CommandServerSocket socket, bool isBackup)
        {
            if (CanCreateSlave)
            {
                if (!isRebuilderPersistenceWaitting) return new ServiceSlave(this, socket, isBackup).Timestamp;
                return -(long)(byte)CallStateEnum.PersistenceRebuilding;
            }
            return -(long)(byte)CallStateEnum.CanNotCreateSlave;
        }
        /// <summary>
        /// Create a slave node
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">Is the backup client
        /// 是否备份客户端</param>
        /// <returns>Verify the timestamp from the node, and a negative number represents the CallStateEnum error status
        /// 从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        public virtual long CreateSlave(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, bool isBackup)
        {
            return createSlave(socket, isBackup);
        }
        /// <summary>
        /// Create a slave node
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">Is the backup client
        /// 是否备份客户端</param>
        /// <returns>Verify the timestamp from the node, and a negative number represents the CallStateEnum error status
        /// 从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        public virtual long CreateSlave(CommandServerSocket socket, CommandServerCallWriteQueue queue, bool isBackup)
        {
            return createSlave(socket, isBackup);
        }
        /// <summary>
        /// Get the client information from the slave node
        /// 获取从节点客户端信息
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="left"></param>
        /// <returns></returns>
#if NetStandard21
        private ServiceSlave? getSlave(long timestamp, out ServiceSlave? left)
#else
        private ServiceSlave getSlave(long timestamp, out ServiceSlave left)
#endif
        {
            var slave = Slave;
            if (slave != null)
            {
                if (slave.Timestamp == timestamp)
                {
                    left = null;
                    return slave;
                }
                for (var next = slave.LinkNext; next != null; next = next.LinkNext)
                {
                    if (next.Timestamp == timestamp)
                    {
                        left = slave;
                        return next;
                    }
                    slave = next;
                }
            }
            left = null;
            return null;
        }
        /// <summary>
        /// Remove the information from the node client
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <returns></returns>
        public CommandServerSendOnly RemoveSlave(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp)
        {
            RemoveSlave(timestamp);
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Remove the information from the node client
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <returns></returns>
        public CommandServerSendOnly RemoveSlave(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp)
        {
            RemoveSlave(timestamp);
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Add the directory and file information of the repair method from the node
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="directory">Directory information of the repair method
        /// 修复方法目录信息</param>
        /// <param name="file">File information of the repair method
        /// 修复方法文件信息</param>
        /// <returns></returns>
        protected void appendRepairNodeMethodDirectoryFile(long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            var left = default(ServiceSlave);
            var slave = getSlave(timestamp, out left);
            if (slave != null && !slave.AppendRepairNodeMethodDirectoryFile(directory, file))
            {
                if (left == null) Slave = slave.LinkNext;
                else left.LinkNext = slave.LinkNext;
            }
        }
        /// <summary>
        /// Add the directory and file information of the repair method from the node
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="directory">Directory information of the repair method
        /// 修复方法目录信息</param>
        /// <param name="file">File information of the repair method
        /// 修复方法文件信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly AppendRepairNodeMethodDirectoryFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            appendRepairNodeMethodDirectoryFile(timestamp, directory, file);
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Add the directory and file information of the repair method from the node
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="directory">Directory information of the repair method
        /// 修复方法目录信息</param>
        /// <param name="file">File information of the repair method
        /// 修复方法文件信息</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly AppendRepairNodeMethodDirectoryFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            appendRepairNodeMethodDirectoryFile(timestamp, directory, file);
            return CommandServerSendOnly.Null;
        }
        /// <summary>
        /// Remove the information from the node client
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="timestamp"></param>
        internal void RemoveSlave(long timestamp)
        {
            var left = default(ServiceSlave);
            var slave = getSlave(timestamp, out left);
            if (slave != null)
            {
                if (left == null) Slave = slave.LinkNext;
                else left.LinkNext = slave.LinkNext;
                slave.Close();
            }
        }
        /// <summary>
        /// Get the repair node method information from slave node
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback">The callback delegate for get the method information of the repair node
        /// 获取修复节点方法信息回调委托</param>
        protected void getRepairNodeMethodPosition(long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                var left = default(ServiceSlave);
                var slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if (!slave.GetRepairNodeMethodPosition(ref refCallback))
                    {
                        if (left == null) Slave = slave.LinkNext;
                        else left.LinkNext = slave.LinkNext;
                    }
                }
                else
                state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                refCallback?.CallbackCancelKeep(new RepairNodeMethodPosition(new RepairNodeMethod(state)));
            }
        }
        /// <summary>
        /// Get the repair node method information from slave node
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback">The callback delegate for get the method information of the repair node
        /// 获取修复节点方法信息回调委托</param>
        public void GetRepairNodeMethodPosition(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback)
        {
            getRepairNodeMethodPosition(timestamp, callback);
        }
        /// <summary>
        /// Get the repair node method information from slave node
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback">The callback delegate for get the method information of the repair node
        /// 获取修复节点方法信息回调委托</param>
        public void GetRepairNodeMethodPosition(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback)
        {
            getRepairNodeMethodPosition(timestamp, callback);
        }
        /// <summary>
        /// Check whether the header of the persistent file matches
        /// 检查持久化文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <returns>The persistent stream has been written to the location and returns -1 in case of failure
        /// 持久化流已写入位置，失败返回 -1</returns>
        public long CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return PersistenceFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition ? PersistencePosition : -1;
        }
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        protected void getPersistenceFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                var left = default(ServiceSlave);
                var slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if ((PersistenceFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition) || position == 0)
                    {
                        if (position <= PersistencePosition)
                        {
                            if (!slave.GetPersistenceFile(position, ref refCallback))
                            {
                                if (left == null) Slave = slave.LinkNext;
                                else left.LinkNext = slave.LinkNext;
                            }
                            return;
                        }
                        else state = CallStateEnum.FilePositionOutOfRange;
                    }
                    else state = CallStateEnum.FileHeadNotMatch;
                }
                else
                state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                refCallback?.CallbackCancelKeep(new PersistenceFileBuffer(state, false));
            }
        }
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            getPersistenceFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
        /// <summary>
        /// Get the persistent file data
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent file
        /// 持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            getPersistenceFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
        /// <summary>
        /// Check whether the header of the persistent callback exception location file matches
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <returns>The written location of the persistent callback exception location file. Return -1 in case of failure
        /// 持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        public long CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return PersistenceCallbackExceptionPositionFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition ? PersistenceCallbackExceptionFilePosition : -1;
        }
        /// <summary>
        /// Get the location data of the persistent callback exception
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback"></param>
        protected void getPersistenceCallbackExceptionPosition(long timestamp, CommandServerKeepCallback<long> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                var left = default(ServiceSlave);
                var slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    slave.PersistenceCallbackExceptionPositionCallback = callback;
                    state = CallStateEnum.Callbacked;
                    return;
                }
                state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback.CallbackCancelKeep(-(long)(ulong)(byte)state);
            }
        }
        /// <summary>
        /// Get the location data of the persistent callback exception
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPosition(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, CommandServerKeepCallback<long> callback)
        {
            getPersistenceCallbackExceptionPosition(timestamp, callback);
        }
        /// <summary>
        /// Get the location data of the persistent callback exception
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPosition(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, CommandServerKeepCallback<long> callback)
        {
            getPersistenceCallbackExceptionPosition(timestamp, callback);
        }
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        protected void getPersistenceCallbackExceptionPositionFile(long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            var refCallback = callback;
            try
            {
                var left = default(ServiceSlave);
                var slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if ((PersistenceCallbackExceptionPositionFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition) || position == 0)
                    {
                        if (position <= PersistenceCallbackExceptionFilePosition)
                        {
                            if (!slave.GetPersistenceCallbackExceptionPositionFile(position, ref refCallback))
                            {
                                if (left == null) Slave = slave.LinkNext;
                                else left.LinkNext = slave.LinkNext;
                            }
                            return;
                        }
                        else state = CallStateEnum.FilePositionOutOfRange;
                    }
                    else state = CallStateEnum.FileHeadNotMatch;
                }
                else
                state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                refCallback?.CallbackCancelKeep(new PersistenceFileBuffer(state, true));
            }
        }
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPositionFile(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            getPersistenceCallbackExceptionPositionFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
        /// <summary>
        /// Get the file data of the persistent callback exception location
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">The timestamp of create the slave node client
        /// 创建从节点客户端时间戳</param>
        /// <param name="fileHeadVersion">The header version information of the persistent callback exception location file
        /// 持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">The starting position of persistent flow rebuild
        /// 持久化流重建起始位置</param>
        /// <param name="position">The starting position of the read file
        /// 读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPositionFile(CommandServerSocket socket, CommandServerCallWriteQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            getPersistenceCallbackExceptionPositionFile(timestamp, fileHeadVersion, rebuildPosition, position, callback);
        }
        /// <summary>
        /// Close the data load
        /// 关闭数据加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="isRetry"></param>
        internal override void CloseLoader(SlaveLoader loader, bool isRetry)
        {
            throw new InvalidOperationException();
        }
#endif
    }
}
