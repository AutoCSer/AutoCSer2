using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// 同步接口测试
        /// </summary>
        public IClientSynchronousController ClientSynchronousController { get; private set; }
        /// <summary>
        /// 仅发送数据接口测试
        /// </summary>
        public IClientSendOnlyController ClientSendOnlyController { get; private set; }
        /// <summary>
        /// 队列接口测试
        /// </summary>
        public IClientQueueController ClientQueueController { get; private set; }
        /// <summary>
        /// 回调接口测试
        /// </summary>
        public IClientCallbackController ClientCallbackController { get; private set; }
        /// <summary>
        /// 保持回调接口测试
        /// </summary>
        public IClientKeepCallbackController ClientKeepCallbackController { get; private set; }
        /// <summary>
        /// 异步任务接口测试
        /// </summary>
        public IClientTaskController ClientTaskController { get; private set; }
        /// <summary>
        /// 保持回调异步任务接口测试
        /// </summary>
        public IClientKeepCallbackTaskController ClientKeepCallbackTaskController { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(IServerSynchronousController), typeof(IClientSynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerSendOnlyController), typeof(IClientSendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerQueueController), typeof(IClientQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerCallbackController), typeof(IClientCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackController), typeof(IClientKeepCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskController), typeof(IClientTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackTaskController), typeof(IClientKeepCallbackTaskController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(CommandClient client) : base(client) { }
        /// <summary>
        /// 调用客户端验证函数
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override async Task<CommandClientReturnValue<CommandServerVerifyState>> CallVerifyMethod(CommandClientController controller)
        {
            return ((IClientSynchronousController)controller).SynchronousSocket(int.MinValue);
        }
    }
}
