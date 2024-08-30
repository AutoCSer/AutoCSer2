using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 1)]
    public interface IServerTaskQueueController
    {
        Task<string> TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue);
        Task<string> TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref);
        Task TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue);
        Task TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue);
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue queue);
        Task TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerTaskQueueController : IServerTaskQueueController
    {
        Task<string> IServerTaskQueueController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        Task<string> IServerTaskQueueController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskQueueController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue)
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor().ToString());
        }
        Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue)
        {
            ServerSynchronousController.SessionObject.Xor();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
