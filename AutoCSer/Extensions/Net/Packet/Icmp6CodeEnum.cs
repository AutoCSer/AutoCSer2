using System;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP V6类型相关代码
    /// </summary>
    public enum Icmp6CodeEnum : byte
    {
        /// <summary>
        /// 无路由目的不可达
        /// </summary>
        Unreachable_Routing = 0,
        /// <summary>
        /// 与管理受禁的目的地通信
        /// </summary>
        Unreachable_Disable = 1,
        /// <summary>
        /// 超出了源地址的范围
        /// </summary>
        Unreachable_Range = 2,
        /// <summary>
        /// 地址不可达
        /// </summary>
        Unreachable_Address = 3,
        /// <summary>
        /// 端口不可达
        /// </summary>
        Unreachable_Port = 4,

        /// <summary>
        /// 传输过程超过了跳数限制
        /// </summary>
        Timeout_Hops = 0,
        /// <summary>
        /// 分装重组时间到期
        /// </summary>
        Timeout_Assembly = 1,

        /// <summary>
        /// 错误的首部字段
        /// </summary>
        ParameterError_Header = 0,
        /// <summary>
        /// 无法识别的首部类型
        /// </summary>
        ParameterError_Type = 1,
        /// <summary>
        /// 无法识别的IPv6选项
        /// </summary>
        ParameterError_Option = 2,
    }
}
