using AutoCSer.Net;
using AutoCSer.Extensions;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerKeepCallbackController
    {
        void KeepCallbackReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallback(int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackReturn(CommandServerKeepCallback<string> Callback);
        void KeepCallback(CommandServerKeepCallback Callback);

        void KeepCallbackCountReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCount(int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountReturn(CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCount(CommandServerKeepCallbackCount Callback);

        void KeepCallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback);
        void KeepCallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback);
        void KeepCallbackQueue(CommandServerCallQueue queue, CommandServerKeepCallback Callback);

        void KeepCallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueue(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback);
        void KeepCallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback);
        void KeepCallbackCountQueue(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerKeepCallbackController : CommandServerBindContextController, IServerKeepCallbackController
    {
        void IServerKeepCallbackController.KeepCallbackReturn(int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallback(int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackReturn(CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallback(CommandServerKeepCallback Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        void IServerKeepCallbackController.KeepCallbackCountReturn(int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCount(int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountReturn(CommandServerKeepCallbackCount<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCount(CommandServerKeepCallbackCount Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }

        void IServerKeepCallbackController.KeepCallbackQueueReturn(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueue(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallback Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueueReturn(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallback<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback);
        }
        void IServerKeepCallbackController.KeepCallbackQueue(CommandServerCallQueue queue, CommandServerKeepCallback Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.AutoKeepCallback(Callback);
        }

        void IServerKeepCallbackController.KeepCallbackCountQueueReturn(CommandServerCallLowPriorityQueue queue, int Value, int Ref, CommandServerKeepCallbackCount<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueue(CommandServerCallQueue queue, int Value, int Ref, CommandServerKeepCallbackCount Callback)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, Ref);
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueueReturn(CommandServerCallQueue queue, CommandServerKeepCallbackCount<string> Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(((CommandServerSessionObject)Socket.SessionObject).Xor(), Callback).AutoCSerExtensions().Catch();
        }
        void IServerKeepCallbackController.KeepCallbackCountQueue(CommandServerCallLowPriorityQueue queue, CommandServerKeepCallbackCount Callback)
        {
            AutoCSer.TestCase.ServerKeepCallbackController.KeepCallback(Callback).AutoCSerExtensions().Catch();
        }
    }
#endif
}
