using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取二进制序列化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ReadBinaryAwaiter<T> : ReadAwaiter<T>
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            ServerReturnValue<T> value = this.result != null ? new ServerReturnValue<T>(this.result) : default(ServerReturnValue<T>);
            deserializer.InternalIndependentDeserializeNotReference(ref value);
            this.result = value.ReturnValue;
        }
    }
}
