using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The status of the reqeust command added to the output queue
    /// 请求命令添加到输出队列的状态
    /// </summary>
    internal enum CommandPushStateEnum : byte
    {
        /// <summary>
        /// Successfully added to the output queue
        /// 成功添加到输出队列
        /// </summary>
        Success,
        /// <summary>
        /// Wait for the number of free outputs
        /// 等待空闲输出数量
        /// </summary>
        WaitCount,
        /// <summary>
        /// The socket has been closed
        /// 套接字已经关闭
        /// </summary>
        Closed,
        /// <summary>
        /// The socket is waiting to connect (The controller used for default initialization)
        /// 等待连接中（用于默认初始化的控制器）
        /// </summary>
        WaitConnect,
        ///// <summary>
        ///// 已添加到命令批处理
        ///// </summary>
        //Batch,
    }
}
