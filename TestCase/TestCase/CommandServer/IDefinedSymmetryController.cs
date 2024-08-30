using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 定义对称测试接口
    /// </summary>
    public interface IDefinedSymmetryController
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
    /// 定义对称测试接口实例
    /// </summary>
    internal sealed class DefinedSymmetryController : IDefinedSymmetryController
    {
        string IDefinedSymmetryController.SynchronousReturn(int Value, ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value, ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, ref Ref).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(Value, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(int Value)
        {
            return ServerSynchronousController.SessionObject.Xor(Value).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(ref int Ref, out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref, out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(ref int Ref)
        {
            return ServerSynchronousController.SessionObject.Xor(ref Ref).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn(out long Out)
        {
            return ServerSynchronousController.SessionObject.Xor(out Out).ToString();
        }
        string IDefinedSymmetryController.SynchronousReturn()
        {
            return ServerSynchronousController.SessionObject.Xor().ToString();
        }
        void IDefinedSymmetryController.Synchronous(int Value, ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref, out Out);
        }
        void IDefinedSymmetryController.Synchronous(int Value, ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, ref Ref);
        }
        void IDefinedSymmetryController.Synchronous(int Value, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(Value, out Out);
        }
        void IDefinedSymmetryController.Synchronous(int Value)
        {
            ServerSynchronousController.SessionObject.Xor(Value);
        }
        void IDefinedSymmetryController.Synchronous(ref int Ref, out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref, out Out);
        }
        void IDefinedSymmetryController.Synchronous(ref int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(ref Ref);
        }
        void IDefinedSymmetryController.Synchronous(out long Out)
        {
            ServerSynchronousController.SessionObject.Xor(out Out);
        }
        void IDefinedSymmetryController.Synchronous()
        {
            ServerSynchronousController.SessionObject.Xor();
        }

        Task<string> IDefinedSymmetryController.AsynchronousTaskReturn(int Value, int Ref)
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor(Value, Ref).ToString());
        }
        Task<string> IDefinedSymmetryController.AsynchronousTaskReturn()
        {
            return Task.FromResult(ServerSynchronousController.SessionObject.Xor().ToString());
        }
        Task IDefinedSymmetryController.AsynchronousTask(int Value, int Ref)
        {
            ServerSynchronousController.SessionObject.Xor(Value, Ref);
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
            string returnValue = client.DefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.DefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.DefinedSymmetryController.SynchronousReturn(clientSessionObject.Value, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = client.DefinedSymmetryController.SynchronousReturn(clientSessionObject.Value);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.DefinedSymmetryController.SynchronousReturn(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = client.DefinedSymmetryController.SynchronousReturn(ref clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            returnValue = client.DefinedSymmetryController.SynchronousReturn(out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            returnValue = client.DefinedSymmetryController.SynchronousReturn();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(clientSessionObject.Value, ref clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(clientSessionObject.Value, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(clientSessionObject.Value);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(ref clientSessionObject.Ref, out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            client.DefinedSymmetryController.Synchronous(ref clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            client.DefinedSymmetryController.Synchronous(out clientSessionObject.Out);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            client.DefinedSymmetryController.Synchronous();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.DefinedSymmetryController.AsynchronousTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            returnValue = await client.DefinedSymmetryController.AsynchronousTaskReturn();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return false;
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            await client.DefinedSymmetryController.AsynchronousTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            await client.DefinedSymmetryController.AsynchronousTask();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return false;
            }

            return true;
        }
    }
}
