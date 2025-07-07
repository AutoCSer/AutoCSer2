using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 心跳检测命令
    /// </summary>
    internal sealed class CheckCommand : BaseCommand
    {
        /// <summary>
        /// 心跳检测命令
        /// </summary>
        /// <param name="socket"></param>
        internal CheckCommand(CommandClientSocket socket) : base(socket, KeepCallbackCommand.KeepCallbackMethod) { }
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
            if (buildInfo.Count == 0 && LinkNext == null)
            {
                Socket.OutputSerializer.Stream.Data.Pointer.Write(CommandListener.CheckMethodIndex);
                buildInfo.AddCount();
                return null;
            }
            var nextCommand = LinkNext;
            ++buildInfo.FreeCount;
            LinkNext = null;
            return nextCommand;
        }
    }
}
