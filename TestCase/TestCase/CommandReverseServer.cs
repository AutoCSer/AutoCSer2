﻿using AutoCSer.Net;
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
            try
            {
                CommandReverseListenerConfig commandClientConfig = new CommandReverseListenerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase),
                    GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
                    CheckSeconds = 0
                };
                await using (CommandReverseListener commandListener = new CommandReverseListener(commandClientConfig))
                {
                    if (!await commandListener.Start())
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }

                    CommandReverseClientConfig commandServerConfig = new CommandReverseClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TestCase) };
                    await using (CommandReverseClient commandReverseClient = new CommandListenerBuilder(32)
                        .Append<IServerSynchronousController>(new ServerSynchronousController())
                        .Append<IServerSendOnlyController>(new ServerSendOnlyController())
                        .Append<IServerQueueController>(new ServerQueueController())
                        .Append<IServerCallbackController>(new ServerCallbackController())
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
                        .Append<ServerBindContext.IServerCallbackController>(server => new ServerBindContext.ServerCallbackController())
                        .Append<ServerBindContext.IServerKeepCallbackController>(server => new ServerBindContext.ServerKeepCallbackController())
                        .Append<ServerBindContext.IServerTaskController>(server => new ServerBindContext.ServerTaskController())
                        .Append<ServerBindContext.IServerKeepCallbackTaskController>(server => new ServerBindContext.ServerKeepCallbackTaskController())
                        .Append<ServerBindContext.IServerTaskQueueController>(server => new ServerBindContext.ServerTaskQueueController())
                        .Append<ServerBindContext.IDefinedSymmetryController>(server => new ServerBindContext.DefinedSymmetryController())
                        .Append<ServerBindContext.IDefinedDissymmetryServerController>(string.Empty, server => new ServerBindContext.DefinedDissymmetryServerController())
                        .CreateCommandListener(commandServerConfig))
                    {
                        CommandClient commandClient = await commandListener.GetCommandClient();
                        if (commandClient == null)
                        {
                            return AutoCSer.Breakpoint.ReturnFalse();
                        }

                        CommandServerSessionObject clientSessionObject = new CommandServerSessionObject();
                        CommandClientSocketEvent client = (CommandClientSocketEvent)commandClient.SocketEvent;
                        if (!ClientSynchronousController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientSendOnlyController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!ClientQueueController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientCallbackController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientKeepCallbackController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientTaskController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientKeepCallbackTaskController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientTaskQueueController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ClientTaskQueueContextController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await DefinedSymmetryController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }

                        if (!ServerBindContext.ClientSynchronousController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientSendOnlyController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!ServerBindContext.ClientQueueController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientCallbackController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientKeepCallbackController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientTaskController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientKeepCallbackTaskController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.ClientTaskQueueController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.DefinedSymmetryController.TestCase(client, clientSessionObject))
                        {
                            return false;
                        }
                        if (!await ServerBindContext.DefinedDissymmetryClientController.TestCase(client, clientSessionObject))
                        {
                            return false;
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