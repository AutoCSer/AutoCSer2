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
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeJsonBuffer(ref result);
        }
    }
}
