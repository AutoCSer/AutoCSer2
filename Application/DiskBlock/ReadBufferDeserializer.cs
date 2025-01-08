using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据缓冲区反序列化
    /// </summary>
    public abstract class ReadBufferDeserializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal abstract void Deserialize(AutoCSer.BinaryDeserializer deserializer);
    }
}
