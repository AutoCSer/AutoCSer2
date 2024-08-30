using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 磁盘块字节数
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(int))]
    internal struct BlockSize
    {
        /// <summary>
        /// 字节数
        /// </summary>
        [FieldOffset(0)]
        internal int Value;
        /// <summary>
        /// 字符数据
        /// </summary>
        [FieldOffset(0)]
        internal char Char;
        /// <summary>
        /// 字节数据
        /// </summary>
        [FieldOffset(0)]
        internal byte Data0;
        /// <summary>
        /// 字节数据
        /// </summary>
        [FieldOffset(1)]
        internal byte Data1;
        /// <summary>
        /// 字节数据
        /// </summary>
        [FieldOffset(2)]
        internal byte Data2;
        /// <summary>
        /// 小于 16 的字节数，使用负数表示，0 用 sbyte.MinValue 表示
        /// </summary>
        [FieldOffset(3)]
        internal sbyte Size;
        /// <summary>
        /// 设置字节数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SetSize(sbyte size)
        {
            Size = size;
            return Value;
        }
        /// <summary>
        /// 设置字节数
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal sbyte GetSize(int size)
        {
            Value = size;
            return Size;
        }
    }
}
