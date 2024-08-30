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
        /// 获取反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T GetValue(object value)
        {
            return ((KeepCallbackResponseDeserializeValue<T>)value).Value.ReturnValue;
        }
    }
}
