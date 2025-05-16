using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流
    /// </summary>
    public class UnmanagedStream : UnmanagedStreamBase
    {
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="unmanagedPool">非托管内存池</param>
        internal UnmanagedStream(UnmanagedPool unmanagedPool) : base(unmanagedPool) { }
        /// <summary>
        /// 非托管内存数据流
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isUnmanaged"></param>
        internal UnmanagedStream(UnmanagedPoolPointer data, bool isUnmanaged = false) : base(ref data, isUnmanaged) { }

        /// <summary>
        /// 默认空非托管内存数据流
        /// </summary>
        internal static readonly UnmanagedStream Null = new UnmanagedStream(default(UnmanagedPoolPointer), false);
    }
}
