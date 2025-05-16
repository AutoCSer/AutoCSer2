using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存池指针
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct UnmanagedPoolPointer
    {
        /// <summary>
        /// 指针
        /// </summary>
        internal Pointer Pointer;
        /// <summary>
        /// 非托管内存池
        /// </summary>
#if NetStandard21
        private UnmanagedPool? unmanagedPool;
#else
        private UnmanagedPool unmanagedPool;
#endif
        /// <summary>
        /// 非托管内存池指针
        /// </summary>
        /// <param name="unmanagedPool">非托管内存池</param>
        /// <param name="data">指针</param>
        internal UnmanagedPoolPointer(UnmanagedPool unmanagedPool, void* data)
        {
            Pointer = new Pointer(data, unmanagedPool.Size);
            this.unmanagedPool = unmanagedPool;
        }
        /// <summary>
        /// 非托管内存池指针
        /// </summary>
        /// <param name="unmanagedPool">非托管内存池</param>
        /// <param name="pointer">指针</param>
        internal UnmanagedPoolPointer(UnmanagedPool unmanagedPool, Pointer pointer)
        {
            Pointer = pointer;
            this.unmanagedPool = unmanagedPool;
        }
        /// <summary>
        /// 非托管内存池指针
        /// </summary>
        /// <param name="pointer">指针</param>
        internal UnmanagedPoolPointer(Pointer pointer)
        {
            Pointer = pointer;
            unmanagedPool = null;
        }
        /// <summary>
        /// 保存缓冲区（一次性单线程队列释放，不允许多次调用）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void PushOnly()
        {
            if (unmanagedPool != null) unmanagedPool.PushOnly(ref Pointer);
            else Unmanaged.FreeOnly(ref Pointer);
        }
        /// <summary>
        /// 保存缓冲区（多线程并发）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push()
        {
            if (unmanagedPool != null) unmanagedPool.PushPool(ref Pointer);
            else Unmanaged.Free(ref Pointer);
        }
        /// <summary>
        /// 保存缓冲区并清空数据
        /// </summary>
        internal void PushSetNull()
        {
            if (unmanagedPool != null)
            {
                unmanagedPool.PushPool(ref Pointer);
                unmanagedPool = null;
            }
            else Unmanaged.Free(ref Pointer);
            Pointer.CurrentIndex = 0;
        }
        /// <summary>
        /// 设置指针
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(void* data, int size)
        {
            Pointer.Set(data, size);
            unmanagedPool = null;
        }
    }
}
