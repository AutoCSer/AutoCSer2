using AutoCSer.Net;
using AutoCSer.TestCase.CommandServerPerformance;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    /// <summary>
    /// 命令服务性能测试客户端接口（客户端回调返回结果）
    /// </summary>
    public interface ICallbackClient
    {
        /// <summary>
        /// 服务端同步返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Synchronous(int left, int right, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 服务端回调返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Callback(int left, int right, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 服务端配置队列执行返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Queue(int left, int right, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 服务端 async 任务返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand Task(int left, int right, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand TaskQueue(int left, int right, Action<CommandClientReturnValue<int>> callback);
        /// <summary>
        /// 服务端 async 任务动态队列返回返回结果
        /// </summary>
        /// <param name="queueKey">队列关键字</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        CallbackCommand TaskQueueKey(int queueKey, int left, int right, Action<CommandClientReturnValue<int>> callback);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnly 应答处理
        /// </summary>
        /// <param name="callback"></param>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand KeepCallback(Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 服务端配合 KeepCallback 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        SendOnlyCommand SendOnly(int left, int right);

        /// <summary>
        /// 服务端保持回调返回结果，配合 SendOnlyTask 应答处理
        /// </summary>
        /// <param name="callback"></param>
        [CommandClientMethod(CallbackType = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.Synchronous)]
        KeepCallbackCommand KeepCallbackCount(Action<CommandClientReturnValue<int>, KeepCallbackCommand> callback);
        /// <summary>
        /// 服务端配合 KeepCallbackCount 应答处理
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        SendOnlyCommand SendOnlyTask(int left, int right);
    }
    /// <summary>
    /// 客户端回调返回结果
    /// </summary>
    internal sealed class CallbackClient : Client
    {
        /// <summary>
        /// 客户端回调返回结果测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            CommandClientConfig<ICallbackClient> commandClientConfig = new CommandClientConfig<ICallbackClient> { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Performance), CheckSeconds = 0 };
            using (CommandClient commandClient = new CommandClient(commandClientConfig, CommandClientInterfaceControllerCreator.GetCreator<ICallbackClient, IService>()))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    return;
                }
                CommandClientSocketEvent<ICallbackClient> client = (CommandClientSocketEvent<ICallbackClient>)commandClient.SocketEvent;
                Left = AutoCSer.Random.Default.Next();

                int testCount = Reset(commandClient, maxTestCount);
                for (int right = testCount; right != 0; await client.InterfaceController.Synchronous(Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.Synchronous));

                testCount = Reset(commandClient, maxTestCount);
                for (int right = testCount; right != 0; await client.InterfaceController.Callback(Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.Callback));

                testCount = Reset(commandClient, maxTestCount);
                for (int right = testCount; right != 0; await client.InterfaceController.Queue(Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.Queue));

                testCount = Reset(commandClient, maxTestCount >> 3);
                for (int right = testCount; right != 0; await client.InterfaceController.TaskQueue(Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.TaskQueue));

                testCount = Reset(commandClient, maxTestCount >> 3);
                for (int right = testCount; right != 0; await client.InterfaceController.TaskQueueKey(0, Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.TaskQueueKey));

                testCount = Reset(commandClient, maxTestCount >> 1);
                for (int right = testCount; right != 0; await client.InterfaceController.Task(Left, --right, CheckSynchronousHandle)) ;
                await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.Task));

                testCount = Reset(commandClient, maxTestCount);
                using (CommandKeepCallback commandKeepCallback = await client.InterfaceController.KeepCallback(CheckSynchronousKeepCallbackHandle))
                {
                    for (int right = testCount; right != 0; await client.InterfaceController.SendOnly(Left, --right)) ;
                    await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.KeepCallback));
                }

                testCount = Reset(commandClient, maxTestCount);
                using (CommandKeepCallback commandKeepCallback = await client.InterfaceController.KeepCallbackCount(CheckSynchronousKeepCallbackHandle))
                {
                    for (int right = testCount; right != 0; await client.InterfaceController.SendOnlyTask(Left, --right)) ;
                    await LoopCompleted(nameof(CallbackClient), nameof(client.InterfaceController.KeepCallbackCount));
                }
            }
        }
    }
}
