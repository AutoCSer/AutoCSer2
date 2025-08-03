using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int;ushort,UShort;short,Short;byte,Byte;sbyte,SByte;string,String;DateTime,DateTime;TimeSpan,TimeSpan*/

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Get the extended operation encapsulation
    /// 获取扩展操作封装
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// Get the AutoCSer extension encapsulation
        /// 获取 AutoCSer 扩展封装
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static AutoCSer.Extensions.ULongExtensions AutoCSerExtensions(this ulong value)
        {
            return new AutoCSer.Extensions.ULongExtensions(value);
        }
    }
    /// <summary>
    /// ulong expansion operation
    /// ulong 扩展操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct ULongExtensions
    {
        /// <summary>
        /// Value
        /// </summary>
        private readonly ulong value;
        /// <summary>
        /// ulong expansion operation
        /// ulong 扩展操作
        /// </summary>
        /// <param name="value"></param>
        public ULongExtensions(ulong value)
        {
            this.value = value;
        }
    }
}
