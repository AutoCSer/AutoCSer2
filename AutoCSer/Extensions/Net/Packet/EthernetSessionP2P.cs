using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 以太网会话点到点协议数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct EthernetSessionP2P
    {
        /// <summary>
        /// 数据包头长度
        /// </summary>
        public const int HeaderSize = 8;

        /// <summary>
        /// 数据
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 数据包是否有效
        /// </summary>
        public bool IsPacket
        {
            get { return data.Array != null; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int Version
        {
            get { return (data.Array[data.Start] >> 4) & 240; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type
        {
            get { return data.Array[data.Start] & 15; }
        }
        /// <summary>
        /// 代码类型
        /// </summary>
        public EthernetCodeEnum Code
        {
            get { return (EthernetCodeEnum)data.Array[data.Start + 1]; }
        }
        /// <summary>
        /// 标识
        /// </summary>
        public ushort SessionId
        {
            get { return BitConverter.ToUInt16(data.Array, data.Start + 2); }
        }
        /// <summary>
        /// 数据包长度(单位未知)
        /// </summary>
        public uint packetSize
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 帧类型
        /// </summary>
        public FrameEnum Frame
        {
            get
            {
                if (data.Array[data.Start + 6] == 0)
                {
                    switch (data.Array[data.Start + 7])
                    {
                        case (byte)EthernetProtocolEnum.IPv4:
                            return FrameEnum.IpV4;
                        case (byte)EthernetProtocolEnum.IPv6:
                            return FrameEnum.IpV6;
                    }
                }
                return FrameEnum.None;
            }
        }
        /// <summary>
        /// 以太网会话点到点协议数据包
        /// </summary>
        /// <param name="data">Data</param>
        public EthernetSessionP2P(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// 以太网会话点到点协议数据包
        /// </summary>
        /// <param name="data">Data</param>
        public unsafe EthernetSessionP2P(ref SubArray<byte> data)
        {
            if (data.Length >= HeaderSize)
            {
                fixed (byte* dataFixed = data.GetFixedBuffer())
                {
                    byte* start = dataFixed + data.Start;
                    uint packetSize = ((uint)*(start + 4) << 8) + *(start + 5);
                    if (data.Length >= packetSize)
                    {
                        this.data = data;
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
    }
}
