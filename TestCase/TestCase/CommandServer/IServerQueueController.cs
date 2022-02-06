using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    public interface IServerQueueController
    {
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, out long Out);
        string QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue);
        void QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value);
        void QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref);
        void QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, out long Out);
        void QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue);
        string QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref);
        string QueueReturn(CommandServerCallQueue queue, int Value, out long Out);
        string QueueReturn(CommandServerCallQueue queue, int Value);
        string QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref, out long Out);
        string QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref);
        string QueueReturn(CommandServerCallQueue queue, out long Out);
        string QueueReturn(CommandServerCallQueue queue);
        void Queue(CommandServerCallQueue queue, int Value, ref int Ref, out long Out);
        void Queue(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref);
        void Queue(CommandServerCallQueue queue, int Value, out long Out);
        void Queue(CommandServerCallLowPriorityQueue queue, int Value);
        void Queue(CommandServerCallQueue queue, ref int Ref, out long Out);
        void Queue(CommandServerCallLowPriorityQueue queue, ref int Ref);
        void Queue(CommandServerCallQueue queue, out long Out);
        void Queue(CommandServerCallLowPriorityQueue queue);
    }
    /// <summary>
    /// 服务端测试接口
    /// </summary>
    internal sealed class ServerQueueController : IServerQueueController
    {
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(Value).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerQueueController.QueueReturnSocket(CommandServerSocket socket, CommandServerCallQueue queue)
        {
            return ((CommandServerSessionObject)socket.SessionObject).Xor().ToString();
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref, out Out);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value, out Out);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, int Value)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(Value);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(ref Ref);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue, out long Out)
        {
            ((CommandServerSessionObject)socket.SessionObject).Xor(out Out);
        }
        void IServerQueueController.QueueSocket(CommandServerSocket socket, CommandServerCallLowPriorityQueue queue) { }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, int Value, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, int Value)
        {
            return ServerSynchronousController.SessionObject.Xor(Value).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue)
        {
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, int Value, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, int Value, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, int Value)
        {
            ServerSynchronousController.SessionObject.Xor(Value);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue) { }
    }
}
