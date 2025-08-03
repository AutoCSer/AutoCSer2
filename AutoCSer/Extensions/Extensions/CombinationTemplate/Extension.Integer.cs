using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// ulong expansion operation
    /// ulong 扩展操作
    /// </summary>
    public partial struct ULongExtensions
    {
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new string ToString()
        {
            return value.toString();
        }
    }
}
