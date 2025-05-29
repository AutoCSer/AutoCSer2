using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerTaskController))]
#endif
    public partial interface IClientTaskController
    {
        ReturnCommand<string> AsynchronousTaskReturn(int Value, int Ref);
        ReturnCommand<string> AsynchronousTaskReturn();
        ReturnCommand AsynchronousTask(int Value, int Ref);
        ReturnCommand AsynchronousTask();

        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturn))]
        Task<string> AsynchronousTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturn))]
        Task<string> AsynchronousTaskReturnAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTask))]
        Task AsynchronousTaskAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTask))]
        Task AsynchronousTaskAsync();

        ReturnQueueCommand<string> TaskQueueReturn(int queueKey, int Ref);
        ReturnQueueCommand TaskQueue(int queueKey, int Ref);

        ReturnCommand<string> TaskQueueException(int queueKey, int Ref);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientTaskController
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
            CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturn();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ServerBindContextClientTaskController.AsynchronousTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ServerBindContextClientTaskController.AsynchronousTask();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturnAsync();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            await client.ServerBindContextClientTaskController.AsynchronousTaskAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            await client.ServerBindContextClientTaskController.AsynchronousTaskAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ServerBindContextClientTaskController.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ServerBindContextClientTaskController.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextClientTaskController.TaskQueueException(clientSessionObject.Value, clientSessionObject.Ref);
            if(returnValue.ReturnType != CommandClientReturnTypeEnum.ServerException || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
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
                if (client?.ServerBindContextClientTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                string returnValue = await client.ServerBindContextClientTaskController.AsynchronousTaskReturnAsync(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (returnValue == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskController.TaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientTaskController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                CommandClientReturnValue<string> returnValue = await client.ServerBindContextClientTaskController.TaskQueueException(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (returnValue.ReturnType != CommandClientReturnTypeEnum.ServerException)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
