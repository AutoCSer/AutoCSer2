using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端套接字事件
    /// </summary>
    public class ProcessGuardClientSocketEvent : CommandClientSocketEvent, IProcessGuardClientSocketEvent
    {
        /// <summary>
        /// 进程守护客户端
        /// </summary>
        public ProcessGuardClient ProcessGuardClient { get; protected set; }
        /// <summary>
        /// 进程守护客户端接口
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public IProcessGuardServiceClientController IProcessGuardClient { get; protected set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(IProcessGuardService), typeof(IProcessGuardServiceClientController));
            }
        }
        /// <summary>
        /// 进程守护客户端套接字事件
        /// </summary>
        /// <param name="commandClient">命令客户端</param>
        public ProcessGuardClientSocketEvent(CommandClient commandClient) : base(commandClient)
        {
            ProcessGuardClient = new ProcessGuardClient(this);
        }
        /// <summary>
        /// 当前套接字通过验证方法，用于手动绑定设置客户端控制器与连接初始化操作，比如初始化保持回调。此调用位于客户端锁操作中，应尽快未完成初始化操作，禁止调用内部嵌套锁操作避免死锁
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public override Task OnSetController(CommandClientSocket socket)
        {
            return ProcessGuardClient.OnClientMethodVerified();
        }
    }
}
