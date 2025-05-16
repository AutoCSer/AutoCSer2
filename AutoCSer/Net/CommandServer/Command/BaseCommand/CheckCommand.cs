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
        /// 创建命令输入数据
        /// </summary>
        /// <param name="buildInfo">TCP 客户端创建命令参数</param>
        /// <returns>是否成功</returns>
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
            ++buildInfo.FreeCount;
            return LinkNext;
        }
    }
}
