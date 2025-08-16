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
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerTwoStageCallbackController), true)]
#endif
    public partial interface IClientTwoStage‌CallbackController
    {
        KeepCallbackCommand TwoStage‌CallbackSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackSocketReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackCountSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountSocketReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackQueueSocketReturn(int Value, int Ref, CommandClientReturnValueParameterCallback<long> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackQueueSocketReturn(CommandClientReturnValueParameterCallback<long> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackCountQueueSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountQueueSocketReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackReadWriteQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackReadWriteQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountReadWriteQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountReadWriteQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackConcurrencyReadQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackConcurrencyReadQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountConcurrencyReadQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountConcurrencyReadQueueReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientTwoStage‌CallbackController
    {
        internal sealed class Stage‌Callback
        {
            private readonly CommandServerSessionObject clientSessionObject;
            private readonly System.Threading.SemaphoreSlim waitLock;
            private long value;
            private long endValue;
            private CommandClientReturnTypeEnum returnType;
            internal Stage‌Callback()
            {
                returnType = CommandClientReturnTypeEnum.Success;
                waitLock = new System.Threading.SemaphoreSlim(0, 1);
            }
            internal Stage‌Callback(CommandServerSessionObject clientSessionObject, bool isRandom) : this()
            {
                this.clientSessionObject = clientSessionObject;
                if (isRandom)
                {
                    clientSessionObject.Value = AutoCSer.Random.Default.Next();
                    clientSessionObject.Ref = AutoCSer.Random.Default.Next();
                }
                value = clientSessionObject.Xor() - 1;
            }
            internal void Callback(CommandClientReturnValue<long> value)
            {
                if (!value.IsSuccess)
                {
                    returnType = value.ReturnType;
                    waitLock.Release();
                    return;
                }
                if (clientSessionObject != null && value.Value != clientSessionObject.Xor())
                {
                    returnType = CommandClientReturnTypeEnum.Unknown;
                    waitLock.Release();
                    return;
                }
                this.value = value.Value;
                endValue = value.Value + ServerKeepCallbackController.KeepCallbackCount;
            }
            internal void KeepCallback(CommandClientReturnValue<string> value, KeepCallbackCommand keepCallbackCommand)
            {
                switch (value.ReturnType)
                {
                    case CommandClientReturnTypeEnum.Success:
                        if (this.value == long.Parse(value.Value)) ++this.value;
                        else
                        {
                            returnType = CommandClientReturnTypeEnum.Unknown;
                            waitLock.Release();
                        }
                        return;
                    case CommandClientReturnTypeEnum.CancelKeepCallback:
                        if (this.value != endValue) returnType = CommandClientReturnTypeEnum.Unknown;
                        waitLock.Release();
                        return;
                    default:
                        returnType = value.ReturnType;
                        waitLock.Release();
                        return;
                }
            }
            internal async Task<bool> Wait(CommandKeepCallback commandKeepCallback)
            {
                if (commandKeepCallback == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                if (returnType == CommandClientReturnTypeEnum.Success)
                {
                    await waitLock.WaitAsync();
                    if (returnType == CommandClientReturnTypeEnum.Success) return true;
                }
                commandKeepCallback.Close();
                return AutoCSer.Breakpoint.ReturnFalse();
            }
        }

        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            Stage‌Callback callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if(!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }


            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, new CommandClientReturnValueParameterCallback<long>(callback.Callback, 0), callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackQueueSocketReturn(new CommandClientReturnValueParameterCallback<long>(callback.Callback, 0), callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountQueueSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }


            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackReadWriteQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackReadWriteQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountReadWriteQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountReadWriteQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }


            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackConcurrencyReadQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackConcurrencyReadQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountConcurrencyReadQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountConcurrencyReadQueueReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            return true;
        }
        internal static void DefaultControllerCallback(CommandClientReturnValue<long> value)
        {
            if (value.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                AutoCSer.ConsoleWriteQueue.WriteLine(value.ReturnType.ToString());
            }
        }
        /// <summary>
        /// 默认控制器测试
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal static async Task<bool> DefaultControllerTestCase(CommandClientSocketEvent client)
        {
            KeepCallbackCommand command = client.ClientTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(0, 0, DefaultControllerCallback, ClientKeepCallbackController.DefaultControllerCallback);
            var keepCallback = await command;
            if (keepCallback != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ClientTwoStage‌CallbackController.TwoStage‌CallbackSocketReturn(DefaultControllerCallback, ClientKeepCallbackController.DefaultControllerCallback);
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
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
                if (client?.ClientTwoStage‌CallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                Stage‌Callback callback = new Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
                {
                    if (!await callback.Wait(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientTwoStage‌CallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                Stage‌Callback callback = new Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
                {
                    if (!await callback.Wait(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientTwoStage‌CallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                Stage‌Callback callback = new Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
                {
                    if (!await callback.Wait(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ClientTwoStage‌CallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                Stage‌Callback callback = new Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackController.TwoStage‌CallbackCountQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
                {
                    if (!await callback.Wait(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            return true;
        }
    }
}
