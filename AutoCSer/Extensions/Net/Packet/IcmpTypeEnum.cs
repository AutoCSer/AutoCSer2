using System;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP类型
    /// </summary>
    public enum IcmpTypeEnum : byte
    {
        /// <summary>
        /// 回显(Ping)应答
        /// </summary>
        EchoAnswer = 0,
        /// <summary>
        /// 目的不可达
        /// </summary>
        Unreachable = 3,
        /// <summary>
        /// 源端被关闭
        /// </summary>
        SourceClosed = 4,
        /// <summary>
        /// 重定向
        /// </summary>
        Redirect = 5,
        /// <summary>
        /// 回显(Ping)请求
        /// </summary>
        EchoRequest = 8,
        /// <summary>
        /// 路由器通告
        /// </summary>
        RouterAdvertisement = 9,
        /// <summary>
        /// 路由器请求
        /// </summary>
        RouterRequest = 10,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 11,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 12,
        /// <summary>
        /// 时间戳请求
        /// </summary>
        TimeRequest = 13,
        /// <summary>
        /// 时间戳应答
        /// </summary>
        TimeAnswer = 14,
        /// <summary>
        /// 信息请求(已作废)
        /// </summary>
        InformationRequest = 15,
        /// <summary>
        /// 信息应答(已作废)
        /// </summary>
        InformationAnswer = 16,
        /// <summary>
        /// 地址掩码请求
        /// </summary>
        MaskRequest = 17,
        /// <summary>
        /// 地址掩码应答
        /// </summary>
        MaskAnswer = 18,
    }
}
