using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取 JSON 对象回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ReadJsonCallback<T> : ReadCallback<T>
    {
        /// <summary>
        /// 读取 JSON 对象回调
        /// </summary>
        /// <param name="callback"></param>
        internal ReadJsonCallback(Action<ReadResult<T>> callback = null) : base(callback) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal unsafe void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            byte* end = deserializer.DeserializeBufferStart();
            if (end != null)
            {
                deserializer.DeserializeJson(((CommandClientSocket)deserializer.Context).ReceiveJsonDeserializer, out value);
                deserializer.DeserializeBufferEnd(end);
            }
        }
    }
}
