using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令添加状态
    /// </summary>
    internal enum CommandPushStateEnum : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 需要等待队列数量
        /// </summary>
        WaitCount,
        /// <summary>
        /// 套接字已经关闭
        /// </summary>
        Closed,
        ///// <summary>
        ///// 已添加到命令批处理
        ///// </summary>
        //Batch,
    }
}
