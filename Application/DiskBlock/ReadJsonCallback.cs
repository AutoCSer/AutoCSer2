using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取 JSON 对象回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if NetStandard21
    internal sealed class ReadJsonCallback<T> : ReadCallback<T?>
#else
    internal sealed class ReadJsonCallback<T> : ReadCallback<T>
#endif
    {
        /// <summary>
        /// 读取 JSON 对象回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal ReadJsonCallback(Action<ReadResult<T?>>? callback = null) : base(callback) { }
#else
        internal ReadJsonCallback(Action<ReadResult<T>> callback = null) : base(callback) { }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal unsafe void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.CommandClientDeserializeJson(ref value);
        }
    }
}
