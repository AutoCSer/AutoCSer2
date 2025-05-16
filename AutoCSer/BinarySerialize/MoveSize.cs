using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 序列化缓冲区移动位置
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct MoveSize
    {
        /// <summary>
        /// 序列化输出缓冲区
        /// </summary>
        private readonly UnmanagedStream stream;
        /// <summary>
        /// 移动以后的当前位置，失败为 0
        /// </summary>
        public readonly int StartIndex;
        /// <summary>
        /// 序列化缓冲区移动位置
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="moveSize"></param>
        internal MoveSize(UnmanagedStream stream, int moveSize)
        {
            this.stream = stream;
            StartIndex = stream.GetMoveSize(moveSize);
        }
        /// <summary>
        /// 写入缓冲区字节数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool WriteSize()
        {
            return stream.SerializeMoveSize(StartIndex);
        }
    }
}
