using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 反序列化线程静态变量
    /// </summary>
    internal sealed class ThreadStaticDeserializer
    {
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        internal readonly JsonDeserializer Deserializer = new JsonDeserializer();

        /// <summary>
        /// 线程静态变量
        /// </summary>
        [ThreadStatic]
#if NetStandard21
        private static ThreadStaticDeserializer? value;
#else
        private static ThreadStaticDeserializer value;
#endif
        /// <summary>
        /// 默认线程静态变量
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static ThreadStaticDeserializer Get()
        {
            return value ?? (value = new ThreadStaticDeserializer());
        }
    }
}
