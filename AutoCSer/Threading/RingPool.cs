using System;
using System.Reflection;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 缓存环池，用于读写线程没有交集的场景，避免内存数据被读写线程交叉访问（并发写性能远不如 Link，仅适合一读一写场景）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal sealed class RingPool<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// 释放空闲缓存对象 定时任务
        /// </summary>
        private sealed class releaseFreeTimerTask : AutoCSer.Threading.SecondTimerArrayNode
        {
            /// <summary>
            /// 缓存环池
            /// </summary>
            private readonly RingPool<T> ringPool;
            /// <summary>
            /// 释放空闲缓存对象 定时任务
            /// </summary>
            /// <param name="ringPool"></param>
            /// <param name="releaseFreeTimeoutSeconds"></param>
            internal releaseFreeTimerTask(RingPool<T> ringPool, int releaseFreeTimeoutSeconds)
                : base(SecondTimer.InternalTaskArray, releaseFreeTimeoutSeconds, SecondTimerKeepModeEnum.After, releaseFreeTimeoutSeconds)
            {
                this.ringPool = ringPool;
                AppendTaskArray();
            }
            /// <summary>
            /// 释放空闲缓存对象 定时任务
            /// </summary>
            protected internal override void OnTimer()
            {
                ringPool.OnTimer();
            }
        }
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad0;
        /// <summary>
        /// 环池数组
        /// </summary>
        private readonly ArrayValue<T>[] ring;
        /// <summary>
        /// 环大小
        /// </summary>
        private readonly int count;
        /// <summary>
        /// 环索引值
        /// </summary>
        private readonly uint countLess;
        /// <summary>
        /// 每个 CPU 高速缓存块容纳对象引用数量
        /// </summary>
        private readonly int cpuCacheBlockObjectCount;
        /// <summary>
        /// 最大可读数量 count - cpuCacheBlockObjectCount
        /// </summary>
        private readonly uint canReadCount;
        /// <summary>
        /// 释放空闲缓存对象 定时任务
        /// </summary>
#if NetStandard21
        private readonly releaseFreeTimerTask? releaseFreeTask;
#else
        private readonly releaseFreeTimerTask releaseFreeTask;
#endif
        //        /// <summary>
        //        /// 清理缓存委托
        //        /// </summary>
        //#if NetStandard21
        //        private Action<int>? clearCacheHandle;
        //#else
        //        private Action<int> clearCacheHandle;
        //#endif
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad1;
        /// <summary>
        /// 预写位置
        /// </summary>
        private volatile int writeIndex;
        /// <summary>
        /// 可写结束位置
        /// </summary>
        private volatile int writeEndIndex;
        /// <summary>
        /// 可写结束位置访问锁
        /// </summary>
        private int writeEndLock = 1;
        /// <summary>
        /// 已写入位置
        /// </summary>
        private volatile int writedIndex;
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad2;
        /// <summary>
        /// 预读位置
        /// </summary>
        private volatile int readIndex;
        /// <summary>
        /// 可读结束位置
        /// </summary>
        private volatile int readEndIndex;
        /// <summary>
        /// 可读结束位置访问锁
        /// </summary>
        private int readEndLock;
        /// <summary>
        /// 已读取位置
        /// </summary>
        private volatile int readedIndex;
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad3;
        /// <summary>
        /// 是否创建了新对象
        /// </summary>
        private bool isGetNewValue;
        /// <summary>
        /// 环池
        /// </summary>
        /// <param name="cacheObjectCount">缓存对象数量</param>
        /// <param name="releaseFreeTimeoutSeconds">释放空闲缓存对象定时间隔秒数</param>
        private RingPool(int cacheObjectCount, int releaseFreeTimeoutSeconds = 0)
        {
            count = cacheObjectCount;
            cpuCacheBlockObjectCount = AutoCSer.Common.CpuCacheBlockObjectCount;
            ring = new ArrayValue<T>[count + (cpuCacheBlockObjectCount << 1)];
            writedIndex = writeIndex = cpuCacheBlockObjectCount;
            writeEndIndex = count - cpuCacheBlockObjectCount;
            canReadCount = (uint)writeEndIndex;
            countLess = (uint)(count - 1);

            //if (isClearCache) AutoCSer.Memory.Common.AddClearCache(clearCacheHandle = clearCache);
            if (releaseFreeTimeoutSeconds > 0) releaseFreeTask = new releaseFreeTimerTask(this, releaseFreeTimeoutSeconds);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (releaseFreeTask != null) releaseFreeTask.KeepSeconds = 0;
            //if (clearCacheHandle != null)
            //{
            //    AutoCSer.Memory.Common.RemoveClearCache(clearCacheHandle);
            //    clearCacheHandle = null;
            //}
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        internal void PushNotNull(T value)
        {
        START:
            int index = writeIndex;
            if (index != writeEndIndex)
            {
                int nextIndex = (int)((uint)index + 1);
                if (System.Threading.Interlocked.CompareExchange(ref writeIndex, nextIndex, index) == index)
                {
                    if (nextIndex == writeEndIndex) System.Threading.Interlocked.Exchange(ref writeEndLock, 0);
                    ring[(int)((uint)index & countLess) + cpuCacheBlockObjectCount].Value = value;
                    while (System.Threading.Interlocked.CompareExchange(ref writedIndex, nextIndex, index) != index) ThreadYield.Yield();
                    return;
                }
                ThreadYield.Yield();
                goto START;
            }
            if (System.Threading.Interlocked.CompareExchange(ref writeEndLock, 1, 0) == 0)
            {
                int newWriteEndIndex = (int)((uint)readedIndex + canReadCount);
                if (newWriteEndIndex != writeEndIndex)
                {
                    writeEndIndex = newWriteEndIndex;
                    goto START;
                }
                System.Threading.Interlocked.Exchange(ref writeEndLock, 0);
            }
        }
        /// <summary>
        /// 弹出数据
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal T? Pop()
#else
        internal T Pop()
#endif
        {
        START:
            int index = readIndex;
            if (index != readEndIndex)
            {
                int nextIndex = (int)((uint)index + 1);
                if (System.Threading.Interlocked.CompareExchange(ref readIndex, nextIndex, index) == index)
                {
                    if (nextIndex == readEndIndex) System.Threading.Interlocked.Exchange(ref readEndLock, 0);
                    T value = ring[(int)((uint)index & countLess) + cpuCacheBlockObjectCount].Pop();
                    while (System.Threading.Interlocked.CompareExchange(ref readedIndex, nextIndex, index) != index) ThreadYield.Yield();
                    return value;
                }
                ThreadYield.Yield();
                goto START;
            }
            if (System.Threading.Interlocked.CompareExchange(ref readEndLock, 1, 0) == 0)
            {
                int newReadEndIndex = (int)((uint)writedIndex - (uint)cpuCacheBlockObjectCount);
                if (newReadEndIndex != readEndIndex)
                {
                    readEndIndex = newReadEndIndex;
                    goto START;
                }
                System.Threading.Interlocked.Exchange(ref readEndLock, 0);
            }
            isGetNewValue = true;
            return null;
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        internal void OnTimer()
        {
            if (writedIndex != readedIndex && !isGetNewValue) TaskQueue.AddDefault(onTimer);
            else isGetNewValue = false;
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private void onTimer()
        {
            int cacheCount = writedIndex - readedIndex, removeCount = cacheCount - cpuCacheBlockObjectCount;
            if (removeCount > 1)
            {
                cacheCount -= (removeCount >>= 1);
                do
                {
                    Pop();
                }
                while (--removeCount != 0 && writedIndex - readedIndex >= cacheCount);
            }
            isGetNewValue = false;
        }
        ///// <summary>
        ///// 清理缓存
        ///// </summary>
        ///// <param name="count"></param>
        //private void clearCache(int count)
        //{
        //    if (count < cpuCacheBlockObjectCount) count = cpuCacheBlockObjectCount;
        //    while (writedIndex - readedIndex > count) Pop();
        //}
        /// <summary>
        /// 默认缓存环池
        /// </summary>
#if NetStandard21
        internal static readonly RingPool<T>? Default;
#else
        internal static readonly RingPool<T> Default;
#endif

        static RingPool()
        {
            RingPoolParameter parameter = AutoCSer.Common.Config.GetRingPoolParameter(typeof(T));
            if (parameter.IsDefault)
            {
                var attribute = typeof(T).GetCustomAttribute(typeof(RingPoolAttribute), false);
                if (attribute != null) parameter = ((RingPoolAttribute)attribute).Parameter;
            }
            int cacheObjectCount = parameter.CacheObjectCount;
            if (cacheObjectCount > 0)
            {
                Default = new RingPool<T>(cacheObjectCount, parameter.ReleaseFreeTimeoutSeconds);
                AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(RingPool<T>));
            }
        }
    }
}
