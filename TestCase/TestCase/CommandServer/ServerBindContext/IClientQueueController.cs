using AutoCSer.Net;
using System;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientQueueController
    {
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
    /// 命令客户端测试（套接字上下文绑定服务端）
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
            CommandClientReturnValue<string> returnValue = client.ServerBindContextClientQueueController.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientQueueController.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientQueueController.QueueReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientQueueController.QueueReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientQueueController.QueueReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientQueueController.QueueReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextClientQueueController.QueueReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextClientQueueController.QueueReturn();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = client.ServerBindContextClientQueueController.Queue(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientQueueController.Queue(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientQueueController.Queue(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientQueueController.Queue(clientSessionObject.Value);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientQueueController.Queue(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientQueueController.Queue(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientQueueController.Queue(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientQueueController.Queue();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
