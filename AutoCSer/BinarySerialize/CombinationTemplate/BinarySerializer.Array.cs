using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;UShort,ushort;Short,short;Byte,byte;SByte,sbyte;Bool,bool;Float,float;Double,double;Decimal,decimal;Char,char;DateTime,DateTime;TimeSpan,TimeSpan;Guid,Guid*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(ulong[]? array)
#else
        public void BinarySerialize(ulong[] array)
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
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, ulong[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, ulong[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(AutoCSer.ListArray<ulong>? array)
#else
        public void BinarySerialize(AutoCSer.ListArray<ulong> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<ulong>? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.ListArray<ulong> array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(AutoCSer.LeftArray<ulong> array)
        {
            primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeULongArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((ulong[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeNullableULongArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((ulong?[]?)array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeULongLeftArray(BinarySerializer binarySerializer, object value)
        {
            AutoCSer.LeftArray<ulong> array = (AutoCSer.LeftArray<ulong>)value;
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        private static void primitiveMemberSerializeULongListArray(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.BinarySerialize((AutoCSer.ListArray<ulong>?)array);
        }
#endif
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, AutoCSer.LeftArray<ulong> array)
        {
            binarySerializer.primitiveSerializeOnly(array.Array, array.Length);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="array"></param>
#if NetStandard21
        public void BinarySerialize(ulong?[]? array)
#else
        public void BinarySerialize(ulong?[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) primitiveSerializeOnly(array);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, ulong?[]? array)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, ulong?[] array)
#endif
        {
            binarySerializer.BinarySerialize(array);
        }

        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void SerializeBuffer(ulong[]? array)
#else
        public void SerializeBuffer(ulong[] array)
#endif
        {
            if (array != null) primitiveSerializeOnly(array, array.Length);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        public void SerializeBuffer(ulong[] array, int count)
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
