using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.Example.CommandServer.Server
{
    /// <summary>
    /// 命令服务示例
    /// </summary>
    internal static class Listener
    {
        /// <summary>
        /// 命令服务
        /// </summary>
        private static CommandListener commandListener;
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        internal static async Task<CommandListener> Start()
        {
            if (commandListener == null)
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Example) };
                commandListener = new CommandListener(commandServerConfig
                    //服务认证 API 必须定义在服务端主控制器中
                    , CommandServerInterfaceControllerCreator.GetCreator<IVerifyController>(new VerifyController())

                    , CommandServerInterfaceControllerCreator.GetCreator<IDefinedSymmetryController>(new DefinedSymmetryController())

                    , CommandServerInterfaceControllerCreator.GetCreator<Synchronous.ISynchronousController>(new Synchronous.SynchronousController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Synchronous.ISendOnlyController>(new Synchronous.SendOnlyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Synchronous.ICallbackController>(new Synchronous.CallbackController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Synchronous.IKeepCallbackController>(new Synchronous.KeepCallbackController())

                    , CommandServerInterfaceControllerCreator.GetCreator<Queue.ISynchronousController>(new Queue.SynchronousController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Queue.ISendOnlyController>(new Queue.SendOnlyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Queue.ICallbackController>(new Queue.CallbackController())
                    , CommandServerInterfaceControllerCreator.GetCreator<Queue.IKeepCallbackController>(new Queue.KeepCallbackController())

                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTask.ISynchronousController>(new AsyncTask.SynchronousController())
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTask.ISendOnlyController>(new AsyncTask.SendOnlyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTask.IKeepCallbackController>(new AsyncTask.KeepCallbackController())

                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueue.ISynchronousController>(new AsyncTaskQueue.SynchronousController())
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueue.ISynchronousKeyController>(new AsyncTaskQueue.SynchronousKeyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueue.ISendOnlyController>(new AsyncTaskQueue.SendOnlyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueue.IKeepCallbackController>(new AsyncTaskQueue.KeepCallbackController())

                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueueContext.ISynchronousKeyController, int>((task, key) => new AsyncTaskQueueContext.SynchronousKeyController(task, key))
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueueContext.ISendOnlyController, int>((task, key) => new AsyncTaskQueueContext.SendOnlyController(task, key))
                    , CommandServerInterfaceControllerCreator.GetCreator<AsyncTaskQueueContext.IKeepCallbackController, int>((task, key) => new AsyncTaskQueueContext.KeepCallbackController(task, key))
                    );
                if (!await commandListener.Start()) return null;
            }
            return commandListener;
        }
    }
}
