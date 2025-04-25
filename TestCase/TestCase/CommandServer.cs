using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class CommandServer
    {
        /// <summary>
        /// 命令服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static async Task<bool> TestCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandServerSessionObject clientSessionObject = new CommandServerSessionObject();
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#if AOT
                ServerSynchronousController.SessionObject = clientSessionObject;
#endif
                if (!ClientSynchronousController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientSendOnlyController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.TestCase(client.ClientQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.TestCase(client.ClientConcurrencyReadQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.TestCase(client.ClientReadWriteQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientCallbackController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientCallbackTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientKeepCallbackController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientKeepCallbackTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await DefinedSymmetryServerController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#if !AOT
                if (!await ClientTaskQueueController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientTaskQueueContextController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#endif

                if (!ServerBindContext.ClientSynchronousController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientSendOnlyController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.TestCase(client.ServerBindContextClientQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.TestCase(client.ServerBindContextClientConcurrencyReadQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.TestCase(client.ServerBindContextClientReadWriteQueueController, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientCallbackController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientCallbackTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientKeepCallbackController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientKeepCallbackTaskController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.DefinedSymmetryServerController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#if !AOT
                if (!await ServerBindContext.ClientTaskQueueController.TestCase(client, clientSessionObject))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#endif
            }
            return true;
        }
#if !AOT
        /// <summary>
        /// 是否 AOT 客户端测试
        /// </summary>
        internal static bool IsAotClient;
        /// <summary>
        /// 创建测试服务端
        /// </summary>
        /// <returns></returns>
        internal static CommandListener CreateCommandListener()
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase) };
            return new CommandListenerBuilder(32)
                    .Append<IServerSynchronousController>(new ServerSynchronousController())
                    .Append<IServerSendOnlyController>(new ServerSendOnlyController())
                    .Append<IServerQueueController>(new ServerQueueController())
                    .Append<IServerConcurrencyReadQueueController>(new ServerConcurrencyReadQueueController())
                    .Append<IServerReadWriteQueueController>(new ServerReadWriteQueueController())
                    .Append<IServerCallbackController>(new ServerCallbackController())
                    .Append<IServerCallbackTaskController>(new ServerCallbackTaskController())
                    .Append<IServerKeepCallbackController>(new ServerKeepCallbackController())
                    .Append<IServerTaskController>(new ServerTaskController())
                    .Append<IServerKeepCallbackTaskController>(new ServerKeepCallbackTaskController())
                    .Append<IDefinedSymmetryController>(new DefinedSymmetryServerController())
                    .Append<IDefinedDissymmetryServerController>(string.Empty, new DefinedDissymmetryServerController())
                    .Append<IServerTaskQueueController>(new ServerTaskQueueController())
                    .Append<IServerTaskQueueContextController, int>((task, key) => new ServerTaskQueueContextController(task, key))

                    .Append<ServerBindContext.IServerSynchronousController>(server => new ServerBindContext.ServerSynchronousController())
                    .Append<ServerBindContext.IServerSendOnlyController>(server => new ServerBindContext.ServerSendOnlyController())
                    .Append<ServerBindContext.IServerQueueController>(server => new ServerBindContext.ServerQueueController())
                    .Append<ServerBindContext.IServerConcurrencyReadQueueController>(server => new ServerBindContext.ServerConcurrencyReadQueueController())
                    .Append<ServerBindContext.IServerReadWriteQueueController>(server => new ServerBindContext.ServerReadWriteQueueController())
                    .Append<ServerBindContext.IServerCallbackController>(server => new ServerBindContext.ServerCallbackController())
                    .Append<ServerBindContext.IServerCallbackTaskController>(server => new ServerBindContext.ServerCallbackTaskController())
                    .Append<ServerBindContext.IServerKeepCallbackController>(server => new ServerBindContext.ServerKeepCallbackController())
                    .Append<ServerBindContext.IServerTaskController>(server => new ServerBindContext.ServerTaskController())
                    .Append<ServerBindContext.IServerKeepCallbackTaskController>(server => new ServerBindContext.ServerKeepCallbackTaskController())
                    .Append<ServerBindContext.IDefinedSymmetryController>(server => new ServerBindContext.DefinedSymmetryServerController())
                    .Append<ServerBindContext.IDefinedDissymmetryServerController>(string.Empty, server => new ServerBindContext.DefinedDissymmetryServerController())
                    .Append<ServerBindContext.IServerTaskQueueController>(server => new ServerBindContext.ServerTaskQueueController())
                    .CreateCommandListener(commandServerConfig);
        }
#endif
    }
}
