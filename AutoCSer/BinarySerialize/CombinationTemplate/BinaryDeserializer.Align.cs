using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;Float,float;Double,double;Decimal,decimal;DateTime,DateTime;TimeSpan,TimeSpan;Guid,Guid*/

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
        private void primitiveDeserialize(ref ulong value)
        {
            value = *(ulong*)Current;
            Current += sizeof(ulong);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">整数</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ulong value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// 整数反序列化
        /// </summary>
        /// <param name="value">整数</param>
        public void BinaryDeserialize(ref ulong? value)
        {
            if (*(int*)Current == 0)
            {
                value = *(ulong*)(Current + sizeof(int));
                Current += sizeof(int) + sizeof(ulong);
            }
            else
            {
                Current += sizeof(int);
                value = null;
            }
        }
#if AOT
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        private static object primitiveMemberDeserializeNullableULong(BinaryDeserializer deserializer)
        {
            ulong? value = default(ulong?);
            deserializer.BinaryDeserialize(ref value);
#pragma warning disable CS8603
            return value;
#pragma warning restore CS8603
        }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref ulong? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// 读取数据 
        /// </summary>
        /// <returns></returns>
        public bool Read(out ulong value)
        {
            value = *(ulong*)Current;
            if ((Current += sizeof(ulong)) <= End) return true;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
    }
}
