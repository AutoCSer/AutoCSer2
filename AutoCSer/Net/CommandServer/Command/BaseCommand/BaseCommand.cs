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
        /// 检查等待添加队列命令
        /// </summary>
        /// <returns>是否需要继续等待</returns>
        internal override bool CheckWaitPush()
        {
            AutoCSer.LogHelper.ErrorIgnoreException($"{GetType().fullName()} 不支持等待");
            return true;
        }
    }
}
