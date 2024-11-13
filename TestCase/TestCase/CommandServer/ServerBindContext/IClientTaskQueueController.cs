using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientTaskQueueController
    {
        ReturnQueueCommand<string> TaskQueueReturn();
        ReturnQueueCommand<string> TaskQueueReturn(int Value, int Ref);
        ReturnQueueCommand TaskQueue();
        ReturnQueueCommand TaskQueue(int Value, int Ref);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
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
            CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskQueueController.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextClientTaskQueueController.TaskQueueReturn();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ServerBindContextClientTaskQueueController.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ServerBindContextClientTaskQueueController.TaskQueue();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
