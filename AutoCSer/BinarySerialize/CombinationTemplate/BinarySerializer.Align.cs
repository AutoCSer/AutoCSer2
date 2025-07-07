using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;Float,float;Double,double;Decimal,decimal;DateTime,DateTime;TimeSpan,TimeSpan*/

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, ulong value)
        {
            binarySerializer.Stream.Write(value);
        }
#if AOT
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(ulong? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(ulong) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeULong(BinarySerializer binarySerializer, object objectValue)
        {
            ulong? value = (ulong?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#else
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, ulong? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
#endif
    }
}
