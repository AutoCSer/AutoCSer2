using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化文件重建
    /// </summary>
    internal sealed class PersistenceRebuilder
    {
        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        internal StreamPersistenceMemoryDatabaseService Service;
        /// <summary>
        /// 持久化文件信息
        /// </summary>
        private FileInfo persistenceFileInfo;
        /// <summary>
        /// 持久化回调异常位置文件信息
        /// </summary>
        private FileInfo persistenceCallbackExceptionPositionFileInfo;
        /// <summary>
        /// 持久化回调异常位置输出缓冲区
        /// </summary>
        private byte[] persistenceDataPositionBuffer;
        /// <summary>
        /// 服务端节点集合
        /// </summary>
        private readonly KeyValue<ServerNode, int>[] nodes;
        /// <summary>
        /// 快照事务关系节点集合
        /// </summary>
        private readonly Dictionary<string, ServerNode> snapshotTransactionNodes;
        /// <summary>
        /// 初始化加载执行异常节点
        /// </summary>
#if NetStandard21
        internal readonly ServerNode? LoadExceptionNode;
#else
        internal readonly ServerNode LoadExceptionNode;
#endif
        /// <summary>
        /// 当前持久化节点
        /// </summary>
#if NetStandard21
        private ServerNode? node;
#else
        private ServerNode node;
#endif
        /// <summary>
        /// 调用持久化链表
        /// </summary>
        private LinkStack<MethodParameter> persistenceQueue;
        /// <summary>
        /// 持久化流重建起始位置
        /// </summary>
        private ulong rebuildPosition;
        /// <summary>
        /// 持久化流已写入位置
        /// </summary>
        private long persistencePosition;
        /// <summary>
        /// 持久化回调异常位置文件已写入位置
        /// </summary>
        private long persistenceCallbackExceptionFilePosition;
        /// <summary>
        /// 重建快照结束位置
        /// </summary>
        private long rebuildSnapshotPosition;
        /// <summary>
        /// 快照事务关系节点版本
        /// </summary>
        private readonly int snapshotTransactionNodeVersion;
        /// <summary>
        /// 快照事务关系节点版本重建
        /// </summary>
        private int isSnapshotTransactionNodeVersionRebuild;
        /// <summary>
        /// 服务端节点数量
        /// </summary>
        private int nodeCount;
        /// <summary>
        /// 是否已关闭
        /// </summary>
        private bool isClosed;
        /// <summary>
        /// 是否已关闭
        /// </summary>
        private bool isClosedOrServiceDisposed { get { return isClosed | Service.IsDisposed; } }
        /// <summary>
        /// 日志流持久化文件重建
        /// </summary>
        /// <param name="service">日志流持久化内存数据库服务端</param>
        internal PersistenceRebuilder(StreamPersistenceMemoryDatabaseService service)
        {
            Service = service;
            nodes = new KeyValue<ServerNode, int>[service.NodeIndex];
            snapshotTransactionNodes = DictionaryCreator.CreateAny<string, ServerNode>();
            snapshotTransactionNodeVersion = Service.SnapshotTransactionNodeVersion;
            service.Rebuilder = this;
#if NetStandard21
            persistenceFileInfo = persistenceCallbackExceptionPositionFileInfo = service.PersistenceFileInfo;
            persistenceDataPositionBuffer = EmptyArray<byte>.Array;
#endif
            bool isLoaded = false;
            try
            {
                foreach (ServerNode node in service.GetNodes())
                {
                    if (node.IsRebuild)
                    {
                        if (!node.IsLoadException)
                        {
                            if (node.SnapshotTransactionNodeCount == 0)
                            {
                                nodes[nodeCount++].Set(node, 1);
                                node.Rebuilding = true;
                            }
                            else snapshotTransactionNodes.Add(node.Key, node);
                        }
                        else
                        {
                            LoadExceptionNode = node;
                            isLoaded = true;
                            while (nodeCount != 0) nodes[--nodeCount].Key.CloseRebuild();
                            Service.Rebuilder = null;
                            return;
                        }
                    }
                }
                while (snapshotTransactionNodes.Count != 0)
                {
                    KeyValuePair<string, ServerNode> node = default(KeyValuePair<string, ServerNode>);
                    foreach (KeyValuePair<string, ServerNode> snapshotTransactionNode in snapshotTransactionNodes)
                    {
                        node = snapshotTransactionNode;
                        break;
                    }
                    int nodeIndex = nodeCount;
                    appendSnapshotTransactionNode(node);
                    nodes[nodeCount - 1].Value = nodeCount - nodeIndex;
                }
                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(createFile);
                isLoaded = true;
            }
            finally
            {
                if (!isLoaded) Close();
            }
        }
        /// <summary>
        /// 添加快照事务关系节点
        /// </summary>
        /// <param name="serverNode"></param>
        private void appendSnapshotTransactionNode(KeyValuePair<string, ServerNode> serverNode)
        {
            if (snapshotTransactionNodes.Remove(serverNode.Key))
            {
                ServerNode node = serverNode.Value;
                nodes[nodeCount++].Key = node;
                node.Rebuilding = true;
                foreach (KeyValuePair<string, ServerNode> snapshotTransactionNode in node.SnapshotTransactionNodes.notNull())
                {
                    appendSnapshotTransactionNode(snapshotTransactionNode);
                }
            }
        }
        /// <summary>
        /// 关闭重建操作
        /// </summary>
        /// <param name="isNodeLog"></param>
        internal void Close(bool isNodeLog = false)
        {
            if (!isClosed)
            {
                isClosed = true;
                try
                {
                    if (object.ReferenceEquals(Service.Rebuilder, this))
                    {
                        node?.CloseRebuild();
                        while (nodeCount != 0) nodes[--nodeCount].Key.CloseRebuild();
                        if (persistenceCallbackExceptionPositionFileInfo.RefreshExists()) persistenceCallbackExceptionPositionFileInfo.Delete();
                        if (persistenceFileInfo.RefreshExists()) persistenceFileInfo.Delete();
                        Service.Rebuilder = null;
                        if (isNodeLog && node != null) AutoCSer.LogHelper.FatalIgnoreException(node.Key);
                    }
                }
                finally { Service.RebuildError(); }
            }
        }
        /// <summary>
        /// 关闭重建操作（输出失败日志）
        /// </summary>
        internal void CloseLog()
        {
            Close(true);
        }
        /// <summary>
        /// 创建重建文件
        /// </summary>
        private unsafe void createFile()
        {
            bool isPersistence = false;
            try
            {
                string backupFileNameSuffix = Service.Config.GetBackupFileNameSuffix() + ".rb";
                persistenceCallbackExceptionPositionFileInfo = new FileInfo(Service.PersistenceCallbackExceptionPositionSwitchFileInfo.FullName + backupFileNameSuffix);
                persistenceFileInfo = new FileInfo(Service.PersistenceSwitchFileInfo.FullName + backupFileNameSuffix);
                persistenceDataPositionBuffer = AutoCSer.Common.GetUninitializedArray<byte>(Math.Max(ServiceLoader.FileHeadSize, sizeof(long)));
                fixed (byte* bufferFixed = persistenceDataPositionBuffer)
                {
                    *(uint*)bufferFixed = ServiceLoader.PersistenceCallbackExceptionPositionFileHead;
                    *(ulong*)(bufferFixed + sizeof(int)) = rebuildPosition = Service.RebuildPersistenceEndPosition;
                    using (FileStream fileStream = persistenceCallbackExceptionPositionFileInfo.Create()) fileStream.Write(persistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize);
                    *(uint*)bufferFixed = ServiceLoader.FieHead;
                    *(long*)(bufferFixed + (sizeof(int) + sizeof(ulong))) = 0;
                    using (FileStream fileStream = persistenceFileInfo.Create()) fileStream.Write(persistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);
                }
                persistenceCallbackExceptionFilePosition = ServiceLoader.ExceptionPositionFileHeadSize;
                persistencePosition = ServiceLoader.FileHeadSize;
                Service.CommandServerCallQueue.AppendReadOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.NextNode));
                isPersistence = true;
            }
            finally
            {
                if (!isPersistence) Close();
            }
        }
        /// <summary>
        /// 检查快照事务关系节点版本
        /// </summary>
        /// <returns></returns>
        private bool checkSnapshotTransactionNodeVersion()
        {
            if (snapshotTransactionNodeVersion == Service.SnapshotTransactionNodeVersion) return true;
            if (Interlocked.CompareExchange(ref isSnapshotTransactionNodeVersionRebuild, 1, 0) == 0)
            {
                Service.CommandServerCallQueue.AppendWriteOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.CloseVersion));
            }
            return false;
        }
        /// <summary>
        /// 关闭版本重建
        /// </summary>
        internal void CloseVersion()
        {
            try
            {
                Close();
            }
            finally
            {
                Service = new PersistenceRebuilder(Service).Service;
            }
        }
        /// <summary>
        /// 持久化下一个节点
        /// </summary>
        internal void NextNode()
        {
            if (!checkSnapshotTransactionNodeVersion()) return;
            while (nodeCount != 0)
            {
                if (!isClosedOrServiceDisposed)
                {
                    KeyValue<ServerNode, int> serverNode = nodes[--nodeCount];
                    int setCount = serverNode.Value;
                    node = serverNode.Key;
                    if (setCount != 0 || !node.IsRemoved)
                    {
                        bool isPersistence = false;
                        try
                        {
                            for (int nodeIndex = nodeCount - setCount; nodeIndex != nodeCount;)
                            {
                                if (!nodes[++nodeIndex].Key.CheckSnapshot()) return;
                            }
                            if (!node.IsRemoved)
                            {
                                AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(getSnapshotArray);
                                isPersistence = true;
                                return;
                            }
                            isPersistence = true;
                        }
                        finally
                        {
                            if (!isPersistence) Service.CommandServerCallQueue.AppendWriteOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.CloseLog));
                        }
                    }
                }
                else return;
            }
            rebuildSnapshotPosition = persistencePosition;
            CheckQueue();
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        private void getSnapshotArray()
        {
            bool isPersistence = false;
            try
            {
                for (int nodeIndex = nodeCount - nodes[nodeCount].Value; nodeIndex != nodeCount; nodes[++nodeIndex].Key.GetSnapshotArray()) ;
                Service.CommandServerCallQueue.ConcurrencyRead(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.GetSnapshotResult));
                isPersistence = true;
            }
            finally
            {
                if (!isPersistence) Close(true);
            }
        }
        /// <summary>
        /// 获取节点快照数据
        /// </summary>
        internal void GetSnapshotResult()
        {
            if (!checkSnapshotTransactionNodeVersion()) return;
            bool isPersistence = false;
            try
            {
                for (int nodeIndex = nodeCount - nodes[nodeCount].Value; nodeIndex != nodeCount;)
                {
                    if (!nodes[++nodeIndex].Key.GetSnapshotResult()) return;
                }
                if (!node.notNull().IsRemoved)
                {
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(nodePersistence);
                    isPersistence = true;
                    return;
                }
                isPersistence = true;
            }
            finally
            {
                if (!isPersistence) Service.CommandServerCallQueue.AppendWriteOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.CloseLog));
            }
            NextNode();
        }
        /// <summary>
        /// 当前节点持久化操作
        /// </summary>
        private void nodePersistence()
        {
            bool isPersistence = false;
            try
            {
                if (node.notNull().Rebuild(this))
                {
                    Service.CommandServerCallQueue.AppendReadOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.NextNode));
                    isPersistence = true;
                    return;
                }
            }
            finally
            {
                if (!isPersistence) Close(true);
            }
        }
        /// <summary>
        /// 检查持久化流已写入位置是否匹配
        /// </summary>
        /// <param name="persistenceStream"></param>
        /// <returns></returns>
        private bool checkPersistencePosition(FileStream persistenceStream)
        {
            persistenceStream.Seek(0, SeekOrigin.End);
            if (persistencePosition != persistenceStream.Length)
            {
                AutoCSer.LogHelper.ErrorIgnoreException($"文件流 {persistenceFileInfo.FullName} 长度 {persistenceStream.Length} 与写入位置 {persistencePosition} 不匹配", LogLevelEnum.Exception | LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 当前节点持久化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="newArray"></param>
        /// <returns></returns>
        internal unsafe bool Rebuild<T>(ref LeftArray<T> array, ref LeftArray<T> newArray)
        {
            var outputSerializer = default(BinarySerializer);
            PersistenceBuffer persistenceBuffer = new PersistenceBuffer(Service);
            try
            {
                if (isClosedOrServiceDisposed) return false;
                using (FileStream persistenceStream = new FileStream(persistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, persistenceBuffer.SendBufferMaxSize, FileOptions.None))
                {
                    if (!checkPersistencePosition(persistenceStream)) return false;
                    persistenceBuffer.GetBufferLength();
                    SubArray<byte> outputData;
                    using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(CommandServerSocket.CommandServerSocketContext))
                    {
                        outputSerializer.SetDefault();
                        persistenceBuffer.OutputStream = outputStream;
                        fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                        {
                            persistenceBuffer.SetStart(dataFixed);
                            persistenceBuffer.Reset();
                            node.notNull().CreateNodeMethodParameter.notNull().PersistenceSerialize(outputSerializer, persistencePosition);
                            outputData = persistenceBuffer.GetData();
                        }
                        if (isClosedOrServiceDisposed) return false;
                        persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);

                        SnapshotMethodSerializer snapshotMethodSerializer = new SnapshotMethodSerializer(outputSerializer, node.notNull(), typeof(T));
                        for (int valueIndex = 0; valueIndex != array.Length;)
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                                do
                                {
                                    persistenceBuffer.Reset();
                                    do
                                    {
                                        T value = array.Array[valueIndex];
                                        if (persistenceBuffer.TrySetCurrentIndex())
                                        {
                                            if (!snapshotMethodSerializer.Serialize(value))
                                            {
                                                persistenceBuffer.RestoreCurrentIndex();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            snapshotMethodSerializer.Serialize(value);
                                            if (persistenceBuffer.CheckDataStart())
                                            {
                                                ++valueIndex;
                                                break;
                                            }
                                        }
                                    }
                                    while (++valueIndex != array.Length);
                                    if (isClosedOrServiceDisposed) return false;
                                    outputData = persistenceBuffer.GetData();
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                }
                                while (valueIndex != array.Length && !persistenceBuffer.CheckNewBuffer());
                            }
                        }
                        for (int valueIndex = 0; valueIndex != newArray.Length;)
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                                do
                                {
                                    persistenceBuffer.Reset();
                                    do
                                    {
                                        T value = newArray.Array[valueIndex];
                                        if (persistenceBuffer.TrySetCurrentIndex())
                                        {
                                            if (!snapshotMethodSerializer.Serialize(value))
                                            {
                                                persistenceBuffer.RestoreCurrentIndex();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            snapshotMethodSerializer.Serialize(value);
                                            if (persistenceBuffer.CheckDataStart())
                                            {
                                                ++valueIndex;
                                                break;
                                            }
                                        }
                                    }
                                    while (++valueIndex != newArray.Length);
                                    if (isClosedOrServiceDisposed) return false;
                                    outputData = persistenceBuffer.GetData();
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                }
                                while (valueIndex != newArray.Length && !persistenceBuffer.CheckNewBuffer());
                            }
                        }
                    }
                    persistencePosition = persistenceStream.Position;
                }
                return true;
            }
            finally
            {
                persistenceBuffer.Free();
                outputSerializer?.FreeContext();
            }
        }
        /// <summary>
        /// 当前节点持久化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="newArray"></param>
        /// <returns></returns>
        internal unsafe bool RebuildSnapshotClone<T>(ref LeftArray<T> array, ref LeftArray<T> newArray) where T : SnapshotCloneObject<T>
        {
            bool isPersistence = false;
            var outputSerializer = default(BinarySerializer);
            object snapshotLock = this;
            PersistenceBuffer persistenceBuffer = new PersistenceBuffer(Service);
            try
            {
                if (isClosedOrServiceDisposed) return false;
                using (FileStream persistenceStream = new FileStream(persistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, persistenceBuffer.SendBufferMaxSize, FileOptions.None))
                {
                    if (!checkPersistencePosition(persistenceStream)) return false;
                    persistenceBuffer.GetBufferLength();
                    SubArray<byte> outputData;
                    using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(CommandServerSocket.CommandServerSocketContext))
                    {
                        outputSerializer.SetDefault();
                        persistenceBuffer.OutputStream = outputStream;
                        fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                        {
                            persistenceBuffer.SetStart(dataFixed);
                            persistenceBuffer.Reset();
                            node.notNull().CreateNodeMethodParameter.notNull().PersistenceSerialize(outputSerializer, persistencePosition);
                            outputData = persistenceBuffer.GetData();
                        }
                        if (isClosedOrServiceDisposed) return false;
                        persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);

                        SnapshotMethodSerializer snapshotMethodSerializer = new SnapshotMethodSerializer(outputSerializer, node.notNull(), typeof(T));
                        for (int valueIndex = 0; valueIndex != array.Length;)
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                                do
                                {
                                    persistenceBuffer.Reset();
                                    do
                                    {
                                        T value = array.Array[valueIndex];
                                        if (persistenceBuffer.TrySetCurrentIndex())
                                        {
                                            Monitor.Enter(value);
                                            snapshotLock = value;
                                            if (snapshotMethodSerializer.Serialize(value.SnapshotValue))
                                            {
                                                value.SnapshotValue = null;
                                                Monitor.Exit(value);
                                                snapshotLock = this;
                                            }
                                            else
                                            {
                                                Monitor.Exit(value);
                                                snapshotLock = this;
                                                persistenceBuffer.RestoreCurrentIndex();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Monitor.Enter(value);
                                            snapshotLock = value;
                                            snapshotMethodSerializer.Serialize(value.SnapshotValue);
                                            value.SnapshotValue = null;
                                            Monitor.Exit(value);
                                            snapshotLock = this;
                                            if (persistenceBuffer.CheckDataStart())
                                            {
                                                ++valueIndex;
                                                break;
                                            }
                                        }
                                    }
                                    while (++valueIndex != array.Length);
                                    if (isClosedOrServiceDisposed) return false;
                                    outputData = persistenceBuffer.GetData();
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                }
                                while (valueIndex != array.Length && !persistenceBuffer.CheckNewBuffer());
                            }
                        }
                        for (int valueIndex = 0; valueIndex != newArray.Length;)
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                                do
                                {
                                    persistenceBuffer.Reset();
                                    do
                                    {
                                        T value = newArray.Array[valueIndex];
                                        if (persistenceBuffer.TrySetCurrentIndex())
                                        {
                                            Monitor.Enter(value);
                                            snapshotLock = value;
                                            if (snapshotMethodSerializer.Serialize(value.SnapshotValue))
                                            {
                                                value.SnapshotValue = null;
                                                Monitor.Exit(value);
                                                snapshotLock = this;
                                            }
                                            else
                                            {
                                                Monitor.Exit(value);
                                                snapshotLock = this;
                                                persistenceBuffer.RestoreCurrentIndex();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Monitor.Enter(value);
                                            snapshotLock = value;
                                            snapshotMethodSerializer.Serialize(value.SnapshotValue);
                                            value.SnapshotValue = null;
                                            Monitor.Exit(value);
                                            snapshotLock = this;
                                            if (persistenceBuffer.CheckDataStart())
                                            {
                                                ++valueIndex;
                                                break;
                                            }
                                        }
                                    }
                                    while (++valueIndex != newArray.Length);
                                    if (isClosedOrServiceDisposed) return false;
                                    outputData = persistenceBuffer.GetData();
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                }
                                while (valueIndex != newArray.Length && !persistenceBuffer.CheckNewBuffer());
                            }
                        }
                    }
                    persistencePosition = persistenceStream.Position;
                }
                isPersistence = true;
                return true;
            }
            finally
            {
                if (!object.ReferenceEquals(snapshotLock, this)) Monitor.Exit(snapshotLock);
                persistenceBuffer.Free();
                outputSerializer?.FreeContext();
                if (!isPersistence)
                {
                    foreach (T value in array) value.CancelSnapshotValue();
                    foreach (T value in newArray) value.CancelSnapshotValue();
                }
            }
        }
        /// <summary>
        /// 当前节点持久化操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        internal unsafe bool Rebuild<T>(ISnapshotEnumerable<T> values)
        {
            var outputSerializer = default(BinarySerializer);
            PersistenceBuffer persistenceBuffer = new PersistenceBuffer(Service);
            try
            {
                if (isClosedOrServiceDisposed) return false;
                using (FileStream persistenceStream = new FileStream(persistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, persistenceBuffer.SendBufferMaxSize, FileOptions.None))
                {
                    if (!checkPersistencePosition(persistenceStream)) return false;
                    persistenceBuffer.GetBufferLength();
                    SubArray<byte> outputData;
                    using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(CommandServerSocket.CommandServerSocketContext))
                    {
                        outputSerializer.SetDefault();
                        persistenceBuffer.OutputStream = outputStream;
                        fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                        {
                            persistenceBuffer.SetStart(dataFixed);
                            persistenceBuffer.Reset();
                            node.notNull().CreateNodeMethodParameter.notNull().PersistenceSerialize(outputSerializer, persistencePosition);
                            outputData = persistenceBuffer.GetData();
                        }
                        if (isClosedOrServiceDisposed) return false;
                        persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);

                        SnapshotMethodSerializer snapshotMethodSerializer = new SnapshotMethodSerializer(outputSerializer, node.notNull(), typeof(T));
                        IEnumerator<T> enumerator = values.SnapshotValues.GetEnumerator();
                        bool isCurrentValue = false;
                        do
                        {
                            fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                            {
                                persistenceBuffer.SetStart(dataFixed);
                                do
                                {
                                    persistenceBuffer.Reset();
                                    do
                                    {
                                        if (!isCurrentValue && !enumerator.MoveNext())
                                        {
                                            if (persistenceBuffer.Count != 0)
                                            {
                                                outputData = persistenceBuffer.GetData();
                                                persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                            }
                                            goto SETPOSITION;
                                        }
                                        T value = enumerator.Current;
                                        if (persistenceBuffer.TrySetCurrentIndex())
                                        {
                                            if (!snapshotMethodSerializer.Serialize(value))
                                            {
                                                persistenceBuffer.RestoreCurrentIndex();
                                                isCurrentValue = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            snapshotMethodSerializer.Serialize(value);
                                            if (persistenceBuffer.CheckDataStart())
                                            {
                                                isCurrentValue = false;
                                                break;
                                            }
                                        }
                                        isCurrentValue = false;
                                    }
                                    while (true);
                                    if (isClosedOrServiceDisposed) return false;
                                    outputData = persistenceBuffer.GetData();
                                    persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                }
                                while (!persistenceBuffer.CheckNewBuffer());
                            }
                        }
                        while (true);
                    }
                SETPOSITION:
                    persistencePosition = persistenceStream.Position;
                }
                return true;
            }
            finally
            {
                values.CloseSnapshot();
                persistenceBuffer.Free();
                outputSerializer?.FreeContext();
            }
        }
        /// <summary>
        /// 添加调用队列
        /// </summary>
        /// <param name="methodParameter"></param>
        internal void PushQueue(MethodParameter methodParameter)
        {
            bool isPushQueue = false;
            try
            {
                //methodParameter = methodParameter.Clone();
                //methodParameter.LinkNext = null;
                //persistenceQueue.IsPushHead(methodParameter);
                persistenceQueue.IsPushHead(methodParameter.Clone());
                isPushQueue = true;
            }
            finally
            {
                if (!isPushQueue) Close();
            }
        }
        /// <summary>
        /// 检查调用队列
        /// </summary>
        internal void CheckQueue()
        {
            if (!persistenceQueue.IsEmpty)
            {
                bool isPersistence = false;
                try
                {
                    if (!isClosedOrServiceDisposed)
                    {
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(queuePersistence);
                        isPersistence = true;
                    }
                }
                finally
                {
                    if (!isPersistence) Close();
                }
            }
            else Service.SetRebuilderPersistenceWaitting();
        }
        /// <summary>
        /// 调用队列持久化
        /// </summary>
        private unsafe void queuePersistence()
        {
            bool isPersistence = false;
            try
            {
                if (isClosedOrServiceDisposed) return;
                if (persistence())
                {
                    Service.CommandServerCallQueue.AppendReadOnly(new PersistenceRebuilderCallback(this, PersistenceRebuilderCallbackTypeEnum.CheckQueue));
                    isPersistence = true;
                }
            }
            finally
            {
                if (!isPersistence) Close();
            }
        }
        /// <summary>
        /// 调用队列持久化
        /// </summary>
        /// <returns></returns>
        private unsafe bool persistence()
        {
            var outputSerializer = default(BinarySerializer);
            var positionStream = default(FileStream);
            PersistenceBuffer persistenceBuffer = new PersistenceBuffer(Service);
            try
            {
                if (isClosedOrServiceDisposed) return false;
                fixed (byte* positionBufferFixed = persistenceDataPositionBuffer)
                {
                    using (FileStream persistenceStream = new FileStream(persistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, persistenceBuffer.SendBufferMaxSize, FileOptions.None))
                    {
                        if (!checkPersistencePosition(persistenceStream)) return false;
                        persistenceBuffer.GetBufferLength();
                        SubArray<byte> outputData;
                        var current = default(MethodParameter);
                        using (UnmanagedStream outputStream = (outputSerializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer()).SetContext(CommandServerSocket.CommandServerSocketContext))
                        {
                            outputSerializer.SetDefault();
                            persistenceBuffer.OutputStream = outputStream;
                            do
                            {
                                fixed (byte* dataFixed = persistenceBuffer.OutputBuffer.GetFixedBuffer())
                                {
                                    persistenceBuffer.SetStart(dataFixed);
                                    do
                                    {
                                        if (current == null)
                                        {
                                            current = persistenceQueue.GetQueue();
                                            if (current == null)
                                            {
                                                persistenceStream.Flush();
                                                current = persistenceQueue.GetQueue();
                                                if (current == null)
                                                {
                                                    persistencePosition = persistenceStream.Position;
                                                    return true;
                                                }
                                            }
                                        }
                                        persistenceBuffer.Reset();
                                        do
                                        {
                                            if (!current.IsPersistenceCallback)
                                            {
                                                if (positionStream == null) positionStream = new FileStream(persistenceCallbackExceptionPositionFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, 12 << 10, FileOptions.None);
                                                *(long*)positionBufferFixed = persistencePosition + persistenceBuffer.Count;
                                                positionStream.Write(persistenceDataPositionBuffer, 0, sizeof(long));
                                            }
                                            if (persistenceBuffer.TrySetCurrentIndex())
                                            {
                                                current = current.PersistenceSerialize(outputSerializer);
                                                if (persistenceBuffer.CheckResizeError()) break;
                                            }
                                            else
                                            {
                                                current = current.PersistenceSerialize(outputSerializer);
                                                if (persistenceBuffer.CheckDataStart()) break;
                                            }
                                        }
                                        while (current != null || (current = persistenceQueue.GetQueue()) != null);
                                        if (isClosedOrServiceDisposed) return false;
                                        outputData = persistenceBuffer.GetData();
                                        persistenceStream.Write(outputData.Array, outputData.Start, outputData.Length);
                                    }
                                    while (!persistenceBuffer.CheckNewBuffer());
                                }
                            }
                            while (true);
                        }
                    }
                }
            }
            finally
            {
                persistenceBuffer.Free();
                outputSerializer?.FreeContext();
                if (positionStream != null)
                {
                    long position = positionStream.Position;
                    positionStream.Dispose();
                    persistenceCallbackExceptionFilePosition = position;
                }
            }
        }
        /// <summary>
        /// 未完成调用队列持久化
        /// </summary>
        /// <returns></returns>
        internal unsafe bool QueuePersistence()
        {
            bool isPersistence = false;
            try
            {
                if (!isClosedOrServiceDisposed)
                {
                    if (persistenceQueue.IsEmpty || persistence())
                    {
                        if (!isClosedOrServiceDisposed)
                        {
                            ulong newRebuildPosition = Service.RebuildPersistenceEndPosition;
                            if (newRebuildPosition != rebuildPosition)
                            {
                                fixed (byte* bufferFixed = persistenceDataPositionBuffer)
                                {
                                    *(uint*)bufferFixed = ServiceLoader.PersistenceCallbackExceptionPositionFileHead;
                                    *(ulong*)(bufferFixed + sizeof(int)) = newRebuildPosition;
                                    using (FileStream fileStream = new FileStream(persistenceCallbackExceptionPositionFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, 4 << 10, FileOptions.None))
                                    {
                                        fileStream.Write(persistenceDataPositionBuffer, 0, ServiceLoader.ExceptionPositionFileHeadSize);
                                    }
                                    *(uint*)bufferFixed = ServiceLoader.FieHead;
                                    *(long*)(bufferFixed + (sizeof(int) + sizeof(ulong))) = persistencePosition;
                                    using (FileStream fileStream = new FileStream(persistenceFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None, 4 << 10, FileOptions.None))
                                    {
                                        fileStream.Write(persistenceDataPositionBuffer, 0, ServiceLoader.FileHeadSize);
                                    }
                                }
                            }
                            string backupFileNameSuffix = Service.Config.GetBackupFileNameSuffix() + ".bak";
                            File.Move(Service.PersistenceFileInfo.FullName, Service.PersistenceFileInfo.FullName + backupFileNameSuffix);
                            File.Move(Service.PersistenceCallbackExceptionPositionFileInfo.FullName, Service.PersistenceCallbackExceptionPositionFileInfo.FullName + backupFileNameSuffix);
                            File.Move(persistenceCallbackExceptionPositionFileInfo.FullName, Service.PersistenceCallbackExceptionPositionSwitchFileInfo.FullName);
                            File.Move(persistenceFileInfo.FullName, Service.PersistenceSwitchFileInfo.FullName);
                            Service.SetRebuild(persistencePosition, persistenceCallbackExceptionFilePosition, rebuildSnapshotPosition);
                            return isPersistence = true;
                        }
                    }
                }
            }
            finally
            {
                if (!isPersistence) Close();
            }
            return false;
        }
    }
}
