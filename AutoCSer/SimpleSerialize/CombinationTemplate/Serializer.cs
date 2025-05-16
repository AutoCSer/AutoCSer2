using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Half;Int128;UInt128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    public unsafe partial class Serializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void serialize(AutoCSer.Memory.UnmanagedStream stream, ulong value)
        {
            stream.Data.Pointer.Write(value);
        }
#if AOT
        /// <summary>
        /// 序列化（用于代码生成）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Serialize(AutoCSer.Memory.UnmanagedStream stream, ulong value)
        {
            stream.Write(value);
        }
#endif
    }
}
