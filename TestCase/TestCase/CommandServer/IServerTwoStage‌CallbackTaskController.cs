using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerTwoStage‌CallbackTaskController
    {
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        Task TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        Task TwoStage‌CallbackTaskReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackTaskReturn(CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);

        Task TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackCountTaskReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        Task TwoStage‌CallbackCountTaskReturn(CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);

        Task TwoStage‌CallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        Task TwoStage‌CallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);

        Task TwoStage‌CallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        Task TwoStage‌CallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerTwoStage‌CallbackTaskController : IServerTwoStage‌CallbackTaskController
    {
        Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }

        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref), Callback, KeepCallback);
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(), Callback, KeepCallback);
        }

        Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref), Callback, KeepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskQueueReturn(CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref), Callback, KeepCallback);
        }

        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref), Callback, KeepCallback);
        }
        async Task IServerTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskQueueReturn(CommandServerCallTaskQueue<int> queue, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(ServerSynchronousController.SessionObject.Xor(queue.Key, Ref), Callback, KeepCallback);
        }
    }
#endif
}
