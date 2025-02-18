using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引
    /// </summary>
    /// <typeparam name="T">数据关键字类型</typeparam>
    internal abstract class HashSetIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 数据关键字集合的磁盘块索引信息
        /// </summary>
        internal BlockIndex BlockIndex;
        /// <summary>
        /// 新增数据关键字
        /// </summary>
        protected HashSet<T> values;
        /// <summary>
        /// 新增数据关键字数量
        /// </summary>
        internal int ValueCount { get { return values.Count; } }
        /// <summary>
        /// 待移除数据关键字
        /// </summary>
        protected HashSet<T> removeValues;
        /// <summary>
        /// 磁盘块索引信息中的数据关键字数量
        /// </summary>
        internal int BlockIndexValueCount;
        /// <summary>
        /// 写入磁盘块索引信息版本号
        /// </summary>
        protected int version;
        /// <summary>
        /// 关键字索引
        /// </summary>
        /// <param name="data">索引数据</param>
        internal HashSetIndex(ref IndexData<T> data)
        {
            BlockIndex = data.BlockIndex;
            BlockIndexValueCount = data.BlockIndexValueCount;
            values = HashSetCreator<T>.Create();
            removeValues = HashSetCreator<T>.Create();
            int count = 0;
            foreach (T value in data.Values)
            {
                if (count != data.ValueCount)
                {
                    values.Add(value);
                    ++count;
                }
                else removeValues.Add(value);
            }
        }
        /// <summary>
        /// 关键字索引
        /// </summary>
        /// <param name="value">匹配数据关键字</param>
        internal HashSetIndex(T value)
        {
            BlockIndex.SetBinarySerializeNullValue();
            values = HashSetCreator<T>.Create();
            removeValues = HashSetCreator<T>.Create();
            values.Add(value);
        }
        /// <summary>
        /// 获取数据关键字数组
        /// </summary>
        /// <returns></returns>
        internal T[] GetValueArray()
        {
            int count = values.Count + removeValues.Count;
            if (count != 0)
            {
                int index = 0;
                T[] array = new T[count];
                foreach (T value in values) array[index++] = value;
                foreach (T value in removeValues) array[index++] = value;
            }
            return EmptyArray<T>.Array;
        }
        /// <summary>
        /// 词语添加匹配数据关键字
        /// </summary>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>新增更新以后的更新数据量，返回 0 表示没有新增更新</returns>
        internal int Append(T value)
        {
            if (values.Add(value))
            {
                ++version;
                if (BlockIndex.IsBinarySerializeNullValue) return values.Count;
                if (!removeValues.Remove(value)) return values.Count + removeValues.Count;
            }
            return 0;
        }
        /// <summary>
        /// 词语移除匹配数据关键字
        /// </summary>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>新增更新以后的更新数据量，返回 0 表示没有新增更新</returns>
        internal int Remove(T value)
        {
            if (BlockIndex.IsBinarySerializeNullValue)
            {
                if (values.Remove(value)) ++version;
            }
            else if (removeValues.Add(value))
            {
                ++version;
                if (!values.Remove(value)) return values.Count + removeValues.Count;
            }
            return 0;
        }
    }
    /// <summary>
    /// 关键字索引
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    internal sealed class HashSetIndex<KT, VT> : HashSetIndex<VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 是否已经启动写入磁盘块索引信息操作
        /// </summary>
        private bool isWrite;
        /// <summary>
        /// 关键字索引
        /// </summary>
        /// <param name="data">索引数据</param>
        internal HashSetIndex(ref IndexData<VT> data) : base(ref data) { }
        /// <summary>
        /// 关键字索引
        /// </summary>
        /// <param name="value">匹配数据关键字</param>
        internal HashSetIndex(VT value) : base(value) { }
        /// <summary>
        /// 尝试重新写入磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        internal void TryWrite(IndexNode<KT, VT> node, KT key)
        {
            if (!isWrite)
            {
                if (BlockIndex.IsBinarySerializeNullValue)
                {
                    VT[] values = this.values.getArray();
                    isWrite = true;
                    write(node, key, values, version).NotWait();
                }
                else
                {
                    isWrite = true;
                    read(node, key).NotWait();
                }
            }
        }
        /// <summary>
        /// 重新写入磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        internal void Write(IndexNode<KT, VT> node, KT key)
        {
            bool isNext = false;
            try
            {
                if (BlockIndex.IsBinarySerializeNullValue) write(node, key, values.getArray(), version).NotWait();
                else read(node, key).NotWait();
                isNext = true;
            }
            finally
            {
                if (!isNext) isWrite = false;
            }
        }
        /// <summary>
        /// 磁盘块索引信息写入完成
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal int Completed(BlockIndex blockIndex, int valueCount, int version)
        {
            if (version == this.version)
            {
                if (values.Count != 0) values.Clear();
                if (removeValues.Count != 0) removeValues.Clear();
                this.BlockIndex = blockIndex;
                BlockIndexValueCount = valueCount;
                isWrite = false;
                return 0;
            }
            return values.Count + removeValues.Count;
        }
        /// <summary>
        /// 磁盘块索引信息写入错误
        /// </summary>
        internal void WriteError()
        {
            isWrite = false;
        }
        /// <summary>
        /// 读取磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task read(IndexNode<KT, VT> node, KT key)
        {
            bool isRead = false;
            try
            {
                ReadResult<VT[]> values = await new ReadBinaryAwaiter<VT[]>(node.GetDiskBlockClient(BlockIndex), BlockIndex);
                if (values.IsSuccess)
                {
                    node.StreamPersistenceMemoryDatabaseCallQueue.AddOnly(new IndexNodeCallback<KT, VT>(this, node, key, values.Value));
                    isRead = true;
                }
            }
            finally
            {
                if (!isRead) node.StreamPersistenceMemoryDatabaseCallQueue.AddOnly(new HashSetIndexCallback<KT, VT>(this));
            }
        }
        /// <summary>
        /// 读取磁盘块索引信息以后重新计算匹配数据关键字数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="valueArray"></param>
        internal void Readed(IndexNode<KT, VT> node, KT key, HashSet<VT> values, VT[] valueArray)
        {
            bool isNext = false, isValue = false;
            try
            {
                foreach (VT value in this.values) isValue |= values.Add(value);
                foreach (VT value in removeValues) isValue |= values.Remove(value);
                if (isValue)
                {
                    if (values.Count != 0) write(node, key, values.getArray(), version).NotWait();
                    else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, BlockIndex.BinarySerializeNullValue, 0, version);
                }
                else node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, BlockIndex, valueArray.Length, version);
                isNext = true;
            }
            finally
            {
                if (!isNext) isWrite = false;
            }
        }
        /// <summary>
        /// 写入磁盘块索引信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private async Task write(IndexNode<KT, VT> node, KT key, VT[] values, int version)
        {
            bool isCompleted = false;
            try
            {
                CommandClientReturnValue<BlockIndex> blockIndex = await node.GetDiskBlockClient(key).ClientSynchronousWrite(WriteBuffer.CreateWriteBufferSerializer(values));
                if (blockIndex.IsSuccess)
                {
                    node.StreamPersistenceMemoryDatabaseMethodParameterCreator.Completed(key, blockIndex.Value, values.Length, version);
                    isCompleted = true;
                }
            }
            finally
            {
                if (!isCompleted) node.StreamPersistenceMemoryDatabaseCallQueue.AddOnly(new HashSetIndexCallback<KT, VT>(this));
            }
        }
    }
}
