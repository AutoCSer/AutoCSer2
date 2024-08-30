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
        Task<string> AsynchronousTaskReturnSocket(CommandServerSocket socket, int Value, int Ref);
        Task<string> AsynchronousTaskReturnSocket(CommandServerSocket socket);
        Task AsynchronousTaskSocket(CommandServerSocket socket, int Value, int Ref);
        Task AsynchronousTaskSocket(CommandServerSocket socket);
        Task<string> AsynchronousTaskReturn(int Value, int Ref);
        Task<string> AsynchronousTaskReturn();
        Task AsynchronousTask(int Value, int Ref);
        Task AsynchronousTask();

        Task<string> TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
        Task TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref);

        Task<string> TaskQueueException(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
    }
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
        Task<string> IServerTaskController.AsynchronousTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        Task<string> IServerTaskController.AsynchronousTaskReturn()
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor().ToString());
        }
        Task IServerTaskController.AsynchronousTask(int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskController.AsynchronousTask()
        {
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task<string> IServerTaskController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        Task IServerTaskController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskController.TaskQueueException(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            throw new AutoCSer.Log.IgnoreException(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
    }
}
