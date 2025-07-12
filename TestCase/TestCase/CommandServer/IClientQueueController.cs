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
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerQueueController), true)]
#endif
    public partial interface IClientQueueController
    {
        CommandClientReturnValue<string> QueueReturnSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(int Value, ref int Ref);
        CommandClientReturnValue<string> QueueReturnSocket(int Value, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(int Value);
        CommandClientReturnValue<string> QueueReturnSocket(ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturnSocket(ref int Ref);
        CommandClientReturnValue<string> QueueReturnSocket(out long Out);
        CommandClientReturnValue<string> QueueReturnSocket();
        CommandClientReturnValue QueueSocket(int Value, ref int Ref, out long Out);
        CommandClientReturnValue QueueSocket(int Value, ref int Ref);
        CommandClientReturnValue QueueSocket(int Value, out long Out);
        CommandClientReturnValue QueueSocket(int Value);
        CommandClientReturnValue QueueSocket(ref int Ref, out long Out);
        CommandClientReturnValue QueueSocket(ref int Ref);
        CommandClientReturnValue QueueSocket(out long Out);
        CommandClientReturnValue QueueSocket();
        CommandClientReturnValue<string> QueueReturn(int Value, ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturn(int Value, ref int Ref);
        CommandClientReturnValue<string> QueueReturn(int Value, out long Out);
        CommandClientReturnValue<string> QueueReturn(int Value);
        CommandClientReturnValue<string> QueueReturn(ref int Ref, out long Out);
        CommandClientReturnValue<string> QueueReturn(ref int Ref);
        CommandClientReturnValue<string> QueueReturn(out long Out);
        CommandClientReturnValue<string> QueueReturn();
        CommandClientReturnValue Queue(int Value, ref int Ref, out long Out);
        CommandClientReturnValue Queue(int Value, ref int Ref);
        CommandClientReturnValue Queue(int Value, out long Out);
        CommandClientReturnValue Queue(int Value);
        CommandClientReturnValue Queue(ref int Ref, out long Out);
        CommandClientReturnValue Queue(ref int Ref);
        CommandClientReturnValue Queue(out long Out);
        CommandClientReturnValue Queue();
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientQueueController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static bool TestCase(IClientQueueController client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = client.QueueReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturnSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturnSocket(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturnSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturnSocket(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.QueueReturnSocket(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.QueueReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = client.QueueSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.QueueSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.QueueSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.QueueSocket(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.QueueSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.QueueSocket(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.QueueSocket(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.QueueSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.QueueReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.QueueReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.QueueReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.Queue(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.Queue(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.Queue(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.Queue(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.Queue(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.Queue(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.Queue(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.Queue();
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
        internal static bool DefaultControllerTestCase(IClientQueueController client)
        {
            int refValue = 0;
            long outValue = 0;
            CommandClientReturnValue<string> returnValue = client.QueueReturnSocket(0, ref refValue, out outValue);
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.QueueReturnSocket();
            if (returnValue.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            CommandClientReturnValue returnType = client.QueueSocket(0, ref refValue, out outValue);
            if (returnType.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = client.QueueSocket();
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
                if (client?.ClientQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue<string> returnValue = client.ClientQueueController.QueueReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue returnType = client.ClientQueueController.Queue(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientConcurrencyReadQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue<string> returnValue = client.ClientConcurrencyReadQueueController.QueueReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientConcurrencyReadQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue returnType = client.ClientConcurrencyReadQueueController.Queue(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientReadWriteQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue<string> returnValue = client.ClientReadWriteQueueController.QueueReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnValue.IsSuccess || returnValue.Value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientReadWriteQueueController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                CommandClientReturnValue returnType = client.ClientReadWriteQueueController.Queue(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (!returnType.IsSuccess)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            return true;
        }
    }
}
