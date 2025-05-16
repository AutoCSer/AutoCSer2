using System;
/*ulong;long;uint;int;DateTime;TimeSpan;float;double;decimal;Guid*/

namespace AutoCSer.Memory
{
    /// <summary>
    /// 指针(指针无法静态初始化与异步操作)
    /// </summary>
    public unsafe partial struct Pointer
    {
        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="value">数据</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeWriteNullable(ulong value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(int) + sizeof(ulong));
#endif
            byte* data = (byte*)Data + CurrentIndex;
            *(int*)data = 0;
            *(ulong*)(data + sizeof(int)) = value;
            CurrentIndex += sizeof(int) + sizeof(ulong);
        }
    }
}
