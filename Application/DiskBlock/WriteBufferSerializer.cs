﻿using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 写入数据缓冲区内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    internal abstract class WriteBufferSerializer
    {
        /// <summary>
        /// 序列化，仅支持 struct 并且不能是自定义序列化类型
        /// </summary>
        /// <param name="serializer"></param>
        internal abstract void Serialize(AutoCSer.BinarySerializer serializer);
    }
    /// <summary>
    /// 内部成员对象序列化为一个可独立反序列化的数据
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal sealed class WriteBufferSerializer<
#if AOT
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicFields)]
#endif
    T> : WriteBufferSerializer
        where T : struct
    {
        /// <summary>
        /// 数据
        /// </summary>
        private T value;
        /// <summary>
        /// 内部成员对象序列化为一个可独立反序列化的数据
        /// </summary>
        /// <param name="value">data</param>
        internal WriteBufferSerializer(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// 序列化，仅支持 struct 并且不能是自定义序列化类型
        /// </summary>
        /// <param name="serializer"></param>
        internal override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.InternalIndependentSerializeNotNull(ref value);
        }
    }
}
