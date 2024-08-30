using AutoCSer.Memory;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 字符串序列化
    /// </summary>
    internal sealed class StringSerializer
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
        internal unsafe void Serialize(AutoCSer.BinarySerializer serializer)
        {
            int index = serializer.SerializeBufferStart();
            if (index >= 0) serializer.SerializeBufferEnd(index, serializer.SerializeBuffer(value));
        }
    }
}
