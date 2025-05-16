using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;float;double;decimal;char;DateTime;TimeSpan;Guid*/

//Int128;UInt128;Half;
namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 可空类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref ulong? value)
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}
