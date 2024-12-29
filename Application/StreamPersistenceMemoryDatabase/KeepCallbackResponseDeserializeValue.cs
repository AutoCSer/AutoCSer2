using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackResponseDeserializeValue<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        internal KeepCallbackResponseDeserializeValue(T? value)
#else
        internal KeepCallbackResponseDeserializeValue(T value)
#endif
        {
            Value.ReturnValue = value;
        }

        /// <summary>
        /// 获取反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static T? GetValue(object value)
#else
        internal static T GetValue(object value)
#endif
        {
            return ((KeepCallbackResponseDeserializeValue<T>)value).Value.ReturnValue;
        }
    }
}
