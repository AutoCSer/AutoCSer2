using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP V6数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Icmp6
    {
        /// <summary>
        /// ICMP类型对应数据包最小长度
        /// </summary>
        private static readonly byte[] minTypeSize;
        /// <summary>
        /// ICMP类型对应数组长度
        /// </summary>
        private const Icmp6TypeEnum maxType = Icmp6TypeEnum.Redirect;

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
        /// ICMP类型
        /// </summary>
        public Icmp6TypeEnum Type
        {
            get { return (Icmp6TypeEnum)data.Array[data.Start]; }
        }
        /// <summary>
        /// 代码
        /// </summary>
        public Icmp6CodeEnum Code
        {
            get { return (Icmp6CodeEnum)data.Array[data.Start + 1]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.Start + 2] << 8) + data.Array[data.Start + 3]; }
        }

        /// <summary>
        /// 分组报文太长 发生差错的物理链路的MTU
        /// </summary>
        public uint OverflowMTU
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 4); }
        }
        /// <summary>
        /// 参数错误相对于原始数据的位置
        /// </summary>
        public uint ParameterErrorIndex
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 4); }
        }

        /// <summary>
        /// 回显(ping)通讯标识
        /// </summary>
        public uint EchoIdentity
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 回显(ping)序列号
        /// </summary>
        public uint EchoSequence
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
        }

        /// <summary>
        /// 路由器通告 跳数限制
        /// </summary>
        public byte RouterAdvertisementHops
        {
            get { return data.Array[data.Start + 4]; }
        }
        /// <summary>
        /// 路由器通告 受管理的地址配置标志(如果主机启用动态主机配置协议DHCP)
        /// </summary>
        public bool RouterAdvertisementManaged
        {
            get { return (data.Array[data.Start + 5] & 0x80) != 0; }
        }
        /// <summary>
        /// 路由器通告 其它有状态的配置标志
        /// </summary>
        public bool RouterAdvertisementStateful
        {
            get { return (data.Array[data.Start + 5] & 0x40) != 0; }
        }
        /// <summary>
        /// 路由器通告 路由器寿命
        /// </summary>
        public uint RouterAdvertisementLife
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
        }
        /// <summary>
        /// 路由器通告 可达时间
        /// </summary>
        public uint RouterAdvertisementTime
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 8); }
        }
        /// <summary>
        /// 路由器通告 重传定时器
        /// </summary>
        public uint RouterAdvertisementTimer
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 12); }
        }

        /// <summary>
        /// 邻居请求 目标地址
        /// </summary>
        public SubArray<byte> NeighborRequestDestination
        {
            get { return new SubArray<byte>(data.Start + 8, 16, data.Array); }
        }

        /// <summary>
        /// 邻居通告 路由器标志
        /// </summary>
        public bool NeighborAdvertisementRouter
        {
            get { return (data.Array[data.Start + 4] & 0x80) != 0; }
        }
        /// <summary>
        /// 邻居通告 响应请求标志
        /// </summary>
        public bool NeighborAdvertisementAnswer
        {
            get { return (data.Array[data.Start + 4] & 0x40) != 0; }
        }
        /// <summary>
        /// 邻居通告 覆盖缓存标志
        /// </summary>
        public bool NeighborAdvertisementOver
        {
            get { return (data.Array[data.Start + 4] & 0x20) != 0; }
        }
        /// <summary>
        /// 邻居通告 目标地址
        /// </summary>
        public SubArray<byte> NeighborAdvertisementDestination
        {
            get { return new SubArray<byte>(data.Start + 8, 16, data.Array); }
        }

        /// <summary>
        /// 重定向 路由器地址
        /// </summary>
        public SubArray<byte> RedirectRouter
        {
            get { return new SubArray<byte>(data.Start + 8, 16, data.Array); }
        }
        /// <summary>
        /// 重定向 目的地址
        /// </summary>
        public SubArray<byte> RedirectDestination
        {
            get { return new SubArray<byte>(data.Start + 24, 16, data.Array); }
        }

        /// <summary>
        /// ICMP数据包扩展
        /// </summary>
        public SubArray<byte> Expand
        {
            get
            {
                int minSize = minTypeSize[data.Array[data.Start]];
                return data.Length > minSize ? new SubArray<byte>(data.Start + minSize, data.Length - minSize, data.Array) : default(SubArray<byte>);
            }
        }
        /// <summary>
        /// ICMP V6数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp6(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// ICMP V6数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp6(ref SubArray<byte> data)
        {
            if (data.Length >= 8)
            {
                byte type = data.Array[data.Start];
                if (type <= (byte)maxType)
                {
                    int minSize = minTypeSize[type];
                    if (minSize != 0 && data.Length >= minSize)
                    {
                        this.data = data;
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
        static Icmp6()
        {
            #region 初始化 ICMP类型对应数据包最小长度
            minTypeSize = new byte[(byte)maxType + 1];
            minTypeSize[(int)Icmp6TypeEnum.Unreachable] = 8;
            minTypeSize[(int)Icmp6TypeEnum.Overflow] = 8;
            minTypeSize[(int)Icmp6TypeEnum.Timeout] = 8;
            minTypeSize[(int)Icmp6TypeEnum.ParameterError] = 8;
            minTypeSize[(int)Icmp6TypeEnum.EchoRequest] = 8;
            minTypeSize[(int)Icmp6TypeEnum.EchoAnswer] = 8;
            minTypeSize[(int)Icmp6TypeEnum.RouterRequest] = 8;
            minTypeSize[(int)Icmp6TypeEnum.RouterAdvertisement] = 16;
            minTypeSize[(int)Icmp6TypeEnum.NeighborRequest] = 24;
            minTypeSize[(int)Icmp6TypeEnum.NeighborAdvertisement] = 24;
            minTypeSize[(int)Icmp6TypeEnum.Redirect] = 40;
            #endregion
        }
    }
}
