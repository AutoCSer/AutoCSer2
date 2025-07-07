using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime;TimeSpan;char;float;double;decimal;Half;Int128;UInt128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

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
        /// <param name="value">data</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Write(ulong value)
        {
#if DEBUG
            debugCheckWriteSize(sizeof(ulong));
#endif
            *(ulong*)((byte*)Data + CurrentIndex) = value;
            CurrentIndex += sizeof(ulong);
        }
    }
}
