using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Keep callback object of the command
    /// 命令保持回调对象
    /// </summary>
    public sealed class CommandKeepCallback : SecondTimerTaskArrayNode, IDisposable
    {
        /// <summary>
        /// Keep callback command
        /// 保持回调命令
        /// </summary>
        public readonly KeepCommand Command;
        /// <summary>
        /// Cancel the command to keep the callback
        /// 取消保持回调的命令
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private CancelKeepCommand cancelKeepCommand;
        /// <summary>
        /// Session callback identifier
        /// 会话回调标识
        /// </summary>
        private CallbackIdentity callbackIdentity;
        /// <summary>
        /// Keep callback object of the command
        /// 命令保持回调对象
        /// </summary>
        /// <param name="command">客户端命令</param>
        internal CommandKeepCallback(KeepCommand command) : base(AutoCSer.Threading.SecondTimer.TaskArray, 0, SecondTimerTaskThreadModeEnum.Synchronous)
        {
            this.Command = command;
        }
        /// <summary>
        /// Release resources (Send cancellation commands to the server regularly)
        /// 释放资源（定时向服务器发送取消命令）
        /// </summary>
        public void Dispose()
        {
            Cancel(true);
        }
        /// <summary>
        /// Forced shutdown (Send a cancellation command to the server immediately)
        /// 强制关闭（立即向服务器发送取消命令）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Close()
        {
            Cancel(false);
        }
        /// <summary>
        /// Cancel the keep callback
        /// 取消保持回调
        /// </summary>
        /// <param name="isTimerTask"></param>
        internal void Cancel(bool isTimerTask)
        {
            CallbackIdentity callbackIdentity = this.callbackIdentity;
            this.callbackIdentity.SetNull();
            if (callbackIdentity.Index != 0 && callbackIdentity.Index != uint.MaxValue)
            {
                CancelKeepCallbackData cancelKeepCallbackData = new CancelKeepCallbackData(ref callbackIdentity);
                Command.Controller.Socket.CommandPool.CancelCallback(ref cancelKeepCallbackData);
                if(isTimerTask) appendTask(callbackIdentity);
                else new CancelKeepCommand(Command.Controller.Socket, callbackIdentity).Push();
            }
        }
        /// <summary>
        /// Add to the scheduled task
        /// 添加到定时任务
        /// </summary>
        /// <param name="callbackIdentity"></param>
#if NET8
        [MemberNotNull(nameof(cancelKeepCommand))]
#endif
        private void appendTask(CallbackIdentity callbackIdentity)
        {
            CommandClientSocket socket = Command.Controller.Socket;
            cancelKeepCommand = new CancelKeepCommand(socket, callbackIdentity);
            TimeoutSeconds = SecondTimer.CurrentSeconds + socket.Client.Config.CancelKeepCallbackSeconds;
            AppendTaskArray();
        }
        /// <summary>
        /// Cancel the keep callback
        /// 取消保持回调
        /// </summary>
        /// <param name="isTimerTask"></param>
        /// <returns></returns>
        internal SecondTimerAppendTaskStateEnum TryCancel(bool isTimerTask)
        {
            CallbackIdentity callbackIdentity = this.callbackIdentity;
            this.callbackIdentity.SetNull();
            if (callbackIdentity.Index != 0 && callbackIdentity.Index != uint.MaxValue)
            {
                CommandClientSocket socket = Command.Controller.Socket;
                CancelKeepCallbackData cancelKeepCallbackData = new CancelKeepCallbackData(ref callbackIdentity);
                socket.CommandPool.CancelCallback(ref cancelKeepCallbackData);
                if (isTimerTask)
                {
                    cancelKeepCommand = new CancelKeepCommand(socket, callbackIdentity);
                    TimeoutSeconds = SecondTimer.CurrentSeconds + socket.Client.Config.CancelKeepCallbackSeconds;
                    SecondTimerAppendTaskStateEnum appendTaskState = TryAppendTaskArray();
                    if (appendTaskState != SecondTimerAppendTaskStateEnum.Completed) return appendTaskState;
                }
                else new CancelKeepCommand(socket, callbackIdentity).Push();
            }
            return SecondTimerAppendTaskStateEnum.Completed;
        }
        /// <summary>
        /// Set the session callback identifier
        /// 设置会话回调标识
        /// </summary>
        /// <param name="index"></param>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int index, uint identity)
        {
            if (callbackIdentity.Index != uint.MaxValue) callbackIdentity.Set(index, identity);
            else appendTask(new CallbackIdentity((uint)index, identity));
        }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal override void OnTimer()
        {
            cancelKeepCommand.Push();
        }
    }
}
