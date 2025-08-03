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
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        public ReadBinaryAwaiter(IDiskBlockClient client, BlockIndex blockIndex) : base(client, blockIndex) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            ServerReturnValue<T> value = this.result != null ? new ServerReturnValue<T>(this.result) : default(ServerReturnValue<T>);
            if (AutoCSer.SimpleSerializeType<T>.IsSimple)
            {
#if AOT
                SimpleDeserialize(deserializer, ref value);
#else
                deserializer.SimpleDeserialize(ref value);
#endif
            }
            else
            {
#if AOT
                InternalIndependentDeserializeNotReference(deserializer, ref value);
#else
                deserializer.InternalIndependentDeserializeNotReference(ref value);
#endif
            }
            this.result = value.ReturnValue;
        }
    }
}
