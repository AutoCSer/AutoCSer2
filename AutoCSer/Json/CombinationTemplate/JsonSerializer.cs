using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;string;SubString;Type;UInt128;Int128;Half*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ulong value)
        {
            jsonSerializer.JsonSerialize(value);
        }
    }
}
