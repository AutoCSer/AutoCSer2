using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Snapshot data
    /// 快照数据
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotResult<T>
    {
        /// <summary>
        /// The actual amount of data in the pre-apply snapshot data container
        /// 预申请快照数据容器真实数据数量
        /// </summary>
        public int Count;
        /// <summary>
        /// Snapshot data beyond the pre-application scope
        /// 超出预申请范围的快照数据
        /// </summary>
        public LeftArray<T> Array;
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        /// <param name="count">The actual amount of data in the pre-apply snapshot data container
        /// 预申请快照数据容器真实数据数量</param>
        public SnapshotResult(int count)
        {
            Count = count;
            Array = new LeftArray<T>(EmptyArray<T>.Array);
        }
        /// <summary>
        /// Snapshot data (1 data element)
        /// 快照数据（1 个数据元素）
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请快照数据容器</param>
        /// <param name="value">Snapshot data
        /// 快照数据</param>
        public SnapshotResult(T[] snapshotArray, T value)
        {
            if (snapshotArray.Length == 1)
            {
                snapshotArray[0] = value;
                Count = 1;
                Array = new LeftArray<T>(EmptyArray<T>.Array);
            }
            else
            {
                Count = 0;
                Array = new LeftArray<T>(new T[] { value});
            }
        }
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        /// <param name="valueCount">The quantity of snapshot data
        /// 快照数据数量</param>
        /// <param name="snapshotArrayLength">The size of the pre-applied snapshot data container
        /// 预申请快照数据容器大小</param>
        public SnapshotResult(int valueCount, int snapshotArrayLength)
        {
            Count = 0;
            Array = new LeftArray<T>(Math.Max(valueCount - snapshotArrayLength, 0));
        }
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        /// <param name="count">The actual amount of data in the pre-apply snapshot data container
        /// 预申请快照数据容器真实数据数量</param>
        /// <param name="array">Snapshot data beyond the pre-application scope
        /// 超出预申请范围的快照数据</param>
        public SnapshotResult(int count, T[] array)
        {
            Count = count;
            Array = new LeftArray<T>(array);
        }
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请快照数据容器</param>
        /// <param name="values">Snapshot data collection
        /// 快照数据集合</param>
        public SnapshotResult(T[] snapshotArray, ICollection<T> values) : this(snapshotArray, values, values.Count) { }
        /// <summary>
        /// Snapshot data
        /// 快照数据
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请快照数据容器</param>
        /// <param name="values">Snapshot data collection
        /// 快照数据集合</param>
        /// <param name="count">Total snapshot data
        /// 快照数据总数</param>
        public SnapshotResult(T[] snapshotArray, IEnumerable<T> values, int count)
        {
            Count = 0;
            Array = new LeftArray<T>(Math.Max(count - snapshotArray.Length, 0));
            Add(snapshotArray, values);
        }
        /// <summary>
        /// Add the snapshot data collection
        /// 添加快照数据集合
        /// </summary>
        /// <param name="snapshotArray"></param>
        /// <param name="values"></param>
        public void Add(T[] snapshotArray, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                if (Count != snapshotArray.Length) snapshotArray[Count++] = value;
                else Array.Add(value);
            }
        }
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请快照数据容器</param>
        /// <param name="value">Snapshot data
        /// 快照数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T[] snapshotArray, T value)
        {
            if (Count != snapshotArray.Length) snapshotArray[Count++] = value;
            else Array.Add(value);
        }
    }
}
