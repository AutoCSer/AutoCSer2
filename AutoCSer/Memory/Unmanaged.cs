using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存
    /// </summary>
    internal unsafe static class Unmanaged
    {
        /// <summary>
        /// 不释放的固定内存申请大小
        /// </summary>
        private static long staticSize;
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns></returns>
        internal static void* GetStatic(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref staticSize, size);
            if (isClear) AutoCSer.Common.Clear(data, size);
            return data;
        }
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetStaticPointer(int size, bool isClear)
        {
            return new Pointer(GetStatic(size, isClear), size);
        }
        /// <summary>
        /// 静态类型初始化申请非托管内存(不释放的固定内存)（8字节补齐）
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetStaticPointer8(int size, bool isClear)
        {
            return GetStaticPointer((size + 7) & (int.MaxValue - 7), isClear);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        internal static void FreeStatic(ref Pointer data)
        {
            int size = Interlocked.Exchange(ref data.ByteSize, 0);
            if (size != 0) FreeStatic(ref data, size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void FreeStatic(ref Pointer data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data.GetDataClearOnly());
            Interlocked.Add(ref staticSize, -size);
        }

        /// <summary>
        /// 非托管内存申请字节数
        /// </summary>
        private static long totalSize;
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        internal static void* Get(int size, bool isClear)
        {
            void* data = (void*)Marshal.AllocHGlobal(size);
            Interlocked.Add(ref totalSize, size);
            if (isClear) AutoCSer.Common.Clear(data, size);
            return data;
        }
        /// <summary>
        /// 申请非托管内存
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetPointer(int size, bool isClear)
        {
            return new Pointer(Get(size, isClear), size);
        }
        /// <summary>
        /// 申请非托管内存（8字节补齐）
        /// </summary>
        /// <param name="size">内存字节数</param>
        /// <param name="isClear">是否需要清除</param>
        /// <returns>非托管内存起始指针</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetPointer8(int size, bool isClear)
        {
            return GetPointer((size + 7) & (int.MaxValue - 7), isClear);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        internal static void Free(ref Pointer data)
        {
            int size = Interlocked.Exchange(ref data.ByteSize, 0);
            if (size != 0) Free(ref data, size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Free(ref Pointer data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data.GetDataClearOnly());
            Interlocked.Add(ref totalSize, -size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Free(void* data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data);
            Interlocked.Add(ref totalSize, -size);
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        internal static void Free(ref byte* data, int size)
        {
            if (data != null)
            {
                Marshal.FreeHGlobal((IntPtr)data);
                data = null;
                Interlocked.Add(ref totalSize, -size);
            }
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        /// <param name="size"></param>
        internal static void FreePool(void* data, int size)
        {
            Marshal.FreeHGlobal((IntPtr)data);
            Interlocked.Add(ref totalSize, -size);
        }
        /// <summary>
        /// 释放内存（一次性单线程队列释放，不允许多次调用）
        /// </summary>
        /// <param name="data">非托管内存起始指针</param>
        internal static void FreeOnly(ref Pointer data)
        {
            Marshal.FreeHGlobal((IntPtr)data.Data);
            Interlocked.Add(ref totalSize, -data.ByteSize);
        }

        /// <summary>
        /// AutoCSer 使用静态内存段，防止碎片化
        /// </summary>
        private static Pointer AutoCSerStatic;
        /// <summary>
        /// 8个0字节（公用）
        /// </summary>
        internal static Pointer NullByte8;
        /// <summary>
        /// 2^n相关32位deBruijn序列集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetDeBruijn32Number()
        {
            return AutoCSerStatic.Slice(8, 32);
        }
        /// <summary>
        /// 星期字符串
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetDateWeekData()
        {
            return AutoCSerStatic.Slice(8 + 32, 8 * sizeof(int));
        }
        /// <summary>
        /// 月份字符串
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetDateMonthData()
        {
            return AutoCSerStatic.Slice(8 + 32 + 8 * sizeof(int), 12 * sizeof(int));
        }
        /// <summary>
        /// JSON 解析字符状态位
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetJsonDeserializeBits()
        {
            return AutoCSerStatic.Slice(8 + 32 + 8 * sizeof(int) + 12 * sizeof(int), 256);
        }
        /// <summary>
        /// JSON 转义字符集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetJsonDeserializeEscapeCharData()
        {
            return AutoCSerStatic.Slice(8 + 32 + 8 * sizeof(int) + 12 * sizeof(int) + 256, JsonDeserializer.EscapeCharSize * sizeof(char));
        }
        /// <summary>
        /// 随机种子
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetRandomSecureSeeds()
        {
            return AutoCSerStatic.Slice(8 + 32 + 8 * sizeof(int) + 12 * sizeof(int) + 256 + JsonDeserializer.EscapeCharSize * sizeof(char), Random.SecureSeedsSize);
        }

        /// <summary>
        /// 可重用字典小质数集合
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Pointer GetReusableDictionaryPrimes()
        {
            return AutoCSerStatic.Slice(8 + 32 + 8 * sizeof(int) + 12 * sizeof(int) + 256 + JsonDeserializer.EscapeCharSize * sizeof(char) + Random.SecureSeedsSize, 4792);
        }

        static Unmanaged()
        {
            AutoCSerStatic = GetStaticPointer(8 + 32 + 8 * sizeof(int) + 12 * sizeof(int) + 256 + JsonDeserializer.EscapeCharSize * sizeof(char) + Random.SecureSeedsSize + 4792, false);
            NullByte8 = AutoCSerStatic.Slice(0, 8);
            *NullByte8.Long = 0;
        }
    }
}
