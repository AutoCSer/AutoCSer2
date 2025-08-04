using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 获取远程元数据命令
    /// </summary>
    internal sealed class RemoteMetadataCommand : BaseCommand
    {
        /// <summary>
        /// 获取远程元数据命令
        /// </summary>
        /// <param name="socket"></param>
        internal RemoteMetadataCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
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
            if (Socket.OutputSerializer.Stream.Write(CommandListener.RemoteMetadataMethodIndex))
            {
                var nextCommand = LinkNext;
                buildInfo.AddCount();
                LinkNext = null;
                return nextCommand;
            }
            buildInfo.IsFullSend = 1;
            return this;
        }
    }
}
