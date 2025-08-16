using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerTaskController
    {
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> AsynchronousTaskReturnSocket(CommandServerSocket socket, int Value, int Ref);
        Task<string> AsynchronousTaskReturnSocket(CommandServerSocket socket);
        Task AsynchronousTaskSocket(CommandServerSocket socket, int Value, int Ref);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task AsynchronousTaskSocket(CommandServerSocket socket);
        Task<string> AsynchronousTaskReturn(int Value, int Ref);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> AsynchronousTaskReturn();
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task AsynchronousTask(int Value, int Ref);
        Task AsynchronousTask();

        Task<string> TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref);
        Task TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue<int> queue, int Ref);

        Task<string> TaskQueueException(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerTaskController : IServerTaskController
    {
        Task<string> IServerTaskController.AsynchronousTaskReturnSocket(CommandServerSocket socket, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task<string> IServerTaskController.AsynchronousTaskReturnSocket(CommandServerSocket socket)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        Task IServerTaskController.AsynchronousTaskSocket(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskController.AsynchronousTaskSocket(CommandServerSocket socket)
        {
            return AutoCSer.Common.CompletedTask;
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturn(int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString();
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturn()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        async Task IServerTaskController.AsynchronousTask(int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
        }
        async Task IServerTaskController.AsynchronousTask()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
        }

        Task<string> IServerTaskController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref).ToString());
        }
        Task IServerTaskController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        async Task<string> IServerTaskController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerSynchronousController.SessionObject.Xor(queue.Key, Ref).ToString();
        }
        async Task IServerTaskController.TaskQueue(CommandServerCallTaskQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(queue.Key, Ref);
        }

        async Task<string> IServerTaskController.TaskQueueException(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            throw new AutoCSer.Log.IgnoreException(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref).ToString());
        }
    }
#endif
}
