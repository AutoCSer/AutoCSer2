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
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 命令添加状态
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
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
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
                *(uint*)write = (uint)CommandListener.CustomDataMethodIndex | (uint)CommandFlagsEnum.SendData;
                *(int*)(write + sizeof(uint)) = dataSize;
                *(int*)(write + (sizeof(uint) + sizeof(int))) = data.Length;
                if (data.Length != 0)
                {
                    AutoCSer.Common.CopyTo(data.Array, data.Start, write + (sizeof(uint) + sizeof(int) * 2), data.Length);
                }
                data.SetEmpty();
                buildInfo.AddCount();
                return LinkNext;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }

        /// <summary>
        /// 等待添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<bool> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 是否成功添加输出队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetResult()
        {
            return pushState == CommandPushStateEnum.Success;
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
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CustomDataCommand GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// 添加命令到发送队列
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
        /// 检查等待添加队列命令
        /// </summary>
        /// <returns>是否需要继续等待</returns>
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
