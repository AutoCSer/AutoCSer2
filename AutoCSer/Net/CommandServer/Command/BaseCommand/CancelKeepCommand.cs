using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 取消异步保持调用回调命令
    /// </summary>
    internal sealed class CancelKeepCommand : BaseCommand
    {
        /// <summary>
        /// 取消保持调用的会话回调标识
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
            if (stream.Data.Pointer.FreeSize >= sizeof(uint) + sizeof(CallbackIdentity) || buildInfo.Count == 0)
            {
                byte* data = stream.GetBeforeMove(sizeof(uint) + sizeof(CallbackIdentity));
                *(uint*)data = CommandListener.CancelKeepMethodIndex;
                *(CallbackIdentity*)(data + sizeof(uint)) = callbackIdentity;
                buildInfo.AddCount();
                return LinkNext;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
    }
}
