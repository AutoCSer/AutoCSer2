using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 回调委托返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CallbackCommandBinarySerializeResponseParameter<T> : CallbackCommandResponseParameter<T>
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="node"></param>
        /// <param name="callback"></param>
        internal CallbackCommandBinarySerializeResponseParameter(ClientNode node, Action<ResponseResult<T>> callback) : base(node, callback) { }
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
