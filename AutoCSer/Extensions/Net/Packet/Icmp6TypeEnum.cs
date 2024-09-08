using System;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP V6类型
    /// </summary>
    public enum Icmp6TypeEnum : byte
    {
        /// <summary>
        /// 目的不可达
        /// </summary>
        Unreachable = 1,
        /// <summary>
        /// 分组报文太长
        /// </summary>
        Overflow = 2,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 3,
        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 4,

        /// <summary>
        /// 回显(ping)请求
        /// </summary>
        EchoRequest = 128,
        /// <summary>
        /// 回显(Ping)应答
        /// </summary>
        EchoAnswer = 129,
        /// <summary>
        /// 路由器请求
        /// </summary>
        RouterRequest = 133,
        /// <summary>
        /// 路由器通告
        /// </summary>
        RouterAdvertisement = 134,
        /// <summary>
        /// 邻居请求
        /// </summary>
        NeighborRequest = 135,
        /// <summary>
        /// 邻居通告
        /// </summary>
        NeighborAdvertisement = 136,
        /// <summary>
        /// 重定向
        /// </summary>
        Redirect = 137,
    }
}
