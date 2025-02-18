using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 分词结果磁盘块索引信息
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    public abstract class WordIdentityBlockIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 操作队列访问锁
        /// </summary>
        private System.Threading.SemaphoreSlim queueLock;
        /// <summary>
        /// 磁盘块索引信息
        /// </summary>
        internal BlockIndex BlockIndex;
        /// <summary>
        /// 操作回调集合
        /// </summary>
#if NetStandard21
        private MethodCallback<WordIdentityBlockIndexUpdateStateEnum>? callbacks;
#else
        private MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callbacks;
#endif
        /// <summary>
        /// 分词结果磁盘块索引信息
        /// </summary>
        protected WordIdentityBlockIndex()
        {
            BlockIndex.SetBinarySerializeNullValue();
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 分词结果磁盘块索引信息
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        protected WordIdentityBlockIndex(BlockIndex blockIndex)
        {
            BlockIndex = blockIndex;
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal async Task Create(WordIdentityBlockIndexNode<T> node, T key)
        {
            await queueLock.WaitAsync();
            try
            {
                if (BlockIndex.IsBinarySerializeNullValue)
                {
                    await AutoCSer.Threading.SwitchAwaiter.Default;
                    var text = await node.GetText(key);
                    if (text != null)
                    {
                        int[] wordIdentitys;
                        if (text.Length != 0)
                        {
                            ResponseResult<int[]> identitys = await node.GetWordIdentitys(text);
                            if (!identitys.IsSuccess) return;
                            wordIdentitys = identitys.Value.notNull();
                            if (wordIdentitys.Length > 1) wordIdentitys.Sort();
                        }
                        else wordIdentitys = EmptyArray<int>.Array;
                        if (wordIdentitys.Length != 0)
                        {
                            ResponseResult result = await node.AppendIndex(wordIdentitys, key);
                            if (!result.IsSuccess) return;
                        }
                        bool isIndex;
                        BlockIndex blockIndex = BlockIndex.GetIndexSize(wordIdentitys, out isIndex);
                        if (!isIndex)
                        {
                            CommandClientReturnValue<BlockIndex> blockIndexResult = await node.GetDiskBlockClient(key).ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(wordIdentitys));
                            if (!blockIndexResult.IsSuccess) return;
                            blockIndex = blockIndexResult.Value;
                        }
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, 0);
                    }
                    else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key);
                }
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally { queueLock.Release(); }
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal async Task Create(WordIdentityBlockIndexNode<T> node, T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            int isCallback = 0;
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                if (BlockIndex.IsBinarySerializeNullValue)
                {
                    await AutoCSer.Threading.SwitchAwaiter.Default;
                    var text = await node.GetText(key);
                    if (text != null)
                    {
                        int[] wordIdentitys;
                        if (text.Length != 0)
                        {
                            ResponseResult<int[]> identitys = await node.GetWordIdentitys(text);
                            if (!identitys.IsSuccess)
                            {
                                state = WordIdentityBlockIndexUpdateStateEnum.GetWordIdentityFailed;
                                return;
                            }
                            wordIdentitys = identitys.Value.notNull();
                            if (wordIdentitys.Length > 1) wordIdentitys.Sort();
                        }
                        else wordIdentitys = EmptyArray<int>.Array;
                        if (wordIdentitys.Length != 0)
                        {
                            ResponseResult result = await node.AppendIndex(wordIdentitys, key);
                            if (!result.IsSuccess)
                            {
                                state = WordIdentityBlockIndexUpdateStateEnum.SetWordIndexFailed;
                                return;
                            }
                        }
                        bool isIndex;
                        BlockIndex blockIndex = BlockIndex.GetIndexSize(wordIdentitys, out isIndex);
                        if (!isIndex)
                        {
                            CommandClientReturnValue<BlockIndex> blockIndexResult = await node.GetDiskBlockClient(key).ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(wordIdentitys));
                            if (!blockIndexResult.IsSuccess)
                            {
                                state = WordIdentityBlockIndexUpdateStateEnum.GetBlockIndexFailed;
                                return;
                            }
                            blockIndex = blockIndexResult.Value;
                        }
                        setCallback(node, callback);
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, callback.Reserve);
                    }
                    else
                    {
                        setCallback(node, callback);
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key);
                    }
                    isCallback = 1;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.Success;
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally { releaseQueueLock(callback, isCallback, state); }
        }
        /// <summary>
        /// 释放操作队列访问锁
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="isCallback"></param>
        /// <param name="state"></param>
        private void releaseQueueLock(MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback, int isCallback, WordIdentityBlockIndexUpdateStateEnum state)
        {
            if (callback.Reserve != 0)
            {
                if (isCallback != 0) queueLock.Release();
                else
                {
                    try
                    {
                        this.callback(callback.Reserve, state);
                    }
                    finally { queueLock.Release(); }
                }
            }
            else
            {
                queueLock.Release();
                callback.Callback(state);
            }
        }
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal async Task Update(WordIdentityBlockIndexNode<T> node, T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            int isCallback = 0;
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                var text = await node.GetText(key);
                if (text != null)
                {
                    int[] wordIdentitys, historyWordIdentitys;
                    if (text.Length != 0)
                    {
                        ResponseResult<int[]> identitys = await node.GetWordIdentitys(text);
                        if (!identitys.IsSuccess)
                        {
                            state = WordIdentityBlockIndexUpdateStateEnum.GetWordIdentityFailed;
                            return;
                        }
                        wordIdentitys = identitys.Value.notNull();
                        if (wordIdentitys.Length > 1) wordIdentitys.Sort();
                    }
                    else wordIdentitys = EmptyArray<int>.Array;
#if NetStandard21
                    var readResult = default(ReadResult<int[]?>);
#else
                    var readResult = default(ReadResult<int[]>);
#endif
                    var diskBlockClient = default(IDiskBlockClient);
                    if (!BlockIndex.GetResult(out readResult))
                    {
                        ReadResult<int[]> readBlockIndexResult = await new ReadBinaryAwaiter<int[]>(diskBlockClient = node.GetDiskBlockClient(BlockIndex), BlockIndex);
                        if (!readBlockIndexResult.IsSuccess)
                        {
                            state = WordIdentityBlockIndexUpdateStateEnum.GetBlockIndexResultFailed;
                            return;
                        }
                        historyWordIdentitys = readBlockIndexResult.Value;
                    }
                    else historyWordIdentitys = readResult.Value.notNull();
                    if (sequenceEqual(wordIdentitys, historyWordIdentitys))
                    {
                        state = WordIdentityBlockIndexUpdateStateEnum.Success;
                        return;
                    }
                    ResponseResult result = await node.AppendIndex(wordIdentitys, historyWordIdentitys, key);
                    if (!result.IsSuccess)
                    {
                        state = WordIdentityBlockIndexUpdateStateEnum.SetWordIndexFailed;
                        return;
                    }
                    bool isIndex;
                    BlockIndex blockIndex = BlockIndex.GetIndexSize(wordIdentitys, out isIndex);
                    if (!isIndex)
                    {
                        if (diskBlockClient == null) diskBlockClient = node.GetDiskBlockClient(key);
                        CommandClientReturnValue<BlockIndex> blockIndexResult = await diskBlockClient.ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(wordIdentitys));
                        if (!blockIndexResult.IsSuccess)
                        {
                            state = WordIdentityBlockIndexUpdateStateEnum.GetBlockIndexFailed;
                            return;
                        }
                        blockIndex = blockIndexResult.Value;
                    }
                    setCallback(node, callback);
                    node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, callback.Reserve);
                }
                else
                {
                    setCallback(node, callback);
                    node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key);
                }
                isCallback = 1;
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally { releaseQueueLock(callback, isCallback, state); }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal async Task Delete(WordIdentityBlockIndexNode<T> node, T key)
        {
            await queueLock.WaitAsync();
            try
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                var text = await node.GetText(key);
                if (text == null) node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key);
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally { queueLock.Release(); }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal async Task Delete(WordIdentityBlockIndexNode<T> node, T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            int isCallback = 0;
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                var text = await node.GetText(key);
                if (text == null)
                {
                    setCallback(node, callback);
                    node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key);
                    isCallback = 1;
                }
                else
                {
                    state = WordIdentityBlockIndexUpdateStateEnum.NotSupportDeleteKey;
                    return;
                }
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally { releaseQueueLock(callback, isCallback, state); }
        }
        /// <summary>
        /// 设置操作版本号
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void setCallback(WordIdentityBlockIndexNode<T> node, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            callback.Reserve = node.NextVersion;
            callback.LinkNext = callbacks;
            callbacks = callback;
        }
        /// <summary>
        /// 操作回调
        /// </summary>
        /// <param name="version">操作版本号</param>
        /// <param name="state">调用状态</param>
        private void callback(int version, WordIdentityBlockIndexUpdateStateEnum state)
        {
            var callback = callbacks;
            if (callback != null)
            {
                if (callback.Reserve == version)
                {
                    callbacks = callback.LinkNext;
                    callback.Callback(state);
                    return;
                }
                for (var nextCallback = callback.LinkNext; nextCallback != null; nextCallback = nextCallback.LinkNext)
                {
                    if (nextCallback.Reserve == version)
                    {
                        callback.LinkNext = nextCallback.LinkNext;
                        nextCallback.Callback(state);
                        return;
                    }
                    callback = nextCallback;
                }
            }
        }
        /// <summary>
        /// 完成数据更新
        /// </summary>
        /// <param name="blockIndex">新的磁盘块索引信息</param>
        /// <param name="version">更新版本号</param>
        /// <returns></returns>
        internal async Task Completed(BlockIndex blockIndex, int version)
        {
            await queueLock.WaitAsync();
            try
            {
                BlockIndex = blockIndex;
                if (version != 0) callback(version, WordIdentityBlockIndexUpdateStateEnum.Success);
            }
            finally { queueLock.Release(); }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <returns></returns>
        internal async Task Deleted()
        {
            await queueLock.WaitAsync();
            var callback = callbacks;
            callbacks = null;
            queueLock.Release();
            while (callback != null)
            {
                callback.Callback(WordIdentityBlockIndexUpdateStateEnum.DeletedNotFoundKey);
                callback = callback.LinkNext;
            }
        }
        /// <summary>
        /// 数组比较
        /// </summary>
        /// <param name="wordIdentitys"></param>
        /// <param name="historyWordIdentitys"></param>
        /// <returns></returns>
        private static unsafe bool sequenceEqual(int[] wordIdentitys, int[] historyWordIdentitys)
        {
            if (wordIdentitys.Length == historyWordIdentitys.Length)
            {
                if (wordIdentitys.Length == 0) return true;
                fixed (int* leftFixed = wordIdentitys, rightFixed = historyWordIdentitys)
                {
                    return AutoCSer.Memory.Common.SequenceEqual(leftFixed, rightFixed, wordIdentitys.Length * sizeof(int));
                }
            }
            return false;
        }
    }
}
