using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;UShort,ushort;Short,short;Byte,byte;SByte,sbyte;Bool,bool;Float,float;Double,double;Decimal,decimal;Char,char;DateTime,DateTime;TimeSpan,TimeSpan;Guid,Guid*/

namespace AutoCSer
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer
    {
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref ulong value)
        {
            value = *(ulong*)Current;
            Current += sizeof(ulong);
        }
#if AOT
        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeULong(BinaryDeserializer deserializer)
        {
            ulong value = default(ulong);
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
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref ulong value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
#endif
    }
}
