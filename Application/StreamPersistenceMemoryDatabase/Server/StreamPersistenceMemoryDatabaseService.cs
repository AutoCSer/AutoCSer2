using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
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
    /// 日志流持久化内存数据库服务端
    /// </summary>
    public class StreamPersistenceMemoryDatabaseService : CommandServerSocketSessionObjectService, IStreamPersistenceMemoryDatabaseService, ICommandServerBindController, IDisposable
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端会话对象操作
        /// </summary>
        public readonly ICommandServerSocketSessionObject<CommandServerSocketSessionObjectService, CommandServerSocketSessionObjectService> CommandServerSocketSessionObject;
        /// <summary>
        /// 持久化缓冲区池
        /// </summary>
        internal readonly ByteArrayPool PersistenceBufferPool;
        /// <summary>
        /// 持久化重建完毕关闭从节点等待事件
        /// </summary>
        private AutoCSer.Threading.OnceAutoWaitHandle rebuildCompletedWaitHandle;
        /// <summary>
        /// 持久化文件头部版本信息
        /// </summary>
        internal uint PersistenceFileHeadVersion;
        /// <summary>
        /// 持久化回调异常位置文件头部版本信息
        /// </summary>
        internal uint PersistenceCallbackExceptionPositionFileHeadVersion;
        /// <summary>
        /// 获取所有有效节点集合（不包括基础操作节点）
        /// </summary>
        internal IEnumerable<ServerNode> GetNodes
        {
            get
            {
                for (int index = NodeIndex; index > 1;)
                {
                    ServerNode node = Nodes[--index].Node;
                    if (node != null) yield return node;
                }
            }
        }
        /// <summary>
        /// 空闲索引集合
        /// </summary>
        private LeftArray<int> freeIndexs;
        /// <summary>
        /// 最后一次生成的从节点时间戳标识
        /// </summary>
        private long slaveClientTimestamp;
        /// <summary>
        /// 重建持久化是否正在等待操作
        /// </summary>
        private bool isRebuilderPersistenceWaitting;
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        /// <param name="config">日志流持久化内存数据库服务端配置</param>
        /// <param name="createServiceNode">创建服务基础操作节点委托</param>
        /// <param name="isMaster">是否主节点</param>
        internal unsafe StreamPersistenceMemoryDatabaseService(StreamPersistenceMemoryDatabaseServiceConfig config, Func<StreamPersistenceMemoryDatabaseService, ServerNode> createServiceNode, bool isMaster) : base(config, isMaster)
        {
            CommandServerSocketSessionObject = config.CommandServerSocketSessionObject ?? new CommandServerSocketSessionObject();
            freeIndexs = new LeftArray<int>(sizeof(int));
            Nodes[0].SetFreeIdentity(ServiceNode.ServiceNodeIndex.Identity);
            PersistenceBufferPool = ByteArrayPool.GetPool((BufferSizeBitsEnum)Math.Max((byte)BufferSizeBitsEnum.Kilobyte4, (byte)config.BufferSizeBits));
            createServiceNode(this);

            if (isMaster)
            {
                if (PersistenceFileInfo.Exists)
                {
                    if (PersistenceCallbackExceptionPositionFileInfo.Exists)
                    {
                        long startTimestamp = Stopwatch.GetTimestamp(), count = new ServiceLoader(this).Load();
                        //if (count != 0) Console.WriteLine($"初始化加载 {count} 条持久化数据耗时 {AutoCSer.Date.GetMillisecondsByTimestamp(Stopwatch.GetTimestamp() - startTimestamp)}ms");
                        for (int index = NodeIndex; index > 1; Nodes[--index].Node?.Loaded()) ;
                    }
                    else throw new Exception($"持久化回调异常位置文件缺失 {PersistenceCallbackExceptionPositionFileInfo.FullName}，请确认文件组完整性");
                }
                else
                {
                    if (PersistenceCallbackExceptionPositionFileInfo.Exists)
                    {
                        File.Move(PersistenceCallbackExceptionPositionFileInfo.FullName, PersistenceCallbackExceptionPositionFileInfo.FullName + config.GetBackupFileNameSuffix() + ".bak");
                    }
                    else
                    {
                        DirectoryInfo directory = PersistenceFileInfo.Directory;
                        if (!directory.Exists) directory.Create();
                    }
                    fixed (byte* bufferFixed = PersistenceDataPositionBuffer)
                    {
                        *(uint*)bufferFixed = PersistenceCallbackExceptionPositionFileHeadVersion = ServiceLoader.PersistenceCallbackExceptionPositionFileHead;
                        *(ulong*)(bufferFixed + sizeof(int)) = 0;
                        using (FileStream fileStream = PersistenceCallbackExceptionPositionFileInfo.Create()) fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);
                        *(uint*)bufferFixed = PersistenceFileHeadVersion = ServiceLoader.FieHead;
                        using (FileStream fileStream = PersistenceFileInfo.Create()) fileStream.Write(PersistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);
                    }
                    PersistenceCallbackExceptionFilePosition = PersistencePosition = ServiceLoader.FileHeadSize;
                }
                IsLoaded = true;
                serviceCallbackWait = new ManualResetEvent(true);
                PersistenceWaitHandle.Set(new object());
                rebuildCompletedWaitHandle.Set(new object());
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(persistence);
                RepairNodeMethodLoaders = null;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                dispose();

                PersistenceWaitHandle.Set();
                PersistenceException(PersistenceQueue.GetClear());
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void dispose()
        {
            IsDisposed = true;

            if (Rebuilder != null) CommandServerCallQueue.AddOnly(new PersistenceRebuilderCallback(Rebuilder, PersistenceRebuilderCallbackTypeEnum.Close));
        }
        /// <summary>
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void ICommandServerBindController.Bind(CommandServerController controller)
        {
            CommandServerCallQueue = controller.CallQueue;
        }
        /// <summary>
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerNode GetServerNode(ref HashString key)
        {
            ServerNode node;
            return nodeDictionary.TryGetValue(key, out node) ? node : null;
        }
        /// <summary>
        /// 初始化加载修复方法
        /// </summary>
        /// <param name="position"></param>
        internal void LoadRepairNodeMethod(long position)
        {
            RepairNodeMethodLoader loader;
            Monitor.Enter(nodeCreatorLock);
            if (!RepairNodeMethodLoaders.Remove(RebuildPosition + (ulong)position, out loader)) Monitor.Exit(nodeCreatorLock);
            else
            {
                Monitor.Exit(nodeCreatorLock);
                do
                {
                    RepairNodeMethod repairNodeMethod = loader.LoadRepair();
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
        /// 获取持久化流已当前写入位置
        /// </summary>
        /// <returns></returns>
        public long GetPersistencePosition()
        {
            return PersistencePosition;
        }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        internal protected NodeIndex GetNodeIndex(string key, NodeInfo nodeInfo)
        {
            if (key != null)
            {
                ServerNode node;
                HashString hashKey = key;
                if (nodeDictionary.TryGetValue(hashKey, out node)) return check(node, ref nodeInfo);
                if (!createKeys.Add(hashKey)) return new NodeIndex(CallStateEnum.NodeCreating);
                int index = GetFreeIndex();
                return new NodeIndex(index, Nodes[index].GetFreeIdentity());
            }
            return new NodeIndex(CallStateEnum.NullKey);
        }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>关键字不存在时返回一个空闲节点标识用于创建节点</returns>
        public virtual NodeIndex GetNodeIndex(CommandServerSocket socket, CommandServerCallQueue queue, string key, NodeInfo nodeInfo)
        {
            return GetNodeIndex(key, nodeInfo);
        }
        /// <summary>
        /// 检查节点信息是否匹配
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        private NodeIndex check(ServerNode node, ref NodeInfo nodeInfo)
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
        /// 创建节点之前检查节点标识是否匹配
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        internal NodeIndex CheckCreateNodeIndex(NodeIndex index, string key, ref NodeInfo nodeInfo)
        {
            if (key != null)
            {
                ServerNode node;
                if (nodeDictionary.TryGetValue(key, out node)) return check(node, ref nodeInfo);
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
        /// 初始化加载数据创建节点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        internal void LoadCreateNode(NodeIndex index, string key)
        {
            if (index.Index >= Nodes.Length) Nodes = AutoCSer.Common.Config.GetCopyArray(Nodes, Math.Max(Nodes.Length << 1, index.Index + 1));
            while (NodeIndex < index.Index) freeIndexs.Add(NodeIndex++);
            if (NodeIndex == index.Index) ++NodeIndex;
            else
            {
                int removeIndex = freeIndexs.IndexOf(index.Index);
                if (removeIndex >= 0) freeIndexs.RemoveToEnd(removeIndex);
            }
            if (Nodes[index.Index].SetFreeIdentity(index.Identity)) createKeys.Add(key);
        }
        /// <summary>
        /// 删除节点持久化参数检查
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveNodeBeforePersistence(NodeIndex index)
        {
            if (Nodes[index.Index].CheckGet(index.Identity) == null) return false;
            freeIndexs.PrepLength(1);
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public bool RemoveNode(NodeIndex index)
        {
            ServerNode node = Nodes[index.Index].GetRemove(index.Identity);
            if (node != null)
            {
                nodeDictionary.Remove(node.Key);
                freeIndexs.Add(index.Index);
                try
                {
                    node.OnRemoved();
                }
                catch(Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                return true;
            }
            return false;
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
        /// 获取节点索引
        /// </summary>
        /// <returns></returns>
        internal int GetFreeIndex()
        {
            int index;
            if (freeIndexs.TryPop(out index)) return index;
            if (NodeIndex == Nodes.Length) Nodes = AutoCSer.Common.Config.GetCopyArray(Nodes, NodeIndex << 1);
            return NodeIndex++;
        }
        /// <summary>
        /// 释放空闲节点
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveFreeIndex(NodeIndex index)
        {
            freeIndexs.PrepLength(1);
            if (Nodes[index.Index].FreeIdentity(index.Identity)) freeIndexs.Add(index.Index);
        }
        /// <summary>
        /// 创建会话对象，用于反序列化时获取服务信息
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly CreateSessionObject(CommandServerSocket socket)
        {
            CommandServerSocketSessionObject.CreateSessionObject(this, socket);
            return null;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        internal void Call(NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        ServerNode node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            if ((state = node.CallState) == CallStateEnum.Success) state = node.Call(methodIndex, ref callback);
                            return;
                        }
                        state = CallStateEnum.NodeIdentityNotMatch;
                    }
                    else state = CallStateEnum.NodeIndexOutOfRange;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.SynchronousCallback(state); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Call(CommandServerSocket socket, CommandServerCallQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<CallStateEnum> callback)
        {
            Call(index, methodIndex, callback);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        internal void CallOutput(NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        ServerNode node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            if ((state = node.CallState) == CallStateEnum.Success) state = node.CallOutput(methodIndex, ref callback);
                            return;
                        }
                        else state = CallStateEnum.NodeIdentityNotMatch;
                    }
                    else state = CallStateEnum.NodeIndexOutOfRange;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.SynchronousCallback(new ResponseParameter(state)); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallOutput(CommandServerSocket socket, CommandServerCallQueue queue, NodeIndex index, int methodIndex, CommandServerCallback<ResponseParameter> callback)
        {
            CallOutput(index, methodIndex, callback);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInput(CommandServerSocket socket, CommandServerCallQueue queue, RequestParameter parameter, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((CallInputMethodParameter)parameter.MethodParameter).CallInput(ref callback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.SynchronousCallback(state); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CallInputOutput(CommandServerSocket socket, CommandServerCallQueue queue, RequestParameter parameter, CommandServerCallback<ResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((CallInputOutputMethodParameter)parameter.MethodParameter).CallInputOutput(ref callback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.SynchronousCallback(new ResponseParameter(state)); }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerSendOnly SendOnly(CommandServerSocket socket, CommandServerCallQueue queue, RequestParameter parameter)
        {
            if (!IsDisposed && parameter.CallState == CallStateEnum.Success) ((SendOnlyMethodParameter)parameter.MethodParameter).SendOnly();
            return null;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        internal void KeepCallback(NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if ((uint)index.Index < (uint)NodeIndex)
                    {
                        ServerNode node = Nodes[index.Index].Get(index.Identity);
                        if (node != null)
                        {
                            if ((state = node.CallState) == CallStateEnum.Success) state = node.KeepCallback(methodIndex, ref callback);
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
                if (callback != null) callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state));
            }
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="callback">返回参数</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void KeepCallback(CommandServerSocket socket, CommandServerCallQueue queue, NodeIndex index, int methodIndex, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            KeepCallback(index, methodIndex, callback);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="parameter">请求参数</param>
        /// <param name="callback">返回参数</param>
        public void InputKeepCallback(CommandServerSocket socket, CommandServerCallQueue queue, RequestParameter parameter, CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!IsDisposed)
                {
                    if (parameter.CallState == CallStateEnum.Success) state = ((InputKeepCallbackMethodParameter)parameter.MethodParameter).InputKeepCallback(ref callback);
                    else state = parameter.CallState;
                }
                else state = CallStateEnum.Disposed;
            }
            finally { callback?.CallbackCancelKeep(new KeepCallbackResponseParameter(state)); }
        }
        /// <summary>
        /// 设置自定义状态对象
        /// </summary>
        /// <param name="sessionObject"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetBeforePersistenceMethodParameterCustomSessionObject(object sessionObject)
        {
            CurrentMethodParameter?.SetBeforePersistenceCustomSessionObject(sessionObject);
        }
        /// <summary>
        /// 获取自定义状态对象
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public object GetBeforePersistenceMethodParameterCustomSessionObject()
        {
            CallInputOutputMethodParameter beforePersistenceMethodParameter = (CurrentMethodParameter as InputMethodParameter)?.BeforePersistenceMethodParameter;
            return (beforePersistenceMethodParameter ?? CurrentMethodParameter)?.GetBeforePersistenceCustomSessionObject();
        }
        /// <summary>
        /// 持久化
        /// </summary>
        private unsafe void persistence()
        {
            bool isRebuild = false;
            MethodParameter head = null, end = null, current = null;
            BinarySerializer outputSerializer = null;
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
                    using (UnmanagedStream outputStream = (outputSerializer = BinarySerializer.YieldPool.Default.Pop() ?? new BinarySerializer()).SetContext(CommandServerSocket.CommandServerSocketContext))
                    {
                        outputSerializer.SetDefault();
                        persistenceBuffer.OutputStream = outputStream;
                        do
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                            RESET:
                                persistenceBuffer.Reset();
                            WAIT:
                                PersistenceWaitHandle.Wait();
                                if (IsDisposed) return;
                                if (isRebuilderPersistenceWaitting)
                                {
                                    PersistenceRebuilderCallback completedCallback = CanCreateSlave ? new PersistenceRebuilderCallback(Rebuilder, PersistenceRebuilderCallbackTypeEnum.Completed) : null;
                                    serviceCallbackWait.WaitOne();
                                    if (Slave != null)
                                    {
                                        CommandServerCallQueue.AddOnly(completedCallback);
                                        rebuildCompletedWaitHandle.Wait();
                                    }
                                    persistenceStream.Dispose();

                                    long persistenceCallbackExceptionFilePosition, newPersistencePosition = Rebuilder.QueuePersistence(out persistenceCallbackExceptionFilePosition);
                                    if (newPersistencePosition != 0)
                                    {
                                        RebuildPosition += (ulong)PersistencePosition;
                                        PersistencePosition = newPersistencePosition;
                                        PersistenceCallbackExceptionFilePosition = persistenceCallbackExceptionFilePosition;
                                        if (current != null)
                                        {
                                            PersistenceQueue.IsPushHead(ref current, end);
                                            head = null;
                                        }
                                        if (!PersistenceQueue.IsEmpty) PersistenceWaitHandle.IsWait = 1;
                                        isRebuilderPersistenceWaitting = false;
                                        Rebuilder = null;
                                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(persistence);
                                        isRebuild = true;
                                    }
                                    return;
                                }
                                if (head == null)
                                {
                                    if ((current = PersistenceQueue.GetClear(out end)) == null)
                                    {
                                        if (IsDisposed) return;
                                        goto WAIT;
                                    }
                                    head = current;
                                }
                                else if (current == null)
                                {
                                    PersistenceQueue.GetToEndClear(ref current, ref end);
                                    if (current == null)
                                    {
                                        if (IsDisposed) return;
                                        goto SETDATA;
                                    }
                                }
                                else
                                {
                                    PersistenceQueue.GetToEndClear(ref end);
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
                                    current = current.PersistenceCallbackIgnoreException(CallStateEnum.PersistenceSerializeException);
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
                                    serviceCallback = new PersistenceCallback(head, current);
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
                                    PersistencePosition = persistenceStream.Position;
                                    if (Interlocked.Increment(ref serviceCallbackCount) == 1) serviceCallbackWait.Reset();
                                    serviceCallback.PersistencePosition = PersistencePosition;
                                    CommandServerCallQueue.AddOnly(serviceCallback);
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
                                if (current != null)
                                {
                                    head = null;
                                    if (PersistenceQueue.IsPushHead(ref current, end)) PersistenceWaitHandle.Set();
                                }
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
                outputSerializer?.FreeContext();
                if (!isRebuild)
                {
                    PersistenceException(head);
                    PersistenceException(PersistenceQueue.GetClear());
                }
            }
        }
        /// <summary>
        /// 初始化加载数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="methodIndex"></param>
        /// <param name="deserializer"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal CallStateEnum Load(NodeIndex index, int methodIndex, BinaryDeserializer deserializer, SubArray<byte> data)
        {
            if ((uint)index.Index < (uint)NodeIndex)
            {
                ServerNode node = Nodes[index.Index].Get(index.Identity);
                if (node != null)
                {
                    if ((uint)methodIndex < (uint)node.NodeCreator.Methods.Length)
                    {
                        Method method = node.NodeCreator.Methods[methodIndex];
                        if (method != null)
                        {
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
                                        CallInputMethodParameter callInputMethodParameter = callInputMethod.CreateParameter(node);
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
                                        CallInputOutputMethodParameter callInputOutputMethodParameter = callInputOutputMethod.CreateParameter(node);
                                        data.MoveStart(-sizeof(int));
                                        if (callInputOutputMethodParameter.Deserialize(deserializer, ref data))
                                        {
                                            CurrentMethodParameter = callInputOutputMethodParameter;
                                            return callInputOutputMethod.LoadCall(callInputOutputMethodParameter);
                                        }
                                        break;
                                    case CallTypeEnum.SendOnly:
                                        SendOnlyMethod sendOnlyMethod = (SendOnlyMethod)method;
                                        SendOnlyMethodParameter sendOnlyMethodParameter = sendOnlyMethod.CreateParameter(node);
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
                                        InputKeepCallbackMethodParameter inputKeepCallbackMethodParameter = inputKeepCallbackMethod.CreateParameter(node);
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
            return CallStateEnum.NodeIndexOutOfRange;
        }
        /// <summary>
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
                        ServerNode loadExceptionNode = new PersistenceRebuilder(this).LoadExceptionNode;
                        if (loadExceptionNode == null) return new RebuildResult(CallStateEnum.Success);
                        return new RebuildResult(Rebuilder.LoadExceptionNode);
                    }
                    return new RebuildResult(CallStateEnum.Success);
                }
                return new RebuildResult(CallStateEnum.PersistenceRebuilding);
            }
            return new RebuildResult(CallStateEnum.Disposed);
        }
        /// <summary>
        /// 重建持久化文件（清除无效数据），注意不支持快照的节点将被抛弃
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual RebuildResult Rebuild(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            return Rebuild();
        }
        /// <summary>
        /// 持久化文件重建异常并已关闭
        /// </summary>
        public virtual void RebuildError() { }
        /// <summary>
        /// 持久化重建完毕关闭从节点
        /// </summary>
        internal void RebuildCompleted()
        {
            ServiceSlave slave = Slave;
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
            while ((slave = slave.LinkNext) != null);
            rebuildCompletedWaitHandle.Set();
        }
        /// <summary>
        /// 设置持久化文件头部版本信息
        /// </summary>
        /// <param name="persistenceFileHeadVersion"></param>
        /// <param name="rebuildPosition"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetPersistenceFileHeadVersion(uint persistenceFileHeadVersion, ulong rebuildPosition)
        {
            PersistenceFileHeadVersion = persistenceFileHeadVersion;
            RebuildPosition = rebuildPosition;
        }
        /// <summary>
        /// 设置重建持久化等待操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetRebuilderPersistenceWaitting()
        {
            isRebuilderPersistenceWaitting = true;
            PersistenceWaitHandle.Set();
        }
        /// <summary>
        /// 获取修复方法信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly"></param>
        /// <param name="methodName"></param>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="methodAttribute"></param>
        /// <returns></returns>
        private CallStateEnum getRepairMethod(NodeIndex index, byte[] rawAssembly, ref RepairNodeMethodName methodName, out ServerNode node, out MethodInfo method, out ServerMethodAttribute methodAttribute)
        {
            method = null;
            methodAttribute = null;
            if ((uint)index.Index < (uint)NodeIndex)
            {
                node = Nodes[index.Index].CheckGet(index.Identity);
                if (node != null) return ServerNodeCreator.GetRepairMethod(rawAssembly, ref methodName, out method, out methodAttribute);
                return CallStateEnum.NodeIdentityNotMatch;
            }
            node = null;
            return CallStateEnum.NodeIndexOutOfRange;
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual void RepairNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                ServerNode node;
                MethodInfo method;
                ServerMethodAttribute methodAttribute;
                state = getRepairMethod(index, rawAssembly, ref methodName, out node, out method, out methodAttribute);
                if (state == CallStateEnum.Success) node.Repair(rawAssembly, method, methodAttribute, ref callback);
            }
            finally { callback?.Callback(state); }
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rawAssembly">程序集文件数据</param>
        /// <param name="methodName">修复方法名称，必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public virtual void BindNodeMethod(NodeIndex index, byte[] rawAssembly, RepairNodeMethodName methodName, CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                if (!methodName.Name.EndsWith(ServerNodeMethod.BeforePersistenceMethodNameSuffix, StringComparison.Ordinal))
                {
                    ServerNode node;
                    MethodInfo method;
                    ServerMethodAttribute methodAttribute;
                    state = getRepairMethod(index, rawAssembly, ref methodName, out node, out method, out methodAttribute);
                    if (state == CallStateEnum.Success) node.Bind(rawAssembly, method, methodAttribute, ref callback);
                }
                else state = CallStateEnum.BindMethodNotSupportBeforePersistence;
            }
            finally { callback?.Callback(state); }
        }
        /// <summary>
        /// 获取从节点时间戳标识
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long GetSlaveClientTimestamp()
        {
            long timestamp = Stopwatch.GetTimestamp();
            if (timestamp == slaveClientTimestamp) ++timestamp;
            slaveClientTimestamp = timestamp;
            return timestamp;
        }
        /// <summary>
        /// 创建从节点
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="isBackup">是否备份客户端</param>
        /// <returns>从节点验证时间戳，负数表示 CallStateEnum 错误状态</returns>
        public virtual long CreateSlave(CommandServerSocket socket, CommandServerCallQueue queue, bool isBackup)
        {
            if (CanCreateSlave)
            {
                if (!isRebuilderPersistenceWaitting) return new ServiceSlave(this, socket, isBackup).Timestamp;
                return -(long)(byte)CallStateEnum.PersistenceRebuilding;
            }
            return -(long)(byte)CallStateEnum.CanNotCreateSlave;
        }
        /// <summary>
        /// 获取从节点客户端信息
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        private ServiceSlave getSlave(long timestamp, out ServiceSlave left)
        {
            ServiceSlave slave = Slave;
            if (slave != null)
            {
                if (slave.Timestamp == timestamp)
                {
                    left = null;
                    return slave;
                }
                for (ServiceSlave next = slave.LinkNext; next != null; next = next.LinkNext)
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
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <returns></returns>
        public CommandServerSendOnly RemoveSlave(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp)
        {
            RemoveSlave(timestamp);
            return null;
        }
        /// <summary>
        /// 从节点添加修复方法目录与文件信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="directory">修复方法目录信息</param>
        /// <param name="file">修复方法文件信息</param>
        /// <returns></returns>
        public CommandServerSendOnly AppendRepairNodeMethodDirectoryFile(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp, RepairNodeMethodDirectory directory, RepairNodeMethodFile file)
        {
            ServiceSlave left;
            ServiceSlave slave = getSlave(timestamp, out left);
            if (slave != null && !slave.AppendRepairNodeMethodDirectoryFile(directory, file))
            {
                if (left == null) Slave = slave.LinkNext;
                else left.LinkNext = slave.LinkNext;
            }
            return null;
        }
        /// <summary>
        /// 移除从节点客户端信息
        /// </summary>
        /// <param name="timestamp"></param>
        internal void RemoveSlave(long timestamp)
        {
            ServiceSlave left;
            ServiceSlave slave = getSlave(timestamp, out left);
            if (slave != null)
            {
                if (left == null) Slave = slave.LinkNext;
                else left.LinkNext = slave.LinkNext;
                slave.Close();
            }
        }
        /// <summary>
        /// 从节点获取修复节点方法信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback">获取修复节点方法信息委托</param>
        public void GetRepairNodeMethodPosition(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp, CommandServerKeepCallback<RepairNodeMethodPosition> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                ServiceSlave left;
                ServiceSlave slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if (!slave.GetRepairNodeMethodPosition(ref callback))
                    {
                        if (left == null) Slave = slave.LinkNext;
                        else left.LinkNext = slave.LinkNext;
                    }
                }
                else state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                callback?.CallbackCancelKeep(new RepairNodeMethodPosition(new RepairNodeMethod(state)));
            }
        }
        /// <summary>
        /// 检查文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化流已写入位置，失败返回 -1</returns>
        public long CheckPersistenceFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return PersistenceFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition ? PersistencePosition : -1;
        }
        /// <summary>
        /// 获取持久化文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceFile(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                ServiceSlave left;
                ServiceSlave slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if ((PersistenceFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition) || position == 0)
                    {
                        if (position <= PersistencePosition)
                        {
                            if (!slave.GetPersistenceFile(position, ref callback))
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
                else state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                callback?.CallbackCancelKeep(new PersistenceFileBuffer(state, false));
            }
        }
        /// <summary>
        /// 检查持久化回调异常位置文件头部是否匹配
        /// </summary>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <returns>持久化回调异常位置文件已写入位置，失败返回 -1</returns>
        public long CheckPersistenceCallbackExceptionPositionFileHead(uint fileHeadVersion, ulong rebuildPosition)
        {
            return PersistenceCallbackExceptionPositionFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition ? PersistenceCallbackExceptionFilePosition : -1;
        }
        /// <summary>
        /// 获取持久化回调异常位置数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPosition(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp, CommandServerKeepCallback<long> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                ServiceSlave left;
                ServiceSlave slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    slave.PersistenceCallbackExceptionPositionCallback = callback;
                    callback = null;
                    return;
                }
                else state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                callback?.CallbackCancelKeep(-(long)(ulong)(byte)state);
            }
        }
        /// <summary>
        /// 获取持久化回调异常位置文件数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="timestamp">创建从节点客户端信息时间戳</param>
        /// <param name="fileHeadVersion">持久化回调异常位置文件头部版本信息</param>
        /// <param name="rebuildPosition">持久化流重建起始位置</param>
        /// <param name="position">读取文件起始位置</param>
        /// <param name="callback"></param>
        public void GetPersistenceCallbackExceptionPositionFile(CommandServerSocket socket, CommandServerCallQueue queue, long timestamp, uint fileHeadVersion, ulong rebuildPosition, long position, CommandServerKeepCallback<PersistenceFileBuffer> callback)
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                ServiceSlave left;
                ServiceSlave slave = getSlave(timestamp, out left);
                if (slave != null)
                {
                    if ((PersistenceCallbackExceptionPositionFileHeadVersion == fileHeadVersion && RebuildPosition == rebuildPosition) || position == 0)
                    {
                        if (position <= PersistenceCallbackExceptionFilePosition)
                        {
                            if (!slave.GetPersistenceCallbackExceptionPositionFile(position, ref callback))
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
                else state = CallStateEnum.SlaveTimestampNotMatch;
            }
            finally
            {
                callback?.CallbackCancelKeep(new PersistenceFileBuffer(state, true));
            }
        }
        /// <summary>
        /// 关闭数据加载
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="isRetry"></param>
        internal override void CloseLoader(SlaveLoader loader, bool isRetry)
        {
            throw new InvalidOperationException();
        }
    }
}
