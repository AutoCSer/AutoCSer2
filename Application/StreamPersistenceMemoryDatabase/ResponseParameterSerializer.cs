using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数序列化
    /// </summary>
    internal abstract class ResponseParameterSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal abstract void Serialize(AutoCSer.BinarySerializer serializer);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>目标对象</returns>
        internal abstract object Deserialize(AutoCSer.BinaryDeserializer deserializer);
    }
}
