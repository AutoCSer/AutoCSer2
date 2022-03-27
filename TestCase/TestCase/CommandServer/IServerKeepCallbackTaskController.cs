using AutoCSer.Net;
using System;
using System.Threading.Tasks;

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

        Task KeepCallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);

        Task KeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerKeepCallbackTaskController : IServerKeepCallbackTaskController
    {
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskSocket(CommandServerSocket socket, CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTask(int Value, int Ref, CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskReturn(CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTask(CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocket(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskReturn(CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTask(CommandServerKeepCallbackCount Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            await ServerTaskController.TaskStart();
            ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackTaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback);
        }
        async Task IServerKeepCallbackTaskController.KeepCallbackCountTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            await ServerTaskController.TaskStart();
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
    }
}
