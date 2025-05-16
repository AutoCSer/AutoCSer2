using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 客户端获取状态
    /// </summary>
    internal enum CommandPoolGetStateEnum : byte
    {
        /// <summary>
        /// 普通命令
        /// </summary>
        Command,
        /// <summary>
        /// 保持回调命令
        /// </summary>
        KeepCallback,
        /// <summary>
        /// 命令序号标识不匹配
        /// </summary>
        IdentityError,
    }
}
