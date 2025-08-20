using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerCallbackController), true)]
    public partial interface IClientCallbackController
    {
        CallbackCommand CallbackSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand CallbackSocket(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackSocketReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand CallbackSocket(Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(Action<CommandClientReturnValue> Callback);

        CallbackCommand CallbackQueueSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocket(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocketReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocket(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientCallbackController
    {
        internal static readonly System.Threading.SemaphoreSlim CallbackWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        internal static Task WaitCallback()
        {
            return CallbackWaitLock.WaitAsync();
        }
        internal static CommandClientReturnValue<string> ReturnValue;
        internal static void Callback(CommandClientReturnValue<string> value)
        {
            ReturnValue = value;
            CallbackWaitLock.Release();
        }
        internal static CommandClientReturnValue ReturnType;
        internal static void Callback(CommandClientReturnValue value)
        {
            ReturnType = value;
            CallbackWaitLock.Release();
        }
        internal static void Callback(CommandClientReturnValue<string> value, CommandClientCallQueue queue)
        {
            ReturnValue = value;
            CallbackWaitLock.Release();
        }
        internal static void Callback(CommandClientReturnValue value, CommandClientCallQueue queue)
        {
            ReturnType = value;
            CallbackWaitLock.Release();
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            return TestCase(client.ClientCallbackController, clientSessionObject);
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(IClientCallbackController client, CommandServerSessionObject clientSessionObject)
        {
#if !AOT
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackSocketReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackSocket(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.Callback(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.Callback(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueSocketReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueSocket(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(ReturnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueue(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif
            return true;
        }
        internal static void DefaultControllerCallback(CommandClientReturnValue<string> value)
        {
            if (value.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                AutoCSer.ConsoleWriteQueue.WriteLine(value.ReturnType.ToString());
            }
        }
        internal static void DefaultControllerCallback(CommandClientReturnValue value)
        {
            if (value.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                AutoCSer.ConsoleWriteQueue.WriteLine(value.ReturnType.ToString());
            }
        }
        internal static void DefaultControllerCallback(CommandClientReturnValue<string> value, CommandClientCallQueue queue)
        {
            if (value.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                AutoCSer.ConsoleWriteQueue.WriteLine(value.ReturnType.ToString());
            }
        }
        internal static void DefaultControllerCallback(CommandClientReturnValue value, CommandClientCallQueue queue)
        {
            if (value.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                AutoCSer.ConsoleWriteQueue.WriteLine(value.ReturnType.ToString());
            }
        }
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            return DefaultControllerTestCase(client.ClientCallbackController);
        }
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> DefaultControllerTestCase(IClientCallbackController client)
        {
            if (await client.CallbackSocketReturn(0, 0, DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackSocket(0, 0, DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackSocketReturn(DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackSocket(DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackQueueSocketReturn(0, 0, DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackQueueSocket(0, 0, DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackQueueSocketReturn(DefaultControllerCallback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (await client.CallbackQueueSocket(DefaultControllerCallback))
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
                if (client?.ClientCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ClientCallbackController.CallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await CallbackWaitLock.WaitAsync();
                if (!ReturnValue.IsSuccess || ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ClientCallbackController.CallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await CallbackWaitLock.WaitAsync();
                if (!ReturnValue.IsSuccess || ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ClientCallbackTaskController.CallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await CallbackWaitLock.WaitAsync();
                if (!ReturnValue.IsSuccess || ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ReturnValue = default(CommandClientReturnValue<string>);
                if (!await client.ClientCallbackTaskController.CallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), Callback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await CallbackWaitLock.WaitAsync();
                if (!ReturnValue.IsSuccess || ReturnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
