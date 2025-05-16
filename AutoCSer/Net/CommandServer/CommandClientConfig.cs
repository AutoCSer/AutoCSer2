using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using System;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client configuration
    /// 命令客户端配置
    /// </summary>
    public class CommandClientConfig : CommandServerConfigBase
    {
        /// <summary>
        /// Maximum number of bytes in the send data buffer. The default is 1MB
        /// 发送数据缓冲区最大字节数，默认为 1MB
        /// </summary>
        public int SendBufferMaxSize = 1 << 20;
        /// <summary>
        /// The maximum number of unprocessed commands, corresponding to the number of concurrent requests, defaults to 8192. Above the specified value will block the call until below the specified value to avoid excessive memory consumption
        /// 最大未处理命令数量，对应并发请求数量，默认为 8192，超过指定值将阻塞调用直到低于指定值避免占用过多内存
        /// </summary>
        public int CommandQueueCount = 1 << 13;
        /// <summary>
        /// The default interval for unidirectional heartbeat packets is 1 second. For services with stable and reliable frequencies, you can set the interval to 0 to disable heartbeat packets. The recommended value for open services is 60
        /// 单向心跳包间隔时间默认为 1 秒，对于频率稳定可靠的服务类型可以设置为 0 禁用心跳包，开放服务建议值为 60
        /// </summary>
        public ushort CheckSeconds = 1;
        /// <summary>
        /// The default value is 0, indicating that there is no timeout logic and the TimeoutSeconds timeout defined on the interface is invalid
        /// 命令调用最大超时秒数，默认为 0 表示无超时逻辑并且接口定义 TimeoutSeconds 超时无效
        /// </summary>
        public ushort CommandMaxTimeoutSeconds;
        /// <summary>
        /// The default value is 0, indicating no check. This parameter is used to check whether queued tasks are blocked or deadlocked for a long time
        /// 同步队列任务执行超时检查秒数，默认为 0 表示不检查，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 0;
        /// <summary>
        /// The default value of the release hold callback sending command is 2. The recommended value of open service is 5
        /// 释放保持回调发送命令定时秒数默认为 2，开放服务建议值为 5
        /// </summary>
        public ushort CancelKeepCallbackSeconds = 2;
        /// <summary>
        /// Number of consecutive failed authentication attempts to create a new client. The default value is 4
        /// 创建新客户端认证连续失败尝试次数，默认为 4
        /// </summary>
        public byte VerifyErrorCount = 4;
        /// <summary>
        /// Command pool initialization binary size, the maximum value is 16, the minimum value is 2, the default is 14 container size is 16384, open service recommended value is 3 Container size is 8
        /// 命令池初始化二进制大小，最大值为 16，最小值为 2，默认为 14 容器大小为 16384，开放服务建议值为 3 容器大小为 8
        /// </summary>
        public byte CommandPoolBits = 14;
        /// <summary>
        /// If the default value is true, only one command controller obtains server controller data to match client controller information. If the default value is true, no server controller data is obtained when there is only one command controller
        /// 默认为 true 表示只有 1 个命令控制器也获取服务端控制器数据用于匹配客户端控制器信息，设置为 true 表示只有 1 个命令控制器时不获取服务端控制器数据
        /// </summary>
        public bool IsServerController = true;
        /// <summary>
        /// The default value true indicates that the connection is automatically started when the client object is created, otherwise it needs to be triggered on the first call
        /// 默认为 true 表示在创建客户端对象的时候自动启动连接，否则需要第一次调用触发
        /// </summary>
        public bool IsAutoSocket = true;
        /// <summary>
        /// The default value is true to start the Nagle algorithm, and for low latency requirements it should be set to false to send data immediately
        /// 默认为 true 表示启动 Nagle 算法，对于低延时需求应该设置为 false 表示立即发送数据
        /// </summary>
        public bool NoDelay = true;
        /// <summary>
        /// Command client socket event controller property binding identification, Defaults to the current type only define attributes BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly
        /// 命令客户端套接字事件控制器属性绑定标识，默认为仅当前类型定义属性 BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly 
        /// </summary>
        public BindingFlags ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Used to override the connection logic after service registration is enabled
        /// 用于启用服务注册以后重写自动启动连接逻辑
        /// </summary>
        /// <param name="client"></param>
        public virtual void AutoCreateSocket(CommandClient client)
        {
            if (IsAutoSocket) client.AutoCreateSocket();
        }
        /// <summary>
        /// Automatic start connection
        /// 自动启动连接
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task AutoCreateSocketAsync(CommandClient client)
        {
            return client.GetSocketAsync();
        }
        /// <summary>
        /// Access the service registry client listening component, the default for new AutoCSer.Net.CommandClientServiceRegistrar(commandClient), one-time upon initial client calls
        /// 获取服务注册客户端监听组件，默认为 new AutoCSer.Net.CommandClientServiceRegistrar(commandClient)，客户端初始化时一次性调用
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public virtual Task<CommandClientServiceRegistrar> GetRegistrar(CommandClient commandClient)
        {
            return Task.FromResult(new CommandClientServiceRegistrar(commandClient));
        }
        /// <summary>
        /// Gets the command client socket event delegate
        /// 获取命令客户端套接字事件委托
        /// </summary>
#if NetStandard21
        public Func<ICommandClient, CommandClientSocketEvent>? GetSocketEventDelegate;
#else
        public Func<ICommandClient, CommandClientSocketEvent> GetSocketEventDelegate;
#endif
        /// <summary>
        /// Gets the command client socket event, which defaults to new CommandClientSocketEvent(commandClient) and is called once upon client initialization
        /// 获取命令客户端套接字事件，默认为 new CommandClientSocketEvent(commandClient)，客户端初始化时一次性调用
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public virtual CommandClientSocketEvent GetSocketEvent(CommandClient commandClient)
        {
            if (GetSocketEventDelegate != null) return GetSocketEventDelegate(commandClient);
            return new CommandClientSocketEvent(commandClient);
        }
        /// <summary>
        /// Gets the binary deserialization configuration parameter, which is called once during client initialization
        /// 获取二进制反序列化配置参数，客户端初始化时一次性调用
        /// </summary>
        /// <returns></returns>
        public virtual AutoCSer.BinarySerialize.DeserializeConfig GetBinaryDeserializeConfig()
        {
            return new AutoCSer.BinarySerialize.DeserializeConfig();
        }
#if NetStandard21
        /// <summary>
        /// Send data coding
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="data">Raw data
        /// 原始数据</param>
        /// <param name="buffer">Output data buffer
        /// 输出数据缓冲区</param>
        /// <param name="outputData">Output data
        /// 输出数据</param>
        /// <param name="outputSeek">Start position of output data
        /// 输出数据起始位置</param>
        /// <param name="outputHeadSize">The output data exceeds the header size
        /// 输出数据多余头部大小</param>
        /// <returns>Whether the sent data is encoded
        /// 发送数据是否编码</returns>
        public virtual bool TransferEncode(CommandClientSocket socket, ReadOnlySpan<byte> data, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
#else
        /// <summary>
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">命令客户端套接字</param>
        /// <param name="data">原始数据</param>
        /// <param name="dataIndex">原始数据开始位置</param>
        /// <param name="dataSize">原始数据字节数量</param>
        /// <param name="buffer">输出数据缓冲区</param>
        /// <param name="outputData">输出数据</param>
        /// <param name="outputSeek">输出数据起始位置</param>
        /// <param name="outputHeadSize">输出数据多余头部大小</param>
        /// <returns>发送数据是否编码</returns>
        public virtual bool TransferEncode(CommandClientSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
#endif
        {
            return false;
            //return AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel.Fastest);
        }
        /// <summary>
        /// Received data decoding
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">Command client socket
        /// 命令客户端套接字</param>
        /// <param name="transferData">The encoded data
        /// 编码后的数据</param>
        /// <param name="outputData">Raw data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decoding is successful
        /// 是否解码成功</returns>
        public virtual bool TransferDecode(CommandClientSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return false;
            //return AutoCSer.Common.Config.Decompress(transferData, ref outputData);
        }
        /// <summary>
        /// The custom data processing, the default return AutoCSer.Net.CommandServer.ClientReceiveErrorTypeEnum.CustomCommandError and close the current socket (attention, because it is receiving data IO thread synchronization calls, If there is a block please open a new thread task processing)
        /// 自定义数据处理，默认返回 AutoCSer.Net.CommandServer.ClientReceiveErrorTypeEnum.CustomCommandError 并关闭当前套接字（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <returns>The client receives data error type. If not Success is returned, the current socket is closed
        /// 客户端接收数据错误类型，返回非 Success 则关闭当前套接字</returns>
        public virtual ClientReceiveErrorTypeEnum OnCustomData(CommandClientSocket socket, ref SubArray<byte> data)
        {
            Log.ErrorIgnoreException(nameof(ClientReceiveErrorTypeEnum.CustomCommandError));
            return ClientReceiveErrorTypeEnum.CustomCommandError;
        }
        /// <summary>
        /// Queue task execution timeout notification
        /// 队列任务执行超时通知
        /// </summary>
        /// <param name="queue">Client execution queue
        /// 客户端执行队列</param>
        /// <param name="seconds">Current task execution seconds
        /// 当前任务执行秒数</param>
        /// <returns></returns>
        public virtual Task OnQueueTimeout(CommandClientCallQueue queue, long seconds) { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 默认空命令客户端配置
        /// </summary>
        internal static readonly CommandClientConfig Null = new CommandClientConfig();
    }
    /// <summary>
    /// Command client configuration
    /// 命令客户端配置
    /// </summary>
    /// <typeparam name="T">Primary controller interface type
    /// 主控制器接口类型</typeparam>
    public class CommandClientConfig<T> : AutoCSer.Net.CommandClientConfig
        where T : class
    {
        /// <summary>
        /// Symmetric interface definition
        /// 是否对称接口定义
        /// </summary>
        public bool IsSymmetryInterface;
        /// <summary>
        /// Get command client socket event (one-time call at initialization)
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new CommandClientSocketEvent<T>(client, IsSymmetryInterface);
        }
        /// <summary>
        /// Create the interface symmetry command client
        /// 创建接口对称命令客户端
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandClient<T> CreateSymmetryClient()
        {
            IsSymmetryInterface = true;
            return new CommandClient<T>(this);
        }
    }
}
