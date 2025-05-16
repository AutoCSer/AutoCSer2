using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Guid*/

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public sealed unsafe partial class JsonSerializer
    {
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, ulong? value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 可空类型转换
        /// </summary>
        /// <param name="value">可空值</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(ulong? value)
        {
            if (value.HasValue) JsonSerialize(value.Value);
            else CharStream.WriteJsonNull();
        }
    }
}
