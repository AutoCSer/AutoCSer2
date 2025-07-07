using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(IsCodeGeneratorMethodEnum = false, IsAutoMethodIndex = true, TaskQueueMaxConcurrent = 1)]
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
        async Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        async Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString();
        }
        async Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor();
        }
        async Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
        }
    }
}
