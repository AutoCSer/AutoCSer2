using AutoCSer.Extensions;

namespace AutoCSer.Document.ServiceThreadStrategy
{
    internal class Program
    {
        /// <summary>
        /// https://zhuanlan.zhihu.com/p/10102634904
        /// </summary>
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document)
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(32)
                .Append<Server.Task.ISynchronousController>(new Server.Task.SynchronousController())
                .Append<Server.Task.ICallbackController>(new Server.Task.CallbackController())
                .Append<Server.Task.IKeepCallbackController>(new Server.Task.KeepCallbackController())
                .Append<Server.Task.ISendOnlyController>(new Server.Task.SendOnlyController())
                .Append<Server.TaskQueue.ISynchronousController>(new Server.TaskQueue.SynchronousController())
                .Append<Server.TaskQueue.ICallbackController>(new Server.TaskQueue.CallbackController())
                .Append<Server.TaskQueue.IKeepCallbackController>(new Server.TaskQueue.KeepCallbackController())
                .Append<Server.TaskQueue.ISendOnlyController>(new Server.TaskQueue.SendOnlyController())
                .Append<Server.TaskQueue.ITaskQueueController>(new Server.TaskQueue.TaskQueueController())
                .Append<Server.TaskQueueController.ISynchronousController, int>((task, key) => new Server.TaskQueueController.SynchronousController(task, key))
                //.Append<Server.TaskQueueController.IKeepCallbackController, int>((task, key) => new Server.TaskQueueController.KeepCallbackController(task, key))
                //.Append<Server.TaskQueueController.ISendOnlyController, int>((task, key) => new Server.TaskQueueController.SendOnlyController(task, key))
                .Append<Server.Queue.ISynchronousController>(new Server.Queue.SynchronousController())
                .Append<Server.Queue.ICallbackController>(new Server.Queue.CallbackController())
                .Append<Server.Queue.IKeepCallbackController>(new Server.Queue.KeepCallbackController())
                .Append<Server.Queue.ISendOnlyController>(new Server.Queue.SendOnlyController())
                .Append<Server.ConcurrencyReadQueue.ISynchronousController>(new Server.ConcurrencyReadQueue.SynchronousController())
                .Append<Server.ConcurrencyReadQueue.ICallbackController>(new Server.ConcurrencyReadQueue.CallbackController())
                .Append<Server.ConcurrencyReadQueue.IKeepCallbackController>(new Server.ConcurrencyReadQueue.KeepCallbackController())
                .Append<Server.ConcurrencyReadQueue.ISendOnlyController>(new Server.ConcurrencyReadQueue.SendOnlyController())
                .Append<Server.ReadWriteQueue.ISynchronousController>(new Server.ReadWriteQueue.SynchronousController())
                .Append<Server.ReadWriteQueue.ICallbackController>(new Server.ReadWriteQueue.CallbackController())
                .Append<Server.ReadWriteQueue.IKeepCallbackController>(new Server.ReadWriteQueue.KeepCallbackController())
                .Append<Server.ReadWriteQueue.ISendOnlyController>(new Server.ReadWriteQueue.SendOnlyController())
                .Append<Server.Synchronous.ISynchronousController>(new Server.Synchronous.SynchronousController())
                .Append<Server.Synchronous.ICallbackController>(new Server.Synchronous.CallbackController())
                .Append<Server.Synchronous.IKeepCallbackController>(new Server.Synchronous.KeepCallbackController())
                .Append<Server.Synchronous.ISendOnlyController>(new Server.Synchronous.SendOnlyController())
                .CreateCommandListener(config))
            {
                if (await commandListener.Start())
                {
                    client().Catch();

                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
        /// <summary>
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> client()
        {
            try
            {
                var client = await Client.CommandClientSocketEvent.CommandClient.SocketEvent.Wait();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                await client.ServerTask_SendOnlyCommandController.Call(1);
                await client.ServerTaskQueue_SendOnlyCommandController.Call(-1, 2);
                //await client.ServerTaskQueueController_SendOnlyCommandController.CreateQueueController(-1).Call(3);
                await client.ServerQueue_SendOnlyCommandController.Call(3);
                await client.ServerConcurrencyReadQueue_SendOnlyCommandController.Call(4);
                await client.ServerReadWriteQueue_SendOnlyCommandController.Call(5);
                await client.ServerSynchronous_SendOnlyCommandController.Call(6);
                await client.TaskQueueController.Call(7);

                int left, right;
                await client.ServerTask_CallbackController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerTask_CallbackController)} {callbackValue.Value}");
                });
                await client.ServerTaskQueue_CallbackController.Add(-1, left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerTaskQueue_CallbackController)} {callbackValue.Value}");
                });
                await client.ServerQueue_CallbackController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerQueue_CallbackController)} {callbackValue.Value}");
                });
                await client.ServerConcurrencyReadQueue_CallbackController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerConcurrencyReadQueue_CallbackController)} {callbackValue.Value}");
                });
                await client.ServerReadWriteQueue_CallbackController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerReadWriteQueue_CallbackController)} {callbackValue.Value}");
                });
                await client.ServerSynchronous_CallbackController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next(), callbackValue =>
                {
                    Console.WriteLine($"{nameof(client.ServerSynchronous_CallbackController)} {callbackValue.Value}");
                });

                var result = await client.ServerTask_ReturnCommandController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerTaskQueue_ReturnCommandController.Add(-1, left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerTaskQueueController_ReturnCommandController.CreateQueueController(-1).Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerQueue_ReturnCommandController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerConcurrencyReadQueue_ReturnCommandController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerReadWriteQueue_ReturnCommandController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.ServerSynchronous_ReturnCommandController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                result = await client.TaskQueueController.Add(left = AutoCSer.Random.Default.Next(), right = AutoCSer.Random.Default.Next());
                if (result.Value != left + right)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                var enumeratorCommand = await client.ServerTask_EnumeratorCommandController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.ServerTaskQueue_EnumeratorCommandController.Callback(-1, left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                //enumeratorCommand = await client.ServerTaskQueueController_EnumeratorCommandController.CreateQueueController(-1).Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                //if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                //{
                //    return AutoCSer.Breakpoint.ReturnFalse();
                //}
                enumeratorCommand = await client.ServerQueue_EnumeratorCommandController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.ServerConcurrencyReadQueue_EnumeratorCommandController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.ServerReadWriteQueue_EnumeratorCommandController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.ServerSynchronous_EnumeratorCommandController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                enumeratorCommand = await client.TaskQueueController.Callback(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.TaskQueueController.CallbackCount(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.TaskQueueController.Enumerable(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
                enumeratorCommand = await client.TaskQueueController.AsyncEnumerable(left = AutoCSer.Random.Default.Next(), right = left + AutoCSer.Random.Default.Next(10) + 1);
                if (!await checkEnumeratorCommand(enumeratorCommand, left, right))
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                await Task.Delay(1000);
            }
            catch(Exception exception)
            {
                AutoCSer.Breakpoint.ConsoleWriteQueue(exception.ToString());
            }
            Console.WriteLine("Completed");
            return true;
        }
        /// <summary>
        /// 持续响应测试
        /// </summary>
        /// <param name="command"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static async Task<bool> checkEnumeratorCommand(AutoCSer.Net.EnumeratorCommand<int>? command, int left, int right)
        {
            if (command == null)
            {
                return false;
            }
            while (await command.MoveNext())
            {
                if(command.Current != left)
                {
                    return false;
                }
                ++left;
            }
            if (left != right + 1 || command.ReturnType != AutoCSer.Net.CommandClientReturnTypeEnum.Success)
            {
                return false;
            }
            return true;
        }
    }
}
