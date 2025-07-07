using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Command client method configuration
    /// 命令客户端方法配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandClientMethodAttribute : CommandMethodAttribute
    {
        /// <summary>
        /// The timeout period for the call is set to 0 by default, indicating no timeout. The maximum value is the client configuration AutoCSer.Net.CommandClientConfig.CommandMaxTimeoutSeconds
        /// 调用超时秒数，默认为 0 表示不超时，最大值为客户端配置 AutoCSer.Net.CommandClientConfig.CommandMaxTimeoutSeconds
        /// </summary>
        public ushort TimeoutSeconds;
        /// <summary>
        /// Data callback thread mode, with the default being CheckRunTask
        /// 数据回调线程模式，默认为 CheckRunTask
        /// </summary>
        public ClientCallbackTypeEnum CallbackType;
        /// <summary>
        /// Synchronous queue sequence number
        /// 同步队列序号
        /// </summary>
        public byte QueueIndex;
        /// <summary>
        /// Whether it is a low-priority synchronous queue
        /// 是否低优先级同步队列
        /// </summary>
        public bool IsLowPriorityQueue;
        /// <summary>
        /// Match the server method name, which is used to specify the matching server method name when the client rewrites the method name in the automatic command sequence number mode
        /// 匹配服务端方法名称，用于自动命令序号模式下客户端改写方法名称时指定匹配的服务端方法名称
        /// </summary>
#if NetStandard21
        public string? MatchMethodName;
#else
        public string MatchMethodName;
#endif

        /// <summary>
        /// Default command client method configuration
        /// 默认命令客户端方法配置
        /// </summary>
        internal static readonly CommandClientMethodAttribute Defafult = new CommandClientMethodAttribute();
    }
}
