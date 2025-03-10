using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 快照数据信息
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SnapshotResult<T>
    {
        /// <summary>
        /// 预申请快照数据容器真实数据数量
        /// </summary>
        public int Count;
        /// <summary>
        /// 超预申请快照数据
        /// </summary>
        public LeftArray<T> Array;
        /// <summary>
        /// 快照数据信息
        /// </summary>
        /// <param name="count">预申请快照数据容器真实数据数量</param>
        public SnapshotResult(int count)
        {
            Count = count;
            Array = new LeftArray<T>(EmptyArray<T>.Array);
        }
        /// <summary>
        /// 快照数据信息
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="value">快照数据集合</param>
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
        /// 快照数据信息
        /// </summary>
        /// <param name="valueCount">数据数量</param>
        /// <param name="snapshotArrayLength">预申请快照数据容器大小</param>
        public SnapshotResult(int valueCount, int snapshotArrayLength)
        {
            Count = 0;
            Array = new LeftArray<T>(Math.Max(valueCount - snapshotArrayLength, 0));
        }
        /// <summary>
        /// 快照数据信息
        /// </summary>
        /// <param name="count">预申请快照数据容器真实数据数量</param>
        /// <param name="array">超预申请快照数据</param>
        public SnapshotResult(int count, T[] array)
        {
            Count = count;
            Array = new LeftArray<T>(array);
        }
        ///// <summary>
        ///// 快照数据信息
        ///// </summary>
        ///// <param name="array">超预申请快照数据</param>
        //public SnapshotResult(T[] array)
        //{
        //    Count = 0;
        //    Array = new LeftArray<T>(array);
        //}
        ///// <summary>
        ///// 快照数据信息
        ///// </summary>
        ///// <param name="array">超预申请快照数据</param>
        //public SnapshotResult(ref LeftArray<T> array)
        //{
        //    Count = 0;
        //    Array = array;
        //}
        ///// <summary>
        ///// 快照数据信息
        ///// </summary>
        ///// <param name="count">预申请快照数据容器真实数据数量</param>
        ///// <param name="array">超预申请快照数据</param>
        //public SnapshotResult(int count, ref LeftArray<T> array)
        //{
        //    Count = count;
        //    Array = array;
        //}
        /// <summary>
        /// 快照数据信息
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="values">快照数据集合</param>
        public SnapshotResult(T[] snapshotArray, ICollection<T> values) : this(snapshotArray, values, values.Count) { }
        /// <summary>
        /// 快照数据信息
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="values">快照数据集合</param>
        /// <param name="count">快照数据总数</param>
        public SnapshotResult(T[] snapshotArray, IEnumerable<T> values, int count)
        {
            Count = 0;
            Array = new LeftArray<T>(Math.Max(count - snapshotArray.Length, 0));
            Add(snapshotArray, values);
        }
        /// <summary>
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
        /// 添加快照数据
        /// </summary>
        /// <param name="snapshotArray">预申请快照数据容器</param>
        /// <param name="value">快照数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(T[] snapshotArray, T value)
        {
            if (Count != snapshotArray.Length) snapshotArray[Count++] = value;
            else Array.Add(value);
        }
    }
}
