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
            try
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.TestCase) };
                using (CommandListener commandListener = new CommandListener(commandServerConfig
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerSynchronousController>(new ServerSynchronousController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerSendOnlyController>(new ServerSendOnlyController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerQueueController>(new ServerQueueController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerCallbackController>(new ServerCallbackController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerKeepCallbackController>(new ServerKeepCallbackController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerTaskController>(new ServerTaskController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerKeepCallbackTaskController>(new ServerKeepCallbackTaskController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerTaskQueueController>(new ServerTaskQueueController())
                    , CommandServerInterfaceControllerCreator.GetCreator<IServerTaskQueueContextController, int>((task, key) => new ServerTaskQueueContextController(task, key))
                    ))
                {
                    if (!await commandListener.Start())
                    {
                        return Program.Breakpoint();
                    }

                    CommandClientConfig commandClientConfig = new CommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.TestCase) };
                    using (CommandClient commandClient = new CommandClient(commandClientConfig))
                    {
                        CommandServerSessionObject clientSessionObject = new CommandServerSessionObject();
                        if (await commandClient.GetSocketAsync() == null)
                        {
                            return Program.Breakpoint();
                        }
                        CommandClientSocketEvent client = (CommandClientSocketEvent)commandClient.SocketEvent;
                        if (!ClientSynchronousController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientSendOnlyController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!ClientQueueController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientCallbackController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientKeepCallbackController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientTaskController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientKeepCallbackTaskController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientTaskQueueController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                        if (!await ClientTaskQueueContextController.TestCase(client, clientSessionObject))
                        {
                            return Program.Breakpoint();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return Program.Breakpoint();
            }
            return true;
        }
    }
}
