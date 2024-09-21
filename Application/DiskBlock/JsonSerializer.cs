using AutoCSer.Memory;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class JsonSerializer<T> : WriteBufferSerializer
    {
        /// <summary>
        /// 数据
        /// </summary>
        private T value;
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="value"></param>
        internal JsonSerializer(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            int index = serializer.SerializeBufferStart();
            if (index >= 0) serializer.SerializeBufferEnd(index, ((CommandClientSocket)serializer.Context).JsonSerializeBuffer(ref value, serializer.Stream));
        }
    }
}
