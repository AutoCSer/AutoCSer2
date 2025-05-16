using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令客户端方法配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandClientMethodAttribute : CommandMethodAttribute
    {
        /// <summary>
        /// 调用超时秒数，默认为 0 表示不超时，最大值为客户端配置 CommandMaxTimeoutSeconds
        /// </summary>
        public ushort TimeoutSeconds;
        /// <summary>
        /// 数据回调线程模式，默认为 CheckRunTask
        /// </summary>
        public ClientCallbackTypeEnum CallbackType;
        /// <summary>
        /// 同步队列序号
        /// </summary>
        public byte QueueIndex;
        /// <summary>
        /// 是否低优先级同步队列队列
        /// </summary>
        public bool IsLowPriorityQueue;
        /// <summary>
        /// 匹配服务端方法名称，用于自动命令序号模式下客户端改写方法名称时指定匹配的服务端方法名称
        /// </summary>
#if NetStandard21
        public string? MatchMethodName;
#else
        public string MatchMethodName;
#endif

        /// <summary>
        /// 默认命令客户端方法配置
        /// </summary>
        internal static readonly CommandClientMethodAttribute Defafult = new CommandClientMethodAttribute();
    }
}
