using System;
using System.Net;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// The server listens for address information
    /// 服务监听地址信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HostEndPoint
    {
        /// <summary>
        /// Server listening address
        /// 服务监听地址
        /// </summary>
        public IPEndPoint IPEndPoint;
        /// <summary>
        /// Server registration address information
        /// 服务注册地址信息
        /// </summary>
        public AutoCSer.Net.HostEndPoint RegisterHost;
        /// <summary>
        /// The server listens for address information
        /// 服务监听地址信息
        /// </summary>
        /// <param name="value"></param>
        public HostEndPoint(AutoCSer.Net.HostEndPoint value)
        {
            RegisterHost = value;
            IPEndPoint = value.IPEndPoint;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator HostEndPoint(AutoCSer.Net.HostEndPoint value)        {            return new HostEndPoint(value);        }
    }
}
