using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块服务
    /// </summary>
    public class DiskBlockService : IDiskBlockService, ICommandServerBindController, IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 服务端执行队列
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal CommandServerCallQueue CommandServerCallQueue;
        /// <summary>
        /// 获取磁盘块当前写入位置委托集合
        /// </summary>
        private CommandServerKeepCallback<long>.Link getPositions;
        /// <summary>
        /// 数据缓存
        /// </summary>
        private readonly Dictionary<HashBytes, HeadLeftArray<long>> dataCache = DictionaryCreator<HashBytes>.Create<HeadLeftArray<long>>();
        /// <summary>
        /// 数据缓存访问锁
        /// </summary>
        private readonly object dataCacheLock = new object();
        /// <summary>
        /// 数据缓存
        /// </summary>
        private readonly ReusableDictionary<long, HashBytes> indexCache = new ReusableDictionary<long, HashBytes>(0, ReusableDictionaryGroupTypeEnum.Roll);
        /// <summary>
        /// 写入数据请求缓存
        /// </summary>
        private readonly Dictionary<HashBytes, WriteRequest> writeCache = DictionaryCreator<HashBytes>.Create<WriteRequest>();
        /// <summary>
        /// 写入数据缓存访问锁
        /// </summary>
        private readonly object writeCacheLock = new object();
        /// <summary>
        /// 数据写入请求链表
        /// </summary>
        private LinkStack<WriteRequest> writeQueue;
        /// <summary>
        /// 磁盘块服务唯一编号
        /// </summary>
        internal readonly uint Identity;
        /// <summary>
        /// 单个缓存最大字节数
        /// </summary>
        private readonly int maxCacheSize;
        /// <summary>
        /// 缓存最大总字节数
        /// </summary>
        private readonly long maxCacheTotalSize;
        /// <summary>
        /// 磁盘块集合访问锁
        /// </summary>
        private readonly object blockLock = new object();
        /// <summary>
        /// 磁盘块集合
        /// </summary>
        private LeftArray<Block> blocks;
        /// <summary>
        /// 当前操作磁盘块
        /// </summary>
        internal Block Block;
        /// <summary>
        /// 切换磁盘块最小字节数
        /// </summary>
        internal readonly long MinSwitchSize;
        /// <summary>
        /// 当前缓存字节数
        /// </summary>
        private long currentCacheSize;
        /// <summary>
        /// 自动写入间隔时钟周期
        /// </summary>
        private readonly long autoFlushTimestamp;
        /// <summary>
        /// 自动写入字节大小
        /// </summary>
        private readonly int autoFlushSize;
        /// <summary>
        /// 是否已经启动写入操作
        /// </summary>
        private int isWrite;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal bool IsDisposed;
        /// <summary>
        /// 默认空磁盘块服务
        /// </summary>
        /// <param name="block"></param>
        internal DiskBlockService(Block block)
        {
            Block = block;
        }
        /// <summary>
        /// 磁盘块服务
        /// </summary>
        /// <param name="config">磁盘块服务配置</param>
        /// <param name="blockCapacity">磁盘块集合初始化大小</param>
        public DiskBlockService(DiskBlockServiceConfig config, int blockCapacity = 0)
        {
            Identity = config.Identity;
            MinSwitchSize = Math.Max(config.MinSwitchSize, 1);
            autoFlushSize = Math.Max(config.AutoFlushSize, 4 << 10);
            autoFlushTimestamp = AutoCSer.Date.GetTimestampByMilliseconds(Math.Max(config.AutoFlushMilliseconds, 1));
            maxCacheSize = Math.Max(config.MaxCacheSize, 0);
            maxCacheTotalSize = Math.Max(config.MaxCacheTotalSize, maxCacheSize);
            blocks = new LeftArray<Block>(blockCapacity);
            Block = NullBlock.Null;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            IsDisposed = true;
            Block.Dispose();
            foreach (Block block in blocks) block.Dispose();
            dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public virtual async ValueTask DisposeAsync()
        {
            IsDisposed = true;
            await Block.DisposeAsync();
            foreach (Block block in blocks) await block.DisposeAsync();
            dispose();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        private void dispose()
        {
            if (!object.ReferenceEquals(Block, NullBlock.Null)) CommandServerCallQueue.AddOnly(new BlockCallback(BlockCallbackTypeEnum.Dispose, Block));
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void DisposeCallback()
        {
            if(dataCache.Count != 0) indexCache.ClearCount();
            Monitor.Enter(dataCacheLock);
            try
            {
                if (dataCache.Count != 0) dataCache.Clear();
            }
            finally { Monitor.Exit(dataCacheLock); }

            getPositions.CancelKeep();
        }
        /// <summary>
        /// 绑定命令服务控制器
        /// </summary>
        /// <param name="controller"></param>
        void ICommandServerBindController.Bind(CommandServerController controller)
        {
            CommandServerCallQueue = controller.CallQueue.notNull();
        }
        /// <summary>
        /// 设置当前操作磁盘块
        /// </summary>
        /// <param name="block"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(Block block)
        {
            blocks.Add(block);
            Block = block;
        }
        /// <summary>
        /// 添加磁盘块
        /// </summary>
        /// <param name="block"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(Block block)
        {
            blocks.Add(block);
        }
        /// <summary>
        /// 切换当前操作磁盘块
        /// </summary>
        /// <param name="block"></param>
        internal void SetSwitch(Block block)
        {
            Monitor.Enter(blockLock);
            try
            {
                blocks.Insert(0, block);
            }
            finally { Monitor.Exit(blockLock); }
            Block = block;
        }
        /// <summary>
        /// 获取磁盘块当前写入位置
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        public void GetPosition(CommandServerSocket socket, CommandServerCallQueue queue, uint identity, CommandServerKeepCallback<long> callback)
        {
            bool isCallback = true;
            try
            {
                if (!IsDisposed)
                {
                    if (identity == this.Identity && object.ReferenceEquals(queue, CommandServerCallQueue))
                    {
                        if (callback.Callback(Block.Position))
                        {
                            getPositions.PushHead(callback);
                            isCallback = false;
                        }
                    }
                    else callback.Callback(long.MinValue);
                }
            }
            finally 
            {
                if (isCallback) callback.CancelKeep();
            }
        }
        /// <summary>
        /// 写入数据回调操作
        /// </summary>
        /// <param name="position"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FlushCallback(long position)
        {
            getPositions.Callback(position);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">写入数据缓冲区，来源于同步 IO 数据缓冲区，需要同步处理数据，否则需要复制数据</param>
        /// <param name="callback">写入数据起始位置</param>
        public void Write(CommandServerSocket socket, WriteBuffer buffer, CommandServerCallback<BlockIndex> callback)
        {
            var request = default(WriteRequest);
            BlockIndex index = new BlockIndex(Identity);
            bool isCallback = true;
            try
            {
                bool isIndex;
                BlockIndex indexSize = BlockIndex.GetIndexSize(ref buffer.Buffer, out isIndex);
                if (isIndex)
                {
                    index = indexSize;
                    return;
                }
                if (IsDisposed) return;

                HashBytes hashBytes = buffer.Buffer;
                int size = buffer.Buffer.Length;
                if (size <= maxCacheSize)
                {
                    HeadLeftArray<long> indexs;
                    Monitor.Enter(dataCacheLock);
                    if (dataCache.TryGetValue(hashBytes, out indexs))
                    {
                        Monitor.Exit(dataCacheLock);
                        index.Set(indexs.Head, size);
                        CommandServerCallQueue.AddOnly(new BlockCallback(BlockCallbackTypeEnum.CheckServiceIndexCacheNode, Block, index.Index));
                        return;
                    }
                    Monitor.Exit(dataCacheLock);
                }

                var cacheRequest = default(WriteRequest);
                Monitor.Enter(writeCacheLock);
                if (writeCache.TryGetValue(hashBytes, out cacheRequest))
                {
                    try
                    {
                        cacheRequest.AppendCallback(callback);
                        isCallback = false;
                    }
                    finally { Monitor.Exit(writeCacheLock); }
                    return;
                }
                Monitor.Exit(writeCacheLock);

                request = new WriteRequest(ref index, ref hashBytes, callback);
                isCallback = false;
                hashBytes = request.GetHashBytes();

                Monitor.Enter(writeCacheLock);
                if (writeCache.TryGetValue(hashBytes, out cacheRequest))
                {
                    try
                    {
                        cacheRequest.AppendCallback(request);
                        request = null;
                    }
                    finally { Monitor.Exit(writeCacheLock); }
                    return;
                }
                Monitor.Exit(writeCacheLock);

                if (writeQueue.IsPushHead(request))
                {
                    request = null;
                    if (Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) write().NotWait();
                }
                else request = null;
            }
            finally 
            {
                request?.CancelCallback(ref index);
                if (isCallback) callback.Callback(index);
            }
        }
        /// <summary>
        /// 检查磁盘块服务缓存位置索引节点
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CheckIndexCacheNode(long index)
        {
            indexCache.ContainsKey(index, true);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <returns></returns>
        private async Task write()
        {
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            do
            {
                do
                {
                    var head = writeQueue.GetQueue().notNull();
                    var request = head;
                    long flushPosition = Block.Position, flushTimestamp = Stopwatch.GetTimestamp() + autoFlushTimestamp;
                    do
                    {
                        try
                        {
                            do
                            {
                                if (!IsDisposed)
                                {
                                    long nextPosition = await Block.Write(request);
                                    if (nextPosition > 0)
                                    {
                                        if (nextPosition - flushPosition >= autoFlushSize || (Stopwatch.GetTimestamp() >= flushTimestamp && nextPosition > flushPosition))
                                        {
                                            flushPosition = await Block.Flush();
                                            CommandServerCallQueue.AddOnly(new BlockCallback(BlockCallbackTypeEnum.Flush, Block));
                                            while (head != request) head = free(head.notNull());
                                            head = free(head);
                                            flushTimestamp = Stopwatch.GetTimestamp() + autoFlushTimestamp;
                                        }
                                    }
                                }
                                else break;
                                request = request.LinkNext;
                            }
                            while (request != null);
                            break;
                        }
                        catch (Exception exception)
                        {
                            if (!exceptionRepeat.IsRepeat(exception)) await AutoCSer.LogHelper.Exception(exception);
                        }
                        request = request.notNull().LinkNext;
                    }
                    while (request != null);
                    try
                    {
                        if (flushPosition != await Block.Flush()) CommandServerCallQueue.AddOnly(new BlockCallback(BlockCallbackTypeEnum.Flush, Block));
                    }
                    catch (Exception exception)
                    {
                        await AutoCSer.LogHelper.Exception(exception);
                    }
                    do
                    {
                        try
                        {
                            while (head != null) head = free(head);
                            break;
                        }
                        catch (Exception exception)
                        {
                            await AutoCSer.LogHelper.Exception(exception);
                        }
                        head = head.notNull().LinkNext;
                    }
                    while (head != null);
                }
                while (!writeQueue.IsEmpty);
                Interlocked.Exchange(ref isWrite, 0);
            }
            while (!writeQueue.IsEmpty && Interlocked.CompareExchange(ref isWrite, 1, 0) == 0);
        }
        /// <summary>
        /// 移除写入数据请求缓存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
#if NetStandard21
        private WriteRequest? free(WriteRequest request)
#else
        private WriteRequest free(WriteRequest request)
#endif
        {
            if (request.OperationType == WriteOperationTypeEnum.Append)
            {
                HashBytes hashBytes = request.GetHashBytes();
                Monitor.Enter(writeCacheLock);
                writeCache.Remove(hashBytes);
                Monitor.Exit(writeCacheLock);
            }
            return request.FreeCallback();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        public void Read(CommandServerSocket socket, CommandServerCallQueue queue, BlockIndex index, CommandServerCallback<ReadBuffer> callback)
        {
            ReadBufferStateEnum state = index.GetReadState();
            bool isCallback = true;
            try
            {
                if (state != ReadBufferStateEnum.Unknown) return;
                if (index.Identity != Identity)
                {
                    state = ReadBufferStateEnum.Identity;
                    return;
                }
                if (IsDisposed)
                {
                    state = ReadBufferStateEnum.Disposed;
                    return;
                }

                HashBytes buffer;
                if (index.Size <= (uint)maxCacheSize && indexCache.TryGetValue(index.Index, out buffer, true))
                {
                    if (buffer.SubArray.Length == index.Size)
                    {
                        callback.SynchronousCallback(new ReadBuffer(ref buffer.SubArray));
                        isCallback = false;
                    }
                    else state = ReadBufferStateEnum.Size;
                    return;
                }

                var block = getReadBlock(index.Index);
                if (block != null) block.Read(ref index, callback, ref isCallback);
                else state = ReadBufferStateEnum.BlockIndex;
            }
            finally 
            {
                if (isCallback) callback.SynchronousCallback(new ReadBuffer(state));
            }
        }
        /// <summary>
        /// 根据索引获取磁盘块
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
#if NetStandard21
        private Block? getReadBlock(long index)
#else
        private Block getReadBlock(long index)
#endif
        {
            if (Block.StartIndex <= index) return Block;
            if (blocks.Length > 1)
            {
                foreach (Block block in blocks)
                {
                    if (block.StartIndex <= index) return block;
                }
            }
            return null;
        }
        /// <summary>
        /// 读取数据请求回调操作
        /// </summary>
        /// <param name="request"></param>
        internal void ReadCallback(ReadRequest request)
        {
            bool isRemoveCache = false;
            long index = request.Index;
            try
            {
                SubArray<byte> buffer = request.Buffer.Buffer;
                if (buffer.Array != null && buffer.Length <= maxCacheSize && !indexCache.ContainsKey(index, true))
                {
                    HeadLeftArray<long> indexs;
                    HashBytes hashBytes = buffer;
                    Monitor.Enter(dataCacheLock);
                    try
                    {
                        if (dataCache.TryGetValue(hashBytes, out indexs))
                        {
                            indexCache.TryGetValue(indexs.Head, out hashBytes);
                            indexs.Array.PrepLength(1);

                            indexCache.Set(index, hashBytes, true);
                            isRemoveCache = true;
                            if (index > indexs.Head) indexs.AddHead(index);
                            else indexs.Add(index);
                            dataCache[hashBytes] = indexs;
                        }
                        else
                        {
                            indexCache.Set(index, hashBytes, true);
                            isRemoveCache = true;
                            dataCache.Add(hashBytes, new HeadLeftArray<long>(index));
                            currentCacheSize += buffer.Length;
                        }
                    }
                    finally { Monitor.Exit(dataCacheLock); }
                    isRemoveCache = false;
                }
            }
            finally
            {
                HashBytes hashBytes;
                if (isRemoveCache) indexCache.Remove(index, out hashBytes);
                if (currentCacheSize > maxCacheTotalSize)
                {
                    HeadLeftArray<long> indexs;
                    Monitor.Enter(dataCacheLock);
                    try
                    {
                        while (currentCacheSize > maxCacheTotalSize && indexCache.RemoveRoll(out hashBytes))
                        {
                            bool isCache = false;
                            if (dataCache.Remove(hashBytes, out indexs))
                            {
                                if (indexs.Head == index)
                                {
                                    if (indexs.Array.Length > 0)
                                    {
                                        setHead(ref indexs);
                                        isCache = true;
                                    }
                                }
                                else
                                {
                                    int removeIndex = indexs.Array.IndexOf(index);
                                    if (removeIndex >= 0) indexs.Array.RemoveToEnd(removeIndex);
                                    isCache = true;
                                }
                            }
                            if (isCache) dataCache[hashBytes] = indexs;
                            else currentCacheSize -= hashBytes.SubArray.Length;
                        }
                    }
                    finally { Monitor.Exit(dataCacheLock); }
                }
            }
        }
        /// <summary>
        /// 设置最大索引为头节点
        /// </summary>
        /// <param name="indexs"></param>
        private static void setHead(ref HeadLeftArray<long> indexs)
        {
            int maxIndex = 0, index = indexs.Array.Length;
            if (index > 1)
            {
                long[] array = indexs.Array.Array;
                do
                {
                    --index;
                    if (array[index] > array[maxIndex]) maxIndex = index;
                }
                while (index > 1);
            }
            indexs.ArrayToHead(maxIndex);
        }

        /// <summary>
        /// 切换磁盘块（正常情况下，只有在需要清理历史垃圾数据时才需要切换磁盘块，切换磁盘块以后，需要自行处理掉所有历史引用，比如可以将数据写入新的磁盘块并更新历史引用，然后删除垃圾磁盘块）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="callback">磁盘块当前写入位置</param>
        public void SwitchBlock(CommandServerSocket socket, uint identity, CommandServerCallback<BlockIndex> callback)
        {
            var request = default(WriteRequest);
            BlockIndex index = new BlockIndex(long.MinValue, BlockIndex.ErrorSize, Identity);
            bool isCallback = true;
            try
            {
                if (IsDisposed) return;
                request = new WriteRequest(WriteOperationTypeEnum.SwitchBlock, callback);
                isCallback = false;
                if (writeQueue.IsPushHead(request))
                {
                    request = null;
                    if (Interlocked.CompareExchange(ref isWrite, 1, 0) == 0) write().NotWait();
                }
                else request = null;
            }
            finally
            {
                request?.CancelCallback(ref index);
                if (isCallback) callback.Callback(index);
            }
        }
        /// <summary>
        /// 获取磁盘块信息集合
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <returns>null 表示失败</returns>
#if NetStandard21
        public BlockInfo[]? GetBlocks(CommandServerSocket socket, uint identity)
#else
        public BlockInfo[] GetBlocks(CommandServerSocket socket, uint identity)
#endif
        {
            return identity == Identity ? blocks.GetArray(p => new BlockInfo(p)) : null;
        }
        /// <summary>
        /// 删除垃圾磁盘块（只有在确认不再存在引用的情况下才删除）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="identity">磁盘块服务唯一编号</param>
        /// <param name="startIndex">磁盘块起始位置，不允许删除当前写操作的磁盘块</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteBlock(CommandServerSocket socket, uint identity, long startIndex)
        {
            if (identity == Identity && blocks.Length > 1)
            {
                var block = default(Block);
                Monitor.Enter(blockLock);
                try
                {
                    int index = blocks.IndexOf(p => p.StartIndex == startIndex);
                    if (index > 0) block = blocks.Array[index];
                }
                finally { Monitor.Exit(blockLock); }
                if (block != null && await block.Delete())
                {
                    Monitor.Enter(blockLock);
                    try
                    {
                        int index = blocks.IndexOf(p => p.StartIndex == startIndex);
                        if (index > 0) blocks.RemoveToEnd(index);
                    }
                    finally { Monitor.Exit(blockLock); }
                }
            }
            return false;
        }
    }
}
