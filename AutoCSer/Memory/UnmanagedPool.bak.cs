using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存池
    /// </summary>
    public unsafe sealed class UnmanagedPool : AutoCSer.Threading.SecondTimerArrayNode
    {
        /// <summary>
        /// 空闲内存地址
        /// </summary>
        private byte* free;
        /// <summary>
        /// 空闲内存地址访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock freeLock;
        /// <summary>
        /// 缓冲区尺寸
        /// </summary>
        public readonly int Size;
        /// <summary>
        /// 是否申请了新的缓冲区
        /// </summary>
        private bool isGetNewBuffer;
        /// <summary>
        /// 内存池
        /// </summary>
        /// <param name="size">缓冲区尺寸</param>
        private UnmanagedPool(int size) : base(AutoCSer.Threading.SecondTimer.InternalTaskArray, SecondTimerKeepModeEnum.After)
        {
            Size = size;
            AppendTaskArray(60 * 60);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区,失败返回null</returns>
        private byte* tryGet()
        {
            freeLock.EnterYield();
            if (free != null)
            {
                byte* value = free;
                free = *(byte**)free;
                freeLock.Exit();
                return value;
            }
            freeLock.Exit();
            isGetNewBuffer = true;
            return null;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Pointer GetPointer()
        {
            byte* data = tryGet();
            return data != null ? new Pointer(data, Size) : Unmanaged.GetPointer(Size, false);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Pointer GetPointer(int size)
        {
            if (size <= Size) return GetPointer();
            return Unmanaged.GetPointer(size, false);
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <returns>缓冲区</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal UnmanagedPoolPointer GetPoolPointer()
        {
            byte* data = tryGet();
            return data != null ? new UnmanagedPoolPointer(this, data) : new UnmanagedPoolPointer(this, Unmanaged.GetPointer(Size, false));
        }
        ///// <summary>
        ///// 获取缓冲区
        ///// </summary>
        ///// <param name="size"></param>
        ///// <returns>缓冲区</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal UnmanagedPoolPointer GetPoolPointer(int size)
        //{
        //    if (size <= Size) return GetPoolPointer();
        //    return new UnmanagedPoolPointer(Unmanaged.GetPointer(size, false));
        //}
        /// <summary>
        /// 保存缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        internal void Push(byte* buffer)
        {
#if DEBUG
            if (buffer == null) throw new Exception("buffer == null");
#endif
            freeLock.EnterYield();
            *(byte**)buffer = free;
            free = buffer;
            freeLock.Exit();
        }
        /// <summary>
        /// 保存缓冲区（一次性单线程队列释放，不允许多次调用）
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushOnly(ref Pointer buffer)
        {
            Push(buffer.Byte);
        }
        /// <summary>
        /// 保存缓冲区（多线程并发）
        /// </summary>
        /// <param name="buffer"></param>
        internal void PushPool(ref Pointer buffer)
        {
            int size = System.Threading.Interlocked.Exchange(ref buffer.ByteSize, 0);
            if (size != 0) Push((byte*)buffer.GetDataClearOnly());
        }
        /// <summary>
        /// 保存缓冲区（多线程并发）
        /// </summary>
        /// <param name="buffer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free(ref Pointer buffer)
        {
            if (buffer.ByteSize == Size) PushPool(ref buffer);
            else Unmanaged.Free(ref buffer);
        }
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        protected internal override void OnTimer()
        {
            if (free != null && !isGetNewBuffer) TaskQueue.AddDefault(onTimer);
            else isGetNewBuffer = false;
        }
        /// <summary>
        /// Clear cache data at regular intervals
        /// 定时清除缓存数据
        /// </summary>
        private void onTimer()
        {
            freeLock.EnterYield();
            if (free != null)
            {
                byte* head = *(byte**)free;
                *(byte**)free = null;
                freeLock.Exit();
                bool isPush = false;
                while (head != null)
                {
                    byte* next = *(byte**)head;
                    if (isPush)
                    {
                        freeLock.EnterYield();
                        *(byte**)head = free;
                        free = head;
                        freeLock.Exit();
                        isPush = false;
                    }
                    else
                    {
                        Unmanaged.FreePool(head, Size);
                        isPush = true;
                    }
                    head = next;
                }
            }
            else freeLock.Exit();
            isGetNewBuffer = false;
        }

        /// <summary>
        /// CPU高速缓存页缓冲区池字节大小 64B
        /// </summary>
        public const int CachePageSize = 64;
        /// <summary>
        /// CPU高速缓存页缓冲区池 64B
        /// </summary>
        public static readonly UnmanagedPool CachePage;
        /// <summary>
        /// 微型缓冲区池字节大小 256B
        /// </summary>
        public const int TinySize = 256;
        /// <summary>
        /// 微型缓冲区池(256B)
        /// </summary>
        public static readonly UnmanagedPool Tiny;
        /// <summary>
        /// 1KB 缓冲区池 1KB
        /// </summary>
        public static readonly UnmanagedPool Kilobyte;
        /// <summary>
        /// 默认缓冲区池字节大小 4KB
        /// </summary>
        public const int DefaultSize = 4 << 10;
        /// <summary>
        /// 默认缓冲区池(4KB)
        /// </summary>
        public static readonly UnmanagedPool Default;
        /// <summary>
        /// 64B 基数排序计数缓冲区字节大小 8KB
        /// </summary>
        internal const int RadixSortCountBufferSize = 256 * 8 * sizeof(int);
        /// <summary>
        /// 64B 基数排序计数缓冲区池(8KB)
        /// </summary>
        internal static readonly UnmanagedPool RadixSortCountBuffer;
        /// <summary>
        /// LZW压缩编码查询表缓冲区(2MB)
        /// </summary>
        internal static readonly UnmanagedPool LzwEncodeTableBuffer;
        /// <summary>
        /// 获取临时缓冲区
        /// </summary>
        /// <param name="length">缓冲区字节长度</param>
        /// <returns>临时缓冲区</returns>
        public static UnmanagedPool GetPool(int length)
        {
            if (length <= DefaultSize)
            {
                if (length <= TinySize) return length <= CachePageSize ? CachePage : Tiny;
                return length <= 1 << 10 ? Kilobyte : Default;
            }
            if (length <= 8 << 10) return RadixSortCountBuffer;
            if ((uint)(length - (1 << 20)) > 1U << 20) return Default; 
            return LzwEncodeTableBuffer;
        }
        /// <summary>
        /// 获取缓冲区
        /// </summary>
        /// <param name="length">缓冲区字节长度</param>
        /// <returns>缓冲区</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UnmanagedPoolPointer GetPoolPointer(int length)
        {
            UnmanagedPool pool = GetPool(length);
            if(length <= pool.Size) return pool.GetPoolPointer();
            return new UnmanagedPoolPointer(Unmanaged.GetPointer(length, false));
        }
        ///// <summary>
        ///// 获取缓冲区
        ///// </summary>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static Pointer GetPointer(int length)
        //{
        //    if (length <= DefaultSize) return (length <= TinySize ? Tiny : Default).GetPointer();
        //    if ((uint)(length - (1 << 20)) > 1U << 20)
        //    {
        //        if (length <= RadixSortCountBufferSize) return RadixSortCountBuffer.GetPointer();
        //        return Unmanaged.GetPointer(length, false);
        //    }
        //    return LzwEncodeTableBuffer.GetPointer();
        //}

        static UnmanagedPool()
        {
            CachePage = new UnmanagedPool(64);
            Tiny = new UnmanagedPool(TinySize);
            Kilobyte = new UnmanagedPool(1 << 10);
            Default = new UnmanagedPool(DefaultSize);
            RadixSortCountBuffer = new UnmanagedPool(RadixSortCountBufferSize);
            LzwEncodeTableBuffer = new UnmanagedPool(4096 * 256 * 2);
        }
    }
}
