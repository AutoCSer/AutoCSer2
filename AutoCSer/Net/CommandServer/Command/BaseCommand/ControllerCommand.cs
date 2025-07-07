using AutoCSer.Memory;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务命令控制器查询命令
    /// </summary>
    internal sealed class ControllerCommand : BaseCommand
    {
        /// <summary>
        /// 服务命令控制器查询命令
        /// </summary>
        /// <param name="socket"></param>
        internal ControllerCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
        /// <summary>
        /// Generate the input data of the request command
        /// 生成请求命令输入数据
        /// </summary>
        /// <param name="buildInfo"></param>
        /// <returns>The next request command
        /// 下一个请求命令</returns>
#if NetStandard21
        internal override Command? Build(ref ClientBuildInfo buildInfo)
#else
        internal override Command Build(ref ClientBuildInfo buildInfo)
#endif
        {
            UnmanagedStream stream = Socket.OutputSerializer.Stream;
            buildInfo.IsFullSend = 1;
            if ((buildInfo.SendBufferSize - stream.Data.Pointer.CurrentIndex) >= sizeof(uint) || buildInfo.Count == 0)
            {
                var nextCommand = LinkNext;
                Socket.OutputSerializer.Stream.Data.Pointer.Write(CommandListener.ControllerMethodIndex);
                buildInfo.AddCount();
                LinkNext = null;
                return nextCommand;
            }
            return this;
        }
    }
}
