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
    internal abstract class RemoveMarkHashKeyIndex
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
        internal readonly RemoveMarkHashSet Values;
        /// <summary>
        /// 等待磁盘块索引信息写入完成以后需要操作的匹配数据关键字
        /// </summary>
        protected LeftArray<KeyValue<uint, bool>> newValues;
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
        internal RemoveMarkHashKeyIndex(ref BlockIndexData<uint> data)
        {
            BlockIndex = data.BlockIndex;
            BlockIndexTotalCount = data.BlockIndexTotalCount;
            BlockIndexValueCount = data.BlockIndexValueCount;
            Values = new RemoveMarkHashSet(data.Values.Length);
            int index = 0;
            foreach (uint value in data.Values)
            {
                if (index == data.ValueCount) Values.AddRemove(value);
                else Values.Add(value);
            }
            newValues.SetEmpty();
        }
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        internal RemoveMarkHashKeyIndex(uint value)
        {
            Values = new RemoveMarkHashSet(1);
            Values.Add(value);
            newValues.SetEmpty();
        }
        /// <summary>
        /// Add matching data keyword (Initialize and load the persistent data)
        /// 添加匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="value"></param>
        internal void AppendLoadPersistence(uint value)
        {
            if (!isException)
            {
                if (Values.Count != RemoveMarkHashSetCapacity.MaxCapacity) Values.Add(value);
                else newValues.Add(new KeyValue<uint, bool>(value, true));
            }
        }
        /// <summary>
        /// Delete the matching data keyword (Initialize and load the persistent data)
        /// 删除匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="value"></param>
        internal void RemoveLoadPersistence(uint value)
        {
            if (!isException)
            {
                if (Values.Count != RemoveMarkHashSetCapacity.MaxCapacity)
                {
                    if (BlockIndexTotalCount == 0) Values.Remove(value);
                    else Values.AddRemove(value);
                }
                else newValues.Add(new KeyValue<uint, bool>(value, false));
            }
        }
        /// <summary>
        /// The operation of writing the disk block index information has been completed
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
            KeyValue<uint, bool>[] newValueArray = newValues.Array;
            foreach (KeyValue<uint, bool> value in newValueArray)
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
        /// The operation of writing the disk block index information has been completed (Initialize and load the persistent data)
        /// 磁盘块索引信息写入完成操作（初始化加载持久化数据）
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
        /// <param name="isClient"></param>
        /// <returns></returns>
        internal BlockIndexData<uint> GetBlockIndexData(bool isClient)
        {
            int valueCount;
            uint[] values = Values.GetArray(out valueCount);
            isClientGetBlockIndexData |= isClient;
            return new BlockIndexData<uint>(this, values, valueCount);
        }
        /// <summary>
        /// 获取关键字数据磁盘块索引信息节点
        /// </summary>
        /// <param name="isClient"></param>
        /// <returns></returns>
        internal BlockIndexData<int> GetIntBlockIndexData(bool isClient)
        {
            int valueCount;
            int[] values = Values.GetIntArray(out valueCount);
            isClientGetBlockIndexData |= isClient;
            return new BlockIndexData<int>(this, values, valueCount);
        }
    }
    /// <summary>
    /// 带移除标记的可重用哈希索引
    /// </summary>
    /// <typeparam name="T">Index keyword type
    /// 索引关键字类型</typeparam>
    internal sealed class RemoveMarkHashKeyIndex<T> : RemoveMarkHashKeyIndex
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        private readonly RemoveMarkHashKeyIndexNode<T> node;
        /// <summary>
        /// 索引关键字
        /// </summary>
        private readonly T key;
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="data">关键字数据磁盘块索引信息节点</param>
        internal RemoveMarkHashKeyIndex(RemoveMarkHashKeyIndexNode<T> node, T key, ref BlockIndexData<uint> data) : base(ref data)
        {
            this.node = node;
            this.key = key;
        }
        /// <summary>
        /// 带移除标记的可重用哈希索引
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        internal RemoveMarkHashKeyIndex(RemoveMarkHashKeyIndexNode<T> node, T key, uint value) : base(value)
        {
            this.node = node;
            this.key = key;
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Loaded()
        {
            if (newValues.Length == 0) newValues.Array = EmptyArray<KeyValue<uint, bool>>.Array;
            if (Values.Count == RemoveMarkHashSetCapacity.MaxCapacity)
            {
                isWriteBlock = true;
                writeDiskBlock().AutoCSerNotWait();
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
            int valueCount;
            do
            {
                try
                {
                    uint[] values = Values.GetArray(out valueCount);
                    CommandClientReturnValue<BlockIndex> blockIndex = await node.GetDiskBlockClient(key).ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(new PersistenceNode<uint>(BlockIndex, values, valueCount)));
                    if (blockIndex.IsSuccess)
                    {
                        node.StreamPersistenceMemoryDatabaseMethodParameterCreator.WriteCompleted(key, blockIndex.Value, valueCount);
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
        /// Add matching data keyword
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void Append(uint value)
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
                else newValues.Add(new KeyValue<uint, bool>(value, true));
            }
        }
        /// <summary>
        /// Delete the matching data keyword
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="value"></param>
        internal void Remove(uint value)
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
                else newValues.Add(new KeyValue<uint, bool>(value, false));
            }
        }
        /// <summary>
        /// The operation of writing the disk block index information has been completed
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        internal void WriteCompleted(BlockIndex blockIndex, int valueCount)
        {
            writeCompleted(blockIndex, valueCount);
            if (newValues.Length == 0)
            {
                newValues.Array = EmptyArray<KeyValue<uint, bool>>.Array;
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
                else writeDiskBlock().AutoCSerNotWait();
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
