using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal sealed class RequestParameterSimpleSerializer<T> : RequestParameterSerializer
        where T : struct
    {
        /// <summary>
        /// 数据
        /// </summary>
        private T value;
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">data</param>
        internal RequestParameterSimpleSerializer(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">data</param>
        internal RequestParameterSimpleSerializer(ref T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref value);
        }
    }
}
