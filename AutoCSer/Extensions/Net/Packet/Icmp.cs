using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Icmp
    {
        /// <summary>
        /// ICMP类型对应数组长度
        /// </summary>
        private const IcmpTypeEnum maxType = IcmpTypeEnum.MaskAnswer;
        /// <summary>
        /// ICMP类型对应数据包最小长度
        /// </summary>
        private static readonly byte[] minTypeSize;

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
        public IcmpTypeEnum Type
        {
            get { return (IcmpTypeEnum)data.Array[data.Start]; }
        }
        /// <summary>
        /// 代码
        /// </summary>
        public IcmpCodeEnum Code
        {
            get { return (IcmpCodeEnum)data.Array[data.Start + 1]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.Start + 2] << 8) + data.Array[data.Start + 3]; }
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
        /// 掩码通讯标识
        /// </summary>
        public uint MaskIdentity
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 掩码序列号
        /// </summary>
        public uint MaskSequence
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
        }
        /// <summary>
        /// 掩码地址
        /// </summary>
        public uint MaskAddress
        {
            get { return BitConverter.ToUInt32(data.Array, data.Start + 8); }
        }

        /// <summary>
        /// 时间戳通讯标识
        /// </summary>
        public uint TimeIdentity
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 时间戳序列号
        /// </summary>
        public uint TimeSequence
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
        }
        /// <summary>
        /// 时间戳请求时间，请求端填写发起时间戳，然后发送报文。(返回的是自午夜开始记算的毫秒数)
        /// </summary>
        public uint TimeRequest
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 8); }
        }
        /// <summary>
        /// 时间戳接收时间，应答系统收到报文填写接收时间戳。
        /// </summary>
        public uint TimeReceive
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 12); }
        }
        /// <summary>
        /// 时间戳发送时间，发送应答时填写发送时间戳。
        /// </summary>
        public uint TimeSend
        {
            get { return AutoCSer.Extensions.Memory.Common.GetUIntBigEndian(data.Array, data.Start + 16); }
        }

        /// <summary>
        /// 下一站网络的MTU
        /// </summary>
        public uint UnreachableMTU
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
        }

        /// <summary>
        /// 重定向应该使用的路由器IP地址
        /// </summary>
        public uint RedirectRouterAddress
        {
            get { return BitConverter.ToUInt32(data.Array, data.Start + 4); }
        }

        /// <summary>
        /// 路由器通告地址数
        /// </summary>
        public byte RouterAdvertisementCount
        {
            get { return data.Array[data.Start + 4]; }
        }
        /// <summary>
        /// 路由器通告地址项长度
        /// </summary>
        public byte RouterAdvertisementAddressLength
        {
            get { return data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 路由器通告生存时间
        /// </summary>
        public uint RouterAdvertisementLifeTime
        {
            get { return ((uint)data.Array[data.Start + 6] << 8) + data.Array[data.Start + 7]; }
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
        /*
         ICMP数据包扩展。整个I P首部最长只能包括15个32bit长的字（即60个字节）。由于IP首部固定长度为20字节，RR选项用去3个字节，这样只剩下37个字节来存放IP地址清单，也就是说只能存放9个IP地址。
         RR选项code值为7。len是RR选项总字节长度，最大39。ptr是一个基于1的指针，指向存放下一个IP地址的位置，它的最小值为4+4x。
         IP时间戳选code值为0x44。len一般是36或40。prt指向下一个时间，最小为5+4x。ooooabxd，oooo为溢出字段，a表示只记录时间(4x)，b表示记录时间与IP地址(8x)，d表示发送端初始化时间与IP地址(8x，只有路由IP匹配时才记录时间戳)。
         源站选路(宽松code值为0x83，严格code值为0x89)，len同RR，ptr同RR。
         如果在IP首部中的选项字段中有多个选项，在开始下一个选项之前必须填入空白字符，另外还可以用另一个值为1的特殊字符NOP。
        */
        /// <summary>
        /// ICMP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// ICMP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp(ref SubArray<byte> data)
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
        static Icmp()
        {
            #region 初始化 ICMP类型对应数据包最小长度
            minTypeSize = new byte[(byte)maxType + 1];
            minTypeSize[(int)IcmpTypeEnum.EchoAnswer] = 8;
            minTypeSize[(int)IcmpTypeEnum.Unreachable] = 8;
            minTypeSize[(int)IcmpTypeEnum.SourceClosed] = 8;
            minTypeSize[(int)IcmpTypeEnum.Redirect] = 8;
            minTypeSize[(int)IcmpTypeEnum.EchoRequest] = 8;
            minTypeSize[(int)IcmpTypeEnum.RouterAdvertisement] = 8;
            minTypeSize[(int)IcmpTypeEnum.RouterRequest] = 8;
            minTypeSize[(int)IcmpTypeEnum.Timeout] = 8;
            //minTypeSize[(int)IcmpTypeEnum.ParameterError] = ;
            minTypeSize[(int)IcmpTypeEnum.TimeRequest] = 20;
            minTypeSize[(int)IcmpTypeEnum.TimeAnswer] = 20;
            minTypeSize[(int)IcmpTypeEnum.MaskRequest] = 12;
            minTypeSize[(int)IcmpTypeEnum.MaskAnswer] = 12;
            #endregion
        }
    }
}
