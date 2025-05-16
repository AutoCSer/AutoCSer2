using System;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extensions;
using AutoCSer.Threading;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 字节数组缓冲区池
    /// </summary>
    public sealed class ByteArrayPool
    {
        /// <summary>
        /// 128KB 避免 GC 压缩
        /// </summary>
        internal const int FixedBufferSize = 1 << (byte)BufferSizeBitsEnum.Kilobyte128;

        /// <summary>
        /// 缓冲区字节大小
        /// </summary>
        internal readonly int Size;
        /// <summary>
        /// 已经创建的缓存区数量
        /// </summary>
        private int bufferCount;
        /// <summary>
        /// 空闲缓冲区集合
        /// </summary>
        private LeftArray<ByteArray> buffers;
        /// <summary>
        /// 字节数组缓冲区池
        /// </summary>
        /// <param name="size"></param>
        /// <param name="bufferCount"></param>
        private ByteArrayPool(int size, int bufferCount)
        {
            Size = size;
            this.bufferCount = bufferCount;
            buffers.SetEmpty();
        }
#if DEBUG
        /// <summary>
        /// 获取字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isDebugSize"></param>
#else
        /// <summary>
        /// 获取字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
#endif
        public void Get(ref ByteArrayBuffer buffer
#if DEBUG
            , bool isDebugSize = true
#endif
            )
        {
#if DEBUG
            //if (isDebugSize && Size == 0x40)
            //{
            //    AutoCSer.ConsoleWriteQueue.Breakpoint();
            //}
#endif
            if (bufferCount >= 0)
            {
                if (Size < FixedBufferSize)
                {
                    Monitor.Enter(this);
                    try
                    {
                        if (buffers.Length == 0)
                        {
                            ByteArray byteArray = new ByteArray(this);
                            buffers.Add(byteArray);
                            ++bufferCount;
                            buffer.Set(byteArray);
                        }
                        else get(ref buffer);
                    }
                    finally { Monitor.Exit(this); }
                }
                else
                {
                    Monitor.Enter(this);
                    if (buffers.Length != 0)
                    {
                        ByteArray byteArray = buffers.Pop();
                        Monitor.Exit(this);
                        buffer.Set(byteArray, 0);
                        return;
                    }
                    ++bufferCount;
                    Monitor.Exit(this);
                    buffer.Set(new ByteArray(this), 0);
                }
            }
            else buffer.Set(new ByteArray(Size), 0);
        }
        /// <summary>
        /// 获取字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void get(ref ByteArrayBuffer buffer)
        {
            ByteArray byteArray = buffers.Array[0];
            buffer.Set(byteArray, byteArray.Indexs.PopInt());
            if (byteArray.Indexs.CurrentIndex == 0) buffers.RemoveToEnd(0);
        }
        /// <summary>
        /// 释放字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void Free(ref ByteArrayBuffer buffer)
        {
            if (bufferCount >= 0)
            {
                if (Size < FixedBufferSize)
                {
                    ByteArray byteArray = buffer.Buffer.notNull();
                    Monitor.Enter(this);
                    try
                    {
                        if (byteArray.Free(ref buffer)) buffers.Add(byteArray);
                    }
                    catch(Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
                    }
                    finally { Monitor.Exit(this); }
                }
                else
                {
                    Monitor.Enter(this);
                    try
                    {
                        buffers.Add(buffer.Buffer.notNull());
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer | LogLevelEnum.Fatal);
                    }
                    finally { Monitor.Exit(this); }
                    buffer.Buffer = null;
                }
            }
            else buffer.Buffer = null;
        }
        /// <summary>
        /// 尝试移除需要清除的字节数组缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void TryRemoveGet(ref ByteArrayBuffer buffer)
        {
            ByteArray byteArray = buffer.Buffer.notNull();
            Monitor.Enter(this);
            try
            {
                if (byteArray.IsRemove)
                {
                    if (buffers.Length == 0)
                    {
                        buffers.UnsafeAdd(byteArray);
                        byteArray.IsRemove = false;
                        ++bufferCount;
                    }
                    else
                    {
                        byteArray.Remove(ref buffer);
                        get(ref buffer);
                    }
                }
            }
            finally { Monitor.Exit(this); }
        }
        /// <summary>
        /// 清理缓存
        /// </summary>
        private void free()
        {
            if (bufferCount >= 0)
            {
                if (Size < FixedBufferSize)
                {
                    if (buffers.Length > 1)
                    {
                        Monitor.Enter(this);
                        try
                        {
                            int endIndex = buffers.Length;
                            if (endIndex > 1)
                            {
                                buffers.Sort(ByteArray.SortComparer);
                                ByteArray[] bufferArray = buffers.Array;
                                do
                                {
                                    ByteArray byteArray = bufferArray[--endIndex];
                                    int removeSize = byteArray.Indexs.FreeSize;
                                    if (removeSize == 0)
                                    {
                                        byteArray.FreeIndex();
                                        buffers.PopOnly();
                                        --bufferCount;
#if DEBUG
                                        checkBufferCount();
#endif
                                        if (endIndex == 0) break;
                                    }
                                    else
                                    {
                                        if (endIndex != 0)
                                        {
                                            int startIndex = 0;
                                            do
                                            {
                                                if (removeSize > 0) removeSize -= bufferArray[startIndex++].Indexs.CurrentIndex;
                                                else
                                                {
                                                    byteArray = bufferArray[--endIndex];
                                                    removeSize += byteArray.Indexs.FreeSize;
                                                }
                                                if (removeSize <= 0)
                                                {
                                                    byteArray.IsRemove = true;
                                                    buffers.PopOnly();
                                                    --bufferCount;
#if DEBUG
                                                    checkBufferCount();
#endif
                                                }
                                            }
                                            while (startIndex != endIndex);
                                        }
                                        break;
                                    }
                                }
                                while (true);
                            }
                        }
                        finally { Monitor.Exit(this); }
                    }
                }
                else if(buffers.Length != 0)
                {
                    Monitor.Enter(this);
                    try
                    {
                        int count = buffers.Length;
                        if (count != 0)
                        {
                            System.Array.Clear(buffers.Array, 0, count);
                            buffers.Length = 0;
                            bufferCount -= count;
#if DEBUG
                            checkBufferCount();
#endif
                        }
                    }
                    finally { Monitor.Exit(this); }
                }
            }
        }
#if DEBUG
        /// <summary>
        /// 检查已经创建的缓存区数量
        /// </summary>
        private void checkBufferCount()
        {
            if (bufferCount < 0)
            {
                int count = bufferCount;
                bufferCount = 0;
                throw new Exception("["+Size.toString()+"]"+count.toString() + " < 0");
            }
        }
#endif

        /// <summary>
        /// 字节数组缓冲区池最小缓冲区二进制位数 4 为 16B
        /// </summary>
        private const int minSizeBits = 4;
        /// <summary>
        /// 字节数组缓冲区池最大缓冲区二进制位数 30 为 1GB
        /// </summary>
        private const int maxSizeBits = 30;
        /// <summary>
        /// 缓冲区池集合
        /// </summary>
        private static readonly ByteArrayPool[] pools = new ByteArrayPool[Math.Min(Math.Max((int)(byte)AutoCSer.Common.Config.MaxByteArrayPoolSizeBits, minSizeBits), maxSizeBits) + 1];
        /// <summary>
        /// 缓冲区池集合访问锁
        /// </summary>
        private static readonly object poolLock = new object();
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static ByteArrayPool getPoolByIndex(int index)
        {
            ByteArrayPool pool = pools[index];
            if (pool == null)
            {
                Monitor.Enter(poolLock);
                try
                {
                    if ((pool = pools[index]) == null) pools[index] = pool = new ByteArrayPool(1 << index, 0);
                }
                finally { Monitor.Exit(poolLock); }
            }
            return pool;
        }
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="bits">缓冲区字节大小二进制位数</param>
        /// <returns></returns>
        public static ByteArrayPool GetPool(BufferSizeBitsEnum bits)
        {
            int index = Math.Max((int)(byte)bits, minSizeBits);
            if (index < pools.Length) return getPoolByIndex(index);
            if (index <= maxSizeBits) return new ByteArrayPool(1 << index, int.MinValue);
            throw new IndexOutOfRangeException(index.toString() +" > " + maxSizeBits.toString());
        }
        /// <summary>
        /// 获取缓冲区池
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static ByteArrayPool? getPool(int size)
#else
        private static ByteArrayPool getPool(int size)
#endif
        {
            int index = Math.Max(((uint)size).upToPower2().deBruijnLog2(), minSizeBits);
            return index < pools.Length ? getPoolByIndex(index) : null;
        }
#if DEBUG
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="isDebugSize"></param>
#else
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
#endif
        internal static void GetBuffer(ref ByteArrayBuffer buffer, int size
#if DEBUG
            , bool isDebugSize = true
#endif
            )
        {
            var pool = getPool(size);
            if (pool != null) pool.Get(ref buffer
#if DEBUG
            , isDebugSize
#endif
            );
            else buffer.Set(new ByteArray(size), 0);
        }
#if DEBUG
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isDebugSize"></param>
        /// <returns></returns>
#else
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ByteArrayBuffer GetBuffer(int size
#if DEBUG
            , bool isDebugSize = true
#endif
            )
        {
            ByteArrayBuffer buffer = default(ByteArrayBuffer);
            GetBuffer(ref buffer, size
#if DEBUG
            , isDebugSize
#endif
                );
            return buffer;
        }
        /// <summary>
        /// 获取独立缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void GetSingleBuffer(ref ByteArrayBuffer buffer, int size)
        {
            var pool = default(ByteArrayPool);
            if (size < FixedBufferSize)
            {
                int index = (byte)BufferSizeBitsEnum.Kilobyte128;
                pool = index < pools.Length ? getPoolByIndex(index) : null;
            }
            else pool = getPool(size);
            if (pool != null) pool.Get(ref buffer);
            else buffer.Set(new ByteArray(size), 0);
        }

        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCache()
        {
            TaskQueue.AddDefault(clearCacheTask);
        }
        /// <summary>
        /// 定时清除缓存数据
        /// </summary>
        private static void clearCacheTask()
        {
            foreach (ByteArrayPool pool in pools) pool?.free();
        }

        static ByteArrayPool()
        {
            int clearSeconds = AutoCSer.Common.Config.GetMemoryCacheClearSeconds();
            if (clearSeconds > 0) AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(clearCache, clearSeconds, Threading.SecondTimerKeepModeEnum.After, clearSeconds);
#if !AOT
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(ByteArrayPool));
#endif
        }
    }
}
