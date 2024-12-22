using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class BinarySerializeResponseParameterAwaiter<T> : ResponseParameterAwaiter<T>
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node"></param>
        internal BinarySerializeResponseParameterAwaiter(ClientNode node) : base(node) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.InternalIndependentDeserializeNotReference(ref Value);
        }
    }
}
