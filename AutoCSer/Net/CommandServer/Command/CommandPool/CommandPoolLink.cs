using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端命令链表节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CommandPoolLink
    {
        /// <summary>
        /// 客户端命令
        /// </summary>
#if NetStandard21
        internal Command? Command;
#else
        internal Command Command;
#endif
        /// <summary>
        /// 下一个命令序号
        /// </summary>
        internal int Next;
        /// <summary>
        /// 命令序号标识
        /// </summary>
        internal uint Identity;
        /// <summary>
        /// 设置客户端命令
        /// </summary>
        /// <param name="command">客户端命令</param>
        /// <param name="identity">命令序号标识</param>
        /// <returns>下一个命令序号</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int Set(Command command, out uint identity)
        {
            Command = command;
            identity = Identity;
            return Next;
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Command? CheckTimeout(uint timeoutSeconds, int nextIndex)
#else
        internal Command CheckTimeout(uint timeoutSeconds, int nextIndex)
#endif
        {
            if (Command?.TimeoutSeconds == timeoutSeconds)
            {
                Command command = Command;
                ++Identity;
                Next = nextIndex;
                Command = null;
                return command;
            }
            return null;
        }
        /// <summary>
        /// 获取客户端命令
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="nextIndex"></param>
        /// <param name="command"></param>
        /// <returns></returns>
#if NetStandard21
        internal CommandPoolGetStateEnum Get(uint identity, int nextIndex, out Command? command)
#else
        internal CommandPoolGetStateEnum Get(uint identity, int nextIndex, out Command command)
#endif
        {
            command = Command;
            if (Identity == identity)
            {
                if (!Command.notNull().IsKeepCallback)
                {
                    Command = null;
                    ++Identity;
                    Next = nextIndex;
                    return CommandPoolGetStateEnum.Command;
                }
                return CommandPoolGetStateEnum.KeepCallback;
            }
            return CommandPoolGetStateEnum.IdentityError;
        }
        /// <summary>
        /// Cancel the keep callback
        /// 取消保持回调
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="nextIndex"></param>
        /// <returns></returns>
#if NetStandard21
        internal Command? CancelCallback(uint identity, int nextIndex)
#else
        internal Command CancelCallback(uint identity, int nextIndex)
#endif
        {
            if (Identity == identity)
            {
                var command = Command;
                ++Identity;
                Next = nextIndex;
                Command = null;
                return command;
            }
            return null;
        }
        /// <summary>
        /// 强行释放命令节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Command? Free()
#else
        internal Command Free()
#endif
        {
            var command = Command;
            ++Identity;
            Command = null;
            return command;
        }
    }
}
