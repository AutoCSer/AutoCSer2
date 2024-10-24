﻿using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientKeepCallbackTaskController
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

        EnumeratorQueueCommand<string> KeepCallbackTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueue(int Value, int Ref);

        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueue(int Value, int Ref);

        EnumeratorCommand<string> EnumerableKeepCallbackCountTaskQueueReturn(int Value, int Ref);

#if !DotNet45 && !NetStandard2
        EnumeratorCommand<string> AsyncEnumerableReturn(int Value, int Ref);
        EnumeratorCommand<string> AsyncEnumerableReturn();

        EnumeratorCommand<string> AsyncEnumerableQueueReturn(int Value, int Ref);
#endif
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal static class ClientKeepCallbackTaskController
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
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTask();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTask();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

#if !DotNet45 && !NetStandard2
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            IAsyncEnumerable<string> asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }

            asyncEnumerable = client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturnAsync();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(asyncEnumerable, clientSessionObject))
            {
                return false;
            }
#endif
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand<string> enumeratorQueueCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand enumeratorQueueCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ServerBindContextClientKeepCallbackTaskController.KeepCallbackCountTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorQueueCommand, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

#if !DotNet45 && !NetStandard2
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableReturn();
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ServerBindContextClientKeepCallbackTaskController.AsyncEnumerableQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await AutoCSer.TestCase.ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return false;
            }
#endif
            return true;
        }
    }
}
