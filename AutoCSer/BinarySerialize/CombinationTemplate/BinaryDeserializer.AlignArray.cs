using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;Float,float;Double,double;Decimal,decimal;DateTime,DateTime;TimeSpan,TimeSpan;Guid,Guid*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref ulong[]? array)
#else
        public void BinaryDeserialize(ref ulong[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(ulong) + sizeof(int);
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
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeULongArray(BinaryDeserializer deserializer)
        {
            var array = default(ulong[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeULongListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<ulong>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeULongLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<ulong>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableULongArray(BinaryDeserializer deserializer)
        {
            var array = default(ulong?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ulong[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ulong[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<ulong>? array)
#else
        public void BinaryDeserialize(ref ListArray<ulong> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = (long)length * sizeof(ulong) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<ulong>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<ulong> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
        public void BinaryDeserialize(ref LeftArray<ulong> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = (long)length * sizeof(ulong) + sizeof(int);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array.Array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<ulong> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref ulong?[]? array)
#else
        public void BinaryDeserialize(ref ulong?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapSize = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapSize <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.BinarySerialize.DeserializeArrayMap arrayMap = new AutoCSer.BinarySerialize.DeserializeArrayMap(Current + sizeof(int));
                        Current += mapSize;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(ulong*)Current;
                                Current += sizeof(ulong);
                            }
                        }
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ulong?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ulong?[] array)
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
        public int DeserializeBuffer(Func<int, ulong[]?> getBuffer, out ulong[]? buffer)
#else
        public int DeserializeBuffer(Func<int, ulong[]> getBuffer, out ulong[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = (long)length * sizeof(ulong) + sizeof(int);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (ulong* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(ulong));
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
                if (length == 0) buffer = EmptyArray<ulong>.Array;
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
