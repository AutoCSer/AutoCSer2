using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取二进制序列化对象回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ReadBinaryCallback<T> : ReadCallback<T>
    {
        /// <summary>
        /// 读取二进制序列化对象回调
        /// </summary>
        /// <param name="callback"></param>
        internal ReadBinaryCallback(Action<ReadResult<T>> callback = null) : base(callback) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal void DeserializeNotReference(AutoCSer.BinaryDeserializer deserializer)
        {
            ServerReturnValue<T> value = new ServerReturnValue<T>(this.value);
            deserializer.InternalIndependentDeserializeNotReference(ref value);
            this.value = value.ReturnValue;
        }
    }
}
