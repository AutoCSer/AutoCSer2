using System;
#pragma warning disable

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 以太网会话点到点协议数据包代码
    /// </summary>
    public enum EthernetCodeEnum : byte
    {
        ActiveDiscoveryInitiation = 9,
        ActiveDiscoveryOffer = 7,
        ActiveDiscoveryTerminate = 0xA7,
        SessionStage‌ = 0
    }
}
