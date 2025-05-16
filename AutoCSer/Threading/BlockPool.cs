using AutoCSer.Extensions;
using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 分块缓存池，用于读写线程没有交集的场景，避免内存数据被读写线程交叉访问
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public sealed class BlockPool<T> : SecondTimerTaskArrayNode, IDisposable
        where T : class
    {
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad0;
        #region 只读数据区
        /// <summary>
        /// 缓存池数组
        /// </summary>
        private readonly ArrayValue<T>[] array;
        /// <summary>
        /// 分块缓存池每块对象数量
        /// </summary>
        private readonly int blockObjectCount;
        /// <summary>
        /// 有效缓存最大位置
        /// </summary>
        private readonly int cacheEndIndex;

        /// <summary>
        /// 清理缓存委托
        /// </summary>
        private Action<int> clearCacheHandle;
        #endregion
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad1;
        #region 读操作数据区
        /// <summary>
        /// 当前读取位置
        /// </summary>
        private int readIndex;
        /// <summary>
        /// 下一个释放读取的位置
        /// </summary>
        private int freeReadIndex;
        /// <summary>
        /// 读取访问锁
        /// </summary>
        private SpinLock readLock;
        /// <summary>
        /// 最后可读取位置
        /// </summary>
        private int endReadIndex;
        /// <summary>
        /// 对齐保留
        /// </summary>
        private readonly int reserve1;
        #endregion
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad2;
        #region 可读取数据操作区（写操作释放）
        /// <summary>
        /// 可读取数据数量
        /// </summary>
        private volatile int newReadCount;
        /// <summary>
        /// 对齐保留
        /// </summary>
        private readonly int reserve2;
        #endregion
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad3;
        #region 可写入数据操作区（读操作释放）
        /// <summary>
        /// 可写入数据数量
        /// </summary>
        private volatile int newWriteCount;
        /// <summary>
        /// 对齐保留
        /// </summary>
        private readonly int reserve3;
        #endregion
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad4;
        #region 写操作数据区
        /// <summary>
        /// 当前写入位置
        /// </summary>
        private int writeIndex;
        /// <summary>
        /// 下一个释放写入的位置
        /// </summary>
        private int freeWriteIndex;
        /// <summary>
        /// 写入访问锁
        /// </summary>
        private SpinLock writeLock;
        /// <summary>
        /// 最后可写入位置
        /// </summary>
        private int endWriteIndex;
        /// <summary>
        /// 对齐保留
        /// </summary>
        private readonly int reserve4;
        #endregion
        /// <summary>
        /// 填充隔离数据
        /// </summary>
        private readonly CpuCachePad pad5;
        /// <summary>
        /// 分块缓存池
        /// </summary>
        /// <param name="cpuCacheBlockCountPerPoolBlock">每个缓存分块包含 CPU 高速缓存块数量</param>
        /// <param name="blockCount">缓存分块数量, 必须大于等于 3</param>
        /// <param name="isClearCache">是否添加到公共清除缓存数据</param>
        /// <param name="releaseFreeTimeoutSeconds">释放空闲缓存对象定时间隔秒数</param>
        public BlockPool(int cpuCacheBlockCountPerPoolBlock, int blockCount, bool isClearCache = true, int releaseFreeTimeoutSeconds = 0)
            : base(SecondTimer.TaskArray, releaseFreeTimeoutSeconds, SecondTimerTaskThreadMode.Synchronous, releaseFreeTimeoutSeconds > 0 ? SecondTimerKeepMode.After : SecondTimerKeepMode.Canceled, releaseFreeTimeoutSeconds)
        {
            freeReadIndex = readIndex = AutoCSer.Common.CpuCacheBlockObjectCount;
            if (cpuCacheBlockCountPerPoolBlock > 1)
            {
                checked { blockObjectCount = cpuCacheBlockCountPerPoolBlock * readIndex; }
            }
            else blockObjectCount = readIndex;
            if (blockCount < 3) blockCount = 3;
            checked { cacheEndIndex = blockObjectCount * blockCount + readIndex; }
            endReadIndex = readIndex;
            newReadCount = -blockObjectCount;
            writeIndex = readIndex;
            freeWriteIndex = readIndex + blockObjectCount;
            endWriteIndex = cacheEndIndex - blockObjectCount;
            array = new ArrayValue<T>[cacheEndIndex + readIndex];

            if (isClearCache) AutoCSer.Memory.Common.AddClearCache(clearCacheHandle = clearCache);
            AppendTaskArray();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            KeepMode = SecondTimerKeepMode.Canceled;
            if (clearCacheHandle != null)
            {
                AutoCSer.Memory.Common.RemoveClearCache(clearCacheHandle);
                clearCacheHandle = null;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            if (value != null) PushNotNull(value);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        internal void PushNotNull(T value)
        {
            writeLock.EnterYield();
            if (writeIndex != freeWriteIndex)
            {
                array[writeIndex++].Value = value;
                if (writeIndex != freeWriteIndex)
                {
                    writeLock.Exit();
                    return;
                }

                if (freeWriteIndex != endWriteIndex) freeWriteIndex += blockObjectCount;
                else
                {
                    int nextCount = cacheEndIndex - freeWriteIndex;
                    if (nextCount != 0)
                    {
                        nextCount = Math.Min(nextCount, newWriteCount);
                        if (nextCount > 0)
                        {
                            System.Threading.Interlocked.Add(ref newWriteCount, -nextCount);
                            freeWriteIndex += blockObjectCount;
                            endWriteIndex += nextCount;
                        }
                    }
                    else
                    {
                        writeIndex = AutoCSer.Common.CpuCacheBlockObjectCount;
                        if (newWriteCount > 0)
                        {
                            nextCount = System.Threading.Interlocked.Exchange(ref newWriteCount, 0);
                            freeWriteIndex = writeIndex + blockObjectCount;
                            endWriteIndex = writeIndex + nextCount;
                        }
                        else endWriteIndex = freeWriteIndex = writeIndex;
                    }
                }
                writeLock.Exit();
                System.Threading.Interlocked.Add(ref newReadCount, blockObjectCount);
                return;
            }
            if (newWriteCount > 0)
            {
                int nextCount = Math.Min(cacheEndIndex - freeWriteIndex, newWriteCount);
                System.Threading.Interlocked.Add(ref newWriteCount, -nextCount);
                freeWriteIndex += blockObjectCount;
                endWriteIndex += nextCount;
                array[writeIndex++].Value = value;
            }
            writeLock.Exit();
        }
        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            readLock.EnterYield();
            if (readIndex != freeReadIndex)
            {
                T value = array[readIndex++].Pop();
                if (readIndex != freeReadIndex)
                {
                    readLock.Exit();
                    return value;
                }

                if (freeReadIndex != endReadIndex) freeReadIndex += blockObjectCount;
                else
                {
                    int nextCount = cacheEndIndex - freeReadIndex;
                    if (nextCount != 0)
                    {
                        nextCount = Math.Min(nextCount, newReadCount);
                        if (nextCount > 0)
                        {
                            System.Threading.Interlocked.Add(ref newReadCount, -nextCount);
                            freeReadIndex += blockObjectCount;
                            endReadIndex += nextCount;
                        }
                    }
                    else
                    {
                        readIndex = AutoCSer.Common.CpuCacheBlockObjectCount;
                        if (newReadCount > 0)
                        {
                            nextCount = System.Threading.Interlocked.Exchange(ref newReadCount, 0);
                            freeReadIndex = readIndex + blockObjectCount;
                            endReadIndex = readIndex + nextCount;
                        }
                        else endReadIndex = freeReadIndex = readIndex;
                    }
                }
                readLock.Exit();
                System.Threading.Interlocked.Add(ref newWriteCount, blockObjectCount);
                return value;
            }
            if (newReadCount > 0)
            {
                int nextCount = Math.Min(cacheEndIndex - freeReadIndex, newReadCount);
                System.Threading.Interlocked.Add(ref newReadCount, -nextCount);
                freeReadIndex += blockObjectCount;
                endReadIndex += nextCount;
                T value = array[readIndex++].Pop();
                readLock.Exit();
                return value;
            }
            readLock.Exit();
            return null;
        }
        /// <summary>
        /// 释放空闲缓存对象 定时任务
        /// </summary>
        protected internal override void OnTimer()
        {
            clearCache(AutoCSer.Common.ProcessorCount);
        }
        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="count"></param>
        private void clearCache(int count)
        {
            if (count < 0) count = 0;
            if (isDisposable)
            {
                while (endReadIndex - readIndex + newReadCount > count)
                {
                    IDisposable value = (IDisposable)Pop();
                    if (value != null)
                    {
                        try
                        {
                            value.Dispose();
                        }
                        catch { }
                    }
                    else break;
                }
            }
            else
            {
                while (endReadIndex - readIndex + newReadCount > count && Pop() != null);
            }
        }
        /// <summary>
        /// 默认分块缓存池
        /// </summary>
        public static readonly BlockPool<T> Default;
        /// <summary>
        /// 是否继承自 IDisposable
        /// </summary>
        private static readonly bool isDisposable;

        static BlockPool()
        {
            BlockPoolParameter parameter = AutoCSer.Common.Config.GetBlockPoolParameter(typeof(T));
            if (parameter.BlockCount > 0)
            {
                Default = new BlockPool<T>(Math.Max(parameter.CpuCacheBlockCountPerPoolBlock, 1), Math.Max(parameter.BlockCount, 3), parameter.IsClearCache, parameter.ReleaseFreeTimeoutSeconds);
                AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(BlockPool<T>));
            }
        }
    }
}
