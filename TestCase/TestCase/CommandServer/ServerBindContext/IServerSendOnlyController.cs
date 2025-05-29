using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerSendOnlyController
    {
        CommandServerSendOnly SendOnly(int Value, int Ref);
        CommandServerSendOnly SendOnly();

        CommandServerSendOnly SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref);
        CommandServerSendOnly SendOnlyQueue(CommandServerCallQueue queue);

        Task<CommandServerSendOnly> SendOnlyTask(int Value, int Ref);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<CommandServerSendOnly> SendOnlyTask();

        Task<CommandServerSendOnly> SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerSendOnlyController : CommandServerBindContextController, IServerSendOnlyController
    {
        CommandServerSendOnly IServerSendOnlyController.SendOnly(int Value, int Ref)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref);
            return SendOnly();
        }
        public CommandServerSendOnly SendOnly()
        {
            AutoCSer.TestCase.ServerSendOnlyController.ReleaseWaitLock();
            return null;
        }

        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerSendOnlyController.SendOnlyQueue(CommandServerCallQueue queue)
        {
            return SendOnly();
        }

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask(int Value, int Ref)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTask()
        {
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }

        Task<CommandServerSendOnly> IServerSendOnlyController.SendOnlyTaskQueue(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(queue.Key, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
    }
}
