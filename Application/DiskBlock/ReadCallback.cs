using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据回调
    /// </summary>
#if NetStandard21
    internal sealed class ReadCallback : ReadCallback<byte[]?>
#else
    internal sealed class ReadCallback : ReadCallback<byte[]>
#endif
    {
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal ReadCallback(Action<ReadResult<byte[]?>>? callback = null) : base(callback) { }
#else
        internal ReadCallback(Action<ReadResult<byte[]>> callback = null) : base(callback) { }
#endif
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref value);
        }
    }
    /// <summary>
    /// 读取数据回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class ReadCallback<T>
    {
        /// <summary>
        /// 回调数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T value;
        /// <summary>
        /// 读取数据回调
        /// </summary>
#if NetStandard21
        private readonly Action<ReadResult<T>>? callback;
#else
        private readonly Action<ReadResult<T>> callback;
#endif
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        protected ReadCallback(Action<ReadResult<T>>? callback)
#else
        protected ReadCallback(Action<ReadResult<T>> callback)
#endif
        {
            this.callback = callback;
        }
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="buffer"></param>
        internal void Callback(CommandClientReturnValue<ReadBuffer> buffer)
        {
            callback.notNull()(GetResult(buffer));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal ReadResult<T> GetResult(CommandClientReturnValue<ReadBuffer> buffer)
        {
            if (buffer.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                if (buffer.Value.State == ReadBufferStateEnum.Success) return new ReadResult<T>(value);
                return new ReadResult<T>(buffer.Value.State);
            }
            return new ReadResult<T>(buffer.ReturnType, buffer.ErrorMessage);
        }
    }
}
