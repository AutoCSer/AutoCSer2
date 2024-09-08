using System;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP类型相关代码
    /// </summary>
    public enum IcmpCodeEnum : byte
    {
        /// <summary>
        /// 默认空值
        /// </summary>
        None = 0,
        /// <summary>
        /// 网络不可达
        /// </summary>
        Unreachable_Network = 0,
        /// <summary>
        /// 主机不可达
        /// </summary>
        Unreachable_Host = 1,
        /// <summary>
        /// 协议不可达
        /// </summary>
        Unreachable_Protocol = 2,
        /// <summary>
        /// 端口不可达
        /// </summary>
        Unreachable_Port = 3,
        /// <summary>
        /// 需要进行分片但设置了不分片比特
        /// </summary>
        Unreachable_Fragment = 4,
        /// <summary>
        /// 源站选路失败
        /// </summary>
        Unreachable_Routing = 5,
        /// <summary>
        /// 目的网络不认识
        /// </summary>
        Unreachable_NetworkUnknow = 6,
        /// <summary>
        /// 目的主机不认识
        /// </summary>
        Unreachable_HostUnknow = 7,
        /// <summary>
        /// 源主机被隔离(已作废)
        /// </summary>
        Unreachable_Isolated = 8,
        /// <summary>
        /// 目的网络被强制禁止
        /// </summary>
        Unreachable_NetworkDisable = 9,
        /// <summary>
        /// 目的主机被强制禁止
        /// </summary>
        Unreachable_HostDisable = 10,
        /// <summary>
        /// 由于服务类型TOS，网络不可达
        /// </summary>
        Unreachable_NetworkTOS = 11,
        /// <summary>
        /// 由于服务类型TOS，主机不可达
        /// </summary>
        Unreachable_HostTOS = 12,
        /// <summary>
        /// 由于过滤，通信被强制禁止
        /// </summary>
        Unreachable_Filter = 13,
        /// <summary>
        /// 主机越权
        /// </summary>
        Unreachable_UltraVires = 14,
        /// <summary>
        /// 优先权中止生效
        /// </summary>
        Unreachable_Priority = 15,

        /// <summary>
        /// 对网络重定向
        /// </summary>
        Redirect_Network = 0,
        /// <summary>
        /// 对主机重定向
        /// </summary>
        Redirect_Host = 1,
        /// <summary>
        /// 对服务类型和网络重定向
        /// </summary>
        Redirect_NetworkTOS = 2,
        /// <summary>
        /// 对服务类型和主机重定向
        /// </summary>
        Redirect_HostTOS = 3,

        /// <summary>
        /// 传输期间生存时间为0
        /// </summary>
        Timeout_Transmission = 0,
        /// <summary>
        /// 在数据报组装期间生存时间为0
        /// </summary>
        Timeout_Assembly = 1,

        /// <summary>
        /// 坏的IP首部（包括各种差错）
        /// </summary>
        ParameterError_IpHeader = 0,
        /// <summary>
        /// 缺少必需的选项
        /// </summary>
        ParameterError_Options = 1,
    }
}
