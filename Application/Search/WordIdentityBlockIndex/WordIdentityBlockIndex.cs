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
    public sealed class WordIdentityBlockIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 操作队列访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim queueLock;
        /// <summary>
        /// 磁盘块索引信息
        /// </summary>
        internal BlockIndex BlockIndex;
        /// <summary>
        /// 是否已经删除
        /// </summary>
        internal bool IsLoadedDeleted;
        /// <summary>
        /// 分词结果磁盘块索引信息
        /// </summary>
        internal WordIdentityBlockIndex()
        {
            BlockIndex.SetBinarySerializeNullValue();
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 分词结果磁盘块索引信息
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        internal WordIdentityBlockIndex(BlockIndex blockIndex)
        {
            BlockIndex = blockIndex;
            queueLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        internal void Loaded(WordIdentityBlockIndexNode<T> node, T key)
        {
            if (!IsLoadedDeleted)
            {
                if (BlockIndex.IsBinarySerializeNullValue) Create(node, key).NotWait();
            }
            else Delete(node, key).NotWait();
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
                    if (text.IsSuccess)
                    {
                        if (text.Value != null)
                        {
                            int[] wordIdentitys;
                            if (text.Value.Length != 0)
                            {
                                ResponseResult<int[]> identitys = await node.GetWordIdentitys(text.Value);
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
                            node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, MethodCallback<WordIdentityBlockIndexUpdateStateEnum>.NullCallback);
                        }
                        else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum>.NullCallback);
                    }
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
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                if (BlockIndex.IsBinarySerializeNullValue)
                {
                    await AutoCSer.Threading.SwitchAwaiter.Default;
                    var text = await node.GetText(key);
                    if (text.IsSuccess)
                    {
                        if (text.Value != null)
                        {
                            int[] wordIdentitys;
                            if (text.Value.Length != 0)
                            {
                                ResponseResult<int[]> identitys = await node.GetWordIdentitys(text.Value);
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
                            node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, callback);
                        }
                        else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key, callback);
                        state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                    }
                    else state = WordIdentityBlockIndexUpdateStateEnum.GetTextFailed;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.Success;
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally
            {
                queueLock.Release();
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
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
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                var text = await node.GetText(key);
                if (text.IsSuccess)
                {
                    if (text.Value != null)
                    {
                        int[] wordIdentitys, historyWordIdentitys;
                        if (text.Value.Length != 0)
                        {
                            ResponseResult<int[]> identitys = await node.GetWordIdentitys(text.Value);
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
                        else historyWordIdentitys = readResult.Value ?? EmptyArray<int>.Array;
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
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex, callback);
                    }
                    else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key, callback);
                    state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.GetTextFailed;
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally
            {
                queueLock.Release();
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
            }
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
                if (text.IsSuccess && text.Value == null)
                {
                    int[] wordIdentitys;
#if NetStandard21
                    var readResult = default(ReadResult<int[]?>);
#else
                    var readResult = default(ReadResult<int[]>);
#endif
                    var diskBlockClient = default(IDiskBlockClient);
                    if (!BlockIndex.GetResult(out readResult))
                    {
                        ReadResult<int[]> readBlockIndexResult = await new ReadBinaryAwaiter<int[]>(diskBlockClient = node.GetDiskBlockClient(BlockIndex), BlockIndex);
                        if (!readBlockIndexResult.IsSuccess) return;
                        wordIdentitys = readBlockIndexResult.Value;
                    }
                    else wordIdentitys = readResult.Value ?? EmptyArray<int>.Array;
                    if (wordIdentitys.Length != 0)
                    {
                        ResponseResult result = await node.RemoveIndex(wordIdentitys, key);
                        if (!result.IsSuccess) return;
                    }
                    node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum>.NullCallback);
                }
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
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            await queueLock.WaitAsync();
            try
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                var text = await node.GetText(key);
                if (text.IsSuccess)
                {
                    if (text.Value == null)
                    {
                        int[] wordIdentitys;
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
                            wordIdentitys = readBlockIndexResult.Value;
                        }
                        else wordIdentitys = readResult.Value ?? EmptyArray<int>.Array;
                        if (wordIdentitys.Length != 0)
                        {
                            ResponseResult result = await node.RemoveIndex(wordIdentitys, key);
                            if (!result.IsSuccess)
                            {
                                state = WordIdentityBlockIndexUpdateStateEnum.SetWordIndexFailed;
                                return;
                            }
                        }
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Deleted(key, callback);
                        state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                    }
                    else
                    {
                        state = WordIdentityBlockIndexUpdateStateEnum.NotSupportDeleteKey;
                        return;
                    }
                }
                else
                {
                    state = WordIdentityBlockIndexUpdateStateEnum.GetTextFailed;
                    return;
                }
            }
            catch (Exception exception)
            {
                await node.OnException(this, exception);
            }
            finally
            {
                queueLock.Release();
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 完成数据更新
        /// </summary>
        /// <param name="blockIndex">新的磁盘块索引信息</param>
        /// <returns></returns>
        internal async Task Completed(BlockIndex blockIndex)
        {
            await queueLock.WaitAsync();
            BlockIndex = blockIndex;
            queueLock.Release();
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
