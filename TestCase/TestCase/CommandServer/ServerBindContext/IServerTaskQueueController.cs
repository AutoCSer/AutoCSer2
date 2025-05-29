using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 1)]
    public interface IServerTaskQueueController
    {
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue);
        Task<string> TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref);
        Task TaskQueue(CommandServerCallTaskQueue queue);
        Task TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerTaskQueueController : CommandServerBindContextController, IServerTaskQueueController
    {
        async Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref);
        }
    }
}
