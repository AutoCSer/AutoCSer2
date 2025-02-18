using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取 JSON 对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ReadJsonAwaiter<T> : ReadAwaiter<T>
    {
        /// <summary>
        /// 读取 JSON 对象
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        internal ReadJsonAwaiter(IDiskBlockClient client, BlockIndex blockIndex) : base(client, blockIndex) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeJsonBuffer(ref result);
        }
    }
}
