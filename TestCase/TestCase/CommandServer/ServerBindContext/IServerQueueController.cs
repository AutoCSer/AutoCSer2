using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IServerQueueController
    {
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
#if !AOT
    /// <summary>
    /// 服务端测试接口（套接字上下文绑定服务端）
    /// </summary>
    internal sealed class ServerQueueController : CommandServerBindContextController, IServerQueueController
    {
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref, out long Out)
        {
            return AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, int Value, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, int Value)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallLowPriorityQueue queue, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out).ToString();
        }
        string IServerQueueController.QueueReturn(CommandServerCallQueue queue)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, int Value, ref int Ref, out long Out)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, int Value, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, int Value, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, int Value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref);
        }
        void IServerQueueController.Queue(CommandServerCallQueue queue, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out);
        }
        void IServerQueueController.Queue(CommandServerCallLowPriorityQueue queue) { }
    }
#endif
}
