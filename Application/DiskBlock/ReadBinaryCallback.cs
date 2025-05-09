using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取二进制序列化对象回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if NetStandard21
    internal sealed class ReadBinaryCallback<T> : ReadCallback<T?>
#else
    internal sealed class ReadBinaryCallback<T> : ReadCallback<T>
#endif
    {
        /// <summary>
        /// 读取二进制序列化对象回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal ReadBinaryCallback(Action<ReadResult<T?>>? callback = null) : base(callback) { }
#else
        internal ReadBinaryCallback(Action<ReadResult<T>> callback = null) : base(callback) { }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            ServerReturnValue<T> value = this.value != null ? new ServerReturnValue<T>(this.value) : default(ServerReturnValue<T>);
#if AOT
            InternalIndependentDeserializeNotReference(deserializer, ref value);
#else
            deserializer.InternalIndependentDeserializeNotReference(ref value);
#endif
            this.value = value.ReturnValue;
        }
    }
}
