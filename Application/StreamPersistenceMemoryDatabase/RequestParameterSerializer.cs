using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 请求参数序列化
    /// </summary>
    internal abstract class RequestParameterSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal abstract void Serialize(AutoCSer.BinarySerializer serializer);
    }
}
