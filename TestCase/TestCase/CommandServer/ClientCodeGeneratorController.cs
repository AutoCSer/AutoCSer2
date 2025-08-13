using AutoCSer.Net;
using System;
using System.Threading.Tasks;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal partial class ClientCodeGeneratorController
    {
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
            CommandClientReturnValue<string> returnValue = await client.ClientCodeGeneratorController.TaskReturnSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientCodeGeneratorController.TaskReturnSocket();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            CommandClientReturnValue returnType = await client.ClientCodeGeneratorController.TaskSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientCodeGeneratorController.TaskSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientCodeGeneratorController.TaskQueueReturnSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientCodeGeneratorController.TaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientCodeGeneratorController.CallbackSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientCodeGeneratorController.CallbackSocketReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientCodeGeneratorController.CallbackSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientCodeGeneratorController.CallbackSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientCodeGeneratorController.TaskCallbackSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientCodeGeneratorController.TaskCallbackSocketReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            returnType = await client.ClientCodeGeneratorController.TaskCallbackSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientCodeGeneratorController.TaskCallbackSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientCodeGeneratorController.QueueSocketInputReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientCodeGeneratorController.QueueSocketReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = await client.ClientCodeGeneratorController.QueueSocketInput(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientCodeGeneratorController.QueueSocket();
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnValue = await client.ClientCodeGeneratorController.SynchronousSocketInputReturn(clientSessionObject.Value);
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnValue = await client.ClientCodeGeneratorController.SynchronousSocketReturn();
            if (!returnValue.IsSuccess
                || !ServerSynchronousController.SessionObject.Check(clientSessionObject)
                || !ServerSynchronousController.SessionObject.CheckXor(returnValue.Value))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            returnType = await client.ClientCodeGeneratorController.SynchronousSocketInput(clientSessionObject.Value);
            if (!returnType.IsSuccess || !ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            returnType = await client.ClientCodeGeneratorController.SynchronousSocket();
            if (!returnType.IsSuccess)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

#if !AOT
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCodeGeneratorController.SendOnlySocketInput(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.WaitSendOnly();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientCodeGeneratorController.SendOnlySocket())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.WaitSendOnly();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            if (!await client.ClientCodeGeneratorController.SendOnlyTaskSocketInput(clientSessionObject.Value, clientSessionObject.Ref))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.WaitSendOnly();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await client.ClientCodeGeneratorController.SendOnlyTaskSocket())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            await ServerSendOnlyController.WaitSendOnly();
            if (!ServerSynchronousController.SessionObject.Check(clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
#endif

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackSocketReturn();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackSocket();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackCountSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackCountSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackCountSocketReturn();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackCountSocket();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackTaskSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackTaskSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackTaskSocketReturn();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackTaskSocket();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackCountTaskSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackCountTaskSocketInput(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommandReturn = await client.ClientCodeGeneratorController.KeepCallbackCountTaskSocketReturn();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommandReturn, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            enumeratorCommand = await client.ClientCodeGeneratorController.KeepCallbackCountTaskSocket();
            if (!await ClientKeepCallbackTaskController.Callback(enumeratorCommand, clientSessionObject))
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }


            ClientTwoStage‌CallbackController.Stage‌Callback callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackCountSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackCountSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }


            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackTaskSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackTaskSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, true);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackCountTaskSocketInputReturn(clientSessionObject.Value, clientSessionObject.Ref, callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            callback = new ClientTwoStage‌CallbackController.Stage‌Callback(clientSessionObject, false);
            using (CommandKeepCallback commandKeepCallback = await client.ClientCodeGeneratorController.TwoStage‌CallbackCountTaskSocketReturn(callback.Callback, callback.KeepCallback))
            {
                if (!await callback.Wait(commandKeepCallback))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }

            return true;
        }
    }
}
