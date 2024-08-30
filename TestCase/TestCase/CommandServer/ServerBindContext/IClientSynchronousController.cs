using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientSynchronousController
    {
        CommandClientReturnValue<string> SynchronousReturn(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(int Value, ref int Ref);
        CommandClientReturnValue<string> SynchronousReturn(int Value, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(int Value);
        CommandClientReturnValue<string> SynchronousReturn(ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(ref int Ref);
        CommandClientReturnValue<string> SynchronousReturn(out long Out);
        CommandClientReturnValue<string> SynchronousReturn();
        CommandClientReturnValue Synchronous(int Value, ref int Ref, out long Out);
        CommandClientReturnValue Synchronous(int Value, ref int Ref);
        CommandClientReturnValue Synchronous(int Value, out long Out);
        CommandClientReturnValue Synchronous(int Value);
        CommandClientReturnValue Synchronous(ref int Ref, out long Out);
        CommandClientReturnValue Synchronous(ref int Ref);
        CommandClientReturnValue Synchronous(out long Out);
        CommandClientReturnValue Synchronous();
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal static class ClientSynchronousController
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
            CommandClientReturnValue<string> returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            return true;
        }
    }
}
