using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
    public interface IClientCallbackController
    {
        CallbackCommand CallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(int Value, int Ref, Action<CommandClientReturnValue> Callback);
        CallbackCommand CallbackReturn(Action<CommandClientReturnValue<string>> Callback);
        CallbackCommand Callback(Action<CommandClientReturnValue> Callback);

        CallbackCommand CallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueueReturn(Action<CommandClientReturnValue<string>, CommandClientCallQueue> Callback);
        CallbackCommand CallbackQueue(Action<CommandClientReturnValue, CommandClientCallQueue> Callback);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal static class ClientCallbackController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            return TestCase(client.ServerBindContextClientCallbackController, clientSessionObject);
        }
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(ServerBindContext.IClientCallbackController client, CommandServerSessionObject clientSessionObject)
        {
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.Callback(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackReturn(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.Callback(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.CallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueueReturn(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnValue.IsSuccess
                || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || AutoCSer.TestCase.ClientCallbackController.ReturnValue.Value != AutoCSer.TestCase.ServerSynchronousController.SessionObject.Xor().ToString())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.CallbackQueue(AutoCSer.TestCase.ClientCallbackController.Callback))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await AutoCSer.TestCase.ClientCallbackController.CallbackWaitLock.WaitAsync();
            if (!AutoCSer.TestCase.ClientCallbackController.ReturnType.IsSuccess || !AutoCSer.TestCase.ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            return true;
        }
    }
}
