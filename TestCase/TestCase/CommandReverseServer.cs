using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class CommandReverseServer
    {
        /// <summary>
        /// 反向命令服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static async Task<bool> TestCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            try
            {
                CommandReverseListenerConfig commandClientConfig = new CommandReverseListenerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase),
                    GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
                };
#if NetStandard21
                await
#endif
                using (CommandReverseListener commandListener = new CommandReverseListener(commandClientConfig))
                {
                    if (!await commandListener.Start())
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }

                    CommandReverseClientConfig commandServerConfig = new CommandReverseClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase) };

#if NetStandard21
                    await
#endif
                    using (CommandReverseClient commandReverseClient = new CommandListenerBuilder(32)
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
                        .Append<IServerTaskQueueController>(new ServerTaskQueueController())
                        .Append<IServerTaskQueueContextController, int>((task, key) => new ServerTaskQueueContextController(task, key))
                        .Append<IDefinedSymmetryController>(new DefinedSymmetryController())
                        .Append<IDefinedDissymmetryServerController>(string.Empty, new DefinedDissymmetryServerController())

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
                        .Append<ServerBindContext.IServerTaskQueueController>(server => new ServerBindContext.ServerTaskQueueController())
                        .Append<ServerBindContext.IDefinedSymmetryController>(server => new ServerBindContext.DefinedSymmetryController())
                        .Append<ServerBindContext.IDefinedDissymmetryServerController>(string.Empty, server => new ServerBindContext.DefinedDissymmetryServerController())
                        .CreateCommandListener(commandServerConfig))
                    {
                        CommandClientSocketEvent client = (CommandClientSocketEvent)await commandListener.GetSocketEvent();
                        if (client == null)
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }

                        CommandServerSessionObject clientSessionObject = new CommandServerSessionObject();
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
                        if (!await ClientTaskQueueController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                        if (!await ClientTaskQueueContextController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                        if (!await DefinedSymmetryController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                        if (!await DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }

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
                        if (!await ServerBindContext.ClientTaskQueueController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                        if (!await ServerBindContext.DefinedSymmetryController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                        if (!await ServerBindContext.DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
