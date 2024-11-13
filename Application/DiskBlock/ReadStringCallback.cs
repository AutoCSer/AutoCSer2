using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取字符串回调
    /// </summary>
#if NetStandard21
    internal sealed class ReadStringCallback : ReadCallback<string?>
#else
    internal sealed class ReadStringCallback : ReadCallback<string>
#endif
    {
        /// <summary>
        /// 读取字符串回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal ReadStringCallback(Action<ReadResult<string?>>? callback = null) : base(callback) { }
#else
        internal ReadStringCallback(Action<ReadResult<string>> callback = null) : base(callback) { }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal unsafe void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            byte* end = deserializer.DeserializeBufferStart();
            if (end != null)
            {
                deserializer.DeserializeBuffer(ref value);
                deserializer.DeserializeBufferEnd(end);
            }
        }
    }
}
