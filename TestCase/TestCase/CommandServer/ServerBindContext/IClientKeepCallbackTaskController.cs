using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerKeepCallbackTaskController))]
#endif
    public partial interface IClientKeepCallbackTaskController
    {
        EnumeratorCommand<string> KeepCallbackTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackTaskReturn();
        EnumeratorCommand KeepCallbackTask();

        EnumeratorCommand<string> KeepCallbackCountTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackCountTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackCountTaskReturn();
        EnumeratorCommand KeepCallbackCountTask();

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskReturn(int Value, int Ref);
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskReturn();

#if NetStandard21
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskReturnAsync();

        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskReturnAsync();

        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskReturnAsync();
#endif

        EnumeratorQueueCommand<string> KeepCallbackTaskQueueReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueue(int queueKey, int Ref);

        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueue(int queueKey, int Ref);

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueReturn(int queueKey, int Ref);

#if NetStandard21
        EnumeratorCommand<string> AsyncEnumerableReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableReturn();

        EnumeratorCommand<string> AsyncEnumerableQueueReturn(int queueKey, int Ref);
#endif
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientKeepCallbackTaskController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTask();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTask();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if NetStandard21
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            IAsyncEnumerable<string> asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand<string> enumeratorQueueCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand enumeratorQueueCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if NetStandard21
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif

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
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#if NetStandard21
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#endif
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorQueueCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorQueueCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#if NetStandard21
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#endif
            return true;
        }
    }
}
