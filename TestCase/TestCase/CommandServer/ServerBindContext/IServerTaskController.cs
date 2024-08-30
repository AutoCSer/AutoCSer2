using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerTaskController
    {
        Task<string> AsynchronousTaskReturn(int Value, int Ref);
        Task<string> AsynchronousTaskReturn();
        Task AsynchronousTask(int Value, int Ref);
        Task AsynchronousTask();

        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref);

        Task<string> TaskQueueException(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerTaskController : CommandServerBindContextController, IServerTaskController
    {
        Task<string> IServerTaskController.AsynchronousTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task<string> IServerTaskController.AsynchronousTaskReturn()
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task IServerTaskController.AsynchronousTask(int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskController.AsynchronousTask()
        {
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskController.TaskQueueException(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            throw new AutoCSer.Log.IgnoreException(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
    }
}
