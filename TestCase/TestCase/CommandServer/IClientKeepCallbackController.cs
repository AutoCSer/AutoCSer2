using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientKeepCallbackController
    {
        KeepCallbackCommand KeepCallbackSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackSocket(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackSocketReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackSocket(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallback(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallback(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackCountSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountSocket(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountSocketReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountSocket(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCount(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCount(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackQueueSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueSocket(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueSocketReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueSocket(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueue(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackCountQueueSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueSocket(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueSocketReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueSocket(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueue(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueue(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientKeepCallbackController
    {
        private struct KeepCallbackCommandResult
        {
            public int CallbackIndex;
            public CommandClientReturnValue ReturnValue;
        }
        private static readonly System.Threading.SemaphoreSlim callbackWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        private static CommandServerSessionObject clientSessionObject;
        private static readonly System.Collections.Generic.Dictionary<KeepCallbackCommand, KeepCallbackCommandResult> keepCallbackCommands = new System.Collections.Generic.Dictionary<KeepCallbackCommand, KeepCallbackCommandResult>();
        private static void callback(CommandClientReturnValue<string> value, KeepCallbackCommand keepCallbackCommand)
        {
            if (!keepCallbackCommands.TryGetValue(keepCallbackCommand, out KeepCallbackCommandResult result))
            {
                keepCallbackCommands.Add(keepCallbackCommand, result = default(KeepCallbackCommandResult));
            }
            switch (value.ReturnType)
            {
                case CommandClientReturnType.Success:
                    if (result.CallbackIndex == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                    {
                        callbackWaitLock.Release();
                        return;
                    }
                    if (value.Value != (ServerSynchronousController.SessionObject.Xor() + result.CallbackIndex).ToString())
                    {
                        callbackWaitLock.Release();
                        return;
                    }
                    ++result.CallbackIndex;
                    keepCallbackCommands[keepCallbackCommand] = result;
                    return;
                case CommandClientReturnType.CancelKeepCallback:
                    if (result.CallbackIndex == ServerKeepCallbackController.KeepCallbackCount)
                    {
                        result.ReturnValue = CommandClientReturnType.Success;
                        keepCallbackCommands[keepCallbackCommand] = result;
                        callbackWaitLock.Release();
                    }
                    else
                    {
                        callbackWaitLock.Release();
                    }
                    return;
                default:
                    result.ReturnValue = value.ReturnType;
                    keepCallbackCommands[keepCallbackCommand] = result;
                    callbackWaitLock.Release();
                    return;
            }
        }
        private static void callback(CommandClientReturnValue value, KeepCallbackCommand keepCallbackCommand)
        {
            if (!keepCallbackCommands.TryGetValue(keepCallbackCommand, out KeepCallbackCommandResult result))
            {
                keepCallbackCommands.Add(keepCallbackCommand, result = default(KeepCallbackCommandResult));
            }
            switch(value.ReturnType)
            {
                case CommandClientReturnType.Success:
                    if (result.CallbackIndex == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                    {
                        callbackWaitLock.Release();
                        return;
                    }
                    ++result.CallbackIndex;
                    keepCallbackCommands[keepCallbackCommand] = result;
                    return;
                case CommandClientReturnType.CancelKeepCallback:
                    if (result.CallbackIndex == ServerKeepCallbackController.KeepCallbackCount)
                    {
                        result.ReturnValue = CommandClientReturnType.Success;
                        keepCallbackCommands[keepCallbackCommand] = result;
                        callbackWaitLock.Release();
                    }
                    else
                    {
                        callbackWaitLock.Release();
                    }
                    return;
                default:
                    result.ReturnValue = value;
                    keepCallbackCommands[keepCallbackCommand] = result;
                    callbackWaitLock.Release();
                    return;
            }
        }
        private static async Task<bool> waitKeepCallback(CommandKeepCallback commandKeepCallback)
        {
            if (commandKeepCallback == null)
            {
                return Program.Breakpoint();
            }
            await callbackWaitLock.WaitAsync();
            if (!keepCallbackCommands.TryGetValue((KeepCallbackCommand)commandKeepCallback.Command, out KeepCallbackCommandResult result))
            {
                return Program.Breakpoint();
            }
            keepCallbackCommands.Remove((KeepCallbackCommand)commandKeepCallback.Command);
            if (!result.ReturnValue.IsSuccess)
            {
                return Program.Breakpoint();
            }
            return true;
        }

        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            ClientKeepCallbackController.clientSessionObject = clientSessionObject;
            keepCallbackCommands.Clear();

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if(!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocketReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocket(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallback(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallback(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocketReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocket(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCount(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCount(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocketReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocket(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueue(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocketReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocket(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueue(clientSessionObject.Value, clientSessionObject.Ref, callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueReturn(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueue(callback))
            {
                if (!await waitKeepCallback(commandKeepCallback))
                {
                    return Program.Breakpoint();
                }
            }

            return true;
        }
    }
}
