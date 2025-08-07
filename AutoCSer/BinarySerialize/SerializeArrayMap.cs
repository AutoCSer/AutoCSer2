using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 数组位图
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct SerializeArrayMap
    {
        /// <summary>
        /// 序列化数据流
        /// </summary>
        private readonly UnmanagedStream stream;
        /// <summary>
        /// 当前位
        /// </summary>
        internal uint Bit;
        /// <summary>
        /// 当前位图
        /// </summary>
        internal uint Map;
        /// <summary>
        /// 当前写入位置
        /// </summary>
        internal int WriteIndex;
#if DEBUG
        /// <summary>
        /// 写入结束位置
        /// </summary>
        private readonly int endIndex;
#endif
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="arrayLength">数组长度</param>
        public SerializeArrayMap(UnmanagedStream stream, int arrayLength)
        {
            this.stream = stream;
            int length = ((arrayLength + (31 + 32)) >> 5) << 2;
            Bit = 1U << 31;
            WriteIndex = stream.GetIndexBeforeMove(length, arrayLength);
            Map = 0;
#if DEBUG
            endIndex = WriteIndex < 0 ? 0 : (WriteIndex + length);
#endif
        }
        /// <summary>
        /// 数组位图
        /// </summary>
        /// <param name="stream">序列化数据流</param>
        /// <param name="arrayLength">数组长度</param>
        /// <param name="prepLength">附加长度</param>
        public SerializeArrayMap(UnmanagedStream stream, int arrayLength, int prepLength)
        {
            this.stream = stream;
            int length = ((arrayLength + (31 + 32)) >> 5) << 2;
            Bit = 1U << 31;
            Map = 0;
            WriteIndex = stream.GetPrepSizeCurrentIndex(length + prepLength);
#if DEBUG
            endIndex = 0;
#endif
            if (WriteIndex != -1)
            {
                *(int*)(stream.Data.Pointer.Byte + WriteIndex) = arrayLength;
                stream.Data.Pointer.CurrentIndex += length;
#if DEBUG
                endIndex = WriteIndex + length;
#endif
            }
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">是否写位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Next(bool value)
        {
            if (value) Map |= Bit;
            NextFalse();
        }
        /// <summary>
        /// 移动到下一个二进制位
        /// </summary>
        public void NextFalse()
        {
            if (Bit == 1)
            {
                WriteIndex += sizeof(int);
#if DEBUG
                debugCheck();
#endif
                *(uint*)(stream.Data.Pointer.Byte + WriteIndex) = Map;
                Bit = 1U << 31;
                Map = 0;
            }
            else Bit >>= 1;
        }
        /// <summary>
        /// Add data
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void NextTrue()
        {
            Map |= Bit;
            NextFalse();
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value">是否写位图</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Next(bool? value)
        {
            if (value.HasValue)
            {
                Map |= Bit;
                if ((bool)value) Map |= (Bit >> 1);
            }
            if (Bit == 2)
            {
                WriteIndex += sizeof(int);
#if DEBUG
                debugCheck();
#endif
                *(uint*)(stream.Data.Pointer.Byte + WriteIndex) = Map;
                Bit = 1U << 31;
                Map = 0;
            }
            else Bit >>= 2;
        }
        /// <summary>
        /// 位图写入结束
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void End()
        {
            WriteIndex += sizeof(int);
#if DEBUG
            debugCheck();
#endif
            if (Bit != 1U << 31) *(uint*)(stream.Data.Pointer.Byte + WriteIndex) = Map;
        }
#if DEBUG
        /// <summary>
        /// 检查数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void debugCheck()
        {
            if (WriteIndex > endIndex) throw new Exception("WriteIndex["+ WriteIndex.toString() +"] > endIndex["+ endIndex.toString() +"]");
        }
#endif
    }
}
