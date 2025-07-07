using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 数组模拟最小堆
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [RemoteType]
    public unsafe class ArrayHeap<KT, VT> : IDisposable
        where KT : IComparable<KT>
    {
        /// <summary>
        /// 默认数组长度
        /// </summary>
        private const int defaultArrayLength = 256;
        /// <summary>
        /// 数据数组
        /// </summary>
        internal KeyValue<KT, VT>[] Array;
        /// <summary>
        /// 最小堆索引
        /// </summary>
        internal AutoCSer.Memory.Pointer Heap;
        /// <summary>
        /// 是否固定内存申请
        /// </summary>
        private bool isStaticUnmanaged;
        /// <summary>
        /// 数据数量
        /// </summary>
        internal int Count
        {
            get { return *Heap.Int; }
        }
        /// <summary>
        /// 数组模拟最小堆
        /// </summary>
        public ArrayHeap() : this(false) { }
        /// <summary>
        /// 数组模拟最小堆
        /// </summary>
        /// <param name="isStaticUnmanaged">是否固定内存申请</param>
        internal ArrayHeap(bool isStaticUnmanaged = false)
        {
            Array = new KeyValue<KT, VT>[defaultArrayLength];
            this.isStaticUnmanaged = isStaticUnmanaged;
            Heap = isStaticUnmanaged ? Unmanaged.GetStaticPointer(defaultArrayLength * sizeof(int), false) : Unmanaged.GetPointer(defaultArrayLength * sizeof(int), false);
            reset(Heap.Int);
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~ArrayHeap()
        {
            Dispose();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            if (isStaticUnmanaged) Unmanaged.FreeStatic(ref Heap);
            else Unmanaged.Free(ref Heap);
        }
        /// <summary>
        /// Clear the data
        /// 清除数据
        /// </summary>
        internal void Clear()
        {
            if (Array.Length == defaultArrayLength)
            {
                int* heapFixed = Heap.Int;
                if (*heapFixed != 0)
                {
                    reset(heapFixed);
                    System.Array.Clear(Array, 0, Array.Length);
                }
            }
            else
            {
                KeyValue<KT, VT>[] newArray = new KeyValue<KT, VT>[defaultArrayLength];
                AutoCSer.Memory.Pointer newHeap = isStaticUnmanaged ? Unmanaged.GetStaticPointer(defaultArrayLength * sizeof(int), false) : Unmanaged.GetPointer(defaultArrayLength * sizeof(int), false), oldHeap = Heap;
                reset(newHeap.Int);
                Array = newArray;
                Heap = newHeap;
                if (isStaticUnmanaged) Unmanaged.FreeStatic(ref oldHeap);
                else Unmanaged.Free(ref oldHeap);
            }
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">数据值</param>
        internal unsafe void Push(KT key, ref VT value)
        {
            int* heapFixed = Heap.Int;
            if (*heapFixed == 0) Array[heapFixed[1]].Set(key, value);
            else
            {
                int heapIndex = *heapFixed + 1;
                if (heapIndex == Array.Length)
                {
                    create();
                    heapFixed = Heap.Int;
                }
                int valueIndex = heapFixed[heapIndex];
                Array[valueIndex].Set(key, value);
                heapFixed[getPushIndex(key, heapIndex)] = valueIndex;
            }
            ++*heapFixed;
        }
        /// <summary>
        /// 重建数据
        /// </summary>
        protected void create()
        {
            int heapIndex = Array.Length, newCount = heapIndex << 1, newHeapSize = newCount * sizeof(int);
            KeyValue<KT, VT>[] newArray = new KeyValue<KT, VT>[newCount];
            AutoCSer.Memory.Pointer newHeap = isStaticUnmanaged ? Unmanaged.GetStaticPointer(newHeapSize, false) : Unmanaged.GetPointer(newHeapSize, false), oldHeap = Heap;
            int* newHeapFixed = newHeap.Int;
            Array.CopyTo(newArray, 0);
            AutoCSer.Memory.Common.Copy(Heap.Byte, newHeapFixed, newHeapSize >> 1);
            do
            {
                --newCount;
                newHeapFixed[newCount] = newCount;
            }
            while (newCount != heapIndex);
            Array = newArray;
            Heap = newHeap;
            if (isStaticUnmanaged) Unmanaged.FreeStatic(ref oldHeap);
            else Unmanaged.Free(ref oldHeap);
        }
        /// <summary>
        /// 获取添加数据位置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="heapIndex"></param>
        /// <returns></returns>
        private int getPushIndex(KT key, int heapIndex)
        {
            int* heapFixed = Heap.Int;
            do
            {
                int parentValueIndex = heapFixed[heapIndex >> 1];
                if (key.CompareTo(Array[parentValueIndex].Key) >= 0) return heapIndex;
                heapFixed[heapIndex] = parentValueIndex;
                if ((heapIndex >>= 1) == 1) return 1;
            }
            while (true);
        }
        /// <summary>
        /// 删除堆顶数据
        /// </summary>
        internal void RemoveTop()
        {
            int* heapFixed = Heap.Int;
            int heapIndex = 1, lastHeapIndex = *heapFixed, lastValueIndex = heapFixed[lastHeapIndex];
            Array[heapFixed[lastHeapIndex] = heapFixed[1]].SetNull();
            for (int maxHeapIndex = (lastHeapIndex + 1) >> 1; heapIndex < maxHeapIndex;)
            {
                int left = heapIndex << 1, right = left + 1;
                if (right != lastHeapIndex)
                {
                    if (Array[heapFixed[left]].Key.CompareTo(Array[heapFixed[right]].Key) < 0)
                    {
                        heapFixed[heapIndex] = heapFixed[left];
                        heapIndex = left;
                    }
                    else
                    {
                        heapFixed[heapIndex] = heapFixed[right];
                        heapIndex = right;
                    }
                }
                else
                {
                    heapFixed[heapIndex] = heapFixed[left];
                    heapIndex = left;
                    break;
                }
            }
            heapFixed[heapIndex == 1 ? 1 : getPushIndex(Array[lastValueIndex].Key, heapIndex)] = lastValueIndex;
            --*heapFixed;
        }

        /// <summary>
        /// 初始化索引
        /// </summary>
        /// <param name="heapFixed"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void reset(int* heapFixed)
        {
            for (int index = defaultArrayLength; index != 0; heapFixed[index] = index) --index;
        }
    }
}
