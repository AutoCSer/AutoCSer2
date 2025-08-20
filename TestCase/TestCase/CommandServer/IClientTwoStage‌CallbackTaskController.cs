using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    [AutoCSer.CodeGenerator.CommandClientController(typeof(IServerTwoStageCallbackTaskController), true)]
    public partial interface IClientTwoStage‌CallbackTaskController
    {
        KeepCallbackCommand TwoStage‌CallbackTaskSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackTaskSocketReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackTaskReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackTaskReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackCountTaskSocketReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountTaskSocketReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountTaskReturn(int Value, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountTaskReturn(Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackTaskQueueSocketReturn(int queueKey, int Ref, CommandClientReturnValueParameterCallback<long> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackTaskQueueReturn(int queueKey, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);

        KeepCallbackCommand TwoStage‌CallbackCountTaskQueueSocketReturn(int queueKey, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
        KeepCallbackCommand TwoStage‌CallbackCountTaskQueueReturn(int queueKey, int Ref, Action<CommandClientReturnValue<long>> Callback, Action<CommandClientReturnValue<string>, KeepCallbackCommand> KeepCallback);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientTwoStage‌CallbackTaskController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, new CommandClientReturnValueParameterCallback<long>(callback.Callback, 0), callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
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
            KeepCallbackCommand command = client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(0, 0, ClientTwoStage‌CallbackController.DefaultControllerCallback, ClientKeepCallbackController.DefaultControllerCallback);
            var keepCallback = await command;
            if (keepCallback != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskSocketReturn(ClientTwoStage‌CallbackController.DefaultControllerCallback, ClientKeepCallbackController.DefaultControllerCallback);
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskReturn(callback.Callback, callback.KeepCallback))
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskReturn(callback.Callback, callback.KeepCallback))
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
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
                ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback();
                using (CommandKeepCallback commandKeepCallback = await client.ClientTwoStage‌CallbackTaskController.TwoStage‌CallbackCountTaskQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), callback.Callback, callback.KeepCallback))
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
