using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;

namespace AutoCSer.Net
{
    /// <summary>
    /// 自定义数据包命令 await bool 是否成功添加输出队列
    /// </summary>
    public sealed class CustomDataCommand : BaseCommand, INotifyCompletion
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private SubArray<byte> data;
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
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// The status of the reqeust command added to the output queue
        /// 请求命令添加到输出队列的状态
        /// </summary>
        private CommandPushStateEnum pushState;
        /// <summary>
        /// 自定义数据包命令
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">输出参数</param>
        internal CustomDataCommand(CommandClientSocket socket, ref SubArray<byte> data) : base(socket, AutoCSer.Net.CommandServer.KeepCallbackCommand.KeepCallbackMethod)
        {
            this.data = data;
            Push();
        }
        /// <summary>
        /// 自定义数据包命令
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">输出参数</param>
        internal CustomDataCommand(CommandClientSocket socket, byte[] data) : base(socket, AutoCSer.Net.CommandServer.KeepCallbackCommand.KeepCallbackMethod)
        {
            this.data.Set(data, 0, data.Length);
            Push();
        }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal unsafe override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal unsafe override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            int dataSize = (data.Length + (sizeof(int) + 3)) & (int.MaxValue), prepLength = dataSize + sizeof(uint) + sizeof(int);
            if (buildInfo.Count == 0 || (buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= prepLength
                || (stream.Data.Pointer.FreeSize >= prepLength && prepLength > buildInfo.SendBufferSize - Command.StreamStartIndex))
            {
                byte* write = stream.GetCanResizeBeforeMove(prepLength);
                var nextCommand = LinkNext;
                *(uint*)write = (uint)CommandListener.CustomDataMethodIndex | (uint)CommandFlagsEnum.SendData;
                *(int*)(write + sizeof(uint)) = dataSize;
                *(int*)(write + (sizeof(uint) + sizeof(int))) = data.Length;
                if (data.Length != 0)
                {
                    AutoCSer.Common.CopyTo(data.Array, data.Start, write + (sizeof(uint) + sizeof(int) * 2), data.Length);
                }
                data.SetEmpty();
                buildInfo.AddCount();
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }

        /// <summary>
        /// Wait for the command to add the output queue
        /// 等待命令添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<bool> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Whether the output queue has been successfully added
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetResult()
        {
            return pushState == CommandPushStateEnum.Success;
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
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CustomDataCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Add commands to the output queue
        /// 添加命令到输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal new void Push()
        {
            pushState = Socket.TryPush(this);
            if (pushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                continuation = Common.EmptyAction;
            }
        }
        /// <summary>
        /// The command waiting for idle output attempts to be added to the output queue again
        /// 等待空闲输出的命令再次尝试添加到输出队列
        /// </summary>
        /// <returns>Is it necessary to keep waiting
        /// 是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            pushState = Socket.TryPush(this);
            if (pushState != CommandPushStateEnum.WaitCount)
            {
                IsCompleted = true;
                if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
                {
                    //try
                    //{
                    Task.Run(continuation);
                    //}
                    //catch (Exception exception)
                    //{
                    //    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                    //}
                }
                return false;
            }
            return true;
        }
    }
}
