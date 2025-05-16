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
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref UInt128[]? array)
#else
        public void BinaryDeserialize(ref UInt128[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(UInt128) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeUInt128Array(BinaryDeserializer deserializer)
        {
            var array = default(UInt128[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref UInt128[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref UInt128[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }

        /// <summary>
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, UInt128[]?> getBuffer, out UInt128[]? buffer)
#else
        public int DeserializeBuffer(Func<int, UInt128[]> getBuffer, out UInt128[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(UInt128) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (UInt128* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(UInt128));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<UInt128>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
    }
}
