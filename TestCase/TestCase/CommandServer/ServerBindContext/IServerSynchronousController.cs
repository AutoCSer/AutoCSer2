using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerSynchronousController
    {
        string SynchronousReturn(int Value, ref int Ref, out long Out);
        string SynchronousReturn(int Value, ref int Ref);
        string SynchronousReturn(int Value, out long Out);
        string SynchronousReturn(int Value);
        string SynchronousReturn(ref int Ref, out long Out);
        string SynchronousReturn(ref int Ref);
        string SynchronousReturn(out long Out);
        string SynchronousReturn();
        void Synchronous(int Value, ref int Ref, out long Out);
        void Synchronous(int Value, ref int Ref);
        void Synchronous(int Value, out long Out);
        void Synchronous(int Value);
        void Synchronous(ref int Ref, out long Out);
        void Synchronous(ref int Ref);
        void Synchronous(out long Out);
        void Synchronous();
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerSynchronousController : CommandServerBindContextController, IServerSynchronousController
    {
        string IServerSynchronousController.SynchronousReturn(int Value, ref int Ref, out long Out)
        {
            return AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value, ref int Ref)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value, out long Out)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(Value).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(ref int Ref, out long Out)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(ref int Ref)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(out long Out)
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn()
        {
            return((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        void IServerSynchronousController.Synchronous(int Value, ref int Ref, out long Out)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out);
        }
        void IServerSynchronousController.Synchronous(int Value, ref int Ref)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerSynchronousController.Synchronous(int Value, out long Out)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out);
        }
        void IServerSynchronousController.Synchronous(int Value)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(Value);
        }
        void IServerSynchronousController.Synchronous(ref int Ref, out long Out)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerSynchronousController.Synchronous(ref int Ref)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref);
        }
        void IServerSynchronousController.Synchronous(out long Out)
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out);
        }
        void IServerSynchronousController.Synchronous()
        {
           ((CommandServerSessionObject)Socket.SessionObject).Xor();
        }
    }
#endif
}
