using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口标识
    /// </summary>
    [AutoCSer.Net.CommandServer.IgnoreInitobjParameter]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PortIdentity
    {
        /// <summary>
        /// 服务启动时钟周期
        /// </summary>
        public long Ticks;
        /// <summary>
        /// 标识
        /// </summary>
        public uint Identity;
        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port;

        /// <summary>
        /// 设置端口标识
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="port"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Set(uint identity, int port)
        {
            Ticks = AutoCSer.Date.StartTime.Ticks;
            Identity = identity;
            Port = (ushort)port;
        }
    }
}
