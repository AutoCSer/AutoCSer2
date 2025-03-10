using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引
    /// </summary>
    /// <typeparam name="T">数据关键字类型</typeparam>
    internal abstract class RemoveMarkHashIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 上一块数据磁盘块索引信息
        /// </summary>
        internal BlockIndex BlockIndex;
        /// <summary>
        /// 历史数据磁盘块索引信息中的数据总数量
        /// </summary>
        internal int BlockIndexTotalCount;
        /// <summary>
        /// 历史数据磁盘块索引信息中的新增数据数量
        /// </summary>
        internal int BlockIndexValueCount;
        /// <summary>
        /// 未持久化数据集合
        /// </summary>
        internal readonly RemoveMarkHashSet<T> Values;
        /// <summary>
        /// 等待磁盘块索引信息写入完成以后需要操作的匹配数据关键字
        /// </summary>
        protected LeftArray<KeyValue<T, bool>> newValues;
        /// <summary>
        /// 是否正在写入磁盘块索引信息
        /// </summary>
        protected bool isWriteBlock;
        /// <summary>
        /// 磁盘块索引信息写入完成操作是否异常，可能是 GetHashCode() 抛出了异常，异常节点在 BUG 修复之前不可用
        /// </summary>
        protected bool isException;
        /// <summary>
        /// 客户端是否获取过关键字数据磁盘块索引信息节点，用于判断是否推送关键字更新数据
        /// </summary>
        protected bool isClientGetBlockIndexData;
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="data">关键字数据磁盘块索引信息节点</param>
        internal RemoveMarkHashIndex(ref BlockIndexData<T> data)
        {
            BlockIndex = data.BlockIndex;
            BlockIndexTotalCount = data.BlockIndexTotalCount;
            BlockIndexValueCount = data.BlockIndexValueCount;
            Values = data.GetRemoveMarkHashSet();
            newValues.SetEmpty();
        }
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="value">匹配数据关键字</param>
        internal RemoveMarkHashIndex(T value)
        {
            Values = new RemoveMarkHashSet<T>(1);
            Values.Add(value);
            newValues.SetEmpty();
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void AppendLoadPersistence(T value)
        {
            if (!isException)
            {
                if (Values.Count != RemoveMarkHashSetCapacity.MaxCapacity) Values.Add(value);
                else newValues.Add(new KeyValue<T, bool>(value, true));
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void RemoveLoadPersistence(T value)
        {
            if (!isException)
            {
                if (Values.Count != RemoveMarkHashSetCapacity.MaxCapacity)
                {
                    if (BlockIndexTotalCount == 0) Values.Remove(value);
                    else Values.AddRemove(value);
                }
                else newValues.Add(new KeyValue<T, bool>(value, false));
            }
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        protected void writeCompleted(BlockIndex blockIndex, int valueCount)
        {
            BlockIndex = blockIndex;
            BlockIndexTotalCount += RemoveMarkHashSetCapacity.MaxCapacity;
            BlockIndexValueCount += valueCount;
            Values.Clear();
        }
        /// <summary>
        /// 加载等待磁盘块索引信息写入完成以后需要操作的匹配数据关键字
        /// </summary>
        /// <returns>返回 false 表示需要触发写入磁盘块索引信息</returns>
        protected bool loadNewValue()
        {
            int index = 0, count = newValues.Length;
            KeyValue<T, bool>[] newValueArray = newValues.Array;
            foreach (KeyValue<T, bool> value in newValueArray)
            {
                if (value.Value) Values.Add(value.Key);
                else Values.AddRemove(value.Key);
                ++index;
                if (Values.Count == RemoveMarkHashSetCapacity.MaxCapacity)
                {
                    newValues.Length -= index;
                    if (newValues.Length != 0) Array.Copy(newValueArray, index, newValueArray, 0, newValues.Length);
                    return false;
                }
                if (--count == 0) return true;
            }
            return true;
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        internal void WriteCompletedLoadPersistence(BlockIndex blockIndex, int valueCount)
        {
            writeCompleted(blockIndex, valueCount);
            if (newValues.Length == 0) return;
            try
            {
                loadNewValue();
            }
            catch (Exception exception)
            {
                isException = true;
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
        }
        /// <summary>
        /// 获取关键字数据磁盘块索引信息节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal BlockIndexData<T> GetBlockIndexData()
        {
            isClientGetBlockIndexData = true;
            return new BlockIndexData<T>(this);
        }
    }
    /// <summary>
    /// 带移除标记的可重用哈希索引
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    internal sealed class RemoveMarkHashIndex<KT, VT> : RemoveMarkHashIndex<VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        private readonly RemoveMarkHashIndexNode<KT, VT> node;
        /// <summary>
        /// 索引关键字
        /// </summary>
        private readonly KT key;
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key">索引关键字</param>
        /// <param name="data">关键字数据磁盘块索引信息节点</param>
        internal RemoveMarkHashIndex(RemoveMarkHashIndexNode<KT, VT> node, KT key, ref BlockIndexData<VT> data) : base(ref data)
        {
            this.node = node;
            this.key = key;
        }
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        internal RemoveMarkHashIndex(RemoveMarkHashIndexNode<KT, VT> node, KT key, VT value) : base(value)
        {
            this.node = node;
            this.key = key;
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Loaded()
        {
            if (newValues.Length == 0) newValues.Array = EmptyArray<KeyValue<VT, bool>>.Array;
            if (Values.Count == RemoveMarkHashSetCapacity.MaxCapacity)
            {
                isWriteBlock = true;
                writeDiskBlock().NotWait();
            }
        }
        /// <summary>
        /// 写入磁盘块索引信息
        /// </summary>
        /// <returns></returns>
        private async Task writeDiskBlock()
        {
            ExceptionRepeat exceptionRepeat = default(ExceptionRepeat);
            CommandClientReturnTypeEnum returnType = CommandClientReturnTypeEnum.Success;
            do
            {
                try
                {
                    PersistenceNode<VT> persistenceNode = new PersistenceNode<VT>(this);
                    CommandClientReturnValue<BlockIndex> blockIndex = await node.GetDiskBlockClient(key).ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(persistenceNode));
                    if (blockIndex.IsSuccess)
                    {
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.WriteCompleted(key, blockIndex.Value, persistenceNode.ValueCount);
                        return;
                    }
                    if (newValues.Count != 0 && returnType != blockIndex.ReturnType) await AutoCSer.LogHelper.Error((returnType = blockIndex.ReturnType).ToString());
                }
                catch (Exception exception)
                {
                    if (!exceptionRepeat.IsRepeat(exception)) await AutoCSer.LogHelper.Exception(exception);
                }
                await Task.Delay(1000);
            }
            while (true);
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void Append(VT value)
        {
            if (!isException)
            {
                if (!isWriteBlock)
                {
                    if (Values.Add(value))
                    {
                        Loaded();
                        if (isClientGetBlockIndexData) node.Callback(key);
                    }
                }
                else newValues.Add(new KeyValue<VT, bool>(value, true));
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void Remove(VT value)
        {
            if (!isException)
            {
                if (!isWriteBlock)
                {
                    if (BlockIndexTotalCount == 0)
                    {
                        if (Values.Remove(value) && isClientGetBlockIndexData) node.Callback(key);
                    }
                    else
                    {
                        if (Values.AddRemove(value))
                        {
                            Loaded();
                            if (isClientGetBlockIndexData) node.Callback(key);
                        }
                    }
                }
                else newValues.Add(new KeyValue<VT, bool>(value, false));
            }
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        internal void WriteCompleted(BlockIndex blockIndex, int valueCount)
        {
            writeCompleted(blockIndex, valueCount);
            if (newValues.Length == 0)
            {
                newValues.Array = EmptyArray<KeyValue<VT, bool>>.Array;
                isWriteBlock = false;
                return;
            }
            try
            {
                if (loadNewValue())
                {
                    newValues.SetEmpty();
                    isWriteBlock = false;
                    if (isClientGetBlockIndexData) node.Callback(key);
                }
                else writeDiskBlock().NotWait();
            }
            catch (Exception exception)
            {
                isException = true;
                newValues.SetEmpty();
                AutoCSer.LogHelper.ExceptionIgnoreException(exception);
            }
        }
    }
}
