using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed partial class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
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
        /// 队列接口测试
        /// </summary>
        public IClientConcurrencyReadQueueController ClientConcurrencyReadQueueController { get; private set; }
        /// <summary>
        /// 队列接口测试
        /// </summary>
        public IClientReadWriteQueueController ClientReadWriteQueueController { get; private set; }
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
        /// 二阶段回调接口测试
        /// </summary>
        public IClientTwoStage‌CallbackController ClientTwoStage‌CallbackController { get; private set; }
        /// <summary>
        /// 异步任务接口测试
        /// </summary>
        public IClientTaskController ClientTaskController { get; private set; }
        /// <summary>
        /// 保持回调异步任务接口测试
        /// </summary>
        public IClientKeepCallbackTaskController ClientKeepCallbackTaskController { get; private set; }
        /// <summary>
        /// 二阶段回调异步任务接口测试
        /// </summary>
        public IClientTwoStage‌CallbackTaskController ClientTwoStage‌CallbackTaskController { get; private set; }
        /// <summary>
        /// 定义对称接口测试
        /// </summary>
        public IDefinedSymmetryController DefinedSymmetryController { get; private set; }
        /// <summary>
        /// 客户端定义非对称测试接口
        /// </summary>
        public IDefinedDissymmetryClientController DefinedDissymmetryClientController { get; private set; }
#if !AOT
        /// <summary>
        /// 控制器异步队列接口测试
        /// </summary>
        public IServerTaskQueueControllerClientController ClientTaskQueueController { get; private set; }
        /// <summary>
        /// 服务端 Task 队列客户端
        /// </summary>
        public AutoCSer.Net.CommandServer.TaskQueueClientController<IClientTaskQueueContextController, int> ClientTaskQueueContextController { get; private set; }
#endif

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
        /// 队列接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientConcurrencyReadQueueController ServerBindContextClientConcurrencyReadQueueController { get; private set; }
        /// <summary>
        /// 队列接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IClientReadWriteQueueController ServerBindContextClientReadWriteQueueController { get; private set; }
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
        /// 定义对称接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IDefinedSymmetryController ServerBindContextDefinedSymmetryController { get; private set; }
        /// <summary>
        /// 客户端定义非对称测试接口（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IDefinedDissymmetryClientController ServerBindContextDefinedDissymmetryClientController { get; private set; }
#if !AOT
        /// <summary>
        /// 控制器异步队列接口测试（套接字上下文绑定服务端）
        /// </summary>
        public ServerBindContext.IServerTaskQueueControllerClientController ServerBindContextClientTaskQueueController { get; private set; }
        /// <summary>
        /// 远程表达式委托接口测试
        /// </summary>
        public IServerRemoteExpressionDelegateControllerClientController ClientRemoteExpressionDelegateController { get; private set; }
#endif
        /// <summary>
        /// 代码生成客户端接口测试
        /// </summary>
        public IServerCodeGeneratorControllerClientController ClientCodeGeneratorController { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                //由于存在短连接测试，客户端与服务端的控制器定义顺序必须保持一致
                yield return new CommandClientControllerCreatorParameter(typeof(IServerSynchronousController), typeof(IClientSynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerSendOnlyController), typeof(IClientSendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerQueueController), typeof(IClientQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerConcurrencyReadQueueController), typeof(IClientConcurrencyReadQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerReadWriteQueueController), typeof(IClientReadWriteQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerCallbackController), typeof(IClientCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerCallbackTaskController), typeof(IClientCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackController), typeof(IClientKeepCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTwoStage‌CallbackController), typeof(IClientTwoStage‌CallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskController), typeof(IClientTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerKeepCallbackTaskController), typeof(IClientKeepCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTwoStage‌CallbackTaskController), typeof(IClientTwoStage‌CallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(IDefinedSymmetryController), typeof(IDefinedSymmetryController));
                yield return new CommandClientControllerCreatorParameter(string.Empty, typeof(IDefinedDissymmetryClientController));
#if !AOT
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskQueueController), typeof(IServerTaskQueueControllerClientController));
                yield return new CommandClientControllerCreatorParameter(typeof(IServerTaskQueueContextController), typeof(int), typeof(IClientTaskQueueContextController));
#endif

                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerSynchronousController), typeof(ServerBindContext.IClientSynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerSendOnlyController), typeof(ServerBindContext.IClientSendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerQueueController), typeof(ServerBindContext.IClientQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerConcurrencyReadQueueController), typeof(ServerBindContext.IClientConcurrencyReadQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerReadWriteQueueController), typeof(ServerBindContext.IClientReadWriteQueueController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerCallbackController), typeof(ServerBindContext.IClientCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerCallbackTaskController), typeof(ServerBindContext.IClientCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerKeepCallbackController), typeof(ServerBindContext.IClientKeepCallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerTaskController), typeof(ServerBindContext.IClientTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerKeepCallbackTaskController), typeof(ServerBindContext.IClientKeepCallbackTaskController));
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IDefinedSymmetryController), typeof(ServerBindContext.IDefinedSymmetryController));
                yield return new CommandClientControllerCreatorParameter(nameof(IDefinedDissymmetryServerController), typeof(ServerBindContext.IDefinedDissymmetryClientController));
#if !AOT
                yield return new CommandClientControllerCreatorParameter(typeof(ServerBindContext.IServerTaskQueueController), typeof(ServerBindContext.IServerTaskQueueControllerClientController));
                if ((bool)customParameter) yield return new CommandClientControllerCreatorParameter(typeof(IServerRemoteExpressionDelegateController), typeof(IServerRemoteExpressionDelegateControllerClientController));
#endif
                yield return new CommandClientControllerCreatorParameter(typeof(IServerCodeGeneratorController), typeof(IServerCodeGeneratorControllerClientController));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(CommandClient client) : this(client, true) { }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="isRemoteExpression"></param>
        public CommandClientSocketEvent(CommandClient client, bool isRemoteExpression) : base(client, isRemoteExpression) { }
#if AOT
        /// <summary>
        /// The notification of the server controller name was not found
        /// 没有找到服务端控制器名称通知
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override Task NotFoundControllerName(CommandClientSocket socket, string controllerName)
        {
            switch (controllerName)
            {
                case "AutoCSer.TestCase.IServerTaskQueueController":
                case "AutoCSer.TestCase.IServerTaskQueueContextController+System.Int32":
                case "AutoCSer.TestCase.ServerBindContext.IServerTaskQueueController":
                case "AutoCSer.TestCase.IServerRemoteExpressionDelegateController":
                    return AutoCSer.Common.CompletedTask;
            }
            return base.NotFoundControllerName(socket, controllerName);
        }
#else
        /// <summary>
        /// Generate the client controller encapsulation type for directly obtaining the return value
        /// 生成直接获取返回值的客户端控制器封装类型
        /// </summary>
        public override bool IsCodeGeneratorReturnValueController { get { return true; } }
#endif
    }
}
