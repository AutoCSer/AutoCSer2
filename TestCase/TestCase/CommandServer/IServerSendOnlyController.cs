using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerSendOnlyController
    {
        CommandServerSendOnly SendOnlySocket(CommandServerSocket socket, int Value, int Ref);
        CommandServerSendOnly SendOnlySocket(CommandServerSocket socket);
        CommandServerSendOnly SendOnly(int Value, int Ref);
        CommandServerSendOnly SendOnly();

        CommandServerSendOnly SendOnlyQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref);
        CommandServerSendOnly SendOnlyQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue);
        CommandServerSendOnly SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref);
        CommandServerSendOnly SendOnlyQueue(CommandServerCallQueue queue);

        Task<CommandServerSendOnly> SendOnlyTaskSocket(CommandServerSocket socket, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskSocket(CommandServerSocket socket);
        Task<CommandServerSendOnly> SendOnlyTask(int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTask();

        Task<CommandServerSendOnly> SendOnlyTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerSendOnlyController : IServerSendOnlyController
    {
        internal static readonly System.Threading.SemaphoreSlim SendOnlyWaitLock = new System.Threading.SemaphoreSlim(0, 1);

        CommandServerSendOnly IServerSendOnlyController.SendOnlySocket(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlySocket(CommandServerSocket socket)
        {
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnly(int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return SendOnly();
        }
        public CommandServerSendOnly SendOnly()
        {
            SendOnlyWaitLock.Release();
            return null;
        }

        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue)
        {
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallQueue queue)
        {
            return SendOnly();
        }

        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskSocket(CommandServerSocket socket, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskSocket(CommandServerSocket socket)
        {
            await ServerTaskController.TaskStart();
            return SendOnly();
        }
        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask(int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return SendOnly();
        }
        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask()
        {
            await ServerTaskController.TaskStart();
            return SendOnly();
        }

        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        async Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return SendOnly();
        }
    }
}
