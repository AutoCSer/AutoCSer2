﻿using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ServiceThreadStrategy.Client
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// 服务端一次性响应 API 客户端示例接口（服务端 Task 异步 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IReturnCommandController ServerTask_ReturnCommandController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 API 客户端示例接口（服务端 Task 异步读写队列 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public TaskQueue.IReturnCommandController ServerTaskQueue_ReturnCommandController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 API 客户端示例接口（服务端 Task 异步队列控制器 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public CommandClientController<IReturnCommandController, int> ServerTaskQueueController_ReturnCommandController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 API 客户端示例接口（服务端 同步队列线程 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IReturnCommandController ServerQueue_ReturnCommandController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 API 客户端示例接口（服务端 IO 线程同步 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IReturnCommandController ServerSynchronous_ReturnCommandController { get; private set; }

        /// <summary>
        /// 服务端一次性响应 回调委托 API 客户端示例接口（服务端 Task 异步 API 一次性响应 回调委托 API 示例接口）
        /// </summary>
        [AllowNull]
        public ICallbackController ServerTask_CallbackController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 回调委托 API 客户端示例接口（服务端 Task 异步读写队列 API 一次性响应 回调委托 API 示例接口）
        /// </summary>
        [AllowNull]
        public TaskQueue.ICallbackController ServerTaskQueue_CallbackController { get; private set; }
        ///// <summary>
        ///// 服务端一次性响应 回调委托 API 客户端示例接口（服务端 Task 异步队列控制器 API 一次性响应 回调委托 API 示例接口）
        ///// </summary>
        //[AllowNull]
        //public CommandClientController<ICallbackController, int> ServerTaskQueueController_CallbackController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 回调委托 API 客户端示例接口（服务端 同步队列线程 API 一次性响应 回调委托 API 示例接口）
        /// </summary>
        [AllowNull]
        public ICallbackController ServerQueue_CallbackController { get; private set; }
        /// <summary>
        /// 服务端一次性响应 回调委托 API 客户端示例接口（服务端 IO 线程同步 API 一次性响应 回调委托 API 示例接口）
        /// </summary>
        [AllowNull]
        public ICallbackController ServerSynchronous_CallbackController { get; private set; }

        /// <summary>
        /// 持续响应 API 客户端示例接口（服务端 Task 异步 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IEnumeratorCommandController ServerTask_EnumeratorCommandController { get; private set; }
        /// <summary>
        /// 持续响应 API 客户端示例接口（服务端 Task 异步读写队列 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public TaskQueue.IEnumeratorCommandController ServerTaskQueue_EnumeratorCommandController { get; private set; }
        ///// <summary>
        ///// 持续响应 API 客户端示例接口（服务端 Task 异步队列控制器 API 一次性响应 示例接口）
        ///// </summary>
        //[AllowNull]
        //public CommandClientController<IEnumeratorCommandController, int> ServerTaskQueueController_EnumeratorCommandController { get; private set; }
        /// <summary>
        /// 持续响应 API 客户端示例接口（服务端 同步队列线程 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IEnumeratorCommandController ServerQueue_EnumeratorCommandController { get; private set; }
        /// <summary>
        /// 持续响应 API 客户端示例接口（服务端 IO 线程同步 API 一次性响应 示例接口）
        /// </summary>
        [AllowNull]
        public IEnumeratorCommandController ServerSynchronous_EnumeratorCommandController { get; private set; }

        /// <summary>
        /// 服务端仅执行 API 客户端示例接口（服务端 Task 异步 API 仅执行 示例接口）
        /// </summary>
        [AllowNull]
        public ISendOnlyCommandController ServerTask_SendOnlyCommandController { get; private set; }
        /// <summary>
        /// 服务端仅执行 API 客户端示例接口（服务端 Task 异步读写队列 API 仅执行 示例接口）
        /// </summary>
        [AllowNull]
        public TaskQueue.ISendOnlyCommandController ServerTaskQueue_SendOnlyCommandController { get; private set; }
        ///// <summary>
        ///// 服务端仅执行 API 客户端示例接口（服务端 Task 异步队列控制器 API 仅执行 示例接口）
        ///// </summary>
        //[AllowNull]
        //public CommandClientController<SendOnlyCommand.ISendOnlyCommandController, int> ServerTaskQueueController_SendOnlyCommandController { get; private set; }
        /// <summary>
        /// 服务端仅执行 API 客户端示例接口（服务端 同步队列线程 API 仅执行 示例接口）
        /// </summary>
        [AllowNull]
        public ISendOnlyCommandController ServerQueue_SendOnlyCommandController { get; private set; }
        /// <summary>
        /// 服务端仅执行 API 客户端示例接口（服务端 IO 线程同步 API 仅执行 示例接口）
        /// </summary>
        [AllowNull]
        public ISendOnlyCommandController ServerSynchronous_SendOnlyCommandController { get; private set; }

        /// <summary>
        /// 服务端控制器 Task 异步读写队列 API 客户端示例接口
        /// </summary>
        [AllowNull]
        public Server.TaskQueue.ITaskQueueControllerClientController TaskQueueController { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Task.ISynchronousController), typeof(IReturnCommandController), null, nameof(ServerTask_ReturnCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueue.ISynchronousController), typeof(TaskQueue.IReturnCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueueController.ISynchronousController), typeof(int), typeof(IReturnCommandController), null, nameof(ServerTaskQueueController_ReturnCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ISynchronousController), typeof(IReturnCommandController), null, nameof(ServerQueue_ReturnCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ISynchronousController), typeof(IReturnCommandController), null, nameof(ServerSynchronous_ReturnCommandController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.Task.ICallbackController), typeof(ICallbackController), null, nameof(ServerTask_CallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueue.ICallbackController), typeof(TaskQueue.ICallbackController));
                //yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueueController.ICallbackController), typeof(int), typeof(ICallbackController), null, nameof(ServerTaskQueueController_CallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ICallbackController), typeof(ICallbackController), null, nameof(ServerQueue_CallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ICallbackController), typeof(ICallbackController), null, nameof(ServerSynchronous_CallbackController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.Task.IKeepCallbackController), typeof(IEnumeratorCommandController), null, nameof(ServerTask_EnumeratorCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueue.IKeepCallbackController), typeof(TaskQueue.IEnumeratorCommandController));
                //yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueueController.IKeepCallbackController), typeof(int), typeof(IEnumeratorCommandController), null, nameof(ServerTaskQueueController_EnumeratorCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.IKeepCallbackController), typeof(IEnumeratorCommandController), null, nameof(ServerQueue_EnumeratorCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.IKeepCallbackController), typeof(IEnumeratorCommandController), null, nameof(ServerSynchronous_EnumeratorCommandController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.Task.ISendOnlyController), typeof(ISendOnlyCommandController), null, nameof(ServerTask_SendOnlyCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueue.ISendOnlyController), typeof(TaskQueue.ISendOnlyCommandController));
                //yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueueController.ISendOnlyController), typeof(int), typeof(SendOnlyCommand.ISendOnlyCommandController), null, nameof(ServerTaskQueueController_SendOnlyCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ISendOnlyController), typeof(ISendOnlyCommandController), null, nameof(ServerQueue_SendOnlyCommandController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ISendOnlyController), typeof(ISendOnlyCommandController), null, nameof(ServerSynchronous_SendOnlyCommandController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.TaskQueue.ITaskQueueController), typeof(Server.TaskQueue.ITaskQueueControllerClientController));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
    }
}