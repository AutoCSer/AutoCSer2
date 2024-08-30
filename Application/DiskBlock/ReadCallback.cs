using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据回调
    /// </summary>
    internal sealed class ReadCallback : ReadCallback<byte[]>
    {
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="callback"></param>
        internal ReadCallback(Action<ReadResult<byte[]>> callback = null) : base(callback) { }
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
        protected T value;
        /// <summary>
        /// 读取数据回调
        /// </summary>
        private readonly Action<ReadResult<T>> callback;
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="callback"></param>
        protected ReadCallback(Action<ReadResult<T>> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 读取数据回调
        /// </summary>
        /// <param name="buffer"></param>
        internal void Callback(CommandClientReturnValue<ReadBuffer> buffer)
        {
            callback(GetResult(buffer));
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
