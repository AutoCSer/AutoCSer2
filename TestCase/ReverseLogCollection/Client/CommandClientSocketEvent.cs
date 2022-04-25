using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ReverseLogCollection;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : TimestampVerifyCommandClientSocketEvent
    {
        /// <summary>
        /// 反向日志收集客户端
        /// </summary>
        private readonly ReverseLogCollectionClient client;
        /// <summary>
        /// 反向日志收集服务客户端
        /// </summary>
        public IReverseLogCollectionClient<LogInfo> ReverseLogCollectionClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IReverseLogCollectionService<LogInfo>), typeof(IReverseLogCollectionClient<LogInfo>));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="reverseLogCollectionClient">反向日志收集客户端</param>
        public CommandClientSocketEvent(CommandClient client, ReverseLogCollectionClient reverseLogCollectionClient) : base(client, AutoCSer.TestCase.Common.Config.TimestampVerifyString)
        {
            this.client = reverseLogCollectionClient;
        }
        /// <summary>
        /// 命令客户端套接字通过认证 API 并自动绑定客户端控制器以后的客户端自定义初始化操作，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <returns></returns>
        protected override async Task onMethodVerified()
        {
            client.OnMethodVerified(ReverseLogCollectionClient);
        }
    }
}
