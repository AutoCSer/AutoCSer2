using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;

namespace AutoCSer
{
    /// <summary>
    /// Array substring
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct LeftArray<T> : IList<T>
    {
        /// <summary>
        /// Original array
        /// 原数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// Effective data length
        /// 有效数据长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// Effective data length
        /// 有效数据长度
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// The number of free slots in the original array
        /// 原数组空闲数量
        /// </summary>
        public int FreeCount { get { return Array.Length - Length; } }
        /// <summary>
        /// Are there any available positions
        /// 是否存在空闲位置
        /// </summary>
        internal bool IsFree { get { return Array.Length != Length; } }
        /// <summary>
        /// Reserve
        /// 保留字段
        /// </summary>
        internal int Reserve;
        /// <summary>
        /// A fixed return of false indicates writable
        /// 固定返回 false 表示可写
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// Set or get the data of the specified location
        /// 设置或获取指定位置数据
        /// </summary>
        /// <param name="index">Specified position
        /// 指定位置</param>
        /// <returns>Data value</returns>
        public T this[int index]
        {
            get
            {
                if ((uint)index < (uint)Length) return Array[index];
                throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
            set
            {
                if ((uint)index < (uint)Length) Array[index] = value;
                else throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
            }
        }
#if AOT
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        public LeftArray()
        {
            Array = EmptyArray<T>.Array;
            Length = Reserve = 0;
        }
#endif
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public LeftArray(int capacity)
        {
            Array = capacity > 0 ? new T[capacity] : EmptyArray<T>.Array;
            Length = Reserve = 0;
        }
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="array">Array</param>
        public LeftArray(T[] array) : this(array.Length, array) { }
        /// <summary>
        /// Array substring
        /// 数组子串
        /// </summary>
        /// <param name="length">Initialize the data length
        /// 初始化数据长度</param>
        /// <param name="array">Original array
        /// 原数组</param>
        internal LeftArray(int length, T[] array)
        {
            Array = array;
            Length = length;
            Reserve = 0;
        }
        /// <summary>
        /// Get the enumerator
        /// 获取枚举器
        /// </summary>
        /// <returns>Enumerator
        /// 枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// Get the enumerator
        /// 获取枚举器
        /// </summary>
        /// <returns>Enumerator
        /// 枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// Return the collection enumeration data
        /// 返回集合枚举数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerable(int index, int count)
        {
            if (count > 0)
            {
                if (index >= 0)
                {
                    int endIndex = index + count;
                    if (endIndex <= Length)
                    {
                        do
                        {
                            yield return Array[index];
                        }
                        while (++index != endIndex);
                    }
                    else throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > Length[" + Length.toString() + "]");
                }
                else throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            }
            else if(count != 0) throw new IndexOutOfRangeException("count[" + count.toString() + "] < 0");
        }
        /// <summary>
        /// Return the collection enumeration data
        /// 返回集合枚举数据
        /// </summary>
        /// <param name="index">Starting position
        /// 起始位置</param>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerable(int index)
        {
            return GetEnumerable(index, Length - index);
        }
        /// <summary>
        /// Return the reverse enumeration data of the collection
        /// 返回集合反向枚举数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> GetReverseEnumerable(int index, int count)
        {
            if (count > 0)
            {
                if (index >= 0)
                {
                    int endIndex = index + count;
                    if (endIndex <= Length)
                    {
                        do
                        {
                            yield return Array[--endIndex];
                        }
                        while (index != endIndex);
                    }
                    else throw new IndexOutOfRangeException("index[" + index.toString() + "] + count[" + count.toString() + "] > Length[" + Length.toString() + "]");
                }
                else throw new IndexOutOfRangeException("index[" + index.toString() + "] < 0");
            }
            else if (count != 0) throw new IndexOutOfRangeException("count[" + count.toString() + "] < 0");
        }
        /// <summary>
        /// Return the reverse enumeration data of the collection
        /// 返回集合反向枚举数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValue<T, int>> GetReverseIndexEnumerable()
        {
            for (int index = Length; index != 0;)
            {
                --index;
                yield return new KeyValue<T, int>(Array[index], index);
            }
        }
        /// <summary>
        /// Set the size of the data container
        /// 设置数据容器大小
        /// </summary>
        /// <param name="capacity">Container size
        /// 容器大小</param>
        private void setCapacity(int capacity)
        {
            T[] newArray = DynamicArray<T>.GetNewArray(capacity);
            Common.CopyTo(Array, newArray);
            Array = newArray;
        }
        /// <summary>
        /// Set the size of the data container
        /// 设置数据容器大小
        /// </summary>
        /// <param name="capacity">Container size
        /// 容器大小</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void checkCapacity(int capacity)
        {
            if (capacity > Array.Length) setCapacity(capacity);
        }
        /// <summary>
        /// Pre-increase the length of valid data
        /// 预增有效数据长度
        /// </summary>
        /// <param name="length"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PrepLength(int length)
        {
            if ((length += Length) > Array.Length) setCapacity(Math.Max(Math.Max(length, Array.Length << 1), DynamicArray.DefalutArrayCapacity));
        }
        /// <summary>
        /// Set the specified location data
        /// 设置指定位置数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        internal void Set(int index, T value)
        {
            if (index >= Array.Length) setCapacity(Math.Max(Math.Max(index + 1, Array.Length << 1), DynamicArray.DefalutArrayCapacity));
            if (index >= Length)
            {
                int length = index + 1;
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, Length, length - Length);
                Length = length;
            }
            Array[index] = value;
        }
        /// <summary>
        /// Empty and release the array, and set the valid length of the data to 0
        /// 置空并释放数组并将数据有效长度设置为 0
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetEmpty()
        {
            Array = EmptyArray<T>.Array;
            Length = 0;
        }
        /// <summary>
        /// Reset the array data
        /// 重置数组数据
        /// </summary>
        /// <param name="array">null is not allowed
        /// 不允许能为 null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T[] array)
        {
            Array = array;
            Length = array.Length;
        }
        /// <summary>
        /// Clear all the data and set the valid length of the data to 0
        /// 清除所有数据并将数据有效长度设置为 0
        /// </summary>
        public void Clear()
        {
            if (Array.Length != 0)
            {
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, 0, Array.Length);
                Length = 0;
            }
        }
        /// <summary>
        /// Clear the current valid data and set the valid length of the data to 0
        /// 清除当前有效数据并将数据有效长度设置为 0
        /// </summary>
        internal void ClearLength()
        {
            if (Length != 0)
            {
                if (DynamicArray<T>.IsClearArray) System.Array.Clear(Array, 0, Length);
                Length = 0;
            }
        }
        /// <summary>
        /// Clear part of the cache
        /// 清除部分缓存
        /// </summary>
        internal void ClearCache()
        {
            int index = Length >> 1;
            if (index != 0)
            {
                System.Array.Clear(Array, index, Length - index);
                Length = index;
            }
        }
        /// <summary>
        /// Array swap
        /// 数组互换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Exchange(ref LeftArray<T> value)
        {
            T[] array = value.Array;
            int length = value.Length;
            value.Array = Array;
            value.Length = Length;
            Array = array;
            Length = length;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeAdd(T value)
        {
            Array[Length++] = value;
        }
        /// <summary>
        /// Add data when there is a free place
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the array is full and the addition failed
        /// 返回 false 表示数组已满，添加失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryAdd(T value)
        {
            if(Array.Length != Length)
            {
                Array[Length++] = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">data</param>
        public void Add(T value)
        {
            if (Array.Length == 0)
            {
                Array = new T[DynamicArray.DefalutArrayCapacity];
                Array[0] = value;
                Length = 1;
            }
            else
            {
                if (Length == Array.Length) setCapacity(Length << 1);
                Array[Length++] = value;
            }
        }
        ///// <summary>
        ///// 添加数据
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public LeftArray<T> Append(T value)
        //{
        //    Add(value);
        //    return this;
        //}
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="values">Data collection
        /// 数据集合</param>
        public void Add(ICollection<T> values)
        {
            int count = values.count();
            if (count != 0)
            {
                checkCapacity(Length + count);
                foreach (T value in values) Array[Length++] = value;
            }
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        public void Add(T[] array)
        {
            int count = array.Length;
            if (count != 0)
            {
                checkCapacity(Length + count);
                array.CopyTo(Array, Length);
                Length += count;
            }
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        internal void Add(T[] array, int startIndex, int count)
        {
            checkCapacity(Length + count);
            System.Array.Copy(array, startIndex, Array, Length, count);
            Length += count;
        }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(params T[] array) { Add(array); }
        /// <summary>
        /// Adding a data collection
        /// 添加数据集合
        /// </summary>
        /// <param name="array">Data collection
        /// 数据集合</param>
        public void Add(ref LeftArray<T> array)
        {
            if (array.Length != 0)
            {
                checkCapacity(Length + array.Length);
                Common.CopyTo(array.Array, Array, Length, array.Length);
                Length += array.Length;
            }
        }
        /// <summary>
        /// Insert data
        /// 插入数据
        /// </summary>
        /// <param name="index">Insert position
        /// 插入位置</param>
        /// <param name="value">data</param>
        public void Insert(int index, T value)
        {
            if ((uint)index <= (uint)Length)
            {
                if (index != Length)
                {
                    int size = Length - index;
                    if (Length == Array.Length)
                    {
                        T[] newArray = DynamicArray<T>.GetNewArray(Length << 1);
                        Common.CopyTo(Array, newArray, 0, index);
                        System.Array.Copy(Array, index, newArray, index + 1, size);
                        Array = newArray;
                    }
                    else System.Array.Copy(Array, index, Array, index + 1, size);
                    Array[index] = value;
                    ++Length;
                }
                else Add(value);
                return;
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "] > Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// Determine whether there is data
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns>Returning false indicates that there is no matching data
        /// 返回 false 表示不存在匹配数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// Get the matching data location
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns>Returning -1 indicates a matching failure
        /// 返回 -1 表示匹配失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Length == 0 ? -1 : System.Array.IndexOf(Array, value, 0, Length);
        }
        /// <summary>
        /// Remove the first matching data
        /// 移除第一个匹配数据
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        public bool Remove(T value)
        {
            int index = IndexOf(value);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Remove the data at the specified location (without clearing the data)
        /// 移除指定位置的数据（不清除数据）
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        internal void RemoveAtOnly(int index)
        {
            int copyIndex = index + 1;
            System.Array.Copy(Array, copyIndex, Array, index, Length - copyIndex);
            --Length;
        }
        /// <summary>
        /// Remove the data at the specified location
        /// 移除指定位置的数据
        /// </summary>
        /// <param name="index">Data location
        /// 数据位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            RemoveAtOnly(index);
#pragma warning disable CS8601
            Array[Length] = default(T);
#pragma warning restore CS8601
        }
        /// <summary>
        /// Get the matching position in the array
        /// 获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">Determine whether the data match
        /// 判断数据是否匹配</param>
        /// <returns>Returning -1 indicates a matching failure
        /// 返回 -1 表示匹配失败</returns>
        private int indexOf(Func<T, bool> isValue)
        {
            int index = 0;
            foreach (T value in Array)
            {
                if (isValue(value)) return index;
                if (++index == Length) return -1;
            }
            return -1;
        }
        /// <summary>
        /// Get the matching position in the array
        /// 获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">Determine whether the data match
        /// 判断数据是否匹配</param>
        /// <returns>Returning -1 indicates a matching failure
        /// 返回 -1 表示匹配失败</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(Func<T, bool> isValue)
        {
            return Length == 0 ? -1 : indexOf(isValue);
        }
        /// <summary>
        /// Replace the first matching value based on the matching conditions. If there is no match, add new data
        /// 根据匹配条件替换第一个匹配值，不存在匹配则添加新数据
        /// </summary>
        /// <param name="value">New data to be added
        /// 待添加的新数据</param>
        /// <param name="isValue">Determine whether the data match
        /// 判断数据是否匹配</param>
        public void ReplaceAdd(T value, Func<T, bool> isValue)
        {
            if (Length != 0)
            {
                int index = indexOf(isValue);
                if (index >= 0)
                {
                    Array[index] = value;
                    return;
                }
            }
            Add(value);
        }
        /// <summary>
        /// Remove the first matching data, and then move the last data to the position of the deleted data
        /// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="value">The match to be removed
        /// 待移除匹配</param>
        /// <returns>Returning false indicates that there is no data match
        /// 返回 false 表示不存在数据匹配</returns>
        public bool RemoveToEnd(T value)
        {
            int index = IndexOf(value);
            if (index >= 0)
            {
                UnsafeRemoveAtToEnd(index);
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        ///// </summary>
        ///// <param name="isValue">Determine whether the data match
        ///// 判断数据是否匹配</param>
        ///// <returns>是否存在移除数据</returns>
        //public bool RemoveToEnd(Func<T, bool> isValue)
        //{
        //    int index = IndexOf(isValue);
        //    if (index >= 0)
        //    {
        //        RemoveToEnd(index);
        //        return true;
        //    }
        //    return false;
        //}
        /// <summary>
        /// Remove all matching data. Each deletion operation moves the current last data to the deleted data position
        /// 移除所有匹配数据，每次删除操作将当前最后一个数据移动到删除数据位置
        /// </summary>
        /// <param name="isValue"></param>
        public void RemoveAllToEnd(Func<T, bool> isValue)
        {
            for (int index = 0; index != Length; ++index)
            {
                if (isValue(Array[index]))
                {
                    do
                    {
                        if (--Length != index)
                        {
                            T value = Array[Length];
                            if (isValue(value)) Array.setDefault(Length);
                            else
                            {
                                Array[index] = value;
                                Array.setDefault(Length);
                                break;
                            }
                        }
                        else
                        {
                            Array.setDefault(index);
                            return;
                        }
                    }
                    while (true);
                }
            }
        }
        /// <summary>
        /// Remove all matching data. Each deletion operation moves the current last data to the deleted data position
        /// 移除所有匹配数据，每次删除操作将当前最后一个数据移动到删除数据位置
        /// </summary>
        /// <param name="isValue"></param>
        /// <returns>Removed the data collection
        /// 被移除数据集合</returns>
        public IEnumerable<T> GetRemoveAllToEnd(Func<T, bool> isValue)
        {
            for (int index = 0; index != Length; ++index)
            {
                T value = Array[index];
                if (isValue(value))
                {
                    yield return value;
                    do
                    {
                        if (--Length != index)
                        {
                            value = Array[Length];
                            if (isValue(value))
                            {
                                yield return value;
                                Array.setDefault(Length);
                            }
                            else
                            {
                                Array[index] = value;
                                Array.setDefault(Length);
                                break;
                            }
                        }
                        else
                        {
                            Array.setDefault(index);
                            goto RETURN;
                        }
                    }
                    while (true);
                }
            }
        RETURN:;
        }
        /// <summary>
        /// Move the last data to the position where the deleted data was located (without clearing the data)
        /// 最后一个数据移动到被删除数据位置（不清除数据）
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveToEndOnly(int index)
        {
            if (index != --Length) Array[index] = Array[Length];
        }
        /// <summary>
        /// Move the last data to the position where the deleted data was located
        /// 将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeRemoveAtToEnd(int index)
        {
            if (index != --Length) Array[index] = Array[Length];
            Array.setDefault(Length);
        }
        /// <summary>
        /// Move the last data to the position where the deleted data was located
        /// 将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAtToEnd(int index)
        {
            if ((uint)index < (uint)Length) UnsafeRemoveAtToEnd(index);
            else throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// Try to remove the last data
        /// 尝试移除最后一个数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T Pop()
        {
#if DEBUG
            if (Length <= 0) throw new Exception("Length[" + Length.toString() + "] <= 0");
#endif
            T value = Array[--Length];
            Array.setDefault(Length);
            return value;
        }
        /// <summary>
        /// Try to pop up the last data (without clearing the data)
        /// 尝试弹出最后一个数据（不清除数据）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool TryPopOnly([MaybeNullWhen(false)] out T value)
#else
        internal bool TryPopOnly(out T value)
#endif
        {
            if (Length != 0)
            {
                value = Array[--Length];
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// Try to pop up the last data
        /// 尝试弹出最后一个数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that there is no data that can be ejected
        /// 返回 false 表示没有可以弹出的数据</returns>
#if NetStandard21
        public bool TryPop([MaybeNullWhen(false)]out T value)
#else
        public bool TryPop(out T value)
#endif
        {
            if (Length != 0)
            {
                value = Array[--Length];
                Array.setDefault(Length);
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// Pop up all the data
        /// 弹出所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> PopAll()
        {
            while (Length != 0)
            {
                T value = Array[--Length];
                Array.setDefault(Length);
                yield return value;
            }
        }
        /// <summary>
        /// Try to remove the last data
        /// 尝试移除最后一个数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PopOnly()
        {
#if DEBUG
            if (Length <= 0) throw new Exception("Length[" + Length.toString() + "] <= 0");
#endif
            Array.setDefault(--Length);
        }
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        /// <returns>The last data. If there is no data, return default(T)
        /// 最后一个数据，没有数据则返回 default(T)</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? LastOrDefault()
#else
        public T LastOrDefault()
#endif
        {
            return Length != 0 ? Array[Length - 1] : default(T);
        }
        /// <summary>
        /// Reverse the array
        /// 逆转数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            if (Length > 1) System.Array.Reverse(Array, 0, Length);
        }
        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="destinationArray">Target array
        /// 目标数组</param>
        /// <param name="index">Target starting position
        /// 目标起始位置</param>
        public void CopyTo(T[] destinationArray, int index)
        {
            if (index >= 0)
            {
                if (Length + index <= destinationArray.Length)
                {
                    if (Length != 0) Common.CopyTo(Array, destinationArray, index, Length);
                    return;
                }
                throw new IndexOutOfRangeException("Length + index[" + (Length + index).toString() + "] > values.Length[" + destinationArray.Length.toString() + "]");
            }
            throw new IndexOutOfRangeException("index[" + index.toString() + "]");
        }
        /// <summary>
        /// Convert to an array
        /// 转换为数组
        /// </summary>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
        {
            if (Length == 0) return EmptyArray<T>.Array;
            return Length == Array.Length ? Array : getArray();
        }
        /// <summary>
        /// Copy the array data to create a new array
        /// 复制数组数据创建新数组
        /// </summary>
        /// <returns>Array</returns>
        private T[] getArray()
        {
            T[] newArray = new T[Length];
            AutoCSer.Common.CopyTo(Array, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// Copy the array data to create a new array
        /// 复制数组数据创建新数组
        /// </summary>
        /// <returns>Array</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray()
        {
            return Length != 0 ? getArray() : EmptyArray<T>.Array;
        }
        /// <summary>
        /// Convert the data types of array elements to create a new array
        /// 转换数组元素数据类型创建新数组
        /// </summary>
        /// <typeparam name="VT">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="getValue">Data conversion delegate
        /// 数据转换委托</param>
        /// <returns>Array</returns>
        public VT[] GetArray<VT>(Func<T, VT> getValue)
        {
            if (Length == 0) return EmptyArray<VT>.Array;
            VT[] newArray = new VT[Length];
            int index = 0;
            do
            {
                newArray[index] = getValue(Array[index]);
            }
            while (++index != Length);
            return newArray;
        }
        /// <summary>
        /// Array data sorting
        /// 数组数据排序
        /// </summary>
        /// <param name="comparer">Data sorting comparator
        /// 数据排序比较器</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, T, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort<T>.Sort(Array, comparer, 0, Length);
        }
        /// <summary>
        /// Connect string
        /// 连接字符串
        /// </summary>
        /// <param name="toString">The delegate that gets the string
        /// 获取字符串的委托</param>
        /// <param name="join">String concatenation symbol
        /// 字符串连接符号</param>
        /// <returns>The string generated by the concatenation operation
        /// 连接操作产生的字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string JoinString(string join, Func<T, string> toString)
        {
            return string.Join(join, GetArray(toString));
        }
        /// <summary>
        /// Get the fixed buffer, DEBUG mode to detect the data range
        /// 获取 fixed 缓冲区，DEBUG 模式对数据范围进行检测
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal T[] GetFixedBuffer()
        {
#if DEBUG
            DebugCheckFixed();
#endif
            return Array;
        }
#if DEBUG
        /// <summary>
        /// Check the data before the fixed operation
        /// fixed 操作之前检查数据
        /// </summary>
        internal void DebugCheckFixed()
        {
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Length > Array.Length) throw new Exception(Length.toString() + " > " + Array.Length.toString());
        }
#endif
    }
}
