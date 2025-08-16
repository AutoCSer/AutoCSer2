using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// Configure the command server
    /// 命令服务端配置
    /// </summary>
    public class CommandServerConfig : CommandServerConfigBase
    {
#if DEBUG
        /// <summary>
        /// The default value is 5 seconds. This parameter is used to check whether queued tasks are blocked or deadlocked for a long time
        /// 同步队列任务执行超时检查秒数默认为 5 ，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 5;
#else
        /// <summary>
        /// The default value is 0, indicating no check. This parameter is used to check whether queued tasks are blocked or deadlocked for a long time
        /// 同步队列任务执行超时检查秒数，默认为 0 表示不检查，用于检查队列任务是否存在长时间阻塞或者死锁问题
        /// </summary>
        public ushort QueueTimeoutSeconds = 0;
#endif
        /// <summary>
        /// The maximum concurrent number of the default read/write queue is set to the number of CPU logical processors minus 1 by default
        /// 默认读写队列最大并发数量，默认为 CPU 逻辑处理器数量 - 1
        /// </summary>
        public int MaxReadWriteQueueConcurrency = AutoCSer.Common.ProcessorCount - 1;

#if !AOT
        /// <summary>
        /// Command server socket User-defined session object operation interface
        /// 命令服务套接字自定义会话对象操作接口
        /// </summary>
#if NetStandard21
        public ICommandListenerSession? SessionObject;
#else
        public ICommandListenerSession SessionObject;
#endif
        /// <summary>
        /// Number of socket asynchronous event object caches. The default value is 256. The recommended value for open services is greater than 1024
        /// 套接字异步事件对象缓存数量，默认为 256，开放服务建议值大于 1024
        /// </summary>
        public int SocketAsyncEventArgsMaxCount = 256;
        /// <summary>
        /// Maximum number of authentication data bytes. The default value is 256 bytes
        /// 最大认证数据字节数量，默认为 256 字节
        /// </summary>
        public int MaxVerifyDataSize = 1 << 8;
        /// <summary>
        /// The default value is 0, indicating no limit. The recommended value for open services is not greater than 1MB
        /// 发送数据缓冲区最大字节数，默认为 0 表示不限制，开放服务建议值不大于 1MB
        /// </summary>
        public int SendBufferMaxSize;
        /// <summary>
        /// The maximum length of the binary deserialized array. The default value is 0, indicating no limit. The open service is recommended to prevent memory occupation attacks according to the actual situation
        /// 二进制反序列化数组最大长度，默认为 0 表示不限制，开放服务建议根据实际情况设置防止内存占用攻击，大数组建议拆分循环调用发送或者保持回调模式接收
        /// </summary>
        public int BinaryDeserializeMaxArraySize;
        /// <summary>
        /// Authentication timeout seconds. The default value is 4
        /// 认证超时秒数，默认为 4
        /// </summary>
        public ushort VerifyTimeoutSeconds = 4;
        /// <summary>
        /// The default value is 1. The recommended value for open service is 512 to avoid slow attacks on the client
        /// 接收发送数据不完整时连续两次最小字节数，默认为 1，开放服务建议值为 512 避免客户端慢攻击
        /// </summary>
        public ushort MinSocketSize = 1;
        /// <summary>
        /// The default value true indicates that the command controller queries the output data containing method name for client match verification
        /// 默认为 true 表示命令控制器查询输出数据包含方法名称用于客户端匹配验证
        /// </summary>
        public bool IsOutputControllerMethodName = true;
        /// <summary>
        /// If the default value is true, abnormal information about the server is displayed. If the service is open, you are advised to set it to false to avoid sensitive information leakage
        /// 默认为 true 表示输出服务端异常信息，开放服务建议设置为 false 避免敏感信息泄漏
        /// </summary>
        public bool IsOutputExceptionMessage = true;
        ///// <summary>
        ///// 最大认证失败次数，异步认证也可能当成失败处理，默认为 2
        ///// </summary>
        //public byte MaxVerifyMethodErrorCount = 2;
        /// <summary>
        /// The default is queue mode
        /// 默认为队列模式
        /// </summary>
        public CommandServerSocketBuildOutputThreadEnum BuildOutputThread;
        /// <summary>
        /// The default is true, indicating that the Nagle algorithm is disabled and data is sent immediately
        /// 默认为 true 表示禁用 Nagle 算法，立即发送数据
        /// </summary>
        public bool NoDelay = true;
        /// <summary>
        /// The default value of false indicates that remote expressions are not supported. Setting it to true means allowing arbitrary code execution, so the client needs to be fully trusted
        /// 默认为 false 表示不支持远程表达式；设置为 true 意味着允许执行任意代码，所以需要客户端完全可信任
        /// </summary>
        public bool IsRemoteExpression;
        /// <summary>
        /// The maximum Task.Run concurrency is set to 1024 by default, and should be set to 0 for highly concurrent lightweight applications
        /// 最大 Task.Run 并发默认为 1024，高并发轻量级应用应该设置为 0
        /// </summary>
        public int MaxTaskRunConcurrent = 1 << 10;
        /// <summary>
        /// The default value 1 indicates the pure queue mode. If the value is greater than 1, the concurrent throughput can be increased. However, the queue lock operation must be added to access shared resources, and the write operation can be executed only after all uncompleted read operations are complete. Therefore, the number of concurrent read tasks should not be too large to avoid a long wait time for write operations
        /// 异步读写队列最大读操作并发任务数量，默认为 1 表示纯队列模式，当设置大于 1 时可提高并发吞吐，但是访问共享资源需要增加队列锁操作，而且写操作需要等待所有未完成读取操作结束以后才能执行，所以并发读取任务数量不宜过大避免造成写操作等待时间过长
        /// </summary>
        public int TaskQueueMaxConcurrent = 1;
        /// <summary>
        /// The number of waiting read and write tasks in the asynchronous read and write queue. The default value is 16 and the minimum value is 1. The number of waiting read and write tasks should not be too large to prevent long write wait time
        /// 异步读写队列写操作等待读取操作任务数量，默认为 16，最小值为 1，等待读取操作任务数量不宜过大避免造成写操作等待时间过长
        /// </summary>
        public int TaskQueueWaitCount = 16;
        /// <summary>
        /// The default value is 60, indicating that the queue is deleted after no new tasks are performed within the specified period. If the value is 0, the queue is deleted immediately after the queue is executed to avoid memory occupation. If the value is negative, the memory is permanently occupied
        /// 异步读写队列驻留超时秒数，默认为 60 表示等待指定时间以后没有新任务再删除队列，设置为 0 表示队列任务执行完以后立即删除队列避免占用内存，设置为负数表示永久驻留内存
        /// </summary>
        public int TaskQueueTimeoutSeconds = 60;
        /// <summary>
        /// The maximum number of keep callbacks is set to 0 by default, indicating no limit. Open services should be set according to the actual situation to avoid memory usage attacks
        /// 最大保持回调数量，默认为 0 表示不限制，开放服务应该根据实际情况设置避免内存占用攻击
        /// </summary>
        public int MaxKeepCallbackCount;

        /// <summary>
        /// Validation sockets, such as IP addresses, return true by default
        /// 验证套接字，比如验证 IP 地址，默认返回 true
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public virtual bool Verify(Socket socket, CommandListener server)
        {
            return true;
        }
        /// <summary>
        /// The custom data processing, the default return AutoCSer.Net.CommandServer.ServerReceiveErrorTypeEnum.CustomCommandError and close the current socket (attention, because it is receiving data IO thread synchronization calls, If there is a block please open a new thread task processing)
        /// 自定义数据处理，默认返回 AutoCSer.Net.CommandServer.ServerReceiveErrorTypeEnum.CustomCommandError 并关闭当前套接字（注意，由于是接收数据 IO 线程同步调用，如果存在阻塞请新开线程任务处理）
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <returns>Server receives data error type. If not Success is returned, the current socket is closed
        /// 服务端接收数据错误类型，返回非 Success 则关闭当前套接字</returns>
        public virtual ServerReceiveErrorTypeEnum OnCustomData(CommandServerSocket socket, ref SubArray<byte> data)
        {
            return ServerReceiveErrorTypeEnum.CustomCommandError;
        }
        /// <summary>
        /// Get the service registration component, which returns new AutoCSer.Net.CommandServiceRegistrar(server) by default and is called all at once during service initialization
        /// 获取服务注册组件，默认返回 new AutoCSer.Net.CommandServiceRegistrar(server)，服务初始化时一次性调用
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public virtual Task<CommandServiceRegistrar> GetRegistrar(CommandListener server)
        {
            return Task.FromResult(new CommandServiceRegistrar(server));
        }
        /// <summary>
        /// Gets binary deserialization configuration parameters
        /// 获取二进制反序列化配置参数，默认返回 new AutoCSer.BinarySerialize.DeserializeConfig { IsDisposeMemberMap = true, MaxArraySize = BinaryDeserializeMaxArraySize 小于等于 0 ? int.MaxValue : BinaryDeserializeMaxArraySize }，服务初始化时一次性调用
        /// </summary>
        /// <returns></returns>
        public virtual AutoCSer.BinarySerialize.DeserializeConfig GetBinaryDeserializeConfig()
        {
            return new AutoCSer.BinarySerialize.DeserializeConfig { MaxArraySize = BinaryDeserializeMaxArraySize <= 0 ? int.MaxValue : BinaryDeserializeMaxArraySize };
        }
        /// <summary>
        /// Send data coding
        /// 发送数据编码
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="data">Original data
        /// 原始数据</param>
        /// <param name="dataIndex">Origin of original data
        /// 原始数据起始位置</param>
        /// <param name="dataSize">Number of bytes of original data
        /// 原始数据字节数</param>
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
        public virtual bool TransferEncode(CommandServerSocket socket, byte[] data, int dataIndex, int dataSize, ref ByteArrayBuffer buffer, ref SubArray<byte> outputData, int outputSeek, int outputHeadSize)
        {
            return false;
            //return AutoCSer.Common.Config.Compress(data, dataIndex, dataSize, ref buffer, ref outputData, outputSeek, outputHeadSize, CompressionLevel.Fastest);
        }
        /// <summary>
        /// Received data decoding
        /// 接收数据解码
        /// </summary>
        /// <param name="socket">Command server socket
        /// 命令服务套接字</param>
        /// <param name="transferData">The encoded data
        /// 编码后的数据</param>
        /// <param name="outputData">Original data buffer waiting to be written
        /// 等待写入的原始数据缓冲区</param>
        /// <returns>Whether the decoding is successful
        /// 是否解码成功</returns>
        public virtual bool TransferDecode(CommandServerSocket socket, SubArray<byte> transferData, ref SubArray<byte> outputData)
        {
            return false;
            //return AutoCSer.Common.Config.Decompress(transferData, ref outputData);
        }
        /// <summary>
        /// Create an asynchronous read/write queue management, the default return new AutoCSer.Net.CommandServerCallTaskQueueTypeSet(server), one-time calls upon initial service
        /// 创建异步读写队列管理，默认返回 new AutoCSer.Net.CommandServerCallTaskQueueTypeSet(server)，服务初始化时一次性调用
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public virtual CommandServerCallTaskQueueTypeSet GetTaskQueueTypeSet(CommandListener server)
        {
            return new CommandServerCallTaskQueueTypeSet(server);
        }
        /// <summary>
        /// Check whether the keyword of the asynchronous read/write queue is valid
        /// 检查异步读写队列关键字是否合法
        /// </summary>
        /// <typeparam name="T">Type of the queue keyword
        /// 队列关键字类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool CheckTaskQueueKey<T>(T key) where T : IEquatable<T>
        {
            return true;
        }
        /// <summary>
        /// Authentication API warnings exist for non-primary controllers.
        /// 非主控制器存在认证 API 警告
        /// </summary>
        /// <param name="controller"></param>
        public virtual void IgnoreVerifyMethod(CommandServerController controller)
        {
            Log.DebugIgnoreException($"扩展控制器 {controller.ControllerName} 验证函数将被忽略", LogLevelEnum.AutoCSer | LogLevelEnum.Debug | LogLevelEnum.Warn);
        }
        /// <summary>
        /// Synchronization queue task timeout notification
        /// 同步队列任务执行超时通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="seconds">Current task execution seconds
        /// 当前任务执行秒数</param>
        /// <returns></returns>
        public virtual Task OnQueueTimeout(CommandServerSocket socket, CommandServerCallQueue queue, long seconds)
        {
            if (queue.Controller == null) Log.DebugIgnoreException($"服务队列 [{queue.Index}] 任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            else Log.DebugIgnoreException($"控制器 {queue.Controller.ControllerName} 队列任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Asynchronous read/write queue task execution timeout notification
        /// 异步读写队列任务执行超时通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="seconds">Current task execution seconds
        /// 当前任务执行秒数</param>
        /// <returns></returns>
        public virtual Task OnQueueTimeout(CommandServerSocket socket, CommandServerCallTaskQueue queue, long seconds)
        {
            Log.DebugIgnoreException($"异步队列 {queue.KeyString} 任务耗时 {seconds} 秒，可能产生死锁并阻塞队列其它任务执行");
            return AutoCSer.Common.CompletedTask;
        }
        ///// <summary>
        ///// 套接字关闭后续处理
        ///// </summary>
        ///// <param name="socket"></param>
        //public virtual void OnSocketClosed(CommandServerSocket socket) { }

        /// <summary>
        /// Output information of the default empty server socket
        /// </summary>
        internal static readonly ServerOutput NullServerOutput = new ServerOutputReturnType(default(CallbackIdentity), default(CommandClientReturnTypeEnum));
#endif
        /// <summary>
        /// Default empty command server configuration
        /// </summary>
        internal static readonly CommandServerConfig Null = new CommandServerConfig { QueueTimeoutSeconds = 0 };
#if NetStandard21
        /// <summary>
        /// Default empty server interface method information
        /// </summary>
        internal static readonly ServerInterfaceMethod NullServerInterfaceMethod = new ServerInterfaceMethod();
#endif
    }
}
