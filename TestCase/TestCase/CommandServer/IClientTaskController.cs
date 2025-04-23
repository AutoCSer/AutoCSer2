using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerTaskController))]
#endif
    public partial interface IClientTaskController
    {
        ReturnCommand<string> AsynchronousTaskReturnSocket(int Value, int Ref);
        ReturnCommand<string> AsynchronousTaskReturnSocket();
        ReturnCommand AsynchronousTaskSocket(int Value, int Ref);
        ReturnCommand AsynchronousTaskSocket();
        ReturnCommand<string> AsynchronousTaskReturn(int Value, int Ref);
        ReturnCommand<string> AsynchronousTaskReturn();
        ReturnCommand AsynchronousTask(int Value, int Ref);
        ReturnCommand AsynchronousTask();

        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturnSocket))]
        Task<string> AsynchronousTaskReturnSocketAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturnSocket))]
        Task<string> AsynchronousTaskReturnSocketAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskSocket))]
        Task AsynchronousTaskSocketAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskSocket))]
        Task AsynchronousTaskSocketAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturn))]
        Task<string> AsynchronousTaskReturnAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTaskReturn))]
        Task<string> AsynchronousTaskReturnAsync();
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTask))]
        Task AsynchronousTaskAsync(int Value, int Ref);
        [CommandClientMethod(MatchMethodName = nameof(IServerTaskController.AsynchronousTask))]
        Task AsynchronousTaskAsync();

        ReturnQueueCommand<string> TaskQueueReturnSocket(int queueKey, int Ref);
        ReturnQueueCommand TaskQueueSocket(int queueKey, int Ref);
        ReturnQueueCommand<string> TaskQueueReturn(int queueKey, int Ref);
        ReturnQueueCommand TaskQueue(int queueKey, int Ref);

        ReturnCommand<string> TaskQueueException(int queueKey, int Ref);
    }
    /// <summary>
    /// 命令客户端测试
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
            CommandClientReturnValue<string> returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ClientTaskController.AsynchronousTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientTaskController.AsynchronousTaskSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.AsynchronousTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.AsynchronousTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientTaskController.AsynchronousTask();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocketAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturnSocketAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            await client.ClientTaskController.AsynchronousTaskSocketAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            await client.ClientTaskController.AsynchronousTaskSocketAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.AsynchronousTaskReturnAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientTaskController.AsynchronousTaskReturnAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            await client.ClientTaskController.AsynchronousTaskAsync(clientSessionObject.Value, clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            await client.ClientTaskController.AsynchronousTaskAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.TaskQueueReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.TaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientTaskController.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientTaskController.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientTaskController.TaskQueueException(clientSessionObject.Value, clientSessionObject.Ref);
            if(returnValue.ReturnType != CommandClientReturnTypeEnum.ServerException || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
