using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 回调参数标志
    /// </summary>
    [Flags]
    internal enum CallbackFlagsEnum : uint
    {
        /// <summary>
        /// 缺省空参数标志
        /// </summary>
        None = 0,
        /// <summary>
        /// 是否二阶段回调的第一阶段回调
        /// </summary>
        TwoStageCallback = 0x20000000,
        /// <summary>
        /// 是否发送数据
        /// </summary>
        SendData = 0x40000000,
        /// <summary>
        /// 是否错误
        /// </summary>
        Error = 0x80000000,
    }
}
