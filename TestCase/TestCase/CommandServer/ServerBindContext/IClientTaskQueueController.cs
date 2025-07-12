using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
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
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskQueueController.TaskQueueReturn(0, 0);
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextClientTaskQueueController.TaskQueueReturn();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue returnType = await client.ServerBindContextClientTaskQueueController.TaskQueue(0, 0);
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ServerBindContextClientTaskQueueController.TaskQueue();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
        /// <summary>
        /// 短连接命令客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> ShortLinkTestCase()
        {
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientTaskQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskQueueController.TaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientTaskQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                CommandClientReturnValue returnType = await client.ServerBindContextClientTaskQueueController.TaskQueue(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
