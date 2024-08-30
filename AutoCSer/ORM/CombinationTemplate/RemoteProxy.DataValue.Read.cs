using System;
/*long,Long;int,Int;short,Short;byte,Byte;bool,Bool;DateTime,DateTime;DateTimeOffset,DateTimeOffset;TimeSpan,TimeSpan;decimal,Decimal;Guid,Guid;double,Double;float,Float*/

namespace AutoCSer.ORM.RemoteProxy
{
    /// <summary>
    /// 数据值包装
    /// </summary>
    internal partial struct DataValue
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long ReadLong()
        {
            return !isNull ? readLong() : default(long);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long? ReadLongNullable()
        {
            if (isNull) return null;
            return readLong();
        }
    }
}
