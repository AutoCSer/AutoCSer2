using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 定义对称测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IDefinedSymmetryController), true)]
#endif
    public partial interface IDefinedSymmetryController
    {
        string SynchronousReturn(int Value, ref int Ref, out long Out);
        string SynchronousReturn(int Value, ref int Ref);
        string SynchronousReturn(int Value, out long Out);
        string SynchronousReturn(int Value);
        string SynchronousReturn(ref int Ref, out long Out);
        string SynchronousReturn(ref int Ref);
        string SynchronousReturn(out long Out);
        string SynchronousReturn();
        void Synchronous(int Value, ref int Ref, out long Out);
        void Synchronous(int Value, ref int Ref);
        void Synchronous(int Value, out long Out);
        void Synchronous(int Value);
        void Synchronous(ref int Ref, out long Out);
        void Synchronous(ref int Ref);
        void Synchronous(out long Out);
        void Synchronous();

        Task<string> AsynchronousTaskReturn(int Value, int Ref);
        Task<string> AsynchronousTaskReturn();
        Task AsynchronousTask(int Value, int Ref);
        Task AsynchronousTask();
    }
    /// <summary>
    /// 定义对称测试接口实例（套接字上下文绑定服务端）
    /// </summary>
    internal partial class DefinedSymmetryServerController : CommandServerBindContextController, IDefinedSymmetryController
    {
        string IDefinedSymmetryController.SynchronousReturn(int Value, ref int Ref, out long Out)
        {
            return AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value, ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(Value).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(ref int Ref, out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(ref int Ref)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(out long Out)
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn()
        {
            return ((CommandServerSessionObject)Socket.SessionObject).Xor().ToString();
        }
        void IDefinedSymmetryController.Synchronous(int Value, ref int Ref, out long Out)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, ref Ref, out Out);
        }
        void IDefinedSymmetryController.Synchronous(int Value, ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, ref Ref);
        }
        void IDefinedSymmetryController.Synchronous(int Value, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value, out Out);
        }
        void IDefinedSymmetryController.Synchronous(int Value)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(Value);
        }
        void IDefinedSymmetryController.Synchronous(ref int Ref, out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref, out Out);
        }
        void IDefinedSymmetryController.Synchronous(ref int Ref)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(ref Ref);
        }
        void IDefinedSymmetryController.Synchronous(out long Out)
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor(out Out);
        }
        void IDefinedSymmetryController.Synchronous()
        {
            ((CommandServerSessionObject)Socket.SessionObject).Xor();
        }

        Task<string> IDefinedSymmetryController.AsynchronousTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref).ToString());
        }
        Task<string> IDefinedSymmetryController.AsynchronousTaskReturn()
        {
            return Task.FromResult(((CommandServerSessionObject)Socket.SessionObject).Xor().ToString());
        }
        Task IDefinedSymmetryController.AsynchronousTask(int Value, int Ref)
        {
            AutoCSer.TestCase.ServerSynchronousController.GetSessionObject(Socket).Xor(Value, Ref);
            return AutoCSer.Common.CompletedTask;
        }
        Task IDefinedSymmetryController.AsynchronousTask()
        {
            return AutoCSer.Common.CompletedTask;
        }

        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Xor(int.MinValue);

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            string returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(clientSessionObject.Value);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = client.ServerBindContextDefinedSymmetryController.SynchronousReturn();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(clientSessionObject.Value);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.ServerBindContextDefinedSymmetryController.Synchronous(ref clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            client.ServerBindContextDefinedSymmetryController.Synchronous(out clientSessionObject.Out);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            client.ServerBindContextDefinedSymmetryController.Synchronous();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ServerBindContextDefinedSymmetryController.AsynchronousTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ServerBindContextDefinedSymmetryController.AsynchronousTaskReturn();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.CheckXor(returnValue))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            await client.ServerBindContextDefinedSymmetryController.AsynchronousTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            await client.ServerBindContextDefinedSymmetryController.AsynchronousTask();
            if (!AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
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
        internal static async Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            int refValue = 0;
            long outValue = 0;
            try
            {
                client.ServerBindContextDefinedSymmetryController.SynchronousReturn(0, ref refValue, out outValue);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.SynchronousReturn();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.Synchronous(0, ref refValue, out outValue);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.Synchronous();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.AsynchronousTaskReturn(0, 0);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.AsynchronousTaskReturn();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.AsynchronousTask(0, 0);
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

            try
            {
                client.ServerBindContextDefinedSymmetryController.AsynchronousTask();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            catch { }

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
                if (client?.ServerBindContextDefinedSymmetryController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                string value = client.ServerBindContextDefinedSymmetryController.SynchronousReturn(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
                if (value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextDefinedSymmetryController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int refValue = AutoCSer.Random.Default.Next();
                long outValue;
                client.ServerBindContextDefinedSymmetryController.Synchronous(AutoCSer.Random.Default.Next(), ref refValue, out outValue);
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextDefinedSymmetryController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                string value = await client.ServerBindContextDefinedSymmetryController.AsynchronousTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
                if (value == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextDefinedSymmetryController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                await client.ServerBindContextDefinedSymmetryController.AsynchronousTask(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next());
            }
            return true;
        }
    }
}
