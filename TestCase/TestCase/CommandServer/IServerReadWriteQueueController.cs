using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerReadWriteQueueController
    {
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue);
        void QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, int Value, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, int Value, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value);
        void QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue);
        string QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref);
        string QueueReturn(CommandServerCallReadQueue queue, int Value, out long Out);
        string QueueReturn(CommandServerCallReadQueue queue, int Value);
        string QueueReturn(CommandServerCallWriteQueue queue, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallWriteQueue queue, ref int Ref);
        string QueueReturn(CommandServerCallReadQueue queue, out long Out);
        string QueueReturn(CommandServerCallReadQueue queue);
        void Queue(CommandServerCallReadQueue queue, int Value, ref int Ref, out long Out);
        void Queue(CommandServerCallWriteQueue queue, int Value, ref int Ref);
        void Queue(CommandServerCallReadQueue queue, int Value, out long Out);
        void Queue(CommandServerCallWriteQueue queue, int Value);
        void Queue(CommandServerCallReadQueue queue, ref int Ref, out long Out);
        void Queue(CommandServerCallWriteQueue queue, ref int Ref);
        void Queue(CommandServerCallReadQueue queue, out long Out);
        void Queue(CommandServerCallWriteQueue queue);
    }
#if !AOT
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerReadWriteQueueController : IServerReadWriteQueueController
    {
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallReadQueue queue)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, int Value, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, int Value)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallReadQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(out Out);
        }
        void IServerReadWriteQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallWriteQueue queue) { }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, int Value, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, int Value)
        {
            return ServerSynchronousController.SessionObject.Xor(Value).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue)
        {
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, int Value, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, int Value, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, int Value, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, int Value)
        {
            ServerSynchronousController.SessionObject.Xor(Value);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue) { }
    }
#endif
}
