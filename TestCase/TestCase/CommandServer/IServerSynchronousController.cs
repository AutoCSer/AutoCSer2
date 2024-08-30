using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerSynchronousController
    {
        string SynchronousReturnSocket(CommandServerSocket socket, int Value, ref int Ref, out long Out);
        string SynchronousReturnSocket(CommandServerSocket socket, int Value, ref int Ref);
        string SynchronousReturnSocket(CommandServerSocket socket, int Value, out long Out);
        string SynchronousReturnSocket(CommandServerSocket socket, int Value);
        string SynchronousReturnSocket(CommandServerSocket socket, ref int Ref, out long Out);
        string SynchronousReturnSocket(CommandServerSocket socket, ref int Ref);
        string SynchronousReturnSocket(CommandServerSocket socket, out long Out);
        string SynchronousReturnSocket(CommandServerSocket socket);
        void SynchronousSocket(CommandServerSocket socket, int Value, ref int Ref, out long Out);
        void SynchronousSocket(CommandServerSocket socket, int Value, ref int Ref);
        void SynchronousSocket(CommandServerSocket socket, int Value, out long Out);
        CommandServerVerifyStateEnum SynchronousSocket(CommandServerSocket socket, int Value);
        void SynchronousSocket(CommandServerSocket socket, ref int Ref, out long Out);
        void SynchronousSocket(CommandServerSocket socket, ref int Ref);
        void SynchronousSocket(CommandServerSocket socket, out long Out);
        void SynchronousSocket(CommandServerSocket socket);
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
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerSynchronousController : IServerSynchronousController
    {
        public static CommandServerSessionObject SessionObject { get;private set; }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, int Value, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, int Value, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturnSocket(CommandServerSocket socket)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, int Value, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out);
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, int Value, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out);
        }
        CommandServerVerifyStateEnum IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, int Value)
        {
            socket.SessionObject = SessionObject = new CommandServerSessionObject();
            SessionObject.Xor(Value);
            return CommandServerVerifyStateEnum.Success;
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref);
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(out Out);
        }
        void IServerSynchronousController.SynchronousSocket(CommandServerSocket socket) { }
        string IServerSynchronousController.SynchronousReturn(int Value, ref int Ref, out long Out)
        {
            return SessionObject.Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value, ref int Ref)
        {
            return SessionObject.Xor(Value, ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value, out long Out)
        {
            return SessionObject.Xor(Value, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(int Value)
        {
            return SessionObject.Xor(Value).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(ref int Ref, out long Out)
        {
            return SessionObject.Xor(ref Ref, out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(ref int Ref)
        {
            return SessionObject.Xor(ref Ref).ToString();
        }
        string IServerSynchronousController.SynchronousReturn(out long Out)
        {
            return SessionObject.Xor(out Out).ToString();
        }
        string IServerSynchronousController.SynchronousReturn()
        {
            return SessionObject.Xor().ToString();
        }
        void IServerSynchronousController.Synchronous(int Value, ref int Ref, out long Out)
        {
            SessionObject.Xor(Value, ref Ref, out Out);
        }
        void IServerSynchronousController.Synchronous(int Value, ref int Ref)
        {
            SessionObject.Xor(Value, ref Ref);
        }
        void IServerSynchronousController.Synchronous(int Value, out long Out)
        {
            SessionObject.Xor(Value, out Out);
        }
        void IServerSynchronousController.Synchronous(int Value)
        {
            SessionObject.Xor(Value);
        }
        void IServerSynchronousController.Synchronous(ref int Ref, out long Out)
        {
            SessionObject.Xor(ref Ref, out Out);
        }
        void IServerSynchronousController.Synchronous(ref int Ref)
        {
            SessionObject.Xor(ref Ref);
        }
        void IServerSynchronousController.Synchronous(out long Out)
        {
            SessionObject.Xor(out Out);
        }
        void IServerSynchronousController.Synchronous()
        {
            SessionObject.Xor();
        }
    }
}
