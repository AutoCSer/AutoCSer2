using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerReadWriteQueueController
    {
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
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerReadWriteQueueController : CommandServerBindContextController, IServerReadWriteQueueController
    {
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, int Value)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallWriteQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerReadWriteQueueController.QueueReturn(CommandServerCallReadQueue queue)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, int Value, ref int Ref, out long Out)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, int Value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallReadQueue queue, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out);
        }
        void IServerReadWriteQueueController.Queue(CommandServerCallWriteQueue queue) { }
    }
}
