using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerSynchronousController), true)]
#endif
    public partial interface IClientSynchronousController
    {
        CommandClientReturnValue<string> SynchronousReturnSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturnSocket(int Value, ref int Ref);
        CommandClientReturnValue<string> SynchronousReturnSocket(int Value, out long Out);
        CommandClientReturnValue<string> SynchronousReturnSocket(int Value);
        CommandClientReturnValue<string> SynchronousReturnSocket(ref int Ref, out long Out);
        CommandClientReturnValue<string> SynchronousReturnSocket(ref int Ref);
        CommandClientReturnValue<string> SynchronousReturnSocket(out long Out);
        CommandClientReturnValue<string> SynchronousReturnSocket();
        CommandClientReturnValue SynchronousSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue SynchronousSocket(int Value, ref int Ref);
        CommandClientReturnValue SynchronousSocket(int Value, out long Out);
        CommandClientReturnValue SynchronousSocket(int Value);
        CommandClientReturnValue SynchronousSocket(ref int Ref, out long Out);
        CommandClientReturnValue SynchronousSocket(ref int Ref);
        CommandClientReturnValue SynchronousSocket(out long Out);
        CommandClientReturnValue SynchronousSocket();
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
    /// 命令客户端测试
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
            if (client.ClientSynchronousController == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Xor(int.MinValue);
            CommandClientReturnValue returnType = client.ClientSynchronousController.SynchronousSocket(int.MinValue);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.SynchronousSocket();
            if (!returnType.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.SynchronousSocket(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.Synchronous(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.Synchronous();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
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
            if (client.ClientSynchronousController == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue returnType = client.ClientSynchronousController.SynchronousSocket(0);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.SynchronousSocket();
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            int refValue = 0;
            long outValue = 0;
            CommandClientReturnValue<string> returnValue = client.ClientSynchronousController.SynchronousReturnSocket(0, ref refValue, out outValue);
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturnSocket();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.ClientSynchronousController.SynchronousSocket(0, ref refValue, out outValue);
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
                if (client?.ClientSynchronousController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue<string> returnValue = client.ClientSynchronousController.SynchronousReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientSynchronousController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue returnType = client.ClientSynchronousController.Synchronous(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
