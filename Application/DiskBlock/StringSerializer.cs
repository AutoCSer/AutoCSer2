using AutoCSer.Memory;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 字符串序列化
    /// </summary>
    internal sealed class StringSerializer : WriteBufferSerializer
    {
        /// <summary>
        /// 字符串
        /// </summary>
        private readonly string value;
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="value">字符串</param>
        internal StringSerializer(string value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(value);
        }
    }
}
