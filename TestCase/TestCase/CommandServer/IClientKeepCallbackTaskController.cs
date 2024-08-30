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
    public interface IClientKeepCallbackTaskController
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

        EnumeratorQueueCommand<string> KeepCallbackTaskQueueSocketReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueueSocket(int Value, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueue(int Value, int Ref);

        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueSocketReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueueSocket(int Value, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueue(int Value, int Ref);

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueSocketReturn(int Value, int Ref);
        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueReturn(int Value, int Ref);

#if !DotNet45 && !NetStandard2
        EnumeratorCommand<string> AsyncEnumerableSocketReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableSocketReturn();
        EnumeratorCommand<string> AsyncEnumerableReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableReturn();

        EnumeratorCommand<string> AsyncEnumerableQueueSocketReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableQueueReturn(int Value, int Ref);
#endif
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientKeepCallbackTaskController
    {
        internal static async Task<bool> Callback(EnumeratorCommand<string> enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                string value = enumeratorCommand.Current;
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return false;
                }
                if (value != (ServerSynchronousController.SessionObject.Xor() + index).ToString())
                {
                    return false;
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return false;
            }
            return true;
        }
        internal static async Task<bool> Callback(IAsyncEnumerable<string> asyncEnumerable, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            await foreach(string value in asyncEnumerable)
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return false;
                }
                if (value != (ServerSynchronousController.SessionObject.Xor() + index).ToString())
                {
                    return false;
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return false;
            }
            return true;
        }
        internal static async Task<bool> Callback(EnumeratorCommand enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return false;
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return false;
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
                    return false;
                }
                if (value != (ServerSynchronousController.SessionObject.Xor() + index).ToString())
                {
                    return false;
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return false;
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
                    return false;
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount || enumeratorCommand.ReturnType != CommandClientReturnTypeEnum.Success)
            {
                return false;
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
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask();
            if (!await Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

#if !DotNet45 && !NetStandard2
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            IAsyncEnumerable<string> asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync();
            if (!await Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }
#endif

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand<string> enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

#if !DotNet45 && !NetStandard2
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableSocketReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableReturn();
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }
#endif
            return true;
        }
    }
}
