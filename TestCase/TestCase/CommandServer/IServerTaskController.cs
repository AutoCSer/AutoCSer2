using AutoCSer.Net;
using System;
using System.Threading.Tasks;

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
        internal static async Task TaskStart()
        {
            //await Task.Delay(1);
        }

        async Task<string> IServerTaskController.AsynchronousTaskReturnSocket(CommandServerSocket socket, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString();
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturnSocket(CommandServerSocket socket)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        async Task IServerTaskController.AsynchronousTaskSocket(CommandServerSocket socket, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
        }
        async Task IServerTaskController.AsynchronousTaskSocket(CommandServerSocket socket)
        {
            await ServerTaskController.TaskStart();
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturn(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString();
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturn()
        {
            await ServerTaskController.TaskStart();
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        async Task IServerTaskController.AsynchronousTask(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
        }
        async Task IServerTaskController.AsynchronousTask()
        {
            await ServerTaskController.TaskStart();
        }

        async Task<string> IServerTaskController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString();
        }
        async Task IServerTaskController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
        }
        async Task<string> IServerTaskController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString();
        }
        async Task IServerTaskController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
        }

        async Task<string> IServerTaskController.TaskQueueException(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            throw new Exception(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
    }
}
