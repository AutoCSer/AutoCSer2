using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientSendOnlyController
    {
        SendOnlyCommand SendOnly(int Value, int Ref);
        SendOnlyCommand SendOnly();

        SendOnlyCommand SendOnlyQueue(int Value, int Ref);
        SendOnlyCommand SendOnlyQueue();

        SendOnlyCommand SendOnlyTask(int Value, int Ref);
        SendOnlyCommand SendOnlyTask();

        SendOnlyCommand SendOnlyTaskQueue(int Value, int Ref);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal static class ClientSendOnlyController
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
            if (!await client.ServerBindContextClientSendOnlyController.SendOnly(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnly())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyQueue(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyQueue())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTask(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTask())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTaskQueue(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
