using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientCallbackController
    {
        CallbackCommand CallbackSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand CallbackSocket(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackSocketReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand CallbackSocket(Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(Action<CommandClientReturnValue> Callback);

        CallbackCommand CallbackQueueSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocket(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocketReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueSocket(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientCallbackController
    {
        internal static readonly System.Threading.SemaphoreSlim CallbackWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        internal static CommandClientReturnValue<string> ReturnValue;
        internal static void Callback(CommandClientReturnValue<string> value)
        {
            ReturnValue = value;
            CallbackWaitLock.Release();
        }
        internal static CommandClientReturnValue ReturnType;
        internal static void Callback(CommandClientReturnValue value)
        {
            ReturnType = value;
            CallbackWaitLock.Release();
        }
        internal static void Callback(CommandClientReturnValue<string> value, CommandClientCallQueue queue)
        {
            ReturnValue = value;
            CallbackWaitLock.Release();
        }
        internal static void Callback(CommandClientReturnValue value, CommandClientCallQueue queue)
        {
            ReturnType = value;
            CallbackWaitLock.Release();
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            return TestCase(client.ClientCallbackController, clientSessionObject);
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(IClientCallbackController client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackSocketReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackSocket(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.Callback(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.Callback(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueSocketReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueSocket(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueReturn(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || ReturnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueue(Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await CallbackWaitLock.WaitAsync();
            if (!ReturnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
