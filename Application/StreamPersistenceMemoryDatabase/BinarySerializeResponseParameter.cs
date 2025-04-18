﻿using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BinarySerializeResponseParameter<T> : ResponseParameter<T>
    {
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="value"></param>
        internal BinarySerializeResponseParameter(T value) : base(value) { }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.InternalIndependentSerializeNotNull(ref Value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        protected override void deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.InternalIndependentDeserializeNotReference(ref Value);
        }
        /// <summary>
        /// 创建持续回调返回参数
        /// </summary>
        /// <returns></returns>
        internal override KeepCallbackResponseParameter CreateKeepCallback()
        {
            return new KeepCallbackResponseParameter(new ResponseParameterBinarySerializer<T>(Value.ReturnValue), 0);
        }
    }
}
