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
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> AsynchronousTaskReturn();
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task AsynchronousTask(int Value, int Ref);
        Task AsynchronousTask();

        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue<int> queue, int Ref);

        Task<string> TaskQueueException(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerTaskController : CommandServerBindContextController, IServerTaskController
    {
        Task<string> IServerTaskController.AsynchronousTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref).ToString());
        }
        async Task<string> IServerTaskController.AsynchronousTaskReturn()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        async Task IServerTaskController.AsynchronousTask(int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
        }
        Task IServerTaskController.AsynchronousTask()
        {
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerTaskController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(queue.Key, Ref).ToString());
        }
        async Task IServerTaskController.TaskQueue(CommandServerCallTaskQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ((CommandServerSessionObject)Socket.SessionObject).Xor(queue.Key, Ref);
        }

        async Task<string> IServerTaskController.TaskQueueException(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            throw new AutoCSer.Log.IgnoreException(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(queue.Key, Ref).ToString());
        }
    }
#endif
}
