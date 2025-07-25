﻿using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// linuxSLL数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct LinuxSLL
    {
        /// <summary>
        /// 数据包头部长度
        /// </summary>
        public const int HeaderSize = 16;

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
        /// 数据包类型
        /// </summary>
        public LinuxTypeEnum Type
        {
            get { return (LinuxTypeEnum)(ushort)((uint)data.Array[data.Start] << 8) + data.Array[data.Start + 1]; }
        }
        /// <summary>
        /// 地址类型
        /// </summary>
        public uint AddressType
        {
            get { return ((uint)data.Array[data.Start + 2] << 8) + data.Array[data.Start + 3]; }
        }
        /// <summary>
        /// 地址长度
        /// </summary>
        public uint AddressSize
        {
            get { return ((uint)data.Array[data.Start + 4] << 8) + data.Array[data.Start + 5]; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public SubArray<byte> Address
        {
            get { return new SubArray<byte>(data.Start + 6, (int)AddressSize, data.Array); }
        }
        /// <summary>
        /// 帧类型
        /// </summary>
        public FrameEnum Frame
        {
            get
            {
                return (FrameEnum)(ushort)(((uint)data.Array[data.Start + 14] << 8) + data.Array[data.Start + 15]);
            }
        }
        /// <summary>
        /// linuxSLL数据包
        /// </summary>
        /// <param name="data">Data</param>
        public LinuxSLL(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// linuxSLL数据包
        /// </summary>
        /// <param name="data">Data</param>
        public unsafe LinuxSLL(ref SubArray<byte> data)
        {
            if (data.Length >= HeaderSize)
            {
                fixed (byte* dataFixed = data.GetFixedBuffer())
                {
                    byte* start = dataFixed + data.Start;
                    if (data.Length >= ((uint)*(start + 4) << 8) + *(start + 5) + 6)
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
