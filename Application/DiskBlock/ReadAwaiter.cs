using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DiskBlock
{
    /// <summary>
    /// 读取数据
    /// </summary>
#if NetStandard21
    internal sealed class ReadAwaiter : ReadAwaiter<byte[]?>
#else
    internal sealed class ReadAwaiter : ReadAwaiter<byte[]>
#endif
    {
        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        internal ReadAwaiter(IDiskBlockClient client, BlockIndex blockIndex) : base(client, blockIndex) { }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref result);
        }
    }
    /// <summary>
    /// 读取数据 await ReadResult{T}
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReadAwaiter<T> : ReadBufferDeserializer, INotifyCompletion
    {
        /// <summary>
        /// 读取数据命令
        /// </summary>
        private ReturnCommand<ReadBuffer> command = CompletedReturnCommand<ReadBuffer>.Default;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 读取数据
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected T result;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 读取数据
        /// </summary>
        protected ReadAwaiter() { }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="client">磁盘块客户端接口</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        internal ReadAwaiter(IDiskBlockClient client, BlockIndex blockIndex)
        {
            command = client.Read(this, blockIndex);
            command.OnCompleted(onCompleted);
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<ReadResult<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Only for supporting await
        /// 仅用于支持 await
        /// </summary>
        /// <returns></returns>
        public virtual ReadResult<T> GetResult()
        {
            CommandClientReturnValue<ReadBuffer> buffer = command.GetResult();
            if (buffer.ReturnType == CommandClientReturnTypeEnum.Success)
            {
                if (buffer.Value.State == ReadBufferStateEnum.Success) return new ReadResult<T>(result);
                return new ReadResult<T>(buffer.Value.State);
            }
            return new ReadResult<T>(buffer.ReturnType, buffer.ErrorMessage);
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReadAwaiter<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 设置错误结果并尝试回调操作
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void onCompleted()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 设置完成状态
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void setCompleted()
        {
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
    }
}
