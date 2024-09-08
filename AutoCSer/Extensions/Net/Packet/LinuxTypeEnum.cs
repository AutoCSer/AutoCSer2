using System;
#pragma warning disable

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// linuxSLL数据包类型
    /// </summary>
    public enum LinuxTypeEnum : ushort
    {
        PacketSentToUs,
        PacketBroadCast,
        PacketMulticast,
        PacketSentToSomeoneElse,
        PacketSentByUs
    }
}
