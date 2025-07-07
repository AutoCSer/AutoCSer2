using AutoCSer.CommandService.Search.ConditionData;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据节点
    /// </summary>
    /// <typeparam name="NT">节点接口类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    public abstract class ConditionDataLocalNode<NT, KT, VT, CT> : ConditionDataNode<NT, KT, VT>
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
        where CT : class
    {
        /// <summary>
        /// 初始化加载数据获取非索引条件查询数据节点单例
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseLocalClientNodeCache<CT> loadClientNode;
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        /// <param name="loadClientNode">初始化加载数据获取非索引条件查询数据节点单例</param>
        protected ConditionDataLocalNode(StreamPersistenceMemoryDatabaseLocalClientNodeCache<CT> loadClientNode)
        {
            this.loadClientNode = loadClientNode;
        }
        /// <summary>
        /// 初始化加载所有数据
        /// </summary>
        /// <returns></returns>
        protected override async Task load()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            var command = default(EnumeratorCommand<VT>);
            do
            {
                try
                {
                    LocalResult<CT> nodeResult = await loadClientNode.GetSynchronousNode();
                    if (nodeResult.IsSuccess)
                    {
                        command = getLoadCommand();
                        if (command != null) command = await command;
                        if (command != null)
                        {
                            CT node = nodeResult.Value.notNull();
                            VT[] values = new VT[Math.Max(loadArraySize, 1)];
                            LocalLoadCallback<NT, KT, VT, CT> callback = new LocalLoadCallback<NT, KT, VT, CT>(this, values);
                            int index = 0;
                            while (await command.MoveNext())
                            {
                                values[index++] = command.Current;
                                if (index == values.Length)
                                {
                                    if (StreamPersistenceMemoryDatabaseNodeIsRemoved) return;
                                    index = await create(node, callback);
                                }
                            }
                            if (index >= 0)
                            {
                                if (index > 0)
                                {
                                    if (StreamPersistenceMemoryDatabaseNodeIsRemoved) return;
                                    if (index != values.Length) Array.Resize(ref callback.Values, index);
                                    index = await create(node, callback);
                                }
                                if (index == 0 && command.ReturnType == CommandClientReturnTypeEnum.Success)
                                {
                                    StreamPersistenceMemoryDatabaseMethodParameterCreator.Loaded();
                                    command = null;
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    await AutoCSer.LogHelper.Exception(exception);
                }
                finally
                {
                    if (command != null)
                    {
                        await ((IAsyncDisposable)command).DisposeAsync();
                        command = null;
                    }
                }
                await Task.Delay(1000);
            }
            while (!StreamPersistenceMemoryDatabaseNodeIsRemoved);
        }
        /// <summary>
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node">客户端节点信息</param>
        /// <param name="callback">初始化加载数据回调</param>
        /// <returns>0 表示成功，-1 表示失败</returns>
        private async Task<int> create(CT node, LocalLoadCallback<NT, KT, VT, CT> callback)
        {
            StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(callback);
            await callback.Wait();
            VT[] values = callback.Values;
            LocalServiceQueueNode<LocalResult>[] responses = callback.CreateResponses;
            int index = callback.NewCount;
            while (index > 0)
            {
                --index;
                responses[index] = loadCreate(node, values[index]);
            }
            for (index = callback.NewCount; index > 0;)
            {
                LocalResult state = await responses[--index];
                if (!state.IsSuccess) return -1;
            }
            return index;
        }
        /// <summary>
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract LocalServiceQueueNode<LocalResult> loadCreate(CT client, VT value);
    }
}
