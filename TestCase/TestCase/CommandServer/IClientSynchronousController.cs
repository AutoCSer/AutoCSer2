using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientSynchronousController
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
        CommandClientReturnValue<CommandServerVerifyState> SynchronousSocket(int Value);
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
    internal static class ClientSynchronousController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static bool TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Xor(int.MinValue);
            CommandClientReturnValue returnType = client.ClientSynchronousController.SynchronousSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue<string> returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturnSocket(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.SynchronousSocket(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = client.ClientSynchronousController.SynchronousSocket(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ClientSynchronousController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturn(out clientSessionObject.Out);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            returnValue = client.ClientSynchronousController.SynchronousReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = client.ClientSynchronousController.Synchronous(ref clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = client.ClientSynchronousController.Synchronous(out clientSessionObject.Out);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            returnType = client.ClientSynchronousController.Synchronous();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
