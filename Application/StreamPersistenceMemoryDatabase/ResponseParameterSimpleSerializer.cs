﻿using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ResponseParameterSimpleSerializer<T> : ResponseParameterSerializer
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// 返回参数序列化
        /// </summary>
        internal ResponseParameterSimpleSerializer() { }
        /// <summary>
        /// 返回参数序列化
        /// </summary>
        /// <param name="value">数据</param>
#if NetStandard21
        internal ResponseParameterSimpleSerializer(T? value)
#else
        internal ResponseParameterSimpleSerializer(T value)
#endif
        {
            this.Value.ReturnValue = value;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref Value);
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
            return deserializer.SimpleDeserialize(ref Value) ? this : null;
        }
    }
}
