using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    class ShortLinkCommandServer
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

            if (!await ClientSynchronousController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#if !AOT
            if (!await ClientSendOnlyController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif
            if (!await ClientQueueController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientCallbackController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientKeepCallbackController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientTwoStage‌CallbackController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientTaskController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientKeepCallbackTaskController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ClientTwoStage‌CallbackTaskController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await DefinedSymmetryServerController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await DefinedDissymmetryClientController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#if !AOT
            if (!await ClientTaskQueueController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif

            if (!await ServerBindContext.ClientSynchronousController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#if !AOT
            if (!await ServerBindContext.ClientSendOnlyController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif
            if (!await ServerBindContext.ClientQueueController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.ClientCallbackController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.ClientKeepCallbackController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.ClientTaskController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.ClientKeepCallbackTaskController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.DefinedSymmetryServerController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ServerBindContext.DefinedDissymmetryClientController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#if !AOT
            if (!await ServerBindContext.ClientTaskQueueController.ShortLinkTestCase())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif
            return true;
        }
        internal static CommandClient CreateCommandClient()
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.ShortLink),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
                IsShortLink = true,
                IsDefaultController = true
            };
            return new CommandClient(commandClientConfig);
        }
    }
}
