using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
#if AOT
using ClientMethodType = AutoCSer.Net.CommandServer.ClientMethod;
#else
using ClientMethodType = AutoCSer.Net.CommandServer.ClientInterfaceMethod;
#endif

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 基本命令，和控制器无关
    /// </summary>
    public abstract class BaseCommand : Command
    {
        /// <summary>
        /// Command client socket
        /// 命令客户端套接字
        /// </summary>
        internal new readonly CommandClientSocket Socket;
        /// <summary>
        /// 基本命令，和控制器无关
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="method"></param>
        internal BaseCommand(CommandClientSocket socket, ClientMethodType method) : base(method)
        {
            Socket = socket;
        }
        /// <summary>
        /// 添加命令到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Push()
        {
            Socket.PushNotCheckCount(this);
        }
        /// <summary>
        /// The command waiting for idle output attempts to be added to the output queue again
        /// 等待空闲输出的命令再次尝试添加到输出队列
        /// </summary>
        /// <returns>Is it necessary to keep waiting
        /// 是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            AutoCSer.LogHelper.ErrorIgnoreException($"{GetType().fullName()} 不支持等待");
            return true;
        }
    }
}
