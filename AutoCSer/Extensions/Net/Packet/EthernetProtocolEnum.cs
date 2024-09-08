using System;
#pragma warning disable

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 点到点协议
    /// </summary>
    internal enum EthernetProtocolEnum : byte
    {
        IPv4 = 0x21,
        IPv6 = 0x57,
        Padding = 1
    }
}
