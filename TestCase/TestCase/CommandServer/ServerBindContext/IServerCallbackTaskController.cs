using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(IsCodeGeneratorMethodEnum = false, IsAutoMethodIndex = true, TaskQueueMaxConcurrent = 1, IsCodeGeneratorClientInterface = false)]
    public interface IServerCallbackTaskController
    {
        Task CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task Callback(int Value, int Ref, CommandServerCallback Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task CallbackReturn(CommandServerCallback<string> Callback);
        Task Callback(CommandServerCallback Callback);

        Task CallbackQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        Task CallbackQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        Task CallbackQueueReturn(CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback);
        Task CallbackQueue(CommandServerCallTaskQueue queue, CommandServerCallback Callback);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerCallbackTaskController : CommandServerBindContextController, IServerCallbackTaskController
    {
        Task IServerCallbackTaskController.CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerCallbackTaskController.Callback(int Value, int Ref, CommandServerCallback Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        async Task IServerCallbackTaskController.CallbackReturn(CommandServerCallback<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task IServerCallbackTaskController.Callback(CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }

        Task IServerCallbackTaskController.CallbackQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerCallbackTaskController.CallbackQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        async Task IServerCallbackTaskController.CallbackQueueReturn(CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task IServerCallbackTaskController.CallbackQueue(CommandServerCallTaskQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
    }
}
