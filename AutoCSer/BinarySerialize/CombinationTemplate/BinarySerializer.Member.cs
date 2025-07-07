using System;
/*ULong,ulong;Long,long;UInt,uint;Int,int;UShort,ushort;Short,short;Byte,byte;SByte,sbyte;Bool,bool;Float,float;Double,double;Decimal,decimal;Char,char;DateTime,DateTime;TimeSpan,TimeSpan*/

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer
    {
#if AOT
        /// <summary>
        /// Member serialization
        /// </summary>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(ulong value)
        {
            Stream.Write(value);
        }
        /// <summary>
        /// Member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        internal static void PrimitiveMemberULongReflection(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Stream.Data.Pointer.Write((ulong)value);
        }
#else
        /// <summary>
        /// Member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void PrimitiveMemberSerialize(BinarySerializer binarySerializer, ulong value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value);
        }
#endif
    }
}
