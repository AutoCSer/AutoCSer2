using System;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 已完成读取数据 await ReadResult{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CompletedReadAwaiter<T> : ReadAwaiter<T>
    {
        /// <summary>
        /// 读取数据
        /// </summary>
        private readonly new ReadResult<T> result;
        /// <summary>
        /// 已完成读取数据
        /// </summary>
        /// <param name="result"></param>
        internal CompletedReadAwaiter(ReadResult<T> result)
        {
            this.result = result;
            setCompleted();
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public override ReadResult<T> GetResult()
        {
            return result;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(BinaryDeserializer deserializer)
        {
            throw new InvalidOperationException();
        }
    }
}
