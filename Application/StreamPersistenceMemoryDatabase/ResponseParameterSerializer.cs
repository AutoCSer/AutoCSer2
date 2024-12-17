using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数序列化
    /// </summary>
    public abstract class ResponseParameterSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public abstract void Serialize(AutoCSer.BinarySerializer serializer);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>目标对象</returns>
#if NetStandard21
        internal abstract object? Deserialize(AutoCSer.BinaryDeserializer deserializer);
#else
        internal abstract object Deserialize(AutoCSer.BinaryDeserializer deserializer);
#endif
    }
}
