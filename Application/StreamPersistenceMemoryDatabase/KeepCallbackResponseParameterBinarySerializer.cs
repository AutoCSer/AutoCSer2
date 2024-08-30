using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackResponseParameterBinarySerializer<T> : ResponseParameterSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>目标对象</returns>
        internal override object Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            KeepCallbackResponseDeserializeValue<T> value = new KeepCallbackResponseDeserializeValue<T>();
            return deserializer.InternalIndependentDeserializeNotReference(ref value.Value) ? value : null;
        }

        /// <summary>
        /// 返回参数序列化
        /// </summary>
        internal static readonly KeepCallbackResponseParameterBinarySerializer<T> Default = new KeepCallbackResponseParameterBinarySerializer<T>();
    }
}
