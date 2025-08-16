using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerConcurrencyReadQueueController
    {
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue);
        string QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref);
        string QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out);
        string QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value);
        string QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref);
        string QueueReturn(CommandServerCallConcurrencyReadQueue queue, out long Out);
        string QueueReturn(CommandServerCallConcurrencyReadQueue queue);
        void Queue(CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref, out long Out);
        void Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref);
        void Queue(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out);
        void Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value);
        void Queue(CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out);
        void Queue(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref);
        void Queue(CommandServerCallConcurrencyReadQueue queue, out long Out);
        void Queue(CommandServerCallConcurrencyReadWriteQueue queue);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerConcurrencyReadQueueController : IServerConcurrencyReadQueueController
    {
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, int Value)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(out Out);
        }
        void IServerConcurrencyReadQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallConcurrencyReadWriteQueue queue) { }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value)
        {
            return ServerSynchronousController.SessionObject.Xor(Value).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue)
        {
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value)
        {
            ServerSynchronousController.SessionObject.Xor(Value);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue) { }
    }
#endif
}
