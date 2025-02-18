using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取字符串
    /// </summary>
#if NetStandard21
    internal sealed class ReadStringAwaiter : ReadAwaiter<string?>
#else
    internal sealed class ReadStringAwaiter : ReadAwaiter<string>
#endif
    {
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        internal ReadStringAwaiter(IDiskBlockClient client, BlockIndex blockIndex) : base(client, blockIndex) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref result);
        }
    }
}
