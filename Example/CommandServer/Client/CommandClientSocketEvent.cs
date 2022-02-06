using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCSer.Net;

namespace AutoCSer.Example.CommandServer.Client
{
    /// <summary>
    /// 默认命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEvent
    {
        /// <summary>
        /// 服务认证 API 示例接口
        /// </summary>
        public IVerifyController VerifyController { get; private set; }
        /// <summary>
        /// 服务端 IO线程同步调用 同步返回数据 示例接口
        /// </summary>
        public Synchronous.ISynchronousController Synchronous_SynchronousController { get; private set; }
        /// <summary>
        /// 服务端 IO线程同步调用 不返回数据（不应答客户端）
        /// </summary>
        public Synchronous.ISendOnlyController Synchronous_SendOnlyController { get; private set; }
        /// <summary>
        /// 服务端 IO线程同步调用 回调委托返回数据 示例接口
        /// </summary>
        public Synchronous.ICallbackController Synchronous_CallbackController { get; private set; }
        /// <summary>
        /// 服务端 IO线程同步调用 保持回调委托返回数据 示例接口
        /// </summary>
        public Synchronous.IKeepCallbackController Synchronous_KeepCallbackController { get; private set; }
        /// <summary>
        /// 服务端 同步队列线程调用 同步返回数据 示例接口
        /// </summary>
        public Queue.ISynchronousController Queue_SynchronousController { get; private set; }
        /// <summary>
        /// 服务端 同步队列线程调用 不返回数据（不应答客户端）
        /// </summary>
        public Queue.ISendOnlyController Queue_SendOnlyController { get; private set; }
        /// <summary>
        /// 服务端 同步队列线程调用 回调委托返回数据 示例接口
        /// </summary>
        public Queue.ICallbackController Queue_CallbackController { get; private set; }
        /// <summary>
        /// 服务端 同步队列线程调用 保持回调委托返回数据 示例接口
        /// </summary>
        public Queue.IKeepCallbackController Queue_KeepCallbackController { get; private set; }
        /// <summary>
        /// 服务端 async Task 调用 同步返回数据 示例接口
        /// </summary>
        public AsyncTask.ISynchronousController AsyncTask_SynchronousController { get; private set; }
        /// <summary>
        /// 服务端 async Task 调用 不返回数据（不应答客户端）
        /// </summary>
        public AsyncTask.ISendOnlyController AsyncTask_SendOnlyController { get; private set; }
        /// <summary>
        /// 服务端 async Task 调用 保持回调委托返回数据 示例接口
        /// </summary>
        public AsyncTask.IKeepCallbackController AsyncTask_KeepCallbackController { get; private set; }
        /// <summary>
        /// 服务端 async Task 读写队列调用 同步返回数据 示例接口
        /// </summary>
        public AsyncTaskQueue.ISynchronousController AsyncTaskQueue_SynchronousController { get; private set; }
        /// <summary>
        /// 服务端 async Task 读写队列调用 不返回数据（不应答客户端）
        /// </summary>
        public AsyncTaskQueue.ISendOnlyController AsyncTaskQueue_SendOnlyController { get; private set; }
        /// <summary>
        /// 服务端 async Task 读写队列调用 保持回调委托返回数据 示例接口
        /// </summary>
        public AsyncTaskQueue.IKeepCallbackController AsyncTaskQueue_KeepCallbackController { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                //服务认证 API 必须定义在客户端主控制器中
                yield return new CommandClientControllerCreatorParameter(typeof(Server.IVerifyController), typeof(IVerifyController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ISynchronousController), typeof(Synchronous.ISynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ISendOnlyController), typeof(Synchronous.ISendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.ICallbackController), typeof(Synchronous.ICallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Synchronous.IKeepCallbackController), typeof(Synchronous.IKeepCallbackController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ISynchronousController), typeof(Queue.ISynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ISendOnlyController), typeof(Queue.ISendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.ICallbackController), typeof(Queue.ICallbackController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.Queue.IKeepCallbackController), typeof(Queue.IKeepCallbackController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTask.ISynchronousController), typeof(AsyncTask.ISynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTask.ISendOnlyController), typeof(AsyncTask.ISendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTask.IKeepCallbackController), typeof(AsyncTask.IKeepCallbackController));

                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTaskQueue.ISynchronousController), typeof(AsyncTaskQueue.ISynchronousController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTaskQueue.ISendOnlyController), typeof(AsyncTaskQueue.ISendOnlyController));
                yield return new CommandClientControllerCreatorParameter(typeof(Server.AsyncTaskQueue.IKeepCallbackController), typeof(AsyncTaskQueue.IKeepCallbackController));
            }
        }

        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        internal CommandClientSocketEvent(CommandClient client) : base(client) { }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override async Task<CommandClientReturnValue<CommandServerVerifyState>> CallVerifyMethod(CommandClientController controller)
        {
            return await ((IVerifyController)controller).VerifyAsync(1, 2);
        }
    }
}
