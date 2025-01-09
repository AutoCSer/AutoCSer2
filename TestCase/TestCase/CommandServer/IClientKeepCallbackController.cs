using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
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
        internal struct KeepCallbackCommandResult
        {
            public int CallbackIndex;
            public CommandClientReturnValue ReturnValue;
        }
        private static readonly System.Threading.SemaphoreSlim callbackWaitLock = new System.Threading.SemaphoreSlim(0, 1);
        internal static CommandServerSessionObject ClientSessionObject;
        internal static readonly System.Collections.Generic.Dictionary<KeepCallbackCommand, KeepCallbackCommandResult> KeepCallbackCommands = new System.Collections.Generic.Dictionary<KeepCallbackCommand, KeepCallbackCommandResult>();
        internal static void Callback(CommandClientReturnValue<string> value, KeepCallbackCommand keepCallbackCommand)
        {
            KeepCallbackCommandResult result;
            if (!KeepCallbackCommands.TryGetValue(keepCallbackCommand, out result))
            {
                KeepCallbackCommands.Add(keepCallbackCommand, result = default(KeepCallbackCommandResult));
            }
            switch (value.ReturnType)
            {
                case CommandClientReturnTypeEnum.Success:
                    if (result.CallbackIndex == 0 && !ServerSynchronousController.SessionObject.Check(ClientSessionObject))
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
                    KeepCallbackCommands[keepCallbackCommand] = result;
                    return;
                case CommandClientReturnTypeEnum.CancelKeepCallback:
                    if (result.CallbackIndex == ServerKeepCallbackController.KeepCallbackCount)
                    {
                        result.ReturnValue = CommandClientReturnTypeEnum.Success;
                        KeepCallbackCommands[keepCallbackCommand] = result;
                        callbackWaitLock.Release();
                    }
                    else
                    {
                        callbackWaitLock.Release();
                    }
                    return;
                default:
                    result.ReturnValue = value.ReturnType;
                    KeepCallbackCommands[keepCallbackCommand] = result;
                    callbackWaitLock.Release();
                    return;
            }
        }
        internal static void Callback(CommandClientReturnValue value, KeepCallbackCommand keepCallbackCommand)
        {
            KeepCallbackCommandResult result;
            if (!KeepCallbackCommands.TryGetValue(keepCallbackCommand, out result))
            {
                KeepCallbackCommands.Add(keepCallbackCommand, result = default(KeepCallbackCommandResult));
            }
            switch(value.ReturnType)
            {
                case CommandClientReturnTypeEnum.Success:
                    if (result.CallbackIndex == 0 && !ServerSynchronousController.SessionObject.Check(ClientSessionObject))
                    {
                        callbackWaitLock.Release();
                        return;
                    }
                    ++result.CallbackIndex;
                    KeepCallbackCommands[keepCallbackCommand] = result;
                    return;
                case CommandClientReturnTypeEnum.CancelKeepCallback:
                    if (result.CallbackIndex == ServerKeepCallbackController.KeepCallbackCount)
                    {
                        result.ReturnValue = CommandClientReturnTypeEnum.Success;
                        KeepCallbackCommands[keepCallbackCommand] = result;
                        callbackWaitLock.Release();
                    }
                    else
                    {
                        callbackWaitLock.Release();
                    }
                    return;
                default:
                    result.ReturnValue = value;
                    KeepCallbackCommands[keepCallbackCommand] = result;
                    callbackWaitLock.Release();
                    return;
            }
        }
        internal static async Task<bool> WaitKeepCallback(CommandKeepCallback commandKeepCallback)
        {
            if (commandKeepCallback == null)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await callbackWaitLock.WaitAsync();
            KeepCallbackCommandResult result;
            if (!KeepCallbackCommands.TryGetValue((KeepCallbackCommand)commandKeepCallback.Command, out result))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            KeepCallbackCommands.Remove((KeepCallbackCommand)commandKeepCallback.Command);
            if (!result.ReturnValue.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
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
            ClientKeepCallbackController.ClientSessionObject = clientSessionObject;
            KeepCallbackCommands.Clear();

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if(!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocketReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackSocket(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallback(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallback(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocketReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountSocket(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCount(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCount(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocketReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueSocket(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueueReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackQueue(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocket(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocketReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueSocket(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueue(clientSessionObject.Value, clientSessionObject.Ref, Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueueReturn(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ClientKeepCallbackController.KeepCallbackCountQueue(Callback))
            {
                if (!await WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            return true;
        }
    }
}
