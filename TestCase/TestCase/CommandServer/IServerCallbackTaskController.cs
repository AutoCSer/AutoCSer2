using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 1, IsCodeGeneratorClientInterface = false)]
    public interface IServerCallbackTaskController
    {
        Task CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        Task CallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback);
        Task CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback);
        Task CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback);
        Task CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback);
        Task Callback(int Value, int Ref, CommandServerCallback Callback);
        Task CallbackReturn(CommandServerCallback<string> Callback);
        Task Callback(CommandServerCallback Callback);

        Task CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        Task CallbackQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        Task CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback);
        Task CallbackQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, CommandServerCallback Callback);
        Task CallbackQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        Task CallbackQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        Task CallbackQueueReturn(CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback);
        Task CallbackQueue(CommandServerCallTaskQueue queue, CommandServerCallback Callback);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerCallbackTaskController : IServerCallbackTaskController
    {
        Task IServerCallbackTaskController.CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.Callback(int Value, int Ref, CommandServerCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackReturn(CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor().ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.Callback(CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }

        Task IServerCallbackTaskController.CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueueSocket(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueueSocket(CommandServerSocket socket, CommandServerCallTaskQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }

        Task IServerCallbackTaskController.CallbackQueueReturn(CommandServerCallTaskQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueue(CommandServerCallTaskLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueueReturn(CommandServerCallTaskLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor().ToString());
            return AutoCSer.Common.CompletedTask;
        }
        Task IServerCallbackTaskController.CallbackQueue(CommandServerCallTaskQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
            return AutoCSer.Common.CompletedTask;
        }
    }
}