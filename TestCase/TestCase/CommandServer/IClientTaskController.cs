using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientTaskController
    {
        ReturnCommand<string> AsynchronousTaskReturnSocket(int Value, int Ref);
        ReturnCommand<string> AsynchronousTaskReturnSocket();
        ReturnCommand AsynchronousTaskSocket(int Value, int Ref);
        ReturnCommand AsynchronousTaskSocket();
        ReturnCommand<string> AsynchronousTaskReturn(int Value, int Ref);
        ReturnCommand<string> AsynchronousTaskReturn();
        ReturnCommand AsynchronousTask(int Value, int Ref);
        ReturnCommand AsynchronousTask();

        ReturnQueueCommand<string> TaskQueueReturnSocket(int Value, int Ref);
        ReturnQueueCommand TaskQueueSocket(int Value, int Ref);
        ReturnQueueCommand<string> TaskQueueReturn(int Value, int Ref);
        ReturnQueueCommand TaskQueue(int Value, int Ref);

        ReturnCommand<string> TaskQueueException(int Value, int Ref);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientTaskController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ClientTaskController.AsynchronousTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await client.ClientTaskController.AsynchronousTaskSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.AsynchronousTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.AsynchronousTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await client.ClientTaskController.AsynchronousTask();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.TaskQueueReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.TaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnValue = await client.ClientTaskController.TaskQueueException(clientSessionObject.Value, clientSessionObject.Ref);
            if(returnValue.ReturnType != CommandClientReturnType.ServerException || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }
            return true;
        }
    }
}
