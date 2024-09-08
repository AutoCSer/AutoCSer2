using System;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// IP 协议
    /// </summary>
    public enum IpProtocolEnum : byte//System.Net.Sockets.ProtocolType
    {
        ///// <summary>
        ///// 未知
        ///// </summary>
        //Unknown = -1,
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// IP协议
        /// </summary>
        IP = 0,
        /// <summary>
        /// IPv6逐跳选项扩展
        /// </summary>
        IPv6HopByHopOptions = 0,
        /// <summary>
        /// 互联网控制报文协议
        /// </summary>
        Icmp = 1,
        /// <summary>
        /// 互联网组管理协议
        /// </summary>
        Igmp = 2,
        /// <summary>
        /// 网关到网关协议
        /// </summary>
        Ggp = 3,
        /// <summary>
        /// IPv4
        /// </summary>
        IPv4 = 4,
        /// <summary>
        /// 传输控制协议
        /// </summary>
        Tcp = 6,
        /// <summary>
        /// 外部网关协议
        /// </summary>
        Egp = 8,
        /// <summary>
        /// PUP协议
        /// </summary>
        Pup = 12,
        /// <summary>
        /// 用户数据报协议
        /// </summary>
        Udp = 17,
        /// <summary>
        /// XNS IDP 协议
        /// </summary>
        Hmp = 20,
        /// <summary>
        /// 互联网数据报协议
        /// </summary>
        Idp = 22,
        /// <summary>
        /// 
        /// </summary>
        Rdp = 27,
        /// <summary>
        /// SO传输入协议
        /// </summary>
        TP = 29,
        /// <summary>
        /// IPv6头部
        /// </summary>
        IPv6 = 41,
        /// <summary>
        /// IPv6选路选项扩展
        /// </summary>
        IPv6RoutingHeader = 43,
        /// <summary>
        /// IPv6分片选项扩展
        /// </summary>
        IPv6FragmentHeader = 44,
        /// <summary>
        /// 资源预留协议
        /// </summary>
        Rsvp = 46,
        /// <summary>
        /// 通用路由封装
        /// </summary>
        Gre = 47,
        /// <summary>
        /// IPv6封装安全性选项扩展
        /// </summary>
        IPSecEncapsulatingSecurityPayload = 50,
        /// <summary>
        /// IPv6鉴别首部扩展
        /// </summary>
        IPSecAuthenticationHeader = 51,
        /// <summary>
        /// 互联网控制报文协议(IPv6)
        /// </summary>
        IcmpV6 = 58,
        /// <summary>
        /// IPv6无下一首部
        /// </summary>
        IPv6NoNextHeader = 59,
        /// <summary>
        /// IPv6目的地选项扩展
        /// </summary>
        IPv6DestinationOptions = 60,
        /// <summary>
        /// 
        /// </summary>
        Rcd = 66,
        /// <summary>
        /// 网络硬盘协议(非官方)
        /// </summary>
        ND = 77,
        /// <summary>
        /// 多播传输协议
        /// </summary>
        Mtp = 92,
        /// <summary>
        /// 封装头
        /// </summary>
        Encap = 98,
        /// <summary>
        /// IPv6协议独立组播
        /// </summary>
        Pim = 103,
        /// <summary>
        /// 压缩头部协议
        /// </summary>
        Comp = 108,
        /// <summary>
        /// 原始IP数据包
        /// </summary>
        Raw = 255,
        ///// <summary>
        ///// 互联网数据包交换协议
        ///// </summary>
        //Ipx = 1000,
        ///// <summary>
        ///// 顺序包交换协议
        ///// </summary>
        //Spx = 1256,
        ///// <summary>
        ///// 顺序包交换协议版本2
        ///// </summary>
        //SpxII = 1257,
    }
}
