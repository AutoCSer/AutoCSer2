using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerKeepCallbackTaskController), true)]
#endif
    public partial interface IClientKeepCallbackTaskController
    {
        EnumeratorCommand<string> KeepCallbackTaskSocketReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackTaskSocket(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackTaskSocketReturn();
        EnumeratorCommand KeepCallbackTaskSocket();
        EnumeratorCommand<string> KeepCallbackTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackTaskReturn();
        EnumeratorCommand KeepCallbackTask();

        EnumeratorCommand<string> KeepCallbackCountTaskSocketReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackCountTaskSocket(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackCountTaskSocketReturn();
        EnumeratorCommand KeepCallbackCountTaskSocket();
        EnumeratorCommand<string> KeepCallbackCountTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackCountTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackCountTaskReturn();
        EnumeratorCommand KeepCallbackCountTask();

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskSocketReturn(int Value, int Ref);
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskSocketReturn();
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskReturn(int Value, int Ref);
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskReturn();

#if NetStandard21
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskSocketReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskSocketReturnAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackTaskReturnAsync();

        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskSocketReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskSocketReturnAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> KeepCallbackCountTaskReturnAsync();

        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskSocketReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskSocketReturnAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn))]
        IAsyncEnumerable<string> EnumerableKeepCallbackCountTaskReturnAsync();
#endif

        EnumeratorQueueCommand<string> KeepCallbackTaskQueueSocketReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueueSocket(int queueKey, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackTaskQueueReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueue(int queueKey, int Ref);

        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueSocketReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueueSocket(int queueKey, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueReturn(int queueKey, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueue(int queueKey, int Ref);

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueSocketReturn(int queueKey, int Ref);
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueReturn(int queueKey, int Ref);

#if NetStandard21
        EnumeratorCommand<string> AsyncEnumerableSocketReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableSocketReturn();
        EnumeratorCommand<string> AsyncEnumerableReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableReturn();

        EnumeratorCommand<string> AsyncEnumerableQueueSocketReturn(int queueKey, int Ref);
        EnumeratorCommand<string> AsyncEnumerableQueueReturn(int queueKey, int Ref);
#endif
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientKeepCallbackTaskController
    {
        internal static async Task<bool> Callback(EnumeratorCommand<string> enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                string value = enumeratorCommand.Current;
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerSynchronousController.SessionObject.CheckXor(value, index))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
#if NetStandard21
        internal static async Task<bool> Callback(IAsyncEnumerable<string> asyncEnumerable, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            await foreach(string value in asyncEnumerable)
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerSynchronousController.SessionObject.CheckXor(value, index))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        internal static async Task<bool> Callback(IAsyncEnumerable<string> asyncEnumerable)
        {
            int index = 0;
            await foreach (string value in asyncEnumerable)
            {
                if (value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
#endif
        internal static async Task<bool> Callback(EnumeratorCommand enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        internal static async Task<bool> Callback(EnumeratorQueueCommand<string> enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                string value = enumeratorCommand.Current;
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerSynchronousController.SessionObject.CheckXor(value, index))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        internal static async Task<bool> Callback(EnumeratorQueueCommand enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        internal static async Task<bool> Callback(EnumeratorCommand<string> enumeratorCommand)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (enumeratorCommand.Current == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
        internal static async Task<bool> Callback(EnumeratorQueueCommand<string> enumeratorCommand)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (enumeratorCommand.Current == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }

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
            EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if NetStandard21
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            IAsyncEnumerable<string> asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand<string> enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if NetStandard21
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif

            return true;
        }
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            EnumeratorCommand<string> stringCommand = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn(0, 0);
            if (await stringCommand != null || stringCommand.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            EnumeratorCommand command = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket(0, 0);
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            stringCommand = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn();
            if (await stringCommand != null || stringCommand.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket();
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if NetStandard21
            try
            {
                client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync(0, 0);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }
#endif
            EnumeratorQueueCommand<string> stringQueueCommand = client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(0, 0);
            if (await stringQueueCommand != null || stringQueueCommand.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            EnumeratorQueueCommand queueCommand = client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocket(0, 0);
            if (await queueCommand != null || queueCommand.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
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
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#if NetStandard21
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                IAsyncEnumerable<string> enumeratorCommandReturn = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#endif
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorQueueCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorQueueCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#if NetStandard21
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = client.ClientKeepCallbackTaskController.AsyncEnumerableReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientKeepCallbackTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                EnumeratorCommand<string> enumeratorCommandReturn = client.ClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!await Callback(enumeratorCommandReturn))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
#endif
            return true;
        }
    }
}
