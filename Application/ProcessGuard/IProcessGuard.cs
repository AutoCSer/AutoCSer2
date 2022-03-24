using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护服务端接口（服务端需要以管理员身份运行）
    /// </summary>
    public interface IProcessGuard
    {
        /// <summary>
        /// 添加待守护进程
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="processInfo">进程信息</param>
        /// <returns>是否添加成功</returns>
        bool Guard(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, ProcessGuardInfo processInfo);
        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="processId">进程标识</param>
        /// <param name="processName">进程名称</param>
        void Remove(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int processId, string processName);
    }
}
