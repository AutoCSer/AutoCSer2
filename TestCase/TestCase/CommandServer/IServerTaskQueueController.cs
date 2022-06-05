using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    [AutoCSer.Net.CommandServerController(TaskQueueMaxConcurrent = 1)]
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
        async Task<string> IServerTaskQueueController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        async Task<string> IServerTaskQueueController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString();
        }
        async Task IServerTaskQueueController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor();
        }
        async Task IServerTaskQueueController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
        }
        async Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue)
        {
            await ServerTaskController.TaskStart();
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        async Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            return ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString();
        }
        async Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor();
        }
        async Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await ServerTaskController.TaskStart();
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
        }
    }
}
