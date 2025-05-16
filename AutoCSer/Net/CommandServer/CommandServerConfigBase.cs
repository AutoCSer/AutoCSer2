using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command service configuration
    /// 命令服务配置
    /// </summary>
    public abstract class CommandServerConfigBase
    {
        /// <summary>
        /// The service name is a unique identifier of the service registration. If the service registration is not required, it is only used for log output
        /// 服务名称，服务注册唯一标识，没有用到服务注册的时候仅用于日志输出
        /// </summary>
#if NetStandard21
        public string? ServerName;
#else
        public string ServerName;
#endif
        ///// <summary>
        ///// 注册服务主机与端口信息
        ///// </summary>
        //public HostEndPoint RegistryHost;
        /// <summary>
        /// The service listens to host and port information
        /// 服务监听主机与端口信息
        /// </summary>
        public HostEndPoint Host;
        /// <summary>
        /// Received data cache pool Byte size Number of binary bits. The default value is 17. The value is 128KB. The recommended value for open services is no more than 12 to avoid excessive memory usage
        /// 接收数据缓存区池字节大小二进制位数量，默认为 17 为 128KB，开放服务建议值不大于 12 避免内存占用过多
        /// </summary>
        public BufferSizeBitsEnum ReceiveBufferSizeBits = BufferSizeBitsEnum.Kilobyte128;
        /// <summary>
        /// Send data cache pool Byte size Number of binary bits. The default value is 17. The value is 128KB. The recommended value for open services is no more than 12 to avoid excessive memory usage
        /// 发送数据缓存区池字节大小二进制位数量，默认为 17 为 128KB，开放服务建议值不大于 12 避免内存占用过多
        /// </summary>
        public BufferSizeBitsEnum SendBufferSizeBits = BufferSizeBitsEnum.Kilobyte128;
        /// <summary>
        /// 默认为 true 表示字符串二进制序列化直接复制内存数据，设置为 false 则对 ASCII 进行编码可以降低空间占用
        /// </summary>
        public bool IsSerializeCopyString = true;
        /// <summary>
        /// The maximum number of bytes of input data, the default is 0 to indicate no limit, the open service recommended value is less than 2^ ReceiveBufferSizeBis-12 and the API is not recommended to produce large object transport behavior
        /// 最大输入数据字节数，默认为 0 表示不限制，开放服务建议值小于 2^ReceiveBufferSizeBis - 12 而且不建议 API 产生大对象传输行为
        /// </summary>
        public int MaxInputSize;// = (16 << 10) - (sizeof(uint) + sizeof(int) * 2);

        /// <summary>
        /// Log processing instance
        /// 日志处理实例
        /// </summary>
        public virtual ILog Log { get { return LogHelper.Default; } }
        /// <summary>
        /// Return a log processing instance if any level is supported
        /// 如果支持指定任意级别则返回日志处理实例
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public ILog? GetAnyLevelLog(LogLevelEnum logLevel)
#else
        public ILog GetAnyLevelLog(LogLevelEnum logLevel)
#endif
        {
            ILog log = Log;
            return log.IsAnyLevel(logLevel) ? log : null;
        }
        /// <summary>
        /// The controller constructs a warning message
        /// 控制器构造警告信息
        /// </summary>
        /// <param name="controllerType">Controller interface type
        /// 控制器接口类型</param>
        /// <param name="messages">Construct warning message
        /// 构造警告信息</param>
        public virtual void OnControllerConstructorMessage(Type controllerType, string[] messages)
        {
            Log.DebugIgnoreException($"{controllerType.fullName()} 控制器生成警告\r\n{string.Join("\r\n", messages)}");
        }

        /// <summary>
        /// 默认空套接字
        /// </summary>
        internal static readonly Socket NullSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// 默认空套接字事件
        /// </summary>
        internal static readonly SocketAsyncEventArgs NullSocketAsyncEventArgs = new SocketAsyncEventArgs();
        /// <summary>
        /// 默认空二进制序列化
        /// </summary>
        internal static readonly BinarySerializer NullBinarySerializer = new BinarySerializer();
        /// <summary>
        /// 默认空监听地址
        /// </summary>
        internal static readonly IPEndPoint NullIPEndPoint = new IPEndPoint(0, 0);
    }
}
