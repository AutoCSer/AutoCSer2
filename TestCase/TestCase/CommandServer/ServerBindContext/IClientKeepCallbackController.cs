﻿using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase.ServerBindContext
{
    /// <summary>
    /// 客户端测试接口（套接字上下文绑定服务端）
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.CommandClientController(typeof(ServerBindContext.IServerKeepCallbackController), true)]
#endif
    public partial interface IClientKeepCallbackController
    {
        KeepCallbackCommand KeepCallbackReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallback(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallback(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackCountReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCount(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCount(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueue(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueueReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackQueue(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);

        KeepCallbackCommand KeepCallbackCountQueueReturn(int Value, int Ref, Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueue(int Value, int Ref, Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueueReturn(Action<CommandClientReturnValue<string>, KeepCallbackCommand> Callback);
        KeepCallbackCommand KeepCallbackCountQueue(Action<CommandClientReturnValue, KeepCallbackCommand> Callback);
    }
    /// <summary>
    /// 命令客户端测试（套接字上下文绑定服务端）
    /// </summary>
    internal partial class ClientKeepCallbackController
    {
        /// <summary>
        /// 命令客户端测试
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientSessionObject"></param>
        /// <returns></returns>
        internal static async Task<bool> TestCase(CommandClientSocketEvent client, CommandServerSessionObject clientSessionObject)
        {
            AutoCSer.TestCase.ClientKeepCallbackController.ClientSessionObject = clientSessionObject;
            AutoCSer.TestCase.ClientKeepCallbackController.KeepCallbackCommands.Clear();

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallback(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackReturn(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallback(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCount(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountReturn(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCount(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackQueue(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackQueueReturn(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackQueue(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountQueueReturn(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountQueue(clientSessionObject.Value, clientSessionObject.Ref, AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountQueueReturn(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountQueue(AutoCSer.TestCase.ClientKeepCallbackController.Callback))
            {
                if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
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
            KeepCallbackCommand command = client.ServerBindContextClientKeepCallbackController.KeepCallbackReturn(0, 0, AutoCSer.TestCase.ClientKeepCallbackController.DefaultControllerCallback);
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ServerBindContextClientKeepCallbackController.KeepCallback(0, 0, AutoCSer.TestCase.ClientKeepCallbackController.DefaultControllerCallback);
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ServerBindContextClientKeepCallbackController.KeepCallbackReturn(AutoCSer.TestCase.ClientKeepCallbackController.DefaultControllerCallback);
            if (await command != null || command.ReturnType != CommandClientReturnTypeEnum.NoSocketCreated)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            command = client.ServerBindContextClientKeepCallbackController.KeepCallback(AutoCSer.TestCase.ClientKeepCallbackController.DefaultControllerCallback);
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
                if (client?.ServerBindContextClientKeepCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientKeepCallbackController.ShortLinkCallback))
                {
                    if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientKeepCallbackController.ShortLinkCallback))
                {
                    if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientKeepCallbackController.ShortLinkCallback))
                {
                    if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            using (CommandClient commandClient = ShortLinkCommandServer.CreateCommandClient())
            {
                CommandClientSocketEvent client = await commandClient.GetSocketEvent<CommandClientSocketEvent>();
                if (client?.ServerBindContextClientKeepCallbackController == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                using (CommandKeepCallback commandKeepCallback = await client.ServerBindContextClientKeepCallbackController.KeepCallbackCountQueueReturn(AutoCSer.Random.Default.Next(), AutoCSer.Random.Default.Next(), AutoCSer.TestCase.ClientKeepCallbackController.ShortLinkCallback))
                {
                    if (!await AutoCSer.TestCase.ClientKeepCallbackController.WaitKeepCallback(commandKeepCallback))
                    {
                        return AutoCSer.Breakpoint.ReturnFalse();
                    }
                }
            }
            return true;
        }
    }
}
