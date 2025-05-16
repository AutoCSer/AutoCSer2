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
    /// 数组子串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct LeftArray<T> : IList<T>
    {
        /// <summary>
        /// 原数组
        /// </summary>
        internal T[] Array;
        /// <summary>
        /// 长度
        /// </summary>
        internal int Length;
        /// <summary>
        /// 长度
        /// </summary>
        public int Count
        {
            get { return Length; }
        }
        /// <summary>
        /// 原数组空闲数量
        /// </summary>
        public int FreeCount { get { return Array.Length - Length; } }
        /// <summary>
        /// 是否存在空闲位置
        /// </summary>
        internal bool IsFree { get { return Array.Length != Length; } }
        /// <summary>
        /// 保留字段
        /// </summary>
        internal int Reserve;
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// 设置或获取值
        /// </summary>
        /// <param name="index">位置</param>
        /// <returns>数据值</returns>
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
        /// 数组子串
        /// </summary>
        public LeftArray()
        {
            Array = EmptyArray<T>.Array;
            Length = Reserve = 0;
        }
#endif
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="capacity">容器大小</param>
        public LeftArray(int capacity)
        {
            Array = capacity > 0 ? new T[capacity] : EmptyArray<T>.Array;
            Length = Reserve = 0;
        }
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="array">数组</param>
        public LeftArray(T[] array) : this(array.Length, array) { }
        ///// <summary>
        ///// 数组子串
        ///// </summary>
        ///// <param name="array">原数组</param>
        ///// <param name="length">初始化数据长度</param>
        //public LeftArray(T[] array, int length)
        //{
        //    if ((uint)length > (uint)array.Length) throw new IndexOutOfRangeException();
        //    Array = array;
        //    Length = length;
        //    Reserve = 0;
        //}
        /// <summary>
        /// 数组子串
        /// </summary>
        /// <param name="length">初始化数据长度</param>
        /// <param name="array">原数组</param>
        internal LeftArray(int length, T[] array)
        {
            Array = array;
            Length = length;
            Reserve = 0;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Length != 0) return new Enumerator<T>.Array(this);
            return Enumerator<T>.Empty;
        }
        /// <summary>
        /// 枚举数据
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
        /// 枚举数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerable(int index)
        {
            return GetEnumerable(index, Length - index);
        }
        /// <summary>
        /// 枚举数据
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
        /// 枚举数据
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
        /// 设置数据容器长度
        /// </summary>
        /// <param name="capacity">数据长度</param>
        private void setCapacity(int capacity)
        {
            T[] newArray = DynamicArray<T>.GetNewArray(capacity);
            Common.CopyTo(Array, newArray);
            Array = newArray;
        }
        /// <summary>
        /// 设置数据容器长度
        /// </summary>
        /// <param name="capacity">数据长度</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void checkCapacity(int capacity)
        {
            if (capacity > Array.Length) setCapacity(capacity);
        }
        /// <summary>
        /// 预增长度
        /// </summary>
        /// <param name="length"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PrepLength(int length)
        {
            if ((length += Length) > Array.Length) setCapacity(Math.Max(Math.Max(length, Array.Length << 1), DynamicArray.DefalutArrayCapacity));
        }
        /// <summary>
        /// 置空并释放数组并将数据有效长度设置为 0
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetEmpty()
        {
            Array = EmptyArray<T>.Array;
            Length = 0;
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="value">数组,不能为null</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(T[] value)
        {
            Array = value;
            Length = value.Length;
        }
        /// <summary>
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
        /// 清除当前长度有效数据并将数据有效长度设置为 0
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
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void UnsafeAdd(T value)
        {
            Array[Length++] = value;
        }
        /// <summary>
        /// 当有空闲位置时添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>如果数组已满则添加失败并返回 false</returns>
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
        /// 添加数据
        /// </summary>
        /// <param name="value">数据</param>
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
        /// 添加数据集合
        /// </summary>
        /// <param name="values">数据集合</param>
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
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
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
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Append(params T[] array) { Add(array); }
        /// <summary>
        /// 添加数据集合
        /// </summary>
        /// <param name="array">数据集合</param>
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
        /// 插入数据
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="value">数据</param>
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
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>是否存在数据</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// 获取匹配数据位置
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns>匹配位置,失败为-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T value)
        {
            return Length == 0 ? -1 : System.Array.IndexOf(Array, value, 0, Length);
        }
        /// <summary>
        /// 移除第一个匹配数据
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否存在移除数据</returns>
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
        /// 移除数据（不清除数据）
        /// </summary>
        /// <param name="index">数据位置</param>
        internal void RemoveAtOnly(int index)
        {
            int copyIndex = index + 1;
            System.Array.Copy(Array, copyIndex, Array, index, Length - copyIndex);
            --Length;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="index">数据位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            RemoveAtOnly(index);
#pragma warning disable CS8601
            Array[Length] = default(T);
#pragma warning restore CS8601
        }
        /// <summary>
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
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
        /// 获取获取数组中的匹配位置
        /// </summary>
        /// <param name="isValue">数据匹配器</param>
        /// <returns>数组中的匹配位置,失败为-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(Func<T, bool> isValue)
        {
            return Length == 0 ? -1 : indexOf(isValue);
        }
        /// <summary>
        /// 根据匹配条件替换第一个匹配值，不存在匹配则添加新数据
        /// </summary>
        /// <param name="value">新数据</param>
        /// <param name="isValue">匹配条件</param>
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
        /// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="value">数据匹配</param>
        /// <returns>是否存在移除数据</returns>
        public bool RemoveToEnd(T value)
        {
            int index = IndexOf(value);
            if (index >= 0)
            {
                RemoveToEnd(index);
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 移除第一个匹配数据，然后将最后一个数据移动到被删除数据位置
        ///// </summary>
        ///// <param name="isValue">数据匹配器</param>
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
        /// 移除所有匹配数据，每次删除操作将当前最后一个数据移动到删除数据位置
        /// </summary>
        /// <param name="isValue"></param>
        /// <returns></returns>
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
        /// 最后一个数据移动到被删除数据位置（不清除数据）
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveToEndOnly(int index)
        {
            if (index != --Length) Array[index] = Array[Length];
        }
        /// <summary>
        /// 最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void RemoveToEnd(int index)
        {
            if (index != --Length) Array[index] = Array[Length];
            Array.setDefault(Length);
        }
        /// <summary>
        /// 最后一个数据移动到被删除数据位置
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAtToEnd(int index)
        {
            if ((uint)index < (uint)Length) RemoveToEnd(index);
            else throw new IndexOutOfRangeException("index[" + index.toString() + "] >= Length[" + Length.toString() + "]");
        }
        /// <summary>
        /// 移除最后一个数据
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
        /// 尝试弹出最后一个数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// 移除最后一个数据
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
        /// 获取最后一个值
        /// </summary>
        /// <returns>最后一个值,失败为default(valueType)</returns>
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
        /// 逆转列表
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            if (Length > 1) System.Array.Reverse(Array, 0, Length);
        }
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="destinationArray">目标数据</param>
        /// <param name="index">目标位置</param>
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
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
        {
            if (Length == 0) return EmptyArray<T>.Array;
            return Length == Array.Length ? Array : getArray();
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        private T[] getArray()
        {
            T[] newArray = new T[Length];
            AutoCSer.Common.CopyTo(Array, newArray, 0, Length);
            return newArray;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <returns>数组</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T[] GetArray()
        {
            return Length != 0 ? getArray() : EmptyArray<T>.Array;
        }
        /// <summary>
        /// 转换数组
        /// </summary>
        /// <typeparam name="VT">数组类型</typeparam>
        /// <param name="getValue">数据获取委托</param>
        /// <returns>数组</returns>
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
        /// 排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Sort(Func<T, T, int> comparer)
        {
            AutoCSer.Algorithm.QuickSort<T>.Sort(Array, comparer, 0, Length);
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="toString">字符串转换器</param>
        /// <param name="join">连接串</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string JoinString(string join, Func<T, string> toString)
        {
            return string.Join(join, GetArray(toString));
        }
        /// <summary>
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
        /// fixed 之前检查数据
        /// </summary>
        internal void DebugCheckFixed()
        {
            if (Length < 0) throw new Exception(Length.toString() + " < 0");
            if (Length > Array.Length) throw new Exception(Length.toString() + " > " + Array.Length.toString());
        }
#endif
    }
}
