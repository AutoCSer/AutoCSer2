﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 默认空返回参数序列化
    /// </summary>
    internal sealed class NullResponseParameterSerializer : ResponseParameterSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer) { throw new InvalidOperationException(); }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>目标对象</returns>
#if NetStandard21
        internal override object? Deserialize(AutoCSer.BinaryDeserializer deserializer)
#else
        internal override object Deserialize(AutoCSer.BinaryDeserializer deserializer)
#endif
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 默认空返回参数序列化
        /// </summary>
        internal static readonly NullResponseParameterSerializer Null = new NullResponseParameterSerializer();
    }
}