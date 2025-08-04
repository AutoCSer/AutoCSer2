using AutoCSer.Extensions;

namespace AutoCSer.Document.RemoteExpression
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandServerConfig config = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
                IsRemoteExpression = true
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListener(config
                , AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator<IRemoteExpressionDelegateService>(new RemoteExpressionDelegateService())))
            {
                if (await commandListener.Start())
                {
                    client().AutoCSerTaskExtensions().Catch();

                    Console.ReadKey();
                }
            }
        }
        /// <summary>
        /// Client-side testing
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> client()
        {
            try
            {
                var client = await CommandClientSocketEvent.CommandClient.SocketEvent.Wait();
                if (client == null)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int parameter = AutoCSer.Random.Default.Next();
                var result = await client.RemoteExpressionDelegateController.Func(new AutoCSer.Net.CommandServer.RemoteExpressionFunc<int>(() => parameter + 1));
                if (result.Value != parameter + 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                parameter = AutoCSer.Random.Default.Next();
                result = await client.RemoteExpressionDelegateController.Func1(new AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int>(p => p + 1), parameter);
                if (result.Value != parameter + 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int parameter2 = AutoCSer.Random.Default.Next();
                result = await client.RemoteExpressionDelegateController.Func2(new AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int>((p1, p2) => p1 + p2), parameter, parameter2);
                if (result.Value != parameter + parameter2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                int parameter3 = AutoCSer.Random.Default.Next();
                result = await client.RemoteExpressionDelegateController.Func3(new AutoCSer.Net.CommandServer.RemoteExpressionFunc<int, int, int, int>((p1, p2, p3) => p1 + p2 + p3), parameter, parameter2, parameter3);
                if (result.Value != parameter + parameter2 + parameter3)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                result = await client.RemoteExpressionDelegateController.Action(new AutoCSer.Net.CommandServer.RemoteExpressionAction(() => ActionTarget.Default.Set(parameter + 1)));
                if (ActionTarget.Default.Value != parameter + 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                parameter = AutoCSer.Random.Default.Next();
                result = await client.RemoteExpressionDelegateController.Action1(new AutoCSer.Net.CommandServer.RemoteExpressionAction<int>(p => ActionTarget.Default.Set(p + 1)), parameter);
                if (ActionTarget.Default.Value != parameter + 1)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                result = await client.RemoteExpressionDelegateController.Action2(new AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int>((p1, p2) => ActionTarget.Default.Set(p1 + p2)), parameter, parameter2);
                if (ActionTarget.Default.Value != parameter + parameter2)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }

                result = await client.RemoteExpressionDelegateController.Action3(new AutoCSer.Net.CommandServer.RemoteExpressionAction<int, int, int>((p1, p2, p3) => ActionTarget.Default.Set(p1 + p2 + p3)), parameter, parameter2, parameter3);
                if (ActionTarget.Default.Value != parameter + parameter2 + parameter3)
                {
                    return AutoCSer.Breakpoint.ReturnFalse();
                }
            }
            catch (Exception exception)
            {
                AutoCSer.Breakpoint.ConsoleWriteQueue(exception.ToString());
            }
            Console.WriteLine("Completed");
            return true;
        }
    }
}
