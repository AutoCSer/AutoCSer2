using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端输出创建参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(int) * 4)]
    internal struct ClientBuildInfo
    {
        /// <summary>
        /// 发送数据缓冲区字节大小
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal int SendBufferSize;
        /// <summary>
        /// 当前释放输出数量，包括错误输出
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int))]
        internal int FreeCount;
        /// <summary>
        /// 当前已经创建输出数量
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int) * 2)]
        internal int Count;
        /// <summary>
        /// 数据是否需要发送数据
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int) * 3)]
        internal byte IsFullSend;
        /// <summary>
        /// 是否错误
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int) * 3 + 1)]
        internal bool IsError;
        /// <summary>
        /// 是否创建了新的缓冲区
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int) * 3 + 2)]
        internal byte IsNewBuffer;
        /// <summary>
        /// 客户端最后一个命令是否设置了回调
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(sizeof(int) * 3 + 3)]
        internal bool IsCallback;
        /// <summary>
        /// 获取并重置当前释放输出数量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetFreeCount()
        {
            int count = FreeCount;
            FreeCount = 0;
            return count;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            IsFullSend = 0;
            Count = 0;
            IsCallback = false;
        }
        /// <summary>
        /// 增加输出计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddCount()
        {
            ++FreeCount;
            ++Count;
        }
        /// <summary>
        /// 设置是否设置了回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetIsCallback()
        {
            AddCount();
            IsCallback = true;
        }
    }
}
