using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientSendOnlyController
    {
        SendOnlyCommand SendOnlySocket(int Value, int Ref);
        SendOnlyCommand SendOnlySocket();
        SendOnlyCommand SendOnly(int Value, int Ref);
        SendOnlyCommand SendOnly();

        SendOnlyCommand SendOnlyQueueSocket(int Value, int Ref);
        SendOnlyCommand SendOnlyQueueSocket();
        SendOnlyCommand SendOnlyQueue(int Value, int Ref);
        SendOnlyCommand SendOnlyQueue();

        SendOnlyCommand SendOnlyTaskSocket(int Value, int Ref);
        SendOnlyCommand SendOnlyTaskSocket();
        SendOnlyCommand SendOnlyTask(int Value, int Ref);
        SendOnlyCommand SendOnlyTask();

        SendOnlyCommand SendOnlyTaskQueueSocket(int queueKey, int Ref);
        SendOnlyCommand SendOnlyTaskQueue(int queueKey, int Ref);
    }
    /// <summary>
    /// 命令客户端测试
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
            if (!await client.ClientSendOnlyController.SendOnlySocket(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnlySocket())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnly(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnly())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyQueueSocket(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnlyQueueSocket())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyQueue(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnlyQueue())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyTaskSocket(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnlyTaskSocket())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyTask(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientSendOnlyController.SendOnlyTask())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientSendOnlyController.SendOnlyTaskQueue(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.SendOnlyWaitLock.WaitAsync();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
