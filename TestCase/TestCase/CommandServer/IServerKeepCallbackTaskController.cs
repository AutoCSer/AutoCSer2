using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerKeepCallbackTaskController
    {
        Task KeepCallbackTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskSocket(CommandServerSocket socket, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTask(int Value, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskReturn(CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTask(CommandServerKeepCallback Callback);

        Task KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTask(int Value, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskReturn(CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTask(CommandServerKeepCallbackCount Callback);

        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskSocketReturn(CommandServerSocket socket);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskReturn(int Value, int Ref);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskReturn();

        Task KeepCallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskQueue(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallback Callback);

        Task KeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallbackCount Callback);

        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
        Task<IEnumerable<string>> EnumerableKeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref);

#if NetStandard21
        IAsyncEnumerable<string> AsyncEnumerableSocketReturn(CommandServerSocket socket, int Value, int Ref);
        IAsyncEnumerable<string> AsyncEnumerableSocketReturn(CommandServerSocket socket);
        IAsyncEnumerable<string> AsyncEnumerableReturn(int Value, int Ref);
        IAsyncEnumerable<string> AsyncEnumerableReturn();

        IAsyncEnumerable<string> AsyncEnumerableQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
        IAsyncEnumerable<string> AsyncEnumerableQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref);
#endif
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerKeepCallbackTaskController : IServerKeepCallbackTaskController
    {
        Task IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskSocket(CommandServerSocket socket, CommandServerKeepCallback Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTask(int Value, int Ref, CommandServerKeepCallback Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(CommandServerKeepCallback<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTask(CommandServerKeepCallback Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(CommandServerKeepCallbackCount Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref)
        {
            return Task.FromResult(ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref)));
        }
        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskSocketReturn(CommandServerSocket socket)
        {
            return Task.FromResult(ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor()));
        }
        async Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn(int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor(Value, Ref));
        }
        async Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskReturn()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor());
        }

        Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueue(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallback Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(queue.Key, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerKeepCallbackCount Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(queue.Key, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            return Task.FromResult(ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref)));
        }
        async Task<IEnumerable<string>> IServerKeepCallbackTaskController.EnumerableKeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref));
        }

#if NetStandard21
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableSocketReturn(CommandServerSocket socket, int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref)))
            {
                yield return value;
            }
        }
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableSocketReturn(CommandServerSocket socket)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor()))
            {
                yield return value;
            }
        }
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableReturn(int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor(Value, Ref)))
            {
                yield return value;
            }
        }
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableReturn()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor()))
            {
                yield return value;
            }
        }

        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref)))
            {
                yield return value;
            }
        }
        async IAsyncEnumerable<string> IServerKeepCallbackTaskController.AsyncEnumerableQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            foreach (string value in ServerKeepCallbackController.KeepCallbackEnumerable(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref)))
            {
                yield return value;
            }
        }
#endif
    }
}
