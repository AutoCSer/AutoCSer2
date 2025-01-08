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
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<ReadResult<T>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// await 支持
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
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReadAwaiter<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
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
        /// 设置返回值命令
        /// </summary>
        /// <param name="command"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ReturnCommand<ReadBuffer> command)
        {
            this.command = command;
            command.OnCompleted(onCompleted);
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
