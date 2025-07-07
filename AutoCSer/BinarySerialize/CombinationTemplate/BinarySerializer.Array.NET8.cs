using System;
/*UInt128;Int128;Half;Complex;Plane;Quaternion;Matrix3x2;Matrix4x4;Vector2;Vector3;Vector4*/

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(UInt128[]? array)
#else
        public void BinarySerialize(UInt128[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, UInt128[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, UInt128[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeUInt128Array(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((UInt128[]?)array);
        }
#endif
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(UInt128[]? array)
#else
        public void SerializeBuffer(UInt128[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(UInt128[] array, int count)
        {
            if (array != null)
            {
                if ((uint)count <= array.Length) primitiveSerializeOnly(array, count);
                else throw new IndexOutOfRangeException();
            }
            else Stream.Write(NullValue);
        }
    }
}
