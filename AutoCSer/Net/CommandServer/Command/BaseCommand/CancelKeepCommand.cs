using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 取消异步保持回调回调命令
    /// </summary>
    internal sealed class CancelKeepCommand : BaseCommand
    {
        /// <summary>
        /// 取消保持回调的会话回调标识
        /// </summary>
        private readonly CallbackIdentity callbackIdentity;
        /// <summary>
        /// 心跳检测命令
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callbackIdentity"></param>
        internal CancelKeepCommand(CommandClientSocket socket, CallbackIdentity callbackIdentity) : base(socket, KeepCallbackCommand.KeepCallbackMethod)
        {
            this.callbackIdentity = callbackIdentity;
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
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) + sizeof(CallbackIdentity) || buildInfo.Count == 0)
            {
                byte* data = stream.GetBeforeMove(sizeof(uint) + sizeof(CallbackIdentity));
                var nextCommand = LinkNext;
                *(uint*)data = CommandListener.CancelKeepMethodIndex;
                *(CallbackIdentity*)(data + sizeof(uint)) = callbackIdentity;
                buildInfo.AddCount();
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
    }
}
