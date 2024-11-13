using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientQueueController
    {
        CommandClientReturnValue<string> QueueReturnSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(int Value, ref int Ref);
        CommandClientReturnValue<string> QueueReturnSocket(int Value, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(int Value);
        CommandClientReturnValue<string> QueueReturnSocket(ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(ref int Ref);
        CommandClientReturnValue<string> QueueReturnSocket(out long Out);
        CommandClientReturnValue<string> QueueReturnSocket();
        CommandClientReturnValue QueueSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue QueueSocket(int Value, ref int Ref);
        CommandClientReturnValue QueueSocket(int Value, out long Out);
        CommandClientReturnValue QueueSocket(int Value);
        CommandClientReturnValue QueueSocket(ref int Ref, out long Out);
        CommandClientReturnValue QueueSocket(ref int Ref);
        CommandClientReturnValue QueueSocket(out long Out);
        CommandClientReturnValue QueueSocket();
        CommandClientReturnValue<string> QueueReturn(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturn(int Value, ref int Ref);
        CommandClientReturnValue<string> QueueReturn(int Value, out long Out);
        CommandClientReturnValue<string> QueueReturn(int Value);
        CommandClientReturnValue<string> QueueReturn(ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturn(ref int Ref);
        CommandClientReturnValue<string> QueueReturn(out long Out);
        CommandClientReturnValue<string> QueueReturn();
        CommandClientReturnValue Queue(int Value, ref int Ref, out long Out);
        CommandClientReturnValue Queue(int Value, ref int Ref);
        CommandClientReturnValue Queue(int Value, out long Out);
        CommandClientReturnValue Queue(int Value);
        CommandClientReturnValue Queue(ref int Ref, out long Out);
        CommandClientReturnValue Queue(ref int Ref);
        CommandClientReturnValue Queue(out long Out);
        CommandClientReturnValue Queue();
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientQueueController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static bool TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = client.ClientQueueController.QueueReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturnSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturnSocket(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturnSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturnSocket(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientQueueController.QueueReturnSocket(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientQueueController.QueueReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = client.ClientQueueController.QueueSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.QueueSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.QueueSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.QueueSocket(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.QueueSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.QueueSocket(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientQueueController.QueueSocket(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientQueueController.QueueSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientQueueController.QueueReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientQueueController.QueueReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientQueueController.QueueReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientQueueController.Queue(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientQueueController.Queue(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientQueueController.Queue();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
