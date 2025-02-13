using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// 索引范围
    /// </summary>
    [RemoteType]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [StructLayout(LayoutKind.Explicit, Size = sizeof(int) * 2)]
    internal struct Range
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        [FieldOffset(0)]
        public int StartIndex;
        /// <summary>
        /// 结束位置
        /// </summary>
        [FieldOffset(sizeof(uint))]
        public int EndIndex;
        /// <summary>
        /// 范围长度
        /// </summary>
        public int Length
        {
            get { return EndIndex - StartIndex; }
        }
        /// <summary>
        /// 索引范围
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置</param>
        internal Range(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
        /// <summary>
        /// 索引范围
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置</param>
        internal Range(long startIndex, long endIndex) : this((int)startIndex, (int)endIndex) { }
        /// <summary>
        /// 重置索引范围
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
        /// <summary>
        /// 重置索引范围
        /// </summary>
        /// <param name="startIndex">起始位置</param>
        /// <param name="endIndex">结束位置</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(long startIndex, long endIndex)
        {
            StartIndex = (int)startIndex;
            EndIndex = (int)endIndex;
        }
    }
}
