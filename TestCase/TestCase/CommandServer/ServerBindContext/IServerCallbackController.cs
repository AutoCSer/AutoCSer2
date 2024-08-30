using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerCallbackController
    {
        void CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback);
        void Callback(int Value, int Ref, CommandServerCallback Callback);
        void CallbackReturn(CommandServerCallback<string> Callback);
        void Callback(CommandServerCallback Callback);

        void CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback);
        void CallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback);
        void CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback);
        void CallbackQueue(CommandServerCallQueue queue, CommandServerCallback Callback);
    }
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerCallbackController : CommandServerBindContextController, IServerCallbackController
    {
        void IServerCallbackController.CallbackReturn(int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.Callback(int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackReturn(CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        void IServerCallbackController.Callback(CommandServerCallback Callback)
        {
            Callback.Callback();
        }

        void IServerCallbackController.CallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref).ToString());
        }
        void IServerCallbackController.CallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            Callback.Callback();
        }
        void IServerCallbackController.CallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerCallback<string> Callback)
        {
            Callback.Callback(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        void IServerCallbackController.CallbackQueue(CommandServerCallQueue queue, CommandServerCallback Callback)
        {
            Callback.Callback();
        }
    }
}
