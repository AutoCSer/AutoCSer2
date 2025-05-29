using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerConcurrencyReadQueueController
    {
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
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerConcurrencyReadQueueController : CommandServerBindContextController, IServerConcurrencyReadQueueController
    {
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref, out long Out)
        {
            return AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, int Value)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerConcurrencyReadQueueController.QueueReturn(CommandServerCallConcurrencyReadQueue queue)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, int Value, ref int Ref, out long Out)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, int Value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadQueue queue, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out);
        }
        void IServerConcurrencyReadQueueController.Queue(CommandServerCallConcurrencyReadWriteQueue queue) { }
    }
}
