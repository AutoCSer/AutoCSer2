using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// IPv4 数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Ip
    {
        /// <summary>
        /// IP标头默认字节数
        /// </summary>
        public const int DefaultHeaderSize = 20;

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
        /// IP标头字节数
        /// </summary>
        public int HeaderSize
        {
            get { return (data.Array[data.Start] & 15) << 2; }
        }
        /// <summary>
        /// IP版本号
        /// </summary>
        public int Version
        {
            get { return data.Array[data.Start] >> 4; }
        }
        /// <summary>
        /// 优先权,现在已被忽略
        /// </summary>
        public int Priority
        {
            get { return data.Array[data.Start + 1] >> 5; }
        }
        /// <summary>
        /// 服务类型TOS字段，暂时大多数TCP/IP实现都不支持TOS特性，4位中只能用1位，下面是推荐值。
        /// 最小时延：如 Telnet，Rlogin，FTP控制，TFTP，SMTP命令，DNS-UDP查询
        /// 最大吞吐量：如 FTP数据，SMTP数据，DNS区域传输
        /// 最高可靠性：如 SNMP，IGP
        /// 最小费用：如 NNTP
        /// </summary>
        public int ServiceType
        {
            get { return (data.Array[data.Start + 1] >> 1) & 15; }
        }
        /// <summary>
        /// IP数据包总字节数(大多数的链路层都会对它进行分片，主机也要求不能接收超过576B[报文512B]的数据报。事实上现在大多数的实现（特别是那些支持网络文件系统NFS的实现）允许超过8192B的IP数据报。)
        /// </summary>
        public uint PacketSize
        {
            get { return ((uint)data.Array[data.Start + 2] << 8) + data.Array[data.Start + 3]; }
        }
        /// <summary>
        /// 唯一标识主机发送的每一个数据报，通常每发一份它的值就会加1。
        /// </summary>
        public uint Identity
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 是否分片
        /// </summary>
        public bool IsFragment
        {
            get { return (data.Array[data.Start + 6] & 0x40) != 0; }
        }
        /// <summary>
        /// 更多分片标志
        /// </summary>
        public bool MoreFragment
        {
            get { return (data.Array[data.Start + 6] & 0x20) != 0; }
        }
        /// <summary>
        /// 分片偏移，该片偏移原始数据报开始处的位置
        /// </summary>
        public uint FragmentOffset
        {
            get { return (((uint)data.Array[data.Start + 6] & 0x1f) << 8) + data.Array[data.Start + 7]; }
        }
        /// <summary>
        /// 生存时间周期，一般为32或64，每经过一个路由器就减1，如果该字段为0，则该数据报被丢弃。
        /// </summary>
        public byte LifeTime
        {
            get { return data.Array[data.Start + 8]; }
        }
        /// <summary>
        /// IP协议
        /// </summary>
        public IpProtocolEnum Protocol
        {
            get { return (IpProtocolEnum)data.Array[data.Start + 9]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.Start + 10] << 8) + data.Array[data.Start + 11]; }
        }
        /// <summary>
        /// 源IP地址
        /// </summary>
        public uint Source
        {
            get { return BitConverter.ToUInt32(data.Array, data.Start + 12); }
        }
        /// <summary>
        /// 目的IP地址
        /// </summary>
        public uint Destination
        {
            get { return BitConverter.ToUInt32(data.Array, data.Start + 16); }
        }
        /// <summary>
        /// IP头扩展
        /// </summary>
        public SubArray<byte> Expand
        {
            get
            {
                return HeaderSize > DefaultHeaderSize ? new SubArray<byte>(data.Start + DefaultHeaderSize, HeaderSize - DefaultHeaderSize, data.Array) : new SubArray<byte>();
            }
        }
        /// <summary>
        /// 下层应用数据包
        /// </summary>
        public SubArray<byte> Packet
        {
            get
            {
                int headerSize = HeaderSize;
                return new SubArray<byte>(data.Start + headerSize, data.Length - headerSize, data.Array);
            }
        }
        /// <summary>
        /// IP头校验和(应用于TCP,UDP等协议)
        /// </summary>
        private uint headerCheckSum
        {
            get
            {
                uint source = Source, destination = Destination, packetSize = (uint)(PacketSize - HeaderSize);
                return (source & 0xffff) + (source >> 16) + (destination & 0xffff) + (destination >> 16)
                    + (packetSize >> 8) + (((packetSize & 0xffU) + (uint)Protocol) << 8);
            }
        }
        /// <summary>
        /// 初始化IP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Ip(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// 初始化IP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public unsafe Ip(ref SubArray<byte> data)
        {
            if (data.Length >= DefaultHeaderSize)
            {
                fixed (byte* dataFixed = data.GetFixedBuffer())
                {
                    byte* start = dataFixed + data.Start;
                    uint packetSize = ((uint)*(start + 2) << 8) + *(start + 3);
                    if (packetSize >= ((*start & 15) << 2) && packetSize <= data.Length)
                    {
                        this.data = new SubArray<byte>(data.Start, (int)packetSize, data.Array);
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
        /// <summary>
        /// 获取校验和，IP、ICMP、IGMP、TCP和UDP协议采用相同的检验和算法(对首部中每个16bit进行二进制反码求和，如果首部在传输过程中没有发生任何差错，那么接收方计算的结果应该为全1。)
        /// </summary>
        /// <param name="data">待校验数据</param>
        /// <param name="checkSum">校验和初始值,默认应为0</param>
        /// <returns>校验和</returns>
        internal unsafe static ushort CreateCheckSum(ref SubArray<byte> data, uint checkSum = 0)
        {
            fixed (byte* fixedData = data.GetFixedBuffer())
            {
                byte* start = fixedData + data.Start, end = start + data.Length - 1;
                while (start < end)
                {
                    checkSum += *((ushort*)start);
                    start += 2;
                }
                if (start == end) checkSum += *start;
            }
            checkSum = (checkSum >> 16) + (checkSum & 0xffffU);
            return (ushort)(~(checkSum + (checkSum >> 16)));
        }
    }
}
