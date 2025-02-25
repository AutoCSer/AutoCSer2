﻿using AutoCSer.Net.CommandServer;
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
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            throw new InvalidOperationException();
        }
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
            ServerReturnValue<T> value = default(ServerReturnValue<T>);
            return deserializer.InternalIndependentDeserializeNotReference(ref value) ? new KeepCallbackResponseDeserializeValue<T>(value.ReturnValue) : null;
        }

        /// <summary>
        /// 返回参数序列化
        /// </summary>
        internal static readonly KeepCallbackResponseParameterBinarySerializer<T> Default = new KeepCallbackResponseParameterBinarySerializer<T>();
    }
}
