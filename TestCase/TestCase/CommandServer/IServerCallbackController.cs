using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerCallbackController
    {
        void CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback);
        void CallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback);
        void CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback);
        void CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback);
        void CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback);
        void Callback(int Value, int Ref, CommandServerCallback Callback);
        void CallbackReturn(CommandServerCallback<string> Callback);
        void Callback(CommandServerCallback Callback);

        void CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        void CallbackQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        void CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback);
        void CallbackQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerCallback Callback);
        void CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        void CallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        void CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback);
        void CallbackQueue(CommandServerCallQueue queue, CommandServerCallback Callback);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerCallbackController : IServerCallbackController
    {
        void IServerCallbackController.CallbackSocketReturn(CommandServerSocket socket, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.CallbackSocket(CommandServerSocket socket, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackSocketReturn(CommandServerSocket socket, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        void IServerCallbackController.CallbackSocket(CommandServerSocket socket, CommandServerCallback Callback)
        {
            Callback.Callback();
        }
        void IServerCallbackController.CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.Callback(int Value, int Ref, CommandServerCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackReturn(CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor().ToString());
        }
        void IServerCallbackController.Callback(CommandServerCallback Callback)
        {
            Callback.Callback();
        }

        void IServerCallbackController.CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.CallbackQueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackQueueSocketReturn(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)socket.SessionObject).Xor().ToString());
        }
        void IServerCallbackController.CallbackQueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
        }

        void IServerCallbackController.CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.CallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            Callback.Callback(ServerSynchronousController.SessionObject.Xor().ToString());
        }
        void IServerCallbackController.CallbackQueue(CommandServerCallQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
        }
    }
}
