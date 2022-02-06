using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 客户端测试接口
    /// </summary>
    public interface IClientKeepCallbackTaskController
    {
        EnumeratorCommand<string> KeepCallbackTaskSocketReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackTaskSocket(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackTaskSocketReturn();
        EnumeratorCommand KeepCallbackTaskSocket();
        EnumeratorCommand<string> KeepCallbackTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackTaskReturn();
        EnumeratorCommand KeepCallbackTask();

        EnumeratorCommand<string> KeepCallbackCountTaskSocketReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackCountTaskSocket(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackCountTaskSocketReturn();
        EnumeratorCommand KeepCallbackCountTaskSocket();
        EnumeratorCommand<string> KeepCallbackCountTaskReturn(int Value, int Ref);
        EnumeratorCommand KeepCallbackCountTask(int Value, int Ref);
        EnumeratorCommand<string> KeepCallbackCountTaskReturn();
        EnumeratorCommand KeepCallbackCountTask();

        EnumeratorQueueCommand<string> KeepCallbackTaskQueueSocketReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueueSocket(int Value, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackTaskQueue(int Value, int Ref);

        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueSocketReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueueSocket(int Value, int Ref);
        EnumeratorQueueCommand<string> KeepCallbackCountTaskQueueReturn(int Value, int Ref);
        EnumeratorQueueCommand KeepCallbackCountTaskQueue(int Value, int Ref);
    }
    /// <summary>
    /// 命令客户端测试
    /// </summary>
    internal static class ClientKeepCallbackTaskController
    {
        private static async Task<bool> callback(EnumeratorCommand<string> enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                string value = enumeratorCommand.Current;
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return Program.Breakpoint();
                }
                if (value != (ServerSynchronousController.SessionObject.Xor() + index).ToString())
                {
                    return Program.Breakpoint();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return Program.Breakpoint();
            }
            return true;
        }
        private static async Task<bool> callback(EnumeratorCommand enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return Program.Breakpoint();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return Program.Breakpoint();
            }
            return true;
        }
        private static async Task<bool> callback(EnumeratorQueueCommand<string> enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                string value = enumeratorCommand.Current;
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return Program.Breakpoint();
                }
                if (value != (ServerSynchronousController.SessionObject.Xor() + index).ToString())
                {
                    return Program.Breakpoint();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
            {
                return Program.Breakpoint();
            }
            return true;
        }
        private static async Task<bool> callback(EnumeratorQueueCommand enumeratorCommand, CommandServerSessionObject clientSessionObject)
        {
            int index = 0;
            while (await enumeratorCommand.MoveNext())
            {
                if (index == 0 && !ServerSynchronousController.SessionObject.Check(clientSessionObject))
                {
                    return Program.Breakpoint();
                }
                ++index;
            }
            if (index != ServerKeepCallbackController.KeepCallbackCount)
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
            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand<string> enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorCommand enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocketReturn();
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskSocket();
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskReturn();
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTask();
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocketReturn();
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskSocket();
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskReturn();
            if (!await callback(enumeratorCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            enumeratorCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTask();
            if (!await callback(enumeratorCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand<string> enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            EnumeratorQueueCommand enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }


            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocketReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueSocket(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommandReturn = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueueReturn(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommandReturn, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            clientSessionObject.Value = AutoCSer.Random.Default.Next();
            clientSessionObject.Ref = AutoCSer.Random.Default.Next();
            enumeratorQueueCommand = await client.ClientKeepCallbackTaskController.KeepCallbackCountTaskQueue(clientSessionObject.Value, clientSessionObject.Ref);
            if (!await callback(enumeratorQueueCommand, clientSessionObject))
            {
                return Program.Breakpoint();
            }

            return true;
        }
    }
}
