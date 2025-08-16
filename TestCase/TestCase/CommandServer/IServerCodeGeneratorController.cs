using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    [CommandServerControllerInterface(IsCodeGeneratorControllerAttribute = false)]
    public partial interface IServerCodeGeneratorController
    {
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<string> TaskReturnSocketInput(CommandServerSocket socket, int Value, int Ref);
        Task<string> TaskReturnSocket(CommandServerSocket socket);
        Task TaskSocketInput(CommandServerSocket socket, int Value, int Ref);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TaskSocket(CommandServerSocket socket);

        Task<string> TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref);
        Task TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref);

        void CallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        void CallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback);

        void CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback);
        void CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback);

        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TaskCallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        Task TaskCallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback);
        Task TaskCallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TaskCallbackSocket(CommandServerSocket socket, CommandServerCallback Callback);

        string QueueSocketInputReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value);
        string QueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue);
        void QueueSocketInput(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value);
        void QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue);

        string SynchronousSocketInputReturn(CommandServerSocket socket, int Value);
        string SynchronousSocketReturn(CommandServerSocket socket);
        void SynchronousSocketInput(CommandServerSocket socket, int Value);
        void SynchronousSocket(CommandServerSocket socket);

        CommandServerSendOnly SendOnlySocketInput(CommandServerSocket socket, int Value, int Ref);
        CommandServerSendOnly SendOnlySocket(CommandServerSocket socket);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task<CommandServerSendOnly> SendOnlyTaskSocketInput(CommandServerSocket socket, int Value, int Ref);
        Task<CommandServerSendOnly> SendOnlyTaskSocket(CommandServerSocket socket);

        void KeepCallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback);
        void KeepCallbackSocket(CommandServerSocket socket, CommandServerKeepCallback Callback);
        void KeepCallbackCountSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback);

        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task KeepCallbackTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        Task KeepCallbackTaskSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback);
        Task KeepCallbackTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task KeepCallbackTaskSocket(CommandServerSocket socket, CommandServerKeepCallback Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task KeepCallbackCountTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        Task KeepCallbackCountTaskSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        Task KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task KeepCallbackCountTaskSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback);

        void TwoStage‌CallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        void TwoStage‌CallbackCountSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        void TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);

        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        Task TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback);
        Task TwoStage‌CallbackCountTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
        [CommandServerMethod(IsSynchronousCallTask = true)]
        Task TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerCodeGeneratorController : IServerCodeGeneratorController
    {
        Task<string> IServerCodeGeneratorController.TaskReturnSocketInput(CommandServerSocket socket, int Value, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        Task<string> IServerCodeGeneratorController.TaskReturnSocket(CommandServerSocket socket)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        Task IServerCodeGeneratorController.TaskSocketInput(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.TaskSocket(CommandServerSocket socket)
        {
            return AutoCSer.Common.CompletedTask;
        }

        Task<string> IServerCodeGeneratorController.TaskQueueReturnSocket(CommandServerSocket socket, CommandServerCallTaskQueue<int> queue, int Ref)
        {
            return Task.FromResult(((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref).ToString());
        }
        Task IServerCodeGeneratorController.TaskQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue<int> queue, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(queue.Key, Ref);
            return AutoCSer.Common.CompletedTask;
        }

        void IServerCodeGeneratorController.CallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        void IServerCodeGeneratorController.CallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCodeGeneratorController.CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        void IServerCodeGeneratorController.CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback)
        {
            Callback.Callback();
        }

        Task IServerCodeGeneratorController.TaskCallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.TaskCallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.TaskCallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.TaskCallbackSocket(CommandServerSocket socket, CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }

        string IServerCodeGeneratorController.QueueSocketInputReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerCodeGeneratorController.QueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerCodeGeneratorController.QueueSocketInput(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value);
        }
        void IServerCodeGeneratorController.QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue) { }

        string IServerCodeGeneratorController.SynchronousSocketInputReturn(CommandServerSocket socket, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerCodeGeneratorController.SynchronousSocketReturn(CommandServerSocket socket)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerCodeGeneratorController.SynchronousSocketInput(CommandServerSocket socket, int Value)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value);
        }
        void IServerCodeGeneratorController.SynchronousSocket(CommandServerSocket socket) { }

        public static CommandServerSendOnly SendOnly()
        {
            ServerSendOnlyController.ReleaseWaitLock();
            return null;
        }
        CommandServerSendOnly IServerCodeGeneratorController.SendOnlySocketInput(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            return SendOnly();
        }
        CommandServerSendOnly IServerCodeGeneratorController.SendOnlySocket(CommandServerSocket socket)
        {
            return SendOnly();
        }
        Task<CommandServerSendOnly> IServerCodeGeneratorController.SendOnlyTaskSocketInput(CommandServerSocket socket, int Value, int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }
        Task<CommandServerSendOnly> IServerCodeGeneratorController.SendOnlyTaskSocket(CommandServerSocket socket)
        {
            SendOnly();
            return AutoCSer.CompletedTask<CommandServerSendOnly>.Default;
        }

        void IServerCodeGeneratorController.KeepCallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        void IServerCodeGeneratorController.KeepCallbackSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        void IServerCodeGeneratorController.KeepCallbackSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        void IServerCodeGeneratorController.KeepCallbackSocket(CommandServerSocket socketf, CommandServerKeepCallback Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        void IServerCodeGeneratorController.KeepCallbackCountSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            ServerKeepCallbackController.KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerCodeGeneratorController.KeepCallbackCountSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerCodeGeneratorController.KeepCallbackCountSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback)
        {
            ServerKeepCallbackController.KeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerCodeGeneratorController.KeepCallbackCountSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback)
        {
            ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }

        Task IServerCodeGeneratorController.KeepCallbackTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.KeepCallbackTaskSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.KeepCallbackTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallback<string> Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.KeepCallbackTaskSocket(CommandServerSocket socket, CommandServerKeepCallback Callback)
        {
            ServerKeepCallbackController.AutoKeepCallback(Callback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerCodeGeneratorController.KeepCallbackCountTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback);
        }
        async Task IServerCodeGeneratorController.KeepCallbackCountTaskSocketInput(CommandServerSocket socket, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        async Task IServerCodeGeneratorController.KeepCallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerKeepCallbackCount<string> Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback);
        }
        async Task IServerCodeGeneratorController.KeepCallbackCountTaskSocket(CommandServerSocket socket, CommandServerKeepCallbackCount Callback)
        {
            await ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        void IServerCodeGeneratorController.TwoStage‌CallbackSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
        }
        void IServerCodeGeneratorController.TwoStage‌CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
        }
        void IServerCodeGeneratorController.TwoStage‌CallbackCountSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }
        void IServerCodeGeneratorController.TwoStage‌CallbackCountSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback).AutoCSerExtensions().Catch();
        }

        Task IServerCodeGeneratorController.TwoStage‌CallbackTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCodeGeneratorController.TwoStage‌CallbackTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallback<string> KeepCallback)
        {
            ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
            return AutoCSer.Common.CompletedTask;
        }
        async Task IServerCodeGeneratorController.TwoStage‌CallbackCountTaskSocketInputReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref), Callback, KeepCallback);
        }
        async Task IServerCodeGeneratorController.TwoStage‌CallbackCountTaskSocketReturn(CommandServerSocket socket, CommandServerCallback<long> Callback, CommandServerKeepCallbackCount<string> KeepCallback)
        {
            await ServerTwoStage‌CallbackController.TwoStage‌Callback(((CommandServerSessionObject)socket.SessionObject).Xor(), Callback, KeepCallback);
        }
    }
#endif
}
