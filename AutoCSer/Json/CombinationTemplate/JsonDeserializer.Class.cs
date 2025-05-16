using System;
/*string;object;Type*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 反序列化
    /// </summary>
    public sealed unsafe partial class JsonDeserializer
    {
        /// <summary>
        /// 基础类型解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref string? value)
#else
        private static void primitiveDeserialize(JsonDeserializer jsonDeserializer, ref string value)
#endif
        {
            jsonDeserializer.JsonDeserialize(ref value);
        }
    }
}
