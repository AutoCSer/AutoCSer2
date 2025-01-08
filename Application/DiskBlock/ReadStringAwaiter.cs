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
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref result);
        }
    }
}
