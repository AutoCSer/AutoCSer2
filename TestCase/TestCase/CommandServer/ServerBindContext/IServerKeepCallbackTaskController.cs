﻿using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerKeepCallbackTaskController
    {
        Task KeepCallbackTaskReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTask(int Value, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskReturn(CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTask(CommandServerKeepCallback Callback);

        Task KeepCallbackCountTaskReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTask(int Value, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskReturn(CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTask(CommandServerKeepCallbackCount Callback);

        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskReturn(int Value, int Ref);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskReturn();

        Task KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);

        Task KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);

        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref);

#if !DotNet45 && !NetStandard2
        IAsyncEnumerable<string> AsyncEnumerableReturn(int Value, int Ref);
        IAsyncEnumerable<string> AsyncEnumerableReturn();

        IAsyncEnumerable<string> AsyncEnumerableQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref);
#endif
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerKeepCallbackTaskController : CommandServerBindContextController, IServerKeepCallbackTaskController
    {
        Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTask(int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTask(CommandServerKeepCallback Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(CommandServerKeepCallbackCount Callback)
        {
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref)));
        }
        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn()
        {
            return Task.FromResult(AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor()));
        }

        Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            await AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref)));
        }

#if !DotNet45 && !NetStandard2
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableReturn(int Value, int Ref)
        {
            await Task.Yield();
            foreach (string value in AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref)))
            {
                yield return value;
            }
        }
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableReturn()
        {
            await Task.Yield();
            foreach (string value in AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor()))
            {
                yield return value;
            }
        }

        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await Task.Yield();
            foreach (string value in AutoCSer.TestCase.ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref)))
            {
                yield return value;
            }
        }
#endif
    }
}