using System;
/*UInt128;Int128;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref UInt128 value)
        {
            value = *(UInt128*)Current;
            Current += sizeof(UInt128);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref UInt128 value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out UInt128 value)
        {
            value = *(UInt128*)Current;
            if ((Current += sizeof(UInt128)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}
