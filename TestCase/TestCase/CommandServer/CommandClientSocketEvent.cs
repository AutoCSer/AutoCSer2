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
        /// 回调接口测试
        /// </summary>
        public IClientCallbackTaskController ClientCallbackTaskController { get; private set; }
        /// <summary>
        /// 保持回调接口测试
        /// </summary>
        public IClientKeepCallbackController ClientKeepCallbackController { get; private set; }
        /// <summary>
        /// 异步任务接口测试
        /// </summary>
        public IClientTaskController ClientTaskController { get; private set; }
        /// <summary>
        /// 控制器异步队列接口测试
        /// </summary>
        public IServerTaskQueueControllerClientController ClientTaskQueueController { get; private set; }
        /// <summary>
        /// 保持回调异步任务接口测试
        /// </summary>
        public IClientKeepCallbackTaskController ClientKeepCallbackTaskController { get; private set; }
        /// <summary>
        /// 服务端 Task 队列客户端
        /// </summary>
        public CommandClientController<IClientTaskQueueContextController, int> ClientTaskQueueContextController { get; private set; }
        /// <summary>
        /// 定义对称接口测试
        /// </summary>
        public IDefinedSymmetryController DefinedSymmetryController { get; private set; }
        /// <summary>
        /// 客户端定义非对称测试接口
        /// </summary>
        public IDefinedDissymmetryClientController DefinedDissymmetryClientController { get; private set; }

        /// <summary>
        /// 同步接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientSynchronousController ServerBindContextClientSynchronousController { get; private set; }
        /// <summary>
        /// 仅发送数据接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientSendOnlyController ServerBindContextClientSendOnlyController { get; private set; }
        /// <summary>
        /// 队列接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientQueueController ServerBindContextClientQueueController { get; private set; }
        /// <summary>
        /// 回调接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientCallbackController ServerBindContextClientCallbackController { get; private set; }
        /// <summary>
        /// 回调接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientCallbackTaskController ServerBindContextClientCallbackTaskController { get; private set; }
        /// <summary>
        /// 保持回调接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientKeepCallbackController ServerBindContextClientKeepCallbackController { get; private set; }
        /// <summary>
        /// 异步任务接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientTaskController ServerBindContextClientTaskController { get; private set; }
        /// <summary>
        /// 保持回调异步任务接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientKeepCallbackTaskController ServerBindContextClientKeepCallbackTaskController { get; private set; }
        /// <summary>
        /// 控制器异步队列接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IServerTaskQueueControllerClientController ServerBindContextClientTaskQueueController { get; private set; }
        /// <summary>
        /// 定义对称接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IDefinedSymmetryController ServerBindContextDefinedSymmetryController { get; private set; }
        /// <summary>
        /// 客户端定义非对称测试接口（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IDefinedDissymmetryClientController ServerBindContextDefinedDissymmetryClientController { get; private set; }
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
                yield return new CommandClientControllerCreatorParameter(typeof(IServerCallbackTaskController), typeof(IClientCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackController), typeof(IClientKeepCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskController), typeof(IClientTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackTaskController), typeof(IClientKeepCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskQueueController), typeof(IServerTaskQueueControllerClientController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskQueueContextController), typeof(int), typeof(IClientTaskQueueContextController));
                yield return new CommandClientControllerCreatorParameter(typeof(IDefinedSymmetryController), typeof(IDefinedSymmetryController));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(IDefinedDissymmetryClientController));

                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerSynchronousController), typeof(ServerBindContext.IClientSynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerSendOnlyController), typeof(ServerBindContext.IClientSendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerQueueController), typeof(ServerBindContext.IClientQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerCallbackController), typeof(ServerBindContext.IClientCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerCallbackTaskController), typeof(ServerBindContext.IClientCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerKeepCallbackController), typeof(ServerBindContext.IClientKeepCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerTaskController), typeof(ServerBindContext.IClientTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerKeepCallbackTaskController), typeof(ServerBindContext.IClientKeepCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerTaskQueueController), typeof(ServerBindContext.IServerTaskQueueControllerClientController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IDefinedSymmetryController), typeof(ServerBindContext.IDefinedSymmetryController));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(ServerBindContext.IDefinedDissymmetryClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
        /// <summary>
        /// 调用客户端验证函数
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(((IClientSynchronousController)controller).SynchronousSocket(int.MinValue));
        }
    }
}
