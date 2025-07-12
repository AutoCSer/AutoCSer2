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
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerSynchronousController), true)]
#endif
    public partial interface IClientSynchronousController
    {
        CommandClientReturnValue<string> SynchronousReturn(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(int Value, ref int Ref);
        CommandClientReturnValue<string> SynchronousReturn(int Value, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(int Value);
        CommandClientReturnValue<string> SynchronousReturn(ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturn(ref int Ref);
        CommandClientReturnValue<string> SynchronousReturn(out long Out);
        CommandClientReturnValue<string> SynchronousReturn();
        CommandClientReturnValue Synchronous(int Value, ref int Ref, out long Out);
        CommandClientReturnValue Synchronous(int Value, ref int Ref);
        CommandClientReturnValue Synchronous(int Value, out long Out);
        CommandClientReturnValue Synchronous(int Value);
        CommandClientReturnValue Synchronous(ref int Ref, out long Out);
        CommandClientReturnValue Synchronous(ref int Ref);
        CommandClientReturnValue Synchronous(out long Out);
        CommandClientReturnValue Synchronous();
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientSynchronousController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static bool TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn();
            if (!returnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(clientSessionObject.Value);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ServerBindContextClientSynchronousController.Synchronous(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous();
            if (!returnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
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
        internal static bool DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            if (client.ServerBindContextClientSynchronousController == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue returnType = client.ServerBindContextClientSynchronousController.Synchronous(0);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous();
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int refValue = 0;
            long outValue = 0;
            CommandClientReturnValue<string> returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(0, ref refValue, out outValue);
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ServerBindContextClientSynchronousController.Synchronous(0, ref refValue, out outValue);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
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
                if (client?.ServerBindContextClientSynchronousController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue<string> returnValue = client.ServerBindContextClientSynchronousController.SynchronousReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientSynchronousController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue returnType = client.ServerBindContextClientSynchronousController.Synchronous(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            if (!AutoCSer.TestCase.ServerSendOnlyController.CheckCount())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
