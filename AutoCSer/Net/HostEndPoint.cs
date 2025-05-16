using AutoCSer.Extensions;
using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Information about the server host and port
    /// 服务端主机与端口信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HostEndPoint : IEquatable<HostEndPoint>
    {
        /// <summary>
        /// Server host name or IP address. If the server cannot be resolved, IPAddress.Any is used by default, such as "www.x.com" or "127.0.0.1"
        /// 服务端主机名称或者 IP 地址，无法解析时默认使用 IPAddress.Any，比如 "www.x.com" 或者 "127.0.0.1"
        /// </summary>
        public string Host;
        /// <summary>
        /// Listening port number
        /// 监听端口号
        /// </summary>
        public ushort Port;
        /// <summary>
        /// Information about the server host and port
        /// 服务端主机与端口信息
        /// </summary>
        /// <param name="port">Listening port number. 0 indicates the allocated port number obtained from the port registration service
        /// 监听端口号，0 表示从端口注册服务获取分配端口号</param>
        /// <param name="host">Server host name or IP address. If the server cannot be resolved, IPAddress.Any is used by default, such as "www.x.com" or "127.0.0.1"
        /// 服务端主机名称或者 IP 地址，无法解析时默认使用 IPAddress.Any，比如 "www.x.com" 或者 "127.0.0.1"</param>
        public HostEndPoint(ushort port, string host = "127.0.0.1")
        {
            Host = host;
            Port = port;
        }
        /// <summary>
        /// Gets the service listening address
        /// 获取服务监听地址
        /// </summary>
        public IPEndPoint IPEndPoint
        {
            get
            {
                var address = default(IPAddress);
                if (string.IsNullOrEmpty(Host) || !IPAddress.TryParse(Host, out address)) address = IPAddress.Any;
                return new IPEndPoint(address, Port);
            }
        }
        /// <summary>
        /// 根据端口号获取服务主机与端口信息
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal HostEndPoint Get(ushort port)
        {
            return new HostEndPoint(port, Host);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HostEndPoint other)
        {
            return Port == other.Port && Host == other.Host;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Port ^ Host.GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<HostEndPoint>());
        }
    }
}
