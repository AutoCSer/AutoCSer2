using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
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

        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<CommandServerSendOnly> SendOnlyTaskSocket(CommandServerSocket socket, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskSocket(CommandServerSocket socket);
        Task<CommandServerSendOnly> SendOnlyTask(int Value, int Ref);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<CommandServerSendOnly> SendOnlyTask();

        Task<CommandServerSendOnly> SendOnlyTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerSendOnlyController : IServerSendOnlyController
    {
        internal static readonly System.Threading.SemaphoreSlim SendOnlyWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        private static bool isReleaseWaitLock;
        internal static Task WaitSendOnly()
        {
#if !AOT
            if (!CommandServer.IsAotClient)
            {
                isReleaseWaitLock = true;
                return SendOnlyWaitLock.WaitAsync();
            }
#endif
            return AutoCSer.Common.CompletedTask;
        }
        internal static void ReleaseWaitLock()
        {
            if (isReleaseWaitLock) SendOnlyWaitLock.Release();
        }

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
            ReleaseWaitLock();
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

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskSocket(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskSocket(CommandServerSocket socket)
        {
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask(int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask()
        {
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(queue.Key, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
    }
}
