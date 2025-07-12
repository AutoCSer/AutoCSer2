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
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerSendOnlyController), true)]
#endif
    public partial interface IClientSendOnlyController
    {
        SendOnlyCommand SendOnly(int Value, int Ref);
        SendOnlyCommand SendOnly();

        SendOnlyCommand SendOnlyQueue(int Value, int Ref);
        SendOnlyCommand SendOnlyQueue();

        SendOnlyCommand SendOnlyTask(int Value, int Ref);
        SendOnlyCommand SendOnlyTask();

        SendOnlyCommand SendOnlyTaskQueue(int queueKey, int Ref);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientSendOnlyController
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
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnly())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
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
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyQueue())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
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
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTask())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
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
            await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!AutoCSer.TestCase.ServerSendOnlyController.CheckCount())
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
            if (await client.ServerBindContextClientSendOnlyController.SendOnly(0, 0))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (await client.ServerBindContextClientSendOnlyController.SendOnly())
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
                if (client?.ServerBindContextClientSendOnlyController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await client.ServerBindContextClientSendOnlyController.SendOnly(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next()))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientSendOnlyController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await client.ServerBindContextClientSendOnlyController.SendOnlyQueue(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next()))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientSendOnlyController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTask(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next()))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientSendOnlyController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await client.ServerBindContextClientSendOnlyController.SendOnlyTaskQueue(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next()))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await AutoCSer.TestCase.ServerSendOnlyController.WaitSendOnly();
            }
            if (!AutoCSer.TestCase.ServerSendOnlyController.CheckCount())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
