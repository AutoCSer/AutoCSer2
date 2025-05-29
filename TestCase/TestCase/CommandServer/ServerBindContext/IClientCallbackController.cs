using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerCallbackController))]
#endif
    public partial interface IClientCallbackController
    {
        CallbackCommand CallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(Action<CommandClientReturnValue> Callback);

        CallbackCommand CallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientCallbackController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            return TestCase(client.ServerBindContextClientCallbackController, clientSessionObject);
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(ServerBindContext.IClientCallbackController client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.Callback(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackReturn(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.Callback(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueReturn(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueue(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
        /// <summary>
        /// 短连接命令客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> ShortLinkTestCase()
        {
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                AutoCSer.TestCase.ClientCallbackController.ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ServerBindContextClientCallbackController.CallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientCallbackController.Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
                if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                AutoCSer.TestCase.ClientCallbackController.ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ServerBindContextClientCallbackController.CallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientCallbackController.Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
                if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                AutoCSer.TestCase.ClientCallbackController.ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ServerBindContextClientCallbackTaskController.CallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientCallbackController.Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
                if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                AutoCSer.TestCase.ClientCallbackController.ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ServerBindContextClientCallbackTaskController.CallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientCallbackController.Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ClientCallbackController.WaitCallback();
                if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
