using System;
/*UShort,ushort;Short,short;SByte,sbyte;Byte,byte;Char,char*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制反数据序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="value">逻辑值</param>
        private void primitiveDeserialize(ref ushort? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(ushort*)Current;
            else value = null;
            Current += sizeof(int);
        }
#if AOT
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeUShortArray(BinaryDeserializer deserializer)
        {
            var array = default(ushort[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeUShortListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<ushort>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static object primitiveMemberDeserializeUShortLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<ushort>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableUShortArray(BinaryDeserializer deserializer)
        {
            var array = default(ushort?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
#endif
        /// <summary>
        /// 整数值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">逻辑值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ushort[]? array)
#else
        public void BinaryDeserialize(ref ushort[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(ushort) + (3 + sizeof(int))) & (long.MaxValue - 3);
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
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<ushort>? array)
#else
        public void BinaryDeserialize(ref ListArray<ushort> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(ushort) + (3 + sizeof(int))) & (long.MaxValue - 3);
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
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<ushort>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<ushort> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
        public void BinaryDeserialize(ref LeftArray<ushort> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long size = ((long)length * sizeof(ushort) + (3 + sizeof(int))) & (long.MaxValue - 3);
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
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<ushort> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="array">数组</param>
#if NetStandard21
        public void BinaryDeserialize(ref ushort?[]? array)
#else
        public void BinaryDeserialize(ref ushort?[] array)
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
                        byte* start = Current;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                array[index] = *(ushort*)Current;
                                Current += sizeof(ushort);
                            }
                        }
                        Current += (int)(start - Current) & 3;
                        if (Current > End) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// 数组反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">数组</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort?[] array)
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
        public int DeserializeBuffer(Func<int, ushort[]?> getBuffer, out ushort[]? buffer)
#else
        public int DeserializeBuffer(Func<int, ushort[]> getBuffer, out ushort[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(ushort) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (ushort* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(ushort));
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
                if (length == 0) buffer = EmptyArray<ushort>.Array;
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
