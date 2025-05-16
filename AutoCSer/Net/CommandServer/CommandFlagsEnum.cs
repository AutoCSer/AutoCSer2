using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 命令参数标志
    /// </summary>
    [Flags]
    internal enum CommandFlagsEnum : uint
    {
        /// <summary>
        /// 缺省空参数标志
        /// </summary>
        None = 0,
        ///// <summary>
        ///// 是否采用JSON序列化,否则使用二进制序列化
        ///// </summary>
        //JsonSerialize = 0x20000000,
        /// <summary>
        /// 是否发送数据
        /// </summary>
        SendData = 0x40000000,
        /// <summary>
        /// 是否需要回调
        /// </summary>
        Callback = 0x80000000,
    }
}
