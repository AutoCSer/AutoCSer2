using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Guid;Half;Int128;UInt128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    public unsafe partial class Deserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte* Deserialize(byte* data, ref ulong value)
        {
            value = *(ulong*)data;
            return data + sizeof(ulong);
        }
    }
}
