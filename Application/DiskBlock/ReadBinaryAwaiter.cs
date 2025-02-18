using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取二进制序列化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ReadBinaryAwaiter<T> : ReadAwaiter<T>
    {
        /// <summary>
        /// 读取二进制序列化对象
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        public ReadBinaryAwaiter(IDiskBlockClient client, BlockIndex blockIndex) : base(client, blockIndex) { }
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
