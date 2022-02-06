using AutoCSer.Net;
using System;
using System.Threading.Tasks;

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
        private static readonly System.Threading.SemaphoreSlim callbackWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        private static CommandClientReturnValue<string> returnValue;
        private static void callback(CommandClientReturnValue<string> value)
        {
            returnValue = value;
            callbackWaitLock.Release();
        }
        private static CommandClientReturnValue returnType;
        private static void callback(CommandClientReturnValue value)
        {
            returnType = value;
            callbackWaitLock.Release();
        }
        private static void callback(CommandClientReturnValue<string> value, CommandClientCallQueue queue)
        {
            returnValue = value;
            callbackWaitLock.Release();
        }
        private static void callback(CommandClientReturnValue value, CommandClientCallQueue queue)
        {
            returnType = value;
            callbackWaitLock.Release();
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackSocketReturn(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackSocket(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.Callback(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackReturn(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.Callback(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackQueueSocketReturn(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackQueueSocket(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCallbackController.CallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackQueueReturn(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || returnValue.Value != ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return Program.Breakpoint();
            }

            if (!await client.ClientCallbackController.CallbackQueue(callback))
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
