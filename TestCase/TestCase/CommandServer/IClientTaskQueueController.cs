using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientTaskQueueController
    {
        ReturnQueueCommand<string> TaskQueueReturnSocket();
        ReturnQueueCommand<string> TaskQueueReturnSocket(int Value, int Ref);
        ReturnQueueCommand TaskQueueSocket();
        ReturnQueueCommand TaskQueueSocket(int Value, int Ref);
        ReturnQueueCommand<string> TaskQueueReturn();
        ReturnQueueCommand<string> TaskQueueReturn(int Value, int Ref);
        ReturnQueueCommand TaskQueue();
        ReturnQueueCommand TaskQueue(int Value, int Ref);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientTaskQueueController
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
            CommandClientReturnValue<string> returnValue = await client.ClientTaskQueueController.TaskQueueReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await client.ClientTaskQueueController.TaskQueueReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ClientTaskQueueController.TaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await client.ClientTaskQueueController.TaskQueueSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskQueueController.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await client.ClientTaskQueueController.TaskQueueReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskQueueController.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await client.ClientTaskQueueController.TaskQueue();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
