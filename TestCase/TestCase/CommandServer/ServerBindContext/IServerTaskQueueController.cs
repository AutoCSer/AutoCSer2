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
        Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task<string> IServerTaskQueueController.TaskQueueReturn(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTaskQueueController.TaskQueue(CommandServerCallTaskQueue queue, int Value, int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
