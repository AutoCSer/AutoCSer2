using System;
/*UInt128;Int128;Half;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref UInt128 value)
        {
            value = *(UInt128*)Current;
            Current += sizeof(UInt128);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeUInt128(BinaryDeserializer deserializer)
        {
            UInt128 value = default(UInt128);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
#else
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref UInt128 value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}
