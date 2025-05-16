using System;
/*UInt128;Int128;Half;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(UInt128 value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberUInt128Reflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((UInt128)value);
        }
#else
        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, UInt128 value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}
