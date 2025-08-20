using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class CommandClientDefaultController
    {
        /// <summary>
        /// 默认控制器测试
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
                Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.TestCase),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, false),
                IsDefaultController = true,
                IsAutoSocket = false
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocketEvent client = (CommandClientSocketEvent)commandClient.SocketEvent;
                if (!ClientSynchronousController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientSendOnlyController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.DefaultControllerTestCase(client.ClientQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.DefaultControllerTestCase(client.ClientConcurrencyReadQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ClientQueueController.DefaultControllerTestCase(client.ClientReadWriteQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientCallbackController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientCallbackTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientKeepCallbackController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientTwoStage‌CallbackController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientKeepCallbackTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ClientTwoStage‌CallbackTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await DefinedSymmetryServerController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await DefinedDissymmetryClientController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#if !AOT
                if (!await ClientTaskQueueController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#endif

                if (!ServerBindContext.ClientSynchronousController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientSendOnlyController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.DefaultControllerTestCase(client.ServerBindContextClientQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.DefaultControllerTestCase(client.ServerBindContextClientConcurrencyReadQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!ServerBindContext.ClientQueueController.DefaultControllerTestCase(client.ServerBindContextClientReadWriteQueueController))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientCallbackController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientCallbackTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientKeepCallbackController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.ClientKeepCallbackTaskController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.DefinedSymmetryServerController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (!await ServerBindContext.DefinedDissymmetryClientController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#if !AOT
                if (!await ServerBindContext.ClientTaskQueueController.DefaultControllerTestCase(client))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
#endif
            }
            return true;
        }
    }
}
