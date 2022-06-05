using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 服务端 Task 队列客户端测试接口
    /// </summary>
    public interface IClientTaskQueueContextController
    {
        ReturnCommand<string> TaskQueueReturn();
        ReturnCommand<string> TaskQueueReturn(int Value, int Ref);
        ReturnCommand TaskQueue();
        ReturnCommand TaskQueue(int Value, int Ref);

        ReturnCommand<string> TaskQueueLowPriorityReturn();
        ReturnCommand<string> TaskQueueLowPriorityReturn(int Value, int Ref);
        ReturnCommand TaskQueueLowPriority();
        ReturnCommand TaskQueueLowPriority(int Value, int Ref);
    }
    /// <summary>
    /// 服务端 Task 队列命令客户端测试
    /// </summary>
    internal static class ClientTaskQueueContextController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            IClientTaskQueueContextController controller = client.ClientTaskQueueContextController.CreateQueueController(1);

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = await controller.TaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await controller.TaskQueueReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await controller.TaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await controller.TaskQueue();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await controller.TaskQueueLowPriorityReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = await controller.TaskQueueLowPriorityReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await controller.TaskQueueLowPriority(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = await controller.TaskQueueLowPriority();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
